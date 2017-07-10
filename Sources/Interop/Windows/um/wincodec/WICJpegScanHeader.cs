// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct WICJpegScanHeader
    {
        #region Fields
        public UINT cComponents;

        public UINT RestartInterval;

        public DWORD ComponentSelectors;

        public DWORD HuffmanTableIndices;

        public BYTE StartSpectralSelection;

        public BYTE EndSpectralSelection;

        public BYTE SuccessiveApproximationHigh;

        public BYTE SuccessiveApproximationLow;
        #endregion
    }
}
