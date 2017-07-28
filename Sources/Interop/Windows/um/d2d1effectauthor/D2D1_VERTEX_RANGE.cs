// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>This defines the range of vertices from a vertex buffer to draw.</summary>
    public /* blittable */ struct D2D1_VERTEX_RANGE
    {
        #region Fields
        [ComAliasName("UINT32")]
        public uint startVertex;

        [ComAliasName("UINT32")]
        public uint vertexCount;
        #endregion
    }
}
