// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public struct D3D12_PARAMETER_DESC
    {
        #region Fields
        public LPSTR Name;

        public LPSTR SemanticName;

        public D3D_SHADER_VARIABLE_TYPE Type;

        public D3D_SHADER_VARIABLE_CLASS Class;

        public uint Rows;

        public uint Columns;

        public D3D_INTERPOLATION_MODE InterpolationMode;

        public D3D_PARAMETER_FLAGS Flags;

        public uint FirstInRegister;

        public uint FirstInComponent;

        public uint FirstOutRegister;

        public uint FirstOutComponent;
        #endregion
    }
}
