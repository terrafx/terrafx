// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TerraFX.ApplicationModel;
using TerraFX.Audio;
using TerraFX.Utilities;

namespace TerraFX.Samples.Audio
{
    public sealed class PlaySampleAudio : Sample
    {
        // Middle C is 261.626 Hz. Guess what note this is :)
        public const double SineWaveFrequency = 277.18;

        // Multiplication factor to generate the above frequency sine wave at a specified sample rate (CD Audio, 44.1KHz in this case)
        public const double SineWaveRate = SineWaveFrequency * 2 * Math.PI / WellKnownSampleRates.CdAudio;

        // Current position in sine wave, in samples
        private int _samplePosition;
        private IAudioService? _service;

        public PlaySampleAudio(string name, ApplicationServiceProvider serviceProvider)
            : base(name, serviceProvider)
        {
            _service = null;
        }

        public override void Initialize(Application application, TimeSpan timeout)
        {
            _service = application.ServiceProvider.AudioService;

            var task = _service.StartAsync();
            if (!task.IsCompleted)
            {
                task.AsTask().Wait();
            }

            base.Initialize(application, timeout);
        }

        public override void Cleanup()
        {
            ExceptionUtilities.ThrowIfNull(_service);

            var task = _service.StopAsync();
            if (!task.IsCompleted)
            {
                task.AsTask().Wait();
            }

            base.Cleanup();
        }

        protected override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
        {
            ExceptionUtilities.ThrowIfNull(sender);

            var application = (Application)sender;
            RunAsync(application).Wait();
        }

        private async Task RunAsync(Application application)
        {
            ExceptionUtilities.ThrowIfNull(_service);

            IAudioAdapter? preferredAdapter = null;
            await foreach (var audioAdapter in _service.EnumerateAudioDevices())
            {
                if (audioAdapter is PulseSinkAdapter sinkAdapter)
                {
                    if (audioAdapter.BitDepth == 16 &&
                        audioAdapter.Channels == 2 &&
                        audioAdapter.SampleRate == WellKnownSampleRates.CdAudio &&
                        audioAdapter.IsBigEndian == false &&
                        sinkAdapter.IsUnsigned == false)
                    {
                        preferredAdapter = audioAdapter;
                        Console.WriteLine($"    Found possible audio adapter: {audioAdapter.Name}");
                    }
                }
            }

            if (preferredAdapter != null)
            {
                var device = await _service.RequestAudioPlaybackDeviceAsync(preferredAdapter);

                Console.WriteLine($"    Playing a {SineWaveFrequency}Hz sine wave...");

                await Task.WhenAll(
                    ProduceAudioAsync(device.Writer),
                    RunDeviceAsync(device)
                );
            }

            application.RequestExit();
        }

        private Task ProduceAudioAsync(PipeWriter writer)
        {
            return Task.Run(ProduceInternalAsync);

            async Task ProduceInternalAsync()
            {
                FlushResult result = default;
                while (!result.IsCompleted)
                {
                    ComputeAudioWaveBlock(writer, ref _samplePosition);

                    result = await writer.FlushAsync();
                }
            }

            static void ComputeAudioWaveBlock(PipeWriter writer, ref int samplePosition)
            {
                var block = writer.GetSpan();
                var buffer = MemoryMarshal.Cast<byte, short>(block);

                for (var x = 0; x < buffer.Length; x += 2)
                {
                    var value = (short)(Math.Sin(samplePosition * SineWaveRate) * short.MaxValue);

                    buffer[x] = value;
                    buffer[x + 1] = value;
                    // Allow overflows to prevent exceptions. This may cause artifacting, but it's unlikely we'll be played for this long enough to matter
                    unchecked { samplePosition += 1; }
                }

                writer.Advance(block.Length);
            }
        }

        private Task RunDeviceAsync(IAudioPlaybackDevice device)
        {
            return Task.Run(RunInternalAsync);

            async Task RunInternalAsync()
            {
                await device.RunAsync();
            }
        }
    }
}
