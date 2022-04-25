// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;

namespace TerraFX.Graphics.Advanced;

/// <summary>An object which is created for a graphics resource.</summary>
public abstract class GraphicsResourceObject : GraphicsDeviceObject
{
    private readonly GraphicsResource _resource;

    /// <summary>Initializes a new instance of the <see cref="GraphicsResourceObject" /> class.</summary>
    /// <param name="resource">The resource for which the object is being created.</param>
    /// <param name="name">The name of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="resource" /> is <c>null</c>.</exception>
    protected GraphicsResourceObject(GraphicsResource resource, string? name = null) : base(resource.Device, name)
    {
        _resource = resource;
    }

    /// <summary>Gets the resource for which the object was created.</summary>
    public GraphicsResource Resource => _resource;
}
