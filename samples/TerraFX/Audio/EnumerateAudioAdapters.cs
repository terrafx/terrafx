// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading.Tasks;
using TerraFX.ApplicationModel;
using TerraFX.Audio;
using TerraFX.Utilities;

namespace TerraFX.Samples.Audio;

public sealed class EnumerateAudioAdapters : Sample
{
    private readonly bool _async = false;
    private IAudioService? _service;

    public EnumerateAudioAdapters(string name, bool @async, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
        _async = @async;
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
        ExceptionUtilities.ThrowIfNull(_service, nameof(_service));

        var task = _service.StopAsync();
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
        ExceptionUtilities.ThrowIfNull(_service, nameof(_service));

        if (_async)
        {
            await foreach (var audioAdapter in _service.EnumerateAudioDevices())
            {
                PrintAudioAdapter(audioAdapter);
            }
        }
        else
        {
            foreach (var audioAdapter in _service.EnumerateAudioDevices())
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
