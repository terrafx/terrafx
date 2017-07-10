// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct WICJpegFrameHeader
    {
        #region Fields
        public UINT Width;

        public UINT Height;

        public WICJpegTransferMatrix TransferMatrix;

        public WICJpegScanType ScanType;

        public UINT cComponents;

        public DWORD ComponentIdentifiers;

        public DWORD SampleFactors;

        public DWORD QuantizationTableIndices;
        #endregion
    }
}
