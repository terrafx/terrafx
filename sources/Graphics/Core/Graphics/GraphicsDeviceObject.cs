// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>An object which is created for a graphics device.</summary>
public abstract class GraphicsDeviceObject : IDisposable
{
    private readonly GraphicsDevice _device;

    /// <summary>Initializes a new instance of the <see cref="GraphicsDeviceObject" /> class.</summary>
    /// <param name="device">The device for which the object is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsDeviceObject(GraphicsDevice device)
    {
        ThrowIfNull(device);
        _device = device;
    }

    /// <summary>Gets the device for which the object was created.</summary>
    public GraphicsDevice Device => _device;

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc cref="Dispose()" />
    /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void Dispose(bool isDisposing);
}
