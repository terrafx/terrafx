// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("5009834F-2D6A-41CE-9E1B-17C5AFF7A782")]
    public /* unmanaged */ unsafe struct IWICBitmapFlipRotator
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICBitmapFlipRotator* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICBitmapFlipRotator* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICBitmapFlipRotator* This
        );
        #endregion

        #region IWICBitmapSource Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSize(
            [In] IWICBitmapFlipRotator* This,
            [Out, ComAliasName("UINT")] uint* puiWidth,
            [Out, ComAliasName("UINT")] uint* puiHeight
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPixelFormat(
            [In] IWICBitmapFlipRotator* This,
            [Out, ComAliasName("WICPixelFormatGUID")] Guid* pPixelFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetResolution(
            [In] IWICBitmapFlipRotator* This,
            [Out] double* pDpiX,
            [Out] double* pDpiY
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyPalette(
            [In] IWICBitmapFlipRotator* This,
            [In] IWICPalette* pIPalette = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyPixels(
            [In] IWICBitmapFlipRotator* This,
            [In, Optional] WICRect* prc,
            [In, ComAliasName("UINT")] uint cbStride,
            [In, ComAliasName("UINT")] uint cbBufferSize,
            [Out, ComAliasName("BYTE[]")] byte* pbBuffer
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Initialize(
            [In] IWICBitmapFlipRotator* This,
            [In, Optional] IWICBitmapSource* pISource,
            [In] WICBitmapTransformOptions options
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICBitmapFlipRotator* This = &this)
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
            fixed (IWICBitmapFlipRotator* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICBitmapFlipRotator* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region IWICBitmapSource Methods
        [return: ComAliasName("HRESULT")]
        public int GetSize(
            [Out, ComAliasName("UINT")] uint* puiWidth,
            [Out, ComAliasName("UINT")] uint* puiHeight
        )
        {
            fixed (IWICBitmapFlipRotator* This = &this)
            {
                return MarshalFunction<_GetSize>(lpVtbl->GetSize)(
                    This,
                    puiWidth,
                    puiHeight
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetPixelFormat(
            [Out, ComAliasName("WICPixelFormatGUID")] Guid* pPixelFormat
        )
        {
            fixed (IWICBitmapFlipRotator* This = &this)
            {
                return MarshalFunction<_GetPixelFormat>(lpVtbl->GetPixelFormat)(
                    This,
                    pPixelFormat
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetResolution(
            [Out] double* pDpiX,
            [Out] double* pDpiY
        )
        {
            fixed (IWICBitmapFlipRotator* This = &this)
            {
                return MarshalFunction<_GetResolution>(lpVtbl->GetResolution)(
                    This,
                    pDpiX,
                    pDpiY
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyPalette(
            [In] IWICPalette* pIPalette = null
        )
        {
            fixed (IWICBitmapFlipRotator* This = &this)
            {
                return MarshalFunction<_CopyPalette>(lpVtbl->CopyPalette)(
                    This,
                    pIPalette
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyPixels(
            [In, Optional] WICRect* prc,
            [In, ComAliasName("UINT")] uint cbStride,
            [In, ComAliasName("UINT")] uint cbBufferSize,
            [Out, ComAliasName("BYTE[]")] byte* pbBuffer
        )
        {
            fixed (IWICBitmapFlipRotator* This = &this)
            {
                return MarshalFunction<_CopyPixels>(lpVtbl->CopyPixels)(
                    This,
                    prc,
                    cbStride,
                    cbBufferSize,
                    pbBuffer
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int Initialize(
            [In, Optional] IWICBitmapSource* pISource,
            [In] WICBitmapTransformOptions options
        )
        {
            fixed (IWICBitmapFlipRotator* This = &this)
            {
                return MarshalFunction<_Initialize>(lpVtbl->Initialize)(
                    This,
                    pISource,
                    options
                );
            }
        }
        #endregion

        #region Structs
        public /* unmanaged */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region IWICBitmapSource Fields
            public IntPtr GetSize;

            public IntPtr GetPixelFormat;

            public IntPtr GetResolution;

            public IntPtr CopyPalette;

            public IntPtr CopyPixels;
            #endregion

            #region Fields
            public IntPtr Initialize;
            #endregion
        }
        #endregion
    }
}

