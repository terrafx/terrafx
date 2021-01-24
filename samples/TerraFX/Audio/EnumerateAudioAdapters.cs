// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using TerraFX.ApplicationModel;
using TerraFX.Audio;
using TerraFX.Utilities;

namespace TerraFX.Samples.Audio
{
    public sealed class EnumerateAudioAdapters : Sample
    {
        private readonly bool _async = false;
        private IAudioProvider? _provider;

        public EnumerateAudioAdapters(string name, bool @async, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
            _async = @async;
            _provider = null;
        }

        public override void Initialize(Application application, TimeSpan timeout)
        {
            _provider = application.GetService<IAudioProvider>();

            var task = _provider.StartAsync();
            if (!task.IsCompleted)
            {
                task.AsTask().Wait();
            }

            base.Initialize(application, timeout);
        }

        public override void Cleanup()
        {
            ExceptionUtilities.ThrowIfNull(_provider, nameof(_provider));

            var task = _provider.StopAsync();
            if (!task.IsCompleted)
            {
                task.AsTask().Wait();
            }

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

            if (_async)
            {
                await foreach (var audioAdapter in _provider.EnumerateAudioDevices())
                {
                    PrintAudioAdapter(audioAdapter);
                }
            }
            else
            {
                foreach (var audioAdapter in _provider.EnumerateAudioDevices())
                {
                    PrintAudioAdapter(audioAdapter);
                }
            }

            application.RequestExit();

            static void PrintAudioAdapter(IAudioAdapter audioAdapter)
            {
                Console.WriteLine($"    Name: {audioAdapter.Name}");
                Console.WriteLine($"        Type: {audioAdapter.DeviceType}");
                Console.WriteLine($"        Channel count: {audioAdapter.Channels}");
                Console.WriteLine($"        Sample rate: {audioAdapter.SampleRate}");
                Console.WriteLine($"        Bit depth: {audioAdapter.BitDepth}");
                Console.WriteLine($"        Endianness: {(audioAdapter.IsBigEndian ? "Big" : "Little")}");

                if (audioAdapter is PulseSourceAdapter source)
                {
                    Console.WriteLine($"        Number type: {(source.IsFloatingPoint ? "float" : $"{(source.IsUnsigned ? "unsigned " : "signed ")}integer")}");
                    Console.WriteLine($"        Number of bits per sample: {source.PackedSize}");
                    Console.WriteLine($"        Description: {source.Description}");
                }

                if (audioAdapter is PulseSinkAdapter sink)
                {
                    Console.WriteLine($"        Number type: {(sink.IsFloatingPoint ? "float" : $"{(sink.IsUnsigned ? "unsigned " : "signed ")}integer")}");
                    Console.WriteLine($"        Number of bits per sample: {sink.PackedSize}");
                    Console.WriteLine($"        Description: {sink.Description}");
                }
            }
        }
    }
}
