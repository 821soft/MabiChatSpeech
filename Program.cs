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
        public static void ChatLogSave(TextBox Log)
        {
            string savefilename = __SavePath + "\\mabichatlog";

            if ( Log.Text.Length == 0)
            {
                return;
            }
            DateTime dt = DateTime.Now;

            switch ( __SaveMode )
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
                    File.AppendAllText(savefilename, $"Chat Log ***{dt:F}***" +Environment.NewLine);
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

        public static bool __WhiteList_AutoAdd
        {
            get { return Properties.Settings.Default.__WhiteList_AutoAdd; }
            set { Properties.Settings.Default.__WhiteList_AutoAdd = value; }
        }


        public static CharacterNameList CharaList = new CharacterNameList();


        public static Main Frm_Main ;
        static public MabiPacket  packets = new MabiPacket();


        public static List<string> TTS_NameList = new List<string>();

        public static string TTS_Names()
        {
            /*
             * 404 台湾 , 409 米国 , 411 日本 , 412 韓国
             * 
             * 
             * 
             */


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

                    Console.WriteLine(" Name:          " + info.Name);
                    Console.WriteLine(" Culture:       " + info.Culture.Name);
                    TTS_NameList.Add($"[{info.Culture.Name}]{info.Name}");
                    retval_txt += info.Name +",";

                    if (info.SupportedAudioFormats.Count != 0)
                    {
                        Console.WriteLine(" Audio formats: " + AudioFormats);
                    }
                    else
                    {
                        Console.WriteLine(" No supported audio formats found");
                    }

                    string AdditionalInfo = "";
                    foreach (string key in info.AdditionalInfo.Keys)
                    {
                        AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                    }

                    Console.WriteLine(" Additional Info - " + AdditionalInfo);
                    Console.WriteLine();
                }
            }
            return (retval_txt.TrimEnd(','));
        }
        static void TTSInfo()
            {

                // Initialize a new instance of the SpeechSynthesizer.  
                using (SpeechSynthesizer synth = new SpeechSynthesizer())
                {

                    // Output information about all of the installed voices.   
                    Console.WriteLine("Installed voices -");
                    foreach (InstalledVoice voice in synth.GetInstalledVoices())
                    {
                        VoiceInfo info = voice.VoiceInfo;
                        string AudioFormats = "";
                        foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                        {
                            AudioFormats += String.Format("{0}\n",
                            fmt.EncodingFormat.ToString());
                        }

                        Console.WriteLine(" Name:          " + info.Name);
                        Console.WriteLine(" Culture:       " + info.Culture.DisplayName);
                        Console.WriteLine(" Age:           " + info.Age);
                        Console.WriteLine(" Gender:        " + info.Gender);
                        Console.WriteLine(" Description:   " + info.Description);
                        Console.WriteLine(" ID:            " + info.Id);
                        Console.WriteLine(" Enabled:       " + voice.Enabled);
                        if (info.SupportedAudioFormats.Count != 0)
                        {
                            Console.WriteLine(" Audio formats: " + AudioFormats);
                        }
                        else
                        {
                            Console.WriteLine(" No supported audio formats found");
                        }

                        string AdditionalInfo = "";
                        foreach (string key in info.AdditionalInfo.Keys)
                        {
                            AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                        }

                        Console.WriteLine(" Additional Info - " + AdditionalInfo);
                        Console.WriteLine();
                    }
                }
            }
 
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //speech_spk = new SpeechSynthesizer();
            //speech_spk.SetOutputToDefaultAudioDevice();
            //Debug.Print(TTS_Names());
            //TTSInfo();
            TTS_Names();
            if ( __TTS1Name == "" )
            {
                __TTS1Name = TTS_NameList[0];
            }
            if (__TTS2Name == "")
            {
                __TTS2Name = TTS_NameList[0];
            }


            //ChanelList();

            Frm_Main = new Main();
            //cap_init();

            Application.Run(Frm_Main);
        }
    }
}
