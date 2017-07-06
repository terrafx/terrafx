// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("A8BE2AC4-199F-4946-B331-79599FB98DE7")]
    unsafe public /* blittable */ struct IDXGISwapChain2
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetSourceSize(
            [In] IDXGISwapChain2* This,
            [In] UINT Width,
            [In] UINT Height
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSourceSize(
            [In] IDXGISwapChain2* This,
            [Out] UINT* pWidth,
            [Out] UINT* pHeight
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetMaximumFrameLatency(
            [In] IDXGISwapChain2* This,
            [In] UINT MaxLatency
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMaximumFrameLatency(
            [In] IDXGISwapChain2* This,
            [Out] UINT* pMaxLatency
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HANDLE GetFrameLatencyWaitableObject(
            [In] IDXGISwapChain2* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetMatrixTransform(
            [In] IDXGISwapChain2* This,
            [In] /* readonly */ DXGI_MATRIX_3X2_F* pMatrix
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMatrixTransform(
            [In] IDXGISwapChain2* This,
            [Out] DXGI_MATRIX_3X2_F* pMatrix
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGISwapChain1.Vtbl BaseVtbl;

            public SetSourceSize SetSourceSize;

            public GetSourceSize GetSourceSize;

            public SetMaximumFrameLatency SetMaximumFrameLatency;

            public GetMaximumFrameLatency GetMaximumFrameLatency;

            public GetFrameLatencyWaitableObject GetFrameLatencyWaitableObject;

            public SetMatrixTransform SetMatrixTransform;

            public GetMatrixTransform GetMatrixTransform;
            #endregion
        }
        #endregion
    }
}
