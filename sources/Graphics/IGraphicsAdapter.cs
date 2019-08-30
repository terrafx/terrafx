// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics
{
    /// <summary>Represents a graphics adapter.</summary>
    public interface IGraphicsAdapter
    {
        /// <summary>Gets the PCI ID of the device.</summary>
        uint DeviceId { get; }

        /// <summary>Gets the name of the device.</summary>
        string DeviceName { get; }

        /// <summary>Gets the <see cref="IGraphicsProvider" /> for the instance.</summary>
        IGraphicsProvider GraphicsProvider { get; }

        /// <summary>Gets the underlying handle for the instance.</summary>
        IntPtr Handle { get; }

        /// <summary>Gets the PCI ID of the vendor.</summary>
        uint VendorId { get; }
    }
}
