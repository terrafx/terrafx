// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics pipeline input which describes an input to a stage of the graphics pipeline.</summary>
public readonly unsafe struct GraphicsPipelineInput
{
    private readonly UnmanagedArray<GraphicsPipelineInputElement> _elements;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPipelineInput" /> struct.</summary>
    /// <param name="elements">The elements that make up the pipeline input.</param>
    public GraphicsPipelineInput(ReadOnlySpan<GraphicsPipelineInputElement> elements)
    {
        _elements = elements.ToUnmanagedArray();
    }

    /// <summary>Gets the elements that make up the pipeline input.</summary>
    public UnmanagedReadOnlySpan<GraphicsPipelineInputElement> Elements => _elements;
}
