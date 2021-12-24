// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics shaders.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsShaderInfo
{
    /// <summary>The bytecode for the shader.</summary>
    public UnmanagedArray<byte> Bytecode;

    /// <summary>The entry point name for the shader.</summary>
    public string EntryPointName;

    /// <summary>The shader kind.</summary>
    public GraphicsShaderKind Kind;
}
