// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_PARAMETER_DESC
    {
        #region Fields
        public LPCSTR Name;

        public LPCSTR SemanticName;

        public D3D_SHADER_VARIABLE_TYPE Type;

        public D3D_SHADER_VARIABLE_CLASS Class;

        public UINT Rows;

        public UINT Columns;

        public D3D_INTERPOLATION_MODE InterpolationMode;

        public D3D_PARAMETER_FLAGS Flags;

        public UINT FirstInRegister;

        public UINT FirstInComponent;

        public UINT FirstOutRegister;

        public UINT FirstOutComponent;
        #endregion
    }
}
