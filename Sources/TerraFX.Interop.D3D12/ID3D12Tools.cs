// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    [Guid("7071E1F0-E84B-4B33-974F-12FA49DE65C5")]
    unsafe public struct ID3D12Tools
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12Tools).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate void EnableShaderInstrumentation(
            [In] ID3D12Tools* This,
            [In] BOOL bEnable
        );

        public /* static */ delegate BOOL ShaderInstrumentationEnabled(
            [In] ID3D12Tools* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public EnableShaderInstrumentation EnableShaderInstrumentation;

            public ShaderInstrumentationEnabled ShaderInstrumentationEnabled;
            #endregion
        }
        #endregion
    }
}
