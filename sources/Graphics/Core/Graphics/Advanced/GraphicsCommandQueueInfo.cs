// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics command queues.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsCommandQueueInfo
{
    /// <summary>The kind of contexts in the command queue.</summary>
    public GraphicsContextKind Kind;
}
