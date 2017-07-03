// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\ObjIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("0000000B-0000-0000-C000-000000000046")]
    unsafe public struct IStorage
    {
        #region Constants
        public static readonly Guid IID = typeof(IStorage).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT CreateStream(
            [In] OLECHAR* pwcsName,
            [In] uint grfMode,
            [In] uint reserved1,
            [In] uint reserved2,
            [Out, Optional] IStream** ppstm
        );

        public /* static */ delegate HRESULT OpenStream(
            [In] OLECHAR* pwcsName,
            [In] void* reserved1,
            [In] uint grfMode,
            [In] uint reserved2,
            [Out]  IStream** ppstm
        );

        public /* static */ delegate HRESULT CreateStorage(
            [In] OLECHAR* pwcsName,
            [In] uint grfMode,
            [In] uint reserved1,
            [In] uint reserved2,
            [Out, Optional] IStorage** ppstg
        );

        public /* static */ delegate HRESULT OpenStorage(
            [In, Optional] OLECHAR* pwcsName,
            [In, Optional] IStorage* pstgPriority,
            [In] uint grfMode,
            [In, Optional] LPOLESTR snbExclude,
            [In] uint reserved,
            [Out, Optional] IStorage** ppstg
        );

        public /* static */ delegate HRESULT CopyTo(
            [In] uint ciidExclude,
            [In, Optional] Guid* rgiidExclude,
            [In, Optional] LPOLESTR snbExclude,
            [In]  IStorage* pstgDest
        );

        public /* static */ delegate HRESULT MoveElementTo(
            [In] OLECHAR* pwcsName,
            [In, Optional] IStorage* pstgDest,
            [In] OLECHAR* pwcsNewName,
            [In] uint grfFlags
        );

        public /* static */ delegate HRESULT Commit(
            [In] uint grfCommitFlags
        );

        public /* static */ delegate HRESULT Revert(
        );

        public /* static */ delegate HRESULT EnumElements(
            [In]  uint reserved1,
            [In]  void* reserved2,
            [In]  uint reserved3,
            [Out] IEnumSTATSTG** ppenum
        );

        public /* static */ delegate HRESULT DestroyElement(
            [In] OLECHAR* pwcsName
        );

        public /* static */ delegate HRESULT RenameElement(
            [In] OLECHAR* pwcsOldName,
            [In] OLECHAR* pwcsNewName
        );

        public /* static */ delegate HRESULT SetElementTimes(
            [In, Optional] OLECHAR* pwcsName,
            [In, Optional] FILETIME* pctime,
            [In, Optional] FILETIME* patime,
            [In, Optional] FILETIME* pmtime
        );

        public /* static */ delegate HRESULT SetClass(
            [In] ref /* readonly */ Guid clsid
        );

        public /* static */ delegate HRESULT SetStateBits(
            [In] uint grfStateBits,
            [In] uint grfMask
        );

        public /* static */ delegate HRESULT Stat(
            [Out] STATSTG* pstatstg,
            [In] uint grfStatFlag
        );
        #endregion

        #region Structs
        public struct Vtbl
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
