// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.
namespace TerraFX.Interop.D3D12
{
    public enum D3D12_PIPELINE_STATE_SUBOBJECT_TYPE
    {
        ROOT_SIGNATURE = 0,

        VS = 1,

        PS = 2,

        DS = 3,

        HS = 4,

        GS = 5,

        CS = 6,

        STREAM_OUTPUT = 7,

        BLEND = 8,

        SAMPLE_MASK = 9,

        RASTERIZER = 10,

        DEPTH_STENCIL = 11,

        INPUT_LAYOUT = 12,

        IB_STRIP_CUT_VALUE = 13,

        PRIMITIVE_TOPOLOGY = 14,

        RENDER_TARGET_FORMATS = 15,

        DEPTH_STENCIL_FORMAT = 16,

        SAMPLE_DESC = 17,

        NODE_MASK = 18,

        CACHED_PSO = 19,

        FLAGS = 20,

        DEPTH_STENCIL1 = 21,

        MAX_VALID = 22
    }
}
