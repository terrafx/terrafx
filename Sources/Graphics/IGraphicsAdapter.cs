// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics
{
    /// <summary>Represents a graphics adapter.</summary>
    public interface IGraphicsAdapter
    {
        #region Properties
        /// <summary>Gets the name of the device.</summary>
        string DeviceName { get; }

        /// <summary>Gets the PCI ID of the vendor.</summary>
        uint VendorId { get; }

        /// <summary>Gets the PCI ID of the device.</summary>
        uint DeviceId { get; }
        #endregion
    }
}
