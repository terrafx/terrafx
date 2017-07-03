// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("00020403-0000-0000-C000-000000000046")]
    unsafe public struct ITypeComp
    {
        #region Constants
        public static readonly Guid IID = typeof(ITypeComp).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT Bind(
            [In] LPOLESTR szName,
            [In] uint lHashVal,
            [In] ushort wFlags,
            [Out] ITypeInfo** ppTInfo,
            [Out] DESCKIND* pDescKind,
            [Out] BINDPTR* pBindPtr
        );

        public /* static */ delegate HRESULT BindType(
            [In] LPOLESTR szName,
            [In] uint lHashVal,
            [Out] ITypeInfo** ppTInfo,
            [Out] ITypeComp** ppTComp
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public Bind Bind;

            public BindType BindType;
            #endregion
        }
        #endregion
    }
}
