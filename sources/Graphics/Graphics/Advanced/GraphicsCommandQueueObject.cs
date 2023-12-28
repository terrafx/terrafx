// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace TerraFX.Graphics.Advanced;

/// <summary>An object which is created for a graphics command queue.</summary>
public abstract class GraphicsCommandQueueObject : GraphicsDeviceObject
{
    private readonly GraphicsCommandQueue _commandQueue;

    /// <summary>Initializes a new instance of the <see cref="GraphicsCommandQueueObject" /> class.</summary>
    /// <param name="commandQueue">The command queue for which the object is being created.</param>
    /// <param name="name">The name of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="commandQueue" /> is <c>null</c>.</exception>
    protected GraphicsCommandQueueObject(GraphicsCommandQueue commandQueue, string? name = null) : base(commandQueue.Device, name)
    {
        _commandQueue = commandQueue;
    }

    /// <summary>Gets the command queue for which the object was created.</summary>
    public GraphicsCommandQueue CommandQueue => _commandQueue;
}
