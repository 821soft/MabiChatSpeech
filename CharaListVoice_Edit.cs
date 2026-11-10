using Microsoft.VisualBasic;
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
        static int aivisPid = -1;
        static int voiceVoxPid = -1;
        private int datatype = -1;
        private bool _updateProgramCharaList = true;

        public string charaname { get; set; } = "";
        public string testword { get; set; } = "";
        internal CharacterNameData? character = new CharacterNameData();
        private ListViewItem DataItem = new ListViewItem();
/*
        public CharaListVoice_Edit(string CharaName)
        {
            InitializeComponent();
            init();
            charaname = CharaName;
            datatype = 0;
            this.Text = "キャラの音声設定:String";

        }
*/
        internal CharaListVoice_Edit(CharacterNameData dat)
            : this(dat, updateProgramCharaList: true)
        {
            this.Text = $"キャラの音声設定:CharacterNameData1";
        }

        internal CharaListVoice_Edit(CharacterNameData dat, bool updateProgramCharaList)
        {
            InitializeComponent();
            init();
            charaname = dat.CharName;
            character = dat;
            _updateProgramCharaList = updateProgramCharaList;
            datatype = 1;
            this.Text = $"キャラの音声設定:updateProgramCharaList2";
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
        private void init()
        {
            // TTSはChatVoiceや外部話者リストがなくても設定できる。
            ConnectQueueEvents();
        }
        private void ConnectQueueEvents()
        {
            if (Main._chatVoice == null) { return; }

            Main._chatVoice.SpeechQueued += EditChatVoice_SpeechQueued;
            Main._chatVoice.QueueBecameEmpty += EditChatVoice_QueueBecameEmpty;
        }
        private void EditChatVoice_SpeechQueued(ChatVoice.SpeechQueueEventArgs e) =>
            VoiceEvents(() => Txt_status.Text = $"{e.PendingCount}件 / {e.Engine}");

        private void EditChatVoice_QueueBecameEmpty() =>
            VoiceEvents(() => Txt_status.Text = "Stay");
        private void VoiceEvents(Action update)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            if (InvokeRequired)
                BeginInvoke(update);
            else
                update();
        }

        private Task<bool> VoiceListPreload() => Task.FromResult(true);
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
                var eng_idx = Lst_VoiceEngen.FindStringExact(EngineDisplayName(character.VoiceEngen));
                Lst_VoiceEngen.SelectedIndex = eng_idx;
                ListBox_VoiceSet(character.VoiceEngen, character.VoiceName);
                Txt_VolumeScal.Text = character.VolumeScale.ToString();
                Txt_SpeedScale.Text = character.SpeedScale.ToString();
                Txt_PitchScal.Text = character.PitchScale.ToString();

                if ((Main.SpekaerCount(character.VoiceEngen) <= 0)&&(character.VoiceEngen!=ChatVoiceEngine.TTS))
                {
                    this.Close();
                    MessageBox.Show(
                        $"音声合成({character.VoiceEngen})が起動していません。",
                        "音声合成",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if (datatype == 2)
            {
                CHK_Enable.Checked = bool.Parse(DataItem.Text);
                Txt_CharaName.Text = DataItem.SubItems[1].Text;
                Cmb_CharaType.Text = DataItem.SubItems[2].Text;

                var eng = ParseEngineName(DataItem.SubItems[3].Text);
                var eng_idx = Lst_VoiceEngen.FindStringExact(EngineDisplayName(eng));
                Lst_VoiceEngen.SelectedIndex = eng_idx;

                ListBox_VoiceSet(eng, DataItem.SubItems[4].Text);
                ;
                //DataItem.SubItems[5].Text;//VoiceID

                Txt_VolumeScal.Text = DataItem.SubItems[6].Text;
                Txt_SpeedScale.Text = DataItem.SubItems[7].Text;
                Txt_PitchScal.Text = DataItem.SubItems[8].Text;

                if ((Main.SpekaerCount(eng) <= 0) && (eng!=ChatVoiceEngine.TTS))
                {
                    this.Close();
                    MessageBox.Show(
                        $"音声合成({eng})が起動していません。",
                        "音声合成",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }

            }
            else
            { }
            ListMakeing = false;

        }

        private void CharaListVoice_Edit_Shown(object sender, EventArgs e)
        {
            var uniqueSet = new HashSet<string>();

            Lbl_VoiceEngen.Text = $"音声種別({Main._chatVoice?.Speakers.Count ?? 0})";
            Lst_VoiceEngen.Items.Clear();
            Lst_VoiceEngen.Items.Add("TTS");
            uniqueSet.Add("VoiceVoxAPI");
            uniqueSet.Add("AivisSpeech");
            if (Main._chatVoice != null)
            {
                foreach (var vo in Main._chatVoice.Speakers)
                    uniqueSet.Add(EngineDisplayName(vo.Engine));
            }
            foreach (var engeinName in uniqueSet)
            {
                Lst_VoiceEngen.Items.Add(engeinName);
            }
            Lst_VoiceEngen.SelectedIndex= 0;
            charadata2form();

        }

        private int ListBox_VoiceSet(ChatVoiceEngine eng, string voiceName)
        {
            int voiceid = -1;

            Lst_VoiceStyle.Items.Clear();
            Lst_VoiceChara.Items.Clear();
            if (eng == ChatVoiceEngine.TTS)
            {
                Lbl_VoiceChara.Text = "音声キャラ(不要)";
                Lbl_VoiceStyle.Text = "スタイル(不要)";
                return voiceid;
            }

            if (Main._chatVoice == null)
                return -1;

            if (voiceName.Length != 0)
            {
                string[] voicenamesp = voiceName.Split('-');

                Lst_VoiceStyle.Items.Clear();
                Lst_VoiceChara.Items.Clear();
                Lbl_VoiceChara.Text = $"音声キャラ(0)";
                Lbl_VoiceStyle.Text = $"スタイル(0)";

                var vclist = new HashSet<string>();
                foreach (var l in Main._chatVoice.Speakers)
                {
                    if (l.Engine == eng)
                    {
                        vclist.Add(l.SpeakerName);
                    }
                }

                foreach (var cv in vclist)
                {
                    Lst_VoiceChara.Items.Add(cv);
                }
                var cv_idx = Lst_VoiceChara.FindStringExact(voicenamesp[0]);
                Lst_VoiceChara.SelectedIndex = cv_idx;

                foreach (var cs in Main._chatVoice.Speakers)
                {
                    if ((cs.Engine==eng)&&(cs.SpeakerName== voicenamesp[0]))
                    {
                        Lst_VoiceStyle.Items.Add(cs.StyleName);
                    }
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

        private ChatVoiceEngine Lst_VoiceEngen_Value()
        {
            ChatVoiceEngine eng = ChatVoiceEngine.TTS;
            if (Lst_VoiceEngen.Items.Count <= 0)
            {
                return (eng);
            }
            var sel_rec = Lst_VoiceEngen.SelectedItem;
            eng = ParseEngineName(sel_rec.ToString());

            return (eng);
        }

        private static ChatVoiceEngine ParseEngineName(string engineName) =>
            string.Equals(engineName, "VoiceVoxAPI", StringComparison.OrdinalIgnoreCase)
                ? ChatVoiceEngine.VoiceVox
                : Enum.Parse<ChatVoiceEngine>(engineName, ignoreCase: true);

        private static string EngineDisplayName(ChatVoiceEngine engine) =>
            engine == ChatVoiceEngine.VoiceVox ? "VoiceVoxAPI" : engine.ToString();

        private static int StoreIndexForEngine(ChatVoiceEngine engine) => engine switch
        {
            ChatVoiceEngine.VoiceVox => 0,
            ChatVoiceEngine.AivisSpeech => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(engine))
        };

        private string Lst_VoiceName_Value()
        {
            string ret = string.Empty;

            if (Lst_VoiceEngen_Value() == ChatVoiceEngine.TTS)
                return Program.__TTS1Name;

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

            if (Lst_VoiceEngen_Value() == ChatVoiceEngine.TTS || Main._chatVoice == null)
                return ret;

            if ((Lst_VoiceChara.Items.Count <= 0) || (Lst_VoiceStyle.Items.Count <= 0))
            {
                return (-2);
            }
            var rec_charaname = Lst_VoiceChara.SelectedItem.ToString();
            var rec_stylename = Lst_VoiceStyle.SelectedItem.ToString();
            var voicename = rec_charaname.ToString() + "-" + rec_stylename.ToString();

            foreach (var spk in Main._chatVoice.Speakers)
            {
                if (spk.Engine == Lst_VoiceEngen_Value())
                { 
                    if(( spk.SpeakerName == rec_charaname)&&(spk.StyleName==rec_stylename))
                    {
                        ret = spk.StyleId;

                        break;
                    }
                }
            }
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

            ChatVoiceEngine eng = ParseEngineName(rec.ToString());
            if (eng == ChatVoiceEngine.TTS || Main._chatVoice == null)
            {
                Lbl_VoiceChara.Text = "音声キャラ(不要)";
                Lbl_VoiceStyle.Text = "スタイル(不要)";
                return;
            }
            int storeIndex = StoreIndexForEngine(eng);
            var vclist = new HashSet<string>();
            foreach (var l in Main._chatVoice.Speakers)
            {
                if (l.Engine == eng)
                {
                    vclist.Add(l.SpeakerName);
                }

            }
            foreach (var cv in vclist)
            {
                Lst_VoiceChara.Items.Add(cv);
            }
            if (Lst_VoiceChara.Items.Count > 0)
                Lst_VoiceChara.SelectedIndex = Math.Clamp(store[storeIndex].chara, 0, Lst_VoiceChara.Items.Count - 1);
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
            ChatVoiceEngine eng = ParseEngineName(rec_eng.ToString());
            if (eng == ChatVoiceEngine.TTS || Main._chatVoice == null || Lst_VoiceChara.SelectedItem == null)
                return;
            int storeIndex = StoreIndexForEngine(eng);

            var rec_chara = Lst_VoiceChara.SelectedItem.ToString();
            foreach (var cs in Main._chatVoice.Speakers)
            {
                if ((cs.Engine == eng) && (cs.SpeakerName == rec_chara))
                {
                    Lst_VoiceStyle.Items.Add(cs.StyleName);
                }

            }
            if (store[storeIndex].chara != Lst_VoiceChara.SelectedIndex)
            {
                if (Lst_VoiceStyle.Items.Count > 0)
                    Lst_VoiceStyle.SelectedIndex = 0;
            }
            else
            {
                if (Lst_VoiceStyle.Items.Count > 0)
                    Lst_VoiceStyle.SelectedIndex = Math.Clamp(store[storeIndex].style, 0, Lst_VoiceStyle.Items.Count - 1);
            }

            Lbl_VoiceStyle.Text = $"スタイル({Lst_VoiceStyle.Items.Count})";

            store[storeIndex].chara = Lst_VoiceChara.SelectedIndex;
            store[storeIndex].style = Lst_VoiceStyle.SelectedIndex;

        }

        private void Btn_PlayTest_Click(object sender, EventArgs e)
        {
            ChatVoiceEngine eng = Lst_VoiceEngen_Value();
            if (eng == ChatVoiceEngine.TTS)
            {
                if (!string.IsNullOrWhiteSpace(Txt_TestWord.Text))
                    Program.Frm_Main.speech_chat(Program.__TTS1Name, Program.__TTS1Volume, Program.__TTS1Speed, Txt_CharaName.Text, Txt_TestWord.Text);
                return;
            }
            var voiceId = Lst_VoiceId_Value();
            float cv = float.Parse(Txt_VolumeScal.Text);
            float cs = float.Parse(Txt_SpeedScale.Text);
            float cp = float.Parse(Txt_PitchScal.Text);

            if (Main._chatVoice == null || voiceId < 0)
            {
            }
            else if (Txt_TestWord.Text.Length <= 0)
            {
            }
            else
            {
                ChatVoiceSettings p = new ChatVoiceSettings(cs, cv, cp);
                Main._chatVoice.Enqueue(Txt_TestWord.Text, eng, voiceId, p);
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
                    "音声割当ファイル（CharacterNameList.txt）に該当するデータがありません。追加しますか？",
                    "音声割当ファイルへの追加",
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

        private void CharaListVoice_Edit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Main._chatVoice != null)
            {
                Main._chatVoice.SpeechQueued -= EditChatVoice_SpeechQueued;
                Main._chatVoice.QueueBecameEmpty -= EditChatVoice_QueueBecameEmpty;
            }

        }

        private void Lst_VoiceStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListMakeing || Lst_VoiceEngen.SelectedItem == null)
                return;
            ChatVoiceEngine engine = ParseEngineName(Lst_VoiceEngen.SelectedItem.ToString());
            if (engine == ChatVoiceEngine.TTS)
                return;
            int storeIndex = StoreIndexForEngine(engine);
            store[storeIndex].chara = Lst_VoiceChara.SelectedIndex;
            store[storeIndex].style = Lst_VoiceStyle.SelectedIndex;
        }
    }
}
