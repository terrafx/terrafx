// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>This defines the input layout of vertices and the vertex shader which processes them.</summary>
    unsafe public /* blittable */ struct D2D1_CUSTOM_VERTEX_BUFFER_PROPERTIES
    {
        #region Fields
        [ComAliasName("BYTE")]
        public /* readonly */ byte* shaderBufferWithInputSignature;

        [ComAliasName("UINT32")]
        public uint shaderBufferSize;

        public /* readonly */ D2D1_INPUT_ELEMENT_DESC* inputElements;

        [ComAliasName("UINT32")]
        public uint elementCount;

        [ComAliasName("UINT32")]
        public uint stride;
        #endregion
    }
}
