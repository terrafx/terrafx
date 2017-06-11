// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.D3D
{
    public enum D3D_NAME
    {
        UNDEFINED = 0,

        POSITION = 1,

        CLIP_DISTANCE = 2,

        CULL_DISTANCE = 3,

        RENDER_TARGET_ARRAY_INDEX = 4,

        VIEWPORT_ARRAY_INDEX = 5,

        VERTEX_ID = 6,

        PRIMITIVE_ID = 7,

        INSTANCE_ID = 8,

        IS_FRONT_FACE = 9,

        SAMPLE_INDEX = 10,

        FINAL_QUAD_EDGE_TESSFACTOR = 11,

        FINAL_QUAD_INSIDE_TESSFACTOR = 12,

        FINAL_TRI_EDGE_TESSFACTOR = 13,

        FINAL_TRI_INSIDE_TESSFACTOR = 14,

        FINAL_LINE_DETAIL_TESSFACTOR = 15,

        FINAL_LINE_DENSITY_TESSFACTOR = 16,

        TARGET = 64,

        DEPTH = 65,

        COVERAGE = 66,

        DEPTH_GREATER_EQUAL = 67,

        DEPTH_LESS_EQUAL = 68,

        STENCIL_REF = 69,

        INNER_COVERAGE = 70
    }
}
