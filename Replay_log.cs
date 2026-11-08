using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
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
        private ChatLogManager _chatLogManager = new ChatLogManager();
        private List<CharacterNameData> _chatvoice { get; set; } = new();
        private CancellationTokenSource? _playbackCancellation;
        private Task? _selectedPlayTask;
        private bool _voiceDisposedForCancel;
        private bool _closingAfterPlaybackCancel;
        private int _listView2SortColumn = -1;
        private SortOrder _listView2SortOrder = SortOrder.None;
        public string LogFileName
        {
            get { return _LogFileName; }
            set { _LogFileName = value; }
        }


        public Replay_log(string fn)
        {
            InitializeComponent();
            FormClosing += Replay_log_FormClosing;
            listView2.ColumnClick += listView2_ColumnClick;
            listView2.MouseDown += listView2_MouseDown;
            CMN_VoiceList.Items.Add("CSVを読み込む", null, LoadVoiceListCsv_Click);
            CMN_VoiceList.Items.Add("CSVに保存", null, SaveVoiceListCsv_Click);
            CMN_VoiceList.Items.Add("選択行を削除", null, DeleteVoiceListRow_Click);
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
                try
                {
                    SaveCastSettingsWithConfirmation();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"キャスト設定を保存できませんでした。\n{ex.Message}", "CSV保存", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Close();
            }
        }

        private async void init()
        {
            if( Main._chatVoice != null)
            {
                RegisterChatVoiceEvents(6);
                ConnectQueueEvents();
            }

        }
        private void ConnectQueueEvents()
        {
            if (Main._chatVoice == null) { return; }
            Main._chatVoice!.SpeechQueued += e =>
            {
                VoiceEvents(() =>
                    lbl_VoiceStatu.Text = $"{e.PendingCount}件 / {e.Engine}");
            };

            Main._chatVoice.QueueBecameEmpty += () =>
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

            if (Main._chatVoice == null)
                return;

            Main._chatVoice.SpeechQueued += ChatVoice_SpeechQueued;
            Main._chatVoice.SpeechStatusChanged += ChatVoice_SpeechStatusChanged;
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

        private void EngeinStatus()
        {

            lbl_VoiceVox.Image = Program.GetEngeinIconImage(Program.__VoiceVox );
            lbl_AivisSpeech.Image = Program.GetEngeinIconImage(Program.__AivisSpeech );
        }

        private void Replay_log_Shown(object sender, EventArgs e)
        {
            var uniqueSet = new HashSet<string>();

            EngeinStatus();

            lbl_FileName.Text = Path.GetFileName(LogFileName);
            lbl_Cast.Text = "なし";
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
            columnHeader11.Width = 80;

            //キャスト一覧
            listView2.Columns.Clear();
            listView2.Columns.Add("有効");            //0
            listView2.Columns.Add("種別");            //1
            listView2.Columns.Add("キャラ名");        //2
            listView2.Columns.Add("音声エンジン");    //3
            listView2.Columns.Add("音声キャラ");      //4
            listView2.Columns.Add("音声ID");          //5
            listView2.Columns.Add("音量倍率");        //6
            listView2.Columns.Add("話速倍率");        //7
            listView2.Columns.Add("音高");            //8

            SetListView2ColumnWidths();

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
                        var sitem = listView2.Items.Add(bool.TrueString);
                        sitem.SubItems.Add(charatype.ToString());               //0
                        sitem.SubItems.Add(charaname.ToString());               //1
                        sitem.SubItems.Add($"{ChatVoiceEngine.TTS}");//2
                        sitem.SubItems.Add(Program.__TTS1Name);                                //3
                        sitem.SubItems.Add("-1");                               //4
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

            string castCsvPath = GetCastCsvPath();
            if (File.Exists(castCsvPath))
            {
                DialogResult result = MessageBox.Show(
                    this,
                    $"キャスト設定ファイルが見つかりました。\n{castCsvPath}\n\n読み込みますか？\n「いいえ」を選ぶとCharacterListの設定を使用します。",
                    "キャスト設定CSV",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (LoadVoiceListCsvFile(castCsvPath))
                        lbl_Cast.Text = Path.GetFileName(castCsvPath);
                }
                else
                {
                    try
                    {
                        File.Delete(castCsvPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, $"CSVを破棄できませんでした。\n{ex.Message}", "CSV破棄", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            UpdateLogCharacterColors();

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
            Stopwatch enqueueScheduleClock = Stopwatch.StartNew();
            TimeSpan enqueueTargetTime = TimeSpan.Zero;

            foreach (var item in listView1.Items.Cast<ListViewItem>())
            {
                cancellationToken.ThrowIfCancellationRequested();


                ChatVoiceEngine eng;
                Debug.Print("item:" + item.ToString());

                if (item.Selected == true)
                {
                    item.EnsureVisible();
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

                double intervalSeconds = double.Parse(item.SubItems[2].Text);
                if (cnt == 0)
                    enqueueTargetTime = TimeSpan.Zero;
                else if (!switchFast)
                    enqueueTargetTime += TimeSpan.FromSeconds(Math.Max(0, intervalSeconds));

                var cn = item.SubItems[4].Text;
                var charaType = item.SubItems[3].Text;
                // キャラ名と種別でListView2の音声割当を検索
                ListViewItem? foundItem = listView2.Items
                    .Cast<ListViewItem>()
                    .FirstOrDefault(row => row.SubItems.Count > 2 &&
                        row.SubItems[1].Text == charaType &&
                        row.SubItems[2].Text == cn);
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

                        // ChatVoice未起動時はChatVoice系エンジンの行をスキップする。
                        // TTS指定の行はMain.speech_chatで引き続き再生する。
                        if (eng != ChatVoiceEngine.TTS && Main._chatVoice == null)
                        {
                            item.Selected = false;
                            continue;
                        }

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

                        bool useTts = eng == ChatVoiceEngine.TTS;
                        if (switchFast && !useTts)
                        {
                            _pendingSpeechRowIndex = item.Index;

                            try
                            {
                                Debug.Print($"queueAndWaitAsync {s},{eng.ToString()},{vid}");
                                await Main._chatVoice.EnqueueAndWaitAsync(s, eng, vid, p, cancellationToken);
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
                            TimeSpan remaining = enqueueTargetTime - enqueueScheduleClock.Elapsed;
                            Debug.Print("次のキュー投入まで:" + Math.Max(0, remaining.TotalMilliseconds).ToString("F0"));
                            if (!useTts)
                            {
                                _pendingSpeechRowIndex = item.Index;
                                try
                                {
                                    if (remaining > TimeSpan.Zero)
                                        await Task.Delay(remaining, cancellationToken);
                                    Main._chatVoice!.Enqueue(s, eng, vid, p);
                                }
                                finally
                                {
                                    _pendingSpeechRowIndex = null;
                                }
                            }
                            else
                            {
                                if (remaining > TimeSpan.Zero)
                                    await Task.Delay(remaining, cancellationToken);
                                Program.Frm_Main.speech_chat(Program.__TTS1Name, Program.__TTS1Volume, Program.__TTS1Speed, cn, s);
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

            while (Main._chatVoice?.PendingCount > 0)
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
                    if (Main._chatVoice != null)
                    {
                        _voiceDisposedForCancel = true;
                        await Main._chatVoice.DisposeAsync();
                    }
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
                        Main._chatVoice = await ChatVoice.CreateMultiEngineAsync();
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
                    ListViewItem? castItem = listView2.Items
                        .Cast<ListViewItem>()
                        .FirstOrDefault(row => row.SubItems.Count >= 9 &&
                            row.SubItems[1].Text == ctype &&
                            row.SubItems[2].Text == cname);
                    if (castItem == null)
                        break;

                    CharacterNameData edited = new CharacterNameData
                    {
                        Enabled = bool.TryParse(castItem.SubItems[0].Text, out bool enabled) && enabled,
                        CharaTypeName = Enum.Parse<_CharaTypeNames>(castItem.SubItems[1].Text),
                        CharName = castItem.SubItems[2].Text,
                        VoiceEngen = Enum.Parse<ChatVoiceEngine>(castItem.SubItems[3].Text, ignoreCase: true),
                        VoiceName = castItem.SubItems[4].Text,
                        VoiceId = int.Parse(castItem.SubItems[5].Text),
                        VolumeScale = float.Parse(castItem.SubItems[6].Text),
                        SpeedScale = float.Parse(castItem.SubItems[7].Text),
                        PitchScale = float.Parse(castItem.SubItems[8].Text)
                    };

                    CharaListVoice_Edit fm = new CharaListVoice_Edit(edited, updateProgramCharaList: false);
                    fm.Owner = this;
                    fm.testword = item.SubItems[5].Text;
                    if (fm.ShowDialog() == DialogResult.OK)
                        RefreshVoiceListItem(item.SubItems[4].Text, item.SubItems[3].Text, edited);
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
                    if (item.SubItems.Count < 9)
                        break;

                    CharacterNameData edited = new CharacterNameData
                    {
                        Enabled = bool.TryParse(item.SubItems[0].Text, out bool enabled) && enabled,
                        CharaTypeName = Enum.Parse<_CharaTypeNames>(ctype),
                        CharName = cname,
                        VoiceEngen = Enum.Parse<ChatVoiceEngine>(item.SubItems[3].Text, ignoreCase: true),
                        VoiceName = item.SubItems[4].Text,
                        VoiceId = int.Parse(item.SubItems[5].Text),
                        VolumeScale = float.Parse(item.SubItems[6].Text),
                        SpeedScale = float.Parse(item.SubItems[7].Text),
                        PitchScale = float.Parse(item.SubItems[8].Text)
                    };

                    CharaListVoice_Edit fm = new CharaListVoice_Edit(edited, updateProgramCharaList: false);
                    fm.Owner = this;
                    if (fm.ShowDialog() == DialogResult.OK)
                        RefreshVoiceListItem(cname, ctype, edited);
                    break;
                }
            }
        }

        
        private void RefreshVoiceListItem(string charaName, string charaType)
        {
            CharacterNameData? data = Program.CharaList_find(charaName, charaType);
            if (data == null)
                return;

            RefreshVoiceListItem(charaName, charaType, data);
        }

        private void RefreshVoiceListItem(string charaName, string charaType, CharacterNameData data)
        {
            bool found = false;
            foreach (ListViewItem row in listView2.Items)
            {
                if (row.SubItems.Count < 9 ||
                    row.SubItems[1].Text != charaType ||
                    row.SubItems[2].Text != charaName)
                    continue;

                row.SubItems[0].Text = data.Enabled.ToString();
                row.SubItems[1].Text = data.CharaTypeName.ToString();
                row.SubItems[2].Text = data.CharName;
                row.SubItems[3].Text = data.VoiceEngen.ToString();
                row.SubItems[4].Text = data.VoiceName;
                row.SubItems[5].Text = data.VoiceId.ToString();
                row.SubItems[6].Text = data.VolumeScale.ToString();
                row.SubItems[7].Text = data.SpeedScale.ToString();
                row.SubItems[8].Text = data.PitchScale.ToString();
                found = true;
                break;
            }

            if (!found)
            {
                var row = listView2.Items.Add(data.Enabled.ToString());
                row.SubItems.Add(data.CharaTypeName.ToString());
                row.SubItems.Add(data.CharName);
                row.SubItems.Add(data.VoiceEngen.ToString());
                row.SubItems.Add(data.VoiceName);
                row.SubItems.Add(data.VoiceId.ToString());
                row.SubItems.Add(data.VolumeScale.ToString());
                row.SubItems.Add(data.SpeedScale.ToString());
                row.SubItems.Add(data.PitchScale.ToString());
                if (_listView2SortColumn >= 0)
                    listView2.Sort();
            }
            UpdateLogCharacterColors();
        }

        private void UpdateLogCharacterColors()
        {
            var voiceAssignments = listView2.Items
                .Cast<ListViewItem>()
                .Where(row => row.SubItems.Count > 2)
                .GroupBy(row => (Type: row.SubItems[1].Text, Name: row.SubItems[2].Text))
                .ToDictionary(group => group.Key, group =>
                    group.Last().SubItems.Count > 3 &&
                    Enum.TryParse(group.Last().SubItems[3].Text, ignoreCase: true, out ChatVoiceEngine engine) &&
                    engine == ChatVoiceEngine.TTS);

            foreach (ListViewItem logRow in listView1.Items)
            {
                bool isTts = logRow.SubItems.Count > 4 &&
                    voiceAssignments.TryGetValue(
                        (Type: logRow.SubItems[3].Text, Name: logRow.SubItems[4].Text),
                        out bool assignedToTts) && assignedToTts;
                logRow.ForeColor = isTts ? Color.Blue : Color.Red;
            }
        }

        private void listView2_ColumnClick(object? sender, ColumnClickEventArgs e)
        {
            if (_listView2SortColumn == e.Column)
            {
                _listView2SortOrder = _listView2SortOrder == SortOrder.Ascending
                    ? SortOrder.Descending
                    : SortOrder.Ascending;
            }
            else
            {
                _listView2SortColumn = e.Column;
                _listView2SortOrder = SortOrder.Ascending;
            }

            listView2.ListViewItemSorter = new VoiceListViewComparer(
                _listView2SortColumn, _listView2SortOrder);
            listView2.Sort();
        }

        private sealed class VoiceListViewComparer : IComparer
        {
            private readonly int _column;
            private readonly SortOrder _order;

            public VoiceListViewComparer(int column, SortOrder order)
            {
                _column = column;
                _order = order;
            }

            public int Compare(object? x, object? y)
            {
                if (x is not ListViewItem left || y is not ListViewItem right)
                    return 0;

                string leftText = _column < left.SubItems.Count ? left.SubItems[_column].Text : "";
                string rightText = _column < right.SubItems.Count ? right.SubItems[_column].Text : "";
                int result;

                if (_column == 0 && bool.TryParse(leftText, out bool leftEnabled) &&
                    bool.TryParse(rightText, out bool rightEnabled))
                {
                    result = leftEnabled.CompareTo(rightEnabled);
                }
                else if ((_column is 5 or 6 or 7 or 8) &&
                    double.TryParse(leftText, out double leftNumber) &&
                    double.TryParse(rightText, out double rightNumber))
                {
                    result = leftNumber.CompareTo(rightNumber);
                }
                else
                {
                    result = StringComparer.CurrentCultureIgnoreCase.Compare(leftText, rightText);
                }

                return _order == SortOrder.Descending ? -result : result;
            }
        }

        private void LoadVoiceListCsv_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "CSV形式のテキスト (*.txt)|*.txt|すべてのファイル (*.*)|*.*",
                Title = "音声設定CSVを読み込む"
            };
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            if (LoadVoiceListCsvFile(dialog.FileName) &&
                string.Equals(Path.GetFullPath(dialog.FileName), Path.GetFullPath(GetCastCsvPath()), StringComparison.OrdinalIgnoreCase))
            {
                lbl_Cast.Text = Path.GetFileName(dialog.FileName);
            }
        }

        private bool LoadVoiceListCsvFile(string path)
        {
            try
            {
                List<CharacterNameData> imported = ParseVoiceListCsv(File.ReadAllText(path, Encoding.UTF8));

                listView2.BeginUpdate();
                try
                {
                    listView2.Items.Clear();
                    foreach (CharacterNameData data in imported)
                    {
                        var row = listView2.Items.Add(data.Enabled.ToString());
                        row.SubItems.Add(data.CharaTypeName.ToString());
                        row.SubItems.Add(data.CharName);
                        row.SubItems.Add(data.VoiceEngen.ToString());
                        row.SubItems.Add(data.VoiceName);
                        row.SubItems.Add(data.VoiceId.ToString(CultureInfo.InvariantCulture));
                        row.SubItems.Add(data.VolumeScale.ToString(CultureInfo.InvariantCulture));
                        row.SubItems.Add(data.SpeedScale.ToString(CultureInfo.InvariantCulture));
                        row.SubItems.Add(data.PitchScale.ToString(CultureInfo.InvariantCulture));
                    }
                }
                finally
                {
                    listView2.EndUpdate();
                }

                if (_listView2SortColumn >= 0)
                    listView2.Sort();
                SetListView2ColumnWidths();
                UpdateLogCharacterColors();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"CSVを読み込めませんでした。\n{ex.Message}", "CSV読み込み", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void SaveVoiceListCsv_Click(object? sender, EventArgs e)
        {
            try
            {
                string outputPath = GetCastCsvPath();
                if (!SaveCastSettingsWithConfirmation())
                    return;
                MessageBox.Show(this, $"CSVを保存しました。\n{outputPath}", "CSV保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"CSVを保存できませんでした。\n{ex.Message}", "CSV保存", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteVoiceListRow_Click(object? sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
                return;

            ListViewItem row = listView2.SelectedItems[0];
            string charaName = row.SubItems.Count > 2 ? row.SubItems[2].Text : row.Text;
            DialogResult result = MessageBox.Show(
                this,
                $"「{charaName}」の行を削除しますか？",
                "音声設定の削除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            listView2.Items.Remove(row);
            UpdateLogCharacterColors();
        }

        private void listView2_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ListViewItem? row = listView2.GetItemAt(e.X, e.Y);
            if (row != null)
                row.Selected = true;
        }

        private void SaveVoiceListCsvFile(string outputPath)
        {
            using var writer = new StreamWriter(outputPath, false, new UTF8Encoding(true));
            writer.WriteLine(string.Join(",", new[] { "Enabled", "Type", "CharaName", "VoiceEngine", "VoiceName", "VoiceID", "VolScal", "SpdScal", "PitScal" }));
            foreach (ListViewItem row in listView2.Items)
                writer.WriteLine(string.Join(",", row.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(sub => EscapeCsv(sub.Text))));
        }

        private bool SaveCastSettingsWithConfirmation()
        {
            List<CharacterNameData> castSettings = ParseVoiceListCsv(BuildVoiceListCsv());
            DialogResult confirmation = ShowLargeCastSaveConfirmation(castSettings.Count);
            if (confirmation == DialogResult.Cancel)
                return false;

            SaveVoiceListCsvFile(GetCastCsvPath());

            if (confirmation == DialogResult.No)
                return true;

            foreach (CharacterNameData data in castSettings)
            {
                CharacterNameData? existing = Program.CharaList_find(data.CharName, data.CharaTypeName);
                if (existing == null)
                    Program.CharaList.Add(data);
                else
                {
                    existing.Enabled = data.Enabled;
                    existing.VoiceEngen = data.VoiceEngen;
                    existing.VoiceName = data.VoiceName;
                    existing.VoiceId = data.VoiceId;
                    existing.VolumeScale = data.VolumeScale;
                    existing.SpeedScale = data.SpeedScale;
                    existing.PitchScale = data.PitchScale;
                }
            }

            return true;
        }

        private DialogResult ShowLargeCastSaveConfirmation(int itemCount)
        {
            using var dialog = new Form
            {
                Text = "キャスト設定ファイルの保存確認",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false,
                ClientSize = new Size(620, 270),
                Font = new Font("Yu Gothic UI", 14F, FontStyle.Regular)
            };

            var message = new Label
            {
                AutoSize = false,
                Location = new Point(24, 22),
                Size = new Size(572, 158),
                Text = $"キャスト設定ファイル({Path.GetFileName(GetCastCsvPath())})の保存方法を選択してください。\n\nはい: キャスト設定ファイルと音声割当ファイル（CharacterNameList.txt）を保存\nいいえ: キャスト設定ファイルのみ保存\nキャンセル: 保存しない\n\n対象: {itemCount}件",
                TextAlign = ContentAlignment.MiddleLeft
            };
            dialog.Controls.Add(message);

            var yesButton = new System.Windows.Forms.Button
            {
                Text = "はい",
                DialogResult = DialogResult.Yes,
                Size = new Size(150, 44),
                Location = new Point(105, 205),
                Font = dialog.Font
            };
            var noButton = new System.Windows.Forms.Button
            {
                Text = "いいえ",
                DialogResult = DialogResult.No,
                Size = new Size(150, 44),
                Location = new Point(265, 205),
                Font = dialog.Font
            };
            var cancelButton = new System.Windows.Forms.Button
            {
                Text = "キャンセル",
                DialogResult = DialogResult.Cancel,
                Size = new Size(150, 44),
                Location = new Point(425, 205),
                Font = dialog.Font
            };
            dialog.Controls.AddRange(new Control[] { yesButton, noButton, cancelButton });
            dialog.AcceptButton = yesButton;
            dialog.CancelButton = cancelButton;

            return dialog.ShowDialog(this);
        }

        private string BuildVoiceListCsv()
        {
            var lines = new List<string>
            {
                "Enabled,Type,CharaName,VoiceEngine,VoiceName,VoiceID,VolScal,SpdScal,PitScal"
            };
            foreach (ListViewItem row in listView2.Items)
                lines.Add(string.Join(",", row.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(sub => EscapeCsv(sub.Text))));
            return string.Join(Environment.NewLine, lines);
        }

        private static CharacterNameData CopyCharacterData(CharacterNameData source) => new()
        {
            Enabled = source.Enabled,
            CharName = source.CharName,
            CharaTypeName = source.CharaTypeName,
            VoiceEngen = source.VoiceEngen,
            VoiceName = source.VoiceName,
            VoiceId = source.VoiceId,
            VolumeScale = source.VolumeScale,
            SpeedScale = source.SpeedScale,
            PitchScale = source.PitchScale
        };

        private string GetCastCsvPath()
        {
            string directory = Path.GetDirectoryName(LogFileName) ?? Environment.CurrentDirectory;
            string fileName = Path.GetFileNameWithoutExtension(LogFileName) + "_Cast.txt";
            return Path.Combine(directory, fileName);
        }

        private void SetListView2ColumnWidths()
        {
            foreach (ColumnHeader column in listView2.Columns)
                column.Width = -1;

            if (listView2.Columns.Count > 0) listView2.Columns[0].Width = 50;
            if (listView2.Columns.Count > 1) listView2.Columns[1].Width = 50;
            if (listView2.Columns.Count > 6) listView2.Columns[6].Width = 70;
            if (listView2.Columns.Count > 7) listView2.Columns[7].Width = 70;
            if (listView2.Columns.Count > 8) listView2.Columns[8].Width = 70;
        }

        private static List<CharacterNameData> ParseVoiceListCsv(string csv)
        {
            List<List<string>> records = ParseCsvRecords(csv);
            var result = new List<CharacterNameData>();
            for (int rowIndex = 0; rowIndex < records.Count; rowIndex++)
            {
                List<string> fields = records[rowIndex];
                if (fields.Count == 1 && string.IsNullOrWhiteSpace(fields[0]))
                    continue;
                if (rowIndex == 0 && fields.Count > 0 && fields[0].TrimStart('\uFEFF').Equals("Enabled", StringComparison.OrdinalIgnoreCase))
                    continue;
                if (fields.Count != 9)
                    throw new FormatException($"{rowIndex + 1}行目は9列必要ですが、{fields.Count}列です。");

                if (!bool.TryParse(fields[0], out bool enabled) ||
                    !Enum.TryParse(fields[1], true, out _CharaTypeNames charaType) ||
                    !Enum.TryParse(fields[3], true, out ChatVoiceEngine engine) ||
                    !int.TryParse(fields[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out int voiceId) ||
                    !float.TryParse(fields[6], NumberStyles.Float, CultureInfo.InvariantCulture, out float volume) ||
                    !float.TryParse(fields[7], NumberStyles.Float, CultureInfo.InvariantCulture, out float speed) ||
                    !float.TryParse(fields[8], NumberStyles.Float, CultureInfo.InvariantCulture, out float pitch))
                    throw new FormatException($"{rowIndex + 1}行目に不正な値があります。");

                result.Add(new CharacterNameData
                {
                    Enabled = enabled,
                    CharaTypeName = charaType,
                    CharName = fields[2],
                    VoiceEngen = engine,
                    VoiceName = fields[4],
                    VoiceId = voiceId,
                    VolumeScale = volume,
                    SpeedScale = speed,
                    PitchScale = pitch
                });
            }
            return result;
        }

        private static List<List<string>> ParseCsvRecords(string csv)
        {
            var records = new List<List<string>>();
            var record = new List<string>();
            var field = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < csv.Length; i++)
            {
                char ch = csv[i];
                if (inQuotes)
                {
                    if (ch == '"' && i + 1 < csv.Length && csv[i + 1] == '"')
                    {
                        field.Append('"');
                        i++;
                    }
                    else if (ch == '"')
                    {
                        inQuotes = false;
                    }
                    else
                    {
                        field.Append(ch);
                    }
                }
                else if (ch == '"' && field.Length == 0)
                {
                    inQuotes = true;
                }
                else if (ch == ',')
                {
                    record.Add(field.ToString());
                    field.Clear();
                }
                else if (ch is '\r' or '\n')
                {
                    record.Add(field.ToString());
                    field.Clear();
                    records.Add(record);
                    record = new List<string>();
                    if (ch == '\r' && i + 1 < csv.Length && csv[i + 1] == '\n')
                        i++;
                }
                else
                {
                    field.Append(ch);
                }
            }

            if (inQuotes)
                throw new FormatException("CSVの引用符が閉じていません。");
            if (field.Length > 0 || record.Count > 0)
            {
                record.Add(field.ToString());
                records.Add(record);
            }
            return records;
        }

        private static string EscapeCsv(string value) =>
            value.IndexOfAny(new[] { ',', '"', '\r', '\n' }) >= 0
                ? $"\"{value.Replace("\"", "\"\"")}\""
                : value;

        private void timer1_Tick(object sender, EventArgs e)
        {
            EngeinStatus();
        }
    }
}
