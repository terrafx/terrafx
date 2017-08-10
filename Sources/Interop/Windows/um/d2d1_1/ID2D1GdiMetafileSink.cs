// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>User-implementable interface for introspecting on a metafile.</summary>
    [Guid("82237326-8111-4F7C-BCF4-B5C1175564FE")]
    unsafe public /* blittable */ struct ID2D1GdiMetafileSink
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Callback for examining a metafile record.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ProcessRecord(
            [In] ID2D1GdiMetafileSink* This,
            [In, ComAliasName("DWORD")] uint recordType,
            [In, Optional] /* readonly */ void* recordData,
            [In, ComAliasName("DWORD")] uint recordDataSize
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr ProcessRecord;
            #endregion
        }
        #endregion
    }
}
