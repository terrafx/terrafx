// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct D3D12_PARAMETER_DESC
    {
        #region Fields
        [ComAliasName("LPCSTR")]
        public /* readonly */ sbyte* Name;

        [ComAliasName("LPCSTR")]
        public /* readonly */ sbyte* SemanticName;

        public D3D_SHADER_VARIABLE_TYPE Type;

        public D3D_SHADER_VARIABLE_CLASS Class;

        [ComAliasName("UINT")]
        public uint Rows;

        [ComAliasName("UINT")]
        public uint Columns;

        public D3D_INTERPOLATION_MODE InterpolationMode;

        public D3D_PARAMETER_FLAGS Flags;

        [ComAliasName("UINT")]
        public uint FirstInRegister;

        [ComAliasName("UINT")]
        public uint FirstInComponent;

        [ComAliasName("UINT")]
        public uint FirstOutRegister;

        [ComAliasName("UINT")]
        public uint FirstOutComponent;
        #endregion
    }
}
