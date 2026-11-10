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

        private void WhiteList_Shown(object sender, EventArgs e)
        {

            CharaList2Lsv();
        }


        private void Lsv_Whitelist_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Lsv_Whitelist_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void DelRec()
        {
            if (Lsv_Whitelist.SelectedItems.Count != 0)
            {
                foreach (ListViewItem rec in Lsv_Whitelist.SelectedItems)
                {
                    rec.Remove();
                }
            }
        }

        private void CharaList2Lsv()
        {
            Lsv_Whitelist.Items.Clear();

            foreach (CharacterNameData character in Program.CharaList)
            {
                Debug.Print(character.ToString());
                Lsv_Whitelist.Items.Add(character.ToItem());
            }


            //            foreach (ColumnHeader col in Lsv_Whitelist.Columns)
            //            {
            //                col.Width = -1;
            //            }


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
            //ListViewItemComparerの作成と設定
            listViewItemSorter = new ListViewItemComparer();
            listViewItemSorter.ColumnModes =
                new ListViewItemComparer.ComparerMode[]
            {
                ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.String
            };
            //ListViewItemSorterを指定する
            Lsv_Whitelist.ListViewItemSorter = listViewItemSorter;

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

        private void Lsv_Whitelist_KeyUp(object sender, KeyEventArgs e)
        {
            // 押されたキーが Delete キーか判定
            if (e.KeyCode == Keys.Delete)
            {
                // 選択されているアイテムが存在するか確認
                if (Lsv_Whitelist.SelectedItems.Count > 0)
                {
                    // 複数選択を考慮して後ろから（または反復して）削除
                    foreach (ListViewItem item in Lsv_Whitelist.SelectedItems)
                    {
                        Lsv_Whitelist.Items.Remove(item);
                    }
                }
            }

        }
    }
}
