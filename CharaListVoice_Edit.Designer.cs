namespace MabiChatSpeech
{
    partial class CharaListVoice_Edit
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
            Lst_VoiceEngen = new System.Windows.Forms.ListBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            CHK_Enable = new System.Windows.Forms.CheckBox();
            label11 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            Txt_CharaName = new System.Windows.Forms.TextBox();
            Cmb_CharaType = new System.Windows.Forms.ComboBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            groupBox6 = new System.Windows.Forms.GroupBox();
            TB_PitchScal = new System.Windows.Forms.TrackBar();
            Txt_PitchScal = new System.Windows.Forms.TextBox();
            groupBox9 = new System.Windows.Forms.GroupBox();
            label7 = new System.Windows.Forms.Label();
            Txt_status = new System.Windows.Forms.TextBox();
            Btn_PlayTest = new System.Windows.Forms.Button();
            Txt_TestWord = new System.Windows.Forms.TextBox();
            groupBox5 = new System.Windows.Forms.GroupBox();
            TB_VolumeScal = new System.Windows.Forms.TrackBar();
            Txt_VolumeScal = new System.Windows.Forms.TextBox();
            groupBox4 = new System.Windows.Forms.GroupBox();
            TB_SpeedScal = new System.Windows.Forms.TrackBar();
            Txt_SpeedScale = new System.Windows.Forms.TextBox();
            Lst_VoiceStyle = new System.Windows.Forms.ListBox();
            Lbl_VoiceStyle = new System.Windows.Forms.Label();
            Lst_VoiceChara = new System.Windows.Forms.ListBox();
            Lbl_VoiceChara = new System.Windows.Forms.Label();
            Lbl_VoiceEngen = new System.Windows.Forms.Label();
            Btn_Undo = new System.Windows.Forms.Button();
            Btn_Update = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TB_PitchScal).BeginInit();
            groupBox9.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TB_VolumeScal).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TB_SpeedScal).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // Lst_VoiceEngen
            // 
            Lst_VoiceEngen.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            Lst_VoiceEngen.FormattingEnabled = true;
            Lst_VoiceEngen.ItemHeight = 28;
            Lst_VoiceEngen.Items.AddRange(new object[] { "VoiceVox", "AivisSpeech" });
            Lst_VoiceEngen.Location = new System.Drawing.Point(22, 95);
            Lst_VoiceEngen.Margin = new System.Windows.Forms.Padding(4);
            Lst_VoiceEngen.Name = "Lst_VoiceEngen";
            Lst_VoiceEngen.Size = new System.Drawing.Size(162, 60);
            Lst_VoiceEngen.TabIndex = 4;
            Lst_VoiceEngen.SelectedIndexChanged += Lst_VoiceEngen_SelectedIndexChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(CHK_Enable);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(Txt_CharaName);
            groupBox1.Controls.Add(Cmb_CharaType);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox1.Font = new System.Drawing.Font("Yu Gothic UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Margin = new System.Windows.Forms.Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4);
            groupBox1.Size = new System.Drawing.Size(1402, 145);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "対象キャラクター";
            // 
            // CHK_Enable
            // 
            CHK_Enable.AutoSize = true;
            CHK_Enable.Location = new System.Drawing.Point(39, 71);
            CHK_Enable.Margin = new System.Windows.Forms.Padding(4);
            CHK_Enable.Name = "CHK_Enable";
            CHK_Enable.Size = new System.Drawing.Size(70, 29);
            CHK_Enable.TabIndex = 1;
            CHK_Enable.Text = "有効";
            CHK_Enable.UseVisualStyleBackColor = true;
            CHK_Enable.CheckedChanged += CHK_Enable_CheckedChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(384, 36);
            label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(68, 25);
            label11.TabIndex = 5;
            label11.Text = "キャラ名";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(150, 36);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(91, 25);
            label10.TabIndex = 4;
            label10.Text = "キャラタイプ";
            // 
            // Txt_CharaName
            // 
            Txt_CharaName.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            Txt_CharaName.Location = new System.Drawing.Point(388, 71);
            Txt_CharaName.Margin = new System.Windows.Forms.Padding(4);
            Txt_CharaName.Name = "Txt_CharaName";
            Txt_CharaName.ReadOnly = true;
            Txt_CharaName.Size = new System.Drawing.Size(978, 47);
            Txt_CharaName.TabIndex = 3;
            // 
            // Cmb_CharaType
            // 
            Cmb_CharaType.Enabled = false;
            Cmb_CharaType.Font = new System.Drawing.Font("Yu Gothic UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            Cmb_CharaType.FormattingEnabled = true;
            Cmb_CharaType.Location = new System.Drawing.Point(155, 71);
            Cmb_CharaType.Margin = new System.Windows.Forms.Padding(4);
            Cmb_CharaType.Name = "Cmb_CharaType";
            Cmb_CharaType.Size = new System.Drawing.Size(188, 39);
            Cmb_CharaType.TabIndex = 2;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(groupBox6);
            groupBox3.Controls.Add(groupBox9);
            groupBox3.Controls.Add(groupBox5);
            groupBox3.Controls.Add(groupBox4);
            groupBox3.Controls.Add(Lst_VoiceStyle);
            groupBox3.Controls.Add(Lbl_VoiceStyle);
            groupBox3.Controls.Add(Lst_VoiceChara);
            groupBox3.Controls.Add(Lbl_VoiceChara);
            groupBox3.Controls.Add(Lbl_VoiceEngen);
            groupBox3.Controls.Add(Lst_VoiceEngen);
            groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox3.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            groupBox3.Location = new System.Drawing.Point(0, 145);
            groupBox3.Margin = new System.Windows.Forms.Padding(4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4);
            groupBox3.Size = new System.Drawing.Size(1402, 704);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "音声合成";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(TB_PitchScal);
            groupBox6.Controls.Add(Txt_PitchScal);
            groupBox6.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            groupBox6.Location = new System.Drawing.Point(699, 335);
            groupBox6.Margin = new System.Windows.Forms.Padding(4);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new System.Windows.Forms.Padding(4);
            groupBox6.Size = new System.Drawing.Size(686, 116);
            groupBox6.TabIndex = 10;
            groupBox6.TabStop = false;
            groupBox6.Text = "音高　（-0.15～0.15）";
            // 
            // TB_PitchScal
            // 
            TB_PitchScal.Location = new System.Drawing.Point(192, 32);
            TB_PitchScal.Margin = new System.Windows.Forms.Padding(4);
            TB_PitchScal.Maximum = 15;
            TB_PitchScal.Minimum = -15;
            TB_PitchScal.Name = "TB_PitchScal";
            TB_PitchScal.Size = new System.Drawing.Size(468, 56);
            TB_PitchScal.TabIndex = 9;
            TB_PitchScal.ValueChanged += TB_PitchScal_ValueChanged;
            // 
            // Txt_PitchScal
            // 
            Txt_PitchScal.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            Txt_PitchScal.Location = new System.Drawing.Point(8, 32);
            Txt_PitchScal.Margin = new System.Windows.Forms.Padding(4);
            Txt_PitchScal.Name = "Txt_PitchScal";
            Txt_PitchScal.ReadOnly = true;
            Txt_PitchScal.Size = new System.Drawing.Size(155, 34);
            Txt_PitchScal.TabIndex = 1;
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(label7);
            groupBox9.Controls.Add(Txt_status);
            groupBox9.Controls.Add(Btn_PlayTest);
            groupBox9.Controls.Add(Txt_TestWord);
            groupBox9.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            groupBox9.Location = new System.Drawing.Point(440, 496);
            groupBox9.Margin = new System.Windows.Forms.Padding(4);
            groupBox9.Name = "groupBox9";
            groupBox9.Padding = new System.Windows.Forms.Padding(4);
            groupBox9.Size = new System.Drawing.Size(945, 190);
            groupBox9.TabIndex = 5;
            groupBox9.TabStop = false;
            groupBox9.Text = "音声テスト";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(15, 46);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(96, 28);
            label7.TabIndex = 11;
            label7.Text = "試聴セリフ";
            // 
            // Txt_status
            // 
            Txt_status.Location = new System.Drawing.Point(662, 135);
            Txt_status.Margin = new System.Windows.Forms.Padding(4);
            Txt_status.Name = "Txt_status";
            Txt_status.ReadOnly = true;
            Txt_status.Size = new System.Drawing.Size(269, 34);
            Txt_status.TabIndex = 12;
            // 
            // Btn_PlayTest
            // 
            Btn_PlayTest.Location = new System.Drawing.Point(662, 75);
            Btn_PlayTest.Margin = new System.Windows.Forms.Padding(4);
            Btn_PlayTest.Name = "Btn_PlayTest";
            Btn_PlayTest.Size = new System.Drawing.Size(235, 52);
            Btn_PlayTest.TabIndex = 11;
            Btn_PlayTest.Text = "音声合成再生";
            Btn_PlayTest.UseVisualStyleBackColor = true;
            Btn_PlayTest.Click += Btn_PlayTest_Click;
            // 
            // Txt_TestWord
            // 
            Txt_TestWord.Location = new System.Drawing.Point(39, 85);
            Txt_TestWord.Margin = new System.Windows.Forms.Padding(4);
            Txt_TestWord.Name = "Txt_TestWord";
            Txt_TestWord.Size = new System.Drawing.Size(539, 34);
            Txt_TestWord.TabIndex = 10;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(TB_VolumeScal);
            groupBox5.Controls.Add(Txt_VolumeScal);
            groupBox5.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            groupBox5.Location = new System.Drawing.Point(699, 194);
            groupBox5.Margin = new System.Windows.Forms.Padding(4);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new System.Windows.Forms.Padding(4);
            groupBox5.Size = new System.Drawing.Size(686, 116);
            groupBox5.TabIndex = 9;
            groupBox5.TabStop = false;
            groupBox5.Text = "音量　(0.0～2.0)";
            // 
            // TB_VolumeScal
            // 
            TB_VolumeScal.Location = new System.Drawing.Point(200, 41);
            TB_VolumeScal.Margin = new System.Windows.Forms.Padding(4);
            TB_VolumeScal.Maximum = 20;
            TB_VolumeScal.Name = "TB_VolumeScal";
            TB_VolumeScal.Size = new System.Drawing.Size(468, 56);
            TB_VolumeScal.TabIndex = 8;
            TB_VolumeScal.Value = 10;
            TB_VolumeScal.ValueChanged += TB_VolumeScal_ValueChanged;
            // 
            // Txt_VolumeScal
            // 
            Txt_VolumeScal.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            Txt_VolumeScal.Location = new System.Drawing.Point(0, 32);
            Txt_VolumeScal.Margin = new System.Windows.Forms.Padding(4);
            Txt_VolumeScal.Name = "Txt_VolumeScal";
            Txt_VolumeScal.ReadOnly = true;
            Txt_VolumeScal.Size = new System.Drawing.Size(155, 34);
            Txt_VolumeScal.TabIndex = 1;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(TB_SpeedScal);
            groupBox4.Controls.Add(Txt_SpeedScale);
            groupBox4.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            groupBox4.Location = new System.Drawing.Point(699, 58);
            groupBox4.Margin = new System.Windows.Forms.Padding(4);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new System.Windows.Forms.Padding(4);
            groupBox4.Size = new System.Drawing.Size(686, 116);
            groupBox4.TabIndex = 8;
            groupBox4.TabStop = false;
            groupBox4.Text = "話速　(0.5～2.0)";
            // 
            // TB_SpeedScal
            // 
            TB_SpeedScal.Location = new System.Drawing.Point(192, 32);
            TB_SpeedScal.Margin = new System.Windows.Forms.Padding(4);
            TB_SpeedScal.Maximum = 20;
            TB_SpeedScal.Minimum = 5;
            TB_SpeedScal.Name = "TB_SpeedScal";
            TB_SpeedScal.Size = new System.Drawing.Size(468, 56);
            TB_SpeedScal.TabIndex = 7;
            TB_SpeedScal.Value = 10;
            TB_SpeedScal.ValueChanged += TB_SpeedScal_ValueChanged;
            // 
            // Txt_SpeedScale
            // 
            Txt_SpeedScale.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            Txt_SpeedScale.Location = new System.Drawing.Point(8, 32);
            Txt_SpeedScale.Margin = new System.Windows.Forms.Padding(4);
            Txt_SpeedScale.Name = "Txt_SpeedScale";
            Txt_SpeedScale.ReadOnly = true;
            Txt_SpeedScale.Size = new System.Drawing.Size(155, 34);
            Txt_SpeedScale.TabIndex = 1;
            // 
            // Lst_VoiceStyle
            // 
            Lst_VoiceStyle.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            Lst_VoiceStyle.FormattingEnabled = true;
            Lst_VoiceStyle.ItemHeight = 28;
            Lst_VoiceStyle.Location = new System.Drawing.Point(435, 95);
            Lst_VoiceStyle.Margin = new System.Windows.Forms.Padding(4);
            Lst_VoiceStyle.Name = "Lst_VoiceStyle";
            Lst_VoiceStyle.Size = new System.Drawing.Size(178, 200);
            Lst_VoiceStyle.TabIndex = 6;
            Lst_VoiceStyle.SelectedIndexChanged += Lst_VoiceStyle_SelectedIndexChanged;
            // 
            // Lbl_VoiceStyle
            // 
            Lbl_VoiceStyle.AutoSize = true;
            Lbl_VoiceStyle.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            Lbl_VoiceStyle.Location = new System.Drawing.Point(435, 56);
            Lbl_VoiceStyle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Lbl_VoiceStyle.Name = "Lbl_VoiceStyle";
            Lbl_VoiceStyle.Size = new System.Drawing.Size(113, 28);
            Lbl_VoiceStyle.TabIndex = 5;
            Lbl_VoiceStyle.Text = "音声スタイル";
            // 
            // Lst_VoiceChara
            // 
            Lst_VoiceChara.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            Lst_VoiceChara.FormattingEnabled = true;
            Lst_VoiceChara.ItemHeight = 28;
            Lst_VoiceChara.Location = new System.Drawing.Point(202, 95);
            Lst_VoiceChara.Margin = new System.Windows.Forms.Padding(4);
            Lst_VoiceChara.Name = "Lst_VoiceChara";
            Lst_VoiceChara.Size = new System.Drawing.Size(209, 592);
            Lst_VoiceChara.TabIndex = 5;
            Lst_VoiceChara.SelectedValueChanged += Lst_VoiceChara_SelectedValueChanged;
            // 
            // Lbl_VoiceChara
            // 
            Lbl_VoiceChara.AutoSize = true;
            Lbl_VoiceChara.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            Lbl_VoiceChara.Location = new System.Drawing.Point(202, 56);
            Lbl_VoiceChara.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Lbl_VoiceChara.Name = "Lbl_VoiceChara";
            Lbl_VoiceChara.Size = new System.Drawing.Size(95, 28);
            Lbl_VoiceChara.TabIndex = 3;
            Lbl_VoiceChara.Text = "音声キャラ";
            // 
            // Lbl_VoiceEngen
            // 
            Lbl_VoiceEngen.AutoSize = true;
            Lbl_VoiceEngen.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            Lbl_VoiceEngen.Location = new System.Drawing.Point(15, 56);
            Lbl_VoiceEngen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Lbl_VoiceEngen.Name = "Lbl_VoiceEngen";
            Lbl_VoiceEngen.Size = new System.Drawing.Size(73, 28);
            Lbl_VoiceEngen.TabIndex = 2;
            Lbl_VoiceEngen.Text = "エンジン";
            // 
            // Btn_Undo
            // 
            Btn_Undo.Location = new System.Drawing.Point(1035, 29);
            Btn_Undo.Margin = new System.Windows.Forms.Padding(4);
            Btn_Undo.Name = "Btn_Undo";
            Btn_Undo.Size = new System.Drawing.Size(166, 52);
            Btn_Undo.TabIndex = 13;
            Btn_Undo.Text = "変更を戻す";
            Btn_Undo.UseVisualStyleBackColor = true;
            Btn_Undo.Click += Btn_Undo_Click;
            // 
            // Btn_Update
            // 
            Btn_Update.Location = new System.Drawing.Point(1232, 29);
            Btn_Update.Margin = new System.Windows.Forms.Padding(4);
            Btn_Update.Name = "Btn_Update";
            Btn_Update.Size = new System.Drawing.Size(152, 52);
            Btn_Update.TabIndex = 14;
            Btn_Update.Text = "変更を反映";
            Btn_Update.UseVisualStyleBackColor = true;
            Btn_Update.Click += Btn_Update_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(Btn_Update);
            panel1.Controls.Add(Btn_Undo);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Font = new System.Drawing.Font("Yu Gothic UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            panel1.Location = new System.Drawing.Point(0, 849);
            panel1.Margin = new System.Windows.Forms.Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1402, 100);
            panel1.TabIndex = 6;
            // 
            // CharaListVoice_Edit
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1402, 949);
            Controls.Add(panel1);
            Controls.Add(groupBox3);
            Controls.Add(groupBox1);
            Font = new System.Drawing.Font("Yu Gothic UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            Margin = new System.Windows.Forms.Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CharaListVoice_Edit";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "キャラのボイス設定";
            FormClosed += CharaListVoice_Edit_FormClosed;
            Shown += CharaListVoice_Edit_Shown;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TB_PitchScal).EndInit();
            groupBox9.ResumeLayout(false);
            groupBox9.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TB_VolumeScal).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TB_SpeedScal).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox Lst_TtsName;
        private System.Windows.Forms.ListBox Lst_VoiceEngen;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label Lbl_VoiceEngen;
        private System.Windows.Forms.ListBox Lst_VoiceChara;
        private System.Windows.Forms.Label Lbl_VoiceChara;
        private System.Windows.Forms.Label Lbl_VoiceStyle;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox Lst_VoiceStyle;
        private System.Windows.Forms.TrackBar TB_SpeedScal;
        private System.Windows.Forms.TextBox Txt_SpeedScale;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TrackBar TB_VolumeScal;
        private System.Windows.Forms.TextBox Txt_VolumeScal;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TrackBar TB_PitchScal;
        private System.Windows.Forms.TextBox Txt_PitchScal;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TrackBar TB_TtsVolume;
        private System.Windows.Forms.TextBox Txt_TtsVolume;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TrackBar TB_TtsSpeed;
        private System.Windows.Forms.TextBox Txt_TtsSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Txt_CharaName;
        private System.Windows.Forms.ComboBox Cmb_CharaType;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button Btn_PlayTest;
        private System.Windows.Forms.TextBox Txt_TestWord;
        private System.Windows.Forms.Button Btn_Update;
        private System.Windows.Forms.Button Btn_Undo;
        private System.Windows.Forms.TextBox Txt_status;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox CHK_Enable;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
    }
}