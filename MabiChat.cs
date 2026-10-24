using MabiChatSpeech;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using VoicevoxClientSharp.ApiClient;
using VoicevoxClientSharp.ApiClient.Models;
using static MabiChatSpeech.MabiChat;
using static System.Windows.Forms.DataFormats;

namespace MabiChatSpeech
{

    internal class MabiChat
    {

    public class ChatLogEntry
    {
        public string Type { get; set; } = "";
        public int Number { get; set; }
        public TimeSpan Time { get; set; }
        public CharacterTypes CharaType { get; set; }
        public string CharaName { get; set; } = "";
        public string Chat { get; set; } = "";

        // 前のレコードからの経過秒数。先頭レコードは0秒。
        public double IntervalSeconds { get; set; }
    }

    public class ChatLogManager
    {
        public List<ChatLogEntry> Logs { get; } = new List<ChatLogEntry>();

        public void Load(string fileName)
        {
            Logs.Clear();

            TimeSpan? previousTime = null;

            foreach (string line in File.ReadLines(fileName))
            {
                // 例: C 1,21:43:25.643,PC ,ドミヌーラ,こんばんはー
                // 分割数を5に制限し、Chat内のカンマは残す。
                string[] parts = line.Split(new[] { ',' }, 5);

                if (parts.Length < 5)
                    continue;

                string[] typeAndNumber = parts[0].Trim().Split(
                    new[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);

                if (typeAndNumber.Length != 2 ||
                    !int.TryParse(typeAndNumber[1], out int number) ||
                    !TimeSpan.TryParseExact(
                        parts[1].Trim(),
                        @"hh\:mm\:ss\.fff",
                        CultureInfo.InvariantCulture,
                        out TimeSpan time))
                {
                    continue;
                }

                if (!Enum.TryParse(
                        parts[2].Trim(),
                        out CharacterTypes CharaType))
                {
                    CharaType = CharacterTypes.User;
                }

                double intervalSeconds = 0;

                if (previousTime.HasValue)
                {
                    TimeSpan interval = time - previousTime.Value;

                    // 時刻が前のレコードより早ければ、日付をまたいだとみなす。
                    if (interval < TimeSpan.Zero)
                        interval += TimeSpan.FromDays(1);

                    intervalSeconds = interval.TotalSeconds;
                }

                Logs.Add(new ChatLogEntry
                {
                    Type = typeAndNumber[0],
                    Number = number,
                    Time = time,
                    CharaType = CharaType,
                    CharaName = parts[3].Trim(),
                    Chat = parts[4],
                    IntervalSeconds = intervalSeconds
                });

                previousTime = time;
            }
        }
    }


    // CharacterNameData Class 
    public record CharacterNameData
        {
            public string CharName { get; set; } = string.Empty;
            public bool Enabled { get; set; }
            public CharacterTypes CharacterType { get; set; }
            public string TtsName { get; set; } = string.Empty;
            public int TtsVolume { get; set; }
            public int TtsSpeed { get; set; }

            public string StyleName { get; set; } = string.Empty;
            public int StyleID { get; set; }
            public float VolumeScale { get; set; } = 1.0f;
            public float SpeedScale { get; set; } = 1.0f;
            public float PitchScale { get; set; }

            public CharacterNameData(
                string charName,
                bool enabled,
                CharacterTypes characterType,
                string ttsName,
                int ttsVolume,
                int ttsSpeed,
                string styleName = "",
                int styleID = 0,
                float volumeScale = 1.0f,
                float speedScale = 1.0f,
                float pitchScale = 0.0f)
            {
                CharName = charName;
                Enabled = enabled;
                CharacterType = characterType;
                TtsName = ttsName;
                TtsVolume = ttsVolume;
                TtsSpeed = ttsSpeed;
                StyleName = styleName;
                StyleID = styleID;
                VolumeScale = volumeScale;
                SpeedScale = speedScale;
                PitchScale = pitchScale;
            }

            public CharacterNameData(string line)
            {
                ForText(line);
            }

            public CharacterNameData(ListViewItem item)
            {
                ForItem(item);
            }

            public CharacterNameData()
            {
            }

            public void ForText(string textline)
            {
                string[] fields = textline.Split(',');

                if (fields.Length < 6)
                {
                    throw new FormatException("CharacterNameDataの行には、最低6項目が必要です。");
                }

                CharName = fields[0];
                Enabled = bool.Parse(fields[1]);

                CharacterType = fields[2] switch
                {
                    "User" => CharacterTypes.User,
                    "Npc" => CharacterTypes.Npc,
                    "Pet" => CharacterTypes.Pet,
                    _ => (CharacterTypes)byte.Parse(fields[2])
                };

                TtsName = fields[3];
                TtsVolume = int.Parse(fields[4]);
                TtsSpeed = int.Parse(fields[5]);

                // 旧形式（6項目）では追加メンバーに既定値を設定
                StyleName = fields.Length > 6 ? fields[6] : string.Empty;
                StyleID = fields.Length > 7 ? int.Parse(fields[7]) : 0;
                VolumeScale = fields.Length > 9 ? float.Parse(fields[8], CultureInfo.InvariantCulture) : 1.0f;
                SpeedScale = fields.Length > 8 ? float.Parse(fields[9], CultureInfo.InvariantCulture) : 1.0f;
                PitchScale = fields.Length > 10 ? float.Parse(fields[10], CultureInfo.InvariantCulture) : 0.0f;
            }

            public string ToText()
            {
                return string.Join(",",
                    CharName,
                    Enabled,
                    CharacterType,
                    TtsName,
                    TtsVolume,
                    TtsSpeed,
                    StyleName,
                    StyleID,
                    VolumeScale.ToString(CultureInfo.InvariantCulture),
                    SpeedScale.ToString(CultureInfo.InvariantCulture),
                    PitchScale.ToString(CultureInfo.InvariantCulture));
            }

            public void ForItem(ListViewItem item)
            {
                CharName = item.SubItems[0].Text;
                Enabled = item.Checked;

                string characterType = item.SubItems[1].Text;
                CharacterType = characterType switch
                {
                    "User" => CharacterTypes.User,
                    "Npc" => CharacterTypes.Npc,
                    "Pet" => CharacterTypes.Pet,
                    _ => (CharacterTypes)byte.Parse(characterType)
                };

                TtsName = item.SubItems[2].Text;
                TtsVolume = int.Parse(item.SubItems[3].Text);
                TtsSpeed = int.Parse(item.SubItems[4].Text);

                StyleName = item.SubItems.Count > 5 ? item.SubItems[5].Text : string.Empty;
                StyleID = item.SubItems.Count > 6 ? int.Parse(item.SubItems[6].Text) : 0;
                VolumeScale = item.SubItems.Count > 7
                    ? float.Parse(item.SubItems[7].Text, CultureInfo.InvariantCulture)
                    : 1.0f;
                SpeedScale = item.SubItems.Count > 8
                    ? float.Parse(item.SubItems[8].Text, CultureInfo.InvariantCulture)
                    : 1.0f;
                PitchScale = item.SubItems.Count > 9
                    ? float.Parse(item.SubItems[9].Text, CultureInfo.InvariantCulture)
                    : 0.0f;
            }

            public ListViewItem ToItem()
            {
                var item = new ListViewItem
                {
                    Text = CharName,
                    Checked = Enabled
                };

                item.SubItems.Add(CharacterType.ToString());
                item.SubItems.Add(TtsName);
                item.SubItems.Add(TtsVolume.ToString());
                item.SubItems.Add(TtsSpeed.ToString());
                item.SubItems.Add(StyleName);
                item.SubItems.Add(StyleID.ToString());
                item.SubItems.Add(VolumeScale.ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(SpeedScale.ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(PitchScale.ToString(CultureInfo.InvariantCulture));

                return item;
            }
        }
    } //End of class CharacterNameList

}

