// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics
{
    /// <summary>Represents a render target view.</summary>
    public interface IRenderTargetView
    {
        /// <summary>Gets the <see cref="ISwapChain" /> for the instance.</summary>
        ISwapChain SwapChain { get; }

        /// <summary>Gets the underlying handle for the instance.</summary>
        IntPtr Handle { get; }
    }
}
