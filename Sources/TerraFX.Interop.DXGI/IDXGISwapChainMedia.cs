// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("DD95B90B-F05F-4F6A-BD65-25BFB264BD84")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGISwapChainMedia
    {
        #region Methods
        void GetFrameStatisticsMedia(out DXGI_FRAME_STATISTICS_MEDIA pStats);

        void SetPresentDuration(uint Duration);

        void CheckPresentDurationSupport(uint DesiredPresentDuration, out uint pClosestSmallerPresentDuration, out uint pClosestLargerPresentDuration);
        #endregion
    }
}
