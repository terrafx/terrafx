// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("04C75BF8-3CE1-473B-ACC5-3CC4F5E94999")]
    public /* blittable */ unsafe struct IWICImageEncoder
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICImageEncoder* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICImageEncoder* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICImageEncoder* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _WriteFrame(
            [In] IWICImageEncoder* This,
            [In] ID2D1Image* pImage,
            [In] IWICBitmapFrameEncode* pFrameEncode,
            [In] WICImageParameters* pImageParameters
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _WriteFrameThumbnail(
            [In] IWICImageEncoder* This,
            [In] ID2D1Image* pImage,
            [In] IWICBitmapFrameEncode* pFrameEncode,
            [In] WICImageParameters* pImageParameters
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _WriteThumbnail(
            [In] IWICImageEncoder* This,
            [In] ID2D1Image* pImage,
            [In] IWICBitmapEncoder* pEncoder,
            [In] WICImageParameters* pImageParameters
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICImageEncoder* This = &this)
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
            fixed (IWICImageEncoder* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICImageEncoder* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int WriteFrame(
            [In] ID2D1Image* pImage,
            [In] IWICBitmapFrameEncode* pFrameEncode,
            [In] WICImageParameters* pImageParameters
        )
        {
            fixed (IWICImageEncoder* This = &this)
            {
                return MarshalFunction<_WriteFrame>(lpVtbl->WriteFrame)(
                    This,
                    pImage,
                    pFrameEncode,
                    pImageParameters
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int WriteFrameThumbnail(
            [In] ID2D1Image* pImage,
            [In] IWICBitmapFrameEncode* pFrameEncode,
            [In] WICImageParameters* pImageParameters
        )
        {
            fixed (IWICImageEncoder* This = &this)
            {
                return MarshalFunction<_WriteFrameThumbnail>(lpVtbl->WriteFrameThumbnail)(
                    This,
                    pImage,
                    pFrameEncode,
                    pImageParameters
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int WriteThumbnail(
            [In] ID2D1Image* pImage,
            [In] IWICBitmapEncoder* pEncoder,
            [In] WICImageParameters* pImageParameters
        )
        {
            fixed (IWICImageEncoder* This = &this)
            {
                return MarshalFunction<_WriteThumbnail>(lpVtbl->WriteThumbnail)(
                    This,
                    pImage,
                    pEncoder,
                    pImageParameters
                );
            }
        }
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region Fields
            public IntPtr WriteFrame;

            public IntPtr WriteFrameThumbnail;

            public IntPtr WriteThumbnail;
            #endregion
        }
        #endregion
    }
}

