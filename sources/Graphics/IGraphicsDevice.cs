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
    }
}
