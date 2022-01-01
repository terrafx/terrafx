// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <summary>A graphics adapter which can be used for computational or graphical operations.</summary>
public abstract unsafe class GraphicsAdapter : GraphicsServiceObject
{
    /// <summary>The information for the graphics adapter.</summary>
    protected GraphicsAdapterInfo AdapterInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsAdapter" /> class.</summary>
    /// <param name="service">The service which enumerated the adapter.</param>
    /// <exception cref="ArgumentNullException"><paramref name="service" /> is <c>null</c>.</exception>
    protected GraphicsAdapter(GraphicsService service) : base(service)
    {
    }

    /// <summary>Gets a description of the adapter.</summary>
    public string Description => AdapterInfo.Description;

    /// <summary>Gets the PCI Device ID (DID) for the adapter.</summary>
    public uint PciDeviceId => AdapterInfo.PciDeviceId;

    /// <summary>Gets the PCI Vendor ID (VID) for the adapter.</summary>
    public uint PciVendorId => AdapterInfo.PciVendorId;

    /// <summary>Creates a new graphics device which utilizes the adapter.</summary>
    /// <exception cref="ObjectDisposedException">The adapter has been disposed.</exception>
    public GraphicsDevice CreateDevice()
    {
        ThrowIfDisposed();

        var createOptions = new GraphicsDeviceCreateOptions {
            CreateMemoryAllocator = default,
        };
        return CreateDeviceUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics device which utilizes the adapter.</summary>
    /// <param name="createOptions">The options to use when creating the device.</param>
    /// <exception cref="ObjectDisposedException">The adapter has been disposed.</exception>
    public GraphicsDevice CreateDevice(in GraphicsDeviceCreateOptions createOptions)
    {
        ThrowIfDisposed();
        return CreateDeviceUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics device which utilizes the adapter.</summary>
    /// <param name="createOptions">The options to use when creating the device.</param>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract GraphicsDevice CreateDeviceUnsafe(in GraphicsDeviceCreateOptions createOptions);
}
