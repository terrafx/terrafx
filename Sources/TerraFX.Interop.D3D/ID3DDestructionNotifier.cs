// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("A06EB39A-50DA-425B-8C31-4EECD6C270F3")]
    unsafe public struct ID3DDestructionNotifier
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3DDestructionNotifier).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT RegisterDestructionCallback(
            [In] ID3DDestructionNotifier* This,
            [In] PFN_DESTRUCTION_CALLBACK callbackFn,
            [In] void* pData,
            [Out] uint* pCallbackID
        );

        public /* static */ delegate HRESULT UnregisterDestructionCallback(
            [In] ID3DDestructionNotifier* This,
            [In] uint callbackID
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public RegisterDestructionCallback RegisterDestructionCallback;

            public UnregisterDestructionCallback UnregisterDestructionCallback;
            #endregion
        }
        #endregion
    }
}
