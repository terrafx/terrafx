// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>User-implementable interface for introspecting on a metafile.</summary>
    [Guid("FD0ECB6B-91E6-411E-8655-395E760F91B4")]
    public /* blittable */ unsafe struct ID2D1GdiMetafileSink1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Callback for examining a metafile record.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ProcessRecord(
            [In] ID2D1GdiMetafileSink1* This,
            [In, ComAliasName("DWORD")] uint recordType,
            [In, Optional] void* recordData,
            [In, ComAliasName("DWORD")] uint recordDataSize,
            [In, ComAliasName("UINT32")] uint flags
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1GdiMetafileSink.Vtbl BaseVtbl;

            public IntPtr ProcessRecord;
            #endregion
        }
        #endregion
    }
}
