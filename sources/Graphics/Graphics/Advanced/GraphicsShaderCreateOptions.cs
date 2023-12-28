// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace TerraFX.Graphics.Advanced;

/// <summary>The options used when creating a graphics shader.</summary>
[StructLayout(LayoutKind.Auto)]
public unsafe struct GraphicsShaderCreateOptions
{
    /// <summary>The bytecode for the shader.</summary>
    public UnmanagedArray<byte> Bytecode;

    /// <summary>The entry point name for the shader.</summary>
    public string EntryPointName;

    /// <summary>The shader kind.</summary>
    public GraphicsShaderKind ShaderKind;

    /// <summary><c>true</c> if the graphics shader should take ownership of <see cref="Bytecode" />; otherwise, <c>false</c>.</summary>
    public bool TakeBytecodeOwnership;
}
