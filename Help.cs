using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MabiChatSpeech
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            // WebView2の環境初期化
            var cacheFolderPath = System.IO.Path.Combine(Program.__SavePath, "webview2cache");
            var webView2Environment = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, cacheFolderPath);
            await webView21.EnsureCoreWebView2Async(webView2Environment);


            // 実行ファイルと同じ場所にある "help/index.html" を表示する場合
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string htmlPath = Path.Combine(appDir, "", "help.html");

            // 存在確認してから読み込む
            if (File.Exists(htmlPath))
            {
                webView21.Source = new Uri(htmlPath);
            }
            else
            {
                MessageBox.Show("ヘルプファイルが見つかりません: " + htmlPath);
                this.Close();
            }
        }

    }
}
