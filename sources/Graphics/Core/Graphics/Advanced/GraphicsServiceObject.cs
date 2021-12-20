// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Graphics.Advanced;

/// <summary>An object which is created for a graphics service.</summary>
public abstract class GraphicsServiceObject : IDisposable
{
    private readonly GraphicsService _service;
    private string _name;

    /// <summary>Initializes a new instance of the <see cref="GraphicsServiceObject" /> class.</summary>
    /// <param name="service">The service for which the object is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="service" /> is <c>null</c>.</exception>
    protected GraphicsServiceObject(GraphicsService service)
    {
        AssertNotNull(service);
        _service = service;

        _name = GetType().Name;
    }

    /// <summary>Gets the name of the object.</summary>
    public string Name => _name;

    /// <summary>Gets the service for which the object was created.</summary>
    public GraphicsService Service => _service;

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Sets the name of the object.</summary>
    /// <param name="value">The new name of the object.</param>
    public virtual void SetName(string value)
    {
        _name = value ?? GetType().Name;
    }

    /// <inheritdoc cref="Dispose()" />
    /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void Dispose(bool isDisposing);

    /// <inheritdoc />
    public override string ToString() => Name;
}
