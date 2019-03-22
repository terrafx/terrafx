// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct D3D12_SHADER_INPUT_BIND_DESC
    {
        #region Fields
        [ComAliasName("LPCSTR")]
        public sbyte* Name;

        public D3D_SHADER_INPUT_TYPE Type;

        [ComAliasName("UINT")]
        public uint BindPoint;

        [ComAliasName("UINT")]
        public uint BindCount;

        [ComAliasName("UINT")]
        public uint uFlags;

        public D3D_RESOURCE_RETURN_TYPE ReturnType;

        public D3D_SRV_DIMENSION Dimension;

        [ComAliasName("UINT")]
        public uint NumSamples;

        [ComAliasName("UINT")]
        public uint Space;

        [ComAliasName("UINT")]
        public uint uID;
        #endregion
    }
}
