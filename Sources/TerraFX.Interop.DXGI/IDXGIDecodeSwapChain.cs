// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("2633066B-4514-4C7A-8FD8-12EA98059D18")]
    unsafe public struct IDXGIDecodeSwapChain
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT PresentBuffer(
            [In] IDXGIDecodeSwapChain* This,
            [In] uint BufferToPresent,
            [In] uint SyncInterval,
            [In] DXGI_PRESENT_FLAG Flags
        );

        public /* static */ delegate HRESULT SetSourceRect(
            [In] IDXGIDecodeSwapChain* This,
            [In] /* readonly */ RECT* pRect
        );

        public /* static */ delegate HRESULT SetTargetRect(
            [In] IDXGIDecodeSwapChain* This,
            [In] /* readonly */ RECT* pRect
        );

        public /* static */ delegate HRESULT SetDestSize(
            [In] IDXGIDecodeSwapChain* This,
            [In] uint Width,
            [In] uint Height
        );

        public /* static */ delegate HRESULT GetSourceRect(
            [In] IDXGIDecodeSwapChain* This,
            [Out] RECT* pRect
        );

        public /* static */ delegate HRESULT GetTargetRect(
            [In] IDXGIDecodeSwapChain* This,
            [Out] RECT* pRect
        );

        public /* static */ delegate HRESULT GetDestSize(
            [In] IDXGIDecodeSwapChain* This,
            [Out] uint* pWidth,
            [Out] uint* pHeight
        );

        public /* static */ delegate HRESULT SetColorSpace(
            [In] IDXGIDecodeSwapChain* This,
            [In] DXGI_MULTIPLANE_OVERLAY_YCbCr_FLAGS ColorSpace
        );

        public /* static */ delegate DXGI_MULTIPLANE_OVERLAY_YCbCr_FLAGS GetColorSpace(
            [In] IDXGIDecodeSwapChain* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public PresentBuffer PresentBuffer;

            public SetSourceRect SetSourceRect;

            public SetTargetRect SetTargetRect;

            public SetDestSize SetDestSize;

            public GetSourceRect GetSourceRect;

            public GetTargetRect GetTargetRect;

            public GetDestSize GetDestSize;

            public SetColorSpace SetColorSpace;

            public GetColorSpace GetColorSpace;
            #endregion
        }
        #endregion
    }
}
