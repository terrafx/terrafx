// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <summary>A graphics adapter which can be used for computational or graphical operations.</summary>
public abstract class GraphicsAdapter : GraphicsServiceObject
{
    /// <summary>Initializes a new instance of the <see cref="GraphicsAdapter" /> class.</summary>
    /// <param name="service">The service which enumerated the adapter.</param>
    /// <exception cref="ArgumentNullException"><paramref name="service" /> is <c>null</c>.</exception>
    protected GraphicsAdapter(GraphicsService service) : base(service)
    {
    }

    /// <summary>Gets the PCI Device ID (DID) for the adapter.</summary>
    /// <exception cref="ObjectDisposedException">The adapter has been disposed and the value was not otherwise cached.</exception>
    public abstract uint DeviceId { get; }

    /// <summary>Gets the name of the adapter.</summary>
    /// <exception cref="ObjectDisposedException">The adapter has been disposed and the value was not otherwise cached.</exception>
    public abstract string Name { get; }

    /// <summary>Gets the PCI Vendor ID (VID) for the adapter.</summary>
    /// <exception cref="ObjectDisposedException">The adapter has been disposed and the value was not otherwise cached.</exception>
    public abstract uint VendorId { get; }

    /// <summary>Creates a new graphics device which utilizes the adapter.</summary>
    /// <exception cref="ObjectDisposedException">The adapter has been disposed.</exception>
    public abstract GraphicsDevice CreateDevice();

    /// <inheritdoc />
    public override string ToString() => Name;
}
