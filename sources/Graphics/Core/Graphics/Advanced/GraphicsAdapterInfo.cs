// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics adapters.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsAdapterInfo
{
    /// <summary>A description of the adapter.</summary>
    public string Description;

    /// <summary>The PCI Device ID (DID) for the adapter.</summary>
    public uint PciDeviceId;

    /// <summary>The PCI Vendor ID (VID) for the adapter.</summary>
    public uint PciVendorId;
}
