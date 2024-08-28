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
                /**/
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
                var item = Program.CharaList.listrec2item(i); 
                Debug.Print($"{i}-{item.Text}");
                Lsv_Whitelist.Items.Add(item);
            }
        }
        private void Lsv2CharaList()
        {
            Program.CharaList.CNlist.Clear();
            for ( int index=0;index< Lsv_Whitelist.Items.Count;index++)
            {
                var item = Program.CharaList.item2listrec(Lsv_Whitelist.Items[index]);
                Program.CharaList.CNlist.Add(item);
            }
            Program.CharaList.FileTextWrite();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

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
        }
    }
}
