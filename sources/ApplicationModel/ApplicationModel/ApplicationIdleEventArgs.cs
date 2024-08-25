// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.ApplicationModel;

/// <summary>Represents the event args that occur during an <see cref="Application.Idle" /> event.</summary>
/// <remarks>This is a struct, rather than derived from <see cref="EventArgs" />, to prevent unnecessary heap allocations.</remarks>
/// <remarks>Initializes a new instance of the <see cref="ApplicationIdleEventArgs" /> class.</remarks>
/// <param name="delta">The delta between the current and previous <see cref="Application.Idle" /> events.</param>
/// <param name="framesPerSecond">The number of frames per second.</param>
public readonly struct ApplicationIdleEventArgs(TimeSpan delta, uint framesPerSecond)
{
    private readonly TimeSpan _delta = delta;
    private readonly uint _framesPerSecond = framesPerSecond;

    /// <summary>Gets the delta between the current and previous <see cref="Application.Idle" /> events.</summary>
    public TimeSpan Delta => _delta;

    /// <summary>Gets the number of frames per second.</summary>
    public uint FramesPerSecond => _framesPerSecond;
}
