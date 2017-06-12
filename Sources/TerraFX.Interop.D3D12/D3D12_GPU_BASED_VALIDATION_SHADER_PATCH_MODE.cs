// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.D3D12
{
    public enum D3D12_GPU_BASED_VALIDATION_SHADER_PATCH_MODE
    {
        NONE = 0,

        STATE_TRACKING_ONLY = 1,

        UNGUARDED_VALIDATION = 2,

        GUARDED_VALIDATION = 3,

        NUM = 4
    }
}
