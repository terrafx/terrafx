// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace TerraFX.Audio
{
    /// <summary>Represents a device which can be used for audio recording.</summary>
    public interface IAudioRecordingDevice : IDisposable
    {
        /// <summary>The adapter used for this recording device.</summary>
        IAudioAdapter Adapter { get; }

        /// <summary>The output data from the underlying device.</summary>
        PipeReader Reader { get; }

        /// <summary>Starts the audio recording device.</summary>
        Task RunAsync();
    }
}
