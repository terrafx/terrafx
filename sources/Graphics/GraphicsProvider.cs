// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;

namespace TerraFX.Graphics
{
    /// <summary>Provides the base access required for interacting with a graphics subsystem.</summary>
    public abstract class GraphicsProvider : IDisposable
    {
        /// <summary>A name of a switch that controls whether <c>debug mode</c> should be enabled for the graphics provider.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppContext.SetSwitch(string, bool)" />.</para>
        ///     <para>Setting this switch after a graphics provider instance has been created has no affect.</para>
        /// </remarks>
        public const string EnableDebugModeSwitchName = "TerraFX.Graphics.GraphicsProvider.EnableDebugMode";

        private readonly bool _debugModeEnabled;

        /// <summary>Initializes a new instance of the <see cref="GraphicsProvider" /> class.</summary>
        protected GraphicsProvider()
        {
            _debugModeEnabled = GetDebugModeEnabled();

            static bool GetDebugModeEnabled()
            {
                if (!AppContext.TryGetSwitch(EnableDebugModeSwitchName, out var debugModeEnabled))
                {
#if DEBUG
                    debugModeEnabled = true;
                    AppContext.SetSwitch(EnableDebugModeSwitchName, debugModeEnabled);
#endif
                }

                return debugModeEnabled;
            }
        }

        /// <summary>Gets the value of the <see cref="EnableDebugModeSwitchName" /> switch when the instance was created.</summary>
        /// <remarks>The exact behavior of <c>debug mode</c> may vary based on the implementation and configuration of the host machine.</remarks>
        public bool DebugModeEnabled => _debugModeEnabled;

        /// <summary>Gets the <see cref="GraphicsAdapter" /> instances currently available.</summary>
        /// <exception cref="ObjectDisposedException">The instance has been disposed.</exception>
        public abstract IEnumerable<GraphicsAdapter> GraphicsAdapters { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
