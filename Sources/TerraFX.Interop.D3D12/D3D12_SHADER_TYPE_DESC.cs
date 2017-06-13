// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public struct D3D12_SHADER_TYPE_DESC
    {
        #region Fields
        public D3D_SHADER_VARIABLE_CLASS Class;

        public D3D_SHADER_VARIABLE_TYPE Type;

        public uint Rows;

        public uint Columns;

        public uint Elements;

        public uint Members;

        public uint Offset;

        public LPSTR Name;
        #endregion
    }
}
