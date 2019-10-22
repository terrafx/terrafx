// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.IO;
using System.IO.Pipelines;
using System.Reflection;
using System.Threading.Tasks;
using TerraFX.ApplicationModel;
using TerraFX.Audio;
using TerraFX.Provider.PulseAudio.Audio;
using TerraFX.Utilities;

namespace TerraFX.Samples.Audio
{
    public sealed class RecordSampleAudio : Sample
    {
        private IAudioProvider? _provider;
        private FileStream? _writeStream;

        public RecordSampleAudio(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
            _provider = null;
        }

        public override void Initialize(Application application)
        {
            _provider = application.GetService<IAudioProvider>();

            var task = _provider.StartAsync();
            if (!task.IsCompleted)
            {
                task.AsTask().Wait();
            }

            do
            {
                Console.Write("    Enter destination filename: ");
                try
                {
                    var file = Console.ReadLine();
                    _writeStream = File.OpenWrite(file);
                    Console.WriteLine($"    Opened {file} for writing.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"    Could not open file: {e.Message}");
                }
            }
            while (_writeStream == null);

            base.Initialize(application);
        }

        public override void Cleanup()
        {
            ExceptionUtilities.ThrowIfNull(_provider, nameof(_provider));

            var task = _provider.StopAsync();
            if (!task.IsCompleted)
            {
                task.AsTask().Wait();
            }

            _writeStream?.Dispose();

            base.Cleanup();
        }

        protected override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
        {
            ExceptionUtilities.ThrowIfNull(sender, nameof(sender));

            var application = (Application)sender;
            RunAsync(application).Wait();
        }

        private async Task RunAsync(Application application)
        {
            ExceptionUtilities.ThrowIfNull(_provider, nameof(_provider));

            IAudioAdapter? preferredAdapter = null;
            await foreach (var audioAdapter in _provider.EnumerateAudioDevices())
            {
                if (audioAdapter is PulseSourceAdapter sinkAdapter)
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
                var device = await _provider.RequestAudioRecordingDeviceAsync(preferredAdapter);

                Console.WriteLine($"    Recording raw audio...");

                await Task.WhenAll(
                    RecordAudioAsync(device.Reader),
                    RunDeviceAsync(device)
                );
            }
            else
            {
                Console.WriteLine("    Could not find any appropriate audio devices.");
            }

            application.RequestExit();
        }

        private async Task RecordAudioAsync(PipeReader reader)
        {
            try
            {
                await reader.CopyToAsync(_writeStream);
            }
            finally
            {
                await _writeStream!.FlushAsync();
            }
        }

        private Task RunDeviceAsync(IAudioRecordingDevice device)
        {
            return Task.Run(RunInternalAsync);

            async Task RunInternalAsync()
            {
                await device.RunAsync();
            }
        }
    }
}
