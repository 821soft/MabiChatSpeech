using MabiChatSpeech;
using SharpPcap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using VoicevoxClientSharp.ApiClient;
using static MabiChatSpeech.MabiChat;
using static MabiChatSpeech.WinApi;

namespace MabiChatSpeech
{
    internal static class Program
    {


        public static List<CharacterNameData> CharaList { get; set; } = new();
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


        public static int __Echa
        {
            get { return Properties.Settings.Default.__Echa; }
            set { Properties.Settings.Default.__Echa = value; }
        }

        public static string __VoiceVox
        {
            get { return Properties.Settings.Default.__VoiceVox; }
            set { Properties.Settings.Default.__VoiceVox = value; }
        }
        public static string __AivisSpeech
        {
            get { return Properties.Settings.Default.__AivisSpeech; }
            set { Properties.Settings.Default.__AivisSpeech = value; }
        }
        public static int __ChatMode_PC
        {
            get { return Properties.Settings.Default.__ChatMode_PC; }
            set { Properties.Settings.Default.__ChatMode_PC = value; }
        }
        public static int __ChatMode_NPC
        {
            get { return Properties.Settings.Default.__ChatMode_NPC; }
            set { Properties.Settings.Default.__ChatMode_NPC = value; }
        }
        public static int __ChatMode_Voice
        {
            get { return Properties.Settings.Default.__ChatMode_Voice; }
            set { Properties.Settings.Default.__ChatMode_Voice = value; }
        }
        public enum ChatMode_PC { DisableChat , DisableTTS , EnableChat , EnableTTS }
        public enum ChatMode_NPC { DisableChat , DisableTTS , EnableChat , EnableTTS }
        public enum ChatMode_Voice { DisableChat , DisableTTS , DisableVoice , EnableChat , EnableTTS , EnableVoice }
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
        public static void tmpfile_write(string [] li)
        {
            File.AppendAllLines(_tmpfname, li);
        }



        static public CharacterNameData? CharaList_find(string name,string ctypeStr)
        {
            var ctype = (_CharaTypeNames)Enum.Parse(typeof(_CharaTypeNames), ctypeStr, true);

            CharacterNameData? character = Program.CharaList.FirstOrDefault(x => ((x.CharName == name)) && (x.CharaTypeName == ctype));
            
            return (character);
        }
        static public CharacterNameData? CharaList_find(string name, _CharaTypeNames charaTypeName)
        {
            CharacterNameData? character = Program.CharaList.FirstOrDefault(x => ((x.CharName == name)) && (x.CharaTypeName == charaTypeName));

            return (character);
        }
        static public bool CharaList_usedEngien(ChatVoiceEngine eng)
        {
            CharacterNameData? character = Program.CharaList.FirstOrDefault(x => x.VoiceEngen == eng);
            if (character != null)
            {
                return (true);
            }

            return (false);
        }
        public enum RunningStatus { Running ,NotRunning, NotIstalled } ;

        public class _RunningStatusEngine 
        {
            public RunningStatus VoiceVox;
            public RunningStatus AivisSpeech;
            public _RunningStatusEngine()
            {
                VoiceVox = IsRunning(__VoiceVox);
                AivisSpeech = IsRunning(__AivisSpeech);
            }


        }
        public static Image GetEngeinIconImage(string foo ) 
        {
            // Voiceの起動チェック
            Image iconImage = null;
            // 1.設定がある場合のみ
            if (string.Equals(foo, __VoiceVox))
            {
                switch (IsRunning(__VoiceVox))
                {
                    case RunningStatus.Running:
                        iconImage = Properties.Resources.vvpp_0_1;
                        break;
                    case RunningStatus.NotRunning:
                        iconImage = Properties.Resources.vvpp_0_1_disable;
                        break;
                    case RunningStatus.NotIstalled:
                        iconImage = Properties.Resources.vvpp_0_1_error;
                        break;
                    default:
                        break;

                }
            }
            else if(string.Equals(foo, __AivisSpeech ))
                {

                var ststusAivisSpeech = IsRunning(__AivisSpeech);
                switch (IsRunning(__AivisSpeech))
                {
                    case RunningStatus.Running:
                        iconImage = Properties.Resources.aisp_0_1;
                        break;
                    case RunningStatus.NotRunning:
                        iconImage = Properties.Resources.aisp_0_1_disable;
                        break;
                    case RunningStatus.NotIstalled:
                        iconImage = Properties.Resources.aisp_0_1_error;
                        break;
                    default:
                        break;
                            
                }
            }
            return iconImage;
        }
        static public RunningStatus IsRunning(string foo)
        {


            if (foo.Trim() == "")
            {
//                Debug.Print($"IsRunning{foo}->RunningStatus.NotIstalled");
                return RunningStatus.NotIstalled;
            }

            var foo_procname = Path.GetFileNameWithoutExtension(foo);
            Process[] foo_processes = Process.GetProcessesByName(foo_procname);

            if (foo_processes.Length == 0)
            {
//                Debug.Print($"IsRunning{foo}->RunningStatus.NotRunning");
                return RunningStatus.NotRunning;
            }
            else
            {
                foreach (var proc in foo_processes)
                {
                    string path = proc.MainModule?.FileName;
                    var comp =  string.Equals(foo, path, StringComparison.OrdinalIgnoreCase);
//                    Debug.Print($"IsRunning comp f({foo})");
//                    Debug.Print($"IsRunning comp p({path}");
//                    Debug.Print($"判定:{comp}");

                    if (comp)
                    {
//                        Debug.Print($"IsRunning{foo}->({proc.Id})RunningStatus.Running");
                        return RunningStatus.Running;
                    }
                    else
                    {
//                        Debug.Print($"IsRunning{foo}->({proc.Id})RunningStatus.NotRunning");

                        continue;
                    }
                }
//                Debug.Print($"IsRunning{foo}->RunningStatus.NotRunning");
                return RunningStatus.NotRunning;
            }

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
                // 出力デバイスリスト
                // 利用可能な再生デバイスを列挙
                /*
                Audio_Enum = new MMDeviceEnumerator();

                Audio_Devices = Audio_Enum.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

                Debug.Print("audiodev----");
                foreach (var devno in Audio_Devices)
                {
                        Debug.Print($"{devno.ID} {devno.State}");
                }
                Debug.Print("audiodev-End");

                Audio_Device = null;
                */

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
            _RunningStatusEngine rse = new _RunningStatusEngine();

            if (rse.VoiceVox == RunningStatus.NotRunning)
            {
                var mret = MessageBox.Show($"VoiceVoxが起動していません。起動してみる？\n{__VoiceVox}",
                                    "エラー",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question);
                if (mret == DialogResult.Yes)
                {
                    Process.Start(__VoiceVox/*, "--load_all_models"*/);
                    Thread.Sleep(1000);

                }
            }
            if (rse.AivisSpeech == RunningStatus.NotRunning)
            {
                var mret = MessageBox.Show($"AivisSpeechが起動していません。起動してみる？\n{__AivisSpeech}",
                                    "エラー",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question);
                if (mret == DialogResult.Yes)
                {
                    Process.Start(__AivisSpeech/* , "--load_all_models"*/);
                    Thread.Sleep(1000);

                }
            }

            //FontItemList = new Collection();
            FontFamily[] ffs = FontFamily.Families;
            foreach (FontFamily ff in ffs)
            {
                Font f = new Font(ff,11,FontStyle.Regular);
                //Debug.Print($"{f.Name} : {f.GdiCharSet}");
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
                Debug.Print($"tmpname {_tmpfname}"); 
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
            MessageBox.Show($"エラー終了.\n {e.ToString()}",
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            Application.Exit();
        }

        public static void EngeinBoot(string execEngeinPath)
        {
            string enginePath = execEngeinPath ; // 実際の run.exe のパスを指定
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = enginePath,
                // 必要に応じて引数を指定（例: --host 127.0.0.1 --port 50021 など）
                UseShellExecute = true,
                CreateNoWindow = false // コンソールウィンドウを表示したくない場合は true
            };

            Process engineProcess = Process.Start(psi);
        }

    }
}
