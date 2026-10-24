namespace MabiChatSpeech
{
    partial class Replay_log
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
            groupBox1 = new System.Windows.Forms.GroupBox();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            lbl_Count = new System.Windows.Forms.Label();
            lbl_filename = new System.Windows.Forms.Label();
            comboBox1 = new System.Windows.Forms.ComboBox();
            button1 = new System.Windows.Forms.Button();
            listView1 = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            CMN_Voice = new System.Windows.Forms.ContextMenuStrip(components);
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            tabPage2 = new System.Windows.Forms.TabPage();
            listView2 = new System.Windows.Forms.ListView();
            columnHeader6 = new System.Windows.Forms.ColumnHeader();
            columnHeader7 = new System.Windows.Forms.ColumnHeader();
            columnHeader8 = new System.Windows.Forms.ColumnHeader();
            columnHeader9 = new System.Windows.Forms.ColumnHeader();
            CMN_VoiceList = new System.Windows.Forms.ContextMenuStrip(components);
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(1091, 125);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Log Info";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 2, 0);
            tableLayoutPanel1.Controls.Add(lbl_Count, 3, 0);
            tableLayoutPanel1.Controls.Add(lbl_filename, 1, 0);
            tableLayoutPanel1.Controls.Add(comboBox1, 3, 2);
            tableLayoutPanel1.Controls.Add(button1, 1, 2);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(3, 23);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.Size = new System.Drawing.Size(1085, 99);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.Location = new System.Drawing.Point(3, 3);
            label1.Margin = new System.Windows.Forms.Padding(3);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(114, 19);
            label1.TabIndex = 0;
            label1.Text = "FileName";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = System.Windows.Forms.DockStyle.Fill;
            label2.Location = new System.Drawing.Point(373, 3);
            label2.Margin = new System.Windows.Forms.Padding(3);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(114, 19);
            label2.TabIndex = 1;
            label2.Text = "総チャット数";
            // 
            // lbl_Count
            // 
            lbl_Count.AutoSize = true;
            lbl_Count.Location = new System.Drawing.Point(493, 3);
            lbl_Count.Margin = new System.Windows.Forms.Padding(3);
            lbl_Count.Name = "lbl_Count";
            lbl_Count.Size = new System.Drawing.Size(50, 19);
            lbl_Count.TabIndex = 2;
            lbl_Count.Text = "label3";
            // 
            // lbl_filename
            // 
            lbl_filename.AutoSize = true;
            lbl_filename.Location = new System.Drawing.Point(123, 3);
            lbl_filename.Margin = new System.Windows.Forms.Padding(3);
            lbl_filename.Name = "lbl_filename";
            lbl_filename.Size = new System.Drawing.Size(50, 19);
            lbl_filename.TabIndex = 3;
            lbl_filename.Text = "label3";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(490, 50);
            comboBox1.Margin = new System.Windows.Forms.Padding(0);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(277, 28);
            comboBox1.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(123, 53);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(94, 24);
            button1.TabIndex = 5;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 });
            listView1.ContextMenuStrip = CMN_Voice;
            listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Location = new System.Drawing.Point(3, 3);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(1077, 628);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            // 
            // CMN_Voice
            // 
            CMN_Voice.ImageScalingSize = new System.Drawing.Size(20, 20);
            CMN_Voice.Name = "CMN_Voice";
            CMN_Voice.Size = new System.Drawing.Size(61, 4);
            CMN_Voice.ItemClicked += CMN_Voice_ItemClicked;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 125);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(1091, 667);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(listView1);
            tabPage1.Location = new System.Drawing.Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(1083, 634);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(listView2);
            tabPage2.Location = new System.Drawing.Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(1083, 634);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // listView2
            // 
            listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader6, columnHeader7, columnHeader8, columnHeader9 });
            listView2.ContextMenuStrip = CMN_VoiceList;
            listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            listView2.FullRowSelect = true;
            listView2.GridLines = true;
            listView2.Location = new System.Drawing.Point(3, 3);
            listView2.MultiSelect = false;
            listView2.Name = "listView2";
            listView2.Size = new System.Drawing.Size(1077, 628);
            listView2.TabIndex = 0;
            listView2.UseCompatibleStateImageBehavior = false;
            listView2.View = System.Windows.Forms.View.Details;
            // 
            // CMN_VoiceList
            // 
            CMN_VoiceList.ImageScalingSize = new System.Drawing.Size(20, 20);
            CMN_VoiceList.Name = "CMN_VoiceList";
            CMN_VoiceList.Size = new System.Drawing.Size(61, 4);
            CMN_VoiceList.ItemClicked += CMN_VoiceList_ItemClicked;
            // 
            // Replay_log
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1091, 792);
            Controls.Add(tabControl1);
            Controls.Add(groupBox1);
            Name = "Replay_log";
            Text = "Reload_log";
            Shown += Replay_log_Shown;
            groupBox1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label lbl_Count;
        private System.Windows.Forms.Label lbl_filename;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ContextMenuStrip CMN_Voice;
        private System.Windows.Forms.ContextMenuStrip CMN_VoiceList;
        private System.Windows.Forms.Button button1;
    }
}