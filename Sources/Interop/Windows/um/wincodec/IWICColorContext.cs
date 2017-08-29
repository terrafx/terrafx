// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("3C613A02-34B2-44EA-9A7C-45AEA9C6FD6D")]
    public /* blittable */ unsafe struct IWICColorContext
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICColorContext* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICColorContext* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICColorContext* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _InitializeFromFilename(
            [In] IWICColorContext* This,
            [In, ComAliasName("LPCWSTR")] char* wzFilename
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _InitializeFromMemory(
            [In] IWICColorContext* This,
            [In, ComAliasName("BYTE[]")] byte* pbBuffer,
            [In, ComAliasName("UINT")] uint cbBufferSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _InitializeFromExifColorSpace(
            [In] IWICColorContext* This,
            [In, ComAliasName("UINT")] uint value
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int __GetType(
            [In] IWICColorContext* This,
            [Out] WICColorContextType* pType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetProfileBytes(
            [In] IWICColorContext* This,
            [In, ComAliasName("UINT")] uint cbBuffer,
            [In, Out, Optional, ComAliasName("BYTE[]")] byte* pbBuffer,
            [Out, ComAliasName("UINT")] uint* pcbActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetExifColorSpace(
            [In] IWICColorContext* This,
            [Out, ComAliasName("UINT")] uint* pValue
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICColorContext* This = &this)
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
            fixed (IWICColorContext* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICColorContext* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int InitializeFromFilename(
            [In, ComAliasName("LPCWSTR")] char* wzFilename
        )
        {
            fixed (IWICColorContext* This = &this)
            {
                return MarshalFunction<_InitializeFromFilename>(lpVtbl->InitializeFromFilename)(
                    This,
                    wzFilename
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int InitializeFromMemory(
            [In, ComAliasName("BYTE[]")] byte* pbBuffer,
            [In, ComAliasName("UINT")] uint cbBufferSize
        )
        {
            fixed (IWICColorContext* This = &this)
            {
                return MarshalFunction<_InitializeFromMemory>(lpVtbl->InitializeFromMemory)(
                    This,
                    pbBuffer,
                    cbBufferSize
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int InitializeFromExifColorSpace(
            [In, ComAliasName("UINT")] uint value
        )
        {
            fixed (IWICColorContext* This = &this)
            {
                return MarshalFunction<_InitializeFromExifColorSpace>(lpVtbl->InitializeFromExifColorSpace)(
                    This,
                    value
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int _GetType(
            [Out] WICColorContextType* pType
        )
        {
            fixed (IWICColorContext* This = &this)
            {
                return MarshalFunction<__GetType>(lpVtbl->_GetType)(
                    This,
                    pType
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetProfileBytes(
            [In, ComAliasName("UINT")] uint cbBuffer,
            [In, Out, Optional, ComAliasName("BYTE[]")] byte* pbBuffer,
            [Out, ComAliasName("UINT")] uint* pcbActual
        )
        {
            fixed (IWICColorContext* This = &this)
            {
                return MarshalFunction<_GetProfileBytes>(lpVtbl->GetProfileBytes)(
                    This,
                    cbBuffer,
                    pbBuffer,
                    pcbActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetExifColorSpace(
            [Out, ComAliasName("UINT")] uint* pValue
        )
        {
            fixed (IWICColorContext* This = &this)
            {
                return MarshalFunction<_GetExifColorSpace>(lpVtbl->GetExifColorSpace)(
                    This,
                    pValue
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
            public IntPtr InitializeFromFilename;

            public IntPtr InitializeFromMemory;

            public IntPtr InitializeFromExifColorSpace;

            public IntPtr _GetType;

            public IntPtr GetProfileBytes;

            public IntPtr GetExifColorSpace;
            #endregion
        }
        #endregion
    }
}

