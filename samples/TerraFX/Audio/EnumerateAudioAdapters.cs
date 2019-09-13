using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TerraFX.ApplicationModel;
using TerraFX.Audio;
using TerraFX.Provider.PulseAudio.Audio;
using TerraFX.Utilities;

namespace TerraFX.Samples.Audio
{
    public sealed class EnumerateAudioAdapters : Sample
    {
        private readonly bool _async = false;

        public EnumerateAudioAdapters(string name, bool @async, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
            _async = @async;
        }

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

                if (_async)
                {
                    await foreach (var audioAdapter in audioProvider.EnumerateAudioDevices())
                    {
                        PrintAudioAdapter(audioAdapter);
                    }
                }
                else
                {
                    foreach (var audioAdapter in audioProvider.EnumerateAudioDevices())
                    {
                        PrintAudioAdapter(audioAdapter);
                    }
                }

                void PrintAudioAdapter(IAudioAdapter audioAdapter)
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
            finally
            {
                await audioProvider.StopAsync();
            }

            application.RequestExit();
        }
    }
}
