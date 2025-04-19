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

        private enum ZOrder { N ,None,Overlaped,Resize,Top,Under,Between};

        private ZOrder SetOverlayZOrder()
        {
            WinApi._WinLayer();

            int lc = 0;
            int lo = 0;
            int lm = 0;
            foreach (IntPtr a in WinApi._Win_order)
            {
                int l = WinApi.GetWindowTextLength(a);
                StringBuilder tsb = new StringBuilder(l + 1);
                WinApi.GetWindowText(a, tsb, tsb.Capacity);

                if (tsb.ToString() == "マビノギ")
                {
                    _mw = a;
                    lm = lc;
                }
                if (a == this.Handle )
                {
                    lo = lc;
                }
                lc++;
            }

            if (_mw == IntPtr.Zero)
            {
/*
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
*/
                return (ZOrder.None);
            }

            int x = 0, y = 0, cx = 0, cy = 0;
            WinApi.WINDOWINFO _mwi = new WinApi.WINDOWINFO();
            WinApi.GetWindowInfo(_mw, ref _mwi);

            WinApi.WINDOWINFO _owi = new WinApi.WINDOWINFO();
            WinApi.GetWindowInfo(this.Handle, ref _owi);
            Boolean Resize = false;


            x = _mwi.rcClient.left;
            y = _mwi.rcClient.top;
            cx = _mwi.rcClient.right - _mwi.rcClient.left;
            cy = _mwi.rcClient.bottom - _mwi.rcClient.top;

            if ( _owi.rcWindow.Equals(_mwi.rcClient) != true )
            {
                Resize = true;
            }


            // マビノギが最上位なら
            if (lm == 0)
            {
                this.Activate();
                WinApi.SetWindowPos(this.Handle, WinApi.HWND_TOPMOST, x, y, cx, cy, (WinApi.SWP_NOSIZE | WinApi.SWP_NOMOVE));
                WinApi.SetWindowPos(WinApi._Win_order[lm], this.Handle, x, y, cx, cy, (WinApi.SWP_NOSIZE | WinApi.SWP_NOMOVE));
                return (ZOrder.Top);
            }
            // オーバーレイがマビノギの下にある場合
            else if (lo > lm)
            {
                WinApi.SetWindowPos(this.Handle, WinApi._Win_order[lm - 1], x, y, cx, cy, (WinApi.SWP_SHOWWINDOW));
                return (ZOrder.Under);
            }
            else if (lo == (lm-1) )
            {
                // 正常
                if ( Resize )
                {
                    WinApi.SetWindowPos(this.Handle, WinApi._Win_order[lm - 1], x, y, cx, cy, (WinApi.SWP_ASYNCWINDOWPOS));
                    return (ZOrder.Resize);
                }
                else
                {
                    return (ZOrder.Overlaped);
                }
            }
            else
            {
                // マビノギとオーバーレイの間に他のウィンドウがある場合
                WinApi.SetWindowPos(this.Handle, WinApi._Win_order[lm - 1], x, y, cx, cy, (WinApi.SWP_NOACTIVATE));
                return (ZOrder.Between);
            }

        }

        private void Overlay_Shown(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            owin = this;
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
        static private ZOrder zsts =ZOrder.N;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //窓の状況
            ZOrder z = SetOverlayZOrder();

            if ( z != zsts )
            {
                Debug.Print($"{z}");
                WinApi.WINDOWINFO _mwi = new WinApi.WINDOWINFO();
                WinApi.GetWindowInfo(_mw, ref _mwi);

                WinApi.WINDOWINFO _owi = new WinApi.WINDOWINFO();
                WinApi.GetWindowInfo(this.Handle, ref _owi);

                Debug.Print($"ov ({_owi.rcWindow.left},{_owi.rcWindow.top}),({_owi.rcWindow.right},{_owi.rcWindow.bottom})");
                Debug.Print($"ma ({_mwi.rcClient.left},{_mwi.rcClient.top}),({_mwi.rcClient.right},{_mwi.rcClient.bottom})");
                foreach (IntPtr a in WinApi._Win_order)
                {
                    WinApi.WINDOWINFO _wi = new WinApi.WINDOWINFO();
                    WinApi.GetWindowInfo(a, ref _wi);
                    int l = WinApi.GetWindowTextLength(a);
                    StringBuilder tsb = new StringBuilder(l + 1);
                    WinApi.GetWindowText(a, tsb, tsb.Capacity);
                    Debug.Print($"{a:x8} {_wi.dwStyle:x8} " + tsb.ToString());
                }
                zsts = z;
            }

            if (z == ZOrder.None)
            {
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
