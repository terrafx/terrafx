// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics
{
    /// <summary>A graphics adapter which can be used for computational or graphical operations.</summary>
    public interface IGraphicsAdapter : IDisposable
    {
        /// <summary>Gets the PCI ID of the device.</summary>
        /// <exception cref="ObjectDisposedException">The instance has been disposed and the value was not otherwise cached.</exception>
        uint DeviceId { get; }

        /// <summary>Gets the name of the device.</summary>
        /// <exception cref="ObjectDisposedException">The instance has been disposed and the value was not otherwise cached.</exception>
        string DeviceName { get; }

        /// <summary>Gets the <see cref="IGraphicsProvider" /> for the instance.</summary>
        IGraphicsProvider GraphicsProvider { get; }

        /// <summary>Gets the PCI ID of the vendor.</summary>
        /// <exception cref="ObjectDisposedException">The instance has been disposed and the value was not otherwise cached.</exception>
        uint VendorId { get; }

        /// <summary>Creates a new <see cref="IGraphicsContext" />.</summary>
        /// <param name="graphicsSurface">The <see cref="IGraphicsSurface" /> on which the graphics context can draw.</param>
        /// <returns>A new <see cref="IGraphicsContext" /> which utilizes the current instance and which can draw on <paramref name="graphicsSurface" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsSurface" /> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">The instance has been disposed.</exception>
        IGraphicsContext CreateGraphicsContext(IGraphicsSurface graphicsSurface);
    }
}
