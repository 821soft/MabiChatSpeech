using MabiChatSpeech.TextEmulate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MabiChatSpeech.Program.NativeMethods;
using static MabiChatSpeech.Program;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;

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

        delegate void deg_Redirect_Text(string c1 , string c2);
        public void RedirectWriteLine(string c1 , string c2)
        {
            if (this.InvokeRequired)
            {
                Invoke(new deg_Redirect_Text(RedirectWriteLine), c1,c2);
            }
            else
            {
                //リダイレクト アクティブ切替
                if(Btn_Redirect.Text == "ON")
                {
                    // リダイレクト
                    NativeMethods.SetForegroundWindow((IntPtr)Btn_Redirect.Tag);
                    string sayword = "";
                    if (Program.__TTS_NameCall == true)
                    {
                        sayword = c1 + "  ";
                    }
                    sayword += c2;

                    KeyboardEmulate keyboardEmulate = new KeyboardEmulate();
                    keyboardEmulate.writeKeys(sayword);

                }


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

        private void Btn_Setup_Click(object sender, EventArgs e)
        {
            Setting Frm_Setting = new Setting();
            Frm_Setting.Owner = this;
            Frm_Setting.ShowDialog();
        }

        private void Btn_LogView_Click(object sender, EventArgs e)
        {

        }

        private void Btn_List_Click(object sender, EventArgs e)
        {
            WhiteList Frm_WhiteList = new WhiteList();
            Frm_WhiteList.Owner = this;
            if (Frm_WhiteList.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void Txt_Chat_TextChanged(object sender, EventArgs e)
        {

        }

        private List<IntPtr> twlist = new List<IntPtr>();

        private bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
        {
            //throw new NotImplementedException();
            //ウィンドウのタイトルの長さを取得する
            var _wi = new WINDOWINFO();
            _wi.cbSize = Marshal.SizeOf(_wi);
            NativeMethods.GetWindowInfo(hWnd, ref _wi);

            var f = !((_wi.dwStyle & 0x10C00000) == 0x10C00000);

            if (f)
            {
                return true;

            }

            f = ((_wi.dwStyle & 0x80000000) == 0x80000000);
            if (f)
            {
                return true;

            }

            f = ((_wi.dwExStyle & 0x00040000) == 0x00040000);
            if (f)
            {
                return true;

            }
            int textLen = NativeMethods.GetWindowTextLength(hWnd);
            if (0 < textLen)
            {
                //ウィンドウのタイトルを取得する
                StringBuilder tsb = new StringBuilder(textLen + 1);
                NativeMethods.GetWindowText(hWnd, tsb, tsb.Capacity);

                string ma = $"{tsb}";
                if (!ma.Equals("マビノギ"))
                {
                    // AllowList.Items.Add($"{_wi.dwStyle:X8}/{_wi.dwExStyle:X8}/{hWnd:X8}/{tsb}/");
                    var item = BTN_SendTask.DropDownItems.Add(ma);
                    item.Tag = hWnd;
                }

                //Debug.Print($"{_wi.dwStyle:X8}/{_wi.dwExStyle:X8}/{hWnd:X8}/{tsb}/");
            }
            return true;
        }




        private void BTN_SendTask_DropDownOpening(object sender, EventArgs e)
        {
            BTN_SendTask.DropDownItems.Clear();
            twlist.Clear();
            NativeMethods.EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);

//                        BTN_SendTask.DropDownItems.Add("CAV " + p.ProcessName + ":" + p.MainWindowTitle);

  

        }


        private void BTN_SendTask_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Btn_Redirect.Tag = e.ClickedItem.Tag;
            BTN_SendTask.Text =  e.ClickedItem.Text;
            NativeMethods.SetForegroundWindow((IntPtr)Btn_Redirect.Tag);
        }


        private void Btn_Redirect_Click(object sender, EventArgs e)
        {
            if ( Btn_Redirect.Text == "OFF" )
            {

                // リダイレクト先が有効か確認
                Btn_Redirect.Text = "ON";
            }
            else
            {
                Btn_Redirect.Text = "OFF";
            }
        }
    }
}
