// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace TerraFX.Graphics.Advanced;

/// <summary>The options used when creating a graphics pipeline descriptor set.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsPipelineDescriptorSetCreateOptions
{
    /// <summary>The resource views for the pipeline descriptor set.</summary>
    public GraphicsResourceView[] ResourceViews;

    /// <summary><c>true</c> if the graphics pipeline descriptor set should take ownership of <see cref="ResourceViews" />; otherwise, <c>false</c>.</summary>
    public bool TakeResourceViewsOwnership;
}
