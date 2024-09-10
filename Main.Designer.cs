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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.SLB_SaveMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.SLB_Client = new System.Windows.Forms.ToolStripStatusLabel();
            this.SLB_Ip = new System.Windows.Forms.ToolStripStatusLabel();
            this.BTN_SendTask = new System.Windows.Forms.ToolStripDropDownButton();
            this.Btn_Redirect = new System.Windows.Forms.ToolStripStatusLabel();
            this.Txt_Chat = new System.Windows.Forms.TextBox();
            this.Tim_Status = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Btn_ChatList = new System.Windows.Forms.ToolStripButton();
            this.Btn_Clear = new System.Windows.Forms.ToolStripButton();
            this.Btn_LogView = new System.Windows.Forms.ToolStripButton();
            this.Btn_Setup = new System.Windows.Forms.ToolStripButton();
            this.Btn_List = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.Cmb_Whitelist = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.Cmb_User = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.Cmb_Npc = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.Cmb_Pet = new System.Windows.Forms.ToolStripComboBox();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SLB_SaveMode,
            this.SLB_Client,
            this.SLB_Ip,
            this.BTN_SendTask,
            this.Btn_Redirect});
            this.statusStrip1.Location = new System.Drawing.Point(0, 372);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(757, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // SLB_SaveMode
            // 
            this.SLB_SaveMode.AutoSize = false;
            this.SLB_SaveMode.Name = "SLB_SaveMode";
            this.SLB_SaveMode.Size = new System.Drawing.Size(100, 17);
            this.SLB_SaveMode.Text = "しない";
            this.SLB_SaveMode.ToolTipText = "チャットログの保存方法";
            // 
            // SLB_Client
            // 
            this.SLB_Client.AutoSize = false;
            this.SLB_Client.Name = "SLB_Client";
            this.SLB_Client.Size = new System.Drawing.Size(70, 17);
            this.SLB_Client.Text = "Online";
            this.SLB_Client.ToolTipText = "ステータス";
            // 
            // SLB_Ip
            // 
            this.SLB_Ip.AutoSize = false;
            this.SLB_Ip.Name = "SLB_Ip";
            this.SLB_Ip.Size = new System.Drawing.Size(120, 17);
            this.SLB_Ip.ToolTipText = "接続先";
            // 
            // BTN_SendTask
            // 
            this.BTN_SendTask.AutoSize = false;
            this.BTN_SendTask.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BTN_SendTask.Image = ((System.Drawing.Image)(resources.GetObject("BTN_SendTask.Image")));
            this.BTN_SendTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BTN_SendTask.Name = "BTN_SendTask";
            this.BTN_SendTask.Size = new System.Drawing.Size(200, 20);
            this.BTN_SendTask.Text = "リダイレクト先";
            this.BTN_SendTask.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_SendTask.DropDownOpening += new System.EventHandler(this.BTN_SendTask_DropDownOpening);
            this.BTN_SendTask.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.BTN_SendTask_DropDownItemClicked);
            // 
            // Btn_Redirect
            // 
            this.Btn_Redirect.Name = "Btn_Redirect";
            this.Btn_Redirect.Size = new System.Drawing.Size(28, 17);
            this.Btn_Redirect.Text = "OFF";
            this.Btn_Redirect.Click += new System.EventHandler(this.Btn_Redirect_Click);
            // 
            // Txt_Chat
            // 
            this.Txt_Chat.BackColor = System.Drawing.Color.Black;
            this.Txt_Chat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Txt_Chat.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Txt_Chat.ForeColor = System.Drawing.Color.Lime;
            this.Txt_Chat.Location = new System.Drawing.Point(0, 25);
            this.Txt_Chat.Multiline = true;
            this.Txt_Chat.Name = "Txt_Chat";
            this.Txt_Chat.ReadOnly = true;
            this.Txt_Chat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Txt_Chat.Size = new System.Drawing.Size(757, 347);
            this.Txt_Chat.TabIndex = 0;
            this.Txt_Chat.WordWrap = false;
            this.Txt_Chat.TextChanged += new System.EventHandler(this.Txt_Chat_TextChanged);
            // 
            // Tim_Status
            // 
            this.Tim_Status.Enabled = true;
            this.Tim_Status.Interval = 1000;
            this.Tim_Status.Tick += new System.EventHandler(this.Tim_Status_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Btn_ChatList,
            this.Btn_Clear,
            this.Btn_LogView,
            this.Btn_Setup,
            this.Btn_List,
            this.toolStripSeparator1,
            this.toolStripLabel4,
            this.Cmb_Whitelist,
            this.toolStripLabel1,
            this.Cmb_User,
            this.toolStripLabel3,
            this.Cmb_Npc,
            this.toolStripLabel2,
            this.Cmb_Pet});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(757, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // Btn_ChatList
            // 
            this.Btn_ChatList.Image = global::MabiChatSpeech.Properties.Resources.NewTeamProject;
            this.Btn_ChatList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_ChatList.Name = "Btn_ChatList";
            this.Btn_ChatList.Size = new System.Drawing.Size(38, 22);
            this.Btn_ChatList.Text = "ID";
            this.Btn_ChatList.ToolTipText = "ID";
            this.Btn_ChatList.Click += new System.EventHandler(this.Btn_Add_Click);
            // 
            // Btn_Clear
            // 
            this.Btn_Clear.Image = global::MabiChatSpeech.Properties.Resources.ClearWindowContent;
            this.Btn_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_Clear.Name = "Btn_Clear";
            this.Btn_Clear.Size = new System.Drawing.Size(53, 22);
            this.Btn_Clear.Text = "Clear";
            this.Btn_Clear.ToolTipText = "\r\n";
            this.Btn_Clear.Click += new System.EventHandler(this.Btn_Clear_Click);
            // 
            // Btn_LogView
            // 
            this.Btn_LogView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Btn_LogView.Image = global::MabiChatSpeech.Properties.Resources.imageres;
            this.Btn_LogView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_LogView.Name = "Btn_LogView";
            this.Btn_LogView.Size = new System.Drawing.Size(23, 22);
            this.Btn_LogView.Text = "toolStripButton2";
            this.Btn_LogView.Click += new System.EventHandler(this.Btn_LogView_Click);
            // 
            // Btn_Setup
            // 
            this.Btn_Setup.Image = global::MabiChatSpeech.Properties.Resources.SettingsPanel;
            this.Btn_Setup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_Setup.Name = "Btn_Setup";
            this.Btn_Setup.Size = new System.Drawing.Size(64, 22);
            this.Btn_Setup.Text = "Setting";
            this.Btn_Setup.Click += new System.EventHandler(this.Btn_Setup_Click);
            // 
            // Btn_List
            // 
            this.Btn_List.Image = global::MabiChatSpeech.Properties.Resources.TeamProject;
            this.Btn_List.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_List.Name = "Btn_List";
            this.Btn_List.Size = new System.Drawing.Size(45, 22);
            this.Btn_List.Text = "List";
            this.Btn_List.Click += new System.EventHandler(this.Btn_List_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel4.Text = "Choice";
            // 
            // Cmb_Whitelist
            // 
            this.Cmb_Whitelist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_Whitelist.DropDownWidth = 60;
            this.Cmb_Whitelist.Items.AddRange(new object[] {
            "OFF",
            "ChatOnly",
            "Voice"});
            this.Cmb_Whitelist.Name = "Cmb_Whitelist";
            this.Cmb_Whitelist.Size = new System.Drawing.Size(75, 25);
            this.Cmb_Whitelist.ToolTipText = "リストアップしたキャラ名のみ";
            this.Cmb_Whitelist.SelectedIndexChanged += new System.EventHandler(this.Cmb_Whitelist_SelectedIndexChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(30, 22);
            this.toolStripLabel1.Text = "User";
            // 
            // Cmb_User
            // 
            this.Cmb_User.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_User.DropDownWidth = 60;
            this.Cmb_User.Items.AddRange(new object[] {
            "OFF",
            "ChatOnly",
            "Voice 1",
            "Voice 2"});
            this.Cmb_User.Name = "Cmb_User";
            this.Cmb_User.Size = new System.Drawing.Size(75, 25);
            this.Cmb_User.ToolTipText = "一般ユーザー";
            this.Cmb_User.SelectedIndexChanged += new System.EventHandler(this.Cmb_User_SelectedIndexChanged);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(29, 22);
            this.toolStripLabel3.Text = "Npc";
            // 
            // Cmb_Npc
            // 
            this.Cmb_Npc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_Npc.DropDownWidth = 60;
            this.Cmb_Npc.Items.AddRange(new object[] {
            "OFF",
            "ChatOnly",
            "Voice 1",
            "Voice 2"});
            this.Cmb_Npc.Name = "Cmb_Npc";
            this.Cmb_Npc.Size = new System.Drawing.Size(75, 25);
            this.Cmb_Npc.ToolTipText = "Npc";
            this.Cmb_Npc.SelectedIndexChanged += new System.EventHandler(this.Cmb_Npc_SelectedIndexChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(37, 22);
            this.toolStripLabel2.Text = "Other";
            // 
            // Cmb_Pet
            // 
            this.Cmb_Pet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_Pet.DropDownWidth = 60;
            this.Cmb_Pet.Items.AddRange(new object[] {
            "OFF",
            "ChatOnly",
            "Voice 1",
            "Voice 2"});
            this.Cmb_Pet.Name = "Cmb_Pet";
            this.Cmb_Pet.Size = new System.Drawing.Size(75, 25);
            this.Cmb_Pet.ToolTipText = "ペット";
            this.Cmb_Pet.SelectedIndexChanged += new System.EventHandler(this.Cmb_Pet_SelectedIndexChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 394);
            this.Controls.Add(this.Txt_Chat);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Main";
            this.Text = "MabiChatSpeech(アルファ)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox Txt_Chat;
        private System.Windows.Forms.ToolStripStatusLabel SLB_Client;
        private System.Windows.Forms.ToolStripStatusLabel SLB_Ip;
        private System.Windows.Forms.Timer Tim_Status;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton Btn_ChatList;
        private System.Windows.Forms.ToolStripButton Btn_Clear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox Cmb_User;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox Cmb_Pet;
        private System.Windows.Forms.ToolStripComboBox Cmb_Npc;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripStatusLabel SLB_SaveMode;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripComboBox Cmb_Whitelist;
        private System.Windows.Forms.ToolStripButton Btn_List;
        private System.Windows.Forms.ToolStripButton Btn_Setup;
        private System.Windows.Forms.ToolStripButton Btn_LogView;
        private System.Windows.Forms.ToolStripDropDownButton BTN_SendTask;
        private System.Windows.Forms.ToolStripStatusLabel Btn_Redirect;
    }
}

