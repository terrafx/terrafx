// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;

namespace TerraFX.Graphics
{
    /// <summary>Provides the base access required for interacting with a graphics subsystem.</summary>
    public abstract class GraphicsProvider : IDisposable
    {
        /// <summary>The name of a switch that controls whether debug mode should be enabled for the provider.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppContext.SetSwitch(string, bool)" />.</para>
        ///     <para>Setting this switch has no affect on providers that have already been created.</para>
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

        /// <summary>Gets the value of the <see cref="EnableDebugModeSwitchName" /> switch from when the provider was created.</summary>
        /// <remarks>The exact behavior of debug mode may vary based on the provider and configuration of the host machine.</remarks>
        public bool DebugModeEnabled => _debugModeEnabled;

        /// <summary>Gets the graphics adapters available to the provider.</summary>
        /// <exception cref="ObjectDisposedException">The provider has been disposed.</exception>
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
