// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AppContextUtilities;

namespace TerraFX.Graphics;

/// <summary>Provides the base access required for interacting with a graphics subsystem.</summary>
public abstract class GraphicsService : IDisposable
{
    /// <summary><c>true</c> if debug mode should be enabled for the service; otherwise, <c>false</c>.</summary>
    /// <remarks>This defaults to <see cref="IsDebug" /> causing it to be enabled for debug builds and disabled for release builds by default.</remarks>
    public static readonly bool EnableDebugMode = GetAppContextData(
        $"{typeof(GraphicsService).FullName}.{nameof(EnableDebugMode)}",
        defaultValue: IsDebug
    );

    /// <summary><c>true</c> if GPU validation should be enabled for the service; otherwise, <c>false</c>.</summary>
    /// <remarks>This defaults to <see cref="IsDebug" /> causing it to be enabled for debug builds and disabled for release builds by default.</remarks>
    public static readonly bool EnableGpuValidation = GetAppContextData(
        $"{typeof(GraphicsService).FullName}.{nameof(EnableGpuValidation)}",
        defaultValue: IsDebug
    );

    /// <summary>Initializes a new instance of the <see cref="GraphicsService" /> class.</summary>
    protected GraphicsService()
    {
    }

    /// <summary>Gets the adapters available to the service.</summary>
    /// <exception cref="ObjectDisposedException">The service has been disposed.</exception>
    public abstract IEnumerable<GraphicsAdapter> Adapters { get; }

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
