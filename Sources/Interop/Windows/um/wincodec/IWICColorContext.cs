// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("3C613A02-34B2-44EA-9A7C-45AEA9C6FD6D")]
    unsafe public /* blittable */ struct IWICColorContext
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int InitializeFromFilename(
            [In] IWICColorContext* This,
            [In, ComAliasName("LPCWSTR")] /* readonly */ char* wzFilename
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int InitializeFromMemory(
            [In] IWICColorContext* This,
            [In, ComAliasName("BYTE")] /* readonly */ byte* pbBuffer,
            [In, ComAliasName("UINT")] uint cbBufferSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int InitializeFromExifColorSpace(
            [In] IWICColorContext* This,
            [In, ComAliasName("UINT")] uint value
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetType(
            [In] IWICColorContext* This,
            [Out] WICColorContextType* pType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetProfileBytes(
            [In] IWICColorContext* This,
            [In, ComAliasName("UINT")] uint cbBuffer,
            [In, Out, Optional, ComAliasName("BYTE")] byte* pbBuffer,
            [Out, ComAliasName("UINT")] uint* pcbActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetExifColorSpace(
            [In] IWICColorContext* This,
            [Out, ComAliasName("UINT")] uint* pValue
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

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
