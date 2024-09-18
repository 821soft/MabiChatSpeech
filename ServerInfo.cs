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

namespace MabiChatSpeech
{
    public partial class ServerInfo : Form
    {

        public ServerInfo()
        {
            InitializeComponent();

            Lsv_Server.Items.Clear();
            //サーバー情報をListViewItemに設定
            string path = @"mabiserverlist.txt";

            // ファイル読み込み＆文字化け防止
            var lines = File.ReadAllLines(path, Encoding.GetEncoding("utf-8"));

            // 1行ずつ読み込んで表示
            int cnt = 1;
            foreach (var line in lines)
            {
                string []coldata = line.Split(',');
                if( coldata.Length>1 )
                {
                    var item = Lsv_Server.Items.Add($"{cnt}");
                    item.SubItems.Add(coldata[0]);
                    item.SubItems.Add(coldata[1]);
                    cnt++;
                }
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
            /*
                        var sts = Program.ScanMabiPort();
                        if (sts == -1)
                        {
                            Txt_ServerInfo.Text = "Client Off";
                            foreach (ListViewItem svrec in Lsv_Server.Items)
                            {
                                if (svrec.BackColor != Color.White)
                                {
                                    svrec.BackColor = Color.White;
                                }
                            }
                        }
                        else
                        {
                            Txt_ServerInfo.Text = "Client Open Port List" + Environment.NewLine ;
                            foreach (var item in Program.MabiPortList)
                            {
                                Txt_ServerInfo.Text += $"{item.RemotePort} {item.RemoteAddr}" + Environment.NewLine;
                                if ( item.RemotePort == 11020 ) 
                                { 
                                    foreach(ListViewItem svrec in Lsv_Server.Items)
                                    {
                                        if ( svrec.SubItems[2].Text == item.RemoteAddr ) 
                                        {
                                            svrec.BackColor = Color.Yellow;
                                        }
                                        else if (svrec.BackColor != Color.White)
                                        {
                                            svrec.BackColor = Color.White;
                                        }
                                    }
                                }
                            }
                        }
            */
        }
    }
}
