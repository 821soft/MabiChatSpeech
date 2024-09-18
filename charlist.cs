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
                MabiPacket.ChatData item = MabiPacket.chatDatas[i];
                if (item.CharacterName != null)
                {

                    if (
                        ((item.CharacterType == CharacterTypes.User)
                        && (MNI_SelUser.Checked == true))
                        ||
                        ((item.CharacterType == CharacterTypes.Npc)
                        && (MNI_SelNpc.Checked == true))
                        ||
                        (
                         (item.CharacterType != CharacterTypes.User) && (item.CharacterType != CharacterTypes.Npc)
                        && (MNI_SelOther.Checked == true))
                    )
                    {
                        var s = Lsv_cs.Items.Add(item.CharacterName);
                        s.SubItems.Add($"{item.CharacterType}");
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
            for ( int idx = 0;idx < Lsv_cs.Items.Count; idx ++ )
            {
                var a_item = Lsv_cs.Items[idx];
                if (a_item.Checked == true)
                {
                    var f = CharaList.FindCharacterName(a_item.Text);
                    if ( f ==  null )
                    {
                        CharacterTypes ty;
                        switch (a_item.SubItems[1].Text)
                        {
                            case "User": ty = CharacterTypes.User; 
                                break;
                            case "Npc": ty = CharacterTypes.Npc;
                                break;
                            case "Pet": ty = CharacterTypes.Pet;
                                break;
                            default:
                                ty = (CharacterTypes)byte.Parse(a_item.SubItems[1].Text);
                                break;
                        }
                        MabiChat.CharacterNameData item = new MabiChat.CharacterNameData(a_item.Text, true,ty,__TTS1Name,__TTS1Volume,__TTS1Speed);
                        CharaList.CNlist.Add(item);
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
    }
}
