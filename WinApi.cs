using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace MabiChatSpeech
{
    public static class WinApi
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWINFO
        {
            public int cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public short atomWindowType;
            public short wCreatorVersion;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        public const int SWP_NOMOVE = 0x0002;//現在位置を保持（X,Yパラメーターを無視）。
        public const int SWP_NOSIZE = 0x0001;//現在のサイズを保持（cx,cyパラメーターを無視）。
        public const int SWP_NOACTIVATE = 0x0010 ;
        public const int SWP_SHOWWINDOW = 0x0040;

        public const int HWND_BOTTOM = 1;
        public const int HWND_NOTOPMOST = -2;
        public const int HWND_TOP = 0;
        public const int HWND_TOPMOST = -1;

        [DllImport("USER32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetDesktopWindow();

        public const uint GW_CHILD = 5;
        public const uint GW_ENABLEDPOPUP = 6;
        public const uint GW_HWNDFIRST = 0;
        public const uint GW_HWNDLAST = 1;
        public const uint GW_HWNDNEXT = 2;
        public const uint GW_HWNDPREV = 3;
        public const uint GW_OWNER = 5;
        [DllImport("USER32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindow(IntPtr HWND , uint uCmd);

        [DllImport("USER32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetNextWindow(IntPtr HWND, uint uCmd);


        [DllImport("USER32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int length);


        public const uint WS_BORDER = 0x00800000;
        public const uint WS_CAPTION = 0x00C00000;
        public const uint WS_CHILD = 0x40000000;
        public const uint WS_CHILDWINDOW = 0x40000000;
        public const uint WS_CLIPCHILDREN = 0x02000000;
        public const uint WS_CLIPSIBLINGS = 0x04000000;
        public const uint WS_DISABLED = 0x08000000;
        public const uint WS_DLGFRAME = 0x00400000;
        public const uint WS_GROUP = 0x00020000;
        public const uint WS_HSCROLL = 0x00100000;
        public const uint WS_ICONIC = 0x20000000;
        public const uint WS_MAXIMIZE = 0x01000000;
        public const uint WS_MAXIMIZEBOX = 0x00010000;
        public const uint WS_MINIMIZE = 0x20000000;
        public const uint WS_MINIMIZEBOX = 0x00020000;
        public const uint WS_OVERLAPPED = 0x00000000;
        public const uint WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
        public const uint WS_POPUP = 0x80000000;
        public const uint WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU);
        public const uint WS_SIZEBOX = 0x00040000;
        public const uint WS_SYSMENU = 0x00080000;
        public const uint WS_TABSTOP = 0x00010000;
        public const uint WS_THICKFRAME = 0x00040000;
        public const uint WS_TILED = 0x00000000;
        public const uint WS_TILEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
        public const uint WS_VISIBLE = 0x10000000;
        public const uint WS_VSCROLL = 0x00200000;

        [DllImport("user32.dll")]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        [DllImport("USER32.DLL")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("USER32.DLL")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowEx(IntPtr parentWnd, IntPtr previousWnd, string className, string windowText);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);


        public static List<IntPtr> _Win_order = new List <IntPtr>();
        public static void _WinLayer()
        {

            IntPtr w = WinApi.GetDesktopWindow();
            WinApi.WINDOWINFO _wi = new WinApi.WINDOWINFO();

            WinApi._Win_order.Clear();
            w = WinApi.GetWindow(w, WinApi.GW_CHILD);
            WinApi.GetWindowInfo(w, ref _wi);


            while (w != IntPtr.Zero)
            {

                if ((_wi.dwStyle & WinApi.WS_VISIBLE) != 0)
                {
                    // if ( ((_wi.dwStyle & WinApi.WS_TABSTOP) != 0) && (_wi.dwStyle & WinApi.WS_ICONIC) == 0) 
                    if ((_wi.dwStyle & 0x80000000) != 0x80000000)
                    {
                        WinApi._Win_order.Add(w);

                    }
                }
                w = WinApi.GetWindow(w, WinApi.GW_HWNDNEXT);
                WinApi.GetWindowInfo(w, ref _wi);

            }

        }

        public static IntPtr _FindWindow(string class_name, string window_name)
        {
            IntPtr hWnd = IntPtr.Zero;
            while (IntPtr.Zero != (hWnd = FindWindowEx(IntPtr.Zero, hWnd, class_name, window_name)))
            {
                return (hWnd);
            }

            return (IntPtr.Zero);
        }
        
        public static RECT GetMabinogiRect()
        {
            RECT rect = new RECT();
            WINDOWINFO _wi = new WINDOWINFO();

            IntPtr m = WinApi._FindWindow("Mabinogi", "マビノギ");
            if (m != IntPtr.Zero)
            {
                GetWindowInfo(m, ref _wi);
                rect = _wi.rcClient ;
            }

            return (rect);
        }
        public static void SetMabinogiOverlay(IntPtr ov)
        {
            RECT rect = new RECT();
            WINDOWINFO _wi = new WINDOWINFO();

            IntPtr m = WinApi._FindWindow("Mabinogi", "マビノギ");
            if (m != IntPtr.Zero)
            {
                GetWindowInfo(m, ref _wi);
                rect = _wi.rcClient;
                WinApi.SetWindowPos(ov, HWND_TOP, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, 0);// SWP_NOACTIVATE | SWP_SHOWWINDOW );
            }

        }
    }
    public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);


    /// <summary>
    /// キーボードをシミュレート
    /// </summary>
    class KeyboardEmulate
    {
        /// <summary>
        /// 現在選択されているウィンドウに対してキーを送信
        /// </summary>
        /// <param name="keys">送信するキー</param>
        public void writeKeys(string keys)
        {
            // 選択しているウィンドウを取得
            IntPtr targetWindowHandle = WinApi.GetForegroundWindow();
            if (targetWindowHandle == IntPtr.Zero)
            {
                // 操作できるウィンドウがない
                return;
            }

            // 現在選択しているウィンドウに対してキーを送信
            SendKeys.Send(keys+ "~");

//            // タイプ後にENTERを送信
//            SendKeys.Send("{ENTER}");
        }

    }

}
