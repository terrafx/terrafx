// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.D3D12
{
    [Flags]
    public enum D3D12_FORMAT_SUPPORT1
    {
        NONE = 0,

        BUFFER = 1,

        IA_VERTEX_BUFFER = 2,

        IA_INDEX_BUFFER = 4,

        SO_BUFFER = 8,

        TEXTURE1D = 16,

        TEXTURE2D = 32,

        TEXTURE3D = 64,

        TEXTURECUBE = 128,

        SHADER_LOAD = 256,

        SHADER_SAMPLE = 512,

        SHADER_SAMPLE_COMPARISON = 1024,

        SHADER_SAMPLE_MONO_TEXT = 2048,

        MIP = 4096,

        RENDER_TARGET = 16384,

        BLENDABLE = 32768,

        DEPTH_STENCIL = 65536,

        MULTISAMPLE_RESOLVE = 262144,

        DISPLAY = 524288,

        CAST_WITHIN_BIT_LAYOUT = 1048576,

        MULTISAMPLE_RENDERTARGET = 2097152,

        MULTISAMPLE_LOAD = 4194304,

        SHADER_GATHER = 8388608,

        BACK_BUFFER_CAST = 16777216,

        TYPED_UNORDERED_ACCESS_VIEW = 33554432,

        SHADER_GATHER_COMPARISON = 67108864,

        DECODER_OUTPUT = 134217728,

        VIDEO_PROCESSOR_OUTPUT = 268435456,

        VIDEO_PROCESSOR_INPUT = 536870912,

        VIDEO_ENCODER = 1073741824
    }
}
