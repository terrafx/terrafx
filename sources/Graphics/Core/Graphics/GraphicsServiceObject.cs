// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>An object which is created for a graphics service.</summary>
public abstract class GraphicsServiceObject
{
    private readonly GraphicsService _service;

    /// <summary>Initializes a new instance of the <see cref="GraphicsServiceObject" /> class.</summary>
    /// <param name="service">The service for which the object is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="service" /> is <c>null</c>.</exception>
    protected GraphicsServiceObject(GraphicsService service)
    {
        ThrowIfNull(service);
        _service = service;
    }

    /// <summary>Gets the service for which the object was created.</summary>
    public GraphicsService Service => _service;

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
