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
using System.Text.RegularExpressions;
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
        public static Color LC_DataNone    = Color.FromArgb(0xa9, 0xa9, 0xa9);
        public static Color LC_DataAdd     = Color.FromArgb(0xff, 0x00, 0x00);
        public static Color LC_DataDisable = Color.FromArgb(0x00, 0x00, 0x00);
        public static Color LC_DataTTS     = Color.FromArgb(0xff, 0x8c, 0x00);
        public static Color LC_DataVVE     = Color.FromArgb(0x32, 0xcd, 0x32);
        public static Color LC_DataASE     = Color.FromArgb(0x00, 0x00, 0xcd);
        public static Color LC_BG_PC       = Color.FromArgb(0xff, 0xff, 0xff);
        public static Color LC_BG_NPC      = Color.FromArgb(0xe3, 0xfa, 0xff);
        public static Color LC_BG_PET      = Color.FromArgb(0xed, 0xff, 0xe3);
        public static Color LC_BG_DOLL     = Color.FromArgb(0xff, 0xff, 0xc8);


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
        public static bool __ChatPet_Mute
        {
            get { return Properties.Settings.Default.__ChatPet_Mute; }
            set { Properties.Settings.Default.__ChatPet_Mute = value; }
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
        public static bool __CharaList_AutoAdd
        {
            get { return Properties.Settings.Default.__CharaList_AutoAdd; }
            set { Properties.Settings.Default.__CharaList_AutoAdd = value; }
        }
        public static string __Pos_Main
        {
            get { return Properties.Settings.Default.__Pos_Main; }
            set { Properties.Settings.Default.__Pos_Main = value; }
        }
        public static bool __Echa
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
        public static string __VoiceVox_Opt
        {
            get { return Properties.Settings.Default.__VoiceVox_Opt; }
            set { Properties.Settings.Default.__VoiceVox_Opt = value; }
        }
        public static string __AivisSpeech_Opt
        {
            get { return Properties.Settings.Default.__AivisSpeech_Opt; }
            set { Properties.Settings.Default.__AivisSpeech_Opt = value; }
        }
        public static string __VoiceVox_Host
        {
            get { return Properties.Settings.Default.__VoiceVox_Host; }
            set { Properties.Settings.Default.__VoiceVox_Host = value; }
        }
        public static string __AivisSpeech_Host
        {
            get { return Properties.Settings.Default.__AivisSpeech_Host; }
            set { Properties.Settings.Default.__AivisSpeech_Host = value; }
        }
        public static bool __VoiceVox_Auto
        {
            get { return Properties.Settings.Default.__VoiceVox_Auto; }
            set { Properties.Settings.Default.__VoiceVox_Auto = value; }
        }
        public static bool __Aivis_Auto
        {
            get { return Properties.Settings.Default.__Aivis_Auto; }
            set { Properties.Settings.Default.__Aivis_Auto = value; }
        }
        public static int __ChatMode
        {
            get { return Properties.Settings.Default.__ChatMode; }
            set { Properties.Settings.Default.__ChatMode = value; }
        }
        public static bool __ChatPC_Only
        {
            get { return Properties.Settings.Default.__ChatPC_Only; }
            set { Properties.Settings.Default.__ChatPC_Only = value; }
        }
        public static bool __ChatNPC_Mute
        {
            get { return Properties.Settings.Default.__ChatNPC_Mute; }
            set { Properties.Settings.Default.__ChatNPC_Mute = value; }
        }
        public static bool __ChatDoll_Mute
        {
            get { return Properties.Settings.Default.__ChatDoll_Mute; }
            set { Properties.Settings.Default.__ChatDoll_Mute = value; }
        }
        public static string __Chat_Regex
        {
            get { return Properties.Settings.Default.__Chat_Regex; }
            set { Properties.Settings.Default.__Chat_Regex = value; }
        }
        public static int __API_WDT
        {
            get { return Properties.Settings.Default.__API_WDT; }
            set { Properties.Settings.Default.__API_WDT = value; }
        }
        public enum ChatMode { Chat , TTS , Voice }
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
        public enum RunningStatus { Loading,Running ,NotRunning, NotIstalled } ;

        public sealed class EngineStatusChangedEventArgs : EventArgs
        {
            public ChatVoiceEngine Engine { get; }
            public RunningStatus PreviousStatus { get; }
            public RunningStatus CurrentStatus { get; }

            public EngineStatusChangedEventArgs(ChatVoiceEngine engine, RunningStatus previousStatus, RunningStatus currentStatus)
            {
                Engine = engine;
                PreviousStatus = previousStatus;
                CurrentStatus = currentStatus;
            }
        }

        public class _RunningStatusEngine 
        {
            public event EventHandler<EngineStatusChangedEventArgs>? StatusChanged;
            public RunningStatus VoiceVox;
            public RunningStatus AivisSpeech;
            public _RunningStatusEngine()
            {
                VoiceVox = IsRunning(__VoiceVox);
                AivisSpeech = IsRunning(__AivisSpeech);
            }
            public void Refresh()
            {
                UpdateStatus(ChatVoiceEngine.VoiceVox, IsRunning(__VoiceVox));
                UpdateStatus(ChatVoiceEngine.AivisSpeech, IsRunning(__AivisSpeech));
            }

            private void UpdateStatus(ChatVoiceEngine engine, RunningStatus currentStatus)
            {
                RunningStatus previousStatus = engine == ChatVoiceEngine.VoiceVox ? VoiceVox : AivisSpeech;
                if (engine == ChatVoiceEngine.VoiceVox)
                    VoiceVox = currentStatus;
                else
                    AivisSpeech = currentStatus;

                if (previousStatus != currentStatus)
                {
                    StatusChanged?.Invoke(this, new EngineStatusChangedEventArgs(engine, previousStatus, currentStatus));
                }
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
                        iconImage = Properties.Resources.Icon_vvpp;
                        break;
                    case RunningStatus.NotRunning:
                        iconImage = Properties.Resources.Icon_vvpp_Disable;
                        break;
                    case RunningStatus.NotIstalled:
                        iconImage = Properties.Resources.Icon_vvpp_Disable;
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
                        iconImage = Properties.Resources.Icon_aisp;
                        break;
                    case RunningStatus.NotRunning:
                        iconImage = Properties.Resources.Icon_aisp_Disable;
                        break;
                    case RunningStatus.NotIstalled:
                        iconImage = Properties.Resources.Icon_aisp_Disable;
                        break;
                    default:
                        break;
                            
                }
            }
            return iconImage;
        }
        static public bool IsEngienRun(ChatVoiceEngine eng)
        {
            bool result = Task.Run(async () =>
            {
                return await ChatVoice.IsChatVoiceEngenRunning(eng);
            }).GetAwaiter().GetResult();

            return (result);
        }

        static public RunningStatus IsRunning(string foo)
        {
            if (foo.Trim() == "")
            {
                return RunningStatus.NotIstalled;
            }

            var foo_procname = Path.GetFileNameWithoutExtension(foo);
            Process[] foo_processes = Process.GetProcessesByName(foo_procname);

            if (foo_processes.Length == 0)
            {
                return RunningStatus.NotRunning;
            }
            else
            {
                try
                {
                    foreach (var proc in foo_processes)
                    {
                        string path = proc.MainModule?.FileName;
                        var comp = string.Equals(foo, path, StringComparison.OrdinalIgnoreCase);
                        //                    Debug.Print($"プロセス実行判定({foo}):{comp}");

                        if (comp)
                        {
                            if (foo == __AivisSpeech)
                            {
                                if (IsEngienRun(ChatVoiceEngine.AivisSpeech) == false)
                                {
                                    return RunningStatus.Loading;
                                }
                            }
                            else if (foo == __VoiceVox)
                            {
                                if (IsEngienRun(ChatVoiceEngine.VoiceVox) == false)
                                {
                                    return RunningStatus.Loading;
                                }
                            }
                            return RunningStatus.Running;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                catch 
                {
                    return RunningStatus.NotRunning;
                }
                return RunningStatus.NotRunning;
            }
        }
        static public DialogResult StartEngien(ChatVoiceEngine eng)
        {
            DialogResult ret = DialogResult.None;
            string exec, opt;

            _RunningStatusEngine rse = new _RunningStatusEngine();
            RunningStatus rs;

            switch (eng)
            {
                case ChatVoiceEngine.VoiceVox:
                    exec = __VoiceVox;
                    opt = __VoiceVox_Opt;
                    rs = rse.VoiceVox;
                    break;
                case ChatVoiceEngine.AivisSpeech:
                    exec = __AivisSpeech;
                    opt = __AivisSpeech_Opt;
                    rs = rse.AivisSpeech;
                    break;
                default:
                    return(DialogResult.None);
            }
            if(rs == RunningStatus.NotRunning)
            {
                try
                {
                    if (opt.Length > 0)
                    {
                        Process.Start(exec, opt);
                    }
                    else
                    {
                        Process.Start(exec);
                    }
                }
                catch (Exception e)
                {
                    while (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    ret = MessageBox.Show($"エラー終了.\n {e.Message}",
                        "音声合成エンジン起動エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            return (ret);
        }
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            using var singleInstanceMutex = new System.Threading.Mutex(
                initiallyOwned: true,
                name: "Local\\821soft.MabiChatSpeech",
                createdNew: out bool isFirstInstance);
            if (!isFirstInstance)
            {
                return;
            }

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

            if (__VoiceVox_Auto )
            {
                var ret = StartEngien(ChatVoiceEngine.VoiceVox);
                if (ret != DialogResult.None)
                {
                    Application.Exit();
                }
            }

            if (__Aivis_Auto) 
            {
                var ret = StartEngien(ChatVoiceEngine.AivisSpeech);
                if (ret != DialogResult.None)
                {
                    Application.Exit();
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


    }
}
