// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

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
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPatterns(
            [In] IWICMetadataReaderInfo* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* guidContainerFormat,
            [In, ComAliasName("UINT")] uint cbSize,
            [Out] WICMetadataPattern* pPattern = null,
            [Out, ComAliasName("UINT")] uint* pcCount = null,
            [Out, ComAliasName("UINT")] uint* pcbActual = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MatchesPattern(
            [In] IWICMetadataReaderInfo* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* guidContainerFormat,
            [In, Optional] IStream* pIStream,
            [Out, ComAliasName("BOOL")] int* pfMatches
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateInstance(
            [In] IWICMetadataReaderInfo* This,
            [Out] IWICMetadataReader** ppIReader = null
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICMetadataHandlerInfo.Vtbl BaseVtbl;

            public IntPtr GetPatterns;

            public IntPtr MatchesPattern;

            public IntPtr CreateInstance;
            #endregion
        }
        #endregion
    }
}
