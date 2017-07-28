// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\objidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("00000109-0000-0000-C000-000000000046")]
    unsafe public  /* blittable */ struct IPersistStream
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int IsDirty(
            [In] IPersistStream* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Load(
            [In] IPersistStream* This,
            [In, Optional] IStream* pStm
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Save(
            [In] IPersistStream* This,
            [In, Optional] IStream* pStm,
            [In, ComAliasName("BOOL")] int fClearDirty
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSizeMax(
            [In] IPersistStream* This,
            [Out] ULARGE_INTEGER* pcbSize
        );
        #endregion

        #region Structs
        public  /* blittable */ struct Vtbl
        {
            #region Fields
            public IPersist.Vtbl BaseVtbl;

            public IsDirty IsDirty;

            public Load Load;

            public Save Save;

            public GetSizeMax GetSizeMax;
            #endregion
        }
        #endregion
    }
}
