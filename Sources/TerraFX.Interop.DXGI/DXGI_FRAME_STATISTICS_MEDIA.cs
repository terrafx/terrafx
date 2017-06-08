// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct DXGI_FRAME_STATISTICS_MEDIA
    {
        public uint PresentCount;
        public uint PresentRefreshCount;
        public uint SyncRefreshCount;
        public long SyncQPCTime;
        public long SyncGPUTime;
        public DXGI_FRAME_PRESENTATION_MODE CompositionMode;
        public uint ApprovedPresentDuration;
    }
}
