// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics;

/// <summary>An object which is created for a graphics swapchain.</summary>
public interface IGraphicsSwapchainObject : IGraphicsRenderPassObject
{
    /// <summary>Gets the swapchain for which the object was created.</summary>
    GraphicsSwapchain Swapchain { get; }
}
