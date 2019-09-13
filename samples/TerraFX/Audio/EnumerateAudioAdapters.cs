using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TerraFX.ApplicationModel;
using TerraFX.Audio;

namespace TerraFX.Samples.Audio
{
    public sealed class EnumerateAudioAdapters : Sample
    {
        bool _started = false;
        public EnumerateAudioAdapters(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override async void OnIdle(object sender, ApplicationIdleEventArgs eventArgs)
        {
            var application = (Application)sender;

            if (_started)
            {
                return;
            }
            else
            {
                _started = true;
            }

            try
            {
                await RunAsync(application);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Unhandled exception caught: {e.GetType().Name}");
                Console.WriteLine(e);
            }

            application.RequestExit();
        }

        private async Task RunAsync(Application application)
        {
            var audioProvider = application.GetService<IAudioProvider>();

            try
            {
                await audioProvider.StartAsync();

                await foreach (var audioAdapter in audioProvider.EnumerateAudioDevices())
                {
                    Console.WriteLine($"Adapter null: {audioAdapter == null}");
                    if (audioAdapter != null)
                    {
                    Console.WriteLine($"    Name: {audioAdapter.Name}");
                    Console.WriteLine($"        Type: {audioAdapter.DeviceType}");
                    Console.WriteLine($"        Channel count: {audioAdapter.Channels}");
                    Console.WriteLine($"        Sample rate: {audioAdapter.SampleRate}");
                    Console.WriteLine($"        Bit depth: {audioAdapter.BitDepth}");
                    Console.WriteLine($"        Endianness: {(audioAdapter.IsBigEndian ? "Big" : "Little")}");
                    }
                }
            }
            finally
            {
                await audioProvider.StopAsync();
            }
        }
    }
}
