namespace MabiChatSpeech
{
    partial class charlist
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
            Lsv_cs = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            MnuTypeselect = new System.Windows.Forms.ContextMenuStrip(components);
            MNI_SelUser = new System.Windows.Forms.ToolStripMenuItem();
            MNI_SelNpc = new System.Windows.Forms.ToolStripMenuItem();
            MNI_SelOther = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            allCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            Btn_Ok = new System.Windows.Forms.Button();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            Btn_Add = new System.Windows.Forms.Button();
            Btn_Cancel = new System.Windows.Forms.Button();
            Btn_Reload = new System.Windows.Forms.Button();
            MnuTypeselect.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // Lsv_cs
            // 
            Lsv_cs.CheckBoxes = true;
            Lsv_cs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2 });
            Lsv_cs.ContextMenuStrip = MnuTypeselect;
            Lsv_cs.Dock = System.Windows.Forms.DockStyle.Fill;
            Lsv_cs.FullRowSelect = true;
            Lsv_cs.GridLines = true;
            Lsv_cs.Location = new System.Drawing.Point(4, 4);
            Lsv_cs.Margin = new System.Windows.Forms.Padding(4);
            Lsv_cs.Name = "Lsv_cs";
            Lsv_cs.Size = new System.Drawing.Size(308, 519);
            Lsv_cs.TabIndex = 0;
            Lsv_cs.UseCompatibleStateImageBehavior = false;
            Lsv_cs.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "CharacterName";
            columnHeader1.Width = 180;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Type";
            columnHeader2.Width = 50;
            // 
            // MnuTypeselect
            // 
            MnuTypeselect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { MNI_SelUser, MNI_SelNpc, MNI_SelOther, toolStripMenuItem1, allCheckToolStripMenuItem, toolStripMenuItem2 });
            MnuTypeselect.Name = "MnuTypeselect";
            MnuTypeselect.Size = new System.Drawing.Size(150, 120);
            MnuTypeselect.Closed += MnuTypeselect_Closed;
            // 
            // MNI_SelUser
            // 
            MNI_SelUser.Checked = true;
            MNI_SelUser.CheckOnClick = true;
            MNI_SelUser.CheckState = System.Windows.Forms.CheckState.Checked;
            MNI_SelUser.Name = "MNI_SelUser";
            MNI_SelUser.Size = new System.Drawing.Size(149, 22);
            MNI_SelUser.Text = "User";
            MNI_SelUser.CheckedChanged += MNI_SelUser_CheckedChanged;
            // 
            // MNI_SelNpc
            // 
            MNI_SelNpc.Checked = true;
            MNI_SelNpc.CheckOnClick = true;
            MNI_SelNpc.CheckState = System.Windows.Forms.CheckState.Checked;
            MNI_SelNpc.Name = "MNI_SelNpc";
            MNI_SelNpc.Size = new System.Drawing.Size(149, 22);
            MNI_SelNpc.Text = "Npc";
            MNI_SelNpc.CheckedChanged += MNI_SelUser_CheckedChanged;
            // 
            // MNI_SelOther
            // 
            MNI_SelOther.Checked = true;
            MNI_SelOther.CheckOnClick = true;
            MNI_SelOther.CheckState = System.Windows.Forms.CheckState.Checked;
            MNI_SelOther.Name = "MNI_SelOther";
            MNI_SelOther.Size = new System.Drawing.Size(149, 22);
            MNI_SelOther.Text = "Other";
            MNI_SelOther.CheckedChanged += MNI_SelUser_CheckedChanged;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(146, 6);
            // 
            // allCheckToolStripMenuItem
            // 
            allCheckToolStripMenuItem.Name = "allCheckToolStripMenuItem";
            allCheckToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            allCheckToolStripMenuItem.Text = "All Check";
            allCheckToolStripMenuItem.Click += allCheckToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(149, 22);
            toolStripMenuItem2.Text = "Reverse Check";
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            // 
            // Btn_Ok
            // 
            Btn_Ok.Dock = System.Windows.Forms.DockStyle.Right;
            Btn_Ok.Location = new System.Drawing.Point(246, 0);
            Btn_Ok.Margin = new System.Windows.Forms.Padding(4);
            Btn_Ok.Name = "Btn_Ok";
            Btn_Ok.Size = new System.Drawing.Size(70, 35);
            Btn_Ok.TabIndex = 1;
            Btn_Ok.Text = "OK";
            Btn_Ok.UseVisualStyleBackColor = true;
            Btn_Ok.Click += Btn_Ok_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel1, 0, 1);
            tableLayoutPanel2.Controls.Add(Lsv_cs, 0, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanel2.Size = new System.Drawing.Size(316, 562);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // panel1
            // 
            panel1.Controls.Add(Btn_Add);
            panel1.Controls.Add(Btn_Cancel);
            panel1.Controls.Add(Btn_Reload);
            panel1.Controls.Add(Btn_Ok);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 527);
            panel1.Margin = new System.Windows.Forms.Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(316, 35);
            panel1.TabIndex = 4;
            // 
            // Btn_Add
            // 
            Btn_Add.Dock = System.Windows.Forms.DockStyle.Left;
            Btn_Add.Location = new System.Drawing.Point(58, 0);
            Btn_Add.Margin = new System.Windows.Forms.Padding(4);
            Btn_Add.Name = "Btn_Add";
            Btn_Add.Size = new System.Drawing.Size(58, 35);
            Btn_Add.TabIndex = 4;
            Btn_Add.Text = "Add";
            Btn_Add.UseVisualStyleBackColor = true;
            Btn_Add.Click += Btn_Add_Click;
            // 
            // Btn_Cancel
            // 
            Btn_Cancel.Dock = System.Windows.Forms.DockStyle.Right;
            Btn_Cancel.Location = new System.Drawing.Point(176, 0);
            Btn_Cancel.Margin = new System.Windows.Forms.Padding(4);
            Btn_Cancel.Name = "Btn_Cancel";
            Btn_Cancel.Size = new System.Drawing.Size(70, 35);
            Btn_Cancel.TabIndex = 3;
            Btn_Cancel.Text = "Cancel";
            Btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // Btn_Reload
            // 
            Btn_Reload.Dock = System.Windows.Forms.DockStyle.Left;
            Btn_Reload.Location = new System.Drawing.Point(0, 0);
            Btn_Reload.Margin = new System.Windows.Forms.Padding(4);
            Btn_Reload.Name = "Btn_Reload";
            Btn_Reload.Size = new System.Drawing.Size(58, 35);
            Btn_Reload.TabIndex = 2;
            Btn_Reload.Text = "Reload";
            Btn_Reload.UseVisualStyleBackColor = true;
            Btn_Reload.Click += Btn_Reload_Click;
            // 
            // charlist
            // 
            AcceptButton = Btn_Ok;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = Btn_Cancel;
            ClientSize = new System.Drawing.Size(316, 562);
            Controls.Add(tableLayoutPanel2);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Margin = new System.Windows.Forms.Padding(4);
            Name = "charlist";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "charlist";
            Shown += charlist_Shown;
            MnuTypeselect.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView Lsv_cs;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button Btn_Ok;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Btn_Reload;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_Add;
        private System.Windows.Forms.ContextMenuStrip MnuTypeselect;
        private System.Windows.Forms.ToolStripMenuItem MNI_SelUser;
        private System.Windows.Forms.ToolStripMenuItem MNI_SelNpc;
        private System.Windows.Forms.ToolStripMenuItem MNI_SelOther;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem allCheckToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}