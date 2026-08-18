namespace MabiChatSpeech
{
    partial class Frm_browser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_browser));
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            BTN_Studio = new System.Windows.Forms.ToolStripButton();
            TST_ChannelID = new System.Windows.Forms.ToolStripTextBox();
            TSL_LiveStream = new System.Windows.Forms.ToolStripButton();
            TSL_LiveChatPopup = new System.Windows.Forms.ToolStripButton();
            TST_LiveID = new System.Windows.Forms.ToolStripTextBox();
            TSL_UrlPos = new System.Windows.Forms.ToolStripLabel();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            TST_Url = new System.Windows.Forms.ToolStripTextBox();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            panel1 = new System.Windows.Forms.Panel();
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            toolStrip1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { BTN_Studio, TST_ChannelID, TSL_LiveStream, TSL_LiveChatPopup, TST_LiveID, TSL_UrlPos, toolStripSeparator2, toolStripLabel2, TST_Url, toolStripSeparator1 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1005, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // BTN_Studio
            // 
            BTN_Studio.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            BTN_Studio.Image = (System.Drawing.Image)resources.GetObject("BTN_Studio.Image");
            BTN_Studio.ImageTransparentColor = System.Drawing.Color.Magenta;
            BTN_Studio.Name = "BTN_Studio";
            BTN_Studio.Size = new System.Drawing.Size(45, 22);
            BTN_Studio.Text = "Studio";
            BTN_Studio.ToolTipText = "Studioに移動";
            BTN_Studio.Click += BTN_Studio_Click;
            // 
            // TST_ChannelID
            // 
            TST_ChannelID.BackColor = System.Drawing.SystemColors.Info;
            TST_ChannelID.Enabled = false;
            TST_ChannelID.Name = "TST_ChannelID";
            TST_ChannelID.ReadOnly = true;
            TST_ChannelID.Size = new System.Drawing.Size(88, 25);
            // 
            // TSL_LiveStream
            // 
            TSL_LiveStream.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            TSL_LiveStream.Image = (System.Drawing.Image)resources.GetObject("TSL_LiveStream.Image");
            TSL_LiveStream.ImageTransparentColor = System.Drawing.Color.Magenta;
            TSL_LiveStream.Name = "TSL_LiveStream";
            TSL_LiveStream.Size = new System.Drawing.Size(32, 22);
            TSL_LiveStream.Text = "Live";
            TSL_LiveStream.Click += TSL_LiveStream_Click;
            // 
            // TSL_LiveChatPopup
            // 
            TSL_LiveChatPopup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            TSL_LiveChatPopup.Image = (System.Drawing.Image)resources.GetObject("TSL_LiveChatPopup.Image");
            TSL_LiveChatPopup.ImageTransparentColor = System.Drawing.Color.Magenta;
            TSL_LiveChatPopup.Name = "TSL_LiveChatPopup";
            TSL_LiveChatPopup.Size = new System.Drawing.Size(35, 22);
            TSL_LiveChatPopup.Text = "Chat";
            TSL_LiveChatPopup.Click += TSL_LiveChatPopup_Click;
            // 
            // TST_LiveID
            // 
            TST_LiveID.BackColor = System.Drawing.SystemColors.Info;
            TST_LiveID.Name = "TST_LiveID";
            TST_LiveID.ReadOnly = true;
            TST_LiveID.Size = new System.Drawing.Size(120, 25);
            TST_LiveID.ToolTipText = "LiveIDを入力、EnterでChatPopupに移動";
            TST_LiveID.KeyDown += TST_LiveID_KeyDown;
            // 
            // TSL_UrlPos
            // 
            TSL_UrlPos.AutoSize = false;
            TSL_UrlPos.Name = "TSL_UrlPos";
            TSL_UrlPos.Size = new System.Drawing.Size(80, 22);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new System.Drawing.Size(28, 22);
            toolStripLabel2.Text = "URL";
            // 
            // TST_Url
            // 
            TST_Url.BackColor = System.Drawing.Color.White;
            TST_Url.Name = "TST_Url";
            TST_Url.Size = new System.Drawing.Size(400, 25);
            TST_Url.ToolTipText = "URL：Enterで移動";
            TST_Url.KeyDown += TST_Url_KeyDown;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // panel1
            // 
            panel1.Controls.Add(webView);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 25);
            panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1005, 425);
            panel1.TabIndex = 2;
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = System.Drawing.Color.White;
            webView.Dock = System.Windows.Forms.DockStyle.Fill;
            webView.Location = new System.Drawing.Point(0, 0);
            webView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            webView.Name = "webView";
            webView.Size = new System.Drawing.Size(1005, 425);
            webView.TabIndex = 1;
            webView.ZoomFactor = 1D;
            webView.NavigationStarting += webView_NavigationStarting;
            webView.NavigationCompleted += webView_NavigationCompleted;
            webView.SourceChanged += webView_SourceChanged;
            webView.ContentLoading += webView_ContentLoading;
            // 
            // Frm_browser
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1005, 450);
            Controls.Add(panel1);
            Controls.Add(toolStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "Frm_browser";
            Text = "YoutubeChat";
            FormClosing += Frm_browser_FormClosing;
            Load += Frm_browser_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripTextBox TST_Url;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox TST_LiveID;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton BTN_Studio;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton TSL_LiveStream;
        private System.Windows.Forms.ToolStripTextBox TST_ChannelID;
        private System.Windows.Forms.ToolStripLabel TSL_UrlPos;
        private System.Windows.Forms.ToolStripButton TSL_LiveChatPopup;
    }
}