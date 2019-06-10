using System;
using System.Collections.Concurrent;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Audio;

namespace TerraFX.Samples.Audio
{
    public sealed class EnumerateAudioDevices : Sample
    {
        public EnumerateAudioDevices(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void OnIdle(object sender, ApplicationIdleEventArgs eventArgs)
        {
            var application = (Application)sender;
            {
                var audioProvider = application.GetService<IAudioProvider>();

                audioProvider.StartAsync().AsTask().GetAwaiter().GetResult();

                var devices = audioProvider.EnumerateAudioDevices();

                int i = 0;
                foreach (var audioDevice in devices)
                {
                    Console.WriteLine($"    Device: {i++} ({audioDevice.DeviceType})");
                    Console.WriteLine($"        Name: {audioDevice.Name}");
                    if (audioDevice is Provider.PulseAudio.Audio.SinkDeviceOptions sinkDevice)
                        Console.WriteLine($"        Description: {sinkDevice.Description}");
                    else if (audioDevice is Provider.PulseAudio.Audio.SourceDeviceOptions sourceDevice)
                        Console.WriteLine($"        Description: {sourceDevice.Description}");
                    Console.WriteLine($"        Sample rate: {audioDevice.SampleRate}");
                    Console.WriteLine($"        Bit depth: {audioDevice.BitDepth}");
                    Console.WriteLine($"        Channels: {audioDevice.Channels}");
                    Console.WriteLine($"        Big endian: {audioDevice.BigEndian}");
                }
            }
            Console.WriteLine("exit requested");
            application.RequestExit();
        }
    }
}
