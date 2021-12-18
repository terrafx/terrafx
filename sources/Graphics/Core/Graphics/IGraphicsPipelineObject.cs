// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics;

/// <summary>An object which is created for a graphics pipeline.</summary>
public interface IGraphicsPipelineObject : IGraphicsRenderPassObject
{
    /// <summary>Gets the render pass for which the object was created.</summary>
    GraphicsPipeline Pipeline { get; }
}
