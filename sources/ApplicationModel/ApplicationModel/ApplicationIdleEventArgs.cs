// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.ApplicationModel;

/// <summary>Represents the event args that occur during an <see cref="Application.Idle" /> event.</summary>
/// <remarks>This is a struct, rather than derived from <see cref="EventArgs" />, to prevent unnecessary heap allocations.</remarks>
public readonly struct ApplicationIdleEventArgs
{
    private readonly TimeSpan _delta;
    private readonly uint _framesPerSecond;

    /// <summary>Initializes a new instance of the <see cref="ApplicationIdleEventArgs" /> class.</summary>
    /// <param name="delta">The delta between the current and previous <see cref="Application.Idle" /> events.</param>
    /// <param name="framesPerSecond">The number of frames per second.</param>
    public ApplicationIdleEventArgs(TimeSpan delta, uint framesPerSecond)
    {
        _delta = delta;
        _framesPerSecond = framesPerSecond;
    }

    /// <summary>Gets the delta between the current and previous <see cref="Application.Idle" /> events.</summary>
    public TimeSpan Delta => _delta;

    /// <summary>Gets the number of frames per second.</summary>
    public uint FramesPerSecond => _framesPerSecond;
}
