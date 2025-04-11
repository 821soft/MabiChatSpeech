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
