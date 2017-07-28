// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\documenttarget.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("1B8EFEC4-3019-4C27-964E-367202156906")]
    unsafe public /* blittable */ struct IPrintDocumentPackageTarget
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPackageTargetTypes(
            [In] IPrintDocumentPackageTarget* This,
            [Out, ComAliasName("UINT32")] uint* targetCount,
            [Out, Optional, ComAliasName("GUID")] Guid **targetTypes
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPackageTarget(
            [In] IPrintDocumentPackageTarget* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* guidTargetType,
            [In, ComAliasName("REFIID")] /* readonly */ Guid* riid,
            [Out, Optional] void** ppvTarget
        );
        
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Cancel(
            [In] IPrintDocumentPackageTarget* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetPackageTargetTypes GetPackageTargetTypes;

            public GetPackageTarget GetPackageTarget;

            public Cancel Cancel;
            #endregion
        }
        #endregion
    }
}
