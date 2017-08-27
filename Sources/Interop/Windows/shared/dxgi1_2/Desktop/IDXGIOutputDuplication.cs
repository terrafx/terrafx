// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.Desktop
{
    [Guid("191CFAC3-A341-470D-B26E-A864F428319C")]
    public /* blittable */ unsafe struct IDXGIOutputDuplication
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
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
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AcquireNextFrame(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("UINT")] uint TimeoutInMilliseconds,
            [Out] DXGI_OUTDUPL_FRAME_INFO* pFrameInfo,
            [Out] IDXGIResource** ppDesktopResource
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFrameDirtyRects(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("UINT")] uint DirtyRectsBufferSize,
            [Out, ComAliasName("RECT[]")] RECT* pDirtyRectsBuffer,
            [Out, ComAliasName("UINT")] uint* pDirtyRectsBufferSizeRequired
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFrameMoveRects(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("UINT")] uint MoveRectsBufferSize,
            [Out, ComAliasName("DXGI_OUTDUPL_MOVE_RECT[]")] DXGI_OUTDUPL_MOVE_RECT* pMoveRectBuffer,
            [Out, ComAliasName("UINT")] uint* pMoveRectsBufferSizeRequired
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFramePointerShape(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("UINT")] uint PointerShapeBufferSize,
            [Out] void* pPointerShapeBuffer,
            [Out, ComAliasName("UINT")] uint* pPointerShapeBufferSizeRequired,
            [Out] DXGI_OUTDUPL_POINTER_SHAPE_INFO* pPointerShapeInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MapDesktopSurface(
            [In] IDXGIOutputDuplication* This,
            [Out] DXGI_MAPPED_RECT* pLockedRect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int UnMapDesktopSurface(
            [In] IDXGIOutputDuplication* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ReleaseFrame(
            [In] IDXGIOutputDuplication* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIObject.Vtbl BaseVtbl;

            public IntPtr GetDesc;

            public IntPtr AcquireNextFrame;

            public IntPtr GetFrameDirtyRects;

            public IntPtr GetFrameMoveRects;

            public IntPtr GetFramePointerShape;

            public IntPtr MapDesktopSurface;

            public IntPtr UnMapDesktopSurface;

            public IntPtr ReleaseFrame;
            #endregion
        }
        #endregion
    }
}
