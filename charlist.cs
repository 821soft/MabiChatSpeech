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

        private void charlist_Shown(object sender, EventArgs e)
        {
            Lsv_cs.Items.Clear();
            for ( int i=0;i< Program.chatDatas.Count;i++)
            {
                ChatData item = Program.chatDatas[i];
                if (item.CharacterName != null) 
                {
                    var s = Lsv_cs.Items.Add(item.CharacterName);
                    s.SubItems.Add($"{item.CharacterType}");
                }
            }
        }
    }
}
