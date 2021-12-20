// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;
using TerraFX.Graphics;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Advanced;

/// <summary>An object which is created for a graphics service.</summary>
public abstract class GraphicsServiceObject : DisposableObject
{
    private readonly GraphicsService _service;

    /// <summary>Initializes a new instance of the <see cref="GraphicsServiceObject" /> class.</summary>
    /// <param name="service">The service for which the object is being created.</param>
    /// <param name="name">The name of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="service" /> is <c>null</c>.</exception>
    protected GraphicsServiceObject(GraphicsService service, string? name = null) : base(name)
    {
        AssertNotNull(service);
        _service = service;
    }

    /// <summary>Gets the service for which the object was created.</summary>
    public GraphicsService Service => _service;
}
