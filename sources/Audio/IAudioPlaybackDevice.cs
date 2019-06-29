// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace TerraFX.Audio
{
    /// <summary>Represents a device which can be used for audio playback.</summary>
    public interface IAudioPlaybackDevice : IDisposable
    {
        /// <summary>The adapter used for this playback device.</summary>
        IAudioAdapter Adapter { get; }


        /// <summary>The input data to be given to the underlying device.</summary>
        PipeWriter Writer { get; }

        /// <summary>Starts the audio playback device.</summary>
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
