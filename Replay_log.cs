using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VoicevoxClientSharp.ApiClient.Models;
using static MabiChatSpeech.ChatVoice;
using static MabiChatSpeech.MabiChat;
using static MabiChatSpeech.WinApi;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MabiChatSpeech
{
    public partial class Replay_log : Form
    {
        private string _LogFileName;
        static public ChatVoice _chatVoice;
        private ChatLogManager _chatLogManager = new ChatLogManager();
        private List<CharacterNameData> _chatvoice { get; set; } = new();
        private CancellationTokenSource? _playbackCancellation;
        private Task? _selectedPlayTask;
        private bool _voiceDisposedForCancel;
        private bool _closingAfterPlaybackCancel;
        public string LogFileName
        {
            get { return _LogFileName; }
            set { _LogFileName = value; }
        }


        public Replay_log(string fn)
        {
            InitializeComponent();
            FormClosing += Replay_log_FormClosing;
            LogFileName = fn;
            init();
        }

        private async void Replay_log_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_closingAfterPlaybackCancel)
                return;

            e.Cancel = true;
            _closingAfterPlaybackCancel = true;
            _playbackCancellation?.Cancel();
            _voiceDisposedForCancel = true;

            try
            {
                if (_chatVoice != null)
                    await _chatVoice.DisposeAsync();

                if (_selectedPlayTask != null)
                {
                    try { await _selectedPlayTask; }
                    catch (OperationCanceledException) { }
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"Replay_log close cleanup error: {ex}");
            }
            finally
            {
                Close();
            }
        }

        private async void init()
        {
            _chatVoice = await ChatVoice.CreateMultiEngineAsync();
            RegisterChatVoiceEvents(6);

            ConnectQueueEvents();

        }
        private void ConnectQueueEvents()
        {
            if (_chatVoice == null) { return; }
            _chatVoice!.SpeechQueued += e =>
            {
                VoiceEvents(() =>
                    lbl_VoiceStatu.Text = $"{e.PendingCount}件 / {e.Engine}");
            };

            _chatVoice.QueueBecameEmpty += () =>
            {
                VoiceEvents(() =>
                    lbl_VoiceStatu.Text = "Stay");
            };
        }
        private void VoiceEvents(Action update)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            if (InvokeRequired)
                BeginInvoke(update);
            else
                update();
        }
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
            ListViewItem? row = rowIndex is int index && index >= 0 && index < listView1.Items.Count
                ? listView1.Items[index]
                : null;

            if (row is null)
                return;

            while (row.SubItems.Count <= _statusSubItemIndex)
                row.SubItems.Add("");

            row.SubItems[_statusSubItemIndex].Text = "キュー待ち";
            _boundSpeechRows.Add(row);
            _speechRowsById[e.RequestId] = row;
            Debug.Print($"ChatVoice_SpeechQueued:{row.Index}");
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

        private void Replay_log_Shown(object sender, EventArgs e)
        {
            var uniqueSet = new HashSet<string>();

            this.Text = LogFileName;
            _chatLogManager.Load(LogFileName);
            lbl_Count.Text = $"{_chatLogManager.Logs.Count}";
            foreach (var item in _chatLogManager.Logs)
            {
                var rec = listView1.Items.Add($"{item.Number}");
                rec.SubItems.Add($"{item.Time:hh\\:mm\\:ss\\.fff}");               //0
                rec.SubItems.Add($"{item.IntervalSeconds:F3}");    //1
                rec.SubItems.Add($"{item.CharaTypeName}");          //2
                rec.SubItems.Add($"{item.CharaName}");          //3
                rec.SubItems.Add($"{item.Chat}");               //4

                uniqueSet.Add(item.CharaName + "," + item.CharaTypeName);

            }
            foreach (var hed in listView1.Columns)
            {
                ((ColumnHeader)hed).Width = -1;
            }

            //キャスト一覧
            listView2.Columns.Clear();
            listView2.Columns.Add("Enabled");
            listView2.Columns.Add("Type");
            listView2.Columns.Add("CharaName");
            listView2.Columns.Add("VoiceEngen");
            listView2.Columns.Add("VoiceName");
            listView2.Columns.Add("VoiceID");
            listView2.Columns.Add("VolScal");
            listView2.Columns.Add("SpdScal");
            listView2.Columns.Add("PitScal");

            listView2.Items.Clear();

            foreach (var citem in uniqueSet)
            {
                var cnames = citem.Split(',');
                string charaname = "";
                _CharaTypeNames charatype = _CharaTypeNames.UNKNOWN;
                if (cnames.Length == 2)
                {
                    charaname = cnames[0];
                    charatype = Enum.Parse<_CharaTypeNames>(cnames[1]);

                    //CharaListを検索
                    CharacterNameData? character = Program.CharaList_find(charaname, charatype);

                    if (character == null)
                    {
                        var sitem = listView2.Items.Add(bool.FalseString);
                        sitem.SubItems.Add(charatype.ToString());               //0
                        sitem.SubItems.Add(charaname.ToString());               //1
                        sitem.SubItems.Add($"{ChatVoiceEngine.VoiceVox}");//2
                        sitem.SubItems.Add("ずんだもん-ノーマル");                                //3
                        sitem.SubItems.Add("3");                               //4
                        sitem.SubItems.Add("1.0");                              //5
                        sitem.SubItems.Add("1.2");                              //6
                        sitem.SubItems.Add("0.0");                          //7
                    }
                    else
                    {
                        var sitem = listView2.Items.Add(character.Enabled.ToString());
                        sitem.SubItems.Add(character.CharaTypeName.ToString()); //0
                        sitem.SubItems.Add(character.CharName.ToString());      //1
                        sitem.SubItems.Add(character.VoiceEngen.ToString());    //2
                        sitem.SubItems.Add(character.VoiceName);                //3
                        sitem.SubItems.Add(character.VoiceId.ToString());       //4
                        sitem.SubItems.Add(character.VolumeScale.ToString());   //5
                        sitem.SubItems.Add(character.SpeedScale.ToString());    //6
                        sitem.SubItems.Add(character.PitchScale.ToString());    //7
                    }


                }
            }
            foreach (ColumnHeader ci in listView2.Columns)
            {
                ci.Width = -1;
            }

        }


        private void CMN_Voice_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void CMN_VoiceList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private async Task SelectedPlay(CancellationToken cancellationToken)
        {
            Regex? speechRegex = null;
            string regexPattern = Txt_Regex.Text;
            if (!string.IsNullOrWhiteSpace(regexPattern))
            {
                try
                {
                    speechRegex = new Regex(regexPattern);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show($"正規表現が正しくありません。\n{ex.Message}", "正規表現");
                    return;
                }
            }

            bool switchFast = CHK_FastPlay.Checked;
            int vid = -1;
            int cnt = -1;
            int firsttime = 0;

            foreach (var item in listView1.Items.Cast<ListViewItem>())
            {
                cancellationToken.ThrowIfCancellationRequested();


                ChatVoiceEngine eng;
                Debug.Print("item:" + item.ToString());

                if (item.Selected == true)
                {
                    cnt++;
                    if (cnt == 0)
                    {
                        CHK_FastPlay.Enabled = false;
                        firsttime = (int)(double.Parse(item.SubItems[2].Text) * 1000.0);
                    }
                }
                else if ((cnt != -1) && (item.Selected == false))
                {
                    CHK_FastPlay.Enabled = true;
                    break;
                }
                else
                {
                    continue;
                }

                var cn = item.SubItems[4].Text;
                //ListView2をキャラ名で調べ、割当音声を取得
                ListViewItem foundItem = listView2.FindItemWithText(cn);
                if (foundItem != null)
                {
                    if (foundItem.SubItems.Count == 0 ||
                        !bool.TryParse(foundItem.SubItems[0].Text, out bool enabled) ||
                        !enabled)
                    {
                        item.Selected = false;
                        continue;
                    }

                    // Itemsコレクション内でのインデックス（位置）を取得する
                    int index = listView2.Items.IndexOf(foundItem);

                    var voiceName = listView2.Items[index].SubItems[4].Text;
                    if (voiceName != "")
                    {
                        // 見つかった位置を利用する処理
                        eng = Enum.Parse<ChatVoiceEngine>(listView2.Items[index].SubItems[3].Text, ignoreCase: true);
                        vid = int.Parse(listView2.Items[index].SubItems[5].Text);

                        var cv = double.Parse(listView2.Items[index].SubItems[6].Text);
                        var cs = double.Parse(listView2.Items[index].SubItems[7].Text);
                        var cp = double.Parse(listView2.Items[index].SubItems[8].Text);
                        ChatVoiceSettings p = new ChatVoiceSettings(cs, cv, cp);

                        var s = item.SubItems[5].Text;
                        if (speechRegex != null)
                        {
                            Match match = speechRegex.Match(s);
                            s = match.Success ? match.Value : s;
                        }

                        var it = (int)(double.Parse(item.SubItems[2].Text) * 1000.0);
                        if (cnt == 0) { it = 0; }

                        if ( switchFast == true )
                        {
                            _pendingSpeechRowIndex = item.Index;

                            try
                            {
                                Debug.Print($"queueAndWaitAsync {s},{eng.ToString()},{vid}");
                                await _chatVoice.EnqueueAndWaitAsync(s, eng, vid, p, cancellationToken);
                                Debug.Print($"queueAndWaitAsync Fin.");
                                _pendingSpeechRowIndex = null;
                            }
                            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }
                            catch (Exception ex)
                            {
                                Debug.Print($"queueAndWaitAsync error: {ex}");
                            }
                            finally
                            {
                                _pendingSpeechRowIndex = null;
                            }
                        }
                        else
                        {
                            Debug.Print($"設定:{eng},{voiceName}({vid})," + p.ToString());
                            Debug.Print("間隔:" + it.ToString());
                            await Task.Delay(it, cancellationToken);
                            if (vid >= 0)
                            {
                                _pendingSpeechRowIndex = item.Index;
                                try
                                {
                                    _chatVoice.Enqueue(s, eng, vid, p);
                                }
                                finally
                                {
                                    _pendingSpeechRowIndex = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Print($"listview2のデータ不具合{index}");
                    }

                }
                else
                {
                    Debug.Print($"listview2にキャラ（{cn}）の音声割当がない");

                }

                item.Selected = false;

            }

            while (_chatVoice!.PendingCount > 0)
                await Task.Delay(50, cancellationToken);

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private async void Btn_Play_Click(object sender, EventArgs e)
        {
            if (_selectedPlayTask != null)
            {
                _playbackCancellation?.Cancel();
                Btn_Play.Enabled = false;
                try
                {
                    _voiceDisposedForCancel = true;
                    await _chatVoice!.DisposeAsync();
                }
                catch (Exception ex)
                {
                    Debug.Print($"Replay_log voice shutdown error: {ex}");
                }
                return;
            }

            _playbackCancellation = new CancellationTokenSource();
            Btn_Play.Text = "Cancel";
            _selectedPlayTask = SelectedPlay(_playbackCancellation.Token);
            try
            {
                await _selectedPlayTask;
            }
            catch (OperationCanceledException)
            {
                // Cancel button requested playback cancellation.
            }
            finally
            {
                _selectedPlayTask = null;
                _playbackCancellation.Dispose();
                _playbackCancellation = null;
                Btn_Play.Text = "Play";
                Btn_Play.Enabled = true;
                CHK_FastPlay.Enabled = true;

                if (_voiceDisposedForCancel && !_closingAfterPlaybackCancel && !IsDisposed)
                {
                    try
                    {
                        _chatVoice = await ChatVoice.CreateMultiEngineAsync();
                        RegisterChatVoiceEvents(6);
                        ConnectQueueEvents();
                        _voiceDisposedForCancel = false;
                    }
                    catch (Exception ex)
                    {
                        Debug.Print($"Replay_log voice restart error: {ex}");
                        lbl_VoiceStatu.Text = "音声エンジン再接続失敗";
                    }
                }
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Selected)
                {
                    var cname = item.SubItems[4].Text;
                    var ctype = item.SubItems[3].Text;
                    var dat = Program.CharaList_find(cname, ctype);
                    if (dat == null)
                    { //該当なし
                        _CharaTypeNames charatype = Enum.Parse<_CharaTypeNames>(ctype);
                        ChatVoiceEngine engine = new ChatVoiceEngine();
                        engine = ChatVoiceEngine.VoiceVox;

                        CharacterNameData cd = new CharacterNameData();

                        cd.Enabled = false;
                        cd.CharName = cname;
                        cd.CharaTypeName = charatype;
                        cd.VoiceEngen = ChatVoiceEngine.VoiceVox;
                        cd.VoiceName = "ずんだもん-ノーマル";
                        cd.VoiceId = 3;
                        cd.VolumeScale = 1.0f;
                        cd.SpeedScale = 1.2f;
                        cd.PitchScale = 0.0f;

                        CharaListVoice_Edit fm = new CharaListVoice_Edit(cd);

                        fm.Owner = this;
                        fm.testword = item.SubItems[5].Text;
                        fm.ShowDialog();
                    }
                    else
                    {
                        CharaListVoice_Edit fm = new CharaListVoice_Edit(dat);

                        fm.Owner = this;
                        fm.testword = item.SubItems[5].Text;
                        fm.ShowDialog();
                    }
                    RefreshVoiceListItem(cname, ctype);
                    break;
                }
            }

        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.Items)
            {
                if (item.Selected)
                {

                    var cname = item.SubItems[2].Text;
                    var ctype = item.SubItems[1].Text;
                    var dat = Program.CharaList_find(cname, ctype);
                    if (dat == null)
                    { //該当なし
                        _CharaTypeNames charatype = Enum.Parse<_CharaTypeNames>(ctype);
                        ChatVoiceEngine engine = new ChatVoiceEngine();
                        engine = ChatVoiceEngine.VoiceVox;

                        CharacterNameData cd = new CharacterNameData();

                        cd.Enabled = false;
                        cd.CharName = cname;
                        cd.CharaTypeName = charatype;
                        cd.VoiceEngen = ChatVoiceEngine.VoiceVox;
                        cd.VoiceName = "ずんだもん-ノーマル";
                        cd.VoiceId = 3;
                        cd.VolumeScale = 1.0f;
                        cd.SpeedScale = 1.2f;
                        cd.PitchScale = 0.0f;

                        CharaListVoice_Edit fm = new CharaListVoice_Edit(cd);

                        fm.Owner = this;
                        fm.testword = item.SubItems[2].Text;
                        fm.ShowDialog();
                    }
                    else
                    {
                        CharaListVoice_Edit fm = new CharaListVoice_Edit(dat);

                        fm.Owner = this;
                        fm.testword = item.SubItems[2].Text;
                        fm.ShowDialog();
                    }
                    RefreshVoiceListItem(cname, ctype);
                    break;
                }
            }
        }

        private void Btn_FastPlay_Click(object sender, EventArgs e)
        {

        }

        private void RefreshVoiceListItem(string charaName, string charaType)
        {
            CharacterNameData? data = Program.CharaList_find(charaName, charaType);
            if (data == null)
                return;

            foreach (ListViewItem row in listView2.Items)
            {
                if (row.SubItems.Count < 9 ||
                    row.SubItems[1].Text != data.CharaTypeName.ToString() ||
                    row.SubItems[2].Text != data.CharName)
                    continue;

                row.SubItems[0].Text = data.Enabled.ToString();
                row.SubItems[3].Text = data.VoiceEngen.ToString();
                row.SubItems[4].Text = data.VoiceName;
                row.SubItems[5].Text = data.VoiceId.ToString();
                row.SubItems[6].Text = data.VolumeScale.ToString();
                row.SubItems[7].Text = data.SpeedScale.ToString();
                row.SubItems[8].Text = data.PitchScale.ToString();
                break;
            }
        }
    }
}
