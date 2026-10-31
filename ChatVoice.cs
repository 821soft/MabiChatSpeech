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
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using static MabiChatSpeech.WinApi;

namespace MabiChatSpeech
{


    public sealed class ChatVoice : IAsyncDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly SynchronizationContext? _eventSynchronizationContext;

        private readonly Channel<SpeechRequest> _queue;
        private readonly CancellationTokenSource _shutdown = new();
        private readonly Task _worker;

        private readonly SpeechMixingSampleProvider _mixer;
        private readonly WaveOutEvent _output = new();
        private readonly ConcurrentDictionary<OwnedSampleProvider, byte>
            _activeInputs = new();

        private int _disposed;
        private int _pendingCount;
        private readonly object _queueStateLock = new();
        private readonly SemaphoreSlim _enqueueAndWaitGate = new(1, 1);

        /// <summary>キューへの追加が受理されたときに通知される。</summary>
        public event Action<SpeechQueueEventArgs>? SpeechQueued;

        /// <summary>音声データの再生開始時に通知される。</summary>
        public event Action<SpeechPlaybackEventArgs>? PlaybackStarted;

        /// <summary>音声データの再生終了時に通知される。</summary>
        public event Action<SpeechPlaybackEventArgs>? PlaybackEnded;

        /// <summary>合成開始から再生終了までの状態が変化したときに通知される。</summary>
        public event Action<SpeechStatusChangedEventArgs>? SpeechStatusChanged;

        /// <summary>キュー内の全要求の合成処理が終わり、キューが空になったときに通知される。</summary>
        public event Action? QueueBecameEmpty;

        /// <summary>処理中を含む未完了要求数。</summary>
        public int PendingCount => Volatile.Read(ref _pendingCount);

        /// <summary>
        /// 初期化時にAivisSpeechから取得した話者・スタイル一覧。
        /// </summary>
        public IReadOnlyList<ChatVoiceSpeaker> Speakers { get; private set; }

        private ChatVoice(
            HttpClient httpClient,
            string baseUrl,
            IReadOnlyList<ChatVoiceSpeaker> speakers)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl.TrimEnd('/');
            Speakers = speakers;
            _eventSynchronizationContext = SynchronizationContext.Current;
            _queue = Channel.CreateUnbounded<SpeechRequest>(
                new UnboundedChannelOptions
                {
                    SingleReader = true,
                    SingleWriter = false
                });

            // ミキサー出力は44.1kHz、ステレオに統一する。
            _mixer = new SpeechMixingSampleProvider(
                WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
            {
                ReadFully = true
            };

            _output.Init(_mixer);
            _output.PlaybackStopped += Output_PlaybackStopped;
            _output.Play();

            // API要求は単一ワーカーでキュー順に処理する。
            _worker = ProcessQueueAsync();
        }

        public static async Task<bool> IsChatVoiceEngenRunning(ChatVoiceEngine eng)
        {
            var ver = await ChatVoice.GetEngineVersionAsync(eng);
            if (ver == string.Empty)
            {
                return (false);
            }
            else
            {
                return (true);
            }

        }
        /// <summary>
        /// 音声合成エンジンに接続し、話者一覧を取得してインスタンスを作成する。
        /// </summary>
        public static async Task<ChatVoice> CreateAsync(
            string baseUrl = "http://127.0.0.1:10101",
            CancellationToken cancellationToken = default,
            ChatVoiceEngine engine = ChatVoiceEngine.AivisSpeech)
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
                                style.Id,
                                baseUrl.TrimEnd('/'),
                                engine)))
                    .ToList()
                    ?? new List<ChatVoiceSpeaker>();

                if (speakers.Count == 0)
                {
                    throw new InvalidOperationException(
                        "音声合成エンジンから利用可能な話者を取得できませんでした。");
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
        /// AivisSpeechとVOICEVOXの話者をまとめたインスタンスを作成する。
        /// </summary>
        public static async Task<ChatVoice> CreateMultiEngineAsync(
            string aivisBaseUrl = "http://127.0.0.1:10101",
            string voiceVoxBaseUrl = "http://127.0.0.1:50021",
            CancellationToken cancellationToken = default)
        {
            var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            try
            {
                var speakers = new List<ChatVoiceSpeaker>();
                await AddSpeakersAsync(httpClient, aivisBaseUrl, ChatVoiceEngine.AivisSpeech, speakers, cancellationToken);
                await AddSpeakersAsync(httpClient, voiceVoxBaseUrl, ChatVoiceEngine.VoiceVox, speakers, cancellationToken);
                if (speakers.Count == 0)
                    throw new InvalidOperationException("利用可能な話者を取得できませんでした。");
                return new ChatVoice(httpClient, aivisBaseUrl, speakers.AsReadOnly());
            }
            catch
            {
                httpClient.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 指定した合成エンジンのHTTP APIからバージョンを取得する。
        /// </summary>
        public static Task<string> GetEngineVersionAsync(
            ChatVoiceEngine engine,
            CancellationToken cancellationToken = default)
        {
            string baseUrl = engine switch
            {
                ChatVoiceEngine.AivisSpeech => "http://127.0.0.1:10101",
                ChatVoiceEngine.VoiceVox => "http://127.0.0.1:50021",
                _ => throw new ArgumentOutOfRangeException(nameof(engine))
            };

            return GetEngineVersionAsync(baseUrl, cancellationToken);
        }

        /// <summary>
        /// 指定したAPI URLの合成エンジンからバージョンを取得する。
        /// </summary>
        public static async Task<string> GetEngineVersionAsync(
            string baseUrl,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);

            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(0.3)
            };

            string url = $"{baseUrl.TrimEnd('/')}/version";
            HttpResponseMessage response;
            try
            {

                response =
                    await httpClient.GetAsync(url, cancellationToken);

            }
            catch (Exception ex)
            {
                return String.Empty;
            }
            response.EnsureSuccessStatusCode();

            string responseText = await response.Content.ReadAsStringAsync(
                cancellationToken);

            string version = responseText.Trim();

            // FastAPI系のAPIは文字列をJSON文字列として返す場合がある。
            try
            {
                using JsonDocument document = JsonDocument.Parse(version);
                if (document.RootElement.ValueKind == JsonValueKind.String)
                    version = document.RootElement.GetString() ?? string.Empty;
            }
            catch (JsonException)
            {
                // JSONでないプレーンテキストの応答はそのまま使う。
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                return string.Empty;
//                throw new InvalidOperationException(
//                    "音声合成エンジンからバージョンを取得できませんでした。");
            }

            return version.Trim();
        }

        /// <summary>
        /// 現在の話者一覧に含まれるエンジンから話者一覧を再取得して更新する。
        /// </summary>
        public async Task RefreshSpeakersAsync(
            CancellationToken cancellationToken = default)
        {
            var endpoints = Speakers
                .Select(speaker => new { speaker.BaseUrl, speaker.Engine })
                .Distinct()
                .ToArray();

            var refreshedSpeakers = new List<ChatVoiceSpeaker>();

            foreach (var endpoint in endpoints)
            {
                await AddSpeakersAsync(
                    _httpClient,
                    endpoint.BaseUrl,
                    endpoint.Engine,
                    refreshedSpeakers,
                    cancellationToken);
            }

            if (refreshedSpeakers.Count == 0)
            {
                throw new InvalidOperationException(
                    "音声合成エンジンから話者一覧を取得できませんでした。");
            }

            Speakers = refreshedSpeakers.AsReadOnly();
        }

        private static async Task AddSpeakersAsync(
            HttpClient httpClient, string baseUrl, ChatVoiceEngine engine,
            List<ChatVoiceSpeaker> destination, CancellationToken cancellationToken)
        {
            string normalizedBaseUrl = baseUrl.TrimEnd('/');
            List<SpeakerResponse>? response;
            try
            {
                response = await httpClient.GetFromJsonAsync<List<SpeakerResponse>>(
                    $"{normalizedBaseUrl}/speakers", cancellationToken);
            }
            catch (HttpRequestException)
            {
                return;
            }
            if (response is null) return;
            destination.AddRange(response.SelectMany(speaker =>
                speaker.Styles.Select(style => new ChatVoiceSpeaker(
                    speaker.Name, style.Name, style.Id, normalizedBaseUrl, engine))));
        }

        /// <summary>
        /// 話者名または一覧の表示文字列を指定して読み上げをキューに追加する。
        /// </summary>
        public void Enqueue(string text, string speakerName, ChatVoiceSettings settings)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(speakerName);
            ChatVoiceSpeaker? exact = Speakers.FirstOrDefault(speaker =>
                speaker.ToString().Equals(speakerName, StringComparison.OrdinalIgnoreCase));
            if (exact is not null)
            {
                Enqueue(text, exact, settings);
                return;
            }

            ChatVoiceSpeaker[] matches = Speakers.Where(speaker =>
                speaker.SpeakerName.Equals(speakerName, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (matches.Length == 0)
                throw new ArgumentException("話者一覧に指定された名前がありません。", nameof(speakerName));
            if (matches.Select(speaker => speaker.BaseUrl).Distinct(StringComparer.OrdinalIgnoreCase).Count() > 1)
                throw new ArgumentException("同名の話者が複数エンジンにあります。一覧の表示文字列を指定してください。", nameof(speakerName));
            Enqueue(text, matches[0], settings);
        }

        public void Enqueue(string text, ChatVoiceSpeaker speaker, ChatVoiceSettings settings)
        {
            ArgumentNullException.ThrowIfNull(speaker);
            if (!Speakers.Contains(speaker))
                throw new ArgumentException("このインスタンスの話者一覧にありません。", nameof(speaker));
            EnqueueCore(text, speaker.StyleId, speaker.BaseUrl, settings);
        }
        /// <summary>
        /// VOICEVOX Engineに接続し、話者一覧を取得してインスタンスを作成する。
        /// </summary>
        public static Task<ChatVoice> CreateVoiceVoxAsync(
            string baseUrl = "http://127.0.0.1:50021",
            CancellationToken cancellationToken = default)
        {
            return CreateAsync(baseUrl, cancellationToken, ChatVoiceEngine.VoiceVox);
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

            ChatVoiceSpeaker? speaker = Speakers.FirstOrDefault(item =>
                item.StyleId == speakerId &&
                item.BaseUrl.Equals(_baseUrl, StringComparison.OrdinalIgnoreCase));
            if (speaker is null)
                throw new ArgumentOutOfRangeException(nameof(speakerId),
                    "この接続先の話者一覧にないStyleIdです。");
            EnqueueCore(text, speakerId, speaker.BaseUrl, settings);
        }

        /// <summary>読み上げをキューに追加し、その音声の再生終了まで待機する。</summary>
        public async Task EnqueueAndWaitAsync(
            string text, int speakerId, ChatVoiceSettings settings,
            CancellationToken cancellationToken = default)
        {
            if (Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException(nameof(ChatVoice));
            if (string.IsNullOrWhiteSpace(text)) return;
            ChatVoiceSpeaker? speaker = Speakers.FirstOrDefault(item =>
                item.StyleId == speakerId &&
                item.BaseUrl.Equals(_baseUrl, StringComparison.OrdinalIgnoreCase));
            if (speaker is null)
                throw new ArgumentOutOfRangeException(nameof(speakerId),
                    "この接続先の話者一覧にないStyleIdです。");

            await EnqueueAndWaitCoreAsync(
                text, speaker.StyleId, speaker.BaseUrl, settings, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>指定したエンジンと話者IDで読み上げ、再生終了まで待機する。</summary>
        public async Task EnqueueAndWaitAsync(
            string text, ChatVoiceEngine engine, int speakerId,
            ChatVoiceSettings settings, CancellationToken cancellationToken = default)
        {
            if (Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException(nameof(ChatVoice));
            if (string.IsNullOrWhiteSpace(text)) return;
            ChatVoiceSpeaker? speaker = Speakers.FirstOrDefault(item =>
                item.Engine == engine && item.StyleId == speakerId);
            if (speaker is null)
                throw new ArgumentOutOfRangeException(nameof(speakerId),
                    "指定したエンジンの話者一覧にStyleIdがありません。");

            await EnqueueAndWaitCoreAsync(
                text, speaker.StyleId, speaker.BaseUrl, settings, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task EnqueueAndWaitCoreAsync(
            string text, int speakerId, string baseUrl, ChatVoiceSettings settings,
            CancellationToken cancellationToken)
        {
            await _enqueueAndWaitGate.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                // 合成待ち・合成中の要求と再生中の音声がなくなるまで待ってから投入する。
                while (PendingCount != 0)
                    await Task.Delay(25, cancellationToken).ConfigureAwait(false);

                await EnqueueCore(text, speakerId, baseUrl, settings)
                    .WaitAsync(cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _enqueueAndWaitGate.Release();
            }
        }

        /// <summary>指定した話者で読み上げ、再生終了まで待機する。</summary>
        public async Task EnqueueAndWaitAsync(
            string text, ChatVoiceSpeaker speaker, ChatVoiceSettings settings,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(speaker);
            if (!Speakers.Contains(speaker))
                throw new ArgumentException("このインスタンスの話者一覧にありません。", nameof(speaker));
            if (string.IsNullOrWhiteSpace(text)) return;

            await EnqueueAndWaitCoreAsync(
                text, speaker.StyleId, speaker.BaseUrl, settings, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// エンジンと話者IDを指定して読み上げをキューに追加する。
        /// </summary>
        public void Enqueue(
            string text, ChatVoiceEngine engine, int speakerId, ChatVoiceSettings settings)
        {
            ChatVoiceSpeaker? speaker = Speakers.FirstOrDefault(item =>
                item.Engine == engine && item.StyleId == speakerId);
            if (speaker is null)
                throw new ArgumentOutOfRangeException(nameof(speakerId),
                    "指定したエンジンの話者一覧にStyleIdがありません。");
            EnqueueCore(text, speaker.StyleId, speaker.BaseUrl, settings);
        }
        private Task EnqueueCore(string text, int speakerId, string baseUrl, ChatVoiceSettings settings)
        {
            if (Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException(nameof(ChatVoice));
            if (string.IsNullOrWhiteSpace(text)) return Task.CompletedTask;
            ArgumentNullException.ThrowIfNull(settings);
            settings.Validate();

            string normalizedBaseUrl = baseUrl.TrimEnd('/');
            ChatVoiceEngine engine = Speakers.FirstOrDefault(item =>
                item.StyleId == speakerId && item.BaseUrl.Equals(normalizedBaseUrl,
                    StringComparison.OrdinalIgnoreCase))?.Engine ?? ChatVoiceEngine.AivisSpeech;
            var request = new SpeechRequest(
                RequestId: Guid.NewGuid(),
                Text: text.Trim(),
                SpeakerId: speakerId,
                BaseUrl: normalizedBaseUrl,
                Engine: engine,
                Settings: settings,
                PlaybackCompletion: new TaskCompletionSource(
                    TaskCreationOptions.RunContinuationsAsynchronously));

            lock (_queueStateLock)
            {
                if (Volatile.Read(ref _disposed) != 0)
                    throw new ObjectDisposedException(nameof(ChatVoice));
                int pendingCount = Interlocked.Increment(ref _pendingCount);
                if (!_queue.Writer.TryWrite(request))
                {
                    Interlocked.Decrement(ref _pendingCount);
                    throw new InvalidOperationException("読み上げキューに追加できませんでした。");
                }
                RaiseSpeechQueued(new SpeechQueueEventArgs(
                    request.RequestId, request.Text, request.Engine, request.SpeakerId, pendingCount));
                RaiseSpeechStatusChanged(new SpeechStatusChangedEventArgs(
                    request.RequestId, request.Text, request.Engine, request.SpeakerId, SpeechStatus.Queued));
            }
            return request.PlaybackCompletion.Task;
        }

        private void CompleteRequest()
        {
            lock (_queueStateLock)
            {
                int pendingCount = Interlocked.Decrement(ref _pendingCount);
                if (pendingCount == 0)
                    RaiseQueueBecameEmpty();
            }
        }

        private void RaisePlaybackStarted(SpeechPlaybackEventArgs args)
        {
            RaiseSpeechStatusChanged(new SpeechStatusChangedEventArgs(
                args.RequestId, args.Text, args.Engine, args.SpeakerId, SpeechStatus.Playing));
            Action<SpeechPlaybackEventArgs>? handlers = PlaybackStarted;
            if (handlers is null) return;

            DispatchEvent(() =>
            {
                foreach (Action<SpeechPlaybackEventArgs> handler in handlers.GetInvocationList())
                {
                    try { handler(args); }
                    catch (Exception ex) { Debug.Print($"[ChatVoice] 再生開始イベントエラー: {ex}"); }
                }
            });
        }

        private void RaisePlaybackEnded(SpeechPlaybackEventArgs args)
        {
            RaiseSpeechStatusChanged(new SpeechStatusChangedEventArgs(
                args.RequestId, args.Text, args.Engine, args.SpeakerId, SpeechStatus.Completed));
            Action<SpeechPlaybackEventArgs>? handlers = PlaybackEnded;
            if (handlers is null) return;

            DispatchEvent(() =>
            {
                foreach (Action<SpeechPlaybackEventArgs> handler in handlers.GetInvocationList())
                {
                    try { handler(args); }
                    catch (Exception ex) { Debug.Print($"[ChatVoice] 再生終了イベントエラー: {ex}"); }
                }
            });
        }

        private void RaiseSpeechStatusChanged(SpeechStatusChangedEventArgs args)
        {
            Action<SpeechStatusChangedEventArgs>? handlers = SpeechStatusChanged;
            if (handlers is null) return;

            DispatchEvent(() =>
            {
                foreach (Action<SpeechStatusChangedEventArgs> handler in handlers.GetInvocationList())
                {
                    try { handler(args); }
                    catch (Exception ex) { Debug.Print($"[ChatVoice] 音声状態イベントエラー: {ex}"); }
                }
            });
        }
        private void RaiseSpeechQueued(SpeechQueueEventArgs args)
        {
            Action<SpeechQueueEventArgs>? handlers = SpeechQueued;
            if (handlers is null) return;

            DispatchEvent(() =>
            {
                foreach (Action<SpeechQueueEventArgs> handler in handlers.GetInvocationList())
                {
                    try { handler(args); }
                    catch (Exception ex) { Debug.Print($"[ChatVoice] キュー追加イベントエラー: {ex}"); }
                }
            });
        }

        private void RaiseQueueBecameEmpty()
        {
            Action? handlers = QueueBecameEmpty;
            if (handlers is null) return;

            DispatchEvent(() =>
            {
                foreach (Action handler in handlers.GetInvocationList())
                {
                    try { handler(); }
                    catch (Exception ex) { Debug.Print($"[ChatVoice] キュー空イベントエラー: {ex}"); }
                }
            });
        }

        private void DispatchEvent(Action callback)
        {
            SynchronizationContext? context = _eventSynchronizationContext;
            if (context is null || ReferenceEquals(SynchronizationContext.Current, context))
            {
                callback();
                return;
            }

            try
            {
                context.Post(static state => ((Action)state!).Invoke(), callback);
            }
            catch (Exception ex)
            {
                Debug.Print($"[ChatVoice] イベント配送エラー: {ex}");
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
                        RaiseSpeechStatusChanged(new SpeechStatusChangedEventArgs(
                            request.RequestId, request.Text, request.Engine, request.SpeakerId, SpeechStatus.Posting));

                        byte[] wav = await SynthesizeAsync(
                            request.Text,
                            request.SpeakerId,
                            request.BaseUrl,
                            request.Settings,
                            _shutdown.Token);

                        RaiseSpeechStatusChanged(new SpeechStatusChangedEventArgs(
                            request.RequestId, request.Text, request.Engine, request.SpeakerId, SpeechStatus.Synthesized));

                        // 再生完了を待たずにミキサーへ追加する。
                        AddWavToMixer(wav, request);
                    }
                    catch (OperationCanceledException)
                        when (_shutdown.IsCancellationRequested)
                    {
                        request.PlaybackCompletion.TrySetCanceled(_shutdown.Token);
                        RaiseSpeechStatusChanged(new SpeechStatusChangedEventArgs(
                            request.RequestId, request.Text, request.Engine, request.SpeakerId, SpeechStatus.Canceled));
                        CompleteRequest();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Debug.Print($"[ChatVoice] 読み上げエラー: {ex}");
                        request.PlaybackCompletion.TrySetException(ex);
                        RaiseSpeechStatusChanged(new SpeechStatusChangedEventArgs(
                            request.RequestId, request.Text, request.Engine, request.SpeakerId,
                            SpeechStatus.Failed, ex.Message));
                        CompleteRequest();
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
            string baseUrl,
            ChatVoiceSettings settings,
            CancellationToken cancellationToken)
        {
            string encodedText = Uri.EscapeDataString(text);
            string queryUrl =
                $"{baseUrl}/audio_query?text={encodedText}&speaker={speakerId}";

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
                $"{baseUrl}/synthesis?speaker={speakerId}";

            using HttpResponseMessage synthesisResponse =
                await _httpClient.PostAsync(
                    synthesisUrl,
                    synthesisContent,
                    cancellationToken);

            synthesisResponse.EnsureSuccessStatusCode();

            return await synthesisResponse.Content.ReadAsByteArrayAsync(
                cancellationToken);
        }

        private void AddWavToMixer(byte[] wavData, SpeechRequest request)
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

                var playbackEventArgs = new SpeechPlaybackEventArgs(
                    request.RequestId, request.Text, request.Engine, request.SpeakerId);
                TimeSpan playbackCompletionTimeout = reader.TotalTime + TimeSpan.FromSeconds(5);
                long expectedSampleCount = (long)Math.Ceiling(
                    reader.TotalTime.TotalSeconds *
                    provider.WaveFormat.SampleRate *
                    provider.WaveFormat.Channels);

                ownedProvider = new OwnedSampleProvider(
                    provider,
                    reader,
                    expectedSampleCount,
                    ended => _activeInputs.TryRemove(ended, out _),
                    playbackEventArgs,
                    RaisePlaybackStarted,
                    playbackArgs =>
                    {
                        request.PlaybackCompletion.TrySetResult();
                        RaisePlaybackEnded(playbackArgs);
                        CompleteRequest();
                    },
                    (playbackArgs, exception) =>
                    {
                        request.PlaybackCompletion.TrySetException(exception);
                        RaiseSpeechStatusChanged(new SpeechStatusChangedEventArgs(
                            playbackArgs.RequestId, playbackArgs.Text, playbackArgs.Engine,
                            playbackArgs.SpeakerId, SpeechStatus.Failed, exception.Message));
                        CompleteRequest();
                    });

                _activeInputs.TryAdd(ownedProvider, 0);
                _mixer.AddMixerInput(ownedProvider);
                _ = MonitorPlaybackCompletionAsync(
                    ownedProvider,
                    playbackEventArgs,
                    playbackCompletionTimeout,
                    _shutdown.Token);

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

        private async Task MonitorPlaybackCompletionAsync(
            OwnedSampleProvider input,
            SpeechPlaybackEventArgs playbackEventArgs,
            TimeSpan timeout,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(timeout, cancellationToken).ConfigureAwait(false);
                if (!input.IsPlaybackCompleted)
                {
                    Debug.Print(
                        $"[ChatVoice] 再生完了通知の保険動作: {playbackEventArgs.RequestId}, " +
                        $"samples={input.SamplesRead}/{input.ExpectedSampleCount}, timeout={timeout}");
                    input.CompletePlayback();
                }
            }
            catch (OperationCanceledException)
            {
                // ChatVoice終了時は監視を終了する。
            }
        }

        private void Output_PlaybackStopped(object? sender, StoppedEventArgs args)
        {
            if (Volatile.Read(ref _disposed) != 0)
                return;

            Exception exception = args.Exception ??
                new InvalidOperationException("音声出力が再生中に停止しました。");

            Debug.Print($"[ChatVoice] 音声出力エラー: {exception}");
            foreach (OwnedSampleProvider input in _activeInputs.Keys)
                input.FailPlayback(exception);
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
            Guid RequestId,
            string Text,
            int SpeakerId,
            string BaseUrl,
            ChatVoiceEngine Engine,
            ChatVoiceSettings Settings,
            TaskCompletionSource PlaybackCompletion);

        /// <summary>音声再生イベントの情報。</summary>
        public sealed class SpeechPlaybackEventArgs : EventArgs
        {
            public SpeechPlaybackEventArgs(
                Guid requestId, string text, ChatVoiceEngine engine, int speakerId)
            {
                RequestId = requestId;
                Text = text;
                Engine = engine;
                SpeakerId = speakerId;
            }

            public Guid RequestId { get; }
            public string Text { get; }
            public ChatVoiceEngine Engine { get; }
            public int SpeakerId { get; }
        }

        public enum SpeechStatus
        {
            Queued,
            Posting,
            Synthesized,
            Playing,
            Completed,
            Failed,
            Canceled
        }

        public sealed class SpeechStatusChangedEventArgs : EventArgs
        {
            public SpeechStatusChangedEventArgs(
                Guid requestId, string text, ChatVoiceEngine engine, int speakerId,
                SpeechStatus status, string? errorMessage = null)
            {
                RequestId = requestId;
                Text = text;
                Engine = engine;
                SpeakerId = speakerId;
                Status = status;
                ErrorMessage = errorMessage;
            }

            public Guid RequestId { get; }
            public string Text { get; }
            public ChatVoiceEngine Engine { get; }
            public int SpeakerId { get; }
            public SpeechStatus Status { get; }
            public string? ErrorMessage { get; }
        }
        public sealed class SpeechQueueEventArgs : EventArgs
        {
            public SpeechQueueEventArgs(Guid requestId, string text, ChatVoiceEngine engine, int speakerId, int pendingCount)
            {
                RequestId = requestId;
                Text = text;
                Engine = engine;
                SpeakerId = speakerId;
                PendingCount = pendingCount;
            }

            public Guid RequestId { get; }
            public string Text { get; }
            public ChatVoiceEngine Engine { get; }
            public int SpeakerId { get; }
            public int PendingCount { get; }
        }
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
        /// MixingSampleProviderを包み、音声出力に渡す。
        /// </summary>
        private sealed class SpeechMixingSampleProvider : ISampleProvider
        {
            private readonly MixingSampleProvider _inner;

            public SpeechMixingSampleProvider(WaveFormat waveFormat)
            {
                _inner = new MixingSampleProvider(waveFormat);
            }

            public WaveFormat WaveFormat => _inner.WaveFormat;

            public bool ReadFully
            {
                get => _inner.ReadFully;
                set => _inner.ReadFully = value;
            }

            public void AddMixerInput(ISampleProvider input)
            {
                ArgumentNullException.ThrowIfNull(input);

                _inner.AddMixerInput(input);
            }

            public int Read(float[] buffer, int offset, int count) =>
                _inner.Read(buffer, offset, count);
        }

        /// <summary>
        /// 再生終了時にWaveFileReaderを解放するミキサー入力。
        /// </summary>
        private sealed class OwnedSampleProvider : ISampleProvider, IDisposable
        {
            private readonly ISampleProvider _source;
            private readonly IDisposable _owner;
            private readonly Action<OwnedSampleProvider> _onDisposed;
            private readonly SpeechPlaybackEventArgs _eventArgs;
            private readonly Action<SpeechPlaybackEventArgs> _onPlaybackStarted;
            private readonly Action<SpeechPlaybackEventArgs> _onPlaybackEnded;
            private readonly Action<SpeechPlaybackEventArgs, Exception> _onPlaybackFailed;
            private readonly long _expectedSampleCount;
            private long _samplesRead;

            private int _inputDisposed;
            private int _playbackStarted;
            private int _playbackEnded;

            public OwnedSampleProvider(
                ISampleProvider source,
                IDisposable owner,
                long expectedSampleCount,
                Action<OwnedSampleProvider> onDisposed,
                SpeechPlaybackEventArgs eventArgs,
                Action<SpeechPlaybackEventArgs> onPlaybackStarted,
                Action<SpeechPlaybackEventArgs> onPlaybackEnded,
                Action<SpeechPlaybackEventArgs, Exception> onPlaybackFailed)
            {
                _source = source;
                _owner = owner;
                _expectedSampleCount = expectedSampleCount;
                _onDisposed = onDisposed;
                _eventArgs = eventArgs;
                _onPlaybackStarted = onPlaybackStarted;
                _onPlaybackEnded = onPlaybackEnded;
                _onPlaybackFailed = onPlaybackFailed;
            }

            public WaveFormat WaveFormat => _source.WaveFormat;
            public bool IsPlaybackCompleted => Volatile.Read(ref _playbackEnded) != 0;
            public long SamplesRead => Interlocked.Read(ref _samplesRead);
            public long ExpectedSampleCount => _expectedSampleCount;

            public int Read(float[] buffer, int offset, int count)
            {
                int samplesRead;
                try
                {
                    samplesRead = _source.Read(buffer, offset, count);
                }
                catch (Exception exception)
                {
                    FailPlayback(exception);
                    throw;
                }

                if (samplesRead > 0)
                {
                    if (Interlocked.Exchange(ref _playbackStarted, 1) == 0)
                        _onPlaybackStarted(_eventArgs);

                    long totalSamplesRead = Interlocked.Add(ref _samplesRead, samplesRead);
                    // リサンプラーの丸めで最終フレームが1サンプル未満ずれる場合がある。
                    long completionThreshold = Math.Max(
                        1, _expectedSampleCount - WaveFormat.Channels);
                    if (_expectedSampleCount > 0 && totalSamplesRead >= completionThreshold)
                        CompletePlayback();
                }
                else if (samplesRead == 0)
                {
                    CompletePlayback();
                    Dispose();
                }

                return samplesRead;
            }

            public void CompletePlayback()
            {
                if (Interlocked.Exchange(ref _playbackEnded, 1) == 0)
                {
                    _onPlaybackEnded(_eventArgs);
                }
            }

            public void FailPlayback(Exception exception)
            {
                if (Interlocked.Exchange(ref _playbackEnded, 1) == 0)
                {
                    _onPlaybackFailed(_eventArgs, exception);
                    Dispose();
                }
            }

            public void Dispose()
            {
                if (Interlocked.Exchange(ref _inputDisposed, 1) != 0)
                    return;

                try
                {
                    CompletePlayback();
                    _owner.Dispose();
                }
                finally
                {
                    _onDisposed(this);
                }
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

    public enum ChatVoiceEngine
    {
        None,
        AivisSpeech,
        VoiceVox
    }
    public sealed record ChatVoiceSpeaker(
        string SpeakerName,
        string StyleName,
        int StyleId,
        string BaseUrl = "",
        ChatVoiceEngine Engine = ChatVoiceEngine.AivisSpeech)
    {
        public override string ToString() =>
            $"{SpeakerName} - {StyleName} ({BaseUrl})";
    }
}



