// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics
{
    /// <summary>Represents a swap chain.</summary>
    public interface ISwapChain
    {
        /// <summary>Gets the <see cref="IGraphicsDevice" /> for the instance.</summary>
        IGraphicsDevice GraphicsDevice { get; }

        /// <summary>Gets the underlying handle for the instance.</summary>
        IntPtr Handle { get; }
    }
}
