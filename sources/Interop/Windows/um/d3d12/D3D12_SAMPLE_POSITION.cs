// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ struct D3D12_SAMPLE_POSITION
    {
        #region Fields
        [ComAliasName("INT8")]
        public sbyte X;

        [ComAliasName("INT8")]
        public sbyte Y;
        #endregion
    }
}
