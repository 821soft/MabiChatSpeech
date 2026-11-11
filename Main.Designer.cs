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
            SSL_VoiceVox = new System.Windows.Forms.ToolStripStatusLabel();
            SSL_AivisSpeech = new System.Windows.Forms.ToolStripStatusLabel();
            SSL_VoiceAPI = new System.Windows.Forms.ToolStripStatusLabel();
            SSL_PC = new System.Windows.Forms.ToolStripStatusLabel();
            SSL_NPC = new System.Windows.Forms.ToolStripStatusLabel();
            SSL_CharaList = new System.Windows.Forms.ToolStripStatusLabel();
            SSL_Echa = new System.Windows.Forms.ToolStripStatusLabel();
            SSL_Youtube = new System.Windows.Forms.ToolStripStatusLabel();
            TPB_Save = new System.Windows.Forms.ToolStripProgressBar();
            TPB_Max = new System.Windows.Forms.ToolStripStatusLabel();
            TPB_Event = new System.Windows.Forms.ToolStripStatusLabel();
            Txt_Chat = new System.Windows.Forms.TextBox();
            Tim_Status = new System.Windows.Forms.Timer(components);
            LSV_chat = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader6 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            Mnu_File = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_SystemSetiing = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_Export = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_Replay = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_Quit = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_Edit = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            Mnu_PC = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_PC_Chat = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_PC_TTS = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_NPC = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_NPC_Chat = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_NPC_TTS = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_CharaList = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_CharaList_Chat = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_CharaList_TTS = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_CharaList_Voice = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            Mnu_CharList_Edit = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_Echa = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            Mnu_Clear = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_View = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_ViewText = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_ViewDump = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_ViewList = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            Mnu_ViewList_No = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_ViewList_Time = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_ViewList_Type = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_ViewList_Name = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            Mnu_YoutubeLive = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            Mnu_Help = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.AutoSize = false;
            statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { SLB_Mode, SLB_SaveMode, SLB_Client, SLB_Ip, SSL_VoiceVox, SSL_AivisSpeech, SSL_VoiceAPI, SSL_PC, SSL_NPC, SSL_CharaList, SSL_Echa, SSL_Youtube, TPB_Save, TPB_Max, TPB_Event });
            statusStrip1.Location = new System.Drawing.Point(0, 462);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 22, 0);
            statusStrip1.ShowItemToolTips = true;
            statusStrip1.Size = new System.Drawing.Size(1486, 72);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // SLB_Mode
            // 
            SLB_Mode.AutoSize = false;
            SLB_Mode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            SLB_Mode.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SLB_Mode.Name = "SLB_Mode";
            SLB_Mode.Size = new System.Drawing.Size(100, 66);
            SLB_Mode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            SLB_Mode.ToolTipText = "Mode";
            // 
            // SLB_SaveMode
            // 
            SLB_SaveMode.AutoSize = false;
            SLB_SaveMode.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SLB_SaveMode.Name = "SLB_SaveMode";
            SLB_SaveMode.Size = new System.Drawing.Size(100, 66);
            SLB_SaveMode.ToolTipText = "チャットログの保存方法";
            // 
            // SLB_Client
            // 
            SLB_Client.AutoSize = false;
            SLB_Client.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SLB_Client.Name = "SLB_Client";
            SLB_Client.Size = new System.Drawing.Size(100, 66);
            SLB_Client.ToolTipText = "接続状態";
            // 
            // SLB_Ip
            // 
            SLB_Ip.AutoSize = false;
            SLB_Ip.Font = new System.Drawing.Font("Meiryo UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
            SLB_Ip.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SLB_Ip.Name = "SLB_Ip";
            SLB_Ip.Size = new System.Drawing.Size(100, 66);
            SLB_Ip.Text = "商店街";
            SLB_Ip.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            SLB_Ip.ToolTipText = "接続先";
            // 
            // SSL_VoiceVox
            // 
            SSL_VoiceVox.Image = Properties.Resources.Icon_vvpp_Disable;
            SSL_VoiceVox.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SSL_VoiceVox.Name = "SSL_VoiceVox";
            SSL_VoiceVox.Size = new System.Drawing.Size(54, 66);
            SSL_VoiceVox.Text = "未設定";
            SSL_VoiceVox.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            SSL_VoiceVox.Click += SSL_VoiceVox_Click;
            // 
            // SSL_AivisSpeech
            // 
            SSL_AivisSpeech.Image = Properties.Resources.Icon_aisp_Disable;
            SSL_AivisSpeech.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SSL_AivisSpeech.LinkVisited = true;
            SSL_AivisSpeech.Name = "SSL_AivisSpeech";
            SSL_AivisSpeech.Size = new System.Drawing.Size(54, 66);
            SSL_AivisSpeech.Text = "未設定";
            SSL_AivisSpeech.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            SSL_AivisSpeech.Click += SSL_AivisSpeech_Click;
            // 
            // SSL_VoiceAPI
            // 
            SSL_VoiceAPI.AutoSize = false;
            SSL_VoiceAPI.Font = new System.Drawing.Font("Yu Gothic UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            SSL_VoiceAPI.Name = "SSL_VoiceAPI";
            SSL_VoiceAPI.Size = new System.Drawing.Size(120, 66);
            SSL_VoiceAPI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            SSL_VoiceAPI.Click += SSL_VoiceAPI_Click;
            // 
            // SSL_PC
            // 
            SSL_PC.Image = Properties.Resources.Icon_PC_Chat_Dis;
            SSL_PC.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SSL_PC.Name = "SSL_PC";
            SSL_PC.Size = new System.Drawing.Size(32, 66);
            // 
            // SSL_NPC
            // 
            SSL_NPC.Image = Properties.Resources.Icon_NPC_Chat_Dis;
            SSL_NPC.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SSL_NPC.Name = "SSL_NPC";
            SSL_NPC.Size = new System.Drawing.Size(32, 66);
            // 
            // SSL_CharaList
            // 
            SSL_CharaList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            SSL_CharaList.Font = new System.Drawing.Font("Yu Gothic UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            SSL_CharaList.Image = Properties.Resources.Icon_CharaList_Chat_Dis;
            SSL_CharaList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SSL_CharaList.LinkVisited = true;
            SSL_CharaList.Name = "SSL_CharaList";
            SSL_CharaList.Size = new System.Drawing.Size(32, 66);
            SSL_CharaList.Click += SSL_CharaList_Click;
            // 
            // SSL_Echa
            // 
            SSL_Echa.Image = Properties.Resources.Icon_EchaOff;
            SSL_Echa.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SSL_Echa.Name = "SSL_Echa";
            SSL_Echa.Size = new System.Drawing.Size(32, 66);
            // 
            // SSL_Youtube
            // 
            SSL_Youtube.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            SSL_Youtube.Name = "SSL_Youtube";
            SSL_Youtube.Size = new System.Drawing.Size(0, 66);
            // 
            // TPB_Save
            // 
            TPB_Save.Maximum = int.MaxValue;
            TPB_Save.Name = "TPB_Save";
            TPB_Save.Size = new System.Drawing.Size(101, 64);
            // 
            // TPB_Max
            // 
            TPB_Max.AutoSize = false;
            TPB_Max.Name = "TPB_Max";
            TPB_Max.Size = new System.Drawing.Size(169, 66);
            // 
            // TPB_Event
            // 
            TPB_Event.Name = "TPB_Event";
            TPB_Event.Size = new System.Drawing.Size(396, 66);
            TPB_Event.Spring = true;
            // 
            // Txt_Chat
            // 
            Txt_Chat.BackColor = System.Drawing.Color.Black;
            Txt_Chat.Dock = System.Windows.Forms.DockStyle.Fill;
            Txt_Chat.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            Txt_Chat.ForeColor = System.Drawing.Color.Lime;
            Txt_Chat.Location = new System.Drawing.Point(0, 37);
            Txt_Chat.Margin = new System.Windows.Forms.Padding(6);
            Txt_Chat.Multiline = true;
            Txt_Chat.Name = "Txt_Chat";
            Txt_Chat.ReadOnly = true;
            Txt_Chat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            Txt_Chat.Size = new System.Drawing.Size(1486, 425);
            Txt_Chat.TabIndex = 0;
            Txt_Chat.WordWrap = false;
            // 
            // Tim_Status
            // 
            Tim_Status.Enabled = true;
            Tim_Status.Interval = 3000;
            Tim_Status.Tick += Tim_Status_Tick;
            // 
            // LSV_chat
            // 
            LSV_chat.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader6, columnHeader5 });
            LSV_chat.Dock = System.Windows.Forms.DockStyle.Fill;
            LSV_chat.Font = new System.Drawing.Font("Yu Gothic UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            LSV_chat.FullRowSelect = true;
            LSV_chat.GridLines = true;
            LSV_chat.Location = new System.Drawing.Point(0, 37);
            LSV_chat.Margin = new System.Windows.Forms.Padding(4);
            LSV_chat.MultiSelect = false;
            LSV_chat.Name = "LSV_chat";
            LSV_chat.Size = new System.Drawing.Size(1486, 425);
            LSV_chat.TabIndex = 2;
            LSV_chat.UseCompatibleStateImageBehavior = false;
            LSV_chat.View = System.Windows.Forms.View.Details;
            LSV_chat.Click += LSV_chat_Click;
            LSV_chat.DoubleClick += LSV_chat_DoubleClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "No";
            columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Time";
            columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "種別";
            columnHeader3.Width = 50;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "キャラ名";
            columnHeader4.Width = 150;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "音声";
            columnHeader6.Width = 80;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "チャット内容";
            columnHeader5.Width = 710;
            // 
            // menuStrip1
            // 
            menuStrip1.Font = new System.Drawing.Font("Yu Gothic UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Mnu_File, Mnu_Edit, Mnu_View });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(9, 4, 0, 4);
            menuStrip1.Size = new System.Drawing.Size(1486, 37);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            // 
            // Mnu_File
            // 
            Mnu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Mnu_SystemSetiing, Mnu_Export, Mnu_Replay, Mnu_Quit });
            Mnu_File.Name = "Mnu_File";
            Mnu_File.Size = new System.Drawing.Size(96, 29);
            Mnu_File.Text = "ファイル(&F)";
            // 
            // Mnu_SystemSetiing
            // 
            Mnu_SystemSetiing.Image = Properties.Resources.Icon_Setting;
            Mnu_SystemSetiing.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_SystemSetiing.Name = "Mnu_SystemSetiing";
            Mnu_SystemSetiing.Size = new System.Drawing.Size(212, 38);
            Mnu_SystemSetiing.Text = "設定";
            Mnu_SystemSetiing.Click += Mnu_SystemSetiing_Click;
            // 
            // Mnu_Export
            // 
            Mnu_Export.Image = Properties.Resources.Icon_Export;
            Mnu_Export.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_Export.Name = "Mnu_Export";
            Mnu_Export.Size = new System.Drawing.Size(212, 38);
            Mnu_Export.Text = "エクスポート(&X)";
            Mnu_Export.Click += Mnu_Export_Click;
            // 
            // Mnu_Replay
            // 
            Mnu_Replay.Image = Properties.Resources.Icon_Replay;
            Mnu_Replay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_Replay.Name = "Mnu_Replay";
            Mnu_Replay.Size = new System.Drawing.Size(212, 38);
            Mnu_Replay.Text = "リプレイ(&R)";
            Mnu_Replay.Click += Mnu_Replay_Click;
            // 
            // Mnu_Quit
            // 
            Mnu_Quit.Image = Properties.Resources.Icon_AppExit;
            Mnu_Quit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_Quit.Name = "Mnu_Quit";
            Mnu_Quit.Size = new System.Drawing.Size(212, 38);
            Mnu_Quit.Text = "終了(&Q)";
            Mnu_Quit.Click += Mnu_Quit_Click;
            // 
            // Mnu_Edit
            // 
            Mnu_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripSeparator7, Mnu_PC, Mnu_NPC, Mnu_CharaList, toolStripSeparator8, Mnu_CharList_Edit, Mnu_Echa, toolStripSeparator4, Mnu_Clear });
            Mnu_Edit.Name = "Mnu_Edit";
            Mnu_Edit.Size = new System.Drawing.Size(81, 29);
            Mnu_Edit.Text = "編集(&E)";
            Mnu_Edit.DropDownClosed += Mnu_Edit_DropDownClosed;
            Mnu_Edit.DropDownOpening += Mnu_Edit_DropDownOpening;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(220, 6);
            // 
            // Mnu_PC
            // 
            Mnu_PC.Checked = true;
            Mnu_PC.CheckState = System.Windows.Forms.CheckState.Checked;
            Mnu_PC.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Mnu_PC_Chat, Mnu_PC_TTS });
            Mnu_PC.Image = Properties.Resources.Icon_PC_Chat;
            Mnu_PC.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_PC.Name = "Mnu_PC";
            Mnu_PC.Size = new System.Drawing.Size(223, 38);
            Mnu_PC.Tag = "0";
            Mnu_PC.Text = "PC";
            Mnu_PC.CheckedChanged += Mnu_PC_CheckedChanged;
            Mnu_PC.Click += Mnu_PC_Click;
            // 
            // Mnu_PC_Chat
            // 
            Mnu_PC_Chat.Checked = true;
            Mnu_PC_Chat.CheckState = System.Windows.Forms.CheckState.Checked;
            Mnu_PC_Chat.Image = Properties.Resources.Icon_PC_Chat;
            Mnu_PC_Chat.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_PC_Chat.Name = "Mnu_PC_Chat";
            Mnu_PC_Chat.Size = new System.Drawing.Size(146, 38);
            Mnu_PC_Chat.Text = "Chat";
            Mnu_PC_Chat.Click += Mnu_PC_Chat_Click;
            // 
            // Mnu_PC_TTS
            // 
            Mnu_PC_TTS.Image = Properties.Resources.Icon_PC_TTS;
            Mnu_PC_TTS.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_PC_TTS.Name = "Mnu_PC_TTS";
            Mnu_PC_TTS.Size = new System.Drawing.Size(146, 38);
            Mnu_PC_TTS.Text = "TTS";
            Mnu_PC_TTS.Click += Mnu_PC_TTS_Click;
            // 
            // Mnu_NPC
            // 
            Mnu_NPC.Checked = true;
            Mnu_NPC.CheckState = System.Windows.Forms.CheckState.Checked;
            Mnu_NPC.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Mnu_NPC_Chat, Mnu_NPC_TTS });
            Mnu_NPC.Image = Properties.Resources.Icon_NPC_Chat;
            Mnu_NPC.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_NPC.Name = "Mnu_NPC";
            Mnu_NPC.Size = new System.Drawing.Size(223, 38);
            Mnu_NPC.Text = "NPC";
            Mnu_NPC.CheckedChanged += Mnu_NPC_CheckedChanged;
            Mnu_NPC.Click += Mnu_NPC_Click;
            // 
            // Mnu_NPC_Chat
            // 
            Mnu_NPC_Chat.Checked = true;
            Mnu_NPC_Chat.CheckState = System.Windows.Forms.CheckState.Checked;
            Mnu_NPC_Chat.Image = Properties.Resources.Icon_NPC_Chat;
            Mnu_NPC_Chat.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_NPC_Chat.Name = "Mnu_NPC_Chat";
            Mnu_NPC_Chat.Size = new System.Drawing.Size(146, 38);
            Mnu_NPC_Chat.Text = "Chat";
            Mnu_NPC_Chat.Click += Mnu_NPC_Chat_Click;
            // 
            // Mnu_NPC_TTS
            // 
            Mnu_NPC_TTS.Image = Properties.Resources.Icon_NPC_TTS;
            Mnu_NPC_TTS.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_NPC_TTS.Name = "Mnu_NPC_TTS";
            Mnu_NPC_TTS.Size = new System.Drawing.Size(146, 38);
            Mnu_NPC_TTS.Text = "TTS";
            Mnu_NPC_TTS.Click += Mnu_NPC_TTS_Click;
            // 
            // Mnu_CharaList
            // 
            Mnu_CharaList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Mnu_CharaList_Chat, Mnu_CharaList_TTS, Mnu_CharaList_Voice });
            Mnu_CharaList.Image = Properties.Resources.Icon_CharaList_Chat;
            Mnu_CharaList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_CharaList.Name = "Mnu_CharaList";
            Mnu_CharaList.Size = new System.Drawing.Size(223, 38);
            Mnu_CharaList.Tag = "1";
            Mnu_CharaList.Text = "キャラリスト";
            Mnu_CharaList.CheckedChanged += Mnu_CharaList_CheckedChanged;
            Mnu_CharaList.Click += Mnu_CharaList_Click;
            // 
            // Mnu_CharaList_Chat
            // 
            Mnu_CharaList_Chat.Checked = true;
            Mnu_CharaList_Chat.CheckState = System.Windows.Forms.CheckState.Checked;
            Mnu_CharaList_Chat.Image = Properties.Resources.Icon_CharaList_Chat;
            Mnu_CharaList_Chat.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_CharaList_Chat.Name = "Mnu_CharaList_Chat";
            Mnu_CharaList_Chat.Size = new System.Drawing.Size(152, 38);
            Mnu_CharaList_Chat.Text = "Chat";
            Mnu_CharaList_Chat.Click += Mnu_CharaList_Chat_Click;
            // 
            // Mnu_CharaList_TTS
            // 
            Mnu_CharaList_TTS.Image = Properties.Resources.Icon_CharaList_TTS;
            Mnu_CharaList_TTS.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_CharaList_TTS.Name = "Mnu_CharaList_TTS";
            Mnu_CharaList_TTS.Size = new System.Drawing.Size(152, 38);
            Mnu_CharaList_TTS.Text = "TTS";
            Mnu_CharaList_TTS.Click += Mnu_CharaList_TTS_Click;
            // 
            // Mnu_CharaList_Voice
            // 
            Mnu_CharaList_Voice.Image = Properties.Resources.Icon_CharaList_VOICE;
            Mnu_CharaList_Voice.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_CharaList_Voice.Name = "Mnu_CharaList_Voice";
            Mnu_CharaList_Voice.Size = new System.Drawing.Size(152, 38);
            Mnu_CharaList_Voice.Text = "Voice";
            Mnu_CharaList_Voice.Click += Mnu_CharaList_Voice_Click;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new System.Drawing.Size(220, 6);
            // 
            // Mnu_CharList_Edit
            // 
            Mnu_CharList_Edit.Image = Properties.Resources.Icon_CharaListEdit;
            Mnu_CharList_Edit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_CharList_Edit.Name = "Mnu_CharList_Edit";
            Mnu_CharList_Edit.Size = new System.Drawing.Size(223, 38);
            Mnu_CharList_Edit.Text = "キャラリスト編集";
            Mnu_CharList_Edit.Click += Mnu_CharList_Edit_Click;
            // 
            // Mnu_Echa
            // 
            Mnu_Echa.Checked = true;
            Mnu_Echa.CheckOnClick = true;
            Mnu_Echa.CheckState = System.Windows.Forms.CheckState.Checked;
            Mnu_Echa.Image = Properties.Resources.Icon_EchaOn;
            Mnu_Echa.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_Echa.Name = "Mnu_Echa";
            Mnu_Echa.Size = new System.Drawing.Size(223, 38);
            Mnu_Echa.Text = "絵チャ保存(&P)";
            Mnu_Echa.CheckedChanged += Mnu_Echa_CheckedChanged;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(220, 6);
            // 
            // Mnu_Clear
            // 
            Mnu_Clear.Image = Properties.Resources.Icon_ScreenClear;
            Mnu_Clear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_Clear.Name = "Mnu_Clear";
            Mnu_Clear.Size = new System.Drawing.Size(223, 38);
            Mnu_Clear.Text = "クリア(&C)";
            Mnu_Clear.Click += Btn_Clear_Click;
            // 
            // Mnu_View
            // 
            Mnu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Mnu_ViewText, Mnu_ViewDump, Mnu_ViewList, toolStripSeparator5, Mnu_ViewList_No, Mnu_ViewList_Time, Mnu_ViewList_Type, Mnu_ViewList_Name, toolStripSeparator6, Mnu_YoutubeLive, toolStripSeparator9, Mnu_Help });
            Mnu_View.Name = "Mnu_View";
            Mnu_View.Size = new System.Drawing.Size(83, 29);
            Mnu_View.Text = "表示(V)";
            // 
            // Mnu_ViewText
            // 
            Mnu_ViewText.Name = "Mnu_ViewText";
            Mnu_ViewText.Size = new System.Drawing.Size(226, 38);
            Mnu_ViewText.Text = "テキスト表示(&T)";
            Mnu_ViewText.Click += Mnu_ViewText_Click;
            // 
            // Mnu_ViewDump
            // 
            Mnu_ViewDump.Name = "Mnu_ViewDump";
            Mnu_ViewDump.Size = new System.Drawing.Size(226, 38);
            Mnu_ViewDump.Text = "Dump表示(&D)";
            Mnu_ViewDump.Click += Mnu_ViewDump_Click;
            // 
            // Mnu_ViewList
            // 
            Mnu_ViewList.Name = "Mnu_ViewList";
            Mnu_ViewList.Size = new System.Drawing.Size(226, 38);
            Mnu_ViewList.Text = "リスト表示(&L)";
            Mnu_ViewList.Click += Mnu_ViewList_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(223, 6);
            // 
            // Mnu_ViewList_No
            // 
            Mnu_ViewList_No.Name = "Mnu_ViewList_No";
            Mnu_ViewList_No.Size = new System.Drawing.Size(226, 38);
            Mnu_ViewList_No.Text = "番号";
            Mnu_ViewList_No.Click += BTN_No_Click;
            // 
            // Mnu_ViewList_Time
            // 
            Mnu_ViewList_Time.Name = "Mnu_ViewList_Time";
            Mnu_ViewList_Time.Size = new System.Drawing.Size(226, 38);
            Mnu_ViewList_Time.Text = "タイムスタンプ";
            Mnu_ViewList_Time.Click += BTN_Time_Click;
            // 
            // Mnu_ViewList_Type
            // 
            Mnu_ViewList_Type.Name = "Mnu_ViewList_Type";
            Mnu_ViewList_Type.Size = new System.Drawing.Size(226, 38);
            Mnu_ViewList_Type.Text = "タイプ";
            Mnu_ViewList_Type.Click += BTN_Type_Click;
            // 
            // Mnu_ViewList_Name
            // 
            Mnu_ViewList_Name.Name = "Mnu_ViewList_Name";
            Mnu_ViewList_Name.Size = new System.Drawing.Size(226, 38);
            Mnu_ViewList_Name.Text = "キャラ名";
            Mnu_ViewList_Name.Click += BTN_Name_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(223, 6);
            // 
            // Mnu_YoutubeLive
            // 
            Mnu_YoutubeLive.Image = Properties.Resources.Icon_Youtube_js;
            Mnu_YoutubeLive.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            Mnu_YoutubeLive.Name = "Mnu_YoutubeLive";
            Mnu_YoutubeLive.Size = new System.Drawing.Size(226, 38);
            Mnu_YoutubeLive.Text = "YoutubeLive(&Y)";
            Mnu_YoutubeLive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            Mnu_YoutubeLive.Click += Mnu_YoutubeLive_Click;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new System.Drawing.Size(223, 6);
            // 
            // Mnu_Help
            // 
            Mnu_Help.Name = "Mnu_Help";
            Mnu_Help.Size = new System.Drawing.Size(226, 38);
            Mnu_Help.Text = "ヘルプ(&F1)";
            Mnu_Help.Click += Mnu_Help_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1486, 534);
            Controls.Add(LSV_chat);
            Controls.Add(Txt_Chat);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Font = new System.Drawing.Font("Yu Gothic UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(6);
            MinimumSize = new System.Drawing.Size(1059, 327);
            Name = "Main";
            StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Text = "Pre261111-1-MabiChatSpeech (Ver.1.200)";
            FormClosing += Main_FormClosing;
            FormClosed += Main_FormClosed;
            Load += Main_Load;
            Shown += Main_Shown;
            KeyDown += Main_KeyDown;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox Txt_Chat;
        private System.Windows.Forms.ToolStripStatusLabel SLB_Client;
        private System.Windows.Forms.ToolStripStatusLabel SLB_Ip;
        private System.Windows.Forms.Timer Tim_Status;
        private System.Windows.Forms.ToolStripStatusLabel SLB_SaveMode;
        private System.Windows.Forms.ToolStripStatusLabel SLB_Mode;
        private System.Windows.Forms.ToolStripStatusLabel SLB_SendTask;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ListView LSV_chat;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ToolStripProgressBar TPB_Save;
        private System.Windows.Forms.ToolStripStatusLabel TPB_Max;
        private System.Windows.Forms.ToolStripStatusLabel TPB_Event;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Mnu_File;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Quit;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Edit;
        private System.Windows.Forms.ToolStripMenuItem Mnu_CharaList;
        private System.Windows.Forms.ToolStripMenuItem Mnu_PC;
        private System.Windows.Forms.ToolStripMenuItem Mnu_NPC;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem Mnu_View;
        private System.Windows.Forms.ToolStripMenuItem Mnu_ViewText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem Mnu_ViewList_No;
        private System.Windows.Forms.ToolStripMenuItem Mnu_ViewList_Time;
        private System.Windows.Forms.ToolStripMenuItem Mnu_ViewList_Type;
        private System.Windows.Forms.ToolStripMenuItem Mnu_ViewList_Name;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem Mnu_YoutubeLive;
        private System.Windows.Forms.ToolStripMenuItem Mnu_PC_Chat;
        private System.Windows.Forms.ToolStripMenuItem Mnu_PC_TTS;
        private System.Windows.Forms.ToolStripMenuItem Mnu_NPC_Chat;
        private System.Windows.Forms.ToolStripMenuItem Mnu_NPC_TTS;
        private System.Windows.Forms.ToolStripMenuItem Mnu_CharaList_Chat;
        private System.Windows.Forms.ToolStripMenuItem Mnu_CharaList_Voice;
        private System.Windows.Forms.ToolStripMenuItem Mnu_ViewDump;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Echa;
        private System.Windows.Forms.ToolStripMenuItem Mnu_ViewList;
        private System.Windows.Forms.ToolStripStatusLabel SSL_CharaList;
        private System.Windows.Forms.ToolStripStatusLabel SSL_PC;
        private System.Windows.Forms.ToolStripStatusLabel SSL_NPC;
        private System.Windows.Forms.ToolStripStatusLabel SSL_Echa;
        private System.Windows.Forms.ToolStripStatusLabel SSL_Youtube;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Export;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Clear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Help;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Replay;
        private System.Windows.Forms.ToolStripStatusLabel SSL_VoiceVox;
        private System.Windows.Forms.ToolStripStatusLabel SSL_AivisSpeech;
        private System.Windows.Forms.ToolStripStatusLabel SSL_VoiceAPI;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ToolStripMenuItem Mnu_CharList_Edit;
        private System.Windows.Forms.ToolStripMenuItem Mnu_SystemSetiing;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem Mnu_CharaList_TTS;
    }
}

