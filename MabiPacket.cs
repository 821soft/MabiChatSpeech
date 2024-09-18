﻿using MabiChatSpeech;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using static MabiChatSpeech.Program;
using System.Collections;
using static MabiChatSpeech.MabiChat;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace MabiChatSpeech
{
    public enum ClinetStatus { off, on, charsel, online }
    public enum CharacterTypes : byte { User = 0x00, Pet = 0x01, Doll = 0x03 ,Npc = 0xf0 }

    // Event Interface
    public interface IMabiPacketObject
    {
        event EventHandler ConnectEvent;
        event EventHandler ChatEvent;
    }
    public class MabiPacketEventArgs : EventArgs
    {
        public string CharacterName;
        public CharacterTypes CharacterType;
        public string ChatWord;
    }

    // Mabinogi Packet Class
    public class MabiPacket
    {
        // Chat Data
        public struct ChatData
        {
            public string CharacterName;
            public CharacterTypes CharacterType;
            public string ChatWord;
        }

        //Nic
        public struct st_adapter
        {
            public string Description;
            public string Name;
            public string ipv4Addr;
            public NetworkInterfaceType ifacetype;
        }

        public static List<ChatData> chatDatas = new List<ChatData>();

        private System.Timers.Timer WDT = new System.Timers.Timer();
        public ClinetStatus csts;
        private int pid;
        private List<st_MabiPort> PortList = new List<st_MabiPort>();
        private string svip = "";
        public string svname = "";
        public bool cap_sts = false;
        private string localip = "";
        private static CaptureDeviceList NetDevs = CaptureDeviceList.Instance;
        private static ILiveDevice capdev;
        private static byte[] tcpbuff = new byte[1024 * 1024 * 4];
        private static int tcpblen;
        private static int bpos = 0;
        private static int pushcnt = 0;

        private static string GetNicName(string ip)
        {
            string nicname = "";

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
                        if ($"{unicast.Address}" == ip)
                        {
                            nicname = $"{adapter.Id}";
                        }
                    }
                    else if (unicast.Address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        // IPv6アドレス
                        //ipaddress.Add(unicast.Address);
                    }
                }
            }

            return nicname;
        }

        // タイマー監視
        private void onWDT(object sender, ElapsedEventArgs e)
        {
            ClinetStatus sts = ClinetStatus.off;
            string ip = "";
            string name = "";

            this.pid = GetMabinogiPid();
            if (this.pid != 0)
            {
                sts = ClinetStatus.on ;
                PortList.Clear();
                PortList = ScanMabiPort(this.pid);
                if ( PortList.Count > 0 ) 
                {
                    sts = ClinetStatus.charsel;
                    if ( localip =="" )
                    {

                        // ローカルIP確定 キャプチャデバイスを指定する
                        localip = PortList[0].LocalAddr;
                        var nic = GetNicName(localip);
                        capdev = null;
                        foreach (var dev in NetDevs)
                        {
                            Debug.Print($"{dev.Name}|{nic}");
                            if (dev.Name.Contains(nic))
                            {
                                capdev = dev;
                            }
                        }

                    }
                    // Caputure開始
                    if ( ( capdev != null) && ( capdev.Started == false) )
                    {
                        capdev_start();
                    }

                    var sv = PortList.Find(x => x.RemotePort == 11020);
                    if (sv.RemotePort == 11020)
                    {
                        // サーバーリストからマッチを探す
                        var csv = ServerList.Find(x => x.ip == sv.RemoteAddr);
                        if (csv.name.Length > 0)
                        {
                            sts = ClinetStatus.online;
                            ip = csv.ip;
                            name = csv.name;
                        }
                    }
                    else
                    {
                        sts = ClinetStatus.charsel;
                    }
                }
            }
            if ( ( sts != csts ) || (svip != ip))
            {
                svip = ip;
                svname = name;
                csts = sts;
                Connect();
            }
        }

        public event EventHandler ConnectEvent;
        void Connect()
        {
            OnConnect(new MabiPacketEventArgs());
        }
        protected virtual void OnConnect(MabiPacketEventArgs e)
        {
            ConnectEvent?.Invoke(this, e);
        }
        public event EventHandler ChatEvent;
        void Chat(ChatData d)
        {
            var e = new MabiPacketEventArgs();
            e.CharacterName = d.CharacterName;
            e.ChatWord = d.ChatWord;
            e.CharacterType = d.CharacterType;
            OnChat(e);
        }
        protected virtual void OnChat(MabiPacketEventArgs e)
        {
            ChatEvent?.Invoke(this, e);
        }

        public MabiPacket()
        {
            ChanelList();
            csts = ClinetStatus.off;
            WDT.Interval = 1000;
            WDT.Elapsed += onWDT;
            WDT.Start();
        }
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

        private static string ipstr(int addr)
        {
            var b = BitConverter.GetBytes(addr);
            return string.Format("{0}.{1}.{2}.{3}", b[0], b[1], b[2], b[3]);
        }

        private static int htons(int i)
        {
            var tmp = (((0x000000ff & i) << 8) + ((0x0000ff00 & i) >> 8))
                       + ((0x00ff0000 & i) << 8) + ((0xff000000 & i) >> 8);
            return (int)tmp;
        }


        private static int GetMabinogiPid()
        {
            System.Diagnostics.Process[] ps =
                System.Diagnostics.Process.GetProcessesByName("client");
            foreach (System.Diagnostics.Process p in ps)
            {
                return (p.Id);
            }
            return (0);
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
        private struct st_MabiServer
        {
            public string name;
            public string ip;
        }
        private static List<st_MabiServer> ServerList = new List<st_MabiServer>();


        private static List <st_MabiPort> ScanMabiPort(int pid)
        {
            var retval = new List <st_MabiPort>();

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

                    if (o.OwningPid == pid)
                    {
                        // マビノギが開いているポート 

                        st_MabiPort arec = new st_MabiPort();
                        arec.uflag = true;
                        arec.State = o.State;
                        arec.LocalPort = htons(o.LocalPort);
                        arec.LocalAddr = ipstr(o.LocalAddr);
                        arec.RemotePort = htons(o.RemotePort);
                        arec.RemoteAddr = ipstr(o.RemoteAddr);

                        retval.Add(arec);
                    }

                    ptr = IntPtr.Add(ptr, Marshal.SizeOf(typeof(MIB_TCPROW_OWNER_PID)));//次のデータ
                }
                Marshal.FreeHGlobal(p);  //メモリ開放
            }
            return (retval);
        }
        private static void ChanelList()
        {
            ServerList.Clear();

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
                    ServerList.Add(item);
                }
            }
        }
        private static int push_packet(int pos, byte[] pay, int len)
        {
            // Debug.Print($"push pos {pos} / len {len}");
            Array.Copy(pay, 0, tcpbuff, pos, len);
            tcpblen = pos + len;
            return (pos + len);

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
                        (tcpbuff[ptn_CNT + 9] == 0x10))// &&
                                                       // (tcpbuff[ptn_CNT + 10] == 0x00)) 
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
                ret.CharacterName = s1.Replace("\0", "");
                string s2 = System.Text.Encoding.UTF8.GetString(da2);
                ret.ChatWord = s2.Replace("\0", "");
            }

            return ret;
        }
        public static void chatdatas_add(ChatData data)
        {
            ChatData item = chatDatas.Find(x => x.CharacterName == data.CharacterName && x.CharacterType == data.CharacterType);
            if (item.CharacterName == null)
            {
                chatDatas.Add(data);
            }
        }


        private void device_OnPacketArrival(object sender, PacketCapture e)
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

                var sip = $"{srcIp}";
                if (svip != sip)
                {
                    // サーバーリストからマッチを探す
                    var csv = ServerList.Find(x => x.ip == sip);
                    if (csv.name.Length > 0)
                    {
                        svip = csv.ip;
                        svname = csv.name;
                    }

                    Connect();
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
                    //チャット受信でイベント
                    Chat(chats);
                }
            }
        }

        private void device_OnCaptureStopped(object sender, CaptureStoppedEventStatus status)
        {
            svip = "";
            svname = "";

            cap_sts = false;
        }

        private void capdev_start()
        {
            capdev.Open(DeviceModes.Promiscuous, 1000);
            capdev.OnPacketArrival += device_OnPacketArrival;
            capdev.OnCaptureStopped += device_OnCaptureStopped;
            capdev.Filter = $"tcp src port 11020 and dst host {localip}";// and dst port {lp}";
            cap_sts = true;
            capdev.StartCapture();
        }
        private void capdev_stop()
        {
            svip = "";
            svname = "";
            cap_sts = false;
        }

    }

}
