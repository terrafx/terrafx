// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodecsdk.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("F7836E16-3BE0-470B-86BB-160D0AECD7DE")]
    unsafe public /* blittable */ struct IWICMetadataWriter
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetValue(
            [In] IWICMetadataWriter* This,
            [In, Optional] /* readonly */ PROPVARIANT* pvarSchema,
            [In] /* readonly */ PROPVARIANT* pvarId,
            [In] /* readonly */ PROPVARIANT* pvarValue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetValueByIndex(
            [In] IWICMetadataWriter* This,
            [In] UINT nIndex,
            [In, Optional] /* readonly */ PROPVARIANT* pvarSchema,
            [In] /* readonly */ PROPVARIANT* pvarId,
            [In] /* readonly */ PROPVARIANT* pvarValue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RemoveValue(
            [In] IWICMetadataWriter* This,
            [In, Optional] /* readonly */ PROPVARIANT* pvarSchema,
            [In] /* readonly */ PROPVARIANT* pvarId
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RemoveValueByIndex(
            [In] IWICMetadataWriter* This,
            [In] UINT nIndex
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICMetadataReader.Vtbl BaseVtbl;

            public SetValue SetValue;

            public SetValueByIndex SetValueByIndex;

            public RemoveValue RemoveValue;

            public RemoveValueByIndex RemoveValueByIndex;
            #endregion
        }
        #endregion
    }
}
