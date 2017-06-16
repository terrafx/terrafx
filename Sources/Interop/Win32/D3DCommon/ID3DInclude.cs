// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\D3DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    // This interface is intended to be implemented by the application, and can
    // be used by various D3D APIs. This enables application-specific handling
    // of #include directives in source files.
    unsafe public struct ID3DInclude
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        // Opens an include file. If successful, it should fill in ppData and
        // pBytes. The data pointer returned must remain valid until Close is
        // subsequently called. The name of the file is encoded in UTF-8 format.
        public /* static */ delegate HRESULT Open(
            [In] ID3DInclude* This,
            [In] D3D_INCLUDE_TYPE IncludeType,
            [In] /* const */ LPSTR pFileName,
            [In] /* const */ void* pParentData,
            [Out] /* const */ void** ppData,
            [Out] uint* pBytes
        );

        // Closes an include file. If Open was successful, Close is guaranteed
        // to be called before the API using this interface returns.
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
