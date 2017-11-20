// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.Desktop
{
    public /* blittable */ struct DXGI_OUTDUPL_FRAME_INFO
    {
        #region Fields
        public LARGE_INTEGER LastPresentTime;

        public LARGE_INTEGER LastMouseUpdateTime;

        [ComAliasName("UINT")]
        public uint AccumulatedFrames;

        [ComAliasName("BOOL")]
        public int RectsCoalesced;

        [ComAliasName("BOOL")]
        public int ProtectedContentMaskedOut;

        public DXGI_OUTDUPL_POINTER_POSITION PointerPosition;

        [ComAliasName("UINT")]
        public uint TotalMetadataBufferSize;

        [ComAliasName("UINT")]
        public uint PointerShapeBufferSize;
        #endregion
    }
}
