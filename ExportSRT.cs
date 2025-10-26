using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MabiChatSpeech
{
    public partial class ExportSRT : Form
    {
        public struct st_SrtData
        {
            public int no;
            public TimeSpan basetime;
            public TimeSpan nexttime;
            public string word;
        }

        private List<st_SrtData> list_SRT = new List<st_SrtData>();
        private List<st_SrtData> list_SRT2 = new List<st_SrtData>();

        private DateTime tim_Start;
        private DateTime tim_End;

        public ExportSRT()
        {
            InitializeComponent();
        }

        private st_SrtData rec2SrtData(string rec)
        {
            st_SrtData item = new st_SrtData();
            string[] columns = rec.Split(',');
            string[] c_no = columns[0].Split('C');
            string[] c_time = columns[1].Split("."); // HH:mm:ss.fff
            string[] c_hms = c_time[0].Split(":");   // HH:mm:ss

            item.no = int.Parse(c_no[1]);
            item.basetime = new TimeSpan(
                int.Parse("0"),
                int.Parse(c_hms[0]),
                int.Parse(c_hms[1]),
                int.Parse(c_hms[2]),
                int.Parse(c_time[1])
                );

            item.word = columns[4];
            return (item);
        }
        private void LoadLog(string fname)
        {

            TimeSpan c_day = new TimeSpan(1, 0, 0, 0, 0);

            listBox1.Items.Clear();
            list_SRT.Clear();

            System.IO.StreamReader sr = new System.IO.StreamReader(fname, System.Text.Encoding.UTF8);
            //内容を一行ずつ読み込む
            while (sr.Peek() > -1)
            {
                var rec = sr.ReadLine();
                if (rec.StartsWith("C") == true)
                {
                    // ２４時またぎのチェック
                    string[] c_rec = rec.Split(',');
                    string c_time = c_rec[1];
                    TimeSpan t1 = TimeSpan.Parse(c_time);

                    listBox1.Items.Add(rec);
                    var item = rec2SrtData(rec);

                    if (list_SRT.Count > 1) // 最初でなければ
                    {
                        // １つ前の時間と比較
                        if (list_SRT[list_SRT.Count - 1].basetime > item.basetime)
                        {
                            // またぎ発生で加算
                            item.basetime = item.basetime.Add(c_day);
                        }
                    }
                    list_SRT.Add(item);
                    //Debug.Print($"t1 {t1.Days} {t1.Hours:00}:{t1.Minutes:00}:{t1.Seconds:00},{t1.Milliseconds:000}");
                    //Debug.Print($"R {item.basetime.Days} {item.basetime.Hours:00}:{item.basetime.Minutes:00}:{item.basetime.Seconds:00},{item.basetime.Milliseconds:000}");
                }
            }
            //閉じる
            sr.Close();
            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0;
            }


        }


        /// <summary>
        /// 連続接続
        /// </summary>

        private void list_SRT_calc_sirial( int pos , int pitch )
        {
            st_SrtData items = new st_SrtData();
            TimeSpan offset_time = new TimeSpan(0, (int)nm_Hour.Value, (int)nm_Minute.Value, (int)nm_Second.Value, (int)nm_Millisecond.Value);
            TimeSpan gap_time = new TimeSpan(0, 0, 0, 0, 1); // 1ms
            TimeSpan term_time = new TimeSpan(0, 0, 0, 3, 0); // 3s
            TimeSpan pitch_time = new TimeSpan(0, 0, 0, (int)(pitch/10), (int)(pitch % 10) * 100); 
            list_SRT2.Clear();
            //選択位置から
            for (int i = pos; i < (list_SRT.Count - 1); i++)
            {
                items = list_SRT[i];
                items.basetime = items.basetime - offset_time;
                if (pitch == 0)
                {
                    items.nexttime = list_SRT[i + 1].basetime - offset_time - gap_time;
                }
                else
                {
                    items.nexttime = items.basetime + pitch_time;

                }
                list_SRT2.Add(items);
            }
            items = list_SRT[list_SRT.Count - 1];
            items.basetime = items.basetime - offset_time;
            if (pitch == 0)
            {
                items.nexttime = items.basetime + term_time;
            }
            else
            {
                items.nexttime = items.basetime + pitch_time;
            }


            list_SRT2.Add(items);
        }

        /// <summary>
        /// 表示時間固定
        /// </summary>
        private void list_SRT_calc_fixtime()
        {

        }

        private bool SRTfileWrite(string fname)
        {

            System.IO.StreamWriter sw = new System.IO.StreamWriter(fname);

            int srt_id = 1;
            for (int i = 0; i < list_SRT2.Count; i++)
            {
                TimeSpan t1 = list_SRT2[i].basetime;
                TimeSpan t2 = list_SRT2[i].nexttime;
                string s = list_SRT2[i].word;

                sw.WriteLine($"{srt_id}");
                sw.WriteLine($"{t1.Days} {t1.Hours:00}:{t1.Minutes:00}:{t1.Seconds:00},{t1.Milliseconds:000} --> {t2.Days} {t2.Hours:00}:{t2.Minutes:00}:{t2.Seconds:00},{t2.Milliseconds:000}");
                sw.WriteLine($"{s}");
                sw.WriteLine("");

                srt_id++;
            }

            sw.Close();

            return (false);
        }

        private void ExportSRT_Load(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Program.__SavePath;
            var ret = openFileDialog1.ShowDialog(this);
            if (ret != DialogResult.OK)
            {
                this.Close();
                return;
            }
            LoadLog(openFileDialog1.FileName);
            lb_FileInfo.Text = $"ファイル情報：{openFileDialog1.FileName}";
        }

        private void BTN_Ok_Click(object sender, EventArgs e)
        {

            saveFileDialog1.InitialDirectory = Program.__SavePath;
            var ret = saveFileDialog1.ShowDialog(this);
            if (ret == DialogResult.OK)
            {
                if (listBox1.SelectedIndex == -1)
                {
                    listBox1.SelectedIndex = 0;
                }
                Debug.Print($"{saveFileDialog1.FileName}");
                list_SRT_calc_sirial(listBox1.SelectedIndex, trackBar1.Value);
                var fret = SRTfileWrite(saveFileDialog1.FileName);
            }

            Close();
        }

        private void BTN_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cur = list_SRT[listBox1.SelectedIndex];
            nm_No.Value = cur.no;
            nm_Hour.Value = cur.basetime.Hours;
            nm_Minute.Value = cur.basetime.Minutes;
            nm_Second.Value = cur.basetime.Seconds;
            nm_Millisecond.Value = cur.basetime.Milliseconds;

            trackBar1.Value = 0;
            textBox1.Text = "連続";

            lb_No.Text = $"{cur.no}";
            lb_SelectTime.Text = $"{cur.basetime.Days}/{cur.basetime.Hours:00}:{cur.basetime.Minutes:00}:{cur.basetime.Seconds:00}.{cur.basetime.Milliseconds:000}";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            var n = (float)trackBar1.Value * (float)0.1 ;
            if (n < (float)0.1)
            {
                textBox1.Text = "連続";
            }
            else
            {
                textBox1.Text = $"{n:0.0}";
            }
        }
    }
}
