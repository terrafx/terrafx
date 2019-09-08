// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics
{
    /// <summary>Represents a graphics device.</summary>
    public interface IGraphicsDevice
    {
        /// <summary>Gets the <see cref="IGraphicsAdapter" /> for the instance.</summary>
        IGraphicsAdapter GraphicsAdapter { get; }

        /// <summary>Gets the underlying handle for the instance.</summary>
        IntPtr Handle { get; }

        /// <summary>Creates a new <see cref="ISwapChain" /> for the instance.</summary>
        /// <param name="graphicsSurface">The <see cref="IGraphicsSurface" /> to which the swap chain belongs.</param>
        /// <returns>A new <see cref="ISwapChain" /> for the instance.</returns>
        ISwapChain CreateSwapChain(IGraphicsSurface graphicsSurface);
    }
}
