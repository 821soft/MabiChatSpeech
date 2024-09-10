using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace MabiChatSpeech.TextEmulate
{
    /// <summary>
    /// ウィンドウ周りのAPI参照クラス
    /// </summary>
    class MyWindowApi
    {
        /// <summary>
        /// 選択しているウィンドウのハンドルを取得
        /// </summary>
        /// <returns>ウィンドウのハンドル</returns>
        [DllImport("USER32.DLL")]
        public static extern IntPtr GetForegroundWindow();
    }
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
            IntPtr targetWindowHandle = MyWindowApi.GetForegroundWindow();
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
