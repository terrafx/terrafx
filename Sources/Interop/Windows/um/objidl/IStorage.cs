// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\objidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("0000000B-0000-0000-C000-000000000046")]
    public /* blittable */ unsafe struct IStorage
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IStorage* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IStorage* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IStorage* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateStream(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] char* pwcsName,
            [In, ComAliasName("DWORD")] uint grfMode,
            [In, ComAliasName("DWORD")] uint reserved1,
            [In, ComAliasName("DWORD")] uint reserved2,
            [Out] IStream** ppstm = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int OpenStream(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] char* pwcsName,
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
            [In, ComAliasName("OLECHAR")] char* pwcsName,
            [In, ComAliasName("DWORD")] uint grfMode,
            [In, ComAliasName("DWORD")] uint reserved1,
            [In, ComAliasName("DWORD")] uint reserved2,
            [Out] IStorage** ppstg = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int OpenStorage(
            [In] IStorage* This,
            [In, Optional, ComAliasName("OLECHAR")] char* pwcsName,
            [In, Optional] IStorage* pstgPriority,
            [In, ComAliasName("DWORD")] uint grfMode,
            [In, Optional, ComAliasName("SNB")] char** snbExclude,
            [In, ComAliasName("DWORD")] uint reserved,
            [Out] IStorage** ppstg = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CopyTo(
            [In] IStorage* This,
            [In, ComAliasName("IID")] Guid* ciidExclude,
            [In, Optional] Guid* rgiidExclude,
            [In, Optional, ComAliasName("SNB")] char** snbExclude,
            [In] IStorage* pstgDest
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MoveElementTo(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] char* pwcsName,
            [In, Optional] IStorage* pstgDest,
            [In, ComAliasName("OLECHAR")] char* pwcsNewName,
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
            [In, ComAliasName("OLECHAR")] char* pwcsName
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RenameElement(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] char* pwcsOldName,
            [In, ComAliasName("OLECHAR")] char* pwcsNewName
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetElementTimes(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] char* pwcsName = null,
            [In] FILETIME* pctime = null,
            [In] FILETIME* patime = null,
            [In] FILETIME* pmtime = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetClass(
            [In] IStorage* This,
            [In, ComAliasName("REFCLSID")] Guid* clsid
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
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region Fields
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
