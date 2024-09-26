﻿
using System;
using System.IO;
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
using static MabiChatSpeech.Program;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;
using static MabiChatSpeech.MabiChat;

namespace MabiChatSpeech
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        // チャット
        delegate void deg_TxtChat_Text(string text);
        public void TxtChatWriteLine(string sx)
        {
            try
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
            catch 
            { 
            }
        }

        // リダイレクト
        delegate void deg_Redirect_Text(string c1 , string c2);
        public void RedirectWriteLine(string c1 , string c2)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new deg_Redirect_Text(RedirectWriteLine), c1, c2);
                }
                else
                {
                    //リダイレクト アクティブ切替
                    if (Btn_Redirect.Text == "ON")
                    {
                        // リダイレクト
                        WinApi.SetForegroundWindow((IntPtr)Btn_Redirect.Tag);
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
            catch
            { 
            } 
        }

        // 読上げ
        delegate void deg_speechChat(string cn , int cv , int cs, string c1, string c2);
        private void speech_chat(string cn , int cv , int cs , string c1, string c2)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new deg_speechChat(speech_chat), cn, cv, cs, c1, c2);
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
            catch
            { 
            }
        }

        private void MNI_Quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Main_Load(object sender, EventArgs e)
        {

            try
            {
                Txt_Chat.ForeColor = Program.Color16List[Program.__ChatFColor];
                Txt_Chat.BackColor = Program.Color16List[Program.__ChatBColor];
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

            //
            if (Program.__SavePath == "")
            {
                // Program.__SavePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                Program.__SavePath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) +"\\821Soft";
            }
            if (!(File.Exists(__SavePath)))
            {
                Directory.CreateDirectory(__SavePath);
            }

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
            Program.packets.ConnectEvent += onConnect;
            Program.packets.ChatEvent += onChat;
            Program.packets.PacketEvent += onDump;
            SLB_Client.Text = $"{Program.packets.csts}";
            SLB_Ip.Text = Program.packets.svname;
            if (Program.packets.cap_sts)
            {
                SLB_IP_ForeColor(Color.Red);
            }
            else
            {
                SLB_IP_ForeColor(Color.Black);
            }


        }

        // Capture Status Color
        delegate void deg_SLB_IP_ForeColor(Color c);
        private void SLB_IP_ForeColor(Color c)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new deg_SLB_IP_ForeColor(SLB_IP_ForeColor), c);
                }
                else
                {
                    SLB_Client.ForeColor = c;
                }
            }
            catch { }
        }

        // Status Change
        private void onConnect(object sender, EventArgs e)
        {
            var x = (MabiPacket)sender;
            var ex = (MabiPacketEventArgs)e;
            SLB_Client.Text = $"{ex.csts}";
            SLB_Ip.Text = ex.svname;
            if (ex.cap_sts)
            {
                SLB_IP_ForeColor( Color.Red );
            }
            else
            {
                SLB_IP_ForeColor(Color.Black);
            }
        }
        private void onDump(object sender, EventArgs e)
        {
            var x = (MabiPacket)sender;
            var c = (MabiPacketEventArgs)e;
            TxtChatWriteLine(c.PacketDump);
        }
        // On Chat
        private void onChat(object sender, EventArgs e)
        {
            var x = (MabiPacket)sender;
            var c = (MabiPacketEventArgs)e;
            var t = DateTime.Now;
            string cc = " ";
            bool f_show = false;
            string tts_name = "";
            int tts_speed = 0;
            int tts_volume = 0;

            if ( ( Program.__WhiteList_AutoAdd == true ) && ( c.CharacterType == CharacterTypes.User) )
            {
                var fc = Program.CharaList.FindCharacterName(c.CharacterName);

                if (fc == null)
                {
                    var item = new CharacterNameData(c.CharacterName,true,c.CharacterType,
                                                        __TTS1Name,__TTS1Volume,__TTS1Speed);
                    Program.CharaList.CNlist.Add(item);
                }
            }

            // フィルタリング
            if ( __ChatSelWhitelist == 0)
            {
                if ( c.CharacterType == CharacterTypes.User )
                {
                    cc = "PC ";
                    f_show = true;
                    switch ( __ChatSelUser )
                    {
                        case 0: // OFF
                            f_show = false;
                            break;
                        case 1: // Chat Only
                            break;
                        case 2: // Voice 1
                            tts_name = __TTS1Name;
                            tts_speed = __TTS1Speed;
                            tts_volume = __TTS1Volume;
                            break;
                        case 3: // Voice 2
                            tts_name = __TTS2Name;
                            tts_speed = __TTS2Speed;
                            tts_volume = __TTS2Volume;
                            break;
                        default:
                            f_show = false;
                            break;
                    }
                }
                else
                {
                    cc = "NPC";
                    f_show = true;
                    switch ( __ChatSelNpc )
                    {
                        case 0: // OFF
                            f_show = false;
                            break;
                        case 1: // Chat Only
                            break;
                        case 2: // Voice 1
                            tts_name = __TTS1Name;
                            tts_speed = __TTS1Speed;
                            tts_volume = __TTS1Volume;
                            break;
                        case 3: // Voice 2
                            tts_name = __TTS2Name;
                            tts_speed = __TTS2Speed;
                            tts_volume = __TTS2Volume;
                            break;
                        default:
                            f_show = false;
                            break;
                    }
                }
            }
            else
            {
                var fc = Program.CharaList.FindCharacterName(c.CharacterName);

                if (fc != null )
                {
                    if (fc.CharacterType == c.CharacterType)
                    {
                        if (fc.Enabled)
                        {
                            if (fc.CharacterType == CharacterTypes.User)
                            {
                                cc = "PC ";
                            }
                            else
                            {
                                cc = "NPC";
                            }
                            f_show = true;
                            if (__ChatSelWhitelist == 2)
                            {
                                tts_name = fc.TtsName;
                                tts_speed = fc.TtsSpeed;
                                tts_volume = fc.TtsVolume;
                            }
                        }
                    }
                }

            }

            if ( f_show == true )
            {
                TxtChatWriteLine($"{t:HH:mm:ss.fff},{cc},{c.CharacterName},{c.ChatWord} " + Environment.NewLine);
            }

            if ( tts_name != "" )
            {
                speech_chat(tts_name, tts_volume, tts_speed, c.CharacterName, c.ChatWord);
            }

            if( Btn_Redirect.Tag != null)
            {
                if (Btn_Redirect.Text == "ON")
                {
                    RedirectWriteLine(c.CharacterName, c.ChatWord);
                }
            }
        }

        // Setting 
        public void settingupd()
        {
            Cmb_Whitelist.SelectedIndex = Program.__ChatSelWhitelist;
            Cmb_User.SelectedIndex = Program.__ChatSelUser;
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
        private void ChatLogSave(System.Windows.Forms.TextBox Log)
        {
            string savefilename = __SavePath + "\\MabiChatLog";

            if (Log.Text.Length == 0)
            {
                return;
            }
            DateTime dt = DateTime.Now;

            switch (__SaveMode)
            {
                case 0: //しない
                    break;
                case 1: // 上書き
                    savefilename += ".txt";
                    File.WriteAllText(savefilename, $"Chat Log ***{dt:F}***" + Environment.NewLine);
                    File.AppendAllText(savefilename, Log.Text);
                    break;
                case 2: // 追記
                    savefilename += ".txt";
                    File.AppendAllText(savefilename, $"Chat Log ***{dt:F}***" + Environment.NewLine);
                    File.AppendAllText(savefilename, Log.Text);
                    break;
                case 3: // タイムスタンプ
                    savefilename += $"_{dt:yyyyMMdd}_{dt:HHmmss}.txt";
                    File.WriteAllText(savefilename, $"Chat Log ***{dt:F}***" + Environment.NewLine);
                    File.AppendAllText(savefilename, Log.Text);
                    break;
                default:
                    break;
            }
        }

        private void Cmb_User_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.__ChatSelUser = Cmb_User.SelectedIndex;
        }

        private void Cmb_Npc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.__ChatSelNpc = Cmb_Npc.SelectedIndex;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.CharaList.FileTextWrite();
            Properties.Settings.Default.Save();
            ChatLogSave(Txt_Chat);
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

        private void Btn_List_Click(object sender, EventArgs e)
        {
            WhiteList Frm_WhiteList = new WhiteList();
            Frm_WhiteList.Owner = this;
            if (Frm_WhiteList.ShowDialog() == DialogResult.OK)
            {
            }
        }

        // Redirect Window Hndle List
        private List<IntPtr> twlist = new List<IntPtr>();

        private bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
        {
            //throw new NotImplementedException();
            //ウィンドウのタイトルの長さを取得する
            var _wi = new WinApi.WINDOWINFO();
            _wi.cbSize = Marshal.SizeOf(_wi);
            WinApi.GetWindowInfo(hWnd, ref _wi);

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
            int textLen = WinApi.GetWindowTextLength(hWnd);
            if (0 < textLen)
            {
                //ウィンドウのタイトルを取得する
                StringBuilder tsb = new StringBuilder(textLen + 1);
                WinApi.GetWindowText(hWnd, tsb, tsb.Capacity);

                string ma = $"{tsb}";
                if (!ma.Equals("マビノギ"))
                {
                    var item = BTN_SendTask.DropDownItems.Add(ma);
                    item.Tag = hWnd;
                }
            }
            return true;
        }

        private void BTN_SendTask_DropDownOpening(object sender, EventArgs e)
        {
            BTN_SendTask.DropDownItems.Clear();
            twlist.Clear();
            WinApi.EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);
        }

        private void BTN_SendTask_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Btn_Redirect.Tag = e.ClickedItem.Tag;
            BTN_SendTask.Text =  e.ClickedItem.Text;
            WinApi.SetForegroundWindow((IntPtr)Btn_Redirect.Tag);
        }

        private void Btn_Redirect_Click(object sender, EventArgs e)
        {
            if ( Btn_Redirect.Text == "OFF" )
            {
                Btn_Redirect.Text = "ON";
            }
            else
            {
                Btn_Redirect.Text = "OFF";
            }
        }

        private void Btn_DumpView_Click(object sender, EventArgs e)
        {
            if ( Btn_DumpView.Checked == false )
            {
                TxtChatWriteLine("Dump Log *** Start" + Environment.NewLine);
                Btn_DumpView.Checked = true;
                Program.packets.PacketMode = PacketModes.Dump;
            }
            else
            {
                TxtChatWriteLine("Dump Log *** End" + Environment.NewLine);
                Btn_DumpView.Checked = false;
                Program.packets.PacketMode = PacketModes.Chat;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            string _Pos_Main = $"{this.Location.X},{this.Location.Y},{this.Width},{this.Height}";
            Program.__Pos_Main = _Pos_Main;

        }

        public Help sf = null;
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                 /*
                  *  Q  W  E  R
                  *   A  S  D  F
                  *    Z  X  C  V  B  N 
                  */
                case Keys.Q: //Quit
                    this.Close();
                    break;
                case Keys.W:
                    break;
                case Keys.E:
                    break;
                case Keys.R: //Redirect Switch
                    Btn_Redirect_Click(sender, (EventArgs)null);
                    break;

                case Keys.A:
                    break;
                case Keys.S: //Setting
                    Btn_Setup_Click(sender, (EventArgs)null);
                    break;
                case Keys.D: //Dump
                    Btn_DumpView_Click(sender, (EventArgs)null);
                    break;
                case Keys.F:
                    break;

                case Keys.Z: //ID
                    Btn_Add_Click(sender, (EventArgs)null);
                    break;
                case Keys.X: //List
                    Btn_List_Click(sender, (EventArgs)null);
                    break;
                case Keys.C: //Clear
                    Btn_Clear_Click(sender, (EventArgs)null);
                    break;
                case Keys.V: //Choice
                    var nc = Cmb_Whitelist.SelectedIndex;
                    nc++;
                    Debug.Print($"{Cmb_Whitelist.Items.Count}:{nc}");
                    if (Cmb_Whitelist.Items.Count <= nc)
                    {
                        nc = 0;
                    }
                    Cmb_Whitelist.SelectedIndex = nc;
                    break;
                case Keys.B: //User
                    var nu = Cmb_User.SelectedIndex;
                    nu++;
                    if (Cmb_User.Items.Count <= nu)
                    {
                        nu = 0;
                    }
                    Cmb_User.SelectedIndex = nu;
                    break;
                case Keys.N: //NPC
                    var nn = Cmb_Npc.SelectedIndex;
                    nn++;
                    if (Cmb_Npc.Items.Count <= nn)
                    {
                        nn = 0;
                    }
                    Cmb_Npc.SelectedIndex = nn;
                    break;
                case Keys.F1: //Help
                    if (this.sf == null || this.sf.IsDisposed)
                    { /* ヌル、または破棄されていたら */
                        this.sf = new Help();
                        this.sf.Show();
                    }
                    this.sf.Activate();
                    break;
                default:
                    break;
            }

        }
    }
}
