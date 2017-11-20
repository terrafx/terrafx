// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ struct WICJpegScanHeader
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint cComponents;

        [ComAliasName("UINT")]
        public uint RestartInterval;

        [ComAliasName("DWORD")]
        public uint ComponentSelectors;

        [ComAliasName("DWORD")]
        public uint HuffmanTableIndices;

        [ComAliasName("BYTE")]
        public byte StartSpectralSelection;

        [ComAliasName("BYTE")]
        public byte EndSpectralSelection;

        [ComAliasName("BYTE")]
        public byte SuccessiveApproximationHigh;

        [ComAliasName("BYTE")]
        public byte SuccessiveApproximationLow;
        #endregion
    }
}
