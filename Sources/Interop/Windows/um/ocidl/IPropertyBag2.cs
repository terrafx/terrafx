// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\ocidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("22F55882-280B-11d0-A8A9-00A0C90C2004")]
    unsafe public  /* blittable */ struct IPropertyBag2
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Read(
            [In] IPropertyBag2* This,
            [In, ComAliasName("ULONG")] uint cProperties,
            [In] PROPBAG2* pPropBag,
            [In, Optional] IErrorLog* pErrLog,
            [Out] VARIANT* pvarValue,
            [In, Out, Optional, ComAliasName("HRESULT")] int* phrError
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Write(
            [In] IPropertyBag2* This,
            [In, ComAliasName("ULONG")] uint cProperties,
            [In] PROPBAG2* pPropBag,
            [In] VARIANT* pvarValue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CountProperties(
            [In] IPropertyBag2* This,
            [Out, ComAliasName("ULONG")] uint* pcProperties
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPropertyInfo(
            [In] IPropertyBag2* This,
            [In, ComAliasName("ULONG")] uint iProperty,
            [In, ComAliasName("ULONG")] uint cProperties,
            [Out] PROPBAG2* pPropBag,
            [Out, ComAliasName("ULONG")] uint* pcProperties
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int LoadObject(
            [In] IPropertyBag2* This,
            [In, ComAliasName("LPCOLESTR")] /* readonly */ char* pstrName,
            [In, ComAliasName("DWORD")] uint dwHint,
            [In, Optional] IUnknown* pUnkObject,
            [In, Optional] IErrorLog* pErrLog
        );
        #endregion

        #region Structs
        public  /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr Read;

            public IntPtr Write;

            public IntPtr CountProperties;

            public IntPtr GetPropertyInfo;

            public IntPtr LoadObject;
            #endregion
        }
        #endregion
    }
}
