using MabiChatSpeech.Properties;
using Microsoft.VisualBasic.Logging;
using Microsoft.Web.WebView2.Core;
using NAudio.Mixer;
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using VoicevoxClientSharp;
using VoicevoxClientSharp.ApiClient;
using VoicevoxClientSharp.ApiClient.Models;
using static MabiChatSpeech.ChatVoice;

using static MabiChatSpeech.MabiChat;
using static MabiChatSpeech.Program;
using static MabiChatSpeech.WinApi;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace MabiChatSpeech
{
    public partial class Main : Form
    {

        static public ChatVoice _chatVoice;
        private static readonly object _chatVoiceLock = new();
        private readonly SemaphoreSlim _voiceInitSemaphore = new(1, 1);
        static _RunningStatusEngine _ChatVoice_Statu = new _RunningStatusEngine();
        static public int chat_cnt = 1; //チャットのナンバリング
        static public DateTime LogHeaderDt = DateTime.Now;

        static public Regex? ChatRegex = null;
        public enum MabiPacketStatus { Off, Other, Chat, Dump }
        //        static public MabiPacketStatus MpacketStatu = MabiPacketStatus.Off;

        private int _statusSubItemIndex;
        private int? _pendingSpeechRowIndex;
        private bool _engineStatusRefreshQueued;
        private int _voiceVoxSpeakerCount;
        private int _aivisSpeechSpeakerCount;

        private readonly Dictionary<Guid, ListViewItem> _speechRowsById = new();
        private readonly HashSet<ListViewItem> _boundSpeechRows = new();
        private void RegisterChatVoiceEvents(
            int statusSubItemIndex)
        {
            if (statusSubItemIndex < 0)
                throw new ArgumentOutOfRangeException();
            _statusSubItemIndex = statusSubItemIndex;

            _chatVoice!.SpeechQueued += ChatVoice_SpeechQueued;
            _chatVoice.SpeechStatusChanged += ChatVoice_SpeechStatusChanged;
        }

        private void ChatVoice_SpeechQueued(ChatVoice.SpeechQueueEventArgs e)
        {
            int? rowIndex = _pendingSpeechRowIndex;
            _pendingSpeechRowIndex = null;
            ListViewItem? row = rowIndex is int index && index >= 0 && index < LSV_chat.Items.Count
                ? LSV_chat.Items[index]
                : null;

            if (row is null)
                return;

            while (row.SubItems.Count <= _statusSubItemIndex)
                row.SubItems.Add("");

            row.SubItems[_statusSubItemIndex].Text = "キュー待ち";
            _boundSpeechRows.Add(row);
            _speechRowsById[e.RequestId] = row;
        }

        private void ChatVoice_SpeechStatusChanged(
            ChatVoice.SpeechStatusChangedEventArgs e)
        {
            if (!_speechRowsById.TryGetValue(e.RequestId, out ListViewItem? row))
                return;

            row.SubItems[_statusSubItemIndex].Text = e.Status switch
            {
                ChatVoice.SpeechStatus.Queued => "キュー待ち",
                ChatVoice.SpeechStatus.Posting => "合成中",
                ChatVoice.SpeechStatus.Synthesized => "再生待ち",
                ChatVoice.SpeechStatus.Playing => "再生中",
                ChatVoice.SpeechStatus.Completed => "完了",
                ChatVoice.SpeechStatus.Failed => "失敗",
                ChatVoice.SpeechStatus.Canceled => "キャンセル",
                _ => e.Status.ToString()
            };
        }
        public enum SpekersCountStatus { NotVoice = -4, Loading = -3, NotRun = -2, EngienNull = -1 }
        static public int SpekaerCount(ChatVoiceEngine eng)
        {
            string module = "";
            RunningStatus sts;
            switch (eng)
            {
                case ChatVoiceEngine.VoiceVox:
                    module = __VoiceVox;
                    sts = _ChatVoice_Statu.VoiceVox;
                    break;
                case ChatVoiceEngine.AivisSpeech:
                    module = __AivisSpeech;
                    sts = _ChatVoice_Statu.AivisSpeech;
                    break;
                default:
                    return ((int)SpekersCountStatus.NotVoice);
            }

            if (sts == RunningStatus.Loading)
            {
                return ((int)SpekersCountStatus.Loading);
            }

            if (sts != RunningStatus.Running)
            {
                return ((int)SpekersCountStatus.NotRun);
            }
            if (_chatVoice == null)
            {
                return ((int)SpekersCountStatus.EngienNull);
            }

            int count = 0;
            foreach (var voices in _chatVoice.Speakers)
            {
                if (voices.Engine == eng)
                {
                    count++;
                }
            }
            return (count);
        }
        private void Tim_Status_Tick(object sender, EventArgs e)
        {
            // Voiceの起動チェック
            // 1.設定がある場合のみ
            _ChatVoice_Statu.Refresh();

            var rsv = _ChatVoice_Statu.VoiceVox;
            switch (rsv)
            {
                case RunningStatus.Loading:
                    SSL_VoiceVox.Text = "Loading";
                    break;
                case RunningStatus.Running:
                    SSL_VoiceVox.Image = Properties.Resources.Icon_vvpp;
                    var cnt = _voiceVoxSpeakerCount;
                    if (cnt <= 0)
                    {
                        SSL_VoiceVox.Text = $"Init";
                    }
                    else
                    {
                        SSL_VoiceVox.Text = $"({cnt})";
                    }
                    break;
                case RunningStatus.NotRunning:
                    SSL_VoiceVox.Image = Properties.Resources.Icon_vvpp_Disable;
                    SSL_VoiceVox.Text = "未起動";
                    break;
                case RunningStatus.NotIstalled:
                    SSL_VoiceVox.Text = "未設定";
                    SSL_VoiceVox.Image = Properties.Resources.Icon_vvpp_Disable;
                    break;
                default:
                    break;

            }
            var rsa = _ChatVoice_Statu.AivisSpeech;
            switch (rsa)
            {
                case RunningStatus.Loading:
                    SSL_AivisSpeech.Text = "Loading";
                    break;
                case RunningStatus.Running:
                    SSL_AivisSpeech.Image = Properties.Resources.Icon_aisp;
                    var cnt = _aivisSpeechSpeakerCount;
                    if (cnt <= 0)
                    {
                        Debug.Print($"SSL_AivisSpeech:{cnt}");
                        SSL_AivisSpeech.Text = $"Init";
                    }
                    else
                    {
                        SSL_AivisSpeech.Text = $"({cnt})";
                    }

                    break;
                case RunningStatus.NotRunning:
                    SSL_AivisSpeech.Image = Properties.Resources.Icon_aisp_Disable;
                    SSL_AivisSpeech.Text = "未起動";
                    break;
                case RunningStatus.NotIstalled:
                    SSL_AivisSpeech.Text = "未設定";
                    SSL_AivisSpeech.Image = Properties.Resources.Icon_aisp_Disable;
                    break;
                default:
                    break;

            }

        }

        private void ChatVoiceEngine_StatusChanged(object? sender, EngineStatusChangedEventArgs e)
        {
            Debug.Print($"音声エンジン状態変更: {e.Engine} {e.PreviousStatus} -> {e.CurrentStatus}");
            // 起動中はAPIがまだ使えないため、合成可能になった時点で初期化する。
            if (e.CurrentStatus == RunningStatus.Loading)
                return;
            if (_engineStatusRefreshQueued || IsDisposed || !IsHandleCreated)
                return;

            _engineStatusRefreshQueued = true;
            BeginInvoke(new Action(() =>
            {
                _engineStatusRefreshQueued = false;
                if (!IsDisposed)
                    init_voice();
            }));
        }

        public void CopyEngineStatusTo(ToolStripStatusLabel voiceVox, ToolStripStatusLabel aivisSpeech)
        {
            voiceVox.Text = SSL_VoiceVox.Text;
            voiceVox.Image = SSL_VoiceVox.Image;
            aivisSpeech.Text = SSL_AivisSpeech.Text;
            aivisSpeech.Image = SSL_AivisSpeech.Image;
        }

        public Main()
        {
            InitializeComponent();

            _ChatVoice_Statu.StatusChanged += ChatVoiceEngine_StatusChanged;

            LoadCharaList(__SavePath + "\\CharacterNameList.txt");
            init_voice();

        }
        static private int init_count = 0;
        public async void init_voice()
        {
            await _voiceInitSemaphore.WaitAsync();
            try
            {
                init_count++;

                // 既存インスタンスを使い続けたまま次のインスタンスを作成する。
                ChatVoice? replacement = await ChatVoice.CreateMultiEngineAsync(__AivisSpeech_Host, __VoiceVox_Host);
                ChatVoice? previous;
                lock (_chatVoiceLock)
                {
                    previous = _chatVoice;
                    _chatVoice = replacement;
                    _voiceVoxSpeakerCount = replacement?.Speakers.Count(speaker => speaker.Engine == ChatVoiceEngine.VoiceVox) ?? 0;
                    _aivisSpeechSpeakerCount = replacement?.Speakers.Count(speaker => speaker.Engine == ChatVoiceEngine.AivisSpeech) ?? 0;
                    if (replacement != null)
                        RegisterChatVoiceEvents(4);
                }

                // 差し替え後に旧インスタンスを破棄する。
                if (previous != null)
                    await previous.DisposeAsync();

                if (replacement == null)
                {
                    //SSL_VoiceAPI.Text = "音声合成不可";
                    SSL_VoiceAPI.Image = Resources.Icon_LED_C_Red;

                    Debug.Print($"init_count:不可:{init_count}");
                }
                else
                {
                    //SSL_VoiceAPI.Text = "音声合成可";
                    SSL_VoiceAPI.Image = Resources.Icon_LED_C_Green;
                    Debug.Print($"init_count:可　:{init_count}");
                }
                init_count = 0;
            }
            finally
            {
                _voiceInitSemaphore.Release();
            }

        }

        public void Voice_chat_say(string words, ChatVoiceEngine eng, int vs, float cv, float cs, float cp)
        {
            if (vs < 0) { return; }
            ChatVoiceSettings p = new ChatVoiceSettings(cs, cv, cp);
            lock (_chatVoiceLock)
            {
                ChatVoice? voice = _chatVoice;
                if (voice == null)
                    return;

                ChatVoiceSpeaker? speaker = voice.Speakers.FirstOrDefault(item =>
                    item.Engine == eng && item.StyleId == vs);
                if (speaker is null)
                {
                    Debug.Print("speake is Null");
                }
                else
                {
                    // Enqueue とインスタンス差し替えを同期し、破棄済みインスタンスへの投入を防ぐ。
                    _pendingSpeechRowIndex = LSV_chat.Items.Count - 1;
                    try
                    {
                        voice.Enqueue(words, eng, vs, p);
                    }
                    finally
                    {
                        _pendingSpeechRowIndex = null;
                    }
                }
            }
        }


        // チャット
        delegate void deg_TxtChat_Text(string text);
        public void TxtChatWriteLine(string sx)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new deg_TxtChat_Text(TxtChatWriteLine), sx);
                }
                else
                {
                    Txt_Chat.AppendText(sx);
                }
            }
            catch
            {
            }
        }

        // LSV_chat
        private void LSV_chat_ItemAdd(CharacterNameData cdata, bool newflag, string no, string ti, string ty, string cn, string wd)
        {
            var citem = LSV_chat.Items.Add(no);
            citem.SubItems.Add(ti);
            citem.SubItems.Add(ty);
            citem.SubItems.Add(cn);
            citem.SubItems.Add("");
            citem.SubItems.Add(wd);

            if (newflag)
            {
                citem.ForeColor = Program.LC_DataAdd;
            }
            else
            {
                citem.ForeColor = Program.LC_DataNone;
            }

            if (cdata.Enabled)
            {
                switch (cdata.VoiceEngen)
                {
                    case ChatVoiceEngine.TTS:
                        citem.ForeColor = Program.LC_DataTTS;
                        break;
                    case ChatVoiceEngine.VoiceVox:
                        citem.ForeColor = Program.LC_DataVVE; ;
                        break;
                    case ChatVoiceEngine.AivisSpeech:
                        citem.ForeColor = Program.LC_DataASE;
                        break;
                }
            }
            else
            {
                citem.ForeColor = Program.LC_DataDisable; ;
            }

            switch (cdata.CharaTypeName)
            {
                case _CharaTypeNames.PC:
                    citem.BackColor = LC_BG_PC;
                    break;
                case _CharaTypeNames.NPC:
                    citem.BackColor = LC_BG_NPC;
                    break;
                case _CharaTypeNames.PET:
                    citem.BackColor = LC_BG_PET;
                    break;
                case _CharaTypeNames.DOLL:
                    citem.BackColor = LC_BG_DOLL;
                    break;
            }


            if (newflag)
            {
                citem.ForeColor = Program.LC_DataAdd;
            }

            if (LSV_chat.SelectedItems.Count == 0)
            {
                LSV_chat.EnsureVisible(LSV_chat.Items.Count - 1);
            }

        }

        delegate void deg_LSV_chat_Add(CharacterNameData cdata, bool newflag, string no, string ti, string ty, string cn, string wd);
        private void LSV_chatWriteLine(CharacterNameData cdata, bool newflag, string no, string ti, string ty, string cn, string wd)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new deg_LSV_chat_Add(LSV_chatWriteLine), cdata, newflag, no, ti, ty, cn, wd);
                }
                else
                {
                    LSV_chat_ItemAdd(cdata, newflag, no, ti, ty, cn, wd);
                }
            }
            catch
            {
            }
        }
        public void LSV_chat_ItemUpdate(int row, string status)
        {
            LSV_chat.Items[row - 1].SubItems[4].Text = status;
        }
        delegate void deg_LSV_chat_Update(int row, string status);
        public void LSV_chat_Update(int row, string status)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new deg_LSV_chat_Update(LSV_chat_ItemUpdate), row, status);
                }
                else
                {
                    LSV_chat_ItemUpdate(row, status);
                }
            }
            catch
            {
            }
        }
        // 読上げ
        delegate void deg_speechChat(string cn, int cv, int cs, string c1, string c2);
        public void speech_chat(string cn, int cv, int cs, string c1, string c2)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new deg_speechChat(speech_chat), cn, cv, cs, c1, c2);
                }
                else
                {
                    SpeechSynthesizer speech_spkp = new SpeechSynthesizer();
                    speech_spkp.SetOutputToDefaultAudioDevice();

                    string[] cnn = cn.Split(']');
                    speech_spkp.SelectVoice(cnn[1]);

                    string sayword = "";
                    if (Program.__TTS_NameCall == true)
                    {
                        sayword = c1 + "  ";
                    }
                    sayword += c2;
                    speech_spkp.Volume = cv;
                    speech_spkp.Rate = cs;
                    speech_spkp.SpeakAsync(sayword);

                }
            }
            catch
            {
            }
        }
        // 読上げ
        delegate void deg_voiceChat(ChatVoiceEngine eng, int cn, float cs, float cv, float cp, string c1, string c2);
        private void voice_chat(ChatVoiceEngine eng, int cn, float cs, float cv, float cp, string c1, string c2)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new deg_voiceChat(voice_chat), eng, cn, cs, cv, cp, c1, c2);
                }
                else
                {

                    string sayword = "";
                    if (Program.__TTS_NameCall == true)
                    {
                        sayword = c1 + "  ";
                    }
                    sayword += c2;

                    Voice_chat_say(sayword, eng, cn, cs, cv, cp);

                }
            }
            catch
            {
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            SSL_PacketStatus.Image = Resources.Icon_LED_Off;
            timer1.Stop();
        }

        delegate void deg_PacketStatus(MabiPacketStatus status);
        private void PacketStatus(MabiPacketStatus status)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new deg_PacketStatus(PacketStatus), status);
            }
            else
            {
                switch (status)
                {
                    case MabiPacketStatus.Off:
                        SSL_PacketStatus.Image = Resources.Icon_LED_Off;
                        break;
                    case MabiPacketStatus.Other:
                        timer1.Stop();
                        SSL_PacketStatus.Image = Resources.Icon_LED_Blue;
                        timer1.Start();
                        break;
                    case MabiPacketStatus.Chat:
                        timer1.Stop();
                        SSL_PacketStatus.Image = Resources.Icon_LED_Orange;
                        timer1.Start();
                        break;
                    case MabiPacketStatus.Dump:
                        timer1.Stop();
                        SSL_PacketStatus.Image = Resources.Icon_LED_Green;
                        timer1.Start();
                        break;
                }
            }
        }

        private st_MabiServerList ConnectSvInfo(string ip)
        {
            st_MabiServerList ret = new st_MabiServerList();
            foreach (var item in MabiPacket.MabiServerList)
            {
                if (item.ip == ip)
                {
                    ret.ip = ip;
                    ret.svno = item.svno;
                    ret.chno = item.chno;
                    return (ret);
                }
            }
            ret.ip = "";
            ret.svno = 0;
            ret.chno = 0;
            return (ret);

        }

        // MabiPacket関連のイベント
        // Status Change
        /// <summary>
        /// 接続状態イベント受信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onConnect(object sender, EventArgs e)
        {
            PacketStatus(MabiPacketStatus.Other);
            var x = (MabiPacket)sender;
            var ex = (MabiPacketEventArgs)e;
            SLB_Clinet_Set(ex.csts);
            var sv = ConnectSvInfo(ex.svip);

            SLB_Ip_Set(sv.svno, sv.chno);

        }
        // ダンプメッセージ
        /// <summary>
        /// ダンプイベント受信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onDump(object sender, EventArgs e)
        {
            PacketStatus(MabiPacketStatus.Dump);
            var x = (MabiPacket)sender;
            var c = (MabiPacketEventArgs)e;
            TxtChatWriteLine(c.PacketDump);
            var li = c.PacketDump.Split(Environment.NewLine);

        }
        public Frm_browser ChatPop = new Frm_browser();

        // On Chat
        /// <summary>
        /// チャットイベント受信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onChat(object sender, EventArgs e)
        {
            try
            {
                PacketStatus(MabiPacketStatus.Chat);
                var x = (MabiPacket)sender;
                var c = (MabiPacketEventArgs)e;
                var t = DateTime.Now;
                var add_flag = false;

                switch (c.CharaTypeName)
                {
                    case _CharaTypeNames.PC:
                        break;
                    case _CharaTypeNames.NPC:
                        if (Program.__ChatNPC_Mute) { return; }
                        break;
                    case _CharaTypeNames.PET:
                        if (Program.__ChatPet_Mute) { return; }
                        break;
                    case _CharaTypeNames.DOLL:
                        if (Program.__ChatDoll_Mute) { return; }
                        break;
                    default:
                        return;
                }

                CharacterNameData? character = CharaList_find(c.CharacterName, c.CharaTypeName);

                //CharacterNameData? character = Program.CharaList.FirstOrDefault(x => x.CharName == c.CharacterName);
                // PCリスト自動追加
                if (Program.__CharaList_AutoAdd == true) //&& (c.CharaTypeName == _CharaTypeNames.PC))
                {
                    if (character == null)
                    {
                        CharacterNameData character_add = new CharacterNameData();
                        character_add.Enabled = false;
                        character_add.CharName = c.CharacterName;
                        character_add.CharaTypeName = c.CharaTypeName;
                        character_add.VoiceEngen = ChatVoiceEngine.TTS;//エンジンをどうするか？
                        character_add.VoiceName = __TTS1Name;
                        character_add.VoiceId = -1;
                        character_add.VolumeScale = 1.0f;
                        character_add.SpeedScale = 1.3f;
                        character_add.PitchScale = 0.0f;
                        Program.CharaList.Add(character_add);
                        character = character_add;
                        add_flag = true;
                    }
                }

                if (character.Visible == false)
                {
                    return;
                }

                var mode = (ChatModes)__ChatMode;

                if (ChatRegex != null) //正規表現の適用
                {
                    var s = c.ChatWord;
                    Match match = ChatRegex.Match(s);
                    c.ChatWord = match.Success ? match.Value : s;
                }

                /* 1.Chat内容を表示*/
                // chat log show
                var cc = c.CharaTypeName.ToString();
                string[] li = { "" };
                LSV_chatWriteLine(character, add_flag, $"{chat_cnt}", $"{t:HH:mm:ss.fff}", $"{cc}", $"{c.CharacterName}", $"{c.ChatWord}");
                // chat log write
                li[0] = $"C {chat_cnt},{t:HH:mm:ss.fff},{cc},{c.CharacterName},{c.ChatWord}";
                TxtChatWriteLine(li[0] + Environment.NewLine);
                chat_cnt++;

                // ChatPopupへ出力
                if (ChatPop.Visible)
                {
                    ChatPop.ChatPopupSend(c.ChatWord);
                }

                /* 2.TTSモードなら未登録でも発声 */
                if (mode == ChatModes.TTS)
                {
                    speech_chat(Program.__TTS1Name, Program.__TTS1Volume, Program.__TTS1Speed,
                                c.CharacterName, c.ChatWord);
                    LSV_chat_Update(chat_cnt - 1, "TTS");
                }
                /* 2.Voiceモードなら登録済みかつ有効ならで発声 */
                else if ((mode == ChatModes.Voice) && (character != null))
                {
                    if (character.Enabled)
                    {
                        if (character.VoiceEngen == ChatVoiceEngine.TTS)
                        {
                            speech_chat(Program.__TTS1Name, Program.__TTS1Volume, Program.__TTS1Speed,
                                        c.CharacterName, c.ChatWord);
                            LSV_chat_Update(chat_cnt - 1, "TTS");
                        }
                        else
                        {
                            voice_chat(character.VoiceEngen, character.VoiceId, character.VolumeScale,
                                        character.SpeedScale, character.PitchScale,
                                        c.CharacterName, c.ChatWord);
                        }
                    }
                }
            }
            catch
            {
                DateTime dt = DateTime.Now;
                string logheader = $"MCS Logiing Start {LogHeaderDt:yyyy/MM/dd HH:mm:ss}" + Environment.NewLine;
                string logfooter = $"MCS Logiing End {dt:yyyy/MM/dd HH:mm:ss}" + Environment.NewLine;
                string savefilename = __SavePath + "\\Error_MabiChatLog.txt";
                Txt_Chat.Text = logheader + Txt_Chat.Text + logfooter;

                File.WriteAllText(savefilename, Txt_Chat.Text);
            }
        }

        /// <summary>
        /// メインLoad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {

            try
            {
                Txt_Chat.ForeColor = Program.Color16List[Program.__ChatFColor];
                Txt_Chat.BackColor = Program.Color16List[Program.__ChatBColor];
            }
            catch
            {
            }
            //フォントをロード
            try
            {
                FontStyle ffs = (FontStyle)Program.__ChatFontStyle;
                Font ff = new Font(Program.__ChatFontName, Program.__ChatFontSize, ffs);
                Txt_Chat.Font = ff;
            }
            catch
            {
            }
            //
            if (Program.__SavePath == "")
            {
                // Program.__SavePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                Program.__SavePath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\821Soft";
            }
            if (!(File.Exists(__SavePath)))
            {
                Directory.CreateDirectory(__SavePath);
            }
            if (!(File.Exists(__SavePath + "\\echa")))
            {
                Directory.CreateDirectory(__SavePath + "\\echa");
            }
            /* アイコン設定*/
            switch ((ChatMode)Program.__ChatMode)
            {
                case ChatMode.Chat:
                    SSL_CharaList.Tag = 1;
                    SSL_CharaList.Image = Resources.Icon_CharaList_Chat;
                    break;
                case ChatMode.TTS:
                    SSL_CharaList.Tag = 2;
                    SSL_CharaList.Image = Resources.Icon_CharaList_TTS;
                    break;
                case ChatMode.Voice:
                    SSL_CharaList.Tag = 3;
                    SSL_CharaList.Image = Resources.Icon_CharaList_VOICE;
                    break;
                default:
                    SSL_CharaList.Tag = 0;
                    break;
                    ;
            }
            Mnu_Echa.Checked = Program.__Echa;

            if (Mnu_Echa.Checked)
            {
                Mnu_Echa.Image = Properties.Resources.Icon_EchaOn;
            }
            else
            {
                Mnu_Echa.Image = Properties.Resources.Icon_EchaOff;
            }

            SSL_Echa.Image = Mnu_Echa.Image;
            Mnu_ViewList_No.Checked = Program.__ChatView_No;
            Mnu_ViewList_Time.Checked = Program.__ChatView_Time;
            Mnu_ViewList_Type.Checked = Program.__ChatView_Type;
            Mnu_ViewList_Name.Checked = Program.__ChatView_Name;

        }
        /// <summary>
        /// ビュークリア
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            Txt_Chat.Text = "";
            LSV_chat.Items.Clear();
            LogHeaderDt = DateTime.Now;
            chat_cnt = 1;

        }

        /// <summary>
        /// 接続状態表示
        /// </summary>
        /// <param name="sts"></param>
        delegate void deg_SLB_Clinet_Set(ClinetStatus sts);
        public void SLB_Clinet_Set(ClinetStatus sts)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new deg_SLB_Clinet_Set(SLB_Clinet_Set), sts);
                }
                else
                {
                    switch (sts)
                    {
                        case ClinetStatus.OFF:
                            SLB_Client.Image = Resources.Icon_LED_C_Off;
                            SLB_Client.Text = "OffLine";
                            break;
                        case ClinetStatus.ON:
                            SLB_Client.Image = Resources.Icon_LED_C_Blue;
                            SLB_Client.Text = "Connect";
                            break;
                        case ClinetStatus.CHARASEL:
                            SLB_Client.Image = Resources.Icon_LED_C_Yellow;
                            SLB_Client.Text = "LogIn";
                            break;
                        case ClinetStatus.ONLINE:
                            SLB_Client.Image = Resources.Icon_LED_C_Green;
                            SLB_Client.Text = "OnLine";
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        delegate void deg_SLB_Ip_Set(int svno, int chno);
        public void SLB_Ip_Set(int svno, int chno)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new deg_SLB_Ip_Set(SLB_Ip_Set), svno, chno);
                }
                else
                {
                    switch (svno)
                    {
                        case 1:
                            SLB_Ip.Image = Resources.icn_mari;
                            break;
                        case 2:
                            SLB_Ip.Image = Resources.icn_ruairi;
                            break;
                        case 3:
                            SLB_Ip.Image = Resources.icn_tarlach;
                            break;
                        default:
                            SLB_Ip.Text = "";
                            break;
                    }

                    if (chno == 0)
                    {
                        SLB_Ip.Text = "";
                    }
                    else if (chno == 7)
                    {
                        SLB_Ip.Text = "商店街";
                    }
                    else
                    {
                        SLB_Ip.Text = $"{chno} ch";
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// メイン表示イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Shown(object sender, EventArgs e)
        {

            // Bug.001
            int h = System.Windows.Forms.Screen.GetWorkingArea(this).Height;
            int w = System.Windows.Forms.Screen.GetWorkingArea(this).Width;
            if ((0 > this.Location.X + this.Size.Width - 8) || ((w - 8) <= this.Location.X))
            {
                this.Location = new Point(1, this.Location.Y);
            }

            if ((0 > this.Location.Y + this.Size.Height - 8) || ((h - 8) <= this.Location.Y))
            {
                this.Location = new Point(this.Location.X, 1);
            }
            // Bug.001


            settingLoad();
            Program.packets.ConnectEvent += onConnect;
            Program.packets.ChatEvent += onChat;
            Program.packets.PacketEvent += onDump;
            SLB_Clinet_Set(Program.packets.csts);
            var sv = MabiPacket.IsSvip(Program.packets.svip);
            SLB_Ip_Set(sv.svno, sv.chno);

            SpeechSynthesizer speech_spkp = new SpeechSynthesizer();
            speech_spkp.SetOutputToDefaultAudioDevice();
            if (Program.packets.csts == ClinetStatus.OFF)
            {
                speech_spkp.SpeakAsync("クライアントはOFFです");
            }
            else
            {
                speech_spkp.SpeakAsync("クライアントはONです");
            }

            LogHeaderDt = DateTime.Now;


        }


        // Setting 
        /// <summary>
        /// 設定状態の保存
        /// </summary>
        public void settingLoad()
        {
            Tim_Status.Interval = Program.__API_WDT;

            SSL_NPC.Tag = (bool)Program.__ChatNPC_Mute;
            if ((bool)SSL_NPC.Tag)
            {
                SSL_NPC.Image = Resources.Icon_NPC_Mute_On;
            }
            else
            {
                SSL_NPC.Image = Resources.Icon_NPC_Mute_Off;
            }


            SSL_CharaList.Tag = Program.__ChatMode;
            switch (Program.__ChatMode)
            {
                case 0:
                    SSL_CharaList.Image = null;
                    break;
                case 1:
                    SSL_CharaList.Image = Resources.Icon_CharaList_Chat;
                    break;
                case 2:
                    SSL_CharaList.Image = Resources.Icon_CharaList_TTS;
                    break;
                case 3:
                    SSL_CharaList.Image = Resources.Icon_CharaList_VOICE;
                    break;
                default:
                    SSL_CharaList.Image = null;
                    break;
            }

            SLB_Mode_Icon(Program.packets.PacketMode);

            switch (Program.__SaveMode)
            {
                case 0: SLB_SaveMode.Image = Resources.Icon_File_NoSave; break;
                case 1: SLB_SaveMode.Image = Resources.Icon_File_OverWrite; break;
                case 2: SLB_SaveMode.Image = Resources.Icon_File_Append; break;
                case 3: SLB_SaveMode.Image = Resources.Icon_File_TimeStamp; break;
                default: SLB_SaveMode.Image = null; break;
            }

            ChatRegex = null;
            if (!string.IsNullOrWhiteSpace(__Chat_Regex))
            {
                try
                {
                    ChatRegex = new Regex(__Chat_Regex);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show($"正規表現が正しくありません。\n{ex.Message}", "正規表現");
                    return;
                }
            }
        }

        /// <summary>
        /// ログのファイル出力
        /// </summary>
        /// <param name="Log"></param>
        private void ChatLogSave(System.Windows.Forms.TextBox Log)
        {
            DateTime dt = DateTime.Now;

            string str = dt.ToString("yyyy/MM/dd HH:mm:ss");
            string logheader = $"MCS Logiing Start {LogHeaderDt:yyyy/MM/dd HH:mm:ss}" + Environment.NewLine;
            string logfooter = $"MCS Logiing End {dt:yyyy/MM/dd HH:mm:ss}" + Environment.NewLine;
            string savefilename = __SavePath + "\\MabiChatLog";
            //                    

            if (__SaveMode == 0)
            {
                return;
            }


            if (Log.Text.Length == 0)
            {
                return;
            }
            Log.Text = logheader + Log.Text + logfooter;
            switch (__SaveMode)
            {
                case 0: //しない
                    break;
                case 1: // 上書き
                    savefilename += ".txt";
                    File.WriteAllText(savefilename, Log.Text);
                    break;
                case 2: // 追記
                    savefilename += ".txt";
                    File.AppendAllText(savefilename, Log.Text);
                    break;
                case 3: // タイムスタンプ
                    savefilename += $"_{dt:yyyyMMdd}_{dt:HHmmss}.txt";
                    File.WriteAllText(savefilename, Log.Text);
                    break;
                default:
                    break;
            }
            // TPB_Save.Visible = false;

        }
        public static void LoadCharaList(string filePath)
        {
            CharaList.Clear();

            try
            {
                foreach (string line in File.ReadLines(filePath, Encoding.UTF8))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        CharaList.Add(new CharacterNameData(line));
                    }
                }

            }
            catch
            {
            }
        }
        public static void SaveCharaList(string filePath)
        {
            using var writer = new StreamWriter(filePath, false, new UTF8Encoding(true));

            foreach (CharacterNameData character in CharaList)
            {
                writer.WriteLine(character.ToText());
            }
        }

        /// <summary>
        /// メインクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveCharaList(__SavePath + "\\CharacterNameList.txt");
            Properties.Settings.Default.Save();
            ChatLogSave(Txt_Chat);
            if (File.Exists(_tmpfname))
            {
                File.Delete(_tmpfname);
            }
        }
        /// <summary>
        /// 設定ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Setup_Click(object sender, EventArgs e)
        {
            Setting Frm_Setting = new Setting
            {
                Owner = this
            };
            Frm_Setting.ShowDialog();
        }

        // Redirect Window Hndle List
        private List<IntPtr> twlist = new List<IntPtr>();
        /// <summary>
        /// タスクリスト取得コールバック
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        /// <summary>
        /// ログモード表示設定
        /// </summary>
        /// <param name="pm"></param>
        private void SLB_Mode_Icon(PacketModes pm)
        {
            switch (pm)
            {
                case PacketModes.Chat:
                    SLB_Mode.Image = Resources.Icon_Log_ChatMode;
                    break;
                case PacketModes.Dump:
                    SLB_Mode.Image = Resources.Icon_Log_DumpMode;
                    break;
                default:
                    SLB_Mode.Image = null;
                    break;
            }
        }
        /// <summary>
        /// ログモード切替
        /// </summary>
        /// <param name="mode"></param>
        private void TextViewMode(PacketModes mode)
        {
            string[] msg = { "" };
            switch (mode)
            {
                case PacketModes.Chat:
                    //                    msg[0] = "MCS Logiing Start" + Environment.NewLine;
                    //                    TxtChatWriteLine(msg[0]);
                    Program.packets.PacketMode = PacketModes.Chat;
                    SLB_Mode_Icon(Program.packets.PacketMode);
                    break;
                case PacketModes.Dump:
                    //                    msg[0] = "Dump Mode *** Start" + Environment.NewLine;
                    //                    TxtChatWriteLine(msg[0]);
                    Program.packets.PacketMode = PacketModes.Dump;
                    SLB_Mode_Icon(Program.packets.PacketMode);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// メインフォームクローズ処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Tim_Status.Stop();
            string _Pos_Main = $"{this.Location.X},{this.Location.Y},{this.Width},{this.Height}";
            Program.__Pos_Main = _Pos_Main;
            packets.capdev_stop();
        }

        public Help sf = null;
        /// <summary>
        /// メインキー入力処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                /*
                 *  Q  W  E  R
                 *   A  S  D  F
                 *    Z  X  C  V  B  N 
                 */
                /// ファイル 
                case Keys.X: //Export
                    ExportSRT frm_ExportSrt = new ExportSRT();
                    frm_ExportSrt.ShowDialog();
                    break;
                case Keys.Q: //Quit
                    this.Close();
                    break;
                /// 編集
                case Keys.P:
                    Mnu_Echa_switch();
                    break;
                case Keys.C: //Clear
                    Btn_Clear_Click(sender, (EventArgs)null);
                    break;
                case Keys.S: //Setting
                    Btn_Setup_Click(sender, (EventArgs)null);
                    break;

                /// 表示 
                case Keys.T: //TextView
                    Mnu_ViewText_Click(sender, (EventArgs)null);
                    break;
                case Keys.D: //DumpView
                    Mnu_ViewDump_Click(sender, (EventArgs)null);
                    break;
                case Keys.L: //ListView
                    Mnu_ViewList_Click(sender, (EventArgs)null);
                    break;
                case Keys.Y: //YoutubeLive
                    Mnu_YoutubeLive_Click(sender, (EventArgs)null);
                    break;
                case Keys.F1: //Help
                    Mnu_Help_Click(sender, (EventArgs)null);
                    break;
                default:
                    break;
            }
        }

        private void SSL_CharaList_Setdata()
        {
            switch ((ChatMode)Program.__ChatMode)
            {
                case ChatMode.Chat:
                    SSL_CharaList.Image = Resources.Icon_CharaList_Chat;
                    break;
                case ChatMode.TTS:
                    SSL_CharaList.Image = Resources.Icon_CharaList_TTS;
                    break;
                case ChatMode.Voice:
                    SSL_CharaList.Image = Resources.Icon_CharaList_VOICE;
                    break;
            }
        }

        private void BTN_No_Click(object sender, EventArgs e)
        {
            if (Program.__ChatView_No == true)
            {
                LSV_chat.Columns[0].Width = 0;
                Program.__ChatView_No = false;
                Mnu_ViewList_No.Checked = false;
            }
            else
            {
                LSV_chat.Columns[0].Width = 50;
                Program.__ChatView_No = true;
                Mnu_ViewList_No.Checked = true;
            }
        }

        private void BTN_Time_Click(object sender, EventArgs e)
        {

            if (Program.__ChatView_Time == true)
            {
                LSV_chat.Columns[1].Width = 0;
                Program.__ChatView_Time = false;
                Mnu_ViewList_Time.Checked = false;
            }
            else
            {
                LSV_chat.Columns[1].Width = 100;
                Program.__ChatView_Time = true;
                Mnu_ViewList_Time.Checked = true;
            }
        }

        private void BTN_Type_Click(object sender, EventArgs e)
        {

            if (Program.__ChatView_Type == true)
            {
                LSV_chat.Columns[2].Width = 0;
                Program.__ChatView_Type = false;
                Mnu_ViewList_Type.Checked = false;

            }
            else
            {
                LSV_chat.Columns[2].Width = 50;
                Program.__ChatView_Type = true;
                Mnu_ViewList_Type.Checked = true;
            }
        }

        private void BTN_Name_Click(object sender, EventArgs e)
        {
            if (Program.__ChatView_Name == true)
            {
                LSV_chat.Columns[3].Width = 0;
                Program.__ChatView_Name = false;
                Mnu_ViewList_Name.Checked = false;
            }
            else
            {
                LSV_chat.Columns[3].Width = 140;
                Program.__ChatView_Name = true;
                Mnu_ViewList_Name.Checked = true;
            }
        }

        private void Mnu_YoutubeLive_Click(object sender, EventArgs e)
        {
            if (ChatPop.Visible == false)
            {
                ChatPop.Show();
            }

        }
        private void Mnu_Echa_switch()
        {
            if (Mnu_Echa.Checked)
            {
                Mnu_Echa.Checked = false;
            }
            else
            {
                Mnu_Echa.Checked = true;
            }
        }

        private void Mnu_Echa_CheckedChanged(object sender, EventArgs e)
        {
            if (Mnu_Echa.Checked)
            {
                Mnu_Echa.Image = Properties.Resources.Icon_EchaOn;
            }
            else
            {
                Mnu_Echa.Image = Properties.Resources.Icon_EchaOff;
            }
            SSL_Echa.Image = Mnu_Echa.Image;
            Program.__Echa = Mnu_Echa.Checked;
        }

        private void Mnu_ViewText_Click(object sender, EventArgs e)
        {

            LSV_chat.Visible = false;
            TextViewMode(PacketModes.Chat);

        }

        private void Mnu_ViewDump_Click(object sender, EventArgs e)
        {
            LSV_chat.Visible = false;
            TextViewMode(PacketModes.Dump);
        }

        private void Mnu_ViewList_Click(object sender, EventArgs e)
        {

            LSV_chat.Visible = true;
            // 強制ChatMode
            TextViewMode(PacketModes.Chat);　// Bug.105_01

        }
        public void SSL_Youtube_State(int sts, string txt)
        {
            if (sts == 0)
            {
                SSL_Youtube.Image = null;
                SSL_Youtube.Text = "";
            }
            else
            {
                SSL_Youtube.Image = Properties.Resources.Icon_Youtube_js;
                SSL_Youtube.Text = txt;
            }

        }

        private void Mnu_Quit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Mnu_Export_Click(object sender, EventArgs e)
        {
            ExportSRT frm_ExportSrt = new ExportSRT();
            frm_ExportSrt.ShowDialog();

        }

        private void Mnu_Help_Click(object sender, EventArgs e)
        {
            if (this.sf == null || this.sf.IsDisposed)
            { /* ヌル、または破棄されていたら */
                this.sf = new Help();
                this.sf.Show();
            }
            this.sf.Activate();

        }

        private void Mnu_Replay_Click(object sender, EventArgs e)
        {

            Replay_log targetForm = Application.OpenForms.OfType<Replay_log>().FirstOrDefault();

            if (targetForm != null)
            {
                // 2. 最小化されている場合は、元のサイズに戻す
                if (targetForm.WindowState == FormWindowState.Minimized)
                {
                    targetForm.WindowState = FormWindowState.Normal;
                }

                // 3. フォームをアクティブにする
                targetForm.Activate();
                return;
            }


            if (_chatVoice == null)
            {
                MessageBox.Show(
                    $"リプレイ機能は、音声合成エンジンの起動が必要です.",
                    "確認",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
            }

            OpenFileDialog ofd = new OpenFileDialog();
            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            ofd.FileName = "MabiChatLog.txt";
            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ofd.InitialDirectory = Program.__SavePath;
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = "TXTファイル(MabiChatLog*.txt)|MabiChatLog*.txt|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]ではじめに選択されるものを指定する
            //2番目の「すべてのファイル」が選択されているようにする
            ofd.FilterIndex = 1;
            //タイトルを設定する
            ofd.Title = "開くファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Replay_log frm_Replay_log = new Replay_log(ofd.FileName);
                frm_Replay_log.Show();
            }
        }

        private void LSV_chat_DoubleClick(object sender, EventArgs e)
        {
            if (LSV_chat.SelectedItems.Count > 0)
            {
                LSV_chat.SelectedItems.Clear();

                if (LSV_chat.Items.Count > 0)
                {
                    // 最後のインデックスを取得して表示範囲までスクロール
                    int lastIndex = LSV_chat.Items.Count - 1;
                    LSV_chat.Items[lastIndex].EnsureVisible();
                }

            }
        }

        private void LSV_chat_Click(object sender, EventArgs e)
        {
            if (_chatVoice == null)
            {
                MessageBox.Show(
                    $"キャラクター別の音声を割り当てる場合は\n音声合成エンジンの起動が必要です.",
                    "確認",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
                return;
            }

            if (LSV_chat.SelectedItems.Count > 0)
            {
                var rec = LSV_chat.SelectedItems[0];
                _CharaTypeNames ct = (_CharaTypeNames)Enum.Parse(typeof(_CharaTypeNames), rec.SubItems[2].Text);
                var cn = rec.SubItems[3].Text;
                var cw = rec.SubItems[5].Text;

                CharacterNameData cdata = Program.CharaList_find(cn, ct);
                if (cdata == null)
                {

                    var d_ret = MessageBox.Show(
                        $"キャラクター別のボイス設定を追加しますか？\n{ct}-{cn}",
                        "キャラ別のボイス設定",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                        );
                    if (d_ret == DialogResult.No)
                    {
                        return;
                    }
                    CharacterNameData ddata = new CharacterNameData();
                    ddata.Enabled = true;
                    ddata.CharName = cn;
                    ddata.CharaTypeName = ct;
                    ddata.VoiceEngen = ChatVoiceEngine.TTS;
                    ddata.VoiceName = "";
                    ddata.VolumeScale = 1.0f;
                    ddata.SpeedScale = 1.3f;
                    ddata.PitchScale = 0.0f;
                    ddata.VoiceId = -1;
                    cdata = ddata;
                }
                CharaListVoice_Edit fm = new CharaListVoice_Edit(cdata);
                fm.Owner = this;
                fm.testword = cw;
                fm.ShowDialog();
                LSV_chat_DoubleClick(sender, e);
            }
        }

        private void Mnu_CharList_Edit_Click(object sender, EventArgs e)
        {
            WhiteList Frm_WhiteList = new WhiteList();
            Frm_WhiteList.Owner = this;
            if (Frm_WhiteList.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void Mnu_SystemSetiing_Click(object sender, EventArgs e)
        {
            Setting Frm_Setting = new Setting
            {
                Owner = this
            };
            Frm_Setting.ShowDialog();

        }

        public enum ChatModes { Mute, Chat, TTS, Voice }

        private ChatModes IsChatMode(CharacterNameData charadata, _CharaTypeNames receiveType)
        {
            if (charadata == null)
            {
                return (ChatModes.Mute);
            }

            return ((ChatModes)__ChatMode);
        }

        private void SSL_CharaList_Click(object sender, EventArgs e)
        {
        }

        private void SSL_AivisSpeech_Click(object sender, EventArgs e)
        {
            StartEngien(ChatVoiceEngine.AivisSpeech);
        }

        private void SSL_VoiceVox_Click(object sender, EventArgs e)
        {
            StartEngien(ChatVoiceEngine.VoiceVox);
        }


        private void SSL_NPC_Click(object sender, EventArgs e)
        {
            if ((bool)SSL_NPC.Tag)
            {
                SSL_NPC.Tag = (bool)false;
                Program.__ChatNPC_Mute = false;
                SSL_NPC.Image = Resources.Icon_NPC_Mute_Off;
            }
            else
            {
                SSL_NPC.Tag = (bool)true;
                Program.__ChatNPC_Mute = true;
                SSL_NPC.Image = Resources.Icon_NPC_Mute_On;
            }
        }

        private void Mnu_Chat_Click(object sender, EventArgs e)
        {
            SSL_CharaList.Image = Resources.Icon_CharaList_Chat;
            SSL_CharaList.Tag = 1;
            __ChatMode = (int)ChatModes.Chat;
        }

        private void Mnu_TTS_Click(object sender, EventArgs e)
        {
            SSL_CharaList.Image = Resources.Icon_CharaList_TTS;
            SSL_CharaList.Tag = 2;
            __ChatMode = (int)ChatModes.TTS;
        }

        private void Mnu_Voice_Click(object sender, EventArgs e)
        {

            SSL_CharaList.Image = Resources.Icon_CharaList_VOICE;
            SSL_CharaList.Tag = 3;
            __ChatMode = (int)ChatModes.Voice;
        }
    }
}
