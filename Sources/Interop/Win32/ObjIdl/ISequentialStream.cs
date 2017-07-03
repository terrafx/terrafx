// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\ObjIdlbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("0C733A30-2A1C-11CE-ADE5-00AA0044773D")]
    unsafe public struct ISequentialStream
    {
        #region Constants
        public static readonly Guid IID = typeof(ISequentialStream).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT Read(
            [Out] void* pv,
            [In] uint cb,
            [Out, Optional] uint* pcbRead
        );

        public /* static */ delegate HRESULT Write(
            [In] void* pv,
            [In] uint cb,
            [Out, Optional] uint* pcbWritten
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public Read Read;

            public Write Write;
            #endregion
        }
        #endregion
    }
}
