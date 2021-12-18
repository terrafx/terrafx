// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Audio;

namespace TerraFX.Samples.ServiceProviders;

public sealed class PulseAudioServiceProvider : XlibWindowServiceProvider
{
    private ValueLazy<PulseAudioService> _audioService;

    public PulseAudioServiceProvider()
    {
        _audioService = new ValueLazy<PulseAudioService>(() => new PulseAudioService());
    }

    public override bool TryGetService(Type serviceType, [NotNullWhen(true)] out object? service)
    {
        if (serviceType.IsAssignableFrom(typeof(PulseAudioService)))
        {
            service = _audioService.Value;
            return true;
        }

        return base.TryGetService(serviceType, out service);
    }

    protected override void DisposeCore(bool isDisposing)
    {
        _audioService.Dispose(DisposeAudioService);
        base.DisposeCore(isDisposing);
    }

    private void DisposeAudioService(IAudioService audioService)
    {
        if (audioService is IDisposable disposableAudioService)
        {
            disposableAudioService.Dispose();
        }
    }
}
