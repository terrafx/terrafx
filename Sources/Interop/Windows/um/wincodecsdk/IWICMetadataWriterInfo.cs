// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodecsdk.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("B22E3FBA-3925-4323-B5C1-9EBFC430F236")]
    unsafe public /* blittable */ struct IWICMetadataWriterInfo
    {

        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetHeader(
            [In] IWICMetadataWriterInfo* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* guidContainerFormat,
            [In, ComAliasName("UINT")] uint cbSize,
            [Out, Optional] WICMetadataHeader* pHeader,
            [Out, Optional, ComAliasName("UINT")] uint* pcbActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateInstance(
            [In] IWICMetadataWriterInfo* This,
            [Out, Optional] IWICMetadataWriter** ppIWriter
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICMetadataHandlerInfo.Vtbl BaseVtbl;

            public GetHeader GetHeader;

            public CreateInstance CreateInstance;
            #endregion
        }
        #endregion
    }
}
