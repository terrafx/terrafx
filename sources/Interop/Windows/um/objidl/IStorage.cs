// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\objidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("0000000B-0000-0000-C000-000000000046")]
    [Unmanaged]
    public unsafe struct IStorage
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IStorage* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IStorage* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IStorage* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateStream(
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
        public /* static */ delegate int _OpenStream(
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
        public /* static */ delegate int _CreateStorage(
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
        public /* static */ delegate int _OpenStorage(
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
        public /* static */ delegate int _CopyTo(
            [In] IStorage* This,
            [In, ComAliasName("IID")] Guid* ciidExclude,
            [In, Optional] Guid* rgiidExclude,
            [In, Optional, ComAliasName("SNB")] char** snbExclude,
            [In] IStorage* pstgDest
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _MoveElementTo(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] char* pwcsName,
            [In, Optional] IStorage* pstgDest,
            [In, ComAliasName("OLECHAR")] char* pwcsNewName,
            [In, ComAliasName("DWORD")] uint grfFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Commit(
            [In] IStorage* This,
            [In, ComAliasName("DWORD")] uint grfCommitFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Revert(
            [In] IStorage* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _EnumElements(
            [In] IStorage* This,
            [In, ComAliasName("DWORD")] uint reserved1,
            [In] void* reserved2,
            [In, ComAliasName("DWORD")] uint reserved3,
            [Out] IEnumSTATSTG** ppenum
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _DestroyElement(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] char* pwcsName
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RenameElement(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] char* pwcsOldName,
            [In, ComAliasName("OLECHAR")] char* pwcsNewName
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetElementTimes(
            [In] IStorage* This,
            [In, ComAliasName("OLECHAR")] char* pwcsName = null,
            [In] FILETIME* pctime = null,
            [In] FILETIME* patime = null,
            [In] FILETIME* pmtime = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetClass(
            [In] IStorage* This,
            [In, ComAliasName("REFCLSID")] Guid* clsid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetStateBits(
            [In] IStorage* This,
            [In, ComAliasName("DWORD")] uint grfStateBits,
            [In, ComAliasName("DWORD")] uint grfMask
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Stat(
            [In] IStorage* This,
            [Out] STATSTG* pstatstg,
            [In, ComAliasName("DWORD")] uint grfStatFlag
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_QueryInterface>(lpVtbl->QueryInterface)(
                    This,
                    riid,
                    ppvObject
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint AddRef()
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int CreateStream(
            [In, ComAliasName("OLECHAR")] char* pwcsName,
            [In, ComAliasName("DWORD")] uint grfMode,
            [In, ComAliasName("DWORD")] uint reserved1,
            [In, ComAliasName("DWORD")] uint reserved2,
            [Out] IStream** ppstm = null
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_CreateStream>(lpVtbl->CreateStream)(
                    This,
                    pwcsName,
                    grfMode,
                    reserved1,
                    reserved2,
                    ppstm
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int OpenStream(
            [In, ComAliasName("OLECHAR")] char* pwcsName,
            [In] void* reserved1,
            [In, ComAliasName("DWORD")] uint grfMode,
            [In, ComAliasName("DWORD")] uint reserved2,
            [Out] IStream** ppstm
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_OpenStream>(lpVtbl->OpenStream)(
                    This,
                    pwcsName,
                    reserved1,
                    grfMode,
                    reserved2,
                    ppstm
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateStorage(
            [In, ComAliasName("OLECHAR")] char* pwcsName,
            [In, ComAliasName("DWORD")] uint grfMode,
            [In, ComAliasName("DWORD")] uint reserved1,
            [In, ComAliasName("DWORD")] uint reserved2,
            [Out] IStorage** ppstg = null
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_CreateStorage>(lpVtbl->CreateStorage)(
                    This,
                    pwcsName,
                    grfMode,
                    reserved1,
                    reserved2,
                    ppstg
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int OpenStorage(
            [In, Optional, ComAliasName("OLECHAR")] char* pwcsName,
            [In, Optional] IStorage* pstgPriority,
            [In, ComAliasName("DWORD")] uint grfMode,
            [In, Optional, ComAliasName("SNB")] char** snbExclude,
            [In, ComAliasName("DWORD")] uint reserved,
            [Out] IStorage** ppstg = null
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_OpenStorage>(lpVtbl->OpenStorage)(
                    This,
                    pwcsName,
                    pstgPriority,
                    grfMode,
                    snbExclude,
                    reserved,
                    ppstg
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyTo(
            [In, ComAliasName("IID")] Guid* ciidExclude,
            [In, Optional] Guid* rgiidExclude,
            [In, Optional, ComAliasName("SNB")] char** snbExclude,
            [In] IStorage* pstgDest
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_CopyTo>(lpVtbl->CopyTo)(
                    This,
                    ciidExclude,
                    rgiidExclude,
                    snbExclude,
                    pstgDest
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int MoveElementTo(
            [In, ComAliasName("OLECHAR")] char* pwcsName,
            [In, Optional] IStorage* pstgDest,
            [In, ComAliasName("OLECHAR")] char* pwcsNewName,
            [In, ComAliasName("DWORD")] uint grfFlags
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_MoveElementTo>(lpVtbl->MoveElementTo)(
                    This,
                    pwcsName,
                    pstgDest,
                    pwcsNewName,
                    grfFlags
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Commit(
            [In, ComAliasName("DWORD")] uint grfCommitFlags
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_Commit>(lpVtbl->Commit)(
                    This,
                    grfCommitFlags
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Revert()
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_Revert>(lpVtbl->Revert)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int EnumElements(
            [In, ComAliasName("DWORD")] uint reserved1,
            [In] void* reserved2,
            [In, ComAliasName("DWORD")] uint reserved3,
            [Out] IEnumSTATSTG** ppenum
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_EnumElements>(lpVtbl->EnumElements)(
                    This,
                    reserved1,
                    reserved2,
                    reserved3,
                    ppenum
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int DestroyElement(
            [In, ComAliasName("OLECHAR")] char* pwcsName
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_DestroyElement>(lpVtbl->DestroyElement)(
                    This,
                    pwcsName
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int RenameElement(
            [In, ComAliasName("OLECHAR")] char* pwcsOldName,
            [In, ComAliasName("OLECHAR")] char* pwcsNewName
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_RenameElement>(lpVtbl->RenameElement)(
                    This,
                    pwcsOldName,
                    pwcsNewName
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetElementTimes(
            [In, ComAliasName("OLECHAR")] char* pwcsName = null,
            [In] FILETIME* pctime = null,
            [In] FILETIME* patime = null,
            [In] FILETIME* pmtime = null
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_SetElementTimes>(lpVtbl->SetElementTimes)(
                    This,
                    pwcsName,
                    pctime,
                    patime,
                    pmtime
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetClass(
            [In, ComAliasName("REFCLSID")] Guid* clsid
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_SetClass>(lpVtbl->SetClass)(
                    This,
                    clsid
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetStateBits(
            [In, ComAliasName("DWORD")] uint grfStateBits,
            [In, ComAliasName("DWORD")] uint grfMask
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_SetStateBits>(lpVtbl->SetStateBits)(
                    This,
                    grfStateBits,
                    grfMask
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Stat(
            [Out] STATSTG* pstatstg,
            [In, ComAliasName("DWORD")] uint grfStatFlag
        )
        {
            fixed (IStorage* This = &this)
            {
                return MarshalFunction<_Stat>(lpVtbl->Stat)(
                    This,
                    pstatstg,
                    grfStatFlag
                );
            }
        }
        #endregion

        #region Structs
        [Unmanaged]
        public struct Vtbl
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

