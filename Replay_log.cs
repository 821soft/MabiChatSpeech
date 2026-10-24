using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        public ChatVoice _chatVoice;
        private ChatLogManager _chatLogManager = new ChatLogManager();
        private List<CharacterNameData> _chatvoice {get; set; } = new ();
        public string LogFileName
        {
            get { return _LogFileName; }
            set { _LogFileName = value; }
        }


        public Replay_log(string fn)
        {
            InitializeComponent();
            LogFileName = fn;
            init();
        }

        public async void init()
        {
            _chatVoice = await ChatVoice.CreateAsync();
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
                rec.SubItems.Add($"{item.Time}");
                rec.SubItems.Add($"{item.IntervalSeconds}");
                rec.SubItems.Add($"{item.CharaName}");
                rec.SubItems.Add($"{item.Chat}");

                uniqueSet.Add(item.CharaName+","+item.CharaType);

            }
            listView1.Columns[0].Width = -1;
            listView1.Columns[1].Width = -1;
            listView1.Columns[2].Width = -1;
            listView1.Columns[3].Width = -1;
            listView1.Columns[4].Width = -1;

            //キャスト一覧
            listView2.Columns.Clear();

            listView2.Columns.Add("CharaName");
            listView2.Columns.Add("Type");
            listView2.Columns.Add("TtsName");
            listView2.Columns.Add("TtsVol");
            listView2.Columns.Add("TtsSpeed");
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
                CharacterTypes charatype = CharacterTypes.Other;
                if ( cnames.Length == 2)
                {
                    charaname = cnames[0];
                    charatype = Enum.Parse <CharacterTypes>(cnames[1]);
                    
                    CharacterNameData? character = Program.CharaList.FirstOrDefault(x => x.CharName == charaname);
                    if (character == null)
                    {
                        character = new CharacterNameData();
                        character.CharName = charaname;
                        character.CharacterType = (CharacterTypes)charatype;
                        character.TtsName = "[ja-JP]Microsoft Haruka Desktop";
                        character.TtsVolume = 80;
                        character.TtsSpeed = 0;
                        character.StyleName = "まい-ノーマル";
                        character.StyleID = 1431611904;
                        character.VolumeScale = 1.0F;
                        character.SpeedScale = 1.3F;
                        character.PitchScale = 0.0F;

                        Program.CharaList.Add(character);
                    }

                    _chatvoice.Add(character);

                    var sitem = listView2.Items.Add(character.CharName);
                    sitem.SubItems.Add( character.CharacterType.ToString());
                    sitem.SubItems.Add(character.TtsName);
                    sitem.SubItems.Add(character.TtsVolume.ToString());
                    sitem.SubItems.Add(character.TtsSpeed.ToString());
                    sitem.SubItems.Add(character.StyleName);
                    sitem.SubItems.Add(character.StyleID.ToString());
                    sitem.SubItems.Add(character.VolumeScale.ToString());
                    sitem.SubItems.Add(character.SpeedScale.ToString());
                    sitem.SubItems.Add(character.PitchScale.ToString());
                }

            }
            foreach ( ColumnHeader ci in listView2.Columns)
            { 
                ci.Width = -1;
            }

            CMN_Voice.Items.Clear();
            var sps = _chatVoice.Speakers;
            foreach (var sp in sps )
            {
                var a = CMN_Voice.Items.Add($"{sp.SpeakerName}-{sp.StyleName}");
                a.Tag = sp.StyleId;
                var b = CMN_VoiceList.Items.Add($"{sp.SpeakerName}-{sp.StyleName}");
                b.Tag = sp.StyleId;

            }
        }


        private void listView1_SizeChanged(object sender, EventArgs e)
        {

            var sels = listView1.SelectedItems;
            if (sels.Count <= 0)
            {
                listView1.ContextMenuStrip = null;
            }
            else
            {
                listView1.ContextMenuStrip = CMN_Voice;
            }
        }

        private void CMN_Voice_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var sels = listView1.SelectedItems;
            var vid = (int)e.ClickedItem.Tag;

            Debug.Print($"Speaker:{e.ClickedItem.Text}");

            foreach (var rec in sels)
            {
                var irec = (ListViewItem)rec;
                var s = irec.SubItems[3].Text;

                var cn = irec.SubItems[2].Text;
                ListViewItem foundItem = listView2.FindItemWithText(cn);
                if (foundItem != null)
                {
                    // Itemsコレクション内でのインデックス（位置）を取得する
                    int index = listView2.Items.IndexOf(foundItem);

                    if (listView2.Items[index].SubItems[2].Text != "")
                    {
                        // 見つかった位置を利用する処理
                        vid = int.Parse(listView2.Items[index].SubItems[2].Text);

                        ChatVoiceSettings p = new ChatVoiceSettings();
                        _chatVoice.Enqueue(s, vid,p);

                    }
                    else
                    {
                        Debug.Print($"{cn}に割り当てた話者がありません.{s}");
                    }

                }
                else
                {
                    Debug.Print($"{cn}に割り当てた話者がありません.{s}");
                }


            }
        }

        private void CMN_VoiceList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var sels = listView2.SelectedItems;
            var vid = (int)e.ClickedItem.Tag;

            //            Debug.Print($"Speaker:{e.ClickedItem.Text}");

            if (sels.Count <= 0)
            {
                return;
            }

            var rec = sels[0];
            var bk = rec.Text;
            rec.SubItems.Clear();
            rec.Text = bk;
            rec.SubItems.Add(e.ClickedItem.Text);
            rec.SubItems.Add($"{vid}");

        }

        private async void SelectedPlay()
        {
            var sels = listView1.SelectedItems;
            var vid =0;

            if (sels.Count <= 0) 
            {
                return;
            }

            int cnt = -1;
            int firsttime = 0;
            //ログの選択内
            foreach (var rec in sels)
            {
                cnt++;
                var irec = (ListViewItem)rec;

                if (cnt == 0)
                {
                    firsttime = (int)(double.Parse(irec.SubItems[2].Text) * 1000.0);
                }
                


                var cn = irec.SubItems[3].Text;
                //ListView2をキャラ名で調べ、割当音声を取得
                ListViewItem foundItem = listView2.FindItemWithText(cn);
                if (foundItem != null)
                {
                    // Itemsコレクション内でのインデックス（位置）を取得する
                    int index = listView2.Items.IndexOf(foundItem);

                    if (listView2.Items[index].SubItems[5].Text != "")
                    {
                        // 見つかった位置を利用する処理
                        vid = int.Parse(listView2.Items[index].SubItems[6].Text);
                        var cv = listView2.Items[index].SubItems[7].Text;
                        var cs = listView2.Items[index].SubItems[8].Text;
                        var cp = listView2.Items[index].SubItems[9].Text;
                        ChatVoiceSettings p = new ChatVoiceSettings(double.Parse(cv), double.Parse(cs), double.Parse(cp));
                        var s = irec.SubItems[4].Text;

                        var it = (int)(double.Parse(irec.SubItems[2].Text) * 1000.0);
                        if (cnt == 0)
                        { it = 0; }

                        Debug.Print("設定:" + p.ToString());
                        Debug.Print("間隔:" + it.ToString());
                        await Task.Delay(it);
                        _chatVoice.Enqueue(s, vid, p);
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


            }
            sels.Clear();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedPlay();
        }
    }
}
