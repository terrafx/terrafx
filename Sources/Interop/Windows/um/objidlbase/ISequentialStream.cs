// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\objidlbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("0C733A30-2A1C-11CE-ADE5-00AA0044773D")]
    unsafe public  /* blittable */ struct ISequentialStream
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Read(
            [In] ISequentialStream* This,
            [Out] void* pv,
            [In, ComAliasName("ULONG")] uint cb,
            [Out, Optional, ComAliasName("ULONG")] uint* pcbRead
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Write(
            [In] ISequentialStream* This,
            [In] /* readonly */ void* pv,
            [In, ComAliasName("ULONG")] uint cb,
            [Out, Optional, ComAliasName("ULONG")] uint* pcbWritten
        );
        #endregion

        #region Structs
        public  /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr Read;

            public IntPtr Write;
            #endregion
        }
        #endregion
    }
}
