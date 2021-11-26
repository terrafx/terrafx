// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.ApplicationModel;
using TerraFX.Audio;

namespace TerraFX.Samples.ServiceProviders;

public sealed class PulseAudioServiceProvider : ApplicationServiceProvider
{
    private ValueLazy<PulseAudioService> _audioService;

    public PulseAudioServiceProvider()
    {
        _audioService = new ValueLazy<PulseAudioService>(() => new PulseAudioService());
    }

    protected override void DisposeCore(bool isDisposing)
    {
        if (isDisposing)
        {
            _audioService.Dispose(DisposeAudioService);
        }
    }

    public override bool TryGetService(Type serviceType, [NotNullWhen(true)] out object? service)
    {
        if (serviceType.IsAssignableFrom(typeof(PulseAudioService)))
        {
            service = _audioService.Value;
            return true;
        }

        service = null;
        return false;
    }

    private void DisposeAudioService(IAudioService audioService)
    {
        if (audioService is IDisposable disposableAudioService)
        {
            disposableAudioService.Dispose();
        }
    }
}
