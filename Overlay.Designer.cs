namespace MabiChatSpeech
{
    partial class Overlay
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
            label1 = new System.Windows.Forms.Label();
            timer1 = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Yu Gothic UI", 18F);
            label1.ForeColor = System.Drawing.Color.Yellow;
            label1.Location = new System.Drawing.Point(566, 211);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(78, 32);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // timer1
            // 
            timer1.Interval = 50;
            timer1.Tick += timer1_Tick;
            // 
            // Overlay
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.DarkGray;
            ClientSize = new System.Drawing.Size(773, 325);
            ControlBox = false;
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Margin = new System.Windows.Forms.Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Overlay";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Overlay";
            Shown += Overlay_Shown;
            Paint += Overlay_Paint;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
    }
}