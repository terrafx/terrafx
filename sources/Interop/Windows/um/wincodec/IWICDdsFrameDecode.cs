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
    [Guid("3D4C0C61-18A4-41E4-BD80-481A4FC9F464")]
    [Unmanaged]
    public unsafe struct IWICDdsFrameDecode
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICDdsFrameDecode* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICDdsFrameDecode* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICDdsFrameDecode* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSizeInBlocks(
            [In] IWICDdsFrameDecode* This,
            [Out, ComAliasName("UINT")] uint* pWidthInBlocks,
            [Out, ComAliasName("UINT")] uint* pHeightInBlocks
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFormatInfo(
            [In] IWICDdsFrameDecode* This,
            [Out] WICDdsFormatInfo* pFormatInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyBlocks(
            [In] IWICDdsFrameDecode* This,
            [In] WICRect* prcBoundsInBlocks,
            [In, ComAliasName("UINT")] uint cbStride,
            [In, ComAliasName("UINT")] uint cbBufferSize,
            [Out, ComAliasName("BYTE[]")] byte* pbBuffer
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICDdsFrameDecode* This = &this)
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
            fixed (IWICDdsFrameDecode* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICDdsFrameDecode* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int GetSizeInBlocks(
            [Out, ComAliasName("UINT")] uint* pWidthInBlocks,
            [Out, ComAliasName("UINT")] uint* pHeightInBlocks
        )
        {
            fixed (IWICDdsFrameDecode* This = &this)
            {
                return MarshalFunction<_GetSizeInBlocks>(lpVtbl->GetSizeInBlocks)(
                    This,
                    pWidthInBlocks,
                    pHeightInBlocks
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFormatInfo(
            [Out] WICDdsFormatInfo* pFormatInfo
        )
        {
            fixed (IWICDdsFrameDecode* This = &this)
            {
                return MarshalFunction<_GetFormatInfo>(lpVtbl->GetFormatInfo)(
                    This,
                    pFormatInfo
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyBlocks(
            [In] WICRect* prcBoundsInBlocks,
            [In, ComAliasName("UINT")] uint cbStride,
            [In, ComAliasName("UINT")] uint cbBufferSize,
            [Out, ComAliasName("BYTE[]")] byte* pbBuffer
        )
        {
            fixed (IWICDdsFrameDecode* This = &this)
            {
                return MarshalFunction<_CopyBlocks>(lpVtbl->CopyBlocks)(
                    This,
                    prcBoundsInBlocks,
                    cbStride,
                    cbBufferSize,
                    pbBuffer
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
            public IntPtr GetSizeInBlocks;

            public IntPtr GetFormatInfo;

            public IntPtr CopyBlocks;
            #endregion
        }
        #endregion
    }
}
