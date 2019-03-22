// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("3B16811B-6A43-4EC9-B713-3D5A0C13B940")]
    [Unmanaged]
    public unsafe struct IWICBitmapSourceTransform
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICBitmapSourceTransform* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICBitmapSourceTransform* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICBitmapSourceTransform* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyPixels(
            [In] IWICBitmapSourceTransform* This,
            [In, Optional] WICRect* prc,
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In, Optional, ComAliasName("WICPixelFormatGUID")] Guid* pguidDstFormat,
            [In] WICBitmapTransformOptions dstTransform,
            [In, ComAliasName("UINT")] uint nStride,
            [In, ComAliasName("UINT")] uint cbBufferSize,
            [Out, ComAliasName("BYTE[]")] byte* pbBuffer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetClosestSize(
            [In] IWICBitmapSourceTransform* This,
            [In, Out, ComAliasName("UINT")] uint* puiWidth,
            [In, Out, ComAliasName("UINT")] uint* puiHeight
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetClosestPixelFormat(
            [In] IWICBitmapSourceTransform* This,
            [In, Out, ComAliasName("WICPixelFormatGUID")] Guid* pguidDstFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _DoesSupportTransform(
            [In] IWICBitmapSourceTransform* This,
            [In] WICBitmapTransformOptions dstTransform,
            [Out, ComAliasName("BOOL")] int* pfIsSupported
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICBitmapSourceTransform* This = &this)
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
            fixed (IWICBitmapSourceTransform* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICBitmapSourceTransform* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int CopyPixels(
            [In, Optional] WICRect* prc,
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In, Optional, ComAliasName("WICPixelFormatGUID")] Guid* pguidDstFormat,
            [In] WICBitmapTransformOptions dstTransform,
            [In, ComAliasName("UINT")] uint nStride,
            [In, ComAliasName("UINT")] uint cbBufferSize,
            [Out, ComAliasName("BYTE[]")] byte* pbBuffer
        )
        {
            fixed (IWICBitmapSourceTransform* This = &this)
            {
                return MarshalFunction<_CopyPixels>(lpVtbl->CopyPixels)(
                    This,
                    prc,
                    uiWidth,
                    uiHeight,
                    pguidDstFormat,
                    dstTransform,
                    nStride,
                    cbBufferSize,
                    pbBuffer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetClosestSize(
            [In, Out, ComAliasName("UINT")] uint* puiWidth,
            [In, Out, ComAliasName("UINT")] uint* puiHeight
        )
        {
            fixed (IWICBitmapSourceTransform* This = &this)
            {
                return MarshalFunction<_GetClosestSize>(lpVtbl->GetClosestSize)(
                    This,
                    puiWidth,
                    puiHeight
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetClosestPixelFormat(
            [In, Out, ComAliasName("WICPixelFormatGUID")] Guid* pguidDstFormat
        )
        {
            fixed (IWICBitmapSourceTransform* This = &this)
            {
                return MarshalFunction<_GetClosestPixelFormat>(lpVtbl->GetClosestPixelFormat)(
                    This,
                    pguidDstFormat
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int DoesSupportTransform(
            [In] WICBitmapTransformOptions dstTransform,
            [Out, ComAliasName("BOOL")] int* pfIsSupported
        )
        {
            fixed (IWICBitmapSourceTransform* This = &this)
            {
                return MarshalFunction<_DoesSupportTransform>(lpVtbl->DoesSupportTransform)(
                    This,
                    dstTransform,
                    pfIsSupported
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
            public IntPtr CopyPixels;

            public IntPtr GetClosestSize;

            public IntPtr GetClosestPixelFormat;

            public IntPtr DoesSupportTransform;
            #endregion
        }
        #endregion
    }
}
