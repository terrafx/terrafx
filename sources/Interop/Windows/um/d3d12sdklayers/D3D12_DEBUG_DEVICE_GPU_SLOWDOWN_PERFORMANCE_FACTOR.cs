// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ struct D3D12_DEBUG_DEVICE_GPU_SLOWDOWN_PERFORMANCE_FACTOR
    {
        #region Fields
        [ComAliasName("FLOAT")]
        public float SlowdownFactor;
        #endregion
    }
}
