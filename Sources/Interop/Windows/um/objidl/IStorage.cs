// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\objidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("0000000B-0000-0000-C000-000000000046")]
    unsafe public  /* blittable */ struct IStorage
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateStream(
            [In] IStorage* This,
            [In] /* readonly */ OLECHAR* pwcsName,
            [In] DWORD grfMode,
            [In] DWORD reserved1,
            [In] DWORD reserved2,
            [Out, Optional] IStream** ppstm
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT OpenStream(
            [In] IStorage* This,
            [In] /* readonly */ OLECHAR* pwcsName,
            [In] void* reserved1,
            [In] DWORD grfMode,
            [In] DWORD reserved2,
            [Out] IStream** ppstm
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateStorage(
            [In] IStorage* This,
            [In] /* readonly */ OLECHAR* pwcsName,
            [In] DWORD grfMode,
            [In] DWORD reserved1,
            [In] DWORD reserved2,
            [Out, Optional] IStorage** ppstg
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT OpenStorage(
            [In] IStorage* This,
            [In, Optional] /* readonly */ OLECHAR* pwcsName,
            [In, Optional] IStorage* pstgPriority,
            [In] DWORD grfMode,
            [In, Optional] SNB snbExclude,
            [In] DWORD reserved,
            [Out, Optional] IStorage** ppstg
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CopyTo(
            [In] IStorage* This,
            [In] /* readonly */ IID* ciidExclude,
            [In, Optional] Guid* rgiidExclude,
            [In, Optional] SNB snbExclude,
            [In] IStorage* pstgDest
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT MoveElementTo(
            [In] IStorage* This,
            [In] /* readonly */ OLECHAR* pwcsName,
            [In, Optional] IStorage* pstgDest,
            [In] /* readonly */ OLECHAR* pwcsNewName,
            [In] DWORD grfFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Commit(
            [In] IStorage* This,
            [In] DWORD grfCommitFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Revert(
            [In] IStorage* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT EnumElements(
            [In] IStorage* This,
            [In] DWORD reserved1,
            [In] void* reserved2,
            [In] DWORD reserved3,
            [Out] IEnumSTATSTG** ppenum
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DestroyElement(
            [In] IStorage* This,
            [In] /* readonly */ OLECHAR* pwcsName
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RenameElement(
            [In] IStorage* This,
            [In] /* readonly */ OLECHAR* pwcsOldName,
            [In] /* readonly */ OLECHAR* pwcsNewName
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetElementTimes(
            [In] IStorage* This,
            [In, Optional] /* readonly */ OLECHAR* pwcsName,
            [In, Optional] /* readonly */ FILETIME* pctime,
            [In, Optional] /* readonly */ FILETIME* patime,
            [In, Optional] /* readonly */ FILETIME* pmtime
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetClass(
            [In] IStorage* This,
            [In] REFCLSID clsid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetStateBits(
            [In] IStorage* This,
            [In] DWORD grfStateBits,
            [In] DWORD grfMask
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Stat(
            [In] IStorage* This,
            [Out] STATSTG* pstatstg,
            [In] DWORD grfStatFlag
        );
        #endregion

        #region Structs
        public  /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public CreateStream CreateStream;

            public OpenStream OpenStream;

            public CreateStorage CreateStorage;

            public OpenStorage OpenStorage;

            public CopyTo CopyTo;

            public MoveElementTo MoveElementTo;

            public Commit Commit;

            public Revert Revert;

            public EnumElements EnumElements;

            public DestroyElement DestroyElement;

            public RenameElement RenameElement;

            public SetElementTimes SetElementTimes;

            public SetClass SetClass;

            public SetStateBits SetStateBits;

            public Stat Stat;
            #endregion
        }
        #endregion
    }
}
