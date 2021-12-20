// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>An view of memory in a graphics resource.</summary>
public abstract unsafe class GraphicsResourceView
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsDevice _device;
    private readonly GraphicsResource _resource;
    private readonly GraphicsService _service;
    private readonly uint _stride;

    /// <summary>Initializes a new instance of the <see cref="GraphicsResourceView" /> class.</summary>
    /// <param name="resource">The resource for which the resource view was created.</param>
    /// <param name="stride">The stride, in bytes, of the elements in the resource view.</param>
    /// <exception cref="ArgumentNullException"><paramref name="resource" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="stride" /> is <c>zero</c>.</exception>
    protected GraphicsResourceView(GraphicsResource resource, uint stride)
    {
        ThrowIfNull(resource);
        ThrowIfZero(stride);

        _adapter = resource.Adapter;
        _device = resource.Device;
        _resource = resource;
        _service = resource.Service;
        _stride = stride;
    }

    /// <summary>Gets the underlying adapter for <see cref="Device" />.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the underlying device for <see cref="Resource" />.</summary>
    public GraphicsDevice Device => _device;

    /// <summary>Gets or sets the name for the device object.</summary>
    public abstract string Name { get; set; }

    /// <summary>Gets the resource for which the object was created.</summary>
    public GraphicsResource Resource => _resource;

    /// <summary>Gets the underlying service for <see cref="Adapter" />.</summary>
    public GraphicsService Service => _service;

    /// <summary>Gets the stride, in bytes, of the elements in the resource view.</summary>
    public uint Stride => _stride;

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <inheritdoc cref="Dispose()" />
    /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void Dispose(bool isDisposing);
}
