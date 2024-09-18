using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MabiChatSpeech;

namespace MabiChatSpeech
{

    internal class MabiChat
    {

        // CharacterNameData Class 
        public class CharacterNameData
        {
            public string CharName;
            public bool Enabled;
            public CharacterTypes CharacterType;
            public string TtsName;
            public int TtsVolume;
            public int TtsSpeed;

            public CharacterNameData(string CharName, bool Enabled, CharacterTypes CharacterType, string TtsName, int TtsVolume, int TtsSpeed)
            {
                this.CharName = CharName;
                this.Enabled = Enabled;
                this.CharacterType = CharacterType;
                this.TtsName = TtsName;
                this.TtsVolume = TtsVolume;
                this.TtsSpeed = TtsSpeed;
            }
            public CharacterNameData(string line)
            {
                ForText(line);
            }
            public CharacterNameData(ListViewItem item)
            {
                ForItem(item);
            }
            public void ForText(string textline)
            {
                string[] f = textline.Split(',');
                this.CharName = f[0];
                this.Enabled = bool.Parse(f[1]);
                switch (f[2])
                {
                    case "User":
                        this.CharacterType = CharacterTypes.User;
                        break;
                    case "Npc":
                        this.CharacterType = CharacterTypes.Npc;
                        break;
                    default:
                        this.CharacterType = (CharacterTypes)Byte.Parse(f[2]);
                        break;
                }
                this.TtsName = f[3];
                this.TtsVolume = int.Parse(f[4]);
                this.TtsSpeed = int.Parse(f[5]);
            }
            public string ToText()
            {
                string retvalue = $"{this.CharName},{this.Enabled},{this.CharacterType},{this.TtsName},{this.TtsVolume},{this.TtsSpeed}";
                return (retvalue);
            }
            public void ForItem(ListViewItem item)
            {
                this.CharName = item.SubItems[0].Text;
                this.Enabled = item.Checked;
                switch (item.SubItems[1].Text)
                {
                    case "User":
                        this.CharacterType = CharacterTypes.User;
                        break;
                    case "Npc":
                        this.CharacterType = CharacterTypes.Npc;
                        break;
                    default:
                        this.CharacterType = (CharacterTypes)Byte.Parse(item.SubItems[1].Text);
                        break;
                }
                this.TtsName = item.SubItems[2].Text;
                this.TtsVolume = int.Parse(item.SubItems[3].Text);
                this.TtsSpeed = int.Parse(item.SubItems[4].Text);
            }
            public ListViewItem ToItem()
            {
                var retvalue = new ListViewItem();
                retvalue.Text = this.CharName;
                retvalue.Checked = this.Enabled;
                retvalue.SubItems.Add($"{this.CharacterType}");
                retvalue.SubItems.Add(this.TtsName);
                retvalue.SubItems.Add($"{this.TtsVolume}");
                retvalue.SubItems.Add($"{this.TtsSpeed}");
                return (retvalue);
            }
        } // CharacterNameData Class - End

        // Character Name List Class
        public class CharacterNameList
        {
            readonly public string filename = "\\CharacterNameList.txt";
            public List<CharacterNameData> CNlist = new List<CharacterNameData>();

            public CharacterNameList()
            {
                FileTextRead();
            }

            public void FileTextRead()
            {

                string fname = Program.__SavePath + filename;

                CNlist.Clear();

                if (File.Exists(@fname) == false)
                {
                    return;
                }
                Debug.Print($"Load Characterlist: {@fname}");

                var lines = File.ReadAllLines(@fname, Encoding.GetEncoding("utf-8"));
                foreach (var line in lines)
                {
                    var data = new CharacterNameData(line);
                    if (data.CharName != null)
                    {
                        CNlist.Add(data);
                    }
                }
                CNlist.Sort((a, b) => a.CharName.CompareTo(b.CharName));

            }
            public void FileTextWrite()
            {
                string fname = Program.__SavePath + filename;
                Debug.Print($"Save Characterlist : {@fname}");

                Encoding enc = Encoding.GetEncoding("utf-8");
                using (StreamWriter writer = new StreamWriter(@fname, false, enc))
                {
                    foreach (var data in CNlist)
                    {
                        writer.WriteLine(data.ToText());
                    }
                    writer.Close();
                }
            }
            public CharacterNameData FindCharacterName(string CName)
            {
                return (CNlist.Find(x => x.CharName == CName));

            }
            public int FindIndexCharacterName(string CName)
            {
                return (CNlist.FindIndex(x => x.CharName == CName));
            }
            public ListViewItem DataToItem(int idx)
            {
                var itemdata = CNlist[idx];

                if (itemdata.CharName != null)
                {
                    return (CNlist[idx].ToItem());
                }

                return (null);
            }
            public CharacterNameData ItemToData(ListViewItem item)
            {

                CharacterNameData itemdata = new CharacterNameData(item);

                return (itemdata);
            }
        } //End of class CharacterNameList

    }
}
