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
            components = new System.ComponentModel.Container();
            BTN_Cancel = new System.Windows.Forms.Button();
            Btn_Ok = new System.Windows.Forms.Button();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            Lsv_Whitelist = new System.Windows.Forms.ListView();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader6 = new System.Windows.Forms.ColumnHeader();
            columnHeader7 = new System.Windows.Forms.ColumnHeader();
            columnHeader8 = new System.Windows.Forms.ColumnHeader();
            columnHeader9 = new System.Windows.Forms.ColumnHeader();
            columnHeader10 = new System.Windows.Forms.ColumnHeader();
            columnHeader11 = new System.Windows.Forms.ColumnHeader();
            Cms_SelectMenu = new System.Windows.Forms.ContextMenuStrip(components);
            Mnu_AllEnable = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_AllDisable = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            Mnu_Delete = new System.Windows.Forms.ToolStripMenuItem();
            panel1 = new System.Windows.Forms.Panel();
            Mnu_Visible_On = new System.Windows.Forms.ToolStripMenuItem();
            Mnu_Visible_Off = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            tableLayoutPanel1.SuspendLayout();
            Cms_SelectMenu.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // BTN_Cancel
            // 
            BTN_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            BTN_Cancel.Location = new System.Drawing.Point(508, 15);
            BTN_Cancel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            BTN_Cancel.Name = "BTN_Cancel";
            BTN_Cancel.Size = new System.Drawing.Size(125, 29);
            BTN_Cancel.TabIndex = 8;
            BTN_Cancel.Text = "Cancel";
            BTN_Cancel.UseVisualStyleBackColor = true;
            BTN_Cancel.Click += BTN_Cancel_Click;
            // 
            // Btn_Ok
            // 
            Btn_Ok.Location = new System.Drawing.Point(672, 15);
            Btn_Ok.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            Btn_Ok.Name = "Btn_Ok";
            Btn_Ok.Size = new System.Drawing.Size(125, 29);
            Btn_Ok.TabIndex = 0;
            Btn_Ok.Text = "Ok";
            Btn_Ok.UseVisualStyleBackColor = true;
            Btn_Ok.Click += Btn_Ok_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(Lsv_Whitelist, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new System.Drawing.Size(828, 348);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // Lsv_Whitelist
            // 
            Lsv_Whitelist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader3, columnHeader5, columnHeader1, columnHeader2, columnHeader6, columnHeader7, columnHeader8, columnHeader9, columnHeader10, columnHeader11 });
            Lsv_Whitelist.ContextMenuStrip = Cms_SelectMenu;
            Lsv_Whitelist.Dock = System.Windows.Forms.DockStyle.Fill;
            Lsv_Whitelist.ForeColor = System.Drawing.SystemColors.WindowText;
            Lsv_Whitelist.FullRowSelect = true;
            Lsv_Whitelist.GridLines = true;
            Lsv_Whitelist.HideSelection = true;
            Lsv_Whitelist.Location = new System.Drawing.Point(0, 0);
            Lsv_Whitelist.Margin = new System.Windows.Forms.Padding(0);
            Lsv_Whitelist.Name = "Lsv_Whitelist";
            Lsv_Whitelist.Size = new System.Drawing.Size(828, 289);
            Lsv_Whitelist.TabIndex = 2;
            Lsv_Whitelist.UseCompatibleStateImageBehavior = false;
            Lsv_Whitelist.View = System.Windows.Forms.View.Details;
            Lsv_Whitelist.ColumnClick += Lsv_Whitelist_ColumnClick;
            Lsv_Whitelist.ItemCheck += Lsv_Whitelist_ItemCheck;
            Lsv_Whitelist.Enter += Lsv_Whitelist_Enter;
            Lsv_Whitelist.KeyUp += Lsv_Whitelist_KeyUp;
            Lsv_Whitelist.Leave += Lsv_Whitelist_Leave;
            Lsv_Whitelist.MouseDoubleClick += Lsv_Whitelist_MouseDoubleClick;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "有効";
            columnHeader3.Width = 45;
            // 
            // columnHeader5
            // 
            columnHeader5.DisplayIndex = 2;
            columnHeader5.Text = "キャラクター名";
            columnHeader5.Width = 170;
            // 
            // columnHeader1
            // 
            columnHeader1.DisplayIndex = 3;
            columnHeader1.Text = "種別";
            columnHeader1.Width = 45;
            // 
            // columnHeader2
            // 
            columnHeader2.DisplayIndex = 4;
            columnHeader2.Text = "音声種別";
            columnHeader2.Width = 100;
            // 
            // columnHeader6
            // 
            columnHeader6.DisplayIndex = 5;
            columnHeader6.Text = "ボイスキャラ";
            columnHeader6.Width = 170;
            // 
            // columnHeader7
            // 
            columnHeader7.DisplayIndex = 6;
            columnHeader7.Text = "ボイスID";
            // 
            // columnHeader8
            // 
            columnHeader8.DisplayIndex = 8;
            columnHeader8.Text = "音量";
            columnHeader8.Width = 50;
            // 
            // columnHeader9
            // 
            columnHeader9.Text = "話速";
            columnHeader9.Width = 50;
            // 
            // columnHeader10
            // 
            columnHeader10.DisplayIndex = 9;
            columnHeader10.Text = "音高";
            columnHeader10.Width = 50;
            // 
            // columnHeader11
            // 
            columnHeader11.DisplayIndex = 1;
            columnHeader11.Text = "表示";
            columnHeader11.Width = 50;
            // 
            // Cms_SelectMenu
            // 
            Cms_SelectMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Mnu_AllEnable, Mnu_AllDisable, toolStripMenuItem1, Mnu_Visible_On, Mnu_Visible_Off, toolStripMenuItem2, Mnu_Delete });
            Cms_SelectMenu.Name = "Cms_SelectMenu";
            Cms_SelectMenu.Size = new System.Drawing.Size(181, 208);
            // 
            // Mnu_AllEnable
            // 
            Mnu_AllEnable.Name = "Mnu_AllEnable";
            Mnu_AllEnable.Size = new System.Drawing.Size(180, 34);
            Mnu_AllEnable.Text = "有効にする";
            Mnu_AllEnable.Click += Mnu_AllEnable_Click;
            // 
            // Mnu_AllDisable
            // 
            Mnu_AllDisable.Name = "Mnu_AllDisable";
            Mnu_AllDisable.Size = new System.Drawing.Size(180, 34);
            Mnu_AllDisable.Text = "無効にする";
            Mnu_AllDisable.Click += Mnu_AllDisable_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // Mnu_Delete
            // 
            Mnu_Delete.Name = "Mnu_Delete";
            Mnu_Delete.Size = new System.Drawing.Size(180, 34);
            Mnu_Delete.Text = "削除(DEL)";
            Mnu_Delete.Click += Mnu_Delete_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(BTN_Cancel);
            panel1.Controls.Add(Btn_Ok);
            panel1.Location = new System.Drawing.Point(0, 289);
            panel1.Margin = new System.Windows.Forms.Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(828, 59);
            panel1.TabIndex = 3;
            // 
            // Mnu_Visible_On
            // 
            Mnu_Visible_On.Name = "Mnu_Visible_On";
            Mnu_Visible_On.Size = new System.Drawing.Size(180, 34);
            Mnu_Visible_On.Text = "表示";
            Mnu_Visible_On.Click += Mnu_Visible_On_Click;
            // 
            // Mnu_Visible_Off
            // 
            Mnu_Visible_Off.Name = "Mnu_Visible_Off";
            Mnu_Visible_Off.Size = new System.Drawing.Size(180, 34);
            Mnu_Visible_Off.Text = "非表示";
            Mnu_Visible_Off.Click += Mnu_Visible_Off_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // WhiteList
            // 
            AcceptButton = Btn_Ok;
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = BTN_Cancel;
            ClientSize = new System.Drawing.Size(828, 348);
            Controls.Add(tableLayoutPanel1);
            Font = new System.Drawing.Font("Yu Gothic UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WhiteList";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "キャラリスト編集";
            Load += WhiteList_Load;
            Shown += WhiteList_Shown;
            tableLayoutPanel1.ResumeLayout(false);
            Cms_SelectMenu.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Btn_Ok;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView Lsv_Whitelist;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button BTN_Cancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ContextMenuStrip Cms_SelectMenu;
        private System.Windows.Forms.ToolStripMenuItem Mnu_AllEnable;
        private System.Windows.Forms.ToolStripMenuItem Mnu_AllDisable;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Delete;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Visible_On;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Visible_Off;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    }
}
