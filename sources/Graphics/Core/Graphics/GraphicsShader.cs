// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <summary>A graphics shader which performs a transformation for a graphics device.</summary>
public abstract class GraphicsShader : GraphicsDeviceObject
{
    /// <summary>The information for the graphics shader.</summary>
    protected GraphicsShaderInfo ShaderInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsShader" /> class.</summary>
    /// <param name="device">The device for which the shader was created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsShader(GraphicsDevice device) : base(device)
    {
    }

    /// <summary>Gets the bytecode for the shader.</summary>
    public UnmanagedReadOnlySpan<byte> Bytecode => ShaderInfo.Bytecode;

    /// <summary>Gets the entry point name for the shader.</summary>
    public string EntryPointName => ShaderInfo.EntryPointName;

    /// <summary>Gets the shader kind.</summary>
    public GraphicsShaderKind Kind => ShaderInfo.Kind;
}
