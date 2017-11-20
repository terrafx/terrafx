// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3dcompiler.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct D3D_SHADER_DATA
    {
        #region Fields
        [ComAliasName("LPCVOID")]
        public void* pBytecode;

        [ComAliasName("SIZE_T")]
        public nuint BytecodeLength;
        #endregion
    }
}
