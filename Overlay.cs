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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        private IntPtr _mw ;
        private IntPtr _tw ;
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
            int x=0,y=0,cx=0,cy=0;
            WinApi.WINDOWINFO _wi = new WinApi.WINDOWINFO();

            // Window一覧取得
            WinApi._WinLayer();

            _mw = IntPtr.Zero ;
            foreach (IntPtr a in WinApi._Win_order )
            {
                WinApi.GetWindowInfo(a, ref _wi);
                int l = WinApi.GetWindowTextLength(a);
                StringBuilder tsb = new StringBuilder(l + 1);
                WinApi.GetWindowText(a, tsb, tsb.Capacity);

                if (tsb.ToString() == "マビノギ" )
                {

                    _mw = a;
                    x = _wi.rcClient.left;
                    y = _wi.rcClient.top;
                    cx = _wi.rcClient.right - _wi.rcClient.left;
                    cy = _wi.rcClient.bottom - _wi.rcClient.top;
                }

                //                Debug.Print($"{a:x8} {_wi.dwStyle:x8} " +tsb.ToString());
            }

            if (_mw != IntPtr.Zero)
            {
                //WinApi.SetForegroundWindow(_mw);

                //WinApi.SetWindowPos(_mw, WinApi.HWND_TOP, x, y, cx, cy, (WinApi.SWP_NOMOVE | WinApi.SWP_NOSIZE));
                // WinApi.SetWindowPos(this.Handle, WinApi.HWND_TOP, x, y, cx, cy, WinApi.SWP_NOACTIVATE);

                WinApi.SetWindowPos(this.Handle, WinApi.HWND_TOPMOST, x, y, cx, cy, WinApi.SWP_SHOWWINDOW);
            }
            else
            {
                Debug.Print($"---Mabanogi Not Run---");
                foreach (IntPtr a in WinApi._Win_order)
                {
                    WinApi.GetWindowInfo(a, ref _wi);
                    int l = WinApi.GetWindowTextLength(a);
                    StringBuilder tsb = new StringBuilder(l + 1);
                    WinApi.GetWindowText(a, tsb, tsb.Capacity);
                    Debug.Print($"{a:x8} {_wi.dwStyle:x8} " + tsb.ToString());
                }
            }



        }
        private int SetOverlayZOrder()
        {
            WinApi._WinLayer();
            _tw = IntPtr.Zero;
            if (_mw == IntPtr.Zero)
            {
                Debug.Print("-NoTarget-");
                foreach (IntPtr a in WinApi._Win_order)
                {
                    WinApi.WINDOWINFO _wi = new WinApi.WINDOWINFO();
                    WinApi.GetWindowInfo(a, ref _wi);
                    int l = WinApi.GetWindowTextLength(a);
                    StringBuilder tsb = new StringBuilder(l + 1);
                    WinApi.GetWindowText(a, tsb, tsb.Capacity);
                    Debug.Print($"{a:x8} {_wi.dwStyle:x8} " + tsb.ToString());
                }
                return (0);
            }

            int lc = 0;
            int lo = 0;
            int lm = 0;
            int lt = -1;
            foreach (IntPtr a in WinApi._Win_order)
            {
                if( a == _mw )
                {
                    lm = lc;
                    lt = lc - 1;
                }
                if (a == owin.Handle )
                {
                    lo = lc;
                }
                lc++;
            }

            // マビノギが最上位なら
            if (lm == 0)
            {
                Debug.Print($"(1) lt={lt} lo={lo} lm={lm}");
                return (1);
            }
            // オーバーレイがマビノギの下にある場合
            else if (lo > lm)
            {
                _tw = WinApi._Win_order[lt];
                Debug.Print($"(-1) lt={lt} lo={lo} lm={lm}");
                return (-1);
            }
            else if (lo == lm-1)
            {
                // 正常
//                Debug.Print($"(0) lt={lt} lo={lo} lm={lm}");
                return (0);
            }
            else
            {
                _tw = WinApi._Win_order[lt];
                Debug.Print($"(-2) lt={lt} lo={lo} lm={lm}");
                return (-2);
            }

        }
        private void SetOverlaySize()
        {
            if( _mw == IntPtr.Zero )
            {
                return;
            }

            WinApi.WINDOWINFO _wi = new WinApi.WINDOWINFO();
            int x = 0, y = 0, cx = 0, cy = 0;

            WinApi.GetWindowInfo(_mw, ref _wi);
            x = _wi.rcClient.left;
            y = _wi.rcClient.top;
            cx = _wi.rcClient.right - _wi.rcClient.left;
            cy = _wi.rcClient.bottom - _wi.rcClient.top;

            foreach (IntPtr a in WinApi._Win_order)
            {
                WinApi.GetWindowInfo(a, ref _wi);
                int l = WinApi.GetWindowTextLength(a);
                StringBuilder tsb = new StringBuilder(l + 1);
                WinApi.GetWindowText(a, tsb, tsb.Capacity);
                Debug.Print($"{a:x8} {_wi.dwStyle:x8} " + tsb.ToString());
            }

            switch ( SetOverlayZOrder() )
            {
                case 1 :
                    WinApi.SetWindowPos(this.Handle, _mw, x, y, cx, cy, WinApi.SWP_NOACTIVATE);
                    break;
                case -1:

//                    WinApi.SetWindowPos(_mw, WinApi.HWND_TOP, x, y, cx, cy, (WinApi.SWP_NOMOVE| WinApi.SWP_NOSIZE));
                    WinApi.SetWindowPos(this.Handle, _mw, x, y, cx, cy, WinApi.SWP_NOACTIVATE);
                    break;
            }

        }

        private void Overlay_Shown(object sender, EventArgs e)
        {

            timer1.Enabled = true;
            owin = this;
            mwlist2();
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
            //窓の状況
            int z = SetOverlayZOrder();
            WinApi.WINDOWINFO _wi = new WinApi.WINDOWINFO();
            WinApi.GetWindowInfo(_mw, ref _wi);
            int x = _wi.rcClient.left;
            int y = _wi.rcClient.top;
            int cx = _wi.rcClient.right - _wi.rcClient.left;
            int cy = _wi.rcClient.bottom - _wi.rcClient.top;
/*
            Debug.Print($"< {z} >");
            foreach (IntPtr a in WinApi._Win_order)
            {
                WinApi.GetWindowInfo(a, ref _wi);
                int l = WinApi.GetWindowTextLength(a);
                StringBuilder tsb = new StringBuilder(l + 1);
                WinApi.GetWindowText(a, tsb, tsb.Capacity);
                Debug.Print($"{a:x8} {_wi.dwStyle:x8} " + tsb.ToString());
            }
*/
            switch ( z )
            {
                case 2 :
                    //子のラベルをクリア
                    foreach (Control co in Controls)
                    {
                        if (co.GetType().Equals(typeof(System.Windows.Forms.Label)))
                        {
                            co.Dispose();
                        }
                    }
                    return;
                case 1:
                    WinApi.SetWindowPos(this.Handle , WinApi.HWND_TOPMOST, x, y, cx, cy, (WinApi.SWP_SHOWWINDOW));
                    break;
                case 0:
                    WinApi.SetWindowPos(this.Handle, _tw, x, y, cx, cy, (WinApi.SWP_NOACTIVATE));
                    break;
                case -1:
                    WinApi.SetWindowPos(this.Handle, _tw, x, y, cx, cy, (WinApi.SWP_NOACTIVATE));
                    break;
                case -2:
                    WinApi.SetWindowPos(this.Handle, _tw, x, y, cx, cy, (WinApi.SWP_NOACTIVATE));
                    break;
                default:
                    //子のラベルをクリア
                    foreach (Control co in Controls)
                    {
                        if (co.GetType().Equals(typeof(System.Windows.Forms.Label)))
                        {
                            co.Dispose();
                        }
                    }
                    return;
            }


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
