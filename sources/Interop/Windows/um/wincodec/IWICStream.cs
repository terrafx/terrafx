// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("135FF860-22B7-4DDF-B0F6-218F4F299A43")]
    [Unmanaged]
    public unsafe struct IWICStream
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICStream* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICStream* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICStream* This
        );
        #endregion

        #region ISequentialStream Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Read(
            [In] IWICStream* This,
            [Out] void* pv,
            [In, ComAliasName("ULONG")] uint cb,
            [Out, ComAliasName("ULONG")] uint* pcbRead = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Write(
            [In] IWICStream* This,
            [In] void* pv,
            [In, ComAliasName("ULONG")] uint cb,
            [Out, ComAliasName("ULONG")] uint* pcbWritten = null
        );
        #endregion

        #region IStream Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Seek(
            [In] IWICStream* This,
            [In] LARGE_INTEGER dlibMove,
            [In, ComAliasName("DWORD")] uint dwOrigin,
            [Out] ULARGE_INTEGER* plibNewPosition = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetSize(
            [In] IWICStream* This,
            [In] ULARGE_INTEGER libNewSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyTo(
            [In] IWICStream* This,
            [In] IStream* pstm,
            [In] ULARGE_INTEGER cb,
            [Out] ULARGE_INTEGER* pcbRead = null,
            [Out] ULARGE_INTEGER* pcbWritten = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Commit(
            [In] IWICStream* This,
            [In, ComAliasName("DWORD")] uint grfCommitFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Revert(
            [In] IWICStream* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _LockRegion(
            [In] IWICStream* This,
            [In] ULARGE_INTEGER libOffset,
            [In] ULARGE_INTEGER cb,
            [In, ComAliasName("DWORD")] uint dwLockType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _UnlockRegion(
            [In] IWICStream* This,
            [In] ULARGE_INTEGER libOffset,
            [In] ULARGE_INTEGER cb,
            [In, ComAliasName("DWORD")] uint dwLockType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Stat(
            [In] IWICStream* This,
            [Out] STATSTG* pstatstg,
            [In, ComAliasName("DWORD")] uint grfStatFlag
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Clone(
            [In] IWICStream* This,
            [Out] IStream** ppstm = null
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _InitializeFromIStream(
            [In] IWICStream* This,
            [In] IStream* pIStream = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _InitializeFromFilename(
            [In] IWICStream* This,
            [In, ComAliasName("LPCWSTR")] char* wzFileName,
            [In, ComAliasName("DWORD")] uint dwDesiredAccess
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _InitializeFromMemory(
            [In] IWICStream* This,
            [In, ComAliasName("WICInProcPointer")] byte* pbBuffer,
            [In, ComAliasName("DWORD")] uint cbBufferSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _InitializeFromIStreamRegion(
            [In] IWICStream* This,
            [In, Optional] IStream* pIStream,
            [In] ULARGE_INTEGER ulOffset,
            [In] ULARGE_INTEGER ulMaxSize
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICStream* This = &this)
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
            fixed (IWICStream* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICStream* This = &this)
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
            fixed (IWICStream* This = &this)
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
            fixed (IWICStream* This = &this)
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

        #region IStream Methods
        [return: ComAliasName("HRESULT")]
        public int Seek(
            [In] LARGE_INTEGER dlibMove,
            [In, ComAliasName("DWORD")] uint dwOrigin,
            [Out] ULARGE_INTEGER* plibNewPosition = null
        )
        {
            fixed (IWICStream* This = &this)
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
            fixed (IWICStream* This = &this)
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
            fixed (IWICStream* This = &this)
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
            fixed (IWICStream* This = &this)
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
            fixed (IWICStream* This = &this)
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
            fixed (IWICStream* This = &this)
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
            fixed (IWICStream* This = &this)
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
            fixed (IWICStream* This = &this)
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
            fixed (IWICStream* This = &this)
            {
                return MarshalFunction<_Clone>(lpVtbl->Clone)(
                    This,
                    ppstm
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int InitializeFromIStream(
            [In] IStream* pIStream = null
        )
        {
            fixed (IWICStream* This = &this)
            {
                return MarshalFunction<_InitializeFromIStream>(lpVtbl->InitializeFromIStream)(
                    This,
                    pIStream
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int InitializeFromFilename(
            [In, ComAliasName("LPCWSTR")] char* wzFileName,
            [In, ComAliasName("DWORD")] uint dwDesiredAccess
        )
        {
            fixed (IWICStream* This = &this)
            {
                return MarshalFunction<_InitializeFromFilename>(lpVtbl->InitializeFromFilename)(
                    This,
                    wzFileName,
                    dwDesiredAccess
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int InitializeFromMemory(
            [In, ComAliasName("WICInProcPointer")] byte* pbBuffer,
            [In, ComAliasName("DWORD")] uint cbBufferSize
        )
        {
            fixed (IWICStream* This = &this)
            {
                return MarshalFunction<_InitializeFromMemory>(lpVtbl->InitializeFromMemory)(
                    This,
                    pbBuffer,
                    cbBufferSize
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int InitializeFromIStreamRegion(
            [In, Optional] IStream* pIStream,
            [In] ULARGE_INTEGER ulOffset,
            [In] ULARGE_INTEGER ulMaxSize
        )
        {
            fixed (IWICStream* This = &this)
            {
                return MarshalFunction<_InitializeFromIStreamRegion>(lpVtbl->InitializeFromIStreamRegion)(
                    This,
                    pIStream,
                    ulOffset,
                    ulMaxSize
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

            #region ISequentialStream Fields
            public IntPtr Read;

            public IntPtr Write;
            #endregion

            #region IStream Fields
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

            #region Fields
            public IntPtr InitializeFromIStream;

            public IntPtr InitializeFromFilename;

            public IntPtr InitializeFromMemory;

            public IntPtr InitializeFromIStreamRegion;
            #endregion
        }
        #endregion
    }
}
