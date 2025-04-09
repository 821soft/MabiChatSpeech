﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MabiChatSpeech
{
    public partial class Overlay : Form
    {
        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);



        static System.Windows.Forms.Control owin;
        public Overlay()
        {
            InitializeComponent();
            BackColor = Color.Red;
            TransparencyKey = BackColor;

        }

        private void Overlay_Shown(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            owin = this;
        }

        private void Overlay_Paint(object sender, PaintEventArgs e)
        {
            /*
            IntPtr desktopDC = GetDC(IntPtr.Zero);
            using (Graphics g = Graphics.FromHdc(desktopDC))
            {
                g.FillEllipse(System.Drawing.Brushes.Red, 100, 100, 30, 30);
            }

            ReleaseDC(IntPtr.Zero, desktopDC);
            */

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //子のラベルの位置を更新
            foreach (Control co in Controls)
            {
                if (co.GetType().Equals(typeof(System.Windows.Forms.Label)))
                {
                    co.Location = new Point(co.Location.X-10, co.Location.Y);

                    if ( co.Location.X > this.Width)
                    {
                        co.Dispose();
                    }
                    if (co.Location.X + co.Width < 0)
                    {
                        co.Dispose();
                    }

                }
            }
        }
        static public void addlabel(int x , int y , string s )
        {
            System.Windows.Forms.Label lb = new System.Windows.Forms.Label();
            lb.AutoSize = true;
            lb.Text = s;
            lb.Location = new Point(x, y);
            owin.Controls.Add(lb);
        }
    }
}
