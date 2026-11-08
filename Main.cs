using MabiChatSpeech.Properties;
using Microsoft.Web.WebView2.Core;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace MabiChatSpeech
{
    public partial class Main : Form
    {

        static public ChatVoice _chatVoice;

        static public int chat_cnt = 1; //チャットのナンバリング
        int VoiceDefid;


        private int _statusSubItemIndex;
        private int? _pendingSpeechRowIndex;

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
        public Main()
        {
            InitializeComponent();

            _RunningStatusEngine rse = new _RunningStatusEngine();

            LoadCharaList(__SavePath + "\\CharacterNameList.txt");
            if (CharaList_usedEngien(ChatVoiceEngine.VoiceVox))
            {
                if (rse.VoiceVox != RunningStatus.Running)
                {
                    // 
                    MessageBox.Show($"CharacterNameList.txtに設定している\n音声合成エンジン(VoiceVox)が起動していません\n {__SavePath}\n{rse.VoiceVox}",
                        "音声合成エンジンエラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    Environment.Exit(1);

                }
            }
            if (CharaList_usedEngien(ChatVoiceEngine.AivisSpeech))
            {
                if (rse.AivisSpeech != RunningStatus.Running)
                {
                    MessageBox.Show($"CharacterNameList.txtに設定している\n音声合成エンジン(AivisSpeech)が起動していません\n {__SavePath}\n{rse.AivisSpeech}",
                        "音声合成エンジンエラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    Environment.Exit(2);
                }
            }
            init_voice();

        }
        public async void IsEngenRunning()
        {
            if (__VoiceVox.Trim() != "")
            {
                if (await IsChatVoiceEngenRunning(ChatVoiceEngine.VoiceVox))
                {
                }
                else
                {
                    throw new InvalidOperationException("VOICEVOXが起動していません");
                    this.Close();
                }
            }
            if (__AivisSpeech.Trim() != "")
            {
                if (await IsChatVoiceEngenRunning(ChatVoiceEngine.AivisSpeech))
                {
                }
                else
                {
                    throw new InvalidOperationException("AivisSpeechが起動していません");
                    this.Close();
                }
            }
        }
        public async void init_voice()

        {
            if (_chatVoice != null)
            {
                SSL_VoiceAPI.Text = "リロード";
                await _chatVoice.DisposeAsync();

            }

            _chatVoice = await ChatVoice.CreateMultiEngineAsync();
            if (_chatVoice == null)
            {
                SSL_VoiceAPI.Text = "音声合成不可";
                __ChatSelWhitelist = 0;
                return;
            }
            else
            {
                SSL_VoiceAPI.Text = "音声合成可";
            }
            RegisterChatVoiceEvents(4);

            await _chatVoice.RefreshSpeakersAsync();



        }

        public void Voice_chat_say(string words, ChatVoiceEngine eng, int vs, float cv, float cs, float cp)
        {
            if (vs < 0) { return; }
            ChatVoiceSettings p = new ChatVoiceSettings(cs, cv, cp);
            if (_chatVoice == null)
            {
                return;
            }
            ChatVoiceSpeaker? speaker = _chatVoice.Speakers.FirstOrDefault(item =>
                item.Engine == eng && item.StyleId == vs);
            if (speaker is null)
            {
                Debug.Print("speake is Null");
            }
            else
            {
                // Enqueue は SpeechQueued を同期的に発火するため、発話元の行番号を先に設定する。
                _pendingSpeechRowIndex = LSV_chat.Items.Count - 1;
                try
                {
                    _chatVoice.Enqueue(words, eng, vs, p);
                }
                finally
                {
                    _pendingSpeechRowIndex = null;
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
                    //TPB_Max.Text = $"{Txt_Chat.Lines.Length}";
                    //TPB_Save.Value = Txt_Chat.Lines.Length;
                }
            }
            catch
            {
            }
        }

        // LSV_chat
        public void LSV_chat_ItemAdd(string no, string ti, string ty, string cn, string wd)
        {
            var citem = LSV_chat.Items.Add(no);
            citem.SubItems.Add(ti);
            citem.SubItems.Add(ty);
            citem.SubItems.Add(cn);
            citem.SubItems.Add("");
            citem.SubItems.Add(wd);

            if (LSV_chat.SelectedItems.Count == 0)
            {
                LSV_chat.EnsureVisible(LSV_chat.Items.Count - 1);
            }

        }

        delegate void deg_LSV_chat_Add(string no, string ti, string ty, string cn, string wd);
        public void LSV_chatWriteLine(string no, string ti, string ty, string cn, string wd)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new deg_LSV_chat_Add(LSV_chatWriteLine), no, ti, ty, cn, wd);
                }
                else
                {
                    LSV_chat_ItemAdd(no, ti, ty, cn, wd);
                }
            }
            catch
            {
            }
        }
        public void LSV_chat_ItemUpdate(int row,string status)
        {
            LSV_chat.Items[row-1].SubItems[4].Text = status;
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
                    LSV_chat_ItemUpdate(row,status);
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

            // 出力デバイス

            /*
            foreach (var dev in Program.Audio_Devices)
            {
                if( dev.FriendlyName == __AudioDevice )
                {
                    Program.Audio_Device = dev;
                }
                Debug.Print($"{dev.FriendlyName} / {dev.ID}");
            }


            if ( Program.Audio_Device != null)
            {

                TPB_Event.Text = Program.Audio_Device.FriendlyName;
            }
            else
            {
                TPB_Event.Text = "Default";
            }
            */
            /* Menuのアイコン設定*/
            // Mnu_PlayerList.Image 
            switch (Program.__ChatSelWhitelist)
            {
                case 0:
                    Mnu_CharaList.Image = Properties.Resources.Icn_SelectUser_off;
                    break;
                case 1:
                    Mnu_CharaList.Image = Properties.Resources.Icn_SelectUser_chat;
                    break;
                case 2:
                    Mnu_CharaList.Image = Properties.Resources.Icn_SelectUser_voice;
                    break;
            }
            SSL_CharaList.Image = Mnu_CharaList.Image;

            switch (Program.__ChatSelUser)
            {
                case 0:
                    Mnu_PC.Image = Properties.Resources.Icn_User_off;
                    break;
                case 1:
                    Mnu_PC.Image = Properties.Resources.Icn_User_chat;
                    break;
                case 2:
                    Mnu_PC.Image = Properties.Resources.Icn_User_v1;
                    break;
                case 3:
                    Mnu_PC.Image = Properties.Resources.Icn_User_v2;
                    break;
            }
            SSL_PC.Image = Mnu_PC.Image;

            switch (Program.__ChatSelNpc)
            {
                case 0:
                    Mnu_NPC.Image = Properties.Resources.Icn_Npc_off;
                    break;
                case 1:
                    Mnu_NPC.Image = Properties.Resources.Icn_Npc_chat;
                    break;
                case 2:
                    Mnu_NPC.Image = Properties.Resources.Icn_Npc_v1;
                    break;
                case 3:
                    Mnu_NPC.Image = Properties.Resources.Icn_Npc_v2;
                    break;
            }
            SSL_NPC.Image = Mnu_NPC.Image;

            switch (Program.__Echa)
            {
                case 0:
                case 1:
                    Mnu_Echa.Image = Properties.Resources.Icn_echa_off;
                    Mnu_Echa.Checked = false;
                    break;
                case 2:
                case 3:
                    Mnu_Echa.Image = Properties.Resources.Icn_echa_Rec;
                    Mnu_Echa.Checked = true;
                    break;

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
            chat_cnt = 1;
            TPB_Event.Text = "";

        }
        /// <summary>
        /// 接続状態表示
        /// </summary>
        /// <param name="sts"></param>
        private void SLB_Clinet_Set(ClinetStatus sts)
        {
            switch (sts)
            {
                case ClinetStatus.OFF:
                    SLB_Client.Image = Properties.Resources.ClientStatus_offline;
                    break;
                case ClinetStatus.ON:
                    SLB_Client.Image = Properties.Resources.ClientStatus_connect;
                    break;
                case ClinetStatus.CHARASEL:
                    SLB_Client.Image = Properties.Resources.ClientStatus_select;
                    break;
                case ClinetStatus.ONLINE:
                    SLB_Client.Image = Properties.Resources.ClientStatus_online;
                    break;
                default:
                    break;
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


            LoadSetting();
            settingupd();
            Program.packets.ConnectEvent += onConnect;
            Program.packets.ChatEvent += onChat;
            Program.packets.PacketEvent += onDump;
            SLB_Clinet_Set(Program.packets.csts);
            SLB_Ip.Image = null;
            SLB_Ip.Text = Program.packets.svname;
            if (Program.packets.cap_sts)
            {
                SLB_IP_ForeColor(Color.Red);
            }
            else
            {
                SLB_IP_ForeColor(Color.Black);
            }

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

        }
        private void LoadSetting()
        {
            Debug.Print($"Load __ChatMode_PC:{Program.__ChatMode_PC},__ChatMode_NPC:{Program.__ChatMode_NPC},__ChatMode_Voice:{Program.__ChatMode_Voice}");
            switch ((ChatMode_PC)(Program.__ChatMode_PC))
            {
                case ChatMode_PC.DisableChat:
                    Mnu_PC.Checked = false;
                    Mnu_PC.Image = Resources.Icon_PC_Chat_Dis;
                    Mnu_PC.Tag = 1;
                    Mnu_PC_Chat.Checked = true;
                    Mnu_PC_TTS.Checked = false;

                    break;
                case ChatMode_PC.DisableTTS:
                    Mnu_PC.Checked = false;
                    Mnu_PC.Image = Resources.Icon_PC_TTS_Dis;
                    Mnu_PC.Tag = 2;
                    Mnu_PC_Chat.Checked = false;
                    Mnu_PC_TTS.Checked = true;
                    break;
                case ChatMode_PC.EnableChat:
                    Mnu_PC.Checked = true;
                    Mnu_PC.Image = Resources.Icon_PC_Chat;
                    Mnu_PC.Tag = 1;
                    Mnu_PC_Chat.Checked = true;
                    Mnu_PC_TTS.Checked = false;
                    break;
                case ChatMode_PC.EnableTTS:
                    Mnu_PC.Checked = true;
                    Mnu_PC.Image = Resources.Icon_PC_TTS;
                    Mnu_PC.Tag = 2;
                    Mnu_PC_Chat.Checked = false;
                    Mnu_PC_TTS.Checked = true;
                    break;
            }

            switch ((ChatMode_NPC)(Program.__ChatMode_NPC))
            {
                case ChatMode_NPC.DisableChat:
                    Mnu_NPC.Checked = false;
                    Mnu_NPC.Image = Resources.Icon_NPC_Chat_Dis;
                    Mnu_NPC.Tag = 1;
                    Mnu_NPC_Chat.Checked = true;
                    Mnu_NPC_TTS.Checked = false;
                    break;
                case ChatMode_NPC.DisableTTS:
                    Mnu_NPC.Checked = false;
                    Mnu_NPC.Image = Resources.Icon_NPC_TTS_Dis;
                    Mnu_NPC.Tag = 2;
                    Mnu_NPC_Chat.Checked = false;
                    Mnu_NPC_TTS.Checked = true;
                    break;
                case ChatMode_NPC.EnableChat:
                    Mnu_NPC.Checked = true;
                    Mnu_NPC.Image = Resources.Icon_NPC_Chat;
                    Mnu_NPC.Tag = 1;
                    Mnu_NPC_Chat.Checked = true;
                    Mnu_NPC_TTS.Checked = false;
                    break;
                case ChatMode_NPC.EnableTTS:
                    Mnu_NPC.Checked = true;
                    Mnu_NPC.Image = Resources.Icon_NPC_TTS;
                    Mnu_NPC.Tag = 2;
                    Mnu_NPC_Chat.Checked = false;
                    Mnu_NPC_TTS.Checked = true;
                    break;
            }
            switch ((ChatMode_Voice)(Program.__ChatMode_Voice))
            {
                case ChatMode_Voice.DisableChat:
                    Mnu_CharaList.Checked = false;
                    Mnu_CharaList.Image = Resources.Icon_CharaList_Chat_Dis;
                    Mnu_CharaList.Tag = 1;
                    Mnu_CharaList_Chat.Checked = true;
                    Mnu_CharaList_TTS.Checked = false;
                    Mnu_CharaList_Voice.Checked = false;

                    break;
                case ChatMode_Voice.DisableTTS:
                    Mnu_CharaList.Checked = false;
                    Mnu_CharaList.Image = Resources.Icon_CharaList_TTS_Dis_;
                    Mnu_CharaList.Tag = 2;
                    Mnu_CharaList_Chat.Checked = false;
                    Mnu_CharaList_TTS.Checked = true;
                    Mnu_CharaList_Voice.Checked = false;
                    break;
                case ChatMode_Voice.DisableVoice:
                    Mnu_CharaList.Checked = false;
                    Mnu_CharaList.Image = Resources.Icon_CharaList_VOICE_Dis;
                    Mnu_CharaList.Tag = 3;
                    Mnu_CharaList_Chat.Checked = false;
                    Mnu_CharaList_TTS.Checked = false;
                    Mnu_CharaList_Voice.Checked = true;
                    break;
                case ChatMode_Voice.EnableChat:
                    Mnu_CharaList.Checked = true;
                    Mnu_CharaList.Image = Resources.Icon_CharaList_Chat;
                    Mnu_CharaList.Tag = 1;
                    Mnu_CharaList_Chat.Checked = true;
                    Mnu_CharaList_TTS.Checked = false;
                    Mnu_CharaList_Voice.Checked = false;
                    break;
                case ChatMode_Voice.EnableTTS:
                    Mnu_CharaList.Checked = true;
                    Mnu_CharaList.Image = Resources.Icon_CharaList_TTS;
                    Mnu_CharaList.Tag = 2;
                    Mnu_CharaList_Chat.Checked = false;
                    Mnu_CharaList_TTS.Checked = true;
                    Mnu_CharaList_Voice.Checked = false;
                    break;
                case ChatMode_Voice.EnableVoice:
                    Mnu_CharaList.Checked = true;
                    Mnu_CharaList.Image = Resources.Icon_CharaList_VOICE;
                    Mnu_CharaList.Tag = 3;
                    Mnu_CharaList_Chat.Checked = false;
                    Mnu_CharaList_TTS.Checked = false;
                    Mnu_CharaList_Voice.Checked = true;
                    break;
            }

            Debug.Print($"Mnu_PC.Tag:{(int)Mnu_PC.Tag},Mnu_PC.Checked:{Mnu_PC.Checked}");
            Debug.Print($"Mnu_NPC.Tag:{(int)Mnu_NPC.Tag},Mnu_NPC.Checked:{Mnu_NPC.Checked}");
            Debug.Print($"Mnu_CharaList.Tag:{(int)Mnu_CharaList.Tag},Mnu_CharaList.Checked:{Mnu_CharaList.Checked}");
        }
        private void SaveSetting()
        {
            Debug.Print($"Mnu_PC.Tag:{(int)Mnu_PC.Tag},Mnu_PC.Checked:{Mnu_PC.Checked}");
            Debug.Print($"Mnu_NPC.Tag:{(int)Mnu_NPC.Tag},Mnu_NPC.Checked:{Mnu_NPC.Checked}");
            Debug.Print($"Mnu_CharaList.Tag:{(int)Mnu_CharaList.Tag},Mnu_CharaList.Checked:{Mnu_CharaList.Checked}");

            //PC
            if ((int)Mnu_PC.Tag == 1)
            {
                if (Mnu_PC.Checked)
                {
                    Program.__ChatMode_PC = (int)ChatMode_PC.EnableChat;
                }
                else
                {
                    Program.__ChatMode_PC = (int)ChatMode_PC.DisableChat;
                }
            }
            else if ((int)Mnu_PC.Tag == 2)
            {
                if (Mnu_PC.Checked)
                {
                    Program.__ChatMode_PC = (int)ChatMode_PC.EnableTTS;
                }
                else
                {
                    Program.__ChatMode_PC = (int)ChatMode_PC.DisableTTS;
                }
            }

            //NPC
            if ((int)Mnu_NPC.Tag == 1)
            {
                if (Mnu_NPC.Checked)
                {
                    Program.__ChatMode_NPC = (int)ChatMode_NPC.EnableChat;
                }
                else
                {
                    Program.__ChatMode_NPC = (int)ChatMode_NPC.DisableChat;
                }
            }
            else if ((int)Mnu_NPC.Tag == 2)
            {
                if (Mnu_NPC.Checked)
                {
                    Program.__ChatMode_NPC = (int)ChatMode_NPC.EnableTTS;
                }
                else
                {
                    Program.__ChatMode_NPC = (int)ChatMode_NPC.DisableTTS;
                }
            }

            //CharaList
            if ((int)Mnu_CharaList.Tag == 1)
            {
                if (Mnu_CharaList.Checked)
                {
                    Program.__ChatMode_Voice = (int)ChatMode_Voice.EnableChat;
                }
                else
                {
                    Program.__ChatMode_Voice = (int)ChatMode_Voice.DisableChat;
                }
            }
            else if ((int)Mnu_CharaList.Tag == 2)
            {
                if (Mnu_CharaList.Checked)
                {
                    Program.__ChatMode_Voice = (int)ChatMode_Voice.EnableTTS;
                }
                else
                {
                    Program.__ChatMode_Voice = (int)ChatMode_Voice.DisableTTS;
                }
            }
            else if ((int)Mnu_CharaList.Tag == 3)
            {
                if (Mnu_CharaList.Checked)
                {
                    Program.__ChatMode_Voice = (int)ChatMode_Voice.EnableVoice;
                }
                else
                {
                    Program.__ChatMode_Voice = (int)ChatMode_Voice.DisableVoice;
                }
            }
            Debug.Print($"save PC:{Program.__ChatMode_PC},NPC:{Program.__ChatMode_NPC},CharaList:{Program.__ChatMode_Voice}");
        }


        // Capture Status Color
        delegate void deg_SLB_IP_ForeColor(Color c);
        private void SLB_IP_ForeColor(Color c)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new deg_SLB_IP_ForeColor(SLB_IP_ForeColor), c);
                }
                else
                {
                    SLB_Client.ForeColor = c;
                }
            }
            catch { }
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
            var x = (MabiPacket)sender;
            var ex = (MabiPacketEventArgs)e;
            SLB_Clinet_Set(ex.csts);
            var sv = ConnectSvInfo(ex.svip);

            switch (sv.svno)
            {
                case 1:
                    SLB_Ip.Image = Properties.Resources.icn_mari;
                    break;
                case 2:
                    SLB_Ip.Image = Properties.Resources.icn_ruairi;
                    break;
                case 3:
                    SLB_Ip.Image = Properties.Resources.icn_tarlach;
                    break;
                default:
                    SLB_Ip.Text = "";
                    break;
            }

            if (sv.chno == 0)
            {
                SLB_Ip.Text = "";
            }
            else if (sv.chno == 7)
            {
                SLB_Ip.Text = "商店街";
            }
            else
            {
                SLB_Ip.Text = $"{sv.chno} ch";
            }

            if (ex.cap_sts)
            {
                SLB_IP_ForeColor(Color.Red);
            }
            else
            {
                SLB_IP_ForeColor(Color.Black);
            }
        }
        // ダンプメッセージ
        /// <summary>
        /// ダンプイベント受信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onDump(object sender, EventArgs e)
        {
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
            var x = (MabiPacket)sender;
            var c = (MabiPacketEventArgs)e;
            var t = DateTime.Now;
            /*
                        string cc = " ";
                        bool f_show = false;
                        bool f_voice = false;
                        string tts_name = "";
                        int tts_speed = 0;
                        int tts_volume = 0;
                        ChatVoiceEngine eng = ChatVoiceEngine.TTS;
                        int cn = -1;
                        float cs = 1.0f;
                        float cv = 1.0f;
                        float cp = 0.0f;
            */
            // PC NPC 以外はキャンセル
            if (((c.CharaTypeName != _CharaTypeNames.PC)&&(c.CharaTypeName != _CharaTypeNames.NPC))
                &&( Program.__ChatSelPet == 1 ))
            {
                return;
            }


            CharacterNameData? character = CharaList_find(c.CharacterName, c.CharaTypeName);

            //CharacterNameData? character = Program.CharaList.FirstOrDefault(x => x.CharName == c.CharacterName);
            // PCリスト自動追加
            if (Program.__WhiteList_AutoAdd == true) //&& (c.CharaTypeName == _CharaTypeNames.PC))
            {
                if (character == null)
                {
                    CharacterNameData character_add = new CharacterNameData();
                    character_add.Enabled = false;
                    character_add.CharName = c.CharacterName;
                    character_add.CharaTypeName = c.CharaTypeName;
                    character_add.VoiceEngen = ChatVoiceEngine.TTS;//エンジンをどうするか？
                    character_add.VoiceName = __TTS2Name;
                    character_add.VoiceId = -1;
                    character_add.VolumeScale = 1.0f;
                    character_add.SpeedScale = 1.0f;
                    character_add.PitchScale = 0.0f;
                    Program.CharaList.Add(character_add);
                    character = character_add;
                }
            }

            var mode = IsChatMode(character, c.CharaTypeName);

            if (mode == ChatModes.Mute)
            {
                return;
            }

            /* 1.Chat内容を表示*/
            // chat log show
            var cc = c.CharaTypeName.ToString();
            string[] li = { "" };
            LSV_chatWriteLine($"{chat_cnt}", $"{t:HH:mm:ss.fff}", $"{cc}", $"{c.CharacterName}", $"{c.ChatWord}");
            // chat log write
            li[0] = $"C {chat_cnt},{t:HH:mm:ss.fff},{cc},{c.CharacterName},{c.ChatWord}";
            TxtChatWriteLine(li[0] + Environment.NewLine);
            chat_cnt++;



            // ChatPopupへ出力
            if (ChatPop.Visible)
            {
                ChatPop.ChatPopupSend(c.ChatWord);
            }


            /* 2.TTSモードなら */

            if ((mode == ChatModes.TTS) || (mode == ChatModes.VoiceTTS))
            {
                speech_chat(Program.__TTS1Name, Program.__TTS1Volume, Program.__TTS1Speed,
                            c.CharacterName, c.ChatWord);
                LSV_chat_Update(chat_cnt-1, "TTS1");

            }
            else if ((mode == ChatModes.Voice) && (character != null))
            {
                if (character.VoiceEngen == ChatVoiceEngine.TTS)
                {
                    speech_chat(Program.__TTS2Name, Program.__TTS2Volume, Program.__TTS2Speed,
                                c.CharacterName, c.ChatWord);
                    LSV_chat_Update(chat_cnt - 1, "TTS2");
                }
                else
                {
                    voice_chat(character.VoiceEngen, character.VoiceId, character.VolumeScale,
                                character.SpeedScale, character.PitchScale,
                                c.CharacterName, c.ChatWord);
                }
            }


            /*
             * チャットの扱い
             * 1.PC or NPC  で
             * 2.表示したチャットの扱いは音無、TTS、Voice（disableは無音）
             * 
             *
             *
             */

            /******************************DEL************************
            // フィルタリング　キャラモード 0=OFF 1=ChatOnly 2=VoiceON
            f_voice = false;
            if (__ChatSelWhitelist == 0)
            // キャラモード OFF時
            {
                //　発言者で分岐
                if (c.CharaTypeName == _CharaTypeNames.PC)
                {
                    // プレイヤーの場合
                    cc = c.CharaTypeName.ToString();
                    f_show = true;
                    switch (__ChatSelUser)
                    {
                        case 0: // OFF
                            f_show = false;
                            f_voice = false;
                            break;
                        case 1: // Chat Only
                            f_voice = false;
                            break;
                        case 2: // Voice 1
                            tts_name = __TTS1Name;
                            tts_speed = __TTS1Speed;
                            tts_volume = __TTS1Volume;
                            f_voice = true;
                            break;
                        case 3: // Voice 2
                            tts_name = __TTS2Name;
                            tts_speed = __TTS2Speed;
                            tts_volume = __TTS2Volume;
                            f_voice = true;
                            break;
                        default:
                            f_show = false;
                            break;
                    }
                }
                else if (c.CharaTypeName == _CharaTypeNames.NPC)
                {
                    // NPCの場合
                    cc = "NPC";
                    f_show = true;
                    switch (__ChatSelNpc)
                    {
                        case 0: // OFF
                            f_show = false;
                            break;
                        case 1: // Chat Only
                            break;
                        case 2: // Voice 1
                            tts_name = __TTS1Name;
                            tts_speed = __TTS1Speed;
                            tts_volume = __TTS1Volume;
                            f_voice = true;
                            break;
                        case 3: // Voice 2
                            tts_name = __TTS2Name;
                            tts_speed = __TTS2Speed;
                            tts_volume = __TTS2Volume;
                            f_voice = true;
                            break;
                        default:
                            f_show = false;
                            break;
                    }
                }
                else
                {
                }
            }
            else
            // チャットまたは読上げ時、キャラリストに発言者が含まれるかチェック
            {
                CharacterNameData? character = Program.CharaList.FirstOrDefault(x => x.CharName == c.CharacterName);

                if (character != null)
                { // リストに該当有
                    if (character.CharaTypeName == c.CharaTypeName)
                    {
                        if (character.Enabled)
                        {
                            if (character.CharaTypeName == _CharaTypeNames.PC)
                            {
                                cc = "PC ";
                            }
                            else
                            {
                                cc = "NPC";
                            }
                            f_show = true;
                            // 読上げONの場合
                            if (__ChatSelWhitelist == 2)
                            {
                                eng = character.VoiceEngen;
                                cn = character.VoiceId;
                                cv = character.VolumeScale;
                                cs = character.SpeedScale;
                                cp = character.PitchScale;
                            }
                            f_voice = true;
                        }
                        else
                        {
                            f_voice = false;
                        }
                    }
                }
                else
                {
                    f_voice = false;
                }

            }

            // 表示処理

            if (f_show == true)
            {
                // chat log show
                string[] li = { "" };
                LSV_chatWriteLine($"{chat_cnt}", $"{t:HH:mm:ss.fff}", $"{cc}", $"{c.CharacterName}", $"{c.ChatWord}");

                // chat log write
                li[0] = $"C {chat_cnt},{t:HH:mm:ss.fff},{cc},{c.CharacterName},{c.ChatWord}";
                TxtChatWriteLine(li[0] + Environment.NewLine);


                chat_cnt++;


                // ChatPopupへ出力
                if (ChatPop.Visible)
                {
                    ChatPop.ChatPopupSend(c.ChatWord);
                }

            }

            // 読上げ処理

            if (f_voice)
            {
                if ((__ChatSelWhitelist == 2) && (eng == ChatVoiceEngine.TTS))
                {
                    tts_name = __TTS1Name;
                    tts_speed = __TTS1Speed;
                    tts_volume = __TTS1Volume;
                    speech_chat(tts_name, tts_volume, tts_speed, c.CharacterName, c.ChatWord);
                }
                else if (__ChatSelWhitelist == 2)
                {
                    voice_chat(eng, cn, cv, cs, cp, c.CharacterName, c.ChatWord);
                }
                else if ((__ChatSelWhitelist == 0) && (tts_name != ""))
                {
                    speech_chat(tts_name, tts_volume, tts_speed, c.CharacterName, c.ChatWord);
                }
            }
            ***************************DEL END*************************/
        }

        // Setting 
        /// <summary>
        /// 設定状態の保存
        /// </summary>
        public void settingupd()
        {
            SSL_NPC_Setdata();
            SSL_PC_Setdata();
            SSL_CharaList_Setdata();

            SLB_Mode_Icon(Program.packets.PacketMode);

            switch (Program.__SaveMode)
            {
                case 0: SLB_SaveMode.Image = Properties.Resources.WriteMode_none; break;
                case 1: SLB_SaveMode.Image = Properties.Resources.WriteMode_overwrite; break;
                case 2: SLB_SaveMode.Image = Properties.Resources.WriteMode_append; break;
                case 3: SLB_SaveMode.Image = Properties.Resources.WriteMode_timestamp; break;
                default: SLB_SaveMode.Image = null; break;
            }
            /*
                        if (Program.Audio_Device != null)
                        {
                            TPB_Event.Text = Program.Audio_Device.DeviceFriendlyName;
                        }
                        else 
                        {
                            TPB_Event.Text = "Default";
                        }
            */

        }

        /// <summary>
        /// ログのファイル出力
        /// </summary>
        /// <param name="Log"></param>
        private void ChatLogSave(System.Windows.Forms.TextBox Log)
        {
            string savefilename = __SavePath + "\\MabiChatLog";

            if (__SaveMode == 0)
            {
                return;
            }

            DateTime dt = DateTime.Now;

            if (Log.Text.Length == 0)
            {
                return;
            }
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
            TPB_Max.Text = "";
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
            SaveSetting();
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
        /// <summary>
        /// キャラクターボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_List_Click(object sender, EventArgs e)
        {
            WhiteList Frm_WhiteList = new WhiteList();
            Frm_WhiteList.Owner = this;
            if (Frm_WhiteList.ShowDialog() == DialogResult.OK)
            {
            }
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
                    SLB_Mode.Image = Properties.Resources.LogMode_chat;
                    break;
                case PacketModes.Dump:
                    SLB_Mode.Image = Properties.Resources.LogMode_dump;
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
                    msg[0] = "Chat Mode *** Start" + Environment.NewLine;
                    TxtChatWriteLine(msg[0]);
                    Program.packets.PacketMode = PacketModes.Chat;
                    SLB_Mode_Icon(Program.packets.PacketMode);
                    break;
                case PacketModes.Dump:
                    msg[0] = "Dump Mode *** Start" + Environment.NewLine;
                    TxtChatWriteLine(msg[0]);
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


        private void SSL_NPC_Setdata()
        {
            switch ((ChatMode_NPC)Program.__ChatMode_NPC)
            {
                case ChatMode_NPC.DisableChat:
                    SSL_NPC.Image = Resources.Icon_NPC_Chat_Dis;
                    break;
                case ChatMode_NPC.DisableTTS:
                    SSL_NPC.Image = Resources.Icon_NPC_TTS_Dis;
                    break;
                case ChatMode_NPC.EnableChat:
                    SSL_NPC.Image = Resources.Icon_NPC_Chat;
                    break;
                case ChatMode_NPC.EnableTTS:
                    SSL_NPC.Image = Resources.Icon_NPC_TTS;
                    break;
            }
        }

        private void SSL_PC_Setdata()
        {
            switch ((ChatMode_PC)Program.__ChatMode_PC)
            {
                case ChatMode_PC.DisableChat:
                    SSL_PC.Image = Resources.Icon_PC_Chat_Dis;
                    break;
                case ChatMode_PC.DisableTTS:
                    SSL_PC.Image = Resources.Icon_PC_TTS_Dis;
                    break;
                case ChatMode_PC.EnableChat:
                    SSL_PC.Image = Resources.Icon_PC_Chat;
                    break;
                case ChatMode_PC.EnableTTS:
                    SSL_PC.Image = Resources.Icon_PC_TTS;
                    break;
            }
        }

        private void SSL_CharaList_Setdata()
        {
            switch ((ChatMode_Voice)Program.__ChatMode_Voice)
            {
                case ChatMode_Voice.DisableChat:
                    SSL_CharaList.Image = Resources.Icon_CharaList_Chat_Dis;
                    break;
                case ChatMode_Voice.DisableTTS:
                    SSL_CharaList.Image = Resources.Icon_CharaList_TTS_Dis_;
                    break;
                case ChatMode_Voice.DisableVoice:
                    SSL_CharaList.Image = Resources.Icon_CharaList_VOICE_Dis;
                    break;
                case ChatMode_Voice.EnableChat:
                    SSL_CharaList.Image = Resources.Icon_CharaList_Chat;
                    break;
                case ChatMode_Voice.EnableTTS:
                    SSL_CharaList.Image = Resources.Icon_CharaList_TTS;
                    break;
                case ChatMode_Voice.EnableVoice:
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
                Mnu_Echa.Image = Properties.Resources.Icn_echa_Rec;
                Program.__Echa = 3;
            }
            else
            {
                Mnu_Echa.Image = Properties.Resources.Icn_echa_off;
                Program.__Echa = 0;
            }
            SSL_Echa.Image = Mnu_Echa.Image;
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
        public void SSL_Youtube_State(int sts)
        {
            if (sts == 0)
            {
                SSL_Youtube.Image = null;
            }
            else
            {
                SSL_Youtube.Image = Properties.Resources.Youtube_js;
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

                int storeChatSelUser = __ChatSelUser;
                Program.__ChatSelUser = 0;

                Replay_log frm_Replay_log = new Replay_log(ofd.FileName);
                frm_Replay_log.ShowDialog();
                Program.__ChatSelUser = storeChatSelUser;

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
                    ddata.SpeedScale = 1.0f;
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

        static RunningStatus VoiceVoxChangeing = RunningStatus.NotIstalled;
        static RunningStatus AivisSpeechChangeing = RunningStatus.NotIstalled;
        private void Tim_Status_Tick(object sender, EventArgs e)
        {
            // Voiceの起動チェック
            // 1.設定がある場合のみ
            var rsv = IsRunning(__VoiceVox);
            switch (rsv)
            {
                case RunningStatus.Running:
                    SSL_VoiceVox.Image = Properties.Resources.vvpp_0_1;
                    break;
                case RunningStatus.NotRunning:
                    SSL_VoiceVox.Image = Properties.Resources.vvpp_0_1_disable;
                    break;
                case RunningStatus.NotIstalled:
                    SSL_VoiceVox.Image = Properties.Resources.vvpp_0_1_error;
                    break;
                default:
                    break;

            }
            var rsa = IsRunning(__AivisSpeech);
            switch (rsa)
            {
                case RunningStatus.Running:
                    SSL_AivisSpeech.Image = Properties.Resources.aisp_0_1;
                    break;
                case RunningStatus.NotRunning:
                    SSL_AivisSpeech.Image = Properties.Resources.aisp_0_1_disable;
                    break;
                case RunningStatus.NotIstalled:
                    SSL_AivisSpeech.Image = Properties.Resources.aisp_0_1_error;
                    break;
                default:
                    break;

            }

            if ((VoiceVoxChangeing != rsv) || (AivisSpeechChangeing != rsa))
            {
                VoiceVoxChangeing = rsv;
                AivisSpeechChangeing = rsa;
                init_voice();
            }

        }

        private void LSV_chat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SSL_VoiceAPI_Click(object sender, EventArgs e)
        {
            init_voice();
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

        public enum ChatModes { Mute, Chat, TTS, VoiceChat, VoiceTTS, Voice }
        /*
                チャット出す条件
                    メッセージがPCでPCが活性かつChatListが不活性の場合
                    メッセージがNPCでNPCが活性かつChatListが不活性の場合
                    ※CharaList活性の場合全てチャットを表示

                TTSを行う条件
                    メッセージがPCでPCがTTS活性かつChatListが不活性の場合
                    メッセージがNPCでNPCがTTS活性かつChatListが不活性の場合
                    CharaList活性の場合、CharaNameListに存在し、有効な場合

                Voiceを行う条件
                    CharaListVoiceが選択されいること
                    CharaNameListに存在し、有効な場合
        */

        private ChatModes IsChatMode(CharacterNameData charadata, _CharaTypeNames receiveType)
        {
            if (Mnu_CharaList.Checked)
            {
                if (charadata != null)
                {
                    if (charadata.Enabled == false)
                    {
                        return (ChatModes.VoiceChat);
                    }
                }
                switch (Mnu_CharaList.Tag)
                {
                    case 1:
                        return (ChatModes.VoiceChat);
                    case 2:
                        return (ChatModes.VoiceTTS);
                    case 3:
                        return (ChatModes.Voice);
                    default:
                        break;
                }
            }

            if ((Mnu_PC.Checked) && (receiveType == _CharaTypeNames.PC))
            {
                if (Mnu_PC_TTS.Checked == true)
                {
                    return (ChatModes.TTS);
                }
                else
                {
                    return (ChatModes.Chat);
                }
            }
            else if ((Mnu_NPC.Checked) && (receiveType == _CharaTypeNames.NPC))
            {
                if (Mnu_NPC_TTS.Checked == true)
                {
                    return (ChatModes.TTS);
                }
                else
                {
                    return (ChatModes.Chat);
                }
            }

            return (ChatModes.Mute);
        }

        private void Mnu_Edit_DropDownOpening(object sender, EventArgs e)
        {
            Mnu_PC_CheckedChanged(sender, e);
            Mnu_NPC_CheckedChanged(sender, e);
            Mnu_CharaList_CheckedChanged(sender, e);
        }
        private void Mnu_Edit_DropDownClosed(object sender, EventArgs e)
        {
            SaveSetting();
            StatusStrip_Update();

        }

        private void Mnu_PC_Click(object sender, EventArgs e)
        {
            Mnu_PC.Checked = !(Mnu_PC.Checked);
            if ((Mnu_PC.Checked) && (Mnu_CharaList.Checked))
            {
                Mnu_CharaList.Checked = false;
            }
            if ((Mnu_PC.Checked == false) && (Mnu_NPC.Checked == false))
            {
                Mnu_CharaList.Checked = true;
            }
        }

        private void Mnu_PC_CheckedChanged(object sender, EventArgs e)
        {
            if (Mnu_PC.Checked)
            {
                if (Mnu_PC_Chat.Checked)
                {
                    Mnu_PC.Image = Resources.Icon_PC_Chat;
                }
                if (Mnu_PC_TTS.Checked)
                {
                    Mnu_PC.Image = Resources.Icon_PC_TTS;
                }
            }
            else
            {
                if (Mnu_PC_Chat.Checked)
                {
                    Mnu_PC.Image = Resources.Icon_PC_Chat_Dis;
                }
                if (Mnu_PC_TTS.Checked)
                {
                    Mnu_PC.Image = Resources.Icon_PC_TTS_Dis;
                }
            }
        }

        private void Mnu_PC_Chat_Click(object sender, EventArgs e)
        {
            Mnu_PC_Chat.Checked = !(Mnu_PC_Chat.Checked);
            Mnu_PC_TTS.Checked = !(Mnu_PC_TTS.Checked);
            if (Mnu_PC_Chat.Checked)
            {
                Mnu_PC.Tag = 1;
            }
        }

        private void Mnu_PC_TTS_Click(object sender, EventArgs e)
        {
            Mnu_PC_Chat.Checked = !(Mnu_PC_Chat.Checked);
            Mnu_PC_TTS.Checked = !(Mnu_PC_TTS.Checked);
            if (Mnu_PC_Chat.Checked)
            {
                Mnu_PC.Tag = 2;
            }
        }

        private void Mnu_NPC_Click(object sender, EventArgs e)
        {
            Mnu_NPC.Checked = !(Mnu_NPC.Checked);

            if ((Mnu_NPC.Checked) && (Mnu_CharaList.Checked))
            {
                Mnu_CharaList.Checked = false;
            }
            if ((Mnu_PC.Checked == false) && (Mnu_NPC.Checked == false))
            {
                Mnu_CharaList.Checked = true;
            }
        }

        private void Mnu_NPC_CheckedChanged(object sender, EventArgs e)
        {

            if (Mnu_NPC.Checked)
            {
                if (Mnu_NPC_Chat.Checked)
                {
                    Mnu_NPC.Image = Resources.Icon_NPC_Chat;
                }
                if (Mnu_NPC_TTS.Checked)
                {
                    Mnu_NPC.Image = Resources.Icon_NPC_TTS;
                }
            }
            else
            {
                if (Mnu_NPC_Chat.Checked)
                {
                    Mnu_NPC.Image = Resources.Icon_NPC_Chat_Dis;
                }
                if (Mnu_NPC_TTS.Checked)
                {
                    Mnu_NPC.Image = Resources.Icon_NPC_TTS_Dis;
                }
            }
        }

        private void Mnu_NPC_Chat_Click(object sender, EventArgs e)
        {
            Mnu_NPC_Chat.Checked = !(Mnu_NPC_Chat.Checked);
            Mnu_NPC_TTS.Checked = !(Mnu_NPC_TTS.Checked);

            if (Mnu_NPC_Chat.Checked)
            {
                Mnu_NPC.Tag = 1;
            }

        }

        private void Mnu_NPC_TTS_Click(object sender, EventArgs e)
        {
            Mnu_NPC_Chat.Checked = !(Mnu_NPC_Chat.Checked);
            Mnu_NPC_TTS.Checked = !(Mnu_NPC_TTS.Checked);

            if (Mnu_NPC_TTS.Checked)
            {
                Mnu_NPC.Tag = 2;
            }
        }

        private void Mnu_CharaList_Click(object sender, EventArgs e)
        {
            Mnu_CharaList.Checked = !(Mnu_CharaList.Checked);
            if (Mnu_CharaList.Checked)
            {
                Mnu_PC.Checked = false;
                Mnu_NPC.Checked = false;
            }
            else
            {
                Mnu_PC.Checked = true;
                Mnu_NPC.Checked = true;
            }
        }

        private void Mnu_CharaList_Chat_Click(object sender, EventArgs e)
        {
            Mnu_CharaList_Chat.Checked = true;
            Mnu_CharaList_TTS.Checked = false;
            Mnu_CharaList_Voice.Checked = false;
            Mnu_CharaList.Tag = 1;

            Mnu_CharaList_CheckedChanged(sender, e);

        }

        private void Mnu_CharaList_TTS_Click(object sender, EventArgs e)
        {
            Mnu_CharaList_Chat.Checked = false;
            Mnu_CharaList_TTS.Checked = true;
            Mnu_CharaList_Voice.Checked = false;
            Mnu_CharaList.Tag = 2;

            Mnu_CharaList_CheckedChanged(sender, e);
        }

        private void Mnu_CharaList_Voice_Click(object sender, EventArgs e)
        {
            Mnu_CharaList_Chat.Checked = false;
            Mnu_CharaList_TTS.Checked = false;
            Mnu_CharaList_Voice.Checked = true;
            Mnu_CharaList.Tag = 3;

            Mnu_CharaList_CheckedChanged(sender, e);

        }


        private void Mnu_CharaList_CheckedChanged(object sender, EventArgs e)
        {

            if (Mnu_CharaList_Chat.Checked)
            {
                Mnu_CharaList.Tag = 1;
            }
            else if (Mnu_CharaList_TTS.Checked)
            {
                Mnu_CharaList.Tag = 2;
            }
            else if (Mnu_CharaList_Voice.Checked)
            {
                Mnu_CharaList.Tag = 3;
            }

            if (Mnu_CharaList.Checked)
            {
                switch (Mnu_CharaList.Tag)
                {
                    case 1:
                        Mnu_CharaList.Image = Resources.Icon_CharaList_Chat;
                        break;
                    case 2:
                        Mnu_CharaList.Image = Resources.Icon_CharaList_TTS;
                        break;
                    case 3:
                        Mnu_CharaList.Image = Resources.Icon_CharaList_VOICE;
                        break;
                }
            }
            else
            {
                switch (Mnu_CharaList.Tag)
                {
                    case 1:
                        Mnu_CharaList.Image = Resources.Icon_CharaList_Chat_Dis;
                        break;
                    case 2:
                        Mnu_CharaList.Image = Resources.Icon_CharaList_TTS_Dis_;
                        break;
                    case 3:
                        Mnu_CharaList.Image = Resources.Icon_CharaList_VOICE_Dis;
                        break;
                }
            }
        }

        private void StatusStrip_Update()
        {
            SSL_PC.Image = Mnu_PC.Image;
            SSL_NPC.Image = Mnu_NPC.Image;
            SSL_CharaList.Image = Mnu_CharaList.Image;
        }

    }
}
