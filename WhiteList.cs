using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics.Eventing.Reader;
using static System.Windows.Forms.LinkLabel;
using SharpPcap;
using System.Numerics;
using System.Diagnostics;
using static MabiChatSpeech.Program;
using System.Reflection;
using System.Collections;
using static MabiChatSpeech.MabiChat;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

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

        private void Btn_Ins_Click(object sender, EventArgs e)
        {
            // logIns
            charlist  clf = new charlist();
            clf.ShowDialog();
            CharaList2Lsv();


        }

        private void WhiteList_Shown(object sender, EventArgs e)
        {

            GPB_Item.Enabled = false;
            CharaList2Lsv();
        }

        private void Lsv_Whitelist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( Lsv_Whitelist.SelectedItems.Count == 0 )
            {
                Txt_Cn.Text = "";
                Cmb_Ct.Text = "User";
                Cmb_TTSName.Text = __TTS1Name;
                NUD_Volume.Value = 0;
                NUD_Speed.Value = 0;
                CHK_Enabled.Checked = true;
                GPB_Item.Enabled = false;
                return;
            }
            var item = (ListViewItem)Lsv_Whitelist.SelectedItems[0];
            if (item != null)
            {
                Txt_Cn.Text = item.SubItems[0].Text;
                Cmb_Ct.Text = item.SubItems[1].Text;
                Cmb_TTSName.Text =(string)item.SubItems[2].Text;
                NUD_Volume.Value = int.Parse(item.SubItems[3].Text);
                NUD_Speed.Value = int.Parse(item.SubItems[4].Text);
                CHK_Enabled.Checked = item.Checked;

                GPB_Item.Enabled = true;
            }
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

        private void CMN_Menu_Opening(object sender, CancelEventArgs e)
        {
            // Selected  or Non-Select
            CMN_Menu.Items.Clear();
            if ( Lsv_Whitelist.SelectedItems.Count > 0 )
            {
                // TTS Item
                var sname = Lsv_Whitelist.SelectedItems[0].SubItems[2].Text;
                // Selected Menu TTSName , Volume , Speed 
                foreach ( var vname in TTS_NameList)
                {
                    var mitem = (ToolStripMenuItem)CMN_Menu.Items.Add( vname );
                    if ( mitem.Text == sname )
                    {
                        mitem.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        mitem.CheckState = CheckState.Unchecked;
                    }
                    mitem.Click += mitem_Click;
                }
                //
                var sep = new ToolStripSeparator();
                CMN_Menu.Items.Add(sep);

                ToolStripTextBox MNT_VolEdit = new ToolStripTextBox();
                MNT_VolEdit.Text = Lsv_Whitelist.SelectedItems[0].SubItems[3].Text;
                MNT_VolEdit.TextChanged += MNT_Vol_Leave;
                CMN_Menu.Items.Add(MNT_VolEdit);

                ToolStripTextBox MNT_SpdEdit = new ToolStripTextBox();
                MNT_SpdEdit.Text = Lsv_Whitelist.SelectedItems[0].SubItems[4].Text;
                MNT_SpdEdit.TextChanged += MNT_Spd_Leave;
                CMN_Menu.Items.Add(MNT_SpdEdit);
            }
            else
            {

            }
        }
        private void mitem_Click(object sender, EventArgs e)
        {
            var mobj = (ToolStripMenuItem)sender;
            Lsv_Whitelist.SelectedItems[0].SubItems[2].Text = mobj.Text;
        }
        private void MNT_Vol_Leave(object sender, EventArgs e)
        {
            var sobj = (ToolStripTextBox)sender;
            var idx = Lsv_Whitelist.SelectedIndices[0];
            if (sobj != null)
            {
                Lsv_Whitelist.Items[idx].SubItems[3].Text = sobj.Text;
            }
        }
        private void MNT_Spd_Leave(object sender, EventArgs e)
        {
            var sobj = (ToolStripTextBox)sender;
            var idx = Lsv_Whitelist.SelectedIndices[0];
            if (sobj != null)
            {
                Lsv_Whitelist.Items[idx].SubItems[4].Text = sobj.Text;
            }
        }

        private void CMN_Menu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {

        }

        private void DelRec()
        {
            if (Lsv_Whitelist.SelectedItems.Count != 0)
            {
                Lsv_Whitelist.SelectedItems[0].Remove();
                Lsv_Whitelist.SelectedItems.Clear();
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DelRec();
        }

        private void InsRec()
        {
            Lsv_Whitelist.SelectedItems.Clear();
            Txt_Cn.Text = "";
            Cmb_Ct.Text = "User";
            CHK_Enabled.Checked = true;
            Cmb_TTSName.Text =  Program.__TTS1Name;
            NUD_Volume.Value = Program.__TTS1Volume ;
            NUD_Speed.Value = Program.__TTS1Speed;
            GPB_Item.Enabled = true;
            Txt_Cn.Focus();

        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            InsRec();
        }

        private void Btn_Upd_Click(object sender, EventArgs e)
        {
            if (Txt_Cn.Text.Trim() == "")
            {
                MessageBox.Show("キャラクター名を入力","Error",
                MessageBoxButtons.OK,MessageBoxIcon.Error);
                
                Txt_Cn.Focus();
            }
            else
            {
                /*既存*/
                foreach( ListViewItem aitem in Lsv_Whitelist.Items)
                {
                    if( aitem.SubItems[0].Text== Txt_Cn.Text)
                    {
                        aitem.Checked = CHK_Enabled.Checked;
                        aitem.SubItems[0].Text = Txt_Cn.Text;
                        aitem.SubItems[1].Text = Cmb_Ct.Text;
                        aitem.SubItems[2].Text = Cmb_TTSName.Text;
                        aitem.SubItems[3].Text = $"{NUD_Volume.Value}";
                        aitem.SubItems[4].Text = $"{NUD_Speed.Value}";
                        GPB_Item.Enabled = false;
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


            }
        }
        private void CharaList2Lsv()
        {
            Lsv_Whitelist.Items.Clear();
            Lsv_Whitelist.Columns.Clear();
            Lsv_Whitelist.Columns.Add("CharacterName");
            Lsv_Whitelist.Columns.Add("Type");
            Lsv_Whitelist.Columns.Add("TTSName");
            Lsv_Whitelist.Columns.Add("Volume");
            Lsv_Whitelist.Columns.Add("Speed");
            Lsv_Whitelist.Columns[0].Width = 180;
            Lsv_Whitelist.Columns[1].Width = 50;
            Lsv_Whitelist.Columns[2].Width = 150;
            Lsv_Whitelist.Columns[3].Width = 50;
            Lsv_Whitelist.Columns[4].Width = 50;

            GPB_Item.Enabled = false;
            Debug.Print($"Read item count:{Program.CharaList.CNlist.Count}");
            for (int i = 0; i < Program.CharaList.CNlist.Count; i++)
            {
                var item = Program.CharaList.DataToItem(i); 
                Debug.Print($"{i}-{item.Text}");
                Lsv_Whitelist.Items.Add(item);
            }
        }
        private void Lsv2CharaList()
        {
            Program.CharaList.CNlist.Clear();
            for ( int index=0;index< Lsv_Whitelist.Items.Count;index++)
            {
                var item = new CharacterNameData(Lsv_Whitelist.Items[index]);
                Program.CharaList.CNlist.Add(item);
            }
            Program.CharaList.CNlist.Sort((a, b) => a.CharName.CompareTo(b.CharName));
            Program.CharaList.FileTextWrite();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
        ListViewItemComparer listViewItemSorter;

        private void WhiteList_Load(object sender, EventArgs e)
        {
            Cmb_TTSName.Items.Clear();
            foreach ( var ttsname in TTS_NameList )
            {
                Cmb_TTSName.Items.Add(ttsname);
            }
            if (Cmb_TTSName.Items.Count < 0)
            {
                Cmb_TTSName.Enabled = false;
            }
            else
            {
                Cmb_TTSName.Enabled = true ;
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
            Lsv_Whitelist.Items.Clear ();
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

    }
}
