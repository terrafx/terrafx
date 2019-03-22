// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public struct WICJpegFrameHeader
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint Width;

        [ComAliasName("UINT")]
        public uint Height;

        public WICJpegTransferMatrix TransferMatrix;

        public WICJpegScanType ScanType;

        [ComAliasName("UINT")]
        public uint cComponents;

        [ComAliasName("DWORD")]
        public uint ComponentIdentifiers;

        [ComAliasName("DWORD")]
        public uint SampleFactors;

        [ComAliasName("DWORD")]
        public uint QuantizationTableIndices;
        #endregion
    }
}
