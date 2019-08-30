// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;

namespace TerraFX.Graphics
{
    /// <summary>Provides access to a graphics subsystem.</summary>
    public interface IGraphicsProvider
    {
        /// <summary>Gets the <see cref="IGraphicsAdapter" /> instances currently available.</summary>
        IEnumerable<IGraphicsAdapter> GraphicsAdapters { get; }

        /// <summary>Gets the underlying handle for the instance.</summary>
        IntPtr Handle { get; }
    }
}
