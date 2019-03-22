// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop.Desktop
{
    [Guid("2633066B-4514-4C7A-8FD8-12EA98059D18")]
    [Unmanaged]
    public unsafe struct IDXGIDecodeSwapChain
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IDXGIDecodeSwapChain* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IDXGIDecodeSwapChain* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IDXGIDecodeSwapChain* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _PresentBuffer(
            [In] IDXGIDecodeSwapChain* This,
            [In, ComAliasName("UINT")] uint BufferToPresent,
            [In, ComAliasName("UINT")] uint SyncInterval,
            [In, ComAliasName("UINT")] uint Flags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetSourceRect(
            [In] IDXGIDecodeSwapChain* This,
            [In] RECT* pRect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetTargetRect(
            [In] IDXGIDecodeSwapChain* This,
            [In] RECT* pRect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetDestSize(
            [In] IDXGIDecodeSwapChain* This,
            [In, ComAliasName("UINT")] uint Width,
            [In, ComAliasName("UINT")] uint Height
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSourceRect(
            [In] IDXGIDecodeSwapChain* This,
            [Out] RECT* pRect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetTargetRect(
            [In] IDXGIDecodeSwapChain* This,
            [Out] RECT* pRect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDestSize(
            [In] IDXGIDecodeSwapChain* This,
            [Out, ComAliasName("UINT")] uint* pWidth,
            [Out, ComAliasName("UINT")] uint* pHeight
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetColorSpace(
            [In] IDXGIDecodeSwapChain* This,
            [In] DXGI_MULTIPLANE_OVERLAY_YCbCr_FLAGS ColorSpace
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DXGI_MULTIPLANE_OVERLAY_YCbCr_FLAGS _GetColorSpace(
            [In] IDXGIDecodeSwapChain* This
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IDXGIDecodeSwapChain* This = &this)
            {
                return MarshalFunction<_QueryInterface>(lpVtbl->QueryInterface)(
                    This,
                    riid,
                    ppvObject
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint AddRef()
        {
            fixed (IDXGIDecodeSwapChain* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IDXGIDecodeSwapChain* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int PresentBuffer(
            [In, ComAliasName("UINT")] uint BufferToPresent,
            [In, ComAliasName("UINT")] uint SyncInterval,
            [In, ComAliasName("UINT")] uint Flags
        )
        {
            fixed (IDXGIDecodeSwapChain* This = &this)
            {
                return MarshalFunction<_PresentBuffer>(lpVtbl->PresentBuffer)(
                    This,
                    BufferToPresent,
                    SyncInterval,
                    Flags
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetSourceRect(
            [In] RECT* pRect
        )
        {
            fixed (IDXGIDecodeSwapChain* This = &this)
            {
                return MarshalFunction<_SetSourceRect>(lpVtbl->SetSourceRect)(
                    This,
                    pRect
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetTargetRect(
            [In] RECT* pRect
        )
        {
            fixed (IDXGIDecodeSwapChain* This = &this)
            {
                return MarshalFunction<_SetTargetRect>(lpVtbl->SetTargetRect)(
                    This,
                    pRect
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetDestSize(
            [In, ComAliasName("UINT")] uint Width,
            [In, ComAliasName("UINT")] uint Height
        )
        {
            fixed (IDXGIDecodeSwapChain* This = &this)
            {
                return MarshalFunction<_SetDestSize>(lpVtbl->SetDestSize)(
                    This,
                    Width,
                    Height
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetSourceRect(
            [Out] RECT* pRect
        )
        {
            fixed (IDXGIDecodeSwapChain* This = &this)
            {
                return MarshalFunction<_GetSourceRect>(lpVtbl->GetSourceRect)(
                    This,
                    pRect
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetTargetRect(
            [Out] RECT* pRect
        )
        {
            fixed (IDXGIDecodeSwapChain* This = &this)
            {
                return MarshalFunction<_GetTargetRect>(lpVtbl->GetTargetRect)(
                    This,
                    pRect
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDestSize(
            [Out, ComAliasName("UINT")] uint* pWidth,
            [Out, ComAliasName("UINT")] uint* pHeight
        )
        {
            fixed (IDXGIDecodeSwapChain* This = &this)
            {
                return MarshalFunction<_GetDestSize>(lpVtbl->GetDestSize)(
                    This,
                    pWidth,
                    pHeight
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetColorSpace(
            [In] DXGI_MULTIPLANE_OVERLAY_YCbCr_FLAGS ColorSpace
        )
        {
            fixed (IDXGIDecodeSwapChain* This = &this)
            {
                return MarshalFunction<_SetColorSpace>(lpVtbl->SetColorSpace)(
                    This,
                    ColorSpace
                );
            }
        }

        public DXGI_MULTIPLANE_OVERLAY_YCbCr_FLAGS GetColorSpace()
        {
            fixed (IDXGIDecodeSwapChain* This = &this)
            {
                return MarshalFunction<_GetColorSpace>(lpVtbl->GetColorSpace)(
                    This
                );
            }
        }
        #endregion

        #region Structs
        [Unmanaged]
        public struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region Fields
            public IntPtr PresentBuffer;

            public IntPtr SetSourceRect;

            public IntPtr SetTargetRect;

            public IntPtr SetDestSize;

            public IntPtr GetSourceRect;

            public IntPtr GetTargetRect;

            public IntPtr GetDestSize;

            public IntPtr SetColorSpace;

            public IntPtr GetColorSpace;
            #endregion
        }
        #endregion
    }
}
