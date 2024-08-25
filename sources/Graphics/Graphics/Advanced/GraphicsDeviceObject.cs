// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;

namespace TerraFX.Graphics.Advanced;

/// <summary>An object which is created for a graphics device.</summary>
/// <remarks>Initializes a new instance of the <see cref="GraphicsDeviceObject" /> class.</remarks>
/// <param name="device">The device for which the object is being created.</param>
/// <param name="name">The name of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
/// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
public abstract class GraphicsDeviceObject(GraphicsDevice device, string? name = null) : GraphicsAdapterObject(device.Adapter, name)
{
    private readonly GraphicsDevice _device = device;

    /// <summary>Gets the device for which the object was created.</summary>
    public GraphicsDevice Device => _device;
}
