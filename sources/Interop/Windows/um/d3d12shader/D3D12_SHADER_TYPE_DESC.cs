// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ unsafe struct D3D12_SHADER_TYPE_DESC
    {
        #region Fields
        public D3D_SHADER_VARIABLE_CLASS Class;

        public D3D_SHADER_VARIABLE_TYPE Type;

        [ComAliasName("UINT")]
        public uint Rows;

        [ComAliasName("UINT")]
        public uint Columns;

        [ComAliasName("UINT")]
        public uint Elements;

        [ComAliasName("UINT")]
        public uint Members;

        [ComAliasName("UINT")]
        public uint Offset;

        [ComAliasName("LPCSTR")]
        public sbyte* Name;
        #endregion
    }
}
