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
    [Guid("F928B7B8-2221-40C1-B72E-7E82F1974D1A")]
    [Unmanaged]
    public unsafe struct IWICPlanarBitmapFrameEncode
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICPlanarBitmapFrameEncode* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICPlanarBitmapFrameEncode* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICPlanarBitmapFrameEncode* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _WritePixels(
            [In] IWICPlanarBitmapFrameEncode* This,
            [In, ComAliasName("UINT")] uint lineCount,
            [In, ComAliasName("WICBitmapPlane[]")] WICBitmapPlane* pPlanes,
            [In, ComAliasName("UINT")] uint cPlanes
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _WriteSource(
            [In] IWICPlanarBitmapFrameEncode* This,
            [In, ComAliasName("IWICBitmapSource*[]")] IWICBitmapSource** ppPlanes,
            [In, ComAliasName("UINT")] uint cPlanes,
            [In] WICRect* prcSource = null
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICPlanarBitmapFrameEncode* This = &this)
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
            fixed (IWICPlanarBitmapFrameEncode* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICPlanarBitmapFrameEncode* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int WritePixels(
            [In, ComAliasName("UINT")] uint lineCount,
            [In, ComAliasName("WICBitmapPlane[]")] WICBitmapPlane* pPlanes,
            [In, ComAliasName("UINT")] uint cPlanes
        )
        {
            fixed (IWICPlanarBitmapFrameEncode* This = &this)
            {
                return MarshalFunction<_WritePixels>(lpVtbl->WritePixels)(
                    This,
                    lineCount,
                    pPlanes,
                    cPlanes
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int WriteSource(
            [In, ComAliasName("IWICBitmapSource*[]")] IWICBitmapSource** ppPlanes,
            [In, ComAliasName("UINT")] uint cPlanes,
            [In] WICRect* prcSource = null
        )
        {
            fixed (IWICPlanarBitmapFrameEncode* This = &this)
            {
                return MarshalFunction<_WriteSource>(lpVtbl->WriteSource)(
                    This,
                    ppPlanes,
                    cPlanes,
                    prcSource
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
            public IntPtr WritePixels;

            public IntPtr WriteSource;
            #endregion
        }
        #endregion
    }
}

