// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>An object which is created for a graphics adapter.</summary>
public abstract class GraphicsAdapterObject : IGraphicsAdapterObject
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsService _service;

    /// <summary>Initializes a new instance of the <see cref="GraphicsAdapterObject" /> class.</summary>
    /// <param name="adapter">The adapter for which the object is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="adapter" /> is <c>null</c>.</exception>
    protected GraphicsAdapterObject(GraphicsAdapter adapter)
    {
        ThrowIfNull(adapter);

        _adapter = adapter;
        _service = adapter.Service;
    }

    /// <summary>Gets the adapter for which the object was created.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the underlying service for <see cref="Adapter" />.</summary>
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
