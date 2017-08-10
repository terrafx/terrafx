// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\objidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("0000000B-0000-0000-C000-000000000046")]
    unsafe public /* blittable */ struct IStorage
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateStream(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] /* readonly */ char* pwcsName,
            [In, ComAliasName("DWORD")] uint grfMode,
            [In, ComAliasName("DWORD")] uint reserved1,
            [In, ComAliasName("DWORD")] uint reserved2,
            [Out, Optional] IStream** ppstm
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int OpenStream(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] /* readonly */ char* pwcsName,
            [In] void* reserved1,
            [In, ComAliasName("DWORD")] uint grfMode,
            [In, ComAliasName("DWORD")] uint reserved2,
            [Out] IStream** ppstm
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateStorage(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] /* readonly */ char* pwcsName,
            [In, ComAliasName("DWORD")] uint grfMode,
            [In, ComAliasName("DWORD")] uint reserved1,
            [In, ComAliasName("DWORD")] uint reserved2,
            [Out, Optional] IStorage** ppstg
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int OpenStorage(
            [In] IStorage* This,
            [In, Optional, ComAliasName("OLECHAR")] /* readonly */ char* pwcsName,
            [In, Optional] IStorage* pstgPriority,
            [In, ComAliasName("DWORD")] uint grfMode,
            [In, Optional, ComAliasName("SNB")] char** snbExclude,
            [In, ComAliasName("DWORD")] uint reserved,
            [Out, Optional] IStorage** ppstg
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CopyTo(
            [In] IStorage* This,
            [In, ComAliasName("IID")] /* readonly */ Guid* ciidExclude,
            [In, Optional] Guid* rgiidExclude,
            [In, Optional, ComAliasName("SNB")] char** snbExclude,
            [In] IStorage* pstgDest
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MoveElementTo(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] /* readonly */ char* pwcsName,
            [In, Optional] IStorage* pstgDest,
            [In, ComAliasName("OLECHAR")] /* readonly */ char* pwcsNewName,
            [In, ComAliasName("DWORD")] uint grfFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Commit(
            [In] IStorage* This,
            [In, ComAliasName("DWORD")] uint grfCommitFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Revert(
            [In] IStorage* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int EnumElements(
            [In] IStorage* This,
            [In, ComAliasName("DWORD")] uint reserved1,
            [In] void* reserved2,
            [In, ComAliasName("DWORD")] uint reserved3,
            [Out] IEnumSTATSTG** ppenum
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DestroyElement(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] /* readonly */ char* pwcsName
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RenameElement(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] /* readonly */ char* pwcsOldName,
            [In, ComAliasName("OLECHAR")] /* readonly */ char* pwcsNewName
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetElementTimes(
            [In] IStorage* This,
            [In, Optional, ComAliasName("OLECHAR")] /* readonly */ char* pwcsName,
            [In, Optional] /* readonly */ FILETIME* pctime,
            [In, Optional] /* readonly */ FILETIME* patime,
            [In, Optional] /* readonly */ FILETIME* pmtime
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetClass(
            [In] IStorage* This,
            [In, ComAliasName("REFCLSID")] /* readonly */ Guid* clsid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetStateBits(
            [In] IStorage* This,
            [In, ComAliasName("DWORD")] uint grfStateBits,
            [In, ComAliasName("DWORD")] uint grfMask
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Stat(
            [In] IStorage* This,
            [Out] STATSTG* pstatstg,
            [In, ComAliasName("DWORD")] uint grfStatFlag
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr CreateStream;

            public IntPtr OpenStream;

            public IntPtr CreateStorage;

            public IntPtr OpenStorage;

            public IntPtr CopyTo;

            public IntPtr MoveElementTo;

            public IntPtr Commit;

            public IntPtr Revert;

            public IntPtr EnumElements;

            public IntPtr DestroyElement;

            public IntPtr RenameElement;

            public IntPtr SetElementTimes;

            public IntPtr SetClass;

            public IntPtr SetStateBits;

            public IntPtr Stat;
            #endregion
        }
        #endregion
    }
}
