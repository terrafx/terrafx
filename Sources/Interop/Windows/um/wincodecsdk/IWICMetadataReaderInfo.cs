// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodecsdk.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("EEBF1F5B-07C1-4447-A3AB-22ACAF78A804")]
    unsafe public /* blittable */ struct IWICMetadataReaderInfo
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetPatterns(
            [In] IWICMetadataReaderInfo* This,
            [In] REFGUID guidContainerFormat,
            [In] UINT cbSize,
            [Out, Optional] WICMetadataPattern* pPattern,
            [Out, Optional] UINT* pcCount,
            [Out, Optional] UINT* pcbActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT MatchesPattern(
            [In] IWICMetadataReaderInfo* This,
            [In] REFGUID guidContainerFormat,
            [In, Optional] IStream* pIStream,
            [Out] BOOL* pfMatches
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateInstance(
            [In] IWICMetadataReaderInfo* This,
            [Out, Optional] IWICMetadataReader** ppIReader
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICMetadataHandlerInfo.Vtbl BaseVtbl;

            public GetPatterns GetPatterns;

            public MatchesPattern MatchesPattern;

            public CreateInstance CreateInstance;
            #endregion
        }
        #endregion
    }
}
