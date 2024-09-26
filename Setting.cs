using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MabiChatSpeech
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }
        public Color ChatFColor = Program.Color16List[Program.__ChatFColor];
        public Color ChatBColor = Program.Color16List[Program.__ChatBColor];
        private void CMB_FF_DrawItem(object sender,  System.Windows.Forms.DrawItemEventArgs e)
        {
            var cmb = (System.Windows.Forms.ComboBox)sender;
            if ( e.Index == -1 )
            {
                return;
            }
            float size = cmb.Font.Size;
            System.Drawing.Font myFont;
            myFont = (Font)cmb.Items[e.Index];

            System.Drawing.Color animalColor = ChatBColor;

            // Draw the background of the item.
            e.DrawBackground();

            // Create a square filled with the animals color. Vary the size
            // of the rectangle based on the length of the animals name.
            Rectangle rectangle = new Rectangle(2, e.Bounds.Top + 2,
                    e.Bounds.Height, e.Bounds.Height - 4);
            e.Graphics.FillRectangle(new SolidBrush(animalColor), rectangle);

            // Draw each string in the array, using a different size, color,
            // and font for each item.
            var color = new System.Drawing.SolidBrush(ChatFColor);

            e.Graphics.DrawString(myFont.Name, myFont, color, new RectangleF(e.Bounds.X + rectangle.Width, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }

        private void CMB_Color_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            var cmb = (System.Windows.Forms.ComboBox)sender;
            if (e.Index == -1)
            {
                return;
            }
            float size = cmb.Font.Size;
            System.Drawing.Font myFont;
            FontFamily family = (FontFamily)cmb.Font.FontFamily;

            System.Drawing.Color animalColor = (Color)cmb.Items[e.Index];

            // Draw the background of the item.
            e.DrawBackground();

            // Create a square filled with the animals color. Vary the size
            // of the rectangle based on the length of the animals name.
            Rectangle rectangle = new Rectangle(2, e.Bounds.Top + 2,
                    e.Bounds.Height, e.Bounds.Height - 4);
            e.Graphics.FillRectangle(new SolidBrush(animalColor), rectangle);

            // Draw each string in the array, using a different size, color,
            // and font for each item.
            myFont = new Font(family, size, FontStyle.Regular);
            e.Graphics.DrawString(animalColor.Name, myFont, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + rectangle.Width, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Setting_Shown(object sender, EventArgs e)
        {

            // データセット
            Txt_CapProgram.Text = Program.__CapureProgram;
            Txt_CapPort.Text = $"{ Program.__CapturePort}";

            Cmb_SaveMode.SelectedIndex = Program.__SaveMode;
            Txt_SavePath.Text = Program.__SavePath;

            CMB_FColor.SelectedIndex = Program.__ChatFColor;
            ChatFColor = Program.Color16List[CMB_FColor.SelectedIndex];

            CMB_BColor.SelectedIndex = Program.__ChatBColor;
            ChatBColor = Program.Color16List[CMB_BColor.SelectedIndex];


            Control[] cs = Program.Frm_Main.Controls.Find("Txt_Chat",true);
            CMB_FF.Font = cs[0].Font;
            CMB_FF.ForeColor = Program.Color16List[Program.__ChatFColor];
            CMB_FF.BackColor = Program.Color16List[Program.__ChatBColor];
            int idx = 0;
            foreach(Font f in CMB_FF.Items)
            {
                if (f.Name == CMB_FF.Font.Name)
                {
                    CMB_FF.SelectedIndex = idx;
                    break;
                }
                idx++;
            }

            CMB_FontSize.Text = $"{Program.__ChatFontSize}";


            Cmb_Whitelist.SelectedIndex = Program.__ChatSelWhitelist;
            Cmb_User.SelectedIndex = Program.__ChatSelUser;
            Cmb_NPC.SelectedIndex = Program.__ChatSelNpc;

            Cmb_TTS1Name.Text = Program.__TTS1Name;
            Nud_TTS1Volume.Value = Program.__TTS1Volume;
            Nud_TTS1Speed.Value = Program.__TTS1Speed;
            Cmb_TTS2Name.Text = Program.__TTS2Name;
            Nud_TTS2Volume.Value = Program.__TTS2Volume;
            Nud_TTS2Speed.Value = Program.__TTS2Speed;
            Chk_TTSMute.Checked = Program.__TTS_Mute;
            Chk_TTSNameCall.Checked = Program.__TTS_NameCall;

            CHK_AutoAdd.Checked = Program.__WhiteList_AutoAdd ;
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            // データセット
            Program.__CapureProgram = Txt_CapProgram.Text ;
            Program.__CapturePort = uint.Parse(Txt_CapPort.Text) ;

            Program.__SaveMode = Cmb_SaveMode.SelectedIndex ;
            Program.__SavePath = Txt_SavePath.Text ;
            Program.__ChatSelWhitelist = Cmb_Whitelist.SelectedIndex ;
            Program.__ChatSelUser = Cmb_User.SelectedIndex ;
            Program.__ChatSelNpc = Cmb_NPC.SelectedIndex ;

            Program.__TTS1Name = Cmb_TTS1Name.Text ;
            Program.__TTS1Volume = (int)(Nud_TTS1Volume.Value);
            Program.__TTS1Speed = (int)Nud_TTS1Speed.Value;
            Program.__TTS2Name = Cmb_TTS2Name.Text;
            Program.__TTS2Volume = (int)Nud_TTS2Volume.Value ;
            Program.__TTS2Speed = (int)Nud_TTS2Speed.Value ;
            Program.__TTS_Mute = Chk_TTSMute.Checked;
            Program.__TTS_NameCall = Chk_TTSNameCall.Checked;
            Program.__ChatFColor = CMB_FColor.SelectedIndex;
            Program.__ChatBColor = CMB_BColor.SelectedIndex;
            Program.__ChatFontSize = float.Parse(CMB_FontSize.Text);


            Font myFont = (Font)CMB_FF.SelectedItem;
            float size = Program.__ChatFontSize;
            Font f = new Font(myFont.FontFamily, size, FontStyle.Regular);

            Control[] cs = Program.Frm_Main.Controls.Find("Txt_Chat", true);
            cs[0].Font = f;
            cs[0].ForeColor = ChatFColor;
            cs[0].BackColor = ChatBColor;


            Program.__ChatFontName = myFont.Name;
            Program.__ChatFontSize = size;
            Program.__WhiteList_AutoAdd =CHK_AutoAdd.Checked;

            Properties.Settings.Default.Save();
            this.Close();
            Program.Frm_Main.settingupd();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_ServerInfo_Click(object sender, EventArgs e)
        {
            ServerInfo Frm_ServerInfo = new ServerInfo();
            Frm_ServerInfo.Owner = this;
            Frm_ServerInfo.ShowDialog();
        }

        private void Txt_SavePath_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.Description = Txt_SavePath.Text;
            fbd.ShowNewFolderButton = true;
            fbd.SelectedPath = Txt_SavePath.Text;

            if ( fbd.ShowDialog() == DialogResult.OK )
            {
                Txt_SavePath.Text = fbd.SelectedPath;
            }
            
        }

        private void Btn_WhiteList_Click(object sender, EventArgs e)
        {
            WhiteList Frm_WhiteList = new WhiteList();
            Frm_WhiteList.Owner = this;
            if ( Frm_WhiteList.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            // TTS
            Cmb_TTS1Name.Items.Clear();
            Cmb_TTS2Name.Items.Clear();
            foreach (var s in Program.TTS_NameList)
            {
                Cmb_TTS1Name.Items.Add(s);
            }
            foreach (var s in Program.TTS_NameList)
            {
                Cmb_TTS2Name.Items.Add(s);
            }
            //Font
            CMB_FColor.Items.Clear();
            foreach (Color c in Program.Color16List)
            {
                CMB_FColor.Items.Add(c);
            }
            this.CMB_FColor.DrawItem += new DrawItemEventHandler(CMB_Color_DrawItem);

            CMB_BColor.Items.Clear();
            foreach (Color c in Program.Color16List)
            {
                CMB_BColor.Items.Add(c);
            }
            this.CMB_BColor.DrawItem += new DrawItemEventHandler(CMB_Color_DrawItem);

            CMB_FF.Items.Clear();
            foreach (Font ff in Program.FontItemList)
            {
                CMB_FF.Items.Add(ff);
            }
            this.CMB_FF.DrawItem += new DrawItemEventHandler(CMB_FF_DrawItem);
        }

        private void CMB_BColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CMB_FF.BackColor = Program.Color16List[CMB_BColor.SelectedIndex];
            ChatBColor = Program.Color16List[CMB_BColor.SelectedIndex];
        }

        private void CMB_FColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CMB_FF.ForeColor = Program.Color16List[CMB_FColor.SelectedIndex];
            ChatFColor = Program.Color16List[CMB_FColor.SelectedIndex];
        }
    }
}
