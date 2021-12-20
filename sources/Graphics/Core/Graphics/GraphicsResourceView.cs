// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;
using System.Reflection;
using TerraFX.Advanced;

namespace TerraFX.Graphics;

/// <summary>An view of memory in a graphics resource.</summary>
public abstract unsafe class GraphicsResourceView : GraphicsDeviceObject
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsDevice _device;
    private readonly GraphicsResource _resource;
    private readonly GraphicsService _service;
    private readonly uint _stride;

    /// <summary>Initializes a new instance of the <see cref="GraphicsResourceView" /> class.</summary>
    /// <param name="resource">The resource for which the resource view was created.</param>
    /// <param name="stride">The stride, in bytes, of the elements in the resource view.</param>
    /// <param name="name">The name of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="resource" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="stride" /> is <c>zero</c>.</exception>
    protected GraphicsResourceView(GraphicsResource resource, uint stride, string? name = null) : base(resource.Device, name)
    {
        ThrowIfNull(resource);
        ThrowIfZero(stride);

        _adapter = resource.Adapter;
        _device = resource.Device;
        _resource = resource;
        _service = resource.Service;
        _stride = stride;
    }

    /// <summary>Gets the resource for which the object was created.</summary>
    public GraphicsResource Resource => _resource;

    /// <summary>Gets the stride, in bytes, of the elements in the resource view.</summary>
    public uint Stride => _stride;
}
