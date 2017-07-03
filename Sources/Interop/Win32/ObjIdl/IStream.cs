// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\ObjIdlbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("0000000C-0000-0000-C000-000000000046")]
    unsafe public struct IStream
    {
        #region Constants
        public static readonly Guid IID = typeof(IStream).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT Seek(
            [In] long dlibMove,
            [In] uint dwOrigin,
            [Out, Optional]  ulong* plibNewPosition
        );

        public /* static */ delegate HRESULT SetSize(
            [In] ulong libNewSize
        );

        public /* static */ delegate HRESULT CopyTo(
            [In] IStream* pstm,
            [In] ulong cb,
            [Out, Optional]  ulong* pcbRead,
            [Out, Optional]  ulong* pcbWritten
        );

        public /* static */ delegate HRESULT Commit(
            [In] uint grfCommitFlags
        );

        public /* static */ delegate HRESULT Revert(
        );

        public /* static */ delegate HRESULT LockRegion(
            [In] ulong libOffset,
            [In] ulong cb,
            [In] uint dwLockType
        );

        public /* static */ delegate HRESULT UnlockRegion(
            [In] ulong libOffset,
            [In] ulong cb,
            [In] uint dwLockType
        );

        public /* static */ delegate HRESULT Stat(
            [Out] STATSTG* pstatstg,
            [In] uint grfStatFlag
        );

        public /* static */ delegate HRESULT Clone(
            [Out, Optional] IStream** ppstm
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ISequentialStream.Vtbl BaseVtbl;

            public Seek Seek;

            public SetSize SetSize;

            public CopyTo CopyTo;

            public Commit Commit;

            public Revert Revert;

            public LockRegion LockRegion;

            public UnlockRegion UnlockRegion;

            public Stat Stat;

            public Clone Clone;
            #endregion
        }
        #endregion
    }
}
