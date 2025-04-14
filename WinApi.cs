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
            public int dwStyle;
            public int dwExStyle;
            public int dwWindowStatus;
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

        public const int GW_CHILD = 5;
        public const int GW_ENABLEDPOPUP = 6;
        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDLAST = 1;
        public const int GW_HWNDNEXT = 2;
        public const int GW_HWNDPREV = 3;
        public const int GW_OWNER = 5;
        [DllImport("USER32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindow(IntPtr HWND , uint uCmd);

        [DllImport("USER32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetNextWindow(IntPtr HWND, uint uCmd);


        [DllImport("USER32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int length);

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


        public static void _WinLayer()
        {
            
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
                WinApi.SetWindowPos(ov, HWND_TOPMOST, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, 0);// SWP_NOACTIVATE | SWP_SHOWWINDOW );
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
