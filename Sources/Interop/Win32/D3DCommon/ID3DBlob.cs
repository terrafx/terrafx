// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\D3DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    // The buffer object is used by D3D to return arbitrary size data.
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
        // Returns a pointer to the beginning of the buffer.
        public /* static */ delegate void* GetBufferPointer(
            [In] ID3DBlob* This
        );

        // Returns the size of the buffer, in bytes.
        public /* static */ delegate nuint GetBufferSize(
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
