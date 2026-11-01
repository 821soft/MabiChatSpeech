using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

using static MabiChatSpeech.MabiChat;
using static MabiChatSpeech.Main;
using static MabiChatSpeech.Program;
using static MabiChatSpeech.WinApi;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MabiChatSpeech
{
    public partial class CharaListVoice_Edit : Form
    {
        static private ChatVoice _chatVoice;
        static int aivisPid = -1;
        static int voiceVoxPid = -1;
        private int datatype = -1;
        private bool _updateProgramCharaList = true;

        public string charaname { get; set; } = "";
        public string testword { get; set; } = "";
        internal CharacterNameData? character = new CharacterNameData();
        private ListViewItem DataItem = new ListViewItem();
        public CharaListVoice_Edit(string CharaName)
        {
            InitializeComponent();
            init();
            charaname = CharaName;
            datatype = 0;
            this.Text = "キャラの音声設定:String";

        }
        internal CharaListVoice_Edit(CharacterNameData dat)
            : this(dat, updateProgramCharaList: true)
        {
        }

        internal CharaListVoice_Edit(CharacterNameData dat, bool updateProgramCharaList)
        {
            InitializeComponent();
            init();
            charaname = dat.CharName;
            character = dat;
            _updateProgramCharaList = updateProgramCharaList;
            datatype = 1;
            this.Text = "キャラの音声設定:CharacterNameData";
        }
        internal CharaListVoice_Edit(ListViewItem item)
        {
            InitializeComponent();
            init();
            DataItem = item;
            charaname = DataItem.SubItems[2].Text;
            datatype = 2;
            this.Text = "キャラの音声設定:ListViewItem";
        }
        private async void init()
        {

            _chatVoice = await ChatVoice.CreateMultiEngineAsync();
            await _chatVoice.RefreshSpeakersAsync();

            ConnectQueueEvents();


        }
        private void ConnectQueueEvents()
        {
            if (_chatVoice == null) { return; }

            _chatVoice!.SpeechQueued += e =>
            {
                VoiceEvents(() =>
                    Txt_status.Text = $"{e.PendingCount}件 / {e.Engine}");
            };

            _chatVoice.QueueBecameEmpty += () =>
            {
                VoiceEvents(() =>
                    Txt_status.Text = "Stay");
            };
        }
        private void VoiceEvents(Action update)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            if (InvokeRequired)
                BeginInvoke(update);
            else
                update();
        }

        private async Task<bool> VoiceListPreload()
        {

            // 前提 エンジンが起動していること
            var engsts_VoiceVox = await ChatVoice.IsChatVoiceEngenRunning(ChatVoiceEngine.VoiceVox);
            var engsts_AivisSpeech = await ChatVoice.IsChatVoiceEngenRunning(ChatVoiceEngine.AivisSpeech);

            if ((engsts_VoiceVox) && (engsts_AivisSpeech))
            {
                if (_chatVoice == null)
                {
                    MessageBox.Show("音声リスト生成中.",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return (false);
                }
                await _chatVoice.RefreshSpeakersAsync();
                // 話者の数を確認
                if (_chatVoice.Speakers.Count == 0)
                {
                    MessageBox.Show("話者が０です.",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return (false);
                }
            }
            else
            {
                string msg = "合成音声エンジンが起動されてません";
                if ((!engsts_VoiceVox) && (!engsts_AivisSpeech))
                {
                }
                else if ((!engsts_VoiceVox))
                {
                    msg += "\nVoiceVox";
                }
                else if ((!engsts_AivisSpeech))
                {
                    msg += "\nAivisSpeech";
                }

                MessageBox.Show(msg,
                    "合成音声エンジンエラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return (false);

            }
            return (true);
        }
        private bool ListMakeing = false;
        private void charadata2form()
        {
            ListMakeing = true;
            Txt_TestWord.Text = testword;

            if (datatype == 1)
            {
                //            character = Program.CharaList.FirstOrDefault(x => x.CharName == charaname);
                if (character == null)
                {
                    this.Close();
                    return;
                }

                CHK_Enable.Checked = character.Enabled;
                Cmb_CharaType.Text = character.CharaTypeName.ToString();
                Txt_CharaName.Text = character.CharName;
                var eng_idx = Lst_VoiceEngen.FindStringExact(character.VoiceEngen.ToString());
                Lst_VoiceEngen.SelectedIndex = eng_idx;
                ListBox_VoiceSet(character.VoiceEngen, character.VoiceName);
            }
            else if (datatype == 2)
            {
                CHK_Enable.Checked = bool.Parse(DataItem.Text);
                Txt_CharaName.Text = DataItem.SubItems[1].Text;
                Cmb_CharaType.Text = DataItem.SubItems[2].Text;

                var eng = (ChatVoiceEngine)Enum.Parse(typeof(ChatVoiceEngine), DataItem.SubItems[3].Text);
                var eng_idx = Lst_VoiceEngen.FindStringExact(eng.ToString());
                Lst_VoiceEngen.SelectedIndex = eng_idx;

                ListBox_VoiceSet(eng, DataItem.SubItems[4].Text);
                ;
                //DataItem.SubItems[5].Text;//VoiceID

                Txt_VolumeScal.Text = DataItem.SubItems[6].Text;
                Txt_SpeedScale.Text = DataItem.SubItems[7].Text;
                Txt_PitchScal.Text = DataItem.SubItems[8].Text;
            }
            else
            { }
            if(Lst_VoiceEngen.SelectedIndex == 0)
            {
                store[0].eng = 0;
                store[0].chara = Lst_VoiceChara.SelectedIndex;
                store[0].style = Lst_VoiceStyle.SelectedIndex;
            }
            else if (Lst_VoiceEngen.SelectedIndex == 1)
            {
                store[1].eng = 1;
                store[1].chara = Lst_VoiceChara.SelectedIndex;
                store[1].style = Lst_VoiceStyle.SelectedIndex;
            }
            else
            {
                store[0].eng = 0;
                store[0].chara = 0;
                store[0].style = 0;
                store[1].eng = 1;
                store[1].chara = 0;
                store[1].style = 0;
            }


            ListMakeing = false;

        }

        private void CharaListVoice_Edit_Shown(object sender, EventArgs e)
        {
            Lbl_VoiceEngen.Text = $"エンジン({Main.VoiceLists.Count})";
            charadata2form();

        }

        private int ListBox_VoiceSet(ChatVoiceEngine eng, string voiceName)
        {
            int voiceid = -1;

            if (voiceName != null)
            {
                string[] voicenamesp = voiceName.Split('-');

                Lst_VoiceStyle.Items.Clear();
                Lst_VoiceChara.Items.Clear();
                Lbl_VoiceChara.Text = $"音声キャラ(0)";
                Lbl_VoiceStyle.Text = $"スタイル(0)";

                var vclist = Main.VoiceCharaList(eng);

                foreach (var cv in vclist)
                {
                    Lst_VoiceChara.Items.Add(cv);
                }
                var cv_idx = Lst_VoiceChara.FindStringExact(voicenamesp[0]);
                Lst_VoiceChara.SelectedIndex = cv_idx;

                var vslist = Main.VoiceStyleList(eng, voicenamesp[0]);
                foreach (var cs in vslist)
                {
                    Lst_VoiceStyle.Items.Add(cs);
                }
                var cs_idx = Lst_VoiceStyle.FindStringExact(voicenamesp[1]);
                Lst_VoiceStyle.SelectedIndex = cs_idx;

                Txt_SpeedScale.Text = character.SpeedScale.ToString();
                Txt_VolumeScal.Text = character.VolumeScale.ToString();
                Txt_PitchScal.Text = character.PitchScale.ToString();
                TB_SpeedScal.Value = (int)(character.SpeedScale * 10.0);
                TB_VolumeScal.Value = (int)(character.VolumeScale * 10.0);
                TB_PitchScal.Value = (int)(character.PitchScale * 100.0);


                Lbl_VoiceChara.Text = $"音声キャラ({Lst_VoiceChara.Items.Count})";
                Lbl_VoiceStyle.Text = $"スタイル({Lst_VoiceStyle.Items.Count})";
                voiceid = character.VoiceId;
            }

            return (voiceid);
        }


        private void TB_TtsSpeed_ValueChanged(object sender, EventArgs e)
        {
            Txt_TtsSpeed.Text = TB_TtsSpeed.Value.ToString();
        }

        private void TB_TtsVolume_ValueChanged(object sender, EventArgs e)
        {
            Txt_TtsVolume.Text = TB_TtsVolume.Value.ToString();

        }

        private void TB_SpeedScal_ValueChanged(object sender, EventArgs e)
        {
            float x = float.Parse(TB_SpeedScal.Value.ToString()) / 10.0f;
            Txt_SpeedScale.Text = x.ToString();
        }


        private void TB_VolumeScal_ValueChanged(object sender, EventArgs e)
        {
            float x = float.Parse(TB_VolumeScal.Value.ToString()) / 10.0f;
            Txt_VolumeScal.Text = x.ToString();

        }
        private void TB_PitchScal_ValueChanged(object sender, EventArgs e)
        {
            float x = float.Parse(TB_PitchScal.Value.ToString()) / 100.0f;
            Txt_PitchScal.Text = x.ToString();

        }

        private string Lst_TtsName_Value()
        {
            if (Lst_TtsName.Items.Count <= 0)
            {
                return (string.Empty);
            }
            var ret = Lst_TtsName.SelectedItem.ToString();
            return (ret);
        }

        private ChatVoiceEngine Lst_VoiceEngen_Value()
        {
            ChatVoiceEngine eng = ChatVoiceEngine.None;
            if (Lst_VoiceEngen.Items.Count <= 0)
            {
                return (eng);
            }
            var sel_rec = Lst_VoiceEngen.SelectedItem;
            eng = (ChatVoiceEngine)Enum.Parse(typeof(ChatVoiceEngine), sel_rec.ToString());

            return (eng);
        }
        private string Lst_VoiceName_Value()
        {
            string ret = string.Empty;

            if ((Lst_VoiceChara.Items.Count <= 0) || (Lst_VoiceStyle.Items.Count <= 0))
            {
                return (ret);
            }
            var rec_charaname = Lst_VoiceChara.SelectedItem;
            var rec_stylename = Lst_VoiceStyle.SelectedItem;
            var voicename = rec_charaname.ToString() + "-" + rec_stylename.ToString();
            return (voicename);
        }

        private int Lst_VoiceId_Value()
        {
            int ret = -1;

            if ((Lst_VoiceChara.Items.Count <= 0) || (Lst_VoiceStyle.Items.Count <= 0))
            {
                return (-2);
            }
            var rec_charaname = Lst_VoiceChara.SelectedItem;
            var rec_stylename = Lst_VoiceStyle.SelectedItem;
            var voicename = rec_charaname.ToString() + "-" + rec_stylename.ToString();
            ret = find_VoiceId(voicename);
            return (ret);
        }

        private record selectIdx
        {
            public int eng { get; set; } = -1;
            public int chara { get; set; } = 0;
            public int style { get; set; } = 0;
        };

        private selectIdx []store = new selectIdx[]
        {
           new selectIdx{ eng = 0 , chara = 0 , style = 0 } ,
           new selectIdx{ eng = 1 , chara = 0 , style = 0 } ,
        };
        private void Lst_VoiceEngen_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.Print($"Lst_VoiceEngen_SelectedIndexChanged=({ListMakeing})  e={Lst_VoiceEngen.SelectedIndex} , c={Lst_VoiceChara.SelectedIndex} ,S={Lst_VoiceStyle.SelectedIndex}");
            if (ListMakeing) { return; }
            Lst_VoiceChara.Items.Clear();
            Lst_VoiceStyle.Items.Clear();
            Lbl_VoiceChara.Text = $"音声キャラ(0)";
            Lbl_VoiceStyle.Text = $"スタイル(0)";


            var rec = Lst_VoiceEngen.SelectedItem;

            if (rec == null)
            {
            }

            ChatVoiceEngine eng = (ChatVoiceEngine)Enum.Parse(typeof(ChatVoiceEngine), rec.ToString());
            var vclist = Main.VoiceCharaList(eng);
            foreach (var cv in vclist)
            {
                Lst_VoiceChara.Items.Add(cv);
            }
            Lst_VoiceChara.SelectedIndex = store[Lst_VoiceEngen.SelectedIndex].chara;
            Lbl_VoiceChara.Text = $"音声キャラ({Lst_VoiceChara.Items.Count})";
            Lbl_VoiceStyle.Text = $"スタイル({Lst_VoiceStyle.Items.Count})";

        }

        private void Lst_VoiceChara_SelectedValueChanged(object sender, EventArgs e)
        {

            Debug.Print($"Lst_VoiceChara_SelectedValueChanged=({ListMakeing}) e={Lst_VoiceEngen.SelectedIndex} , c={Lst_VoiceChara.SelectedIndex} ,S={Lst_VoiceStyle.SelectedIndex}");
            if (ListMakeing) { return; }
            Lst_VoiceStyle.Items.Clear();
            Lbl_VoiceStyle.Text = $"スタイル(0)";

            if (Lst_VoiceEngen.SelectedIndex == -1)
            {
                Debug.Print("Lst_VoiceEngen 選択NG");
                return;
            }
            var rec_eng = Lst_VoiceEngen.SelectedItem;
            ChatVoiceEngine eng = (ChatVoiceEngine)Enum.Parse(typeof(ChatVoiceEngine), rec_eng.ToString());

            var rec_chara = Lst_VoiceChara.SelectedItem;
            var vslist = Main.VoiceStyleList(eng, rec_chara.ToString());
            foreach (var cs in vslist)
            {
                Lst_VoiceStyle.Items.Add(cs);
            }
            if (store[Lst_VoiceEngen.SelectedIndex].chara != Lst_VoiceChara.SelectedIndex)
            {
                Lst_VoiceStyle.SelectedIndex = 0;
            }
            else
            {
                Lst_VoiceStyle.SelectedIndex = store[Lst_VoiceEngen.SelectedIndex].style;
            }

            Lbl_VoiceStyle.Text = $"スタイル({Lst_VoiceStyle.Items.Count})";

            store[Lst_VoiceEngen.SelectedIndex].chara = Lst_VoiceChara.SelectedIndex;
            store[Lst_VoiceEngen.SelectedIndex].style = Lst_VoiceStyle.SelectedIndex;

        }

        private void Btn_PlayTest_Click(object sender, EventArgs e)
        {
            ChatVoiceEngine eng = Lst_VoiceEngen_Value();
            var voiceId = Lst_VoiceId_Value();
            float cv = float.Parse(Txt_VolumeScal.Text);
            float cs = float.Parse(Txt_SpeedScale.Text);
            float cp = float.Parse(Txt_PitchScal.Text);

            if (voiceId < 0)
            {
            }
            else if (Txt_TestWord.Text.Length <= 0)
            {
            }
            else
            {
                ChatVoiceSettings p = new ChatVoiceSettings(cs, cv, cp);
                _chatVoice.Enqueue(Txt_TestWord.Text, eng, voiceId, p);
            }
        }

        private void str_dataset()
        {

        }
        private bool dat_dataset()
        {
            _CharaTypeNames ct = (_CharaTypeNames)Enum.Parse(typeof(_CharaTypeNames), Cmb_CharaType.Text.ToString());

            if (!_updateProgramCharaList)
            {
                if (character == null)
                    return false;

                character.Enabled = CHK_Enable.Checked;
                character.CharName = Txt_CharaName.Text;
                character.CharaTypeName = ct;
                character.VoiceEngen = Lst_VoiceEngen_Value();
                character.VoiceName = Lst_VoiceName_Value();
                character.VoiceId = Lst_VoiceId_Value();
                character.VolumeScale = float.Parse(Txt_VolumeScal.Text);
                character.SpeedScale = float.Parse(Txt_SpeedScale.Text);
                character.PitchScale = float.Parse(Txt_PitchScal.Text);
                return true;
            }

            int index = Program.CharaList.FindIndex(x => ((x.CharName == Txt_CharaName.Text) && (x.CharaTypeName == ct)));
            if (index < 0)
            {
                DialogResult result = MessageBox.Show(
                    "CharaListに該当するデータがありません。追加しますか？",
                    "CharaListへの追加",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                    return false;
                if (result == DialogResult.No)
                    return true;

                Program.CharaList.Add(new CharacterNameData
                {
                    Enabled = CHK_Enable.Checked,
                    CharName = Txt_CharaName.Text,
                    CharaTypeName = ct,
                    VoiceEngen = Lst_VoiceEngen_Value(),
                    VoiceName = Lst_VoiceName_Value(),
                    VoiceId = Lst_VoiceId_Value(),
                    VolumeScale = float.Parse(Txt_VolumeScal.Text),
                    SpeedScale = float.Parse(Txt_SpeedScale.Text),
                    PitchScale = float.Parse(Txt_PitchScal.Text)
                });
                return true;

            }
            Program.CharaList[index].Enabled = CHK_Enable.Checked;
            Program.CharaList[index].VoiceEngen = Lst_VoiceEngen_Value();
            Program.CharaList[index].VoiceName = Lst_VoiceName_Value();
            Program.CharaList[index].VoiceId = Lst_VoiceId_Value();
            Program.CharaList[index].VolumeScale = float.Parse(Txt_VolumeScal.Text);
            Program.CharaList[index].SpeedScale = float.Parse(Txt_SpeedScale.Text);
            Program.CharaList[index].PitchScale = float.Parse(Txt_PitchScal.Text);
            return true;

        }
        private void item_dataset()
        {
            _CharaTypeNames ct = (_CharaTypeNames)Enum.Parse(typeof(_CharaTypeNames), Cmb_CharaType.Text.ToString());
            DataItem.Text = CHK_Enable.Checked.ToString();
            DataItem.SubItems[1].Text = Txt_CharaName.Text;
            DataItem.SubItems[2].Text = ct.ToString();
            DataItem.SubItems[3].Text = Lst_VoiceEngen_Value().ToString();
            DataItem.SubItems[4].Text = Lst_VoiceName_Value().ToString();
            DataItem.SubItems[5].Text = Lst_VoiceId_Value().ToString();
            DataItem.SubItems[6].Text = Txt_VolumeScal.Text;
            DataItem.SubItems[7].Text = Txt_SpeedScale.Text;
            DataItem.SubItems[8].Text = Txt_PitchScal.Text;
        }
        private void Btn_Update_Click(object sender, EventArgs e)
        {
            switch (datatype)
            {
                case 0:
                    str_dataset();
                    break;
                case 1:
                    if (!dat_dataset())
                        return;
                    break;
                case 2:
                    item_dataset();
                    break;
                default:
                    break;


            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Btn_Undo_Click(object sender, EventArgs e)
        {
            charadata2form();
            if (Lst_VoiceEngen.SelectedIndex == 0)
            {
                store[1].eng = 1;
                store[1].chara = 0;
                store[1].style = 0;
            }
            else if (Lst_VoiceEngen.SelectedIndex == 1)
            {
                store[0].eng = 0;
                store[0].chara = 0;
                store[0].style = 0;
            }
            else
            {
                store[0].eng = 0;
                store[0].chara = 0;
                store[0].style = 0;
                store[1].eng = 1;
                store[1].chara = 0;
                store[1].style = 0;
            }
        }

        private void CHK_Enable_CheckedChanged(object sender, EventArgs e)
        {

        }

        private async void CharaListVoice_Edit_Load(object sender, EventArgs e)

        {

            bool sts = await VoiceListPreload();
            if (sts == false)
            {

                this.Close();
            }
        }

        private async void Dispose_Voice()
        {
            await _chatVoice.DisposeAsync();
        }
        private void CharaListVoice_Edit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_chatVoice != null)
            {
                Dispose_Voice();
            }

        }

        private void Lst_VoiceStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
                store[Lst_VoiceEngen.SelectedIndex].chara = Lst_VoiceChara.SelectedIndex;
                store[Lst_VoiceEngen.SelectedIndex].style = Lst_VoiceStyle.SelectedIndex;
        }
    }
}
