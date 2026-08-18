using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NAudio.Wave.WaveInterop;

namespace MabiChatSpeech
{
    public partial class Frm_browser : Form
    {
        private string YoutubeStudioURL = "https://studio.youtube.com/";
        private string LiveChatBaseURL = "https://studio.youtube.com/live_chat?is_popout=1&v=";
        private string LiveConsoleURL = "https://studio.youtube.com/video/";
        private string LiveChatID = "";
        private string ChannelID = "";
        private UrlPos CurUrlPos=UrlPos.Other;
        public Frm_browser()
        {
            InitializeComponent();
        }
        /// <summary>
        /// URL文字列からchannelIDを取り出す
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetChannelID(string url)
        {
            string ret = "";
            if (url.Contains("/channel/"))
            {
                string[] sub = url.Split('/');

                int i = Array.IndexOf(sub,"channel");
                ret = sub[i+1];
            }
            return (ret);
        }

        /// <summary>
        /// URL文字列からliveIDを取り出す
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetLiveID(string url)
        {
            string ret = "";

            if (url.Contains("/video/"))
            {
                string[] sub = url.Split('/');
                if (sub[sub.Length - 1] == "livestreaming")
                {
                    ret = sub[sub.Length - 2];
                }
            }

            return (ret);
        }

        enum UrlPos { Studio , Channel , Console , ChatPopup , Other};
        /// <summary>
        /// URL文字列から状態を判定する
        /// </summary>
        /// <param name="s">　呼び出し元</param>
        /// <param name="url">　URL </param>
        /// <returns></returns>
        private  UrlPos URL_Type(string s , string url)
        {
            UrlPos ret = UrlPos.Other;
            string url_studio = YoutubeStudioURL;
            string url_studio_LiveChannel = YoutubeStudioURL + "channel/" + ChannelID;
            string url_studio_LiveConsole = LiveConsoleURL + LiveChatID + "/livestreaming";
            string url_studio_LiveChat = LiveChatBaseURL + LiveChatID;
            var uri = webView.Source;
            if (uri != null)
            {
                TST_Url.Text = uri.ToString();
            }

            if (url == url_studio)
            {
                TSL_UrlPos.Text = "Studio";
                TSL_LiveStream.Enabled = false;
                TSL_LiveChatPopup.Enabled = false;
                TSL_LiveChatPopup.ForeColor = Color.Black;
                TSL_LiveChatPopup.BackColor = SystemColors.Control;

                ChannelID = "";
                TST_ChannelID.Text = ChannelID;
                LiveChatID = "";
                TST_LiveID.Text = LiveChatID;

                ret = UrlPos.Studio;
            }
            else if (url.Contains( url_studio_LiveChannel))
            {
                TSL_UrlPos.Text = "Channel";
                ChannelID = GetChannelID(url);
                TSL_LiveStream.Enabled = true;
                TSL_LiveChatPopup.Enabled = false;
                TSL_LiveStream.ForeColor = Color.Black;
                TSL_LiveChatPopup.ForeColor = Color.Black;
                TSL_LiveChatPopup.BackColor = SystemColors.Control;
                TST_ChannelID.Text = ChannelID;

                ret = UrlPos.Channel;
            }
            else if (url.Contains("/video/"))
            {
                TSL_UrlPos.Text = "Console";
                LiveChatID = GetLiveID(url);
                TSL_LiveStream.Enabled = true;
                TSL_LiveChatPopup.Enabled = true;
                TSL_LiveStream.ForeColor = Color.Red;
                TSL_LiveChatPopup.ForeColor = Color.Black;
                TSL_LiveChatPopup.BackColor = SystemColors.Control;

                TST_LiveID.Text = LiveChatID;

                ret = UrlPos.Console;
            }
            else if (url == url_studio_LiveChat)
            {
                TSL_UrlPos.Text = "ChatPopup";
                TSL_LiveStream.Enabled = true;
                TSL_LiveChatPopup.Enabled = true;
                TSL_LiveStream.ForeColor = Color.Red;
                TSL_LiveChatPopup.ForeColor = Color.White;
                TSL_LiveChatPopup.BackColor = Color.Red ;
                ret = UrlPos.ChatPopup;
            }
            else
            {
                TSL_UrlPos.Text = "Other";
                TSL_LiveStream.Enabled = false;
                TSL_LiveChatPopup.Enabled = false;
                TSL_LiveStream.ForeColor = Color.Gray;
                TSL_LiveChatPopup.ForeColor = Color.Gray;
                TSL_LiveChatPopup.BackColor = SystemColors.Control;
                ret = UrlPos.Other;
            }
            Debug.Print($"Call:{s} URLPOS:({CurUrlPos}=>{ret}) {url} ChannelID{ChannelID} LiveID{LiveChatID}");

            return ret;
        }
        /// <summary>
        /// URL移動開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webView_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            var str = webView.Source.ToString();
            CurUrlPos = URL_Type("→NavigationStarting.URL",str);
        }

        /// <summary>
        /// コンテンツロード開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webView_ContentLoading(object sender, CoreWebView2ContentLoadingEventArgs e)
        {
            var str = webView.Source.ToString();
            Debug.Print($"→ContentLoading.URL={str}");
        }

        /// <summary>
        /// URL移動完了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            var str = webView.Source.ToString();
            CurUrlPos = URL_Type("→NaviComp.URL", str);
            if (e.IsSuccess == false)
            {
                return;
            }
            if (CurUrlPos == UrlPos.ChatPopup )
            {
                ChatPopupSend("Start forwarding the chat by MCS.");
            }

        }

        /// <summary>
        /// URLの遷移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webView_SourceChanged(object sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs e)
        {
            var str = webView.Source.ToString();
            CurUrlPos = URL_Type("→SourceChanged.URL" , str);
            //URL_Check();
        }

        /// <summary>
        /// ChatPopoutにJavaScriptでコメントを送信する
        /// </summary>
        /// <param name="message"></param>
        public async void SendYouTubeLiveChatPopupAsync(string message)
        {
            // JavaScriptのエスケープ処理
            string safeMessage = System.Web.HttpUtility.JavaScriptStringEncode(message);

            // 文字入力と送信ボタンのクリックを行うJavaScript
            string jsCode = $@"
        (function() {{
            // 1. 入力欄の特定と文字注入
            const chatInput = document.querySelector('#input.yt-live-chat-text-input-field-renderer');
            if (!chatInput) {{ 
                return( document.querySelectorAll(selector) );
                return 'Input field not found';
            }}

            chatInput.focus();
            chatInput.textContent = '{safeMessage}';
            chatInput.dispatchEvent(new Event('input', {{ bubbles: true }}));

            // 2. 送信ボタンを特定してクリック
            // ※ yt-button-renderer などの内側にある実際のボタン要素（button または #button）を狙います
            const sendButton = document.querySelector('#send-button button, #send-button #button');
            
            if (sendButton) {{
                // ボタンが有効化（Disabledが解除）されるのをわずかに待つか、直接クリック
                sendButton.click();
                return 'Sent {safeMessage}';
            }} else {{
                return 'Send button not found';
            }}
        }})();
    ";

            // WebView2でスクリプトを実行
            string result = await webView.CoreWebView2.ExecuteScriptAsync(jsCode);

            // 実行結果をデバッグ出力（"Sent" が返れば成功）
            Debug.Print($"Chat Result: {result}");
        }
        /// <summary>
        /// chatpopupに送信のスレッド操作
        /// </summary>
        /// <param name="s"></param>
        public void ChatPopupSend(string s)
        {
            if (webView.InvokeRequired)
            {
                // UIスレッド以外から呼ばれた場合は、UIスレッドに処理を任せる
                webView.Invoke(new Action(() =>
                {
                    SendYouTubeLiveChatPopupAsync(s);
                }));
            }
            else
            {
                // すでにUIスレッドにいる場合はそのまま実行
                SendYouTubeLiveChatPopupAsync(s);
            }
        }

        /// <summary>
        /// Formを閉じる動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_browser_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void TST_Url_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    e.SuppressKeyPress = true;
                    Uri url;
                    url = new Uri(TST_Url.Text);
                    webView.Source = url;
                    break;

                default:
                    break;

            }
        }

        private void TST_LiveID_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    e.SuppressKeyPress = true;
                    LiveChatID = TST_LiveID.Text.Trim(' ');
                    string LiveChatUrl = LiveChatBaseURL + LiveChatID;
                    Uri url;
                    url = new Uri(LiveChatUrl);
                    webView.Source = url;
                    TST_Url.Text = LiveChatBaseURL + LiveChatID;
                    break;

                default:
                    break;

            }
        }

        private void Frm_browser_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"WebView2の初期化に失敗しました: {ex.Message}", "エラー");
            }

            // 初期表示は studio.youtube.com
            Uri url;
            url = new Uri(YoutubeStudioURL);
            webView.Source = url;


        }

        /// <summary>
        /// WebView2の環境設定
        /// </summary>
        private async void InitializeAsync()
        {
            var cacheFolderPath = System.IO.Path.Combine(Program.__SavePath, "webview2cache");
            var webView2Environment = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, cacheFolderPath);
            await webView.EnsureCoreWebView2Async(webView2Environment);
        }

        /// <summary>
        /// Youtube Studioボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_Studio_Click(object sender, EventArgs e)
        {
            Debug.Print("StudioClick");

            Uri url;
            url = new Uri(YoutubeStudioURL);
            webView.Source = url;
            TST_Url.Text = YoutubeStudioURL;
        }
        /// <summary>
        /// Live Streamボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TSL_LiveStream_Click(object sender, EventArgs e)
        {
            Debug.Print("LiveClick");
            string LiveChatUrl = YoutubeStudioURL + "/channel/" + ChannelID + "/livestreaming";
            Uri url;
            url = new Uri(LiveChatUrl);
            webView.Source = url;
            TST_Url.Text = LiveChatUrl;

        }
        /// <summary>
        /// Live Chatボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TSL_LiveChatPopup_Click(object sender, EventArgs e)
        {
            Debug.Print("ChatClick");

            // URLからLiveIDを切り取る
            string[] subs = TST_Url.Text.Split('/');
            if (subs.Length == 0)
            {
                return;
            }
            if (subs[subs.Length - 1] == "livestreaming")
            {
                LiveChatID = subs[subs.Length - 2];
                TST_LiveID.Text = LiveChatID;
                string LiveChatUrl = LiveChatBaseURL + LiveChatID;
                Uri url;
                url = new Uri(LiveChatUrl);
                webView.Source = url;
                TST_Url.Text = LiveChatBaseURL + LiveChatID;

            }
            else
            {
                string[] subs2 = subs[subs.Length - 1].Split('=');
                if (subs2.Length == 0)
                {
                    return;
                }
                if (subs2[0] == "live_chat?is_popout")
                {
                    LiveChatID = subs2[2];
                    TST_LiveID.Text = LiveChatID;
                    string LiveChatUrl = LiveChatBaseURL + LiveChatID;
                    Uri url;
                    url = new Uri(LiveChatUrl);
                    webView.Source = url;
                    TST_Url.Text = LiveChatBaseURL + LiveChatID;

                }
            }
        }


    }
}
