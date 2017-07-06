// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_QUERY_VIDEO_MEMORY_INFO
    {
        #region Fields
        public UINT64 Budget;

        public UINT64 CurrentUsage;

        public UINT64 AvailableForReservation;

        public UINT64 CurrentReservation;
        #endregion
    }
}
