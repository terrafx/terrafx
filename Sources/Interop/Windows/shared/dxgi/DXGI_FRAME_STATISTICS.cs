// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_FRAME_STATISTICS
    {
        #region Fields
        public UINT PresentCount;

        public UINT PresentRefreshCount;

        public UINT SyncRefreshCount;

        public LARGE_INTEGER SyncQPCTime;

        public LARGE_INTEGER SyncGPUTime;
        #endregion
    }
}
