// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 8, Size = 32)]
    public struct DXGI_FRAME_STATISTICS
    {
        #region Fields
        public uint PresentCount;

        public uint PresentRefreshCount;

        public uint SyncRefreshCount;

        public long SyncQPCTime;

        public long SyncGPUTime;
        #endregion
    }
}
