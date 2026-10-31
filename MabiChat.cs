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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MabiChatSpeech
{

    internal class MabiChat
    {

    public class ChatLogEntry
    {
        public string Type { get; set; } = "";
        public int Number { get; set; }
        public TimeSpan Time { get; set; }
        public _CharaTypeNames  CharaTypeName { get; set; }
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
                        out _CharaTypeNames CharaTypeName))
                {
                        CharaTypeName = _CharaTypeNames.UNKNOWN;
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
                    CharaTypeName = CharaTypeName,
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
            public bool Enabled { get; set; }
            public string CharName { get; set; } = string.Empty;
            public _CharaTypeNames CharaTypeName { get; set; } = _CharaTypeNames.UNKNOWN;
            public ChatVoiceEngine VoiceEngen { get; set; } = ChatVoiceEngine.VoiceVox;
            public string VoiceName { get; set; } = "ずんだもん-ノーマル";
            public int VoiceId { get; set; } = 3;
            public float VolumeScale { get; set; } = 1.0f;
            public float SpeedScale { get; set; } = 1.0f;
            public float PitchScale { get; set; } = 0.0f;

            public CharacterNameData(
                bool enabled ,
                string charName,
                _CharaTypeNames charaNameType,
                ChatVoiceEngine VoiceEngen=ChatVoiceEngine.VoiceVox,
                string voiceName = "ずんだもん-ノーマル",
                int voiceId= 3,
                float volumeScale = 1.0f,
                float speedScale = 1.0f,
                float pitchScale = 0.0f)
            {
                Enabled = enabled;
                CharName = charName;
                CharaTypeName = charaNameType;
                VoiceName = voiceName;
                VoiceId = voiceId;
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

                if (fields.Length < 9)
                {
                    throw new FormatException("CharacterNameDataの行には、最低9項目が必要です。");
                }

                Enabled = bool.Parse(fields[0]);
                CharName = fields[1];
                CharaTypeName = (_CharaTypeNames)Enum.Parse(typeof(_CharaTypeNames), fields[2]);

                if (fields.Length > 3 && Enum.TryParse(fields[3], out ChatVoiceEngine voice))
                {
                    VoiceEngen = voice;
                }
                else
                {
                    VoiceEngen = ChatVoiceEngine.None;
                }
                VoiceName = fields.Length > 4 ? fields[4] : string.Empty;
                VoiceId = fields.Length > 5 ? int.Parse(fields[5]) : -1;
                VolumeScale = fields.Length > 6 ? float.Parse(fields[6], CultureInfo.InvariantCulture) : 1.0f;
                SpeedScale = fields.Length > 7 ? float.Parse(fields[7], CultureInfo.InvariantCulture) : 1.0f;
                PitchScale = fields.Length > 8 ? float.Parse(fields[8], CultureInfo.InvariantCulture) : 0.0f;
            }

            public string ToText()
            {
                return string.Join(",",
                    Enabled,
                    CharName,
                    CharaTypeName,
                    VoiceEngen,
                    VoiceName,
                    VoiceId,
                    VolumeScale.ToString(CultureInfo.InvariantCulture),
                    SpeedScale.ToString(CultureInfo.InvariantCulture),
                    PitchScale.ToString(CultureInfo.InvariantCulture));
            }

            public void ForItem(ListViewItem item)
            {
                Enabled = bool.Parse(item.Text);
                CharName = item.SubItems[1].Text;

                CharaTypeName = (_CharaTypeNames)Enum.Parse(typeof(_CharaTypeNames), item.SubItems[2].Text);


                if (item.SubItems.Count > 3  && Enum.TryParse(item.SubItems[3].Text, out ChatVoiceEngine voice))
                {
                    VoiceEngen = voice;
                }
                else
                {
                    VoiceEngen = ChatVoiceEngine.None;
                }

                VoiceName = item.SubItems.Count > 4 ? item.SubItems[4].Text : string.Empty;
                VoiceId = item.SubItems.Count > 5 ? int.Parse(item.SubItems[5].Text) : 0;
                VolumeScale = item.SubItems.Count > 6
                    ? float.Parse(item.SubItems[6].Text, CultureInfo.InvariantCulture)
                    : 1.0f;
                SpeedScale = item.SubItems.Count > 7
                    ? float.Parse(item.SubItems[7].Text, CultureInfo.InvariantCulture)
                    : 1.0f;
                PitchScale = item.SubItems.Count > 8
                    ? float.Parse(item.SubItems[8].Text, CultureInfo.InvariantCulture)
                    : 0.0f;
            }

            public ListViewItem ToItem()
            {
                var item = new ListViewItem(Enabled.ToString());
                item.SubItems.Add(CharName.ToString());
                item.SubItems.Add(CharaTypeName.ToString());
                item.SubItems.Add(VoiceEngen.ToString());
                item.SubItems.Add(VoiceName);
                item.SubItems.Add(VoiceId.ToString());
                item.SubItems.Add(VolumeScale.ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(SpeedScale.ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(PitchScale.ToString(CultureInfo.InvariantCulture));

                return item;
            }
        }
    } //End of class CharacterNameList

}

