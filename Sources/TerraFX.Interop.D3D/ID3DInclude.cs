// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public struct ID3DInclude
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT Open(
            [In] ID3DInclude* This,
            [In] D3D_INCLUDE_TYPE IncludeType,
            [In] LPSTR pFileName,
            [In] void* pParentData,
            [Out] void** ppData,
            [Out] uint* pBytes
        );

        public /* static */ delegate HRESULT Close(
            [In] ID3DInclude* This,
            [In] void* pData
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public Open Open;

            public Close Close;
            #endregion
        }
        #endregion
    }
}
