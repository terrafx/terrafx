// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\Unkwnbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("00000000-0000-0000-C000-000000000046")]
    unsafe public struct IUnknown
    {
        #region Constants
        public static readonly Guid IID = typeof(IUnknown).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT QueryInterface(
            [In] IUnknown* This,
            [In] ref /* readonly */ Guid riid,
            [Out] void** ppvObject
        );

        public /* static */ delegate uint AddRef(
            [In] IUnknown* This
        );

        public /* static */ delegate uint Release(
            [In] IUnknown* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public QueryInterface QueryInterface;

            public AddRef AddRef;

            public Release Release;
            #endregion
        }
        #endregion
    }
}
