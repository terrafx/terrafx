// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.IO.Pipelines;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TerraFX.ApplicationModel;
using TerraFX.Audio;
using TerraFX.Provider.PulseAudio.Audio;
using TerraFX.Utilities;

namespace TerraFX.Samples.Audio
{
    public sealed class PlaySampleAudio : Sample
    {
        public PlaySampleAudio(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {}

        public override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
        {
            ExceptionUtilities.ThrowIfNull(sender, nameof(sender));

            var application = (Application)sender;
            RunAsync(application).Wait();
        }

        private async Task RunAsync(Application application)
        {
            var audioProvider = application.GetService<IAudioProvider>();

            try
            {
                await audioProvider.StartAsync();


                IAudioAdapter? preferredAdapter = null;
                await foreach (var audioAdapter in audioProvider.EnumerateAudioDevices())
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
                    var device = await audioProvider.RequestAudioPlaybackDeviceAsync(preferredAdapter);

                    Console.WriteLine("    Playing a tune...");

                    await Task.WhenAll(
                        ProduceAudioAsync(device.Writer),
                        RunDeviceAsync(device)
                    );
                }

            }
            finally
            {
                await audioProvider.StopAsync();
            }

            application.RequestExit();
        }

        private int samplePosition = 0;
        private Task ProduceAudioAsync(PipeWriter writer)
        {
            return Task.Run(ProduceInternalAsync);

            async Task ProduceInternalAsync()
            {
                FlushResult result = default;
                while (!result.IsCompleted)
                {
                    ComputeAudioWaveBlock(writer, ref samplePosition);

                    result = await writer.FlushAsync();
                }
            }

            static void ComputeAudioWaveBlock(PipeWriter writer, ref int samplePosition)
            {
                var block = writer.GetSpan();
                var buffer = MemoryMarshal.Cast<byte, short>(block);

                for (int x = 0; x < buffer.Length; x += 2)
                {
                    var value = unchecked((short)(
                        ((samplePosition << 5) & 0x07ff) | ((samplePosition >> 4) & 0x0720) | (samplePosition >> 6) | (samplePosition & 0x030f)
                    ));

                    buffer[x] = value;
                    buffer[x + 1] = value;
                    samplePosition += 1;
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
