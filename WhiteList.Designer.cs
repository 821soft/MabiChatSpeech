namespace MabiChatSpeech
{
    partial class WhiteList
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
            this.components = new System.ComponentModel.Container();
            this.panel2 = new System.Windows.Forms.Panel();
            this.GPB_Item = new System.Windows.Forms.GroupBox();
            this.Cmb_TTSName = new System.Windows.Forms.ComboBox();
            this.Btn_Upd = new System.Windows.Forms.Button();
            this.CHK_Enabled = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Txt_Cn = new System.Windows.Forms.TextBox();
            this.NUD_Speed = new System.Windows.Forms.NumericUpDown();
            this.Cmb_Ct = new System.Windows.Forms.ComboBox();
            this.NUD_Volume = new System.Windows.Forms.NumericUpDown();
            this.BTN_Cancel = new System.Windows.Forms.Button();
            this.Btn_Ins = new System.Windows.Forms.Button();
            this.Btn_Ok = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Lsv_Whitelist = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CMN_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.GPB_Item.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Speed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Volume)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.CMN_Menu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.GPB_Item);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 397);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(534, 100);
            this.panel2.TabIndex = 1;
            // 
            // GPB_Item
            // 
            this.GPB_Item.Controls.Add(this.Cmb_TTSName);
            this.GPB_Item.Controls.Add(this.Btn_Upd);
            this.GPB_Item.Controls.Add(this.CHK_Enabled);
            this.GPB_Item.Controls.Add(this.label4);
            this.GPB_Item.Controls.Add(this.label3);
            this.GPB_Item.Controls.Add(this.label2);
            this.GPB_Item.Controls.Add(this.label1);
            this.GPB_Item.Controls.Add(this.Txt_Cn);
            this.GPB_Item.Controls.Add(this.NUD_Speed);
            this.GPB_Item.Controls.Add(this.Cmb_Ct);
            this.GPB_Item.Controls.Add(this.NUD_Volume);
            this.GPB_Item.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GPB_Item.Location = new System.Drawing.Point(0, 0);
            this.GPB_Item.Name = "GPB_Item";
            this.GPB_Item.Size = new System.Drawing.Size(534, 100);
            this.GPB_Item.TabIndex = 7;
            this.GPB_Item.TabStop = false;
            this.GPB_Item.Text = "編集";
            // 
            // Cmb_TTSName
            // 
            this.Cmb_TTSName.FormattingEnabled = true;
            this.Cmb_TTSName.Location = new System.Drawing.Point(71, 37);
            this.Cmb_TTSName.Name = "Cmb_TTSName";
            this.Cmb_TTSName.Size = new System.Drawing.Size(191, 20);
            this.Cmb_TTSName.TabIndex = 13;
            // 
            // Btn_Upd
            // 
            this.Btn_Upd.Location = new System.Drawing.Point(459, 71);
            this.Btn_Upd.Name = "Btn_Upd";
            this.Btn_Upd.Size = new System.Drawing.Size(75, 23);
            this.Btn_Upd.TabIndex = 12;
            this.Btn_Upd.Text = "追加・更新";
            this.Btn_Upd.UseVisualStyleBackColor = true;
            this.Btn_Upd.Click += new System.EventHandler(this.Btn_Upd_Click);
            // 
            // CHK_Enabled
            // 
            this.CHK_Enabled.AutoSize = true;
            this.CHK_Enabled.Location = new System.Drawing.Point(474, 41);
            this.CHK_Enabled.Name = "CHK_Enabled";
            this.CHK_Enabled.Size = new System.Drawing.Size(48, 16);
            this.CHK_Enabled.TabIndex = 11;
            this.CHK_Enabled.Text = "有効";
            this.CHK_Enabled.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(370, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "速度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(270, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "音量";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(268, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "タイプ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "キャラクタ名";
            // 
            // Txt_Cn
            // 
            this.Txt_Cn.Location = new System.Drawing.Point(71, 12);
            this.Txt_Cn.Name = "Txt_Cn";
            this.Txt_Cn.Size = new System.Drawing.Size(191, 19);
            this.Txt_Cn.TabIndex = 3;
            // 
            // NUD_Speed
            // 
            this.NUD_Speed.Location = new System.Drawing.Point(405, 36);
            this.NUD_Speed.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NUD_Speed.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.NUD_Speed.Name = "NUD_Speed";
            this.NUD_Speed.Size = new System.Drawing.Size(43, 19);
            this.NUD_Speed.TabIndex = 6;
            // 
            // Cmb_Ct
            // 
            this.Cmb_Ct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_Ct.Enabled = false;
            this.Cmb_Ct.FormattingEnabled = true;
            this.Cmb_Ct.Items.AddRange(new object[] {
            "User",
            "Pet",
            "Npc"});
            this.Cmb_Ct.Location = new System.Drawing.Point(305, 11);
            this.Cmb_Ct.Name = "Cmb_Ct";
            this.Cmb_Ct.Size = new System.Drawing.Size(74, 20);
            this.Cmb_Ct.TabIndex = 4;
            // 
            // NUD_Volume
            // 
            this.NUD_Volume.Location = new System.Drawing.Point(305, 36);
            this.NUD_Volume.Name = "NUD_Volume";
            this.NUD_Volume.Size = new System.Drawing.Size(43, 19);
            this.NUD_Volume.TabIndex = 5;
            this.NUD_Volume.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // BTN_Cancel
            // 
            this.BTN_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BTN_Cancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BTN_Cancel.Location = new System.Drawing.Point(384, 0);
            this.BTN_Cancel.Name = "BTN_Cancel";
            this.BTN_Cancel.Size = new System.Drawing.Size(75, 28);
            this.BTN_Cancel.TabIndex = 8;
            this.BTN_Cancel.Text = "Cancel";
            this.BTN_Cancel.UseVisualStyleBackColor = true;
            this.BTN_Cancel.Click += new System.EventHandler(this.BTN_Cancel_Click);
            // 
            // Btn_Ins
            // 
            this.Btn_Ins.Location = new System.Drawing.Point(0, 3);
            this.Btn_Ins.Name = "Btn_Ins";
            this.Btn_Ins.Size = new System.Drawing.Size(116, 23);
            this.Btn_Ins.TabIndex = 2;
            this.Btn_Ins.Text = "チャット内容から追加";
            this.Btn_Ins.UseVisualStyleBackColor = true;
            this.Btn_Ins.Click += new System.EventHandler(this.Btn_Ins_Click);
            // 
            // Btn_Ok
            // 
            this.Btn_Ok.Dock = System.Windows.Forms.DockStyle.Right;
            this.Btn_Ok.Location = new System.Drawing.Point(459, 0);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new System.Drawing.Size(75, 28);
            this.Btn_Ok.TabIndex = 0;
            this.Btn_Ok.Text = "Ok";
            this.Btn_Ok.UseVisualStyleBackColor = true;
            this.Btn_Ok.Click += new System.EventHandler(this.Btn_Ok_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Lsv_Whitelist, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(534, 525);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // Lsv_Whitelist
            // 
            this.Lsv_Whitelist.CheckBoxes = true;
            this.Lsv_Whitelist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.Lsv_Whitelist.ContextMenuStrip = this.CMN_Menu;
            this.Lsv_Whitelist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Lsv_Whitelist.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Lsv_Whitelist.FullRowSelect = true;
            this.Lsv_Whitelist.GridLines = true;
            this.Lsv_Whitelist.HideSelection = false;
            this.Lsv_Whitelist.Location = new System.Drawing.Point(0, 0);
            this.Lsv_Whitelist.Margin = new System.Windows.Forms.Padding(0);
            this.Lsv_Whitelist.MultiSelect = false;
            this.Lsv_Whitelist.Name = "Lsv_Whitelist";
            this.Lsv_Whitelist.Size = new System.Drawing.Size(534, 397);
            this.Lsv_Whitelist.TabIndex = 2;
            this.Lsv_Whitelist.UseCompatibleStateImageBehavior = false;
            this.Lsv_Whitelist.View = System.Windows.Forms.View.Details;
            this.Lsv_Whitelist.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.Lsv_Whitelist_ColumnClick);
            this.Lsv_Whitelist.SelectedIndexChanged += new System.EventHandler(this.Lsv_Whitelist_SelectedIndexChanged);
            this.Lsv_Whitelist.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Lsv_Whitelist_KeyDown);
            this.Lsv_Whitelist.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Lsv_Whitelist_KeyPress);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "CharacterName";
            this.columnHeader5.Width = 180;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Type";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "TTSName";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Volume";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Speed";
            // 
            // CMN_Menu
            // 
            this.CMN_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem2,
            this.clearToolStripMenuItem});
            this.CMN_Menu.Name = "CMN_Menu";
            this.CMN_Menu.ShowCheckMargin = true;
            this.CMN_Menu.ShowImageMargin = false;
            this.CMN_Menu.ShowItemToolTips = false;
            this.CMN_Menu.Size = new System.Drawing.Size(118, 76);
            this.CMN_Menu.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.CMN_Menu_Closing);
            this.CMN_Menu.Opening += new System.ComponentModel.CancelEventHandler(this.CMN_Menu_Opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.toolStripMenuItem1.Text = "Insert";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(114, 6);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.clearToolStripMenuItem.Text = "All Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BTN_Cancel);
            this.panel1.Controls.Add(this.Btn_Ok);
            this.panel1.Controls.Add(this.Btn_Ins);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 497);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(534, 28);
            this.panel1.TabIndex = 3;
            // 
            // WhiteList
            // 
            this.AcceptButton = this.Btn_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BTN_Cancel;
            this.ClientSize = new System.Drawing.Size(534, 525);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WhiteList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "キャラ指定リスト";
            this.Load += new System.EventHandler(this.WhiteList_Load);
            this.Shown += new System.EventHandler(this.WhiteList_Shown);
            this.panel2.ResumeLayout(false);
            this.GPB_Item.ResumeLayout(false);
            this.GPB_Item.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Speed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Volume)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.CMN_Menu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button Btn_Ok;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView Lsv_Whitelist;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button Btn_Ins;
        private System.Windows.Forms.NumericUpDown NUD_Speed;
        private System.Windows.Forms.NumericUpDown NUD_Volume;
        private System.Windows.Forms.ComboBox Cmb_Ct;
        private System.Windows.Forms.TextBox Txt_Cn;
        private System.Windows.Forms.GroupBox GPB_Item;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_Upd;
        private System.Windows.Forms.CheckBox CHK_Enabled;
        private System.Windows.Forms.ContextMenuStrip CMN_Menu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Button BTN_Cancel;
        private System.Windows.Forms.ComboBox Cmb_TTSName;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
    }
}