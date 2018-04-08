// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ struct D3D12_VIEWPORT
    {
        #region Fields
        [ComAliasName("FLOAT")]
        public float TopLeftX;

        [ComAliasName("FLOAT")]
        public float TopLeftY;

        [ComAliasName("FLOAT")]
        public float Width;

        [ComAliasName("FLOAT")]
        public float Height;

        [ComAliasName("FLOAT")]
        public float MinDepth;

        [ComAliasName("FLOAT")]
        public float MaxDepth;
        #endregion
    }
}
