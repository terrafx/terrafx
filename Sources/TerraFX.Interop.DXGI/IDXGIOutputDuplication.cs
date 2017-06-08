// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("191CFAC3-A341-470D-B26E-A864F428319C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGIOutputDuplication : IDXGIObject
    {
        #region IDXGIObject
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);
        #endregion

        #region Methods
        [PreserveSig]
        void GetDesc(out DXGI_OUTDUPL_DESC pDesc);

        void AcquireNextFrame([In] uint TimeoutInMilliseconds, out DXGI_OUTDUPL_FRAME_INFO pFrameInfo, [MarshalAs(UnmanagedType.Interface)] out IDXGIResource ppDesktopResource);

        void GetFrameDirtyRects([In] uint DirtyRectsBufferSize, out RECT pDirtyRectsBuffer, out uint pDirtyRectsBufferSizeRequired);

        void GetFrameMoveRects([In] uint MoveRectsBufferSize, out DXGI_OUTDUPL_MOVE_RECT pMoveRectBuffer, out uint pMoveRectsBufferSizeRequired);

        void GetFramePointerShape([In] uint PointerShapeBufferSize, [Out] IntPtr pPointerShapeBuffer, out uint pPointerShapeBufferSizeRequired, out DXGI_OUTDUPL_POINTER_SHAPE_INFO pPointerShapeInfo);

        void MapDesktopSurface(out DXGI_MAPPED_RECT pLockedRect);

        void UnMapDesktopSurface();

        void ReleaseFrame();
        #endregion
    }
}
