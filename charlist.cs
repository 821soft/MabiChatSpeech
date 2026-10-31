using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MabiChatSpeech.MabiChat;
using static MabiChatSpeech.Program;

namespace MabiChatSpeech
{
    public partial class charlist : Form
    {
        public charlist()
        {
            InitializeComponent();
        }

        private void LsvShow()
        {
            Lsv_cs.Items.Clear();
            for (int i = 0; i < MabiPacket.chatDatas.Count; i++)
            {
                ChatData item = MabiPacket.chatDatas[i];
                if (item.CharacterName != null)
                {

                    if (
                        ((item.CharaTypeName == _CharaTypeNames.PC)
                        && (MNI_SelUser.Checked == true))
                        ||
                        ((item.CharaTypeName == _CharaTypeNames.NPC)
                        && (MNI_SelNpc.Checked == true))
                        ||
                        (
                         (item.CharaTypeName != _CharaTypeNames.PC) && (item.CharaTypeName != _CharaTypeNames.NPC)
                        && (MNI_SelOther.Checked == true))
                    )
                    {
                        var s = Lsv_cs.Items.Add(item.CharacterName);
                        s.SubItems.Add(item.CharaTypeName.ToString());
                    }
                }
            }
        }
        private void charlist_Shown(object sender, EventArgs e)
        {
            LsvShow();
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            /*
            foreach ( var t in Program.chatDatas ) {
            var f = Program.CharaList.FindCharacterName("")
            */
            for (int idx = 0; idx < Lsv_cs.Items.Count; idx++)
            {
                var a_item = Lsv_cs.Items[idx];
                if (a_item.Checked == true)
                {
                    CharacterNameData? character = Program.CharaList.FirstOrDefault(x => x.CharName == a_item.Text);
                    if ( character == null )
                    {
                        character = new CharacterNameData();
                        character.CharName = a_item.Text;
                        // 
                        character.CharaTypeName = (_CharaTypeNames)Enum.Parse(typeof(_CharaTypeNames), a_item.SubItems[1].Text); 
                        character.Enabled = true;


                        CharaList.Add(character);
                    }
                }
            }
            this.Close();
        }

        private void Btn_Reload_Click(object sender, EventArgs e)
        {
            LsvShow();
        }

        private void MnuTypeselect_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
        }

        private void MNI_SelUser_CheckedChanged(object sender, EventArgs e)
        {
            LsvShow();

        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {

        }

        private void allCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int idx = 0; idx < Lsv_cs.Items.Count; idx++)
            {
                var a_item = Lsv_cs.Items[idx];
                a_item.Checked = true;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            for (int idx = 0; idx < Lsv_cs.Items.Count; idx++)
            {
                var a_item = Lsv_cs.Items[idx];
                if (a_item.Checked == true)
                {
                    a_item.Checked = false;
                }
                else
                {
                    a_item.Checked = true;
                }
            }
        }
    }
}
