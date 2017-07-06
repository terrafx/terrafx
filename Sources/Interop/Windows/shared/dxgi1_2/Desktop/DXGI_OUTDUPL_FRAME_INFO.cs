// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.Desktop
{
    public /* blittable */ struct DXGI_OUTDUPL_FRAME_INFO
    {
        #region Fields
        public LARGE_INTEGER LastPresentTime;

        public LARGE_INTEGER LastMouseUpdateTime;

        public UINT AccumulatedFrames;

        public BOOL RectsCoalesced;

        public BOOL ProtectedContentMaskedOut;

        public DXGI_OUTDUPL_POINTER_POSITION PointerPosition;

        public UINT TotalMetadataBufferSize;

        public UINT PointerShapeBufferSize;
        #endregion
    }
}
