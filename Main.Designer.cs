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
            Txt_Chat = new System.Windows.Forms.TextBox();
            Tim_Status = new System.Windows.Forms.Timer(components);
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            Btn_Clear = new System.Windows.Forms.ToolStripButton();
            Btn_Setup = new System.Windows.Forms.ToolStripButton();
            Btn_List = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            SDB_SelectList = new System.Windows.Forms.ToolStripDropDownButton();
            toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            SDB_User = new System.Windows.Forms.ToolStripDropDownButton();
            toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            SDB_Npc = new System.Windows.Forms.ToolStripDropDownButton();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            Btn_echa = new System.Windows.Forms.ToolStripButton();
            BTN_No = new System.Windows.Forms.ToolStripButton();
            BTN_Time = new System.Windows.Forms.ToolStripButton();
            BTN_Type = new System.Windows.Forms.ToolStripButton();
            BTN_Name = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            BTN_Redirect = new System.Windows.Forms.ToolStripButton();
            SDB_SendTask = new System.Windows.Forms.ToolStripDropDownButton();
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { SLB_Mode, SLB_SaveMode, SLB_Client, SLB_Ip });
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
            SLB_Mode.AutoSize = false;
            SLB_Mode.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SLB_Mode.Name = "SLB_Mode";
            SLB_Mode.Size = new System.Drawing.Size(64, 24);
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
            SLB_Ip.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
            SLB_Ip.Image = Properties.Resources.icn_tarlach;
            SLB_Ip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            SLB_Ip.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SLB_Ip.Name = "SLB_Ip";
            SLB_Ip.Size = new System.Drawing.Size(120, 24);
            SLB_Ip.Text = "商店街";
            SLB_Ip.ToolTipText = "接続先";
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
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Btn_Clear, Btn_Setup, Btn_List, toolStripSeparator1, SDB_SelectList, SDB_User, SDB_Npc, toolStripSeparator2, Btn_echa, BTN_No, BTN_Time, BTN_Type, BTN_Name, toolStripSeparator3, BTN_Redirect, SDB_SendTask });
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
            Btn_Clear.ToolTipText = "表示クリア";
            Btn_Clear.Click += Btn_Clear_Click;
            // 
            // Btn_Setup
            // 
            Btn_Setup.Image = Properties.Resources.Icn_Settings;
            Btn_Setup.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Btn_Setup.ImageTransparentColor = System.Drawing.Color.Magenta;
            Btn_Setup.Name = "Btn_Setup";
            Btn_Setup.Size = new System.Drawing.Size(28, 28);
            Btn_Setup.ToolTipText = "設定";
            Btn_Setup.Click += Btn_Setup_Click;
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
            // SDB_SelectList
            // 
            SDB_SelectList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            SDB_SelectList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem9, toolStripMenuItem10, toolStripMenuItem11 });
            SDB_SelectList.Image = Properties.Resources.Icn_SelectUser;
            SDB_SelectList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SDB_SelectList.ImageTransparentColor = System.Drawing.Color.Magenta;
            SDB_SelectList.Name = "SDB_SelectList";
            SDB_SelectList.Size = new System.Drawing.Size(37, 28);
            SDB_SelectList.Text = "toolStripDropDownButton1";
            SDB_SelectList.ToolTipText = "キャラクターモード";
            SDB_SelectList.DropDownOpening += SDB_SelectList_DropDownOpening;
            SDB_SelectList.DropDownItemClicked += SDB_SelectList_DropDownItemClicked;
            SDB_SelectList.DoubleClick += SDB_SelectList_Click;
            // 
            // toolStripMenuItem9
            // 
            toolStripMenuItem9.Image = Properties.Resources.Icn_SelectUser_off;
            toolStripMenuItem9.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripMenuItem9.Name = "toolStripMenuItem9";
            toolStripMenuItem9.Size = new System.Drawing.Size(193, 30);
            toolStripMenuItem9.Tag = "0";
            toolStripMenuItem9.Text = "CharaMode Off";
            // 
            // toolStripMenuItem10
            // 
            toolStripMenuItem10.Image = Properties.Resources.Icn_SelectUser_chat;
            toolStripMenuItem10.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripMenuItem10.Name = "toolStripMenuItem10";
            toolStripMenuItem10.Size = new System.Drawing.Size(193, 30);
            toolStripMenuItem10.Tag = "1";
            toolStripMenuItem10.Text = "CharaMode Chatonly";
            // 
            // toolStripMenuItem11
            // 
            toolStripMenuItem11.Image = Properties.Resources.Icn_SelectUser_voice;
            toolStripMenuItem11.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripMenuItem11.Name = "toolStripMenuItem11";
            toolStripMenuItem11.Size = new System.Drawing.Size(193, 30);
            toolStripMenuItem11.Tag = "2";
            toolStripMenuItem11.Text = "CharaMode Voice";
            // 
            // SDB_User
            // 
            SDB_User.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            SDB_User.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem5, toolStripMenuItem6, toolStripMenuItem7, toolStripMenuItem8 });
            SDB_User.Image = Properties.Resources.Icn_User;
            SDB_User.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SDB_User.ImageTransparentColor = System.Drawing.Color.Magenta;
            SDB_User.Name = "SDB_User";
            SDB_User.Size = new System.Drawing.Size(37, 28);
            SDB_User.Text = "Player";
            SDB_User.DropDownOpening += SDB_User_DropDownOpening;
            SDB_User.DropDownItemClicked += SDB_User_DropDownItemClicked;
            SDB_User.DoubleClick += SDB_User_Click;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Image = Properties.Resources.Icn_User_off;
            toolStripMenuItem5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new System.Drawing.Size(188, 30);
            toolStripMenuItem5.Tag = "0";
            toolStripMenuItem5.Text = "Player Off";
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Image = Properties.Resources.Icn_User_chat;
            toolStripMenuItem6.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new System.Drawing.Size(188, 30);
            toolStripMenuItem6.Tag = "1";
            toolStripMenuItem6.Text = "Player Chatonly";
            // 
            // toolStripMenuItem7
            // 
            toolStripMenuItem7.Image = Properties.Resources.Icn_User_v1;
            toolStripMenuItem7.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripMenuItem7.Name = "toolStripMenuItem7";
            toolStripMenuItem7.Size = new System.Drawing.Size(188, 30);
            toolStripMenuItem7.Tag = "2";
            toolStripMenuItem7.Text = "Player Voice1";
            // 
            // toolStripMenuItem8
            // 
            toolStripMenuItem8.Image = Properties.Resources.Icn_User_v2;
            toolStripMenuItem8.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripMenuItem8.Name = "toolStripMenuItem8";
            toolStripMenuItem8.Size = new System.Drawing.Size(188, 30);
            toolStripMenuItem8.Tag = "3";
            toolStripMenuItem8.Text = "Player Voice2";
            // 
            // SDB_Npc
            // 
            SDB_Npc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            SDB_Npc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4 });
            SDB_Npc.Image = Properties.Resources.Icn_NPC;
            SDB_Npc.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SDB_Npc.ImageTransparentColor = System.Drawing.Color.Magenta;
            SDB_Npc.Name = "SDB_Npc";
            SDB_Npc.Size = new System.Drawing.Size(37, 28);
            SDB_Npc.Text = "NPC";
            SDB_Npc.DropDownOpening += SDB_Npc_DropDownOpening;
            SDB_Npc.DropDownItemClicked += SDB_Npc_DropDownItemClicked;
            SDB_Npc.DoubleClick += SDB_Npc_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Image = Properties.Resources.Icn_Npc_off;
            toolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(155, 30);
            toolStripMenuItem1.Tag = "0";
            toolStripMenuItem1.Text = "NPC Off";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Image = Properties.Resources.Icn_Npc_chat;
            toolStripMenuItem2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(155, 30);
            toolStripMenuItem2.Tag = "1";
            toolStripMenuItem2.Text = "NPC Chatonly";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Image = Properties.Resources.Icn_Npc_v1;
            toolStripMenuItem3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(155, 30);
            toolStripMenuItem3.Tag = "2";
            toolStripMenuItem3.Text = "NPC Voice1";
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Image = Properties.Resources.Icn_Npc_v2;
            toolStripMenuItem4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(155, 30);
            toolStripMenuItem4.Tag = "3";
            toolStripMenuItem4.Text = "NPC Voice2";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
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
            Btn_echa.ToolTipText = "Echa";
            Btn_echa.Click += Btn_echa_Click;
            // 
            // BTN_No
            // 
            BTN_No.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            BTN_No.Image = Properties.Resources.Icn_ViewSwtch_No_off;
            BTN_No.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            BTN_No.ImageTransparentColor = System.Drawing.Color.Magenta;
            BTN_No.Name = "BTN_No";
            BTN_No.Size = new System.Drawing.Size(28, 28);
            BTN_No.Text = "toolStripButton1";
            BTN_No.ToolTipText = "Display SeqNo";
            BTN_No.Click += BTN_No_Click;
            // 
            // BTN_Time
            // 
            BTN_Time.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            BTN_Time.Image = Properties.Resources.Icn_ViewSwtch_Time_off;
            BTN_Time.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            BTN_Time.ImageTransparentColor = System.Drawing.Color.Magenta;
            BTN_Time.Name = "BTN_Time";
            BTN_Time.Size = new System.Drawing.Size(28, 28);
            BTN_Time.Text = "toolStripButton2";
            BTN_Time.ToolTipText = "Display Time";
            BTN_Time.Click += BTN_Time_Click;
            // 
            // BTN_Type
            // 
            BTN_Type.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            BTN_Type.Image = Properties.Resources.Icn_ViewSwtch_Type_off;
            BTN_Type.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            BTN_Type.ImageTransparentColor = System.Drawing.Color.Magenta;
            BTN_Type.Name = "BTN_Type";
            BTN_Type.Size = new System.Drawing.Size(28, 28);
            BTN_Type.Text = "toolStripButton3";
            BTN_Type.ToolTipText = "Display Type";
            BTN_Type.Click += BTN_Type_Click;
            // 
            // BTN_Name
            // 
            BTN_Name.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            BTN_Name.Image = Properties.Resources.Icn_ViewSwtch_Name_off;
            BTN_Name.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            BTN_Name.ImageTransparentColor = System.Drawing.Color.Magenta;
            BTN_Name.Name = "BTN_Name";
            BTN_Name.Size = new System.Drawing.Size(28, 28);
            BTN_Name.Text = "toolStripButton4";
            BTN_Name.ToolTipText = "Display CharaName";
            BTN_Name.Click += BTN_Name_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // BTN_Redirect
            // 
            BTN_Redirect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            BTN_Redirect.Image = Properties.Resources.Icn_Sendstop;
            BTN_Redirect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            BTN_Redirect.ImageTransparentColor = System.Drawing.Color.Magenta;
            BTN_Redirect.Name = "BTN_Redirect";
            BTN_Redirect.Size = new System.Drawing.Size(28, 28);
            BTN_Redirect.Text = "toolStripDropDownButton1";
            BTN_Redirect.ToolTipText = "Redirect Action";
            BTN_Redirect.Click += BTN_Redirect_Click;
            // 
            // SDB_SendTask
            // 
            SDB_SendTask.AutoSize = false;
            SDB_SendTask.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            SDB_SendTask.Image = (System.Drawing.Image)resources.GetObject("SDB_SendTask.Image");
            SDB_SendTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            SDB_SendTask.Name = "SDB_SendTask";
            SDB_SendTask.Size = new System.Drawing.Size(250, 28);
            SDB_SendTask.DropDownOpening += SDB_SendTask_DropDownOpening;
            SDB_SendTask.DropDownItemClicked += SDB_SendTask_DropDownItemClicked;
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
            MinimumSize = new System.Drawing.Size(750, 228);
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
        private System.Windows.Forms.ToolStripStatusLabel SLB_SaveMode;
        private System.Windows.Forms.ToolStripButton Btn_List;
        private System.Windows.Forms.ToolStripButton Btn_Setup;
        private System.Windows.Forms.ToolStripButton BTN_Redirect;
        private System.Windows.Forms.ToolStripStatusLabel SLB_Mode;
        private System.Windows.Forms.ToolStripButton Btn_echa;
        private System.Windows.Forms.ToolStripStatusLabel SLB_SendTask;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton BTN_No;
        private System.Windows.Forms.ToolStripButton BTN_Time;
        private System.Windows.Forms.ToolStripButton BTN_Type;
        private System.Windows.Forms.ToolStripButton BTN_Name;
        private System.Windows.Forms.ToolStripDropDownButton SDB_Npc;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripDropDownButton SDB_User;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripDropDownButton SDB_SelectList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripDropDownButton SDB_SendTask;
    }
}

