// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TerraFX.Audio
{
    /// <summary>Provides access to an audio subsystem.</summary>
    public interface IAudioService
    {
        /// <summary>Enumerates the available audio adapters reported by the underlying subsystem.</summary>
        IAudioAdapterEnumerable EnumerateAudioDevices();

        /// <summary>Requests an available audio playback device from the underlying subsystem.</summary>
        /// <param name="adapter">The adapter to use.</param>
        /// <returns>Returns a Task which, when completed, returns the audio device requested.</returns>
        ValueTask<IAudioPlaybackDevice> RequestAudioPlaybackDeviceAsync(IAudioAdapter adapter);

        /// <summary>Requests an available audio recording device from the underlying subsystem.</summary>
        /// <param name="adapter">The adapter to use.</param>
        /// <returns>Returns a Task which, when completed, returns the audio device requested.</returns>
        ValueTask<IAudioRecordingDevice> RequestAudioRecordingDeviceAsync(IAudioAdapter adapter);

        /// <summary>Starts any asynchronous processing necessary to use this device.</summary>
        ValueTask StartAsync(CancellationToken cancellationToken = default);

        /// <summary>Stops any asynchronous processing necessary for this device to function.</summary>
        ValueTask StopAsync(CancellationToken cancellationToken = default);
    }
}
