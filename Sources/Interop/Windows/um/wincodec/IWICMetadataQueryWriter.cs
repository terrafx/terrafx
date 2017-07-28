// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("A721791A-0DEF-4D06-BD91-2118BF1DB10B")]
    unsafe public /* blittable */ struct IWICMetadataQueryWriter
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetMetadataByName(
            [In] IWICMetadataQueryWriter* This,
            [In, ComAliasName("LPCWSTR")] /* readonly */ char* wzName,
            [In] /* readonly */ PROPVARIANT* pvarValue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RemoveMetadataByName(
            [In] IWICMetadataQueryWriter* This,
            [In, ComAliasName("LPCWSTR")] /* readonly */ char* wzName
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICMetadataQueryReader.Vtbl BaseVtbl;

            public SetMetadataByName SetMetadataByName;

            public RemoveMetadataByName RemoveMetadataByName;
            #endregion
        }
        #endregion
    }
}
