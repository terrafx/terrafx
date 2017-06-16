// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.
namespace TerraFX.Interop
{
    public enum D3D12_FEATURE
    {
        D3D12_OPTIONS = 0,

        ARCHITECTURE = 1,

        FEATURE_LEVELS = 2,

        FORMAT_SUPPORT = 3,

        MULTISAMPLE_QUALITY_LEVELS = 4,

        FORMAT_INFO = 5,

        GPU_VIRTUAL_ADDRESS_SUPPORT = 6,

        SHADER_MODEL = 7,

        D3D12_OPTIONS1 = 8,

        ROOT_SIGNATURE = 12,

        ARCHITECTURE1 = 16,

        D3D12_OPTIONS2 = 18,

        SHADER_CACHE = 19,

        COMMAND_QUEUE_PRIORITY = 20
    }
}
