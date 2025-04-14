using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MabiChatSpeech
{
    public partial class Overlay : Form
    {
        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);



        static System.Windows.Forms.Control owin;

        //クリックの透過
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020;
                return cp;
            }
        }

        public Overlay()
        {
            InitializeComponent();
            BackColor = Program.Color16List[Program.__ChatBColor];
            TransparencyKey = BackColor;

        }

        private void mwlist()
        {
            int l = 0;
            //全てのプロセスを列挙する
            foreach (System.Diagnostics.Process p in
                System.Diagnostics.Process.GetProcesses())
            {
                //メインウィンドウのタイトルがある時だけ列挙する
                if (p.MainWindowTitle.Length != 0)
                {
                    Debug.Print( $"プロセス名:{p.ProcessName}タイトル名:{p.MainWindowTitle}");
                }
                WinApi.WINDOWINFO _wi = new WinApi.WINDOWINFO();

                IntPtr m = WinApi._FindWindow(p.ProcessName, p.MainWindowTitle);
                if (m != IntPtr.Zero)
                {
                    WinApi.GetWindowInfo(m, ref _wi);
                }

            }
        }
        private void mwlist2()
        {

            IntPtr dw = WinApi.GetDesktopWindow();
            IntPtr w = WinApi.GetWindow(dw, WinApi.GW_HWNDFIRST);
            Debug.Print(System.Runtime.InteropServices.GetLastWin32Error());
            while (w != IntPtr.Zero )
            {
                w = WinApi.GetWindow(w, WinApi.GW_HWNDNEXT);
                int l = WinApi.GetWindowTextLength(w);
                if (l > 0)
                {
                    StringBuilder tsb = new StringBuilder(l + 1);
                    WinApi.GetWindowText(w, tsb, tsb.Capacity);
                    Debug.Print(tsb.ToString());

                }

            }

        }
        private void Overlay_Shown(object sender, EventArgs e)
        {

            timer1.Enabled = true;
            owin = this;
            WinApi.RECT rect = new WinApi.RECT();

            rect = WinApi.GetMabinogiRect();
            WinApi.SetMabinogiOverlay(this.Handle);
            mwlist2();
//            this.Text = $"{rect.left},{rect.top},{rect.right},{rect.bottom}";
            /*
                        this.Top = rect.top;
                        this.Left = rect.left;
                        this.Width = rect.right - rect.left;
                        this.Height =   rect.bottom - rect.top;
             */
        }

        private void Overlay_Paint(object sender, PaintEventArgs e)
        {
            /*
            IntPtr desktopDC = GetDC(IntPtr.Zero);
            using (Graphics g = Graphics.FromHdc(desktopDC))
            {
                g.FillEllipse(System.Drawing.Brushes.Red, 100, 100, 30, 30);
            }

            ReleaseDC(IntPtr.Zero, desktopDC);
            */

        }

        /*
         * FindWindow でターゲットのwhnd 
         * GetWindowInfo でターゲット情報
         * オーバーレイ窓のサイズ設定
         * 
         * 
         */
        private void timer1_Tick(object sender, EventArgs e)
        {
            //子のラベルの位置を更新
            foreach (Control co in Controls)
            {
                if (co.GetType().Equals(typeof(System.Windows.Forms.Label)))
                {
                    co.Location = new Point(co.Location.X-10, co.Location.Y);

                    if ( co.Location.X > this.Width)
                    {
                        co.Dispose();
                    }
                    if (co.Location.X + co.Width < 0)
                    {
                        co.Dispose();
                    }

                }
            }
        }
        static public void addlabel( string s )
        {
            System.Windows.Forms.Label lb = new System.Windows.Forms.Label();
            lb.AutoSize = true;
            lb.Text = s;
            lb.ForeColor = Program.Color16List[Program.__ChatFColor];
            lb.Font = new System.Drawing.Font("ＭＳ ゴシック", 18,
                System.Drawing.FontStyle.Bold , System.Drawing.GraphicsUnit.Point, 128);

            //領域の高さから行数確定
            int lmax = (int)(owin.Height / lb.Height) -1 ;
            Random rnd = new Random();
            int y = rnd.Next(0, lmax) * lb.Height;

            lb.Location = new Point(owin.Width, y);
            owin.Controls.Add(lb);
        }
        static public void addlabel_l(int l,string s)
        {
            System.Windows.Forms.Label lb = new System.Windows.Forms.Label();
            lb.AutoSize = true;
            lb.Text = s;
            lb.ForeColor = Program.Color16List[Program.__ChatFColor];
            lb.Font = new System.Drawing.Font("ＭＳ ゴシック", 18,
                System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);

            //領域の高さから行数確定
            int lmax = (int)(owin.Height / lb.Height) - 1;
            int y = l * lb.Height;

            lb.Location = new Point(owin.Width, y);
            owin.Controls.Add(lb);
        }
    }
}
