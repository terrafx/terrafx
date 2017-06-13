// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("8BA5FB08-5195-40E2-AC58-0D989C3A0102")]
    unsafe public struct ID3DBlob
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3DBlob).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate void* GetBufferPointer(
            [In] ID3DBlob* This
        );

        public /* static */ delegate UIntPtr GetBufferSize(
            [In] ID3DBlob* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetBufferPointer GetBufferPointer;

            public GetBufferSize GetBufferSize;
            #endregion
        }
        #endregion
    }
}
