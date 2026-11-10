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
            panel1 = new System.Windows.Forms.Panel();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // BTN_Cancel
            // 
            BTN_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            BTN_Cancel.Dock = System.Windows.Forms.DockStyle.Right;
            BTN_Cancel.Location = new System.Drawing.Point(800, 0);
            BTN_Cancel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            BTN_Cancel.Name = "BTN_Cancel";
            BTN_Cancel.Size = new System.Drawing.Size(125, 59);
            BTN_Cancel.TabIndex = 8;
            BTN_Cancel.Text = "Cancel";
            BTN_Cancel.UseVisualStyleBackColor = true;
            BTN_Cancel.Click += BTN_Cancel_Click;
            // 
            // Btn_Ok
            // 
            Btn_Ok.Dock = System.Windows.Forms.DockStyle.Right;
            Btn_Ok.Location = new System.Drawing.Point(925, 0);
            Btn_Ok.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            Btn_Ok.Name = "Btn_Ok";
            Btn_Ok.Size = new System.Drawing.Size(125, 59);
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
            tableLayoutPanel1.Size = new System.Drawing.Size(1050, 511);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // Lsv_Whitelist
            // 
            Lsv_Whitelist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader3, columnHeader5, columnHeader1, columnHeader2, columnHeader6, columnHeader7, columnHeader8, columnHeader9, columnHeader10 });
            Lsv_Whitelist.Dock = System.Windows.Forms.DockStyle.Fill;
            Lsv_Whitelist.ForeColor = System.Drawing.SystemColors.WindowText;
            Lsv_Whitelist.FullRowSelect = true;
            Lsv_Whitelist.GridLines = true;
            Lsv_Whitelist.Location = new System.Drawing.Point(0, 0);
            Lsv_Whitelist.Margin = new System.Windows.Forms.Padding(0);
            Lsv_Whitelist.Name = "Lsv_Whitelist";
            Lsv_Whitelist.Size = new System.Drawing.Size(1050, 452);
            Lsv_Whitelist.TabIndex = 2;
            Lsv_Whitelist.UseCompatibleStateImageBehavior = false;
            Lsv_Whitelist.View = System.Windows.Forms.View.Details;
            Lsv_Whitelist.ColumnClick += Lsv_Whitelist_ColumnClick;
            Lsv_Whitelist.ItemCheck += Lsv_Whitelist_ItemCheck;
            Lsv_Whitelist.KeyUp += Lsv_Whitelist_KeyUp;
            Lsv_Whitelist.MouseDoubleClick += Lsv_Whitelist_MouseDoubleClick;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "有効";
            columnHeader3.Width = 45;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "キャラクター名";
            columnHeader5.Width = 170;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "種別";
            columnHeader1.Width = 45;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "音声種別";
            columnHeader2.Width = 100;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "ボイスキャラ";
            columnHeader6.Width = 170;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "ボイスID";
            // 
            // columnHeader8
            // 
            columnHeader8.Text = "話速";
            columnHeader8.Width = 50;
            // 
            // columnHeader9
            // 
            columnHeader9.Text = "音量";
            columnHeader9.Width = 50;
            // 
            // columnHeader10
            // 
            columnHeader10.Text = "音高";
            columnHeader10.Width = 50;
            // 
            // panel1
            // 
            panel1.Controls.Add(BTN_Cancel);
            panel1.Controls.Add(Btn_Ok);
            panel1.Location = new System.Drawing.Point(0, 452);
            panel1.Margin = new System.Windows.Forms.Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1050, 59);
            panel1.TabIndex = 3;
            // 
            // WhiteList
            // 
            AcceptButton = Btn_Ok;
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = BTN_Cancel;
            ClientSize = new System.Drawing.Size(1050, 511);
            Controls.Add(tableLayoutPanel1);
            Font = new System.Drawing.Font("Yu Gothic UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            HelpButton = true;
            Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WhiteList";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "キャラリスト";
            Load += WhiteList_Load;
            Shown += WhiteList_Shown;
            tableLayoutPanel1.ResumeLayout(false);
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
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}