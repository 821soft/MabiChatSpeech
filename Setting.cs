using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MabiChatSpeech
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Setting_Shown(object sender, EventArgs e)
        {

            // データセット
            Txt_CapProgram.Text = Program.__CapureProgram;
            Txt_CapPort.Text = $"{ Program.__CapturePort}";

            Cmb_SaveMode.SelectedIndex = Program.__SaveMode;
            Txt_SavePath.Text = Program.__SavePath;


            Control[] cs = Program.Frm_Main.Controls.Find("Txt_Chat",true);
            Lab_Font.Font = cs[0].Font;
            Lab_Font.Text = $"{cs[0].Font.Size}pt {cs[0].Font.FontFamily.Name}";
            Lab_Font.ForeColor = cs[0].ForeColor;
            Lab_Font.BackColor = cs[0].BackColor;


            Cmb_Whitelist.SelectedIndex = Program.__ChatSelWhitelist;
            Cmb_User.SelectedIndex = Program.__ChatSelUser;
            Cmb_Pet.SelectedIndex = Program.__ChatSelPet;
            Cmb_NPC.SelectedIndex = Program.__ChatSelNpc;

            Cmb_TTS1Name.Text = Program.__TTS1Name;
            Nud_TTS1Volume.Value = Program.__TTS1Volume;
            Nud_TTS1Speed.Value = Program.__TTS1Speed;
            Cmb_TTS2Name.Text = Program.__TTS2Name;
            Nud_TTS2Volume.Value = Program.__TTS2Volume;
            Nud_TTS2Speed.Value = Program.__TTS2Speed;
            Chk_TTSMute.Checked = Program.__TTS_Mute;
            Chk_TTSNameCall.Checked = Program.__TTS_NameCall;

            CHK_AutoAdd.Checked = Program.__WhiteList_AutoAdd ;

        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {

            // データセット
            Program.__CapureProgram = Txt_CapProgram.Text ;
            Program.__CapturePort = uint.Parse(Txt_CapPort.Text) ;

            Program.__SaveMode = Cmb_SaveMode.SelectedIndex ;
            Program.__SavePath = Txt_SavePath.Text ;


            // 

            Program.__ChatSelWhitelist = Cmb_Whitelist.SelectedIndex ;
            Program.__ChatSelUser = Cmb_User.SelectedIndex ;
            Program.__ChatSelPet = Cmb_Pet.SelectedIndex ;
            Program.__ChatSelNpc = Cmb_NPC.SelectedIndex ;

            Program.__TTS1Name = Cmb_TTS1Name.Text ;
            Program.__TTS1Volume = (int)(Nud_TTS1Volume.Value);
            Program.__TTS1Speed = (int)Nud_TTS1Speed.Value;
            Program.__TTS2Name = Cmb_TTS2Name.Text;
            Program.__TTS2Volume = (int)Nud_TTS2Volume.Value ;
            Program.__TTS2Speed = (int)Nud_TTS2Speed.Value ;
            Program.__TTS_Mute = Chk_TTSMute.Checked;
            Program.__TTS_NameCall = Chk_TTSNameCall.Checked;


            Control[] cs = Program.Frm_Main.Controls.Find("Txt_Chat", true);
            cs[0].Font = Lab_Font.Font;
            cs[0].ForeColor = Lab_Font.ForeColor;
            cs[0].BackColor = Lab_Font.BackColor;

            Program.__ChatForeColor = Lab_Font.ForeColor.ToArgb();
            Program.__ChatBackColor = Lab_Font.BackColor.ToArgb();


            Program.__ChatFontName = Lab_Font.Font.Name;
            Program.__ChatFontSize = Lab_Font.Font.Size;
            Program.__ChatFontStyle = (int)Lab_Font.Font.Style;
            Program.__ChatFontCharSet = Lab_Font.Font.GdiCharSet;
            Program.__WhiteList_AutoAdd =CHK_AutoAdd.Checked;

            Properties.Settings.Default.Save();
            this.Close();
            Program.Frm_Main.settingupd();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_Font_Click(object sender, EventArgs e)
        {
            fontDialog1 = new FontDialog();
            fontDialog1.ShowColor = true;

            Control[] cs = Program.Frm_Main.Controls.Find("Txt_Chat", true);
            fontDialog1.Font = cs[0].Font;
            fontDialog1.Color = cs[0].ForeColor;

            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                Lab_Font.Font = fontDialog1.Font;
                Lab_Font.ForeColor = fontDialog1.Color;
            }
        }

        private void Btn_BackColor_Click(object sender, EventArgs e)
        {
            Control[] cs = Program.Frm_Main.Controls.Find("Txt_Chat", true);

            colorDialog1 = new ColorDialog();
            colorDialog1.Color = cs[0].BackColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Lab_Font.BackColor = colorDialog1.Color;
            }

        }

        private void Btn_ServerInfo_Click(object sender, EventArgs e)
        {
            ServerInfo Frm_ServerInfo = new ServerInfo();
            Frm_ServerInfo.Owner = this;
            Frm_ServerInfo.ShowDialog();
        }

        private void Txt_SavePath_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.Description = Txt_SavePath.Text;
            fbd.ShowNewFolderButton = true;
            fbd.SelectedPath = Txt_SavePath.Text;

            if ( fbd.ShowDialog() == DialogResult.OK )
            {
                Txt_SavePath.Text = fbd.SelectedPath;
            }
            
        }

        private void Btn_WhiteList_Click(object sender, EventArgs e)
        {
            WhiteList Frm_WhiteList = new WhiteList();
            Frm_WhiteList.Owner = this;
            if ( Frm_WhiteList.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            // TTS
            Cmb_TTS1Name.Items.Clear();
            Cmb_TTS2Name.Items.Clear();
            foreach (var s in Program.TTS_NameList)
            {
                Cmb_TTS1Name.Items.Add(s);
            }
            foreach (var s in Program.TTS_NameList)
            {
                Cmb_TTS2Name.Items.Add(s);
            }
        }
    }
}
