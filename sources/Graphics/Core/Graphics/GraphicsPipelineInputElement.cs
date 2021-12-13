// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics;

/// <summary>A graphics pipeline input element which describes an element graphics pipeline input.</summary>
public readonly struct GraphicsPipelineInputElement
{
    private readonly GraphicsPipelineInputElementKind _kind;
    private readonly GraphicsFormat _format;
    private readonly uint _size;
    private readonly uint _alignment;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPipelineInputElement" /> struct.</summary>
    /// <param name="kind">The kind of the input element.</param>
    /// <param name="format">The format of the input element.</param>
    /// <param name="size">The size, in bytes, of the input element.</param>
    /// <param name="alignment">The alignment, in bytes, of the input element.</param>
    public GraphicsPipelineInputElement(GraphicsPipelineInputElementKind kind, GraphicsFormat format, uint size, uint alignment)
    {
        _kind = kind;
        _format = format;
        _size = size;
        _alignment = alignment;
    }

    /// <summary>Gets the alignment, in bytes, of the input element.</summary>
    public uint Alignment => _alignment;

    /// <summary>Gets the format of the input element.</summary>
    public GraphicsFormat Format => _format;

    /// <summary>Gets the kind of the input element.</summary>
    public GraphicsPipelineInputElementKind Kind => _kind;

    /// <summary>Gets the size, in bytes, of the input element.</summary>
    public uint Size => _size;
}
