using SharpPcap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Web;
using System.Collections;
using System.Data.SqlTypes;
using System.Speech.AudioFormat;
using System.Diagnostics.PerformanceData;
using MabiChatSpeech;
using static MabiChatSpeech.MabiChat;

namespace MabiChatSpeech
{
    internal static class Program
    {
        public static CharacterNameList CharaList = new CharacterNameList();
        public static Main Frm_Main;
        public static string _tmpfname =""; 
        static public MabiPacket packets = new MabiPacket();
        public static List<string> TTS_NameList = new List<string>();
        public static Color[] Color16List = {
                Color.Black, Color.Gray, Color.Silver, Color.White,
                Color.Maroon,Color.Red,Color.Purple,Color.Fuchsia,
                Color.Green,Color.Lime,Color.Olive,Color.Yellow,
                Color.Navy,Color.Blue,Color.Teal,Color.Aqua
            };
        public static List<Font> FontItemList = new List<Font>();
        /*
         * 設定データ
         */

        public static string __CapureProgram
        {
            get { return Properties.Settings.Default.__CapureProgram; }
            set { Properties.Settings.Default.__CapureProgram = value; }
        }

        public static uint __CapturePort
        {
            get { return Properties.Settings.Default.__CapturePort; }
            set { Properties.Settings.Default.__CapturePort = value; }
        }
        public static int __SaveMode
        {
            get { return Properties.Settings.Default.__SaveMode; }
            set { Properties.Settings.Default.__SaveMode = value; }
        }
        public static string __SavePath
        {
            get { return Properties.Settings.Default.__SavePath; }
            set { Properties.Settings.Default.__SavePath = value; }
        }
        public static string __ChatFontName
        {
            get { return Properties.Settings.Default.__ChatFontName; }
            set { Properties.Settings.Default.__ChatFontName = value; }
        }
        public static float __ChatFontSize
        {
            get { return Properties.Settings.Default.__ChatFontSize; }
            set { Properties.Settings.Default.__ChatFontSize = value; }
        }
        public static int __ChatFontStyle
        {
            get { return Properties.Settings.Default.__ChatFontStyle; }
            set { Properties.Settings.Default.__ChatFontStyle = value; }
        }
        public static bool __ChatFontBold
        {
            get { return Properties.Settings.Default.__ChatFontBold; }
            set { Properties.Settings.Default.__ChatFontBold = value; }
        }
        public static bool __ChatFontItalic
        {
            get { return Properties.Settings.Default.__ChatFontItalic; }
            set { Properties.Settings.Default.__ChatFontItalic = value; }
        }
        public static int __ChatFontCharSet
        {
            get { return Properties.Settings.Default.__ChatFontCharSet; }
            set { Properties.Settings.Default.__ChatFontCharSet = value; }
        }
        public static int __ChatForeColor
        {
            get { return Properties.Settings.Default.__ChatForeColor; }
            set { Properties.Settings.Default.__ChatForeColor = value; }
        }
        public static int __ChatBackColor
        {
            get { return Properties.Settings.Default.__ChatBackColor; }
            set { Properties.Settings.Default.__ChatBackColor = value; }
        }
        public static int __ChatFColor
        {
            get { return Properties.Settings.Default.__ChatFColor; }
            set { Properties.Settings.Default.__ChatFColor = value; }
        }
        public static int __ChatBColor
        {
            get { return Properties.Settings.Default.__ChatBColor; }
            set { Properties.Settings.Default.__ChatBColor = value; }
        }
        public static int __ChatSelWhitelist
        {
            get { return Properties.Settings.Default.__ChatSelWhitelist; }
            set { Properties.Settings.Default.__ChatSelWhitelist = value; }
        }
        public static int __ChatSelUser
        {
            get { return Properties.Settings.Default.__ChatSelUser; }
            set { Properties.Settings.Default.__ChatSelUser = value; }
        }
        public static int __ChatSelPet
        {
            get { return Properties.Settings.Default.__ChatSelPet; }
            set { Properties.Settings.Default.__ChatSelPet = value; }
        }
        public static int __ChatSelNpc
        {
            get { return Properties.Settings.Default.__ChatSelNpc; }
            set { Properties.Settings.Default.__ChatSelNpc = value; }
        }
        public static bool __ChatFaceEmo
        {
            get { return Properties.Settings.Default.__ChatFaceEmo; }
            set { Properties.Settings.Default.__ChatFaceEmo = value; }
        }
        public static string __TTS1Name
        {
            get { return Properties.Settings.Default.__TTS1Name; }
            set { Properties.Settings.Default.__TTS1Name = value; }
        }
        public static int __TTS1Volume
        {
            get { return Properties.Settings.Default.__TTS1Volume; }
            set { Properties.Settings.Default.__TTS1Volume = value; }
        }
        public static int __TTS1Speed
        {
            get { return Properties.Settings.Default.__TTS1Speed; }
            set { Properties.Settings.Default.__TTS1Speed = value; }
        }
        public static string __TTS2Name
        {
            get { return Properties.Settings.Default.__TTS2Name; }
            set { Properties.Settings.Default.__TTS2Name = value; }
        }
        public static int __TTS2Volume
        {
            get { return Properties.Settings.Default.__TTS2Volume; }
            set { Properties.Settings.Default.__TTS2Volume = value; }
        }
        public static int __TTS2Speed
        {
            get { return Properties.Settings.Default.__TTS2Speed; }
            set { Properties.Settings.Default.__TTS2Speed = value; }
        }
        public static bool __TTS_Mute
        {
            get { return Properties.Settings.Default.__TTS_Mute; }
            set { Properties.Settings.Default.__TTS_Mute = value; }
        }
        public static bool __TTS_NameCall
        {
            get { return Properties.Settings.Default.__TTS_NameCall; }
            set { Properties.Settings.Default.__TTS_NameCall = value; }
        }

        public static bool __ChatView_No
        {
            get { return Properties.Settings.Default.__ChatView_No; }
            set { Properties.Settings.Default.__ChatView_No = value; }
        }

        public static bool __ChatView_Time
        {
            get { return Properties.Settings.Default.__ChatView_Time; }
            set { Properties.Settings.Default.__ChatView_Time = value; }
        }

        public static bool __ChatView_Type
        {
            get { return Properties.Settings.Default.__ChatView_Type; }
            set { Properties.Settings.Default.__ChatView_Type = value; }
        }

        public static bool __ChatView_Name
        {
            get { return Properties.Settings.Default.__ChatView_Name; }
            set { Properties.Settings.Default.__ChatView_Name = value; }
        }

        public static bool __WhiteList_AutoAdd
        {
            get { return Properties.Settings.Default.__WhiteList_AutoAdd; }
            set { Properties.Settings.Default.__WhiteList_AutoAdd = value; }
        }

        public static string __Pos_Main
        {
            get { return Properties.Settings.Default.__Pos_Main; }
            set { Properties.Settings.Default.__Pos_Main = value; }
        }


        public static string TTS_Names()
        {
            string retval_txt ="";
            TTS_NameList.Clear();

            // Initialize a new instance of the SpeechSynthesizer.  
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                // Output information about all of the installed voices.   
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    VoiceInfo info = voice.VoiceInfo;
                    string AudioFormats = "";
                    foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                    {
                        AudioFormats += String.Format("{0}\n",
                        fmt.EncodingFormat.ToString());
                    }

                    TTS_NameList.Add($"[{info.Culture.Name}]{info.Name}");
                    retval_txt += info.Name +",";
                }
            }
            return (retval_txt.TrimEnd(','));
        }
        public static void tmpfile_write(string li)
        {
            File.AppendAllText(_tmpfname,li);
        }
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try 
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                TTS_Names();
            }
            catch (Exception e)
            {
                while(e.InnerException != null)
                {
                    e = e.InnerException;
                }
                MessageBox.Show($"起動できません.\n {e.Message}",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit();
            }
            if ( __TTS1Name == "" )
            {
                __TTS1Name = TTS_NameList[0];
            }
            if (__TTS2Name == "")
            {
                __TTS2Name = TTS_NameList[0];
            }

            //FontItemList = new Collection();
            FontFamily[] ffs = FontFamily.Families;
            foreach (FontFamily ff in ffs)
            {
                Font f = new Font(ff,11,FontStyle.Regular);
                Debug.Print($"{f.Name} : {f.GdiCharSet}");
                FontItemList.Add(f);
            }
            string [] _Pos_Main = __Pos_Main.Split(',');
            Point winpos = new Point();
            winpos.X = int.Parse( _Pos_Main[0]);
            winpos.Y = int.Parse(_Pos_Main[1]);
            Size winsize = new Size();
            winsize.Width = int.Parse(_Pos_Main[2]);
            winsize.Height = int.Parse(_Pos_Main[3]);

            try
            {
                _tmpfname = Path.GetTempFileName();
                Frm_Main = new Main();
                Frm_Main.Location = winpos;
                Frm_Main.Size = winsize;
                Application.Run(Frm_Main);
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }
                MessageBox.Show($"エラー終了.\n {e.Message}",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit();
            }

        }
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //どこにもキャッチされなかった例外があったときここが実行される
        }

    }
}
