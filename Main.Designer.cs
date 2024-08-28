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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MNI_File = new System.Windows.Forms.ToolStripMenuItem();
            this.MNI_Quit = new System.Windows.Forms.ToolStripMenuItem();
            this.MNI_Setting = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.SLB_SaveMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.SLB_Client = new System.Windows.Forms.ToolStripStatusLabel();
            this.SLB_Ip = new System.Windows.Forms.ToolStripStatusLabel();
            this.Txt_Chat = new System.Windows.Forms.TextBox();
            this.Tim_Status = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Btn_Add = new System.Windows.Forms.ToolStripButton();
            this.Btn_Clear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.Cmb_Whitelist = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.Cmb_User = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.Cmb_Pet = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.Cmb_Npc = new System.Windows.Forms.ToolStripComboBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MNI_File,
            this.MNI_Setting});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(541, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MNI_File
            // 
            this.MNI_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MNI_Quit});
            this.MNI_File.Name = "MNI_File";
            this.MNI_File.Size = new System.Drawing.Size(67, 20);
            this.MNI_File.Text = "ファイル(&F)";
            // 
            // MNI_Quit
            // 
            this.MNI_Quit.Name = "MNI_Quit";
            this.MNI_Quit.Size = new System.Drawing.Size(115, 22);
            this.MNI_Quit.Text = "終了(&Q)";
            this.MNI_Quit.Click += new System.EventHandler(this.MNI_Quit_Click);
            // 
            // MNI_Setting
            // 
            this.MNI_Setting.Name = "MNI_Setting";
            this.MNI_Setting.Size = new System.Drawing.Size(57, 20);
            this.MNI_Setting.Text = "設定(&E)";
            this.MNI_Setting.Click += new System.EventHandler(this.MNI_Setting_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SLB_SaveMode,
            this.SLB_Client,
            this.SLB_Ip});
            this.statusStrip1.Location = new System.Drawing.Point(0, 257);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(541, 22);
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
            // Txt_Chat
            // 
            this.Txt_Chat.BackColor = System.Drawing.Color.Black;
            this.Txt_Chat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Txt_Chat.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Txt_Chat.ForeColor = System.Drawing.Color.Lime;
            this.Txt_Chat.Location = new System.Drawing.Point(0, 49);
            this.Txt_Chat.Multiline = true;
            this.Txt_Chat.Name = "Txt_Chat";
            this.Txt_Chat.ReadOnly = true;
            this.Txt_Chat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Txt_Chat.Size = new System.Drawing.Size(541, 208);
            this.Txt_Chat.TabIndex = 0;
            this.Txt_Chat.WordWrap = false;
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
            this.Btn_Add,
            this.Btn_Clear,
            this.toolStripSeparator1,
            this.toolStripLabel4,
            this.Cmb_Whitelist,
            this.toolStripLabel1,
            this.Cmb_User,
            this.toolStripLabel2,
            this.Cmb_Pet,
            this.toolStripLabel3,
            this.Cmb_Npc});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(541, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // Btn_Add
            // 
            this.Btn_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Btn_Add.Image = ((System.Drawing.Image)(resources.GetObject("Btn_Add.Image")));
            this.Btn_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_Add.Name = "Btn_Add";
            this.Btn_Add.Size = new System.Drawing.Size(23, 22);
            this.Btn_Add.Text = "A";
            this.Btn_Add.Visible = false;
            this.Btn_Add.Click += new System.EventHandler(this.Btn_Add_Click);
            // 
            // Btn_Clear
            // 
            this.Btn_Clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Btn_Clear.Image = ((System.Drawing.Image)(resources.GetObject("Btn_Clear.Image")));
            this.Btn_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_Clear.Name = "Btn_Clear";
            this.Btn_Clear.Size = new System.Drawing.Size(61, 22);
            this.Btn_Clear.Text = "ChatClear";
            this.Btn_Clear.ToolTipText = "チャットをクリア";
            this.Btn_Clear.Click += new System.EventHandler(this.Btn_Clear_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabel4.Text = "指定";
            // 
            // Cmb_Whitelist
            // 
            this.Cmb_Whitelist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_Whitelist.DropDownWidth = 60;
            this.Cmb_Whitelist.Items.AddRange(new object[] {
            "OFF",
            "チャットのみ",
            "音声"});
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
            "チャットのみ",
            "音声１",
            "音声２"});
            this.Cmb_User.Name = "Cmb_User";
            this.Cmb_User.Size = new System.Drawing.Size(75, 25);
            this.Cmb_User.ToolTipText = "一般ユーザー";
            this.Cmb_User.SelectedIndexChanged += new System.EventHandler(this.Cmb_User_SelectedIndexChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(24, 22);
            this.toolStripLabel2.Text = "Pet";
            // 
            // Cmb_Pet
            // 
            this.Cmb_Pet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_Pet.DropDownWidth = 60;
            this.Cmb_Pet.Items.AddRange(new object[] {
            "OFF",
            "チャットのみ",
            "音声１",
            "音声２"});
            this.Cmb_Pet.Name = "Cmb_Pet";
            this.Cmb_Pet.Size = new System.Drawing.Size(75, 25);
            this.Cmb_Pet.ToolTipText = "ペット";
            this.Cmb_Pet.SelectedIndexChanged += new System.EventHandler(this.Cmb_Pet_SelectedIndexChanged);
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
            "チャットのみ",
            "音声１",
            "音声２"});
            this.Cmb_Npc.Name = "Cmb_Npc";
            this.Cmb_Npc.Size = new System.Drawing.Size(75, 25);
            this.Cmb_Npc.ToolTipText = "Npc";
            this.Cmb_Npc.SelectedIndexChanged += new System.EventHandler(this.Cmb_Npc_SelectedIndexChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 279);
            this.Controls.Add(this.Txt_Chat);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "MabiChatSpeech(アルファ)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox Txt_Chat;
        private System.Windows.Forms.ToolStripMenuItem MNI_File;
        private System.Windows.Forms.ToolStripMenuItem MNI_Quit;
        private System.Windows.Forms.ToolStripMenuItem MNI_Setting;
        private System.Windows.Forms.ToolStripStatusLabel SLB_Client;
        private System.Windows.Forms.ToolStripStatusLabel SLB_Ip;
        private System.Windows.Forms.Timer Tim_Status;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton Btn_Add;
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
    }
}

