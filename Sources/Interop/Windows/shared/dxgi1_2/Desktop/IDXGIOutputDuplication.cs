// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.Desktop
{
    [Guid("191CFAC3-A341-470D-B26E-A864F428319C")]
    unsafe public /* blittable */ struct IDXGIOutputDuplication
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDesc(
            [In] IDXGIOutputDuplication* This,
            [Out] DXGI_OUTDUPL_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AcquireNextFrame(
            [In] IDXGIOutputDuplication* This,
            [In] UINT TimeoutInMilliseconds,
            [Out] DXGI_OUTDUPL_FRAME_INFO* pFrameInfo,
            [Out] IDXGIResource** ppDesktopResource
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFrameDirtyRects(
            [In] IDXGIOutputDuplication* This,
            [In] UINT DirtyRectsBufferSize,
            [Out] RECT* pDirtyRectsBuffer,
            [Out] UINT* pDirtyRectsBufferSizeRequired
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFrameMoveRects(
            [In] IDXGIOutputDuplication* This,
            [In] UINT MoveRectsBufferSize,
            [Out] DXGI_OUTDUPL_MOVE_RECT* pMoveRectBuffer,
            [Out] UINT* pMoveRectsBufferSizeRequired
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFramePointerShape(
            [In] IDXGIOutputDuplication* This,
            [In] UINT PointerShapeBufferSize,
            [Out] void* pPointerShapeBuffer,
            [Out] UINT* pPointerShapeBufferSizeRequired,
            [Out] DXGI_OUTDUPL_POINTER_SHAPE_INFO* pPointerShapeInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT MapDesktopSurface(
            [In] IDXGIOutputDuplication* This,
            [Out] DXGI_MAPPED_RECT* pLockedRect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT UnMapDesktopSurface(
            [In] IDXGIOutputDuplication* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT ReleaseFrame(
            [In] IDXGIOutputDuplication* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
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
