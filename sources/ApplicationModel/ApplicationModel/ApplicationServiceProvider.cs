// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Graphics;
using TerraFX.Threading;
using TerraFX.UI;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.ApplicationModel;

/// <summary>Provides services for an application.</summary>
public abstract class ApplicationServiceProvider : IDisposable, IServiceProvider
{
    private VolatileState _state;

    /// <summary>Initializes a new instance of the <see cref="ApplicationServiceProvider" /> class.</summary>
    protected ApplicationServiceProvider()
    {
        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="ApplicationServiceProvider" /> class.</summary>
    ~ApplicationServiceProvider()
        => Dispose(isDisposing: false);

    /// <summary>Gets the graphics service for the application.</summary>
    /// <exception cref="ArgumentOutOfRangeException">A graphics service is not available for the application.</exception>
    public GraphicsService GraphicsService => GetService<GraphicsService>();

    /// <summary>Gets the UI service for the application.</summary>
    /// <exception cref="ArgumentOutOfRangeException">A UI service is not available for the application.</exception>
    public UIService UIService => GetService<UIService>();

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Gets the service of the specified type for the application.</summary>
    /// <typeparam name="TService">The type of the service to retrieve.</typeparam>
    /// <returns>The <typeparamref name="TService" /> instance for the application.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><typeparamref name="TService" /> is not supported.</exception>
    public TService GetService<TService>()
        where TService : class
    {
        if (!TryGetService<TService>(out var service))
        {
            ThrowForUnsupportedValue(typeof(TService));
        }
        return service;
    }

    /// <summary>Tries to get the service of the specified type for the application.</summary>
    /// <typeparam name="TService">The type of the service to retrieve.</typeparam>
    /// <param name="service">On success, contains the <typeparamref name="TService" /> instance for the application; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if an instance of <typeparamref name="TService" /> exists for the application; otherwise, <c>false</c>.</returns>
    public bool TryGetService<TService>([NotNullWhen(true)] out TService? service)
        where TService : class
    {
        if (!TryGetService(typeof(TService), out var result))
        {
            service = null;
            return false;
        }

        service = (TService)result;
        return true;
    }

    /// <summary>Tries to get the service of the specified type for the application.</summary>
    /// <param name="serviceType">The type of the service to retrieve.</param>
    /// <param name="service">On success, contains the <paramref name="serviceType" /> instance for the application; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if an instance of <paramref name="serviceType" /> exists for the application; otherwise, <c>false</c>.</returns>
    public abstract bool TryGetService(Type serviceType, [NotNullWhen(true)] out object? service);

    /// <inheritdoc cref="Dispose()" />
    /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void DisposeCore(bool isDisposing);

    private void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            DisposeCore(isDisposing);
        }

        _state.EndDispose();
    }

    object? IServiceProvider.GetService(Type serviceType)
        => TryGetService(serviceType, out var service) ? service : null;
}
