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
using static MabiChatSpeech.Program.Whitelist;
using System.Data.SqlTypes;
using System.Speech.AudioFormat;
using static MabiChatSpeech.Program.CharacterNameList;

namespace MabiChatSpeech
{
    internal static class Program
    {
        [DllImport("iphlpapi.dll")]
        extern static int GetExtendedTcpTable(IntPtr pTcpTable, ref int pdwSize,
            bool bOrder, uint ulAf, TCP_TABLE_CLASS TableClass, int Reserved);

        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPROW_OWNER_PID
        {
            public int State;
            public int LocalAddr;
            public int LocalPort;
            public int RemoteAddr;
            public int RemotePort;
            public int OwningPid;
        }

        enum TCP_TABLE_CLASS
        {
            TCP_TABLE_BASIC_LISTENER,
            TCP_TABLE_BASIC_CONNECTIONS,
            TCP_TABLE_BASIC_ALL,
            TCP_TABLE_OWNER_PID_LISTENER,
            TCP_TABLE_OWNER_PID_CONNECTIONS,
            TCP_TABLE_OWNER_PID_ALL,
            TCP_TABLE_OWNER_MODULE_LISTENER,
            TCP_TABLE_OWNER_MODULE_CONNECTIONS,
            TCP_TABLE_OWNER_MODULE_ALL
        };

        public static string[] StateStrings =
        {
            "", "CLOSED","LISTENING","SYN_SENT","SYN_RCVD","ESTABLISHED","FIN_WAIT1",
            "FIN_WAIT2","CLOSE_WAIT","CLOSING","LAST_ACK","TIME_WAIT","DELETE_TCB"
        };

        public struct st_adapter
        {
            public string Description;
            public string Name;
            public string ipv4Addr;
            public NetworkInterfaceType ifacetype;
        }


        public struct st_MabiPort
        {
            public bool uflag;
            public int State;
            public string LocalAddr;
            public int LocalPort;
            public string RemoteAddr;
            public int RemotePort;

        }

        public struct st_MabiServer
        {
            public string name;
            public string ip;
        }

        public static List<st_MabiPort> MabiPortList = new List<st_MabiPort>();
        public static List<string> charlist  = new List<string>();

        public static List<st_MabiServer> chlist = new List<st_MabiServer>();

        // TTS 
        public class tts
        {

        }

        // Character Name List Class
        public enum CharacterTypes : byte { User = 0x00, Pet = 0x01, Npc = 0xf0 }
        public class CharacterNameList
        {
            readonly public string filename = "\\CharacterNameList.txt";
            public struct listrec
            {
                public string CharName;
                public bool Enabled;
                public CharacterTypes CharacterType;
                public string TtsName;
                public int TtsVolume;
                public int TtsSpeed;

                public listrec(string CharName ,bool Enabled , CharacterTypes CharacterType, string TtsName, int TtsVolume, int TtsSpeed)
                {
                    this.CharName = CharName;
                    this.Enabled = Enabled;
                    this.CharacterType = CharacterType;
                    this.TtsName = TtsName;
                    this.TtsVolume = TtsVolume;
                    this.TtsSpeed = TtsSpeed;
                }

                public ListViewItem ToItem()
                {
                    var retvalue = new ListViewItem();
                    retvalue.Text = this.CharName;
                    retvalue.Checked = this.Enabled;
                    retvalue.SubItems.Add($"{this.CharacterType}");
                    retvalue.SubItems.Add(this.TtsName);
                    retvalue.SubItems.Add($"{this.TtsVolume}");
                    retvalue.SubItems.Add($"{this.TtsSpeed}");
                    return (retvalue);
                }
            }
            public CharacterNameList()
            {
                FileTextRead();
            }
            public List<listrec> CNlist = new List<listrec>();

            public listrec line2listrec(string l)
            {
                var listrec = new listrec();
                /*
                 * TextFile Format
                 *  UserName,Enabled,CharacterType,TTSName,TTS_Volueme,TTS_Speed
                 *  <string>,<bool>,<string>,<string>,<Num>,<Num>
                 *  abv,User,Microsoft Hanhan Desktop,80,3
                 * 
                 */
                string[] lsp = l.Split(',');

                Debug.Print($"{l} lsp.Length:{lsp.Length}");
                if (lsp.Length >= 6)
                {
                    Debug.Print($"lsp:{lsp[0]} {lsp[1]} {lsp[2]} {lsp[3]} {lsp[4]}");
                    listrec.CharName = lsp[0];
                    listrec.Enabled = bool.Parse(lsp[1]);
                    switch ( lsp[2] )
                    {
                        case "User":
                            listrec.CharacterType = CharacterTypes.User;
                            break;
                        case "Pet":
                            listrec.CharacterType = CharacterTypes.Pet;
                            break;
                        case "Npc":
                            listrec.CharacterType = CharacterTypes.Npc;
                            break;
                        default:
                            break;
                    }
                    listrec.TtsName = lsp[3];
                    listrec.TtsVolume = int.Parse(lsp[4]);
                    listrec.TtsSpeed = int.Parse(lsp[5]);
                    return listrec;
                }
                return (listrec);
            }

            public void FileTextRead()
            {

                string fname = __SavePath + filename;

                CNlist.Clear();

                if ( File.Exists(@fname) == false)
                {
                    return;
                }
                Debug.Print($"Load Characterlist: {@fname}");

                var lines = File.ReadAllLines(@fname, Encoding.GetEncoding("utf-8"));
                foreach (var line in lines)
                {
                    var listrec = line2listrec(line);
                    if ( listrec.CharName != null)
                    {
                        CNlist.Add(listrec);
                    }
                }
                CNlist.Sort((a, b) => a.CharName.CompareTo(b.CharName));

            }
            private string listrec2Text(listrec listrec)
            {
                string writeline = $"{listrec.CharName},{listrec.Enabled},{listrec.CharacterType},{listrec.TtsName},{listrec.TtsVolume},{listrec.TtsSpeed}";
                return(writeline);
            }
            public void FileTextWrite()
            {
                string fname = __SavePath + filename;
                Debug.Print($"Save Characterlist : {@fname}");

                Encoding enc = Encoding.GetEncoding("utf-8");
                using (StreamWriter writer = new StreamWriter(@fname, false, enc))
                {
                    foreach (var listrec in CNlist)
                    {
                        string writeline = listrec2Text(listrec);
                        writer.WriteLine(writeline);
                        Debug.Print(writeline);
                    }
                    writer.Close();
                }
            }
            public listrec? FindCharacterName(string CName )
            {
                return (CNlist.Find(x => x.CharName == CName));

            }
            public int FindIndexCharacterName(string CName)
            {
                return (CNlist.FindIndex(x => x.CharName == CName));
            }
            public ListViewItem listrec2item(int idx)
            {
                var itemdata = CNlist[idx];

                if (itemdata.CharName != null)
                {
                    var item = new ListViewItem();
                    item.Checked = itemdata.Enabled;
                    item.Text = itemdata.CharName;
                    item.SubItems.Add($"{itemdata.CharacterType}");
                    item.SubItems.Add($"{itemdata.TtsName}");
                    item.SubItems.Add($"{itemdata.TtsVolume}");
                    item.SubItems.Add($"{itemdata.TtsSpeed}");
                    return (item);
                }

                return (null);
            }
            public listrec item2listrec(ListViewItem item)
            {
                var itemdata = new listrec();
                itemdata.CharName =  item.Text;
                itemdata.Enabled = item.Checked;
                switch(item.SubItems[1].Text)
                {
                    case "User":
                        itemdata.CharacterType = CharacterTypes.User;
                        break;
                    case "Pet":
                        itemdata.CharacterType = CharacterTypes.Pet;
                        break;
                    case "Npc":
                        itemdata.CharacterType = CharacterTypes.Npc;
                        break;
                }
                itemdata.TtsName = item.SubItems[2].Text;
                itemdata.TtsVolume = int.Parse(item.SubItems[3].Text);
                itemdata.TtsSpeed = int.Parse(item.SubItems[4].Text);

                return (itemdata);
            }
            // Old Data
            public void FileTextOldRead()
            {

                string path = @"whitelist.txt";

                CNlist.Clear();

                var lines = File.ReadAllLines(path, Encoding.GetEncoding("utf-8"));

                foreach (var line in lines)
                {
                    var l = line.Trim();
                    string[] lsp = l.Split(',');

                    if ( lsp.Length>=5 )
                    {
                        var data = new listrec();
                        data.CharName = lsp[0];
                        if (lsp[1] == "1")
                        {
                            data.Enabled = true;
                        }
                        else
                        {
                            data.Enabled = false;
                        }

                        switch (lsp[2])
                        {
                            case "User":
                                data.CharacterType = CharacterTypes.User;
                                break;
                            case "Pet":
                                data.CharacterType = CharacterTypes.Pet;
                                break;
                            case "Npc":
                                data.CharacterType = CharacterTypes.Npc;
                                break;
                        }

                        data.TtsVolume = int.Parse(lsp[3]);
                        data.TtsSpeed = int.Parse(lsp[4]);
                        data.TtsName = "Microsoft Zira Desktop";
                        CNlist.Add(data);
                    }

                }
                CNlist.Sort((a, b) => a.CharName.CompareTo(b.CharName));
            }
        } //End of class CharacterNameList


        public static void ChanelList()
        {
            string path = @"mabiserverlist.txt";

            var lines = File.ReadAllLines(path, Encoding.GetEncoding("utf-8"));

            foreach (var line in lines)
            {
                string[] coldata = line.Split(',');
                if (coldata.Length > 1)
                {
                    var item = new st_MabiServer();
                    item.name = coldata[0];
                    item.ip = coldata[1];
                    chlist.Add(item);
                }
            }
        }
        public static string ChanelName(string ip)
        {
            foreach (var rec in chlist)
            {
                if (rec.ip == ip)
                {
                    return rec.name;
                }
            }
            return "";
        }


        public class Whitelist
        {
            string path = @"whitelist.txt";

            public struct rec
            {
                public string CharName;
                public bool Enabled;
                public string TtsNo;
                public int TtsVolume;
                public int TtsSpeed;
            }
            public List<rec> Wlist = new List<rec>();

            public Whitelist()
            {
                Read();
            }

            public string rec2line(rec r)
            {
                string ret_v = "";
                int f = 0;
                if (r.Enabled == true)
                {
                    f = 1;
                }
                ret_v = $"{r.CharName},{f},{r.TtsNo},{r.TtsVolume},{r.TtsSpeed}" + Environment.NewLine;

                return (ret_v);
            }

            public rec item2rec(string cn,bool ce , string ctn , int ctv , int cts )
            {
                var ret_rec = new rec();
                ret_rec.CharName = cn;
                ret_rec.Enabled  = ce;
                ret_rec.TtsNo = ctn;
                ret_rec.TtsVolume = ctv;
                ret_rec.TtsSpeed = cts;

                return (ret_rec ); 
            }
            public rec line2rec(string l)
            {
                var ret_rec = new rec();
                if ( l.StartsWith("'") )
                {
                    return(ret_rec);
                }
                string[] coldata = l.Split(',');
                if (coldata.Length > 4)
                {
                    ret_rec.CharName = coldata[0];
                    if (coldata[1] == "1")
                    {
                        ret_rec.Enabled = true;
                    }
                    else
                    {
                        ret_rec.Enabled = false;
                    }

                    ret_rec.TtsNo = coldata[2];
                    ret_rec.TtsVolume = int.Parse(coldata[3]);
                    ret_rec.TtsSpeed = int.Parse(coldata[4]);
                }


                return (ret_rec);
            }

            public void Read()
            {
                Wlist.Clear();
                // ファイル読み込み＆文字化け防止
                var lines = File.ReadAllLines(path, Encoding.GetEncoding("utf-8"));

                // 1行ずつ読み込んで表示
                foreach (var line in lines)
                {
                    if (line.StartsWith("'") == false)
                    {
                        var arec = line2rec(line);
                        if (arec.CharName != null)
                        {
                            Wlist.Add(arec);
                        }

                    }
                }



            }

            public void Write()
            {
                string wline="";
                foreach ( var arec in Wlist)
                {
                    if ( arec.CharName.Length !=0 )
                    {
                        wline += rec2line(arec);
                    }
                }

                Encoding enc = Encoding.GetEncoding("utf-8");
                using (StreamWriter writer = new StreamWriter(path, false, enc))
                {
                    writer.WriteLine(wline);
                    writer.Close();
                }

            }

            public rec FindCName ( string cn  )
            {
                rec frec = new rec();
                foreach (var arec in Wlist )
                {
                    if (arec.CharName == cn)
                    {
                        frec = arec;
                        return ( frec );
                    }

                }
                return ( frec ) ;
            }
        }


        public static Timer Wdt;
        private static string LocalIP="";
        public static string ServerIP = "";
        public static ClinetStatus WdtStatus;
        
        private static byte[] tcpbuff = new byte[1024 * 1024 * 4];
        private static int tcpblen;
        private static int bpos = 0;
        private static int pushcnt = 0;
        //private static int a_pushcnt = 0;



        /// <summary>
        /// clientのPIDを返す
        /// </summary>
        /// <returns></returns>
        public static int GetMabinogiPid()
        {
            System.Diagnostics.Process[] ps =
                System.Diagnostics.Process.GetProcessesByName("client");
            foreach (System.Diagnostics.Process p in ps)
            {
                return (p.Id);
            }
            return (0);
        }
        /// <summary>
        /// ipアドレスのコンバート
        /// </summary>
        /// <param name="addr"></param>
        /// <returns>string ipアドレスの文字列</returns>
        private static string ipstr(int addr)
        {
            var b = BitConverter.GetBytes(addr);
            return string.Format("{0}.{1}.{2}.{3}", b[0], b[1], b[2], b[3]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int htons(int i)
        {
            var tmp = (((0x000000ff & i) << 8) + ((0x0000ff00 & i) >> 8))
                       + ((0x00ff0000 & i) << 8) + ((0xff000000 & i) >> 8);
            return (int)tmp;
        }


        /// <summary>
        /// マビノギが開いているポートをスキャンする
        /// </summary>
        /// <returns>　-1 マビノギ未起動 / 開いているポート数 </returns>
        public static int ScanMabiPort()
        {

            MabiPortList.Clear();
            var mabipid = GetMabinogiPid();

            if (mabipid == 0)
            {
                return -1;
            }


            int size = 0;
            uint AF_INET = 2; // IPv4
            //必要サイズの取得            
            GetExtendedTcpTable(IntPtr.Zero, ref size, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
            var p = Marshal.AllocHGlobal(size);//メモリ割当て
            //TCPテーブルの取得            
            if (GetExtendedTcpTable(p, ref size, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0) == 0)
            {
                var num = Marshal.ReadInt32(p);//MIB_TCPTABLE_OWNER_PID.dwNumEntries(データ数)
                var ptr = IntPtr.Add(p, 4);
                for (int i = 0; i < num; i++)
                {
                    var o = (MIB_TCPROW_OWNER_PID)Marshal.PtrToStructure(ptr, typeof(MIB_TCPROW_OWNER_PID));

                    if (o.RemoteAddr == 0)
                    {
                        o.RemotePort = 0;//RemoteAddrが0の場合は、RemotePortも0にする
                    }

                    if (o.OwningPid == mabipid)
                    {
                        /* マビノギが開いているポート */

                        st_MabiPort arec = new st_MabiPort();
                        arec.uflag = true;
                        arec.State = o.State;
                        arec.LocalPort = htons(o.LocalPort);
                        arec.LocalAddr = ipstr(o.LocalAddr);
                        arec.RemotePort = htons(o.RemotePort);
                        arec.RemoteAddr = ipstr(o.RemoteAddr);

                        MabiPortList.Add(arec);
                    }

                    ptr = IntPtr.Add(ptr, Marshal.SizeOf(typeof(MIB_TCPROW_OWNER_PID)));//次のデータ
                }
                Marshal.FreeHGlobal(p);  //メモリ開放
            }
            return (MabiPortList.Count);
        }

        /// <summary>
        /// マビノギが開いているポートをスキャンする
        /// </summary>
        /// <returns>　-1 マビノギ未起動 / 開いているポート数 </returns>
        public static int MabiPortStatus( int PortNo )
        {
            var mabipid = GetMabinogiPid();

            if (mabipid == 0)
            {
                return -1;
            }


            int ret = -1;

            int size = 0;
            uint AF_INET = 2; // IPv4
            //必要サイズの取得            
            GetExtendedTcpTable(IntPtr.Zero, ref size, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
            var p = Marshal.AllocHGlobal(size);//メモリ割当て
            //TCPテーブルの取得            
            if (GetExtendedTcpTable(p, ref size, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0) == 0)
            {
                var num = Marshal.ReadInt32(p);//MIB_TCPTABLE_OWNER_PID.dwNumEntries(データ数)
                var ptr = IntPtr.Add(p, 4);
                for (int i = 0; i < num; i++)
                {
                    var o = (MIB_TCPROW_OWNER_PID)Marshal.PtrToStructure(ptr, typeof(MIB_TCPROW_OWNER_PID));

                    if (o.RemoteAddr == 0)
                    {
                        o.RemotePort = 0;//RemoteAddrが0の場合は、RemotePortも0にする
                    }

                    if (o.OwningPid == mabipid )
                    {
                        /* マビノギが開いているポート */

                        if (PortNo == htons(o.RemotePort))
                        {
                            ret = o.State;
                        }

                    }

                    ptr = IntPtr.Add(ptr, Marshal.SizeOf(typeof(MIB_TCPROW_OWNER_PID)));//次のデータ
                }
                Marshal.FreeHGlobal(p);  //メモリ開放
            }
            return (ret);
        }


        /// <summary>
        /// clientが開いているportを列挙する
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static int IsOpenPort(int port)
        {
            foreach (var item in MabiPortList)
            {
                if (item.RemotePort == port)
                {
                    return (item.State);
                }
            }
            return (-1);
        }



        /// <summary>
        /// clientが開いているportを列挙する
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static st_MabiPort PortInfo(int port)
        {
            st_MabiPort obj = new st_MabiPort();
            foreach (var item in MabiPortList)
            {
                if (item.RemotePort == port)
                {
                    return (item);
                }
            }
            return (obj);
        }

        /// <summary>
        /// clientが開いているportを列挙する
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string ClientIpAddress()
        {
            foreach (var item in MabiPortList)
            {
                if (item.LocalAddr.Length > 0)
                {
                    return (item.LocalAddr);
                }
            }
            return ("");
        }

        /// <summary>
        /// NICを列挙する
        /// </summary>
        /// <returns>List<st_adapter> NICリスト</returns>
        public static List<st_adapter> GetLocalIPAddress()
        {

            var adp = new List<st_adapter>();

            // 物理インターフェース情報をすべて取得
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // 各インターフェースごとの情報を調べる
            foreach (var adapter in interfaces)
            {
                // 有効なインターフェースのみを対象とする
                if (adapter.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }

                // インターフェースに設定されたIPアドレス情報を取得
                var properties = adapter.GetIPProperties();

                // 設定されているすべてのユニキャストアドレスについて
                foreach (var unicast in properties.UnicastAddresses)
                {
                    if (unicast.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        // IPv4アドレス
                        st_adapter x = new st_adapter();
                        x.Description = $"{adapter.Name}";
                        x.Name = $"{adapter.Id}";
                        x.ipv4Addr = $"{unicast.Address}";
                        x.ifacetype = adapter.NetworkInterfaceType; ;

                        adp.Add(x);

                    }
                    else if (unicast.Address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        // IPv6アドレス
                        //ipaddress.Add(unicast.Address);
                    }
                }
            }

            return adp;
        }

        /// <summary>
        /// IPAddress から対応するNIC番号を返す
        /// </summary>
        /// <param name="laddr"></param>
        /// <returns></returns>
        public static int findNic(string laddr)
        {
            var adp = GetLocalIPAddress();
            foreach (var ad in adp)
            {
                if (ad.ipv4Addr == laddr)
                {
                    int ret = 0;
                    foreach (var ac in NetDevs)
                    {
                        if (ac.Name.Contains(ad.Name))
                        {
                            return ret;
                        }
                        ret++;
                    }
                }
            }


            return (-1);

        }

        /*
         * プロセススキャン (clientの状態監視）
         * ポートスキャン   (ローカルアドレスを特定、接続状態監視)
         * 
         * スキャンは一定間隔で行う
         * 内容変更時はイベントで処理したい
         * 
         */

        
        public static bool cap_init()
        {
            // パケットキャプチャに関する初期設定
            // 1.監視対象の設定()
            Wdt = new Timer();
            Wdt.Interval = 1000;
            Wdt.Tick += Wdt_Tick;
            WdtStatus = ClinetStatus.off;
            Wdt.Start();
            return (true);
        }

        private static int push_packet(int pos, byte[] pay, int len)
        {
            // Debug.Print($"push pos {pos} / len {len}");
            Array.Copy(pay, 0, tcpbuff, pos, len);
            tcpblen = pos + len;
            return (pos + len);

        }

        public struct ChatData
        {
            public string CharacterName;
            public CharacterTypes CharacterType;
            public string ChatWord;
        }
        public static List <ChatData> chatDatas = new List <ChatData>();

        public static void chatdatas_add(ChatData data)
        {
            ChatData item = chatDatas.Find(x => x.CharacterName == data.CharacterName && x.CharacterType == data.CharacterType);
            if (item.CharacterName == null)
            {

                chatDatas.Add(data);
            }
        }


        private static ChatData analyses_packet(int len)
        {
            var ret = new ChatData();
            ret.CharacterName = "";
            ret.ChatWord = "";

            byte[] da1 = new byte[2048];
            byte[] da2 = new byte[2048];

            int st1_len = 0;
            int st2_len = 0;
            int st2_idx = 0;
            int st1_idx = 0;


            //*** データ部のサーチ 0x00 
            byte[] PTN_Start = new byte[8] { 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x52, 0x6c }; // 8byte 
            for (int ptn_CNT = 0; ptn_CNT < len - 24; ptn_CNT++)
            {
                //*** 先頭パターン
                //   0  1  2  3  4  5  6  7
                //  00 00 00 03 00 00 52 6C

                if ((tcpbuff[ptn_CNT] == PTN_Start[0]) && (tcpbuff[ptn_CNT + 1] == PTN_Start[1]) &&
                     (tcpbuff[ptn_CNT + 2] == PTN_Start[2]) && (tcpbuff[ptn_CNT + 3] == PTN_Start[3]) &&
                     (tcpbuff[ptn_CNT + 4] == PTN_Start[4]) && (tcpbuff[ptn_CNT + 5] == PTN_Start[5]) &&
                     (tcpbuff[ptn_CNT + 6] == PTN_Start[6]) && (tcpbuff[ptn_CNT + 7] == PTN_Start[7]))
                {
                    //*** 先頭パターン
                    //   8  9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24
                    //  00 10 XX 00 00 xx xx xx xx 03 00 01 00 06 00 NN
                    //  00 10 XX 00 00 xx xx xx xx xx 03 00 01 00 06 00 NN
                    //        00
                    //  
                    if ((tcpbuff[ptn_CNT + 8] == 0x00) &&
                        (tcpbuff[ptn_CNT + 9] == 0x10))/* &&
                        (tcpbuff[ptn_CNT + 10] == 0x00)) */
                    {
                        ret.CharacterType = (CharacterTypes)tcpbuff[ptn_CNT + 10];
                        //*** 先頭パターン
                        //   8  9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24
                        //  00 10 XX 00 00 xx xx xx xx 03 00 01 00 06 00 NN
                        //  00 10 XX 00 00 xx xx xx xx xx 03 00 01 00 06 00 NN

                        if ((tcpbuff[ptn_CNT + 17] == 0x03) && (tcpbuff[ptn_CNT + 18] == 0x00) &&
                            (tcpbuff[ptn_CNT + 19] == 0x01) && (tcpbuff[ptn_CNT + 20] == 0x00) &&
                            (tcpbuff[ptn_CNT + 21] == 0x06) && (tcpbuff[ptn_CNT + 22] == 0x00))
                        {
                            st1_len = tcpbuff[ptn_CNT + 23];
                            st1_idx = ptn_CNT + 23;
                            st2_idx = st1_idx + 1 + st1_len + 2;
                            st2_len = tcpbuff[st2_idx];
                            break;
                        }
                        else if ((tcpbuff[ptn_CNT + 18] == 0x03) && (tcpbuff[ptn_CNT + 19] == 0x00) &&
                                (tcpbuff[ptn_CNT + 20] == 0x01) && (tcpbuff[ptn_CNT + 21] == 0x00) &&
                                (tcpbuff[ptn_CNT + 22] == 0x06) && (tcpbuff[ptn_CNT + 23] == 0x00))
                        {
                            st1_len = tcpbuff[ptn_CNT + 24];
                            st1_idx = ptn_CNT + 24;
                            st2_idx = st1_idx + 1 + st1_len + 2;
                            st2_len = tcpbuff[st2_idx];
                            break;
                        }
                    }
                }
            }


            // チャットの出力
            if ((st1_len != 0) && (st2_len != 0))
            {

                Array.Copy(tcpbuff, st1_idx + 1, da1, 0, st1_len);
                Array.Copy(tcpbuff, st2_idx + 1, da2, 0, st2_len);

                string s1 = System.Text.Encoding.UTF8.GetString(da1);
                ret.CharacterName = s1.Replace("\0","");
                string s2 = System.Text.Encoding.UTF8.GetString(da2);
                ret.ChatWord = s2.Replace("\0", "");
            }

            return ret;
        }


        private static void device_OnPacketArrival(object sender, PacketCapture e)
        {
            var time = e.Header.Timeval.Date;
            var rawPacket = e.GetPacket();
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            var tcpPacket = packet.Extract<PacketDotNet.TcpPacket>();
            if (tcpPacket != null)
            {
                var ipPacket = (PacketDotNet.IPPacket)tcpPacket.ParentPacket;
                System.Net.IPAddress srcIp = ipPacket.SourceAddress;
                System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
                int srcPort = tcpPacket.SourcePort;
                int dstPort = tcpPacket.DestinationPort;
                int psts = MabiPortStatus(11020);

                var sip = $"{srcIp}";
                if ( ServerIP != sip )
                {
                    ServerIP = sip ;
                }

                if (tcpPacket.Push == false)
                {

                    bpos = push_packet(bpos, tcpPacket.PayloadData, tcpPacket.PayloadData.Length);
                    bpos = 0;
                    //dumptext(tcpPacket, ad, dhd, pushcnt);
                    if (tcpPacket.PayloadData.Length > 0)
                    {
                        pushcnt++;
                    }

                    return;
                }


                bpos = push_packet(bpos, tcpPacket.PayloadData, tcpPacket.PayloadData.Length);
                bpos = 0;

                //dumptext(tcpPacket, ad, dhd, pushcnt);
                pushcnt = 0;
                ChatData chats = analyses_packet(tcpblen);
                if ((chats.ChatWord !="" ) && (chats.CharacterName !="" ))
                {
                    chatdatas_add(chats);

                    // 設定に関係なくキャラ名採取
                    CharacterNameList.listrec n = CharaList.CNlist.Find(x => x.CharName == chats.CharacterName);
                    CharacterNameList.listrec item = new listrec();
                    // 該当なし
                    if ( n.CharName == null )
                    {
                        if ( ( chats.CharacterType == CharacterTypes.User ) && ( Program.__WhiteList_AutoAdd == true ))
                        {
                            // ユーザーなら追加
                            item.CharName = chats.CharacterName;
                            item.CharacterType = chats.CharacterType;
                            item.Enabled = true;
                            item.TtsName = __TTS1Name;
                            item.TtsVolume = __TTS1Volume;
                            item.TtsSpeed = __TTS1Speed;
                            Program.CharaList.CNlist.Add(item);
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        item = n;
                    }

                    /*
                     * 設定状況　
                     * WhiteListならキャラ名でマッチング
                     * 
                     */
                    bool dip = false;
                    int cv = 0;
                    int cs = 0;
                    string cn = "" ;


                    if ( __ChatSelWhitelist != 0 )
                    {
                        
                        if (item.Enabled==true )
                        {
                            dip = true;
                            cn = item.TtsName;
                            cs = item.TtsSpeed;
                            cv = item.TtsVolume;
                        }
                    }
                    else if ((( __ChatSelUser == 1 ) && (chats.CharacterType == CharacterTypes.User )) ||
                         (( __ChatSelPet == 1 ) && (chats.CharacterType == CharacterTypes.Pet )) ||
                         (( __ChatSelNpc == 1 ) && (chats.CharacterType == CharacterTypes.Npc )) )
                    {
                        dip = true;
                    }
                    else if ((__ChatSelWhitelist == 2 ) || (__ChatSelWhitelist == 3 ))
                    {
                        dip = true;
                    }
                    else if (((__ChatSelUser == 2) && (chats.CharacterType == CharacterTypes.User)) ||
                         ((__ChatSelPet == 2) && (chats.CharacterType == CharacterTypes.Pet )) ||
                         ((__ChatSelNpc == 2) && (chats.CharacterType == CharacterTypes.Npc)))
                    {
                        dip = true;
                        cn = Program.__TTS1Name;
                        cv = Program.__TTS1Volume;
                        cs = Program.__TTS1Speed;
                    }
                    else if (((__ChatSelUser == 3) && (chats.CharacterType == CharacterTypes.User)) ||
                         ((__ChatSelPet == 3) && (chats.CharacterType == CharacterTypes.Pet)) ||
                         ((__ChatSelNpc == 3) && (chats.CharacterType == CharacterTypes.Npc)))
                    {
                        dip = true;
                        cn = Program.__TTS2Name;
                        cv = Program.__TTS2Volume;
                        cs = Program.__TTS2Speed;
                    }


                    if ( (dip == true) && (chats.ChatWord != "") )
                    {

                        var tm = DateTime.Now;


                        if ( cn != "" )
                        {
                            Frm_Main.speech_chat( cn , cv,cs, chats.CharacterName, chats.ChatWord);
                            Frm_Main.TxtChatWriteLine($"{tm:HH:mm:ss.ff} < {chats.CharacterName} | {chats.ChatWord}" + Environment.NewLine);
                        }
                        else
                        {

                            Frm_Main.TxtChatWriteLine($"{tm:HH:mm:ss.ff} | {chats.CharacterName} | {chats.ChatWord}" + Environment.NewLine);
                        }
                    }
                }
            }
        }


        private static void device_OnCaptureStopped(object sender, CaptureStoppedEventStatus status)
        {
            Debug.Print("Event!! Stop Capture");
            ServerIP = "";

            Wdt.Start();
        }

        private static void capdev_start()
        {
            capdev.Open(DeviceModes.Promiscuous, 1000);
            capdev.OnPacketArrival += device_OnPacketArrival;
            capdev.OnCaptureStopped += device_OnCaptureStopped;
            capdev.Filter = $"tcp src port 11020 and dst host {LocalIP}";// and dst port {lp}";
            capdev.StartCapture();
        }
        private static void capdev_stop()
        {
            capdev.StopCapture();
            ServerIP = "";
            Wdt.Start();
        }

        public static void Wdt_chkstat()
        {

            /*
             * ProcIdスキャン off
             * Portスキャン 
             * 11020 なら Online
             * 11000 なら CharSel
             * 上記以外は ClientOn
             */

            if (ScanMabiPort() == -1)
            {
                WdtStatus = ClinetStatus.off;
                if (capdev != null)
                {
                    capdev_stop();
                    capdev.Close();
                    capdev.Dispose();
                }
                LocalIP = "";
                ServerIP = "";
                return;
            }
            WdtStatus = ClinetStatus.on;
            // ポートが開いていたらローカル側のIPアドレスを取得
            string local_ip = ClientIpAddress();
            if (local_ip == "")
            {
                return;
            }
            else if (local_ip != LocalIP)
            {
                LocalIP = local_ip;
                //Nicデバイスを特定
                int n = findNic(LocalIP);
                if (n == -1)
                {
                    // error
                    return;
                }
                // キャプチャ対象とする
                if (capdev != null)
                {
                    capdev.Dispose();
                }
                capdev = NetDevs[n];

                //キャプチャ開始 Port 11020でキャプチャ開始
                capdev_start();

            }

            if (capdev.Started == true)
            {
                Wdt.Stop();
            }


            if ( IsOpenPort(11020) != -1 )
            {
                WdtStatus = ClinetStatus.online;
            }
            else if ( IsOpenPort(11000) != -1 )
            {
                WdtStatus = ClinetStatus.charsel;
                Program.ServerIP = "";
            }
        }



        private static void Wdt_Tick(object sender, EventArgs e)
        {
            Wdt_chkstat();
            Debug.Print($"Wdts {WdtStatus}");
        }

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

        public enum ClinetStatus { off, on, charsel, online }
        public static CaptureDeviceList NetDevs = CaptureDeviceList.Instance;
        public static ILiveDevice capdev;

        //public static Whitelist wl = new Whitelist();
        public static CharacterNameList CharaList = new CharacterNameList();


        public static Main Frm_Main ;


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


            ChanelList();
            Frm_Main = new Main();
            cap_init();

            Application.Run(Frm_Main);
        }
    }
}
