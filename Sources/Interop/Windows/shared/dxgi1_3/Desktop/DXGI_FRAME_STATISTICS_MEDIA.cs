// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.Desktop
{
    public /* blittable */ struct DXGI_FRAME_STATISTICS_MEDIA
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint PresentCount;

        [ComAliasName("UINT")]
        public uint PresentRefreshCount;

        [ComAliasName("UINT")]
        public uint SyncRefreshCount;

        public LARGE_INTEGER SyncQPCTime;

        public LARGE_INTEGER SyncGPUTime;

        public DXGI_FRAME_PRESENTATION_MODE CompositionMode;

        [ComAliasName("UINT")]
        public uint ApprovedPresentDuration;
        #endregion
    }
}
