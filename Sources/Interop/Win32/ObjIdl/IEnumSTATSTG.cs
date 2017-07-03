// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\ObjIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("0000000D-0000-0000-C000-000000000046")]
    unsafe public struct IEnumSTATSTG
    {
        #region Constants
        public static readonly Guid IID = typeof(IEnumSTATSTG).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT Next(
            [In] uint celt,
            [Out] STATSTG* rgelt,
            [Out, Optional] uint* pceltFetched
        );

        public /* static */ delegate HRESULT Skip(
            [In] uint celt
        );

        public /* static */ delegate HRESULT Reset(
        );

        public /* static */ delegate HRESULT Clone(
            [Out, Optional] IEnumSTATSTG **ppenum
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public Next Next;

            public Skip Skip;

            public Reset Reset;

            public Clone Clone;
            #endregion
        }
        #endregion
    }
}
