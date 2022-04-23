// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;

namespace TerraFX.Graphics.Advanced;

/// <summary>An object which is created for a graphics adapter.</summary>
public abstract class GraphicsAdapterObject : GraphicsServiceObject
{
    private readonly GraphicsAdapter _adapter;

    /// <summary>Initializes a new instance of the <see cref="GraphicsAdapterObject" /> class.</summary>
    /// <param name="adapter">The adapter for which the object is being created.</param>
    /// <param name="name">The name of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="adapter" /> is <c>null</c>.</exception>
    protected GraphicsAdapterObject(GraphicsAdapter adapter, string? name = null) : base(adapter.Service, name)
    {
        _adapter = adapter;
    }

    /// <summary>Gets the adapter for which the object was created.</summary>
    public GraphicsAdapter Adapter => _adapter;
}
