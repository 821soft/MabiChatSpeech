using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketDotNet;

namespace MabiChatSpeech
{
    public partial class ServerInfo : Form
    {

        public ServerInfo()
        {
            InitializeComponent();

            Lsv_Server.Items.Clear();

            // 1行ずつ読み込んで表示
            var lines = Program.packets.SVList();
            int cnt = 1;
            foreach (var line in lines)
            {
                    var item = Lsv_Server.Items.Add($"{cnt}");
                    item.SubItems.Add(line.ip);
                    item.SubItems.Add(line.name);
                    cnt++;
            }


        }


        private void Btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ServerInfo_Shown(object sender, EventArgs e)
        {
            Tim_ServerInfo.Start();
        }

        private void ServerInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            Tim_ServerInfo.Stop();
        }

        private void Tim_ServerInfo_Tick(object sender, EventArgs e)
        {
            // タイミングでPortListが更新されるのでCopy
            List<st_MabiPort> Ports = new List<st_MabiPort>(Program.packets.PortList);
            Txt_ServerInfo.Text = "";
            foreach ( var pl in Ports)
            {
                Txt_ServerInfo.AppendText($"{pl.RemotePort}:{pl.RemoteAddr} -> {pl.LocalAddr}:{pl.LocalPort}" + Environment.NewLine);
            }

        }
    }
}
