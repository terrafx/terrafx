// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;

namespace TerraFX.Graphics
{
    /// <summary>Provides the base access required for interacting with a graphics subsystem.</summary>
    public interface GraphicsProvider : IDisposable
    {
        /// <summary>A name of a switch that controls whether <c>debug mode</c> should be enabled for the graphics provider.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppContext.SetSwitch(string, bool)" />.</para>
        ///     <para>Setting this switch after a graphics provider instance has been created has no affect.</para>
        /// </remarks>
        public const string EnableDebugModeSwitchName = "TerraFX.Graphics.IGraphicsProvider.EnableDebugMode";

        /// <summary>Gets the value of the <see cref="EnableDebugModeSwitchName" /> switch when the instance was created.</summary>
        /// <remarks>The exact behavior of <c>debug mode</c> may vary based on the implementation and configuration of the host machine.</remarks>
        public bool DebugModeEnabled { get; }

        /// <summary>Gets the <see cref="GraphicsAdapter" /> instances currently available.</summary>
        /// <exception cref="ObjectDisposedException">The instance has been disposed.</exception>
        IEnumerable<GraphicsAdapter> GraphicsAdapters { get; }
    }
}
