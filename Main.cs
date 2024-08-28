using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MabiChatSpeech
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        delegate void deg_TxtChat_Text(string text);
        public void TxtChatWriteLine(string sx)
        {
            if (this.InvokeRequired)
            {
                Invoke(new deg_TxtChat_Text(TxtChatWriteLine), sx);
            }
            else
            {
                Txt_Chat.AppendText(sx);
            }
        }

        delegate void deg_speechChat(string cn , int cv , int cs, string c1, string c2);

        public void speech_chat(string cn , int cv , int cs , string c1, string c2)
        {
            if (this.InvokeRequired)
            {
                Invoke(new deg_speechChat(speech_chat), cn, cv , cs ,c1, c2);
            }
            else
            {
                SpeechSynthesizer speech_spkp = new SpeechSynthesizer();
                speech_spkp.SetOutputToDefaultAudioDevice();
                string[] cnn = cn.Split(']');
                speech_spkp.SelectVoice(cnn[1]);

                string sayword = "";
                if (Program.__TTS_NameCall == true)
                {
                    sayword = c1 + "  ";
                }
                sayword += c2;
                speech_spkp.Volume = cv;
                speech_spkp.Rate = cs;
                speech_spkp.SpeakAsync(sayword);
            }
        }



        private void MNI_Quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MNI_Setting_Click(object sender, EventArgs e)
        {
            Setting Frm_Setting = new Setting();
            Frm_Setting.Owner = this;
            Frm_Setting.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {

            try
            {
                Txt_Chat.ForeColor = Color.FromArgb(Program.__ChatForeColor);
                Txt_Chat.BackColor = Color.FromArgb(Program.__ChatBackColor);
            }
            catch 
            { 
            }


            //フォントをロード
            try
            {
                FontStyle ffs = (FontStyle)Program.__ChatFontStyle;
                Font ff = new Font(Program.__ChatFontName, Program.__ChatFontSize,ffs);
                Txt_Chat.Font = ff;
            }
            catch
            {
            }

            //未実装なので実行パス
            if (Program.__SavePath == "")
            {
                Program.__SavePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            }
        }

        private void Tim_Status_Tick(object sender, EventArgs e)
        {
            Program.Wdt_chkstat();

            SLB_Client.Text = $"{Program.WdtStatus}"; 
            SLB_Ip.Text = Program.ChanelName(Program.ServerIP);

        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {

            charlist csw  = new charlist();
            csw.ShowDialog();

        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            Txt_Chat.Text = "";
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            settingupd();
        }
        public void settingupd()
        {
            Cmb_Whitelist.SelectedIndex = Program.__ChatSelWhitelist;
            Cmb_User.SelectedIndex = Program.__ChatSelUser;
            Cmb_Pet.SelectedIndex = Program.__ChatSelPet;
            Cmb_Npc.SelectedIndex = Program.__ChatSelNpc;
            switch (Program.__SaveMode)
            {
                case 0: SLB_SaveMode.Text = "しない"; break;
                case 1: SLB_SaveMode.Text = "上書き"; break;
                case 2: SLB_SaveMode.Text = "追記"; break;
                case 3: SLB_SaveMode.Text = "タイムスタンプ"; break;
                default: SLB_SaveMode.Text = "----"; break;
            }
        }


        private void Cmb_User_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.__ChatSelUser = Cmb_User.SelectedIndex;
        }
        private void Cmb_Pet_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.__ChatSelPet = Cmb_Pet.SelectedIndex;
        }
        private void Cmb_Npc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.__ChatSelNpc = Cmb_Npc.SelectedIndex;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.CharaList.FileTextWrite();
            Properties.Settings.Default.Save();
            Program.ChatLogSave(Txt_Chat);
        }

        private void Cmb_Whitelist_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.__ChatSelWhitelist = Cmb_Whitelist.SelectedIndex;
        }
    }
}
