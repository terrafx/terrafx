// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;

namespace TerraFX.Graphics;

/// <summary>Provides the base access required for interacting with a graphics subsystem.</summary>
public abstract class GraphicsService : IDisposable
{
    /// <summary>The name of a switch that controls whether debug mode should be enabled for the service.</summary>
    /// <remarks>
    ///     <para>This name is meant to be used with <see cref="AppContext.SetSwitch(string, bool)" />.</para>
    ///     <para>Setting this switch has no affect on services that have already been created.</para>
    /// </remarks>
    public const string EnableDebugModeSwitchName = "TerraFX.Graphics.GraphicsService.EnableDebugMode";

    private readonly bool _debugModeEnabled;

    /// <summary>Initializes a new instance of the <see cref="GraphicsService" /> class.</summary>
    protected GraphicsService()
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

    /// <summary>Gets the adapters available to the service.</summary>
    /// <exception cref="ObjectDisposedException">The service has been disposed.</exception>
    public abstract IEnumerable<GraphicsAdapter> Adapters { get; }

    /// <summary>Gets the value of the <see cref="EnableDebugModeSwitchName" /> switch from when the service was created.</summary>
    /// <remarks>The exact behavior of debug mode may vary based on the service and configuration of the host machine.</remarks>
    public bool DebugModeEnabled => _debugModeEnabled;

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
