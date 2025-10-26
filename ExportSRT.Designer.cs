namespace MabiChatSpeech
{
    partial class ExportSRT
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
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            BTN_Cancel = new System.Windows.Forms.Button();
            BTN_Ok = new System.Windows.Forms.Button();
            listBox1 = new System.Windows.Forms.ListBox();
            panel2 = new System.Windows.Forms.Panel();
            lb_FileInfo = new System.Windows.Forms.Label();
            panel3 = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            trackBar1 = new System.Windows.Forms.TrackBar();
            label5 = new System.Windows.Forms.Label();
            lb_No = new System.Windows.Forms.Label();
            nm_No = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            lb_SelectTime = new System.Windows.Forms.Label();
            nm_Millisecond = new System.Windows.Forms.NumericUpDown();
            nm_Second = new System.Windows.Forms.NumericUpDown();
            nm_Minute = new System.Windows.Forms.NumericUpDown();
            nm_Hour = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            process1 = new System.Diagnostics.Process();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nm_No).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nm_Millisecond).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nm_Second).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nm_Minute).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nm_Hour).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 3);
            tableLayoutPanel1.Controls.Add(listBox1, 0, 1);
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Controls.Add(panel3, 0, 2);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanel1.Size = new System.Drawing.Size(766, 347);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            panel1.Controls.Add(BTN_Cancel);
            panel1.Controls.Add(BTN_Ok);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(3, 315);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(760, 29);
            panel1.TabIndex = 0;
            // 
            // BTN_Cancel
            // 
            BTN_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            BTN_Cancel.Location = new System.Drawing.Point(600, 3);
            BTN_Cancel.Name = "BTN_Cancel";
            BTN_Cancel.Size = new System.Drawing.Size(75, 23);
            BTN_Cancel.TabIndex = 1;
            BTN_Cancel.Text = "Cancel";
            BTN_Cancel.UseVisualStyleBackColor = true;
            BTN_Cancel.Click += BTN_Cancel_Click;
            // 
            // BTN_Ok
            // 
            BTN_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            BTN_Ok.Location = new System.Drawing.Point(681, 3);
            BTN_Ok.Name = "BTN_Ok";
            BTN_Ok.Size = new System.Drawing.Size(75, 23);
            BTN_Ok.TabIndex = 0;
            BTN_Ok.Text = "Ok";
            BTN_Ok.UseVisualStyleBackColor = true;
            BTN_Ok.Click += BTN_Ok_Click;
            // 
            // listBox1
            // 
            listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new System.Drawing.Point(3, 38);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(760, 181);
            listBox1.TabIndex = 1;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // panel2
            // 
            panel2.Controls.Add(lb_FileInfo);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(760, 29);
            panel2.TabIndex = 2;
            // 
            // lb_FileInfo
            // 
            lb_FileInfo.AutoSize = true;
            lb_FileInfo.Location = new System.Drawing.Point(3, 6);
            lb_FileInfo.Name = "lb_FileInfo";
            lb_FileInfo.Size = new System.Drawing.Size(65, 15);
            lb_FileInfo.TabIndex = 0;
            lb_FileInfo.Text = "ファイル情報";
            // 
            // panel3
            // 
            panel3.Controls.Add(label3);
            panel3.Controls.Add(textBox1);
            panel3.Controls.Add(trackBar1);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(lb_No);
            panel3.Controls.Add(nm_No);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(lb_SelectTime);
            panel3.Controls.Add(nm_Millisecond);
            panel3.Controls.Add(nm_Second);
            panel3.Controls.Add(nm_Minute);
            panel3.Controls.Add(nm_Hour);
            panel3.Controls.Add(label2);
            panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            panel3.Location = new System.Drawing.Point(3, 225);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(760, 84);
            panel3.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(560, 24);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(55, 15);
            label3.TabIndex = 13;
            label3.Text = "表示秒数";
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Location = new System.Drawing.Point(681, 44);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(62, 23);
            textBox1.TabIndex = 12;
            // 
            // trackBar1
            // 
            trackBar1.Location = new System.Drawing.Point(491, 42);
            trackBar1.Maximum = 100;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new System.Drawing.Size(184, 45);
            trackBar1.TabIndex = 11;
            trackBar1.TickFrequency = 5;
            trackBar1.Value = 1;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(180, 24);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(310, 15);
            label5.TabIndex = 10;
            label5.Text = "書出しNo　　時刻　 時　 　　分　　　 秒　　　　 ミリ秒";
            // 
            // lb_No
            // 
            lb_No.Location = new System.Drawing.Point(26, 46);
            lb_No.Name = "lb_No";
            lb_No.Size = new System.Drawing.Size(44, 21);
            lb_No.TabIndex = 9;
            lb_No.Text = "99999";
            // 
            // nm_No
            // 
            nm_No.Enabled = false;
            nm_No.Location = new System.Drawing.Point(190, 42);
            nm_No.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nm_No.Name = "nm_No";
            nm_No.Size = new System.Drawing.Size(57, 23);
            nm_No.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(26, 24);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(83, 15);
            label1.TabIndex = 7;
            label1.Text = "起点No　時刻";
            // 
            // lb_SelectTime
            // 
            lb_SelectTime.Location = new System.Drawing.Point(76, 44);
            lb_SelectTime.Name = "lb_SelectTime";
            lb_SelectTime.Size = new System.Drawing.Size(75, 23);
            lb_SelectTime.TabIndex = 6;
            lb_SelectTime.Text = "99:99:99.999";
            // 
            // nm_Millisecond
            // 
            nm_Millisecond.Location = new System.Drawing.Point(419, 42);
            nm_Millisecond.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nm_Millisecond.Name = "nm_Millisecond";
            nm_Millisecond.Size = new System.Drawing.Size(57, 23);
            nm_Millisecond.TabIndex = 5;
            // 
            // nm_Second
            // 
            nm_Second.Location = new System.Drawing.Point(368, 42);
            nm_Second.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            nm_Second.Name = "nm_Second";
            nm_Second.Size = new System.Drawing.Size(45, 23);
            nm_Second.TabIndex = 4;
            // 
            // nm_Minute
            // 
            nm_Minute.Location = new System.Drawing.Point(317, 42);
            nm_Minute.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            nm_Minute.Name = "nm_Minute";
            nm_Minute.Size = new System.Drawing.Size(45, 23);
            nm_Minute.TabIndex = 3;
            // 
            // nm_Hour
            // 
            nm_Hour.Location = new System.Drawing.Point(266, 42);
            nm_Hour.Name = "nm_Hour";
            nm_Hour.Size = new System.Drawing.Size(45, 23);
            nm_Hour.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(0, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(55, 15);
            label2.TabIndex = 0;
            label2.Text = "出力設定";
            // 
            // openFileDialog1
            // 
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Title = "チャットログ";
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "str";
            saveFileDialog1.Filter = "SRTファイル(*.srt)|*.srt|すべてのファイル(*.*)|*.*";
            // 
            // process1
            // 
            process1.StartInfo.Domain = "";
            process1.StartInfo.LoadUserProfile = false;
            process1.StartInfo.Password = null;
            process1.StartInfo.StandardErrorEncoding = null;
            process1.StartInfo.StandardInputEncoding = null;
            process1.StartInfo.StandardOutputEncoding = null;
            process1.StartInfo.UseCredentialsForNetworkingOnly = false;
            process1.StartInfo.UserName = "";
            process1.SynchronizingObject = this;
            // 
            // ExportSRT
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(766, 347);
            Controls.Add(tableLayoutPanel1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ExportSRT";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Export .srt";
            Load += ExportSRT_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)nm_No).EndInit();
            ((System.ComponentModel.ISupportInitialize)nm_Millisecond).EndInit();
            ((System.ComponentModel.ISupportInitialize)nm_Second).EndInit();
            ((System.ComponentModel.ISupportInitialize)nm_Minute).EndInit();
            ((System.ComponentModel.ISupportInitialize)nm_Hour).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button BTN_Cancel;
        private System.Windows.Forms.Button BTN_Ok;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lb_FileInfo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.NumericUpDown nm_Millisecond;
        private System.Windows.Forms.NumericUpDown nm_Second;
        private System.Windows.Forms.NumericUpDown nm_Minute;
        private System.Windows.Forms.NumericUpDown nm_Hour;
        private System.Windows.Forms.Label lb_SelectTime;
        private System.Windows.Forms.NumericUpDown nm_No;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lb_No;
        private System.Diagnostics.Process process1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
    }
}