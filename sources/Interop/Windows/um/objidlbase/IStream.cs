// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\objidlbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("0000000C-0000-0000-C000-000000000046")]
    public /* unmanaged */ unsafe struct IStream
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IStream* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IStream* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IStream* This
        );
        #endregion

        #region ISequentialStream Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Read(
            [In] IStream* This,
            [Out] void* pv,
            [In, ComAliasName("ULONG")] uint cb,
            [Out, ComAliasName("ULONG")] uint* pcbRead = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Write(
            [In] IStream* This,
            [In] void* pv,
            [In, ComAliasName("ULONG")] uint cb,
            [Out, ComAliasName("ULONG")] uint* pcbWritten = null
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Seek(
            [In] IStream* This,
            [In] LARGE_INTEGER dlibMove,
            [In, ComAliasName("DWORD")] uint dwOrigin,
            [Out] ULARGE_INTEGER* plibNewPosition = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetSize(
            [In] IStream* This,
            [In] ULARGE_INTEGER libNewSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyTo(
            [In] IStream* This,
            [In] IStream* pstm,
            [In] ULARGE_INTEGER cb,
            [Out] ULARGE_INTEGER* pcbRead = null,
            [Out] ULARGE_INTEGER* pcbWritten = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Commit(
            [In] IStream* This,
            [In, ComAliasName("DWORD")] uint grfCommitFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Revert(
            [In] IStream* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _LockRegion(
            [In] IStream* This,
            [In] ULARGE_INTEGER libOffset,
            [In] ULARGE_INTEGER cb,
            [In, ComAliasName("DWORD")] uint dwLockType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _UnlockRegion(
            [In] IStream* This,
            [In] ULARGE_INTEGER libOffset,
            [In] ULARGE_INTEGER cb,
            [In, ComAliasName("DWORD")] uint dwLockType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Stat(
            [In] IStream* This,
            [Out] STATSTG* pstatstg,
            [In, ComAliasName("DWORD")] uint grfStatFlag
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Clone(
            [In] IStream* This,
            [Out] IStream** ppstm = null
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IStream* This = &this)
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
            fixed (IStream* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IStream* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region ISequentialStream Methods
        [return: ComAliasName("HRESULT")]
        public int Read(
            [Out] void* pv,
            [In, ComAliasName("ULONG")] uint cb,
            [Out, ComAliasName("ULONG")] uint* pcbRead = null
        )
        {
            fixed (IStream* This = &this)
            {
                return MarshalFunction<_Read>(lpVtbl->Read)(
                    This,
                    pv,
                    cb,
                    pcbRead
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Write(
            [In] void* pv,
            [In, ComAliasName("ULONG")] uint cb,
            [Out, ComAliasName("ULONG")] uint* pcbWritten = null
        )
        {
            fixed (IStream* This = &this)
            {
                return MarshalFunction<_Write>(lpVtbl->Write)(
                    This,
                    pv,
                    cb,
                    pcbWritten
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int Seek(
            [In] LARGE_INTEGER dlibMove,
            [In, ComAliasName("DWORD")] uint dwOrigin,
            [Out] ULARGE_INTEGER* plibNewPosition = null
        )
        {
            fixed (IStream* This = &this)
            {
                return MarshalFunction<_Seek>(lpVtbl->Seek)(
                    This,
                    dlibMove,
                    dwOrigin,
                    plibNewPosition
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetSize(
            [In] ULARGE_INTEGER libNewSize
        )
        {
            fixed (IStream* This = &this)
            {
                return MarshalFunction<_SetSize>(lpVtbl->SetSize)(
                    This,
                    libNewSize
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyTo(
            [In] IStream* pstm,
            [In] ULARGE_INTEGER cb,
            [Out] ULARGE_INTEGER* pcbRead = null,
            [Out] ULARGE_INTEGER* pcbWritten = null
        )
        {
            fixed (IStream* This = &this)
            {
                return MarshalFunction<_CopyTo>(lpVtbl->CopyTo)(
                    This,
                    pstm,
                    cb,
                    pcbRead,
                    pcbWritten
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Commit(
            [In, ComAliasName("DWORD")] uint grfCommitFlags
        )
        {
            fixed (IStream* This = &this)
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
            fixed (IStream* This = &this)
            {
                return MarshalFunction<_Revert>(lpVtbl->Revert)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int LockRegion(
            [In] ULARGE_INTEGER libOffset,
            [In] ULARGE_INTEGER cb,
            [In, ComAliasName("DWORD")] uint dwLockType
        )
        {
            fixed (IStream* This = &this)
            {
                return MarshalFunction<_LockRegion>(lpVtbl->LockRegion)(
                    This,
                    libOffset,
                    cb,
                    dwLockType
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int UnlockRegion(
            [In] ULARGE_INTEGER libOffset,
            [In] ULARGE_INTEGER cb,
            [In, ComAliasName("DWORD")] uint dwLockType
        )
        {
            fixed (IStream* This = &this)
            {
                return MarshalFunction<_UnlockRegion>(lpVtbl->UnlockRegion)(
                    This,
                    libOffset,
                    cb,
                    dwLockType
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Stat(
            [Out] STATSTG* pstatstg,
            [In, ComAliasName("DWORD")] uint grfStatFlag
        )
        {
            fixed (IStream* This = &this)
            {
                return MarshalFunction<_Stat>(lpVtbl->Stat)(
                    This,
                    pstatstg,
                    grfStatFlag
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Clone(
            [Out] IStream** ppstm = null
        )
        {
            fixed (IStream* This = &this)
            {
                return MarshalFunction<_Clone>(lpVtbl->Clone)(
                    This,
                    ppstm
                );
            }
        }
        #endregion

        #region Structs
        public /* unmanaged */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region ISequentialStream Fields
            public IntPtr Read;

            public IntPtr Write;
            #endregion

            #region Fields
            public IntPtr Seek;

            public IntPtr SetSize;

            public IntPtr CopyTo;

            public IntPtr Commit;

            public IntPtr Revert;

            public IntPtr LockRegion;

            public IntPtr UnlockRegion;

            public IntPtr Stat;

            public IntPtr Clone;
            #endregion
        }
        #endregion
    }
}

