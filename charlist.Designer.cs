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
            this.components = new System.ComponentModel.Container();
            this.Lsv_cs = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.MnuTypeselect = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MNI_SelUser = new System.Windows.Forms.ToolStripMenuItem();
            this.MNI_SelNpc = new System.Windows.Forms.ToolStripMenuItem();
            this.MNI_SelOther = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.allCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Btn_Ok = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Btn_Add = new System.Windows.Forms.Button();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_Reload = new System.Windows.Forms.Button();
            this.MnuTypeselect.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Lsv_cs
            // 
            this.Lsv_cs.CheckBoxes = true;
            this.Lsv_cs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.Lsv_cs.ContextMenuStrip = this.MnuTypeselect;
            this.Lsv_cs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Lsv_cs.FullRowSelect = true;
            this.Lsv_cs.GridLines = true;
            this.Lsv_cs.HideSelection = false;
            this.Lsv_cs.Location = new System.Drawing.Point(3, 3);
            this.Lsv_cs.Name = "Lsv_cs";
            this.Lsv_cs.Size = new System.Drawing.Size(265, 416);
            this.Lsv_cs.TabIndex = 0;
            this.Lsv_cs.UseCompatibleStateImageBehavior = false;
            this.Lsv_cs.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "CharacterName";
            this.columnHeader1.Width = 180;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 50;
            // 
            // MnuTypeselect
            // 
            this.MnuTypeselect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MNI_SelUser,
            this.MNI_SelNpc,
            this.MNI_SelOther,
            this.toolStripMenuItem1,
            this.allCheckToolStripMenuItem,
            this.toolStripMenuItem2});
            this.MnuTypeselect.Name = "MnuTypeselect";
            this.MnuTypeselect.Size = new System.Drawing.Size(150, 120);
            this.MnuTypeselect.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.MnuTypeselect_Closed);
            // 
            // MNI_SelUser
            // 
            this.MNI_SelUser.Checked = true;
            this.MNI_SelUser.CheckOnClick = true;
            this.MNI_SelUser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MNI_SelUser.Name = "MNI_SelUser";
            this.MNI_SelUser.Size = new System.Drawing.Size(149, 22);
            this.MNI_SelUser.Text = "User";
            this.MNI_SelUser.CheckedChanged += new System.EventHandler(this.MNI_SelUser_CheckedChanged);
            // 
            // MNI_SelNpc
            // 
            this.MNI_SelNpc.Checked = true;
            this.MNI_SelNpc.CheckOnClick = true;
            this.MNI_SelNpc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MNI_SelNpc.Name = "MNI_SelNpc";
            this.MNI_SelNpc.Size = new System.Drawing.Size(149, 22);
            this.MNI_SelNpc.Text = "Npc";
            this.MNI_SelNpc.CheckedChanged += new System.EventHandler(this.MNI_SelUser_CheckedChanged);
            // 
            // MNI_SelOther
            // 
            this.MNI_SelOther.Checked = true;
            this.MNI_SelOther.CheckOnClick = true;
            this.MNI_SelOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MNI_SelOther.Name = "MNI_SelOther";
            this.MNI_SelOther.Size = new System.Drawing.Size(149, 22);
            this.MNI_SelOther.Text = "Other";
            this.MNI_SelOther.CheckedChanged += new System.EventHandler(this.MNI_SelUser_CheckedChanged);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(146, 6);
            // 
            // allCheckToolStripMenuItem
            // 
            this.allCheckToolStripMenuItem.Name = "allCheckToolStripMenuItem";
            this.allCheckToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.allCheckToolStripMenuItem.Text = "All Check";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItem2.Text = "Reverse Check";
            // 
            // Btn_Ok
            // 
            this.Btn_Ok.Dock = System.Windows.Forms.DockStyle.Right;
            this.Btn_Ok.Location = new System.Drawing.Point(211, 0);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new System.Drawing.Size(60, 28);
            this.Btn_Ok.TabIndex = 1;
            this.Btn_Ok.Text = "OK";
            this.Btn_Ok.UseVisualStyleBackColor = true;
            this.Btn_Ok.Click += new System.EventHandler(this.Btn_Ok_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.Lsv_cs, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(271, 450);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Btn_Add);
            this.panel1.Controls.Add(this.Btn_Cancel);
            this.panel1.Controls.Add(this.Btn_Reload);
            this.panel1.Controls.Add(this.Btn_Ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 422);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(271, 28);
            this.panel1.TabIndex = 4;
            // 
            // Btn_Add
            // 
            this.Btn_Add.Dock = System.Windows.Forms.DockStyle.Left;
            this.Btn_Add.Location = new System.Drawing.Point(50, 0);
            this.Btn_Add.Name = "Btn_Add";
            this.Btn_Add.Size = new System.Drawing.Size(50, 28);
            this.Btn_Add.TabIndex = 4;
            this.Btn_Add.Text = "Add";
            this.Btn_Add.UseVisualStyleBackColor = true;
            this.Btn_Add.Click += new System.EventHandler(this.Btn_Add_Click);
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.Btn_Cancel.Location = new System.Drawing.Point(151, 0);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(60, 28);
            this.Btn_Cancel.TabIndex = 3;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // Btn_Reload
            // 
            this.Btn_Reload.Dock = System.Windows.Forms.DockStyle.Left;
            this.Btn_Reload.Location = new System.Drawing.Point(0, 0);
            this.Btn_Reload.Name = "Btn_Reload";
            this.Btn_Reload.Size = new System.Drawing.Size(50, 28);
            this.Btn_Reload.TabIndex = 2;
            this.Btn_Reload.Text = "Reload";
            this.Btn_Reload.UseVisualStyleBackColor = true;
            this.Btn_Reload.Click += new System.EventHandler(this.Btn_Reload_Click);
            // 
            // charlist
            // 
            this.AcceptButton = this.Btn_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Btn_Cancel;
            this.ClientSize = new System.Drawing.Size(271, 450);
            this.Controls.Add(this.tableLayoutPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "charlist";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "charlist";
            this.Shown += new System.EventHandler(this.charlist_Shown);
            this.MnuTypeselect.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

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