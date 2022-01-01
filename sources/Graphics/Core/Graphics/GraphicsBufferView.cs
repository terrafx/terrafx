// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics buffer view.</summary>
public abstract unsafe class GraphicsBufferView : GraphicsResourceView
{
    /// <summary>The information for the graphics buffer view.</summary>
    protected GraphicsBufferViewInfo BufferViewInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsBufferView" /> class.</summary>
    /// <param name="buffer">The buffer for which the buffer view was created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="buffer" /> is <c>null</c></exception>
    protected GraphicsBufferView(GraphicsBuffer buffer) : base(buffer)
    {
        ResourceViewInfo.Kind = GraphicsResourceKind.Buffer;
    }

    /// <summary>Gets the buffer view kind.</summary>
    public new GraphicsBufferKind Kind => BufferViewInfo.Kind;

    /// <summary>Gets the memory region in which the buffer view exists.</summary>
    public ref readonly GraphicsMemoryRegion MemoryRegion => ref BufferViewInfo.MemoryRegion;

    /// <inheritdoc cref="GraphicsResourceObject.Resource" />
    public new GraphicsBuffer Resource => base.Resource.As<GraphicsBuffer>();
}
