namespace MabiChatSpeech
{
    partial class Main
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            SLB_Mode = new System.Windows.Forms.ToolStripStatusLabel();
            SLB_SaveMode = new System.Windows.Forms.ToolStripStatusLabel();
            SLB_Client = new System.Windows.Forms.ToolStripStatusLabel();
            SLB_Ip = new System.Windows.Forms.ToolStripStatusLabel();
            SLB_SendTask = new System.Windows.Forms.ToolStripStatusLabel();
            BTN_SendTask = new System.Windows.Forms.ToolStripDropDownButton();
            Btn_Redirect = new System.Windows.Forms.ToolStripStatusLabel();
            Txt_Chat = new System.Windows.Forms.TextBox();
            Tim_Status = new System.Windows.Forms.Timer(components);
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            Btn_Clear = new System.Windows.Forms.ToolStripButton();
            Btn_Setup = new System.Windows.Forms.ToolStripButton();
            Btn_echa = new System.Windows.Forms.ToolStripButton();
            Btn_List = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            Cmb_Whitelist = new System.Windows.Forms.ToolStripComboBox();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            Cmb_User = new System.Windows.Forms.ToolStripComboBox();
            toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            Cmb_Npc = new System.Windows.Forms.ToolStripComboBox();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            imageList1 = new System.Windows.Forms.ImageList(components);
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { SLB_Mode, SLB_SaveMode, SLB_Client, SLB_Ip, SLB_SendTask, BTN_SendTask, Btn_Redirect });
            statusStrip1.Location = new System.Drawing.Point(0, 172);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.ShowItemToolTips = true;
            statusStrip1.Size = new System.Drawing.Size(740, 29);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // SLB_Mode
            // 
            SLB_Mode.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SLB_Mode.Name = "SLB_Mode";
            SLB_Mode.Size = new System.Drawing.Size(0, 24);
            SLB_Mode.ToolTipText = "Mode";
            // 
            // SLB_SaveMode
            // 
            SLB_SaveMode.AutoSize = false;
            SLB_SaveMode.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SLB_SaveMode.Name = "SLB_SaveMode";
            SLB_SaveMode.Size = new System.Drawing.Size(64, 24);
            SLB_SaveMode.ToolTipText = "チャットログの保存方法";
            // 
            // SLB_Client
            // 
            SLB_Client.AutoSize = false;
            SLB_Client.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SLB_Client.Name = "SLB_Client";
            SLB_Client.Size = new System.Drawing.Size(60, 24);
            SLB_Client.ToolTipText = "接続状態";
            // 
            // SLB_Ip
            // 
            SLB_Ip.AutoSize = false;
            SLB_Ip.Image = Properties.Resources.icn_tarlach;
            SLB_Ip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            SLB_Ip.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SLB_Ip.Name = "SLB_Ip";
            SLB_Ip.Size = new System.Drawing.Size(120, 24);
            SLB_Ip.Text = "000";
            SLB_Ip.ToolTipText = "接続先";
            // 
            // SLB_SendTask
            // 
            SLB_SendTask.Image = Properties.Resources.Icn_Sendtask;
            SLB_SendTask.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SLB_SendTask.Name = "SLB_SendTask";
            SLB_SendTask.Size = new System.Drawing.Size(24, 24);
            // 
            // BTN_SendTask
            // 
            BTN_SendTask.AutoSize = false;
            BTN_SendTask.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            BTN_SendTask.Image = (System.Drawing.Image)resources.GetObject("BTN_SendTask.Image");
            BTN_SendTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            BTN_SendTask.Name = "BTN_SendTask";
            BTN_SendTask.Size = new System.Drawing.Size(200, 27);
            BTN_SendTask.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            BTN_SendTask.ToolTipText = "リダイレクト先タスク";
            BTN_SendTask.DropDownOpening += BTN_SendTask_DropDownOpening;
            BTN_SendTask.DropDownItemClicked += BTN_SendTask_DropDownItemClicked;
            // 
            // Btn_Redirect
            // 
            Btn_Redirect.Image = Properties.Resources.Icn_Sendstop;
            Btn_Redirect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Btn_Redirect.Name = "Btn_Redirect";
            Btn_Redirect.Size = new System.Drawing.Size(52, 24);
            Btn_Redirect.Text = "OFF";
            Btn_Redirect.ToolTipText = "リダイレクトのONOFF";
            Btn_Redirect.Click += Btn_Redirect_Click;
            // 
            // Txt_Chat
            // 
            Txt_Chat.BackColor = System.Drawing.Color.Black;
            Txt_Chat.Dock = System.Windows.Forms.DockStyle.Fill;
            Txt_Chat.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            Txt_Chat.ForeColor = System.Drawing.Color.Lime;
            Txt_Chat.Location = new System.Drawing.Point(0, 31);
            Txt_Chat.Margin = new System.Windows.Forms.Padding(4);
            Txt_Chat.Multiline = true;
            Txt_Chat.Name = "Txt_Chat";
            Txt_Chat.ReadOnly = true;
            Txt_Chat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            Txt_Chat.Size = new System.Drawing.Size(740, 141);
            Txt_Chat.TabIndex = 0;
            Txt_Chat.WordWrap = false;
            // 
            // Tim_Status
            // 
            Tim_Status.Interval = 1000;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Btn_Clear, Btn_Setup, Btn_echa, Btn_List, toolStripSeparator1, toolStripLabel4, Cmb_Whitelist, toolStripLabel1, Cmb_User, toolStripLabel3, Cmb_Npc, toolStripSeparator2, toolStripButton1, toolStripButton2, toolStripButton3, toolStripButton4 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(740, 31);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // Btn_Clear
            // 
            Btn_Clear.Image = Properties.Resources.Icn_pageclear;
            Btn_Clear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Btn_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            Btn_Clear.Name = "Btn_Clear";
            Btn_Clear.Size = new System.Drawing.Size(28, 28);
            Btn_Clear.ToolTipText = "テキストクリア";
            Btn_Clear.Click += Btn_Clear_Click;
            // 
            // Btn_Setup
            // 
            Btn_Setup.Image = Properties.Resources.Icn_Settings;
            Btn_Setup.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Btn_Setup.ImageTransparentColor = System.Drawing.Color.Magenta;
            Btn_Setup.Name = "Btn_Setup";
            Btn_Setup.Size = new System.Drawing.Size(28, 28);
            Btn_Setup.ToolTipText = "各種設定";
            Btn_Setup.Click += Btn_Setup_Click;
            // 
            // Btn_echa
            // 
            Btn_echa.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            Btn_echa.Image = Properties.Resources.Icn_echa_off;
            Btn_echa.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Btn_echa.ImageTransparentColor = System.Drawing.Color.Magenta;
            Btn_echa.Name = "Btn_echa";
            Btn_echa.Size = new System.Drawing.Size(28, 28);
            Btn_echa.Text = "toolStripButton1";
            // 
            // Btn_List
            // 
            Btn_List.Image = Properties.Resources.Icn_UserList_;
            Btn_List.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Btn_List.ImageTransparentColor = System.Drawing.Color.Magenta;
            Btn_List.Name = "Btn_List";
            Btn_List.Size = new System.Drawing.Size(28, 28);
            Btn_List.ToolTipText = "キャラクターリスト";
            Btn_List.Click += Btn_List_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel4
            // 
            toolStripLabel4.Image = Properties.Resources.Icn_SelectUser;
            toolStripLabel4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripLabel4.Name = "toolStripLabel4";
            toolStripLabel4.Size = new System.Drawing.Size(24, 28);
            // 
            // Cmb_Whitelist
            // 
            Cmb_Whitelist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            Cmb_Whitelist.DropDownWidth = 60;
            Cmb_Whitelist.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            Cmb_Whitelist.Items.AddRange(new object[] { "OFF", "ChatOnly", "Voice" });
            Cmb_Whitelist.Name = "Cmb_Whitelist";
            Cmb_Whitelist.Size = new System.Drawing.Size(87, 31);
            Cmb_Whitelist.ToolTipText = "リストに登録したキャラのみ";
            Cmb_Whitelist.SelectedIndexChanged += Cmb_Whitelist_SelectedIndexChanged;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Image = Properties.Resources.Icn_User;
            toolStripLabel1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(54, 28);
            toolStripLabel1.Text = "User";
            // 
            // Cmb_User
            // 
            Cmb_User.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            Cmb_User.DropDownWidth = 60;
            Cmb_User.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            Cmb_User.Items.AddRange(new object[] { "OFF", "ChatOnly", "Voice 1", "Voice 2" });
            Cmb_User.Name = "Cmb_User";
            Cmb_User.Size = new System.Drawing.Size(87, 31);
            Cmb_User.ToolTipText = "一般ユーザーすべて";
            Cmb_User.SelectedIndexChanged += Cmb_User_SelectedIndexChanged;
            Cmb_User.Click += Cmb_User_Click;
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Image = Properties.Resources.Icn_NPC;
            toolStripLabel3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new System.Drawing.Size(53, 28);
            toolStripLabel3.Text = "Npc";
            // 
            // Cmb_Npc
            // 
            Cmb_Npc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            Cmb_Npc.DropDownWidth = 60;
            Cmb_Npc.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            Cmb_Npc.Items.AddRange(new object[] { "OFF", "ChatOnly", "Voice 1", "Voice 2" });
            Cmb_Npc.Name = "Cmb_Npc";
            Cmb_Npc.Size = new System.Drawing.Size(87, 31);
            Cmb_Npc.ToolTipText = "その他のキャラ";
            Cmb_Npc.SelectedIndexChanged += Cmb_Npc_SelectedIndexChanged;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(23, 28);
            toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new System.Drawing.Size(23, 28);
            toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton3.Image = (System.Drawing.Image)resources.GetObject("toolStripButton3.Image");
            toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new System.Drawing.Size(23, 28);
            toolStripButton3.Text = "toolStripButton3";
            // 
            // toolStripButton4
            // 
            toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton4.Image = (System.Drawing.Image)resources.GetObject("toolStripButton4.Image");
            toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new System.Drawing.Size(23, 28);
            toolStripButton4.Text = "toolStripButton4";
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "Icn_chatoff.png");
            imageList1.Images.SetKeyName(1, "Icn_chatonly.png");
            imageList1.Images.SetKeyName(2, "Icn_Voice.png");
            // 
            // Main
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(740, 201);
            Controls.Add(Txt_Chat);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4);
            MinimumSize = new System.Drawing.Size(752, 232);
            Name = "Main";
            StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Text = "MabiChatSpeech(ベータ5.2)";
            FormClosing += Main_FormClosing;
            FormClosed += Main_FormClosed;
            Load += Main_Load;
            Shown += Main_Shown;
            KeyDown += Main_KeyDown;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox Txt_Chat;
        private System.Windows.Forms.ToolStripStatusLabel SLB_Client;
        private System.Windows.Forms.ToolStripStatusLabel SLB_Ip;
        private System.Windows.Forms.Timer Tim_Status;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton Btn_Clear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox Cmb_User;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox Cmb_Npc;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripStatusLabel SLB_SaveMode;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripComboBox Cmb_Whitelist;
        private System.Windows.Forms.ToolStripButton Btn_List;
        private System.Windows.Forms.ToolStripButton Btn_Setup;
        private System.Windows.Forms.ToolStripDropDownButton BTN_SendTask;
        private System.Windows.Forms.ToolStripStatusLabel Btn_Redirect;
        private System.Windows.Forms.ToolStripStatusLabel SLB_Mode;
        private System.Windows.Forms.ToolStripButton Btn_echa;
        private System.Windows.Forms.ToolStripStatusLabel SLB_SendTask;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ImageList imageList1;
    }
}

