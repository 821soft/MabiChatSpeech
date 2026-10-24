using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MabiChatSpeech
{


    public sealed class ChatVoice : IAsyncDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        private readonly Channel<SpeechRequest> _queue;
        private readonly CancellationTokenSource _shutdown = new();
        private readonly Task _worker;

        private readonly MixingSampleProvider _mixer;
        private readonly WaveOutEvent _output = new();
        private readonly ConcurrentDictionary<OwnedSampleProvider, byte>
            _activeInputs = new();

        private int _disposed;

        /// <summary>
        /// 初期化時にAivisSpeechから取得した話者・スタイル一覧。
        /// </summary>
        public IReadOnlyList<ChatVoiceSpeaker> Speakers { get; }

        private ChatVoice(
            HttpClient httpClient,
            string baseUrl,
            IReadOnlyList<ChatVoiceSpeaker> speakers)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl.TrimEnd('/');
            Speakers = speakers;

            _queue = Channel.CreateUnbounded<SpeechRequest>(
                new UnboundedChannelOptions
                {
                    SingleReader = true,
                    SingleWriter = false
                });

            // ミキサー出力は44.1kHz、ステレオに統一する。
            _mixer = new MixingSampleProvider(
                WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
            {
                ReadFully = true
            };

            _output.Init(_mixer);
            _output.Play();

            // API要求は単一ワーカーでキュー順に処理する。
            _worker = ProcessQueueAsync();
        }

        /// <summary>
        /// AivisSpeechに接続し、話者一覧を取得してインスタンスを作成する。
        /// </summary>
        public static async Task<ChatVoice> CreateAsync(
            string baseUrl = "http://127.0.0.1:10101",
            CancellationToken cancellationToken = default)
        {
            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            try
            {
                string url = $"{baseUrl.TrimEnd('/')}/speakers";

                List<SpeakerResponse>? response =
                    await httpClient.GetFromJsonAsync<List<SpeakerResponse>>(
                        url,
                        cancellationToken);

                List<ChatVoiceSpeaker> speakers = response?
                    .SelectMany(speaker =>
                        speaker.Styles.Select(style =>
                            new ChatVoiceSpeaker(
                                speaker.Name,
                                style.Name,
                                style.Id)))
                    .ToList()
                    ?? new List<ChatVoiceSpeaker>();

                if (speakers.Count == 0)
                {
                    throw new InvalidOperationException(
                        "AivisSpeechから利用可能な話者を取得できませんでした。");
                }

                return new ChatVoice(
                    httpClient,
                    baseUrl,
                    speakers.AsReadOnly());
            }
            catch
            {
                httpClient.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 本文、話者、音声設定をキューに追加する。
        /// 合成・再生の完了を待たずに戻る。
        /// </summary>
        public void Enqueue(
            string text,
            int speakerId,
            ChatVoiceSettings settings)
        {
            if (Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException(nameof(ChatVoice));

            if (string.IsNullOrWhiteSpace(text))
                return;

            if (!Speakers.Any(speaker => speaker.StyleId == speakerId))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(speakerId),
                    "取得した話者一覧にないStyleIdです。");
            }

            ArgumentNullException.ThrowIfNull(settings);
            settings.Validate();

            var request = new SpeechRequest(
                text.Trim(),
                speakerId,
                settings);

            if (!_queue.Writer.TryWrite(request))
            {
                throw new InvalidOperationException(
                    "読み上げキューに追加できませんでした。");
            }
        }

        private async Task ProcessQueueAsync()
        {
            try
            {
                await foreach (SpeechRequest request in
                    _queue.Reader.ReadAllAsync(_shutdown.Token))
                {
                    try
                    {
                        byte[] wav = await SynthesizeAsync(
                            request.Text,
                            request.SpeakerId,
                            request.Settings,
                            _shutdown.Token);

                        // 再生完了を待たずにミキサーへ追加する。
                        AddWavToMixer(wav);
                    }
                    catch (OperationCanceledException)
                        when (_shutdown.IsCancellationRequested)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        Debug.Print($"[ChatVoice] 読み上げエラー: {ex}");
                    }
                }
            }
            catch (OperationCanceledException)
                when (_shutdown.IsCancellationRequested)
            {
                // DisposeAsyncによる終了
            }
        }

        private async Task<byte[]> SynthesizeAsync(
            string text,
            int speakerId,
            ChatVoiceSettings settings,
            CancellationToken cancellationToken)
        {
            string encodedText = Uri.EscapeDataString(text);
            string queryUrl =
                $"{_baseUrl}/audio_query?text={encodedText}&speaker={speakerId}";

            using HttpResponseMessage queryResponse =
                await _httpClient.PostAsync(
                    queryUrl,
                    content: null,
                    cancellationToken);

            queryResponse.EnsureSuccessStatusCode();

            string queryJson =
                await queryResponse.Content.ReadAsStringAsync(
                    cancellationToken);

            JsonObject query = JsonNode.Parse(queryJson)?.AsObject()
                ?? throw new InvalidOperationException(
                    "audio_queryの応答をJSONとして解析できませんでした。");

            query["speedScale"] = settings.SpeedScale;
            query["volumeScale"] = settings.VolumeScale;
            query["pitchScale"] = settings.PitchScale;

            using var synthesisContent = new StringContent(
                query.ToJsonString(),
                Encoding.UTF8,
                "application/json");

            string synthesisUrl =
                $"{_baseUrl}/synthesis?speaker={speakerId}";

            using HttpResponseMessage synthesisResponse =
                await _httpClient.PostAsync(
                    synthesisUrl,
                    synthesisContent,
                    cancellationToken);

            synthesisResponse.EnsureSuccessStatusCode();

            return await synthesisResponse.Content.ReadAsByteArrayAsync(
                cancellationToken);
        }

        private void AddWavToMixer(byte[] wavData)
        {
            var stream = new MemoryStream(wavData);
            WaveFileReader? reader = null;
            OwnedSampleProvider? ownedProvider = null;
            bool ownershipTransferred = false;

            try
            {
                reader = new WaveFileReader(stream);
                ISampleProvider provider = reader.ToSampleProvider();

                if (provider.WaveFormat.Channels == 1)
                {
                    provider = new MonoToStereoSampleProvider(provider);
                }
                else if (provider.WaveFormat.Channels != 2)
                {
                    throw new InvalidOperationException(
                        $"未対応のチャンネル数です: {provider.WaveFormat.Channels}");
                }

                if (provider.WaveFormat.SampleRate != _mixer.WaveFormat.SampleRate)
                {
                    provider = new WdlResamplingSampleProvider(
                        provider,
                        _mixer.WaveFormat.SampleRate);
                }

                ownedProvider = new OwnedSampleProvider(
                    provider,
                    reader,
                    ended => _activeInputs.TryRemove(ended, out _));

                _activeInputs.TryAdd(ownedProvider, 0);
                _mixer.AddMixerInput(ownedProvider);

                // 以降はミキサー入力がreaderを所有する。
                ownershipTransferred = true;
            }
            finally
            {
                if (!ownershipTransferred)
                {
                    if (ownedProvider is not null)
                        ownedProvider.Dispose();
                    else if (reader is not null)
                        reader.Dispose();
                    else
                        stream.Dispose();
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
                return;

            _queue.Writer.TryComplete();
            _shutdown.Cancel();

            try
            {
                await _worker;
            }
            catch (OperationCanceledException)
            {
                // 終了処理
            }
            finally
            {
                _output.Stop();
                _output.Dispose();

                foreach (OwnedSampleProvider input in _activeInputs.Keys)
                    input.Dispose();

                _httpClient.Dispose();
                _shutdown.Dispose();
            }
        }

        private sealed record SpeechRequest(
            string Text,
            int SpeakerId,
            ChatVoiceSettings Settings);

        private sealed class SpeakerResponse
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = "";

            [JsonPropertyName("styles")]
            public List<StyleResponse> Styles { get; set; } = new();
        }

        private sealed class StyleResponse
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = "";

            [JsonPropertyName("id")]
            public int Id { get; set; }
        }

        /// <summary>
        /// 再生終了時にWaveFileReaderを解放するミキサー入力。
        /// </summary>
        private sealed class OwnedSampleProvider : ISampleProvider, IDisposable
        {
            private readonly ISampleProvider _source;
            private readonly IDisposable _owner;
            private readonly Action<OwnedSampleProvider> _onDisposed;

            private int _disposed;

            public OwnedSampleProvider(
                ISampleProvider source,
                IDisposable owner,
                Action<OwnedSampleProvider> onDisposed)
            {
                _source = source;
                _owner = owner;
                _onDisposed = onDisposed;
            }

            public WaveFormat WaveFormat => _source.WaveFormat;

            public int Read(float[] buffer, int offset, int count)
            {
                int samplesRead = _source.Read(buffer, offset, count);

                if (samplesRead == 0)
                    Dispose();

                return samplesRead;
            }

            public void Dispose()
            {
                if (Interlocked.Exchange(ref _disposed, 1) != 0)
                    return;

                _owner.Dispose();
                _onDisposed(this);
            }
        }
    }

    public sealed record ChatVoiceSettings(
        double SpeedScale = 1.0,
        double VolumeScale = 1.0,
        double PitchScale = 0.0)
    {
        public void Validate()
        {
            if (!double.IsFinite(SpeedScale) ||
                SpeedScale < 0.5 || SpeedScale > 2.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(SpeedScale),
                    "話速は0.5～2.0で指定してください。");
            }

            if (!double.IsFinite(VolumeScale) ||
                VolumeScale < 0.0 || VolumeScale > 2.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(VolumeScale),
                    "音量は0.0～2.0で指定してください。");
            }

            if (!double.IsFinite(PitchScale) ||
                PitchScale < -0.15 || PitchScale > 0.15)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(PitchScale),
                    "ピッチは-0.15～0.15で指定してください。");
            }
        }
    }

    public sealed record ChatVoiceSpeaker(
        string SpeakerName,
        string StyleName,
        int StyleId)
    {
        public override string ToString() => $"{SpeakerName} - {StyleName}";
    }
}