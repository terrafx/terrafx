// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;

namespace TerraFX.Graphics.Advanced;

/// <summary>An object which is created for a graphics resource.</summary>
/// <remarks>Initializes a new instance of the <see cref="GraphicsResourceObject" /> class.</remarks>
/// <param name="resource">The resource for which the object is being created.</param>
/// <param name="name">The name of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
/// <exception cref="ArgumentNullException"><paramref name="resource" /> is <c>null</c>.</exception>
public abstract class GraphicsResourceObject(GraphicsResource resource, string? name = null) : GraphicsDeviceObject(resource.Device, name)
{
    private readonly GraphicsResource _resource = resource;

    /// <summary>Gets the resource for which the object was created.</summary>
    public GraphicsResource Resource => _resource;
}
