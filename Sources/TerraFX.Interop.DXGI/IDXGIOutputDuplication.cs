// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("191CFAC3-A341-470D-B26E-A864F428319C")]
    unsafe public struct IDXGIOutputDuplication
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate void GetDesc(
            [In] IDXGIOutputDuplication* This,
            [Out] DXGI_OUTDUPL_DESC* pDesc
        );

        public /* static */ delegate HRESULT AcquireNextFrame(
            [In] IDXGIOutputDuplication* This,
            [In] uint TimeoutInMilliseconds,
            [Out] DXGI_OUTDUPL_FRAME_INFO* pFrameInfo,
            [Out] IDXGIResource** ppDesktopResource
        );

        public /* static */ delegate HRESULT GetFrameDirtyRects(
            [In] IDXGIOutputDuplication* This,
            [In] uint DirtyRectsBufferSize,
            [Out] RECT* pDirtyRectsBuffer,
            [Out] uint* pDirtyRectsBufferSizeRequired
        );

        public /* static */ delegate HRESULT GetFrameMoveRects(
            [In] IDXGIOutputDuplication* This,
            [In] uint MoveRectsBufferSize,
            [Out] DXGI_OUTDUPL_MOVE_RECT* pMoveRectBuffer,
            [Out] uint* pMoveRectsBufferSizeRequired
        );

        public /* static */ delegate HRESULT GetFramePointerShape(
            [In] IDXGIOutputDuplication* This,
            [In] uint PointerShapeBufferSize,
            [Out] void* pPointerShapeBuffer,
            [Out] uint* pPointerShapeBufferSizeRequired,
            [Out] DXGI_OUTDUPL_POINTER_SHAPE_INFO* pPointerShapeInfo
        );

        public /* static */ delegate HRESULT MapDesktopSurface(
            [In] IDXGIOutputDuplication* This,
            [Out] DXGI_MAPPED_RECT* pLockedRect
        );

        public /* static */ delegate HRESULT UnMapDesktopSurface(
            [In] IDXGIOutputDuplication* This
        );

        public /* static */ delegate HRESULT ReleaseFrame(
            [In] IDXGIOutputDuplication* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIObject.Vtbl BaseVtbl;

            public GetDesc GetDesc;

            public AcquireNextFrame AcquireNextFrame;

            public GetFrameDirtyRects GetFrameDirtyRects;

            public GetFrameMoveRects GetFrameMoveRects;

            public GetFramePointerShape GetFramePointerShape;

            public MapDesktopSurface MapDesktopSurface;

            public UnMapDesktopSurface UnMapDesktopSurface;

            public ReleaseFrame ReleaseFrame;
            #endregion
        }
        #endregion
    }
}
