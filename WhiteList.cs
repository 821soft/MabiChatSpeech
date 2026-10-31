using SharpPcap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static MabiChatSpeech.MabiChat;
using static MabiChatSpeech.Main;
using static MabiChatSpeech.Program;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace MabiChatSpeech
{
    public partial class WhiteList : Form
    {
        public WhiteList()
        {
            InitializeComponent();
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            Lsv2CharaList();
            this.Close();
        }

        private void BTN_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_Ins_Click(object sender, EventArgs e)
        {
            // logIns
            charlist clf = new charlist();
            clf.ShowDialog();
            CharaList2Lsv();


        }

        private void WhiteList_Shown(object sender, EventArgs e)
        {

            CharaList2Lsv();
        }


        private void Lsv_Whitelist_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Lsv_Whitelist_KeyDown(object sender, KeyEventArgs e)
        {
            //Debug.Print($"{e.KeyCode}");
            if (e.KeyCode == Keys.Delete)
            {
                DelRec();
            }
            if (e.KeyCode == Keys.Insert)
            {
                InsRec();
            }

        }

        private void DelRec()
        {
            if (Lsv_Whitelist.SelectedItems.Count != 0)
            {
                Lsv_Whitelist.SelectedItems[0].Remove();
                Lsv_Whitelist.SelectedItems.Clear();
            }
        }

        private void InsRec()
        {
            Lsv_Whitelist.SelectedItems.Clear();
            Txt_Cn.Text = "";
            Cmb_Ct.Text = "User";
            CHK_Enabled.Checked = true;
            Cmb_TTSName.Text = Program.__TTS1Name;
            NUD_Volume.Value = Program.__TTS1Volume;
            NUD_Speed.Value = Program.__TTS1Speed;
            GPB_Item.Enabled = true;
            Txt_Cn.Focus();

        }


        private void Btn_Upd_Click(object sender, EventArgs e)
        {
            if (Txt_Cn.Text.Trim() == "")
            {
                MessageBox.Show("キャラクター名を入力", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

                Txt_Cn.Focus();
            }
            else
            {
                /*既存*/
                foreach (ListViewItem aitem in Lsv_Whitelist.Items)
                {
                    if (aitem.SubItems[0].Text == Txt_Cn.Text)
                    {
                        aitem.Checked = CHK_Enabled.Checked;
                        aitem.SubItems[0].Text = Txt_Cn.Text;
                        aitem.SubItems[1].Text = Cmb_Ct.Text;
                        aitem.SubItems[2].Text = Cmb_TTSName.Text;
                        aitem.SubItems[3].Text = $"{NUD_Volume.Value}";
                        aitem.SubItems[4].Text = $"{NUD_Speed.Value}";
                        GPB_Item.Enabled = false;
                        aitem.SubItems[5].Text = Txt_VoiceEngen.Text;
                        aitem.SubItems[6].Text = Cmb_VoiceName.Text;
                        aitem.SubItems[7].Text = Txt_StyleID.Text;
                        aitem.SubItems[8].Text = Txt_VolumeScal.Text;
                        aitem.SubItems[9].Text = Txt_SpeedScal.Text;
                        aitem.SubItems[10].Text = Txt_PitchScal.Text;
                        return;
                    }
                }
                var item = Lsv_Whitelist.Items.Add(Txt_Cn.Text);
                item.Checked = CHK_Enabled.Checked;
                item.SubItems.Add(Cmb_Ct.Text);
                item.SubItems.Add(Cmb_TTSName.Text);
                item.SubItems.Add($"{NUD_Volume.Value}");
                item.SubItems.Add($"{NUD_Speed.Value}");
                GPB_Item.Enabled = false;
                item.SubItems.Add(Txt_VoiceEngen.Text);
                item.SubItems.Add(Cmb_VoiceName.Text);
                item.SubItems.Add(Txt_StyleID.Text);
                item.SubItems.Add(Txt_VolumeScal.Text);
                item.SubItems.Add(Txt_SpeedScal.Text);
                item.SubItems.Add(Txt_PitchScal.Text);

            }
        }
        private void CharaList2Lsv()
        {
            // Program.CharaList.Clear();


            Lsv_Whitelist.Items.Clear();
            Lsv_Whitelist.Columns.Clear();
            Lsv_Whitelist.Columns.Add("有効");
            Lsv_Whitelist.Columns.Add("キャラ名");    //1
            Lsv_Whitelist.Columns.Add("種別");        //2      
            Lsv_Whitelist.Columns.Add("エンジン");    //3      
            Lsv_Whitelist.Columns.Add("ボイス名");    //4
            Lsv_Whitelist.Columns.Add("ボイスID");    //5
            Lsv_Whitelist.Columns.Add("音量");        //6
            Lsv_Whitelist.Columns.Add("話速");        //7
            Lsv_Whitelist.Columns.Add("音高");        //8

            foreach (CharacterNameData character in Program.CharaList)
            {
                Debug.Print(character.ToString());
                Lsv_Whitelist.Items.Add(character.ToItem());
            }
            foreach (ColumnHeader col in Lsv_Whitelist.Columns)
            {
                col.Width = -1;
            }


        }
        private void Lsv2CharaList()
        {
            Program.CharaList.Clear();

            foreach (ListViewItem item in Lsv_Whitelist.Items)
            {
                Program.CharaList.Add(new CharacterNameData(item));
            }

            Main.SaveCharaList(__SavePath + "\\CharacterNameList.txt");
        }

        ListViewItemComparer listViewItemSorter;

        private void WhiteList_Load(object sender, EventArgs e)
        {
            Cmb_TTSName.Items.Clear();
            foreach (var ttsname in TTS_NameList)
            {
                Cmb_TTSName.Items.Add(ttsname);
            }
            if (Cmb_TTSName.Items.Count < 0)
            {
                Cmb_TTSName.Enabled = false;
            }
            else
            {
                Cmb_TTSName.Enabled = true;
            }
            //ListViewItemComparerの作成と設定
            listViewItemSorter = new ListViewItemComparer();
            listViewItemSorter.ColumnModes =
                new ListViewItemComparer.ComparerMode[]
            {
                ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.Integer,
                ListViewItemComparer.ComparerMode.Integer
            };
            //ListViewItemSorterを指定する
            Lsv_Whitelist.ListViewItemSorter = listViewItemSorter;

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Lsv_Whitelist_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //クリックされた列を設定
            listViewItemSorter.Column = e.Column;
            //並び替える
            Lsv_Whitelist.Sort();
        }
        /// <summary>
        /// ListViewの項目の並び替えに使用するクラス
        /// </summary>
        public class ListViewItemComparer : IComparer
        {
            /// <summary>
            /// 比較する方法
            /// </summary>
            public enum ComparerMode
            {
                /// <summary>
                /// 文字列として比較
                /// </summary>
                String,
                /// <summary>
                /// 数値（Int32型）として比較
                /// </summary>
                Integer,
                /// <summary>
                /// 日時（DataTime型）として比較
                /// </summary>
                DateTime
            };

            private int _column;
            private SortOrder _order;
            private ComparerMode _mode;
            private ComparerMode[] _columnModes;

            /// <summary>
            /// 並び替えるListView列の番号
            /// </summary>
            public int Column
            {
                set
                {
                    //現在と同じ列の時は、昇順降順を切り替える
                    if (_column == value)
                    {
                        if (_order == SortOrder.Ascending)
                        {
                            _order = SortOrder.Descending;
                        }
                        else if (_order == SortOrder.Descending)
                        {
                            _order = SortOrder.Ascending;
                        }
                    }
                    _column = value;
                }
                get
                {
                    return _column;
                }
            }
            /// <summary>
            /// 昇順か降順か
            /// </summary>
            public SortOrder Order
            {
                set
                {
                    _order = value;
                }
                get
                {
                    return _order;
                }
            }
            /// <summary>
            /// 並び替えの方法
            /// </summary>
            public ComparerMode Mode
            {
                set
                {
                    _mode = value;
                }
                get
                {
                    return _mode;
                }
            }
            /// <summary>
            /// 列ごとの並び替えの方法
            /// </summary>
            public ComparerMode[] ColumnModes
            {
                set
                {
                    _columnModes = value;
                }
            }

            /// <summary>
            /// ListViewItemComparerクラスのコンストラクタ
            /// </summary>
            /// <param name="col">並び替える列の番号</param>
            /// <param name="ord">昇順か降順か</param>
            /// <param name="cmod">並び替えの方法</param>
            public ListViewItemComparer(
                int col, SortOrder ord, ComparerMode cmod)
            {
                _column = col;
                _order = ord;
                _mode = cmod;
            }
            public ListViewItemComparer()
            {
                _column = 0;
                _order = SortOrder.Ascending;
                _mode = ComparerMode.String;
            }

            //xがyより小さいときはマイナスの数、大きいときはプラスの数、
            //同じときは0を返す
            public int Compare(object x, object y)
            {
                if (_order == SortOrder.None)
                {
                    //並び替えない時
                    return 0;
                }

                int result = 0;
                //ListViewItemの取得
                ListViewItem itemx = (ListViewItem)x;
                ListViewItem itemy = (ListViewItem)y;

                //並べ替えの方法を決定
                if (_columnModes != null && _columnModes.Length > _column)
                {
                    _mode = _columnModes[_column];
                }

                //並び替えの方法別に、xとyを比較する
                switch (_mode)
                {
                    case ComparerMode.String:
                        //文字列をとして比較
                        result = string.Compare(itemx.SubItems[_column].Text,
                            itemy.SubItems[_column].Text);
                        break;
                    case ComparerMode.Integer:
                        //Int32に変換して比較
                        //.NET Framework 2.0からは、TryParseメソッドを使うこともできる
                        result = int.Parse(itemx.SubItems[_column].Text).CompareTo(
                            int.Parse(itemy.SubItems[_column].Text));
                        break;
                    case ComparerMode.DateTime:
                        //DateTimeに変換して比較
                        //.NET Framework 2.0からは、TryParseメソッドを使うこともできる
                        result = DateTime.Compare(
                            DateTime.Parse(itemx.SubItems[_column].Text),
                            DateTime.Parse(itemy.SubItems[_column].Text));
                        break;
                }

                //降順の時は結果を+-逆にする
                if (_order == SortOrder.Descending)
                {
                    result = -result;
                }

                //結果を返す
                return result;
            }
        }

        private void Lsv_Whitelist_DoubleClick(object sender, EventArgs e)
        {

        }

        private void Cmb_VoiceName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Txt_StyleID.Text = $"{Main.VoiceLists[Cmb_VoiceName.SelectedIndex].VoiceId}";
            Txt_VoiceEngen.Text = Main.VoiceLists[Cmb_VoiceName.SelectedIndex].VoiceEngen.ToString();
        }

        private void CMN_Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }
        private void CharaListVoice_edit()
        {

            foreach (ListViewItem item in Lsv_Whitelist.Items)
            {
                if (item.Selected)
                {

                    var cname = item.SubItems[1].Text;
                    var ctype = item.SubItems[2].Text;
                    var dat = Program.CharaList_find(cname, ctype);
                    if (dat == null)
                    { //該当なし
                        return;
                    }

                    CharaListVoice_Edit fm = new CharaListVoice_Edit(item);
                    fm.Owner = this;
                    fm.testword = item.SubItems[1].Text;
                    fm.ShowDialog();
                    break;
                }
            }

        }
        private void Lsv_Whitelist_Click(object sender, EventArgs e)
        {

        }

        private void Lsv_Whitelist_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isDoubleClicking)
            {
                e.NewValue = e.CurrentValue;
            }

        }
        private bool isDoubleClicking = false;

        private void Lsv_Whitelist_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 1. フラグを立てる
            isDoubleClicking = true;
            foreach (ListViewItem item in Lsv_Whitelist.Items)
            {
                if (item.Selected)
                {
                    item.Checked = !(item.Checked);
                    break;
                }
            }
            // 2. ダブルクリック時の「本来やりたい処理」をここに書く
            CharaListVoice_edit();
            // 3. イベントの最後にフラグを戻す
            isDoubleClicking = false;

        }
    }
}
