using MabiChatSpeech;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace MabiChatSpeech
{
    /*
     
状態

	    実行	通信	使用Port
Off	    未実行	なし	なし
On　　	実行 	なし	なし
Online	実行	あり	11020以外	キャプチャ開始
InErin	実行	あり	11020

PIDからTCPテーブルをサーチ

状態変化
    client.exeのプロセス状態
    TCPテーブル監視
    11020のサーバーIP変化
    11020が閉じた

変化時にイベント生成


キャプチャによる変化



ターゲットの実行状況監視
Port使用状況監視
　パケットキャプチャの起動管理


チャット受信
SharpPcap からのパケット処理
　チャットであればイベント発行


*/

    public class MabiPacket 
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

        private struct st_adapter
        {
            public string Description;
            public string Name;
            public string ipv4Addr;
            public NetworkInterfaceType ifacetype;
        }


        private struct st_MabiPort
        {
            public bool uflag;
            public int State;
            public string LocalAddr;
            public int LocalPort;
            public string RemoteAddr;
            public int RemotePort;

        }

        public enum ConnectStatus { None,Offline,Online,Login,Erin };
        public string TargetProgram="client";
        public uint TargetPort = 11020 ;
        public uint CharaSelPort = 11000;
        public string ServerIP ="" ;
        public string LocalIP="" ;
        private List<st_MabiPort> MabiPortList = new List<st_MabiPort>();
        private CaptureDeviceList NetDevs = CaptureDeviceList.Instance;
        public static ILiveDevice capdev;
        private static byte[] tcpbuff = new byte[1024 * 1024 * 4];
        private static int tcpblen;
        private static int bpos = 0;
        private static int pushcnt = 0;
        public ConnectStatus ConnectState = ConnectStatus.None;

        public static System.Timers.Timer Wdt  ;

        public event EventHandler MabiPacketArrival;
        public event EventHandler MabiConnection;
        public event EventHandler MabiChat;



        // パケット関連
        /*
         * ターゲット監視
         * タイマーで PIDからPortをスキャン
         * 
         * パケット監視
         * sharpcapによるパケット監視
         */

        /// <summary>
        /// clientのPIDを返す
        /// </summary>
        /// <returns></returns>
        public int GetMabinogiPid()
        {
            System.Diagnostics.Process[] ps =
                System.Diagnostics.Process.GetProcessesByName(TargetProgram);
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
        private string ipstr(int addr)
        {
            var b = BitConverter.GetBytes(addr);
            return string.Format("{0}.{1}.{2}.{3}", b[0], b[1], b[2], b[3]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int htons(int i)
        {
            var tmp = (((0x000000ff & i) << 8) + ((0x0000ff00 & i) >> 8))
                       + ((0x00ff0000 & i) << 8) + ((0xff000000 & i) >> 8);
            return (int)tmp;
        }


        /// <summary>
        /// マビノギが開いているポートをスキャンする
        /// </summary>
        /// <returns>　-1 マビノギ未起動 / 開いているポート数 </returns>
        public int ScanMabiPort()
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
        public int MabiPortStatus(uint PortNo)
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

                    if (o.OwningPid == mabipid)
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
        public int IsOpenPort(uint port)
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
        private st_MabiPort PortInfo(uint port)
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
        public string ClientIpAddress()
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
        private List<st_adapter> GetLocalIPAddress()
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
        private int findNic(string laddr)
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

        public bool cap_init()
        {
            // パケットキャプチャに関する初期設定
            // 1.監視対象の設定()
            Wdt = new System.Timers.Timer() ;
            Wdt.Interval = 1000;
            Wdt.Elapsed += Wdt_Tick;

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
        public static List<ChatData> chatDatas = new List<ChatData>();

        public static void chatdatas_add(ChatData data)
        {
            ChatData item = chatDatas.Find(x => x.CharacterName == data.CharacterName && x.CharacterType == data.CharacterType);
            if (item.CharacterName == null)
            {

                chatDatas.Add(data);
            }
        }


        private  ChatData analyses_packet(int len)
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
                        ret.CharacterType = (MabiChat.CharacterTypes)tcpbuff[ptn_CNT + 10];
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
                ret.CharacterName = s1.Replace("\0", "");
                string s2 = System.Text.Encoding.UTF8.GetString(da2);
                ret.ChatWord = s2.Replace("\0", "");
            }

            return ret;
        }


        private  void device_OnPacketArrival(object sender, PacketCapture e)
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
                int psts = MabiPortStatus( TargetPort );

                var sip = $"{srcIp}";
                if (ServerIP != sip)
                {
                    ServerIP = sip;
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
                if ((chats.ChatWord != "") && (chats.CharacterName != ""))
                {
                    chatdatas_add(chats);

                    // 設定に関係なくキャラ名採取
                    CharacterNameData n = CharaList.CNlist.Find(x => x.CharName == chats.CharacterName);
                    // 該当なし
                    if (n == null)
                    {
                        if ((chats.CharacterType == CharacterTypes.User) && (Program.__WhiteList_AutoAdd == true))
                        {
                            CharacterNameData item = new CharacterNameData(chats.CharacterName, true, chats.CharacterType,
                                                        __TTS1Name, __TTS1Volume, __TTS1Speed);
                            Program.CharaList.CNlist.Add(item);
                            n = item;
                        }
                    }

                    /*
                     * 設定状況　
                     * WhiteListならキャラ名でマッチング
                     * 
                     */
                    bool dip = false;
                    int cv = 0;
                    int cs = 0;
                    string cn = "";


                    if (__ChatSelWhitelist != 0)
                    {
                        if (n != null)
                        {
                            if (n.Enabled == true)
                            {
                                dip = true;
                            }

                            if (__ChatSelWhitelist == 2)
                            {
                                cn = n.TtsName;
                                cs = n.TtsSpeed;
                                cv = n.TtsVolume;

                            }
                        }
                    }
                    else if (((__ChatSelUser == 1) && (chats.CharacterType == CharacterTypes.User)) ||
                         ((__ChatSelPet == 1) && (chats.CharacterType == CharacterTypes.Pet)) ||
                         ((__ChatSelNpc == 1) && (chats.CharacterType == CharacterTypes.Npc)))
                    {
                        dip = true;
                    }
                    else if (((__ChatSelUser == 2) && (chats.CharacterType == CharacterTypes.User)) ||
                         ((__ChatSelPet == 2) && (chats.CharacterType == CharacterTypes.Pet)) ||
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

                    if ((dip == true) && (chats.ChatWord != ""))
                    {
                        var tm = DateTime.Now;

                        if (cn != "")
                        {
                            Frm_Main.speech_chat(cn, cv, cs, chats.CharacterName, chats.ChatWord);
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


        private  void device_OnCaptureStopped(object sender, CaptureStoppedEventStatus status)
        {
            Debug.Print("Event!! Stop Capture");
            ServerIP = "";

            Wdt.Start();
        }

        private void capdev_start()
        {
            capdev.Open(DeviceModes.Promiscuous, 1000);
            capdev.OnPacketArrival += device_OnPacketArrival;
            capdev.OnCaptureStopped += device_OnCaptureStopped;
            capdev.Filter = $"tcp src port {TargetPort} and dst host {LocalIP}";// and dst port {lp}";
            capdev.StartCapture();
        }
        private  void capdev_stop()
        {
            capdev.StopCapture();
            ServerIP = "";
            Wdt.Start();
        }

        public  void Wdt_chkstat()
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


            if (IsOpenPort(TargetPort) != -1)
            {
                WdtStatus = ClinetStatus.online;
            }
            else if (IsOpenPort(CharaSelPort) != -1)
            {
                WdtStatus = ClinetStatus.charsel;
                Program.ServerIP = "";
            }
        }



        private void Wdt_Tick(object sender, EventArgs e)
        {
            Wdt_chkstat();
            Debug.Print($"Wdts {ConnectState}");
        }



    }
}
