// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.D3D;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    unsafe public static class D3D12
    {
        #region Constants
        public const uint _16BIT_INDEX_STRIP_CUT_VALUE = 0xFFFF;

        public const uint _32BIT_INDEX_STRIP_CUT_VALUE = 0xFFFFFFFF;

        public const uint _8BIT_INDEX_STRIP_CUT_VALUE = 0xFF;

        public const uint APPEND_ALIGNED_ELEMENT = 0xFFFFFFFF;

        public const uint ARRAY_AXIS_ADDRESS_RANGE_BIT_COUNT = 9;

        public const uint CLIP_OR_CULL_DISTANCE_COUNT = 8;

        public const uint CLIP_OR_CULL_DISTANCE_ELEMENT_COUNT = 2;

        public const uint COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT = 14;

        public const uint COMMONSHADER_CONSTANT_BUFFER_COMPONENTS = 4;

        public const uint COMMONSHADER_CONSTANT_BUFFER_COMPONENT_BIT_COUNT = 32;

        public const uint COMMONSHADER_CONSTANT_BUFFER_HW_SLOT_COUNT = 15;

        public const uint COMMONSHADER_CONSTANT_BUFFER_PARTIAL_UPDATE_EXTENTS_BYTE_ALIGNMENT = 16;

        public const uint COMMONSHADER_CONSTANT_BUFFER_REGISTER_COMPONENTS = 4;

        public const uint COMMONSHADER_CONSTANT_BUFFER_REGISTER_COUNT = 15;

        public const uint COMMONSHADER_CONSTANT_BUFFER_REGISTER_READS_PER_INST = 1;

        public const uint COMMONSHADER_CONSTANT_BUFFER_REGISTER_READ_PORTS = 1;

        public const uint COMMONSHADER_FLOWCONTROL_NESTING_LIMIT = 64;

        public const uint COMMONSHADER_IMMEDIATE_CONSTANT_BUFFER_REGISTER_COMPONENTS = 4;

        public const uint COMMONSHADER_IMMEDIATE_CONSTANT_BUFFER_REGISTER_COUNT = 1;

        public const uint COMMONSHADER_IMMEDIATE_CONSTANT_BUFFER_REGISTER_READS_PER_INST = 1;

        public const uint COMMONSHADER_IMMEDIATE_CONSTANT_BUFFER_REGISTER_READ_PORTS = 1;

        public const uint COMMONSHADER_IMMEDIATE_VALUE_COMPONENT_BIT_COUNT = 32;

        public const uint COMMONSHADER_INPUT_RESOURCE_REGISTER_COMPONENTS = 1;

        public const uint COMMONSHADER_INPUT_RESOURCE_REGISTER_COUNT = 128;

        public const uint COMMONSHADER_INPUT_RESOURCE_REGISTER_READS_PER_INST = 1;

        public const uint COMMONSHADER_INPUT_RESOURCE_REGISTER_READ_PORTS = 1;

        public const uint COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT = 128;

        public const uint COMMONSHADER_SAMPLER_REGISTER_COMPONENTS = 1;

        public const uint COMMONSHADER_SAMPLER_REGISTER_COUNT = 16;

        public const uint COMMONSHADER_SAMPLER_REGISTER_READS_PER_INST = 1;

        public const uint COMMONSHADER_SAMPLER_REGISTER_READ_PORTS = 1;

        public const uint COMMONSHADER_SAMPLER_SLOT_COUNT = 16;

        public const uint COMMONSHADER_SUBROUTINE_NESTING_LIMIT = 32;

        public const uint COMMONSHADER_TEMP_REGISTER_COMPONENTS = 4;

        public const uint COMMONSHADER_TEMP_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint COMMONSHADER_TEMP_REGISTER_COUNT = 4096;

        public const uint COMMONSHADER_TEMP_REGISTER_READS_PER_INST = 3;

        public const uint COMMONSHADER_TEMP_REGISTER_READ_PORTS = 3;

        public const uint COMMONSHADER_TEXCOORD_RANGE_REDUCTION_MAX = 10;

        public const int COMMONSHADER_TEXCOORD_RANGE_REDUCTION_MIN = -10;

        public const int COMMONSHADER_TEXEL_OFFSET_MAX_NEGATIVE = -8;

        public const uint COMMONSHADER_TEXEL_OFFSET_MAX_POSITIVE = 7;

        public const uint CONSTANT_BUFFER_DATA_PLACEMENT_ALIGNMENT = 256;

        public const uint CS_4_X_BUCKET00_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 256;

        public const uint CS_4_X_BUCKET00_MAX_NUM_THREADS_PER_GROUP = 64;

        public const uint CS_4_X_BUCKET01_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 240;

        public const uint CS_4_X_BUCKET01_MAX_NUM_THREADS_PER_GROUP = 68;

        public const uint CS_4_X_BUCKET02_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 224;

        public const uint CS_4_X_BUCKET02_MAX_NUM_THREADS_PER_GROUP = 72;

        public const uint CS_4_X_BUCKET03_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 208;

        public const uint CS_4_X_BUCKET03_MAX_NUM_THREADS_PER_GROUP = 76;

        public const uint CS_4_X_BUCKET04_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 192;

        public const uint CS_4_X_BUCKET04_MAX_NUM_THREADS_PER_GROUP = 84;

        public const uint CS_4_X_BUCKET05_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 176;

        public const uint CS_4_X_BUCKET05_MAX_NUM_THREADS_PER_GROUP = 92;

        public const uint CS_4_X_BUCKET06_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 160;

        public const uint CS_4_X_BUCKET06_MAX_NUM_THREADS_PER_GROUP = 100;

        public const uint CS_4_X_BUCKET07_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 144;

        public const uint CS_4_X_BUCKET07_MAX_NUM_THREADS_PER_GROUP = 112;

        public const uint CS_4_X_BUCKET08_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 128;

        public const uint CS_4_X_BUCKET08_MAX_NUM_THREADS_PER_GROUP = 128;

        public const uint CS_4_X_BUCKET09_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 112;

        public const uint CS_4_X_BUCKET09_MAX_NUM_THREADS_PER_GROUP = 144;

        public const uint CS_4_X_BUCKET10_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 96;

        public const uint CS_4_X_BUCKET10_MAX_NUM_THREADS_PER_GROUP = 168;

        public const uint CS_4_X_BUCKET11_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 80;

        public const uint CS_4_X_BUCKET11_MAX_NUM_THREADS_PER_GROUP = 204;

        public const uint CS_4_X_BUCKET12_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 64;

        public const uint CS_4_X_BUCKET12_MAX_NUM_THREADS_PER_GROUP = 256;

        public const uint CS_4_X_BUCKET13_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 48;

        public const uint CS_4_X_BUCKET13_MAX_NUM_THREADS_PER_GROUP = 340;

        public const uint CS_4_X_BUCKET14_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 32;

        public const uint CS_4_X_BUCKET14_MAX_NUM_THREADS_PER_GROUP = 512;

        public const uint CS_4_X_BUCKET15_MAX_BYTES_TGSM_WRITABLE_PER_THREAD = 16;

        public const uint CS_4_X_BUCKET15_MAX_NUM_THREADS_PER_GROUP = 768;

        public const uint CS_4_X_DISPATCH_MAX_THREAD_GROUPS_IN_Z_DIMENSION = 1;

        public const uint CS_4_X_RAW_UAV_BYTE_ALIGNMENT = 256;

        public const uint CS_4_X_THREAD_GROUP_MAX_THREADS_PER_GROUP = 768;

        public const uint CS_4_X_THREAD_GROUP_MAX_X = 768;

        public const uint CS_4_X_THREAD_GROUP_MAX_Y = 768;

        public const uint CS_4_X_UAV_REGISTER_COUNT = 1;

        public const uint CS_DISPATCH_MAX_THREAD_GROUPS_PER_DIMENSION = 65535;

        public const uint CS_TGSM_REGISTER_COUNT = 8192;

        public const uint CS_TGSM_REGISTER_READS_PER_INST = 1;

        public const uint CS_TGSM_RESOURCE_REGISTER_COMPONENTS = 1;

        public const uint CS_TGSM_RESOURCE_REGISTER_READ_PORTS = 1;

        public const uint CS_THREADGROUPID_REGISTER_COMPONENTS = 3;

        public const uint CS_THREADGROUPID_REGISTER_COUNT = 1;

        public const uint CS_THREADIDINGROUPFLATTENED_REGISTER_COMPONENTS = 1;

        public const uint CS_THREADIDINGROUPFLATTENED_REGISTER_COUNT = 1;

        public const uint CS_THREADIDINGROUP_REGISTER_COMPONENTS = 3;

        public const uint CS_THREADIDINGROUP_REGISTER_COUNT = 1;

        public const uint CS_THREADID_REGISTER_COMPONENTS = 3;

        public const uint CS_THREADID_REGISTER_COUNT = 1;

        public const uint CS_THREAD_GROUP_MAX_THREADS_PER_GROUP = 1024;

        public const uint CS_THREAD_GROUP_MAX_X = 1024;

        public const uint CS_THREAD_GROUP_MAX_Y = 1024;

        public const uint CS_THREAD_GROUP_MAX_Z = 64;

        public const uint CS_THREAD_GROUP_MIN_X = 1;

        public const uint CS_THREAD_GROUP_MIN_Y = 1;

        public const uint CS_THREAD_GROUP_MIN_Z = 1;

        public const uint CS_THREAD_LOCAL_TEMP_REGISTER_POOL = 16384;

        public const float DEFAULT_BLEND_FACTOR_ALPHA = 1.0f;

        public const float DEFAULT_BLEND_FACTOR_BLUE = 1.0f;

        public const float DEFAULT_BLEND_FACTOR_GREEN = 1.0f;

        public const float DEFAULT_BLEND_FACTOR_RED = 1.0f;

        public const float DEFAULT_BORDER_COLOR_COMPONENT = 0.0f;

        public const uint DEFAULT_DEPTH_BIAS = 0;

        public const float DEFAULT_DEPTH_BIAS_CLAMP = 0.0f;

        public const uint DEFAULT_MAX_ANISOTROPY = 16;

        public const float DEFAULT_MIP_LOD_BIAS = 0.0f;

        public const uint DEFAULT_MSAA_RESOURCE_PLACEMENT_ALIGNMENT = 4194304;

        public const uint DEFAULT_RENDER_TARGET_ARRAY_INDEX = 0;

        public const uint DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT = 65536;

        public const uint DEFAULT_SAMPLE_MASK = 0xFFFFFFFF;

        public const uint DEFAULT_SCISSOR_ENDX = 0;

        public const uint DEFAULT_SCISSOR_ENDY = 0;

        public const uint DEFAULT_SCISSOR_STARTX = 0;

        public const uint DEFAULT_SCISSOR_STARTY = 0;

        public const float DEFAULT_SLOPE_SCALED_DEPTH_BIAS = 0.0f;

        public const uint DEFAULT_STENCIL_READ_MASK = 0xFF;

        public const uint DEFAULT_STENCIL_REFERENCE = 0;

        public const uint DEFAULT_STENCIL_WRITE_MASK = 0xFF;

        public const uint DEFAULT_VIEWPORT_AND_SCISSORRECT_INDEX = 0;

        public const uint DEFAULT_VIEWPORT_HEIGHT = 0;

        public const float EFAULT_VIEWPORT_MAX_DEPTH = 0.0f;

        public const float EFAULT_VIEWPORT_MIN_DEPTH = 0.0f;

        public const uint DEFAULT_VIEWPORT_TOPLEFTX = 0;

        public const uint DEFAULT_VIEWPORT_TOPLEFTY = 0;

        public const uint DEFAULT_VIEWPORT_WIDTH = 0;

        public const uint DESCRIPTOR_RANGE_OFFSET_APPEND = 0xFFFFFFFF;

        public const uint DRIVER_RESERVED_REGISTER_SPACE_VALUES_END = 0xFFFFFFF7;

        public const uint DRIVER_RESERVED_REGISTER_SPACE_VALUES_START = 0xFFFFFFF0;

        public const uint DS_INPUT_CONTROL_POINTS_MAX_TOTAL_SCALARS = 3968;

        public const uint DS_INPUT_CONTROL_POINT_REGISTER_COMPONENTS = 4;

        public const uint DS_INPUT_CONTROL_POINT_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint DS_INPUT_CONTROL_POINT_REGISTER_COUNT = 32;

        public const uint DS_INPUT_CONTROL_POINT_REGISTER_READS_PER_INST = 2;

        public const uint DS_INPUT_CONTROL_POINT_REGISTER_READ_PORTS = 1;

        public const uint DS_INPUT_DOMAIN_POINT_REGISTER_COMPONENTS = 3;

        public const uint DS_INPUT_DOMAIN_POINT_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint DS_INPUT_DOMAIN_POINT_REGISTER_COUNT = 1;

        public const uint DS_INPUT_DOMAIN_POINT_REGISTER_READS_PER_INST = 2;

        public const uint DS_INPUT_DOMAIN_POINT_REGISTER_READ_PORTS = 1;

        public const uint DS_INPUT_PATCH_CONSTANT_REGISTER_COMPONENTS = 4;

        public const uint DS_INPUT_PATCH_CONSTANT_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint DS_INPUT_PATCH_CONSTANT_REGISTER_COUNT = 32;

        public const uint DS_INPUT_PATCH_CONSTANT_REGISTER_READS_PER_INST = 2;

        public const uint DS_INPUT_PATCH_CONSTANT_REGISTER_READ_PORTS = 1;

        public const uint DS_INPUT_PRIMITIVE_ID_REGISTER_COMPONENTS = 1;

        public const uint DS_INPUT_PRIMITIVE_ID_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint DS_INPUT_PRIMITIVE_ID_REGISTER_COUNT = 1;

        public const uint DS_INPUT_PRIMITIVE_ID_REGISTER_READS_PER_INST = 2;

        public const uint DS_INPUT_PRIMITIVE_ID_REGISTER_READ_PORTS = 1;

        public const uint DS_OUTPUT_REGISTER_COMPONENTS = 4;

        public const uint DS_OUTPUT_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint DS_OUTPUT_REGISTER_COUNT = 32;

        public const float FLOAT16_FUSED_TOLERANCE_IN_ULP = 0.6f;

        public const float FLOAT32_MAX = 3.402823466e+38f;

        public const float FLOAT32_TO_INTEGER_TOLERANCE_IN_ULP = 0.6f;

        public const float FLOAT_TO_SRGB_EXPONENT_DENOMINATOR = 2.4f;

        public const float FLOAT_TO_SRGB_EXPONENT_NUMERATOR = 1.0f;

        public const float FLOAT_TO_SRGB_OFFSET = 0.055f;

        public const float FLOAT_TO_SRGB_SCALE_1 = 12.92f;

        public const float FLOAT_TO_SRGB_SCALE_2 = 1.055f;

        public const float FLOAT_TO_SRGB_THRESHOLD = 0.0031308f;

        public const float FTOI_INSTRUCTION_MAX_INPUT = 2147483647.999f;

        public const float FTOI_INSTRUCTION_MIN_INPUT = -2147483648.999f;

        public const float FTOU_INSTRUCTION_MAX_INPUT = 4294967295.999f;

        public const float FTOU_INSTRUCTION_MIN_INPUT = 0.0f;

        public const uint GS_INPUT_INSTANCE_ID_READS_PER_INST = 2;

        public const uint GS_INPUT_INSTANCE_ID_READ_PORTS = 1;

        public const uint GS_INPUT_INSTANCE_ID_REGISTER_COMPONENTS = 1;

        public const uint GS_INPUT_INSTANCE_ID_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint GS_INPUT_INSTANCE_ID_REGISTER_COUNT = 1;

        public const uint GS_INPUT_PRIM_CONST_REGISTER_COMPONENTS = 1;

        public const uint GS_INPUT_PRIM_CONST_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint GS_INPUT_PRIM_CONST_REGISTER_COUNT = 1;

        public const uint GS_INPUT_PRIM_CONST_REGISTER_READS_PER_INST = 2;

        public const uint GS_INPUT_PRIM_CONST_REGISTER_READ_PORTS = 1;

        public const uint GS_INPUT_REGISTER_COMPONENTS = 4;

        public const uint GS_INPUT_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint GS_INPUT_REGISTER_COUNT = 32;

        public const uint GS_INPUT_REGISTER_READS_PER_INST = 2;

        public const uint GS_INPUT_REGISTER_READ_PORTS = 1;

        public const uint GS_INPUT_REGISTER_VERTICES = 32;

        public const uint GS_MAX_INSTANCE_COUNT = 32;

        public const uint GS_MAX_OUTPUT_VERTEX_COUNT_ACROSS_INSTANCES = 1024;

        public const uint GS_OUTPUT_ELEMENTS = 32;

        public const uint GS_OUTPUT_REGISTER_COMPONENTS = 4;

        public const uint GS_OUTPUT_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint GS_OUTPUT_REGISTER_COUNT = 32;

        public const uint HS_CONTROL_POINT_PHASE_INPUT_REGISTER_COUNT = 32;

        public const uint HS_CONTROL_POINT_PHASE_OUTPUT_REGISTER_COUNT = 32;

        public const uint HS_CONTROL_POINT_REGISTER_COMPONENTS = 4;

        public const uint HS_CONTROL_POINT_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint HS_CONTROL_POINT_REGISTER_READS_PER_INST = 2;

        public const uint HS_CONTROL_POINT_REGISTER_READ_PORTS = 1;

        public const uint HS_FORK_PHASE_INSTANCE_COUNT_UPPER_BOUND = 0xFFFFFFFF;

        public const uint HS_INPUT_FORK_INSTANCE_ID_REGISTER_COMPONENTS = 1;

        public const uint HS_INPUT_FORK_INSTANCE_ID_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint HS_INPUT_FORK_INSTANCE_ID_REGISTER_COUNT = 1;

        public const uint HS_INPUT_FORK_INSTANCE_ID_REGISTER_READS_PER_INST = 2;

        public const uint HS_INPUT_FORK_INSTANCE_ID_REGISTER_READ_PORTS = 1;

        public const uint HS_INPUT_JOIN_INSTANCE_ID_REGISTER_COMPONENTS = 1;

        public const uint HS_INPUT_JOIN_INSTANCE_ID_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint HS_INPUT_JOIN_INSTANCE_ID_REGISTER_COUNT = 1;

        public const uint HS_INPUT_JOIN_INSTANCE_ID_REGISTER_READS_PER_INST = 2;

        public const uint HS_INPUT_JOIN_INSTANCE_ID_REGISTER_READ_PORTS = 1;

        public const uint HS_INPUT_PRIMITIVE_ID_REGISTER_COMPONENTS = 1;

        public const uint HS_INPUT_PRIMITIVE_ID_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint HS_INPUT_PRIMITIVE_ID_REGISTER_COUNT = 1;

        public const uint HS_INPUT_PRIMITIVE_ID_REGISTER_READS_PER_INST = 2;

        public const uint HS_INPUT_PRIMITIVE_ID_REGISTER_READ_PORTS = 1;

        public const uint HS_JOIN_PHASE_INSTANCE_COUNT_UPPER_BOUND = 0xFFFFFFFF;

        public const float HS_MAXTESSFACTOR_LOWER_BOUND = 1.0f;

        public const float HS_MAXTESSFACTOR_UPPER_BOUND = 64.0f;

        public const uint HS_OUTPUT_CONTROL_POINTS_MAX_TOTAL_SCALARS = 3968;

        public const uint HS_OUTPUT_CONTROL_POINT_ID_REGISTER_COMPONENTS = 1;

        public const uint HS_OUTPUT_CONTROL_POINT_ID_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint HS_OUTPUT_CONTROL_POINT_ID_REGISTER_COUNT = 1;

        public const uint HS_OUTPUT_CONTROL_POINT_ID_REGISTER_READS_PER_INST = 2;

        public const uint HS_OUTPUT_CONTROL_POINT_ID_REGISTER_READ_PORTS = 1;

        public const uint HS_OUTPUT_PATCH_CONSTANT_REGISTER_COMPONENTS = 4;

        public const uint HS_OUTPUT_PATCH_CONSTANT_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint HS_OUTPUT_PATCH_CONSTANT_REGISTER_COUNT = 32;

        public const uint HS_OUTPUT_PATCH_CONSTANT_REGISTER_READS_PER_INST = 2;

        public const uint HS_OUTPUT_PATCH_CONSTANT_REGISTER_READ_PORTS = 1;

        public const uint HS_OUTPUT_PATCH_CONSTANT_REGISTER_SCALAR_COMPONENTS = 128;

        public const uint IA_DEFAULT_INDEX_BUFFER_OFFSET_IN_BYTES = 0;

        public const uint IA_DEFAULT_PRIMITIVE_TOPOLOGY = 0;

        public const uint IA_DEFAULT_VERTEX_BUFFER_OFFSET_IN_BYTES = 0;

        public const uint IA_INDEX_INPUT_RESOURCE_SLOT_COUNT = 1;

        public const uint IA_INSTANCE_ID_BIT_COUNT = 32;

        public const uint IA_INTEGER_ARITHMETIC_BIT_COUNT = 32;

        public const uint IA_PATCH_MAX_CONTROL_POINT_COUNT = 32;

        public const uint IA_PRIMITIVE_ID_BIT_COUNT = 32;

        public const uint IA_VERTEX_ID_BIT_COUNT = 32;

        public const uint IA_VERTEX_INPUT_RESOURCE_SLOT_COUNT = 32;

        public const uint IA_VERTEX_INPUT_STRUCTURE_ELEMENTS_COMPONENTS = 128;

        public const uint IA_VERTEX_INPUT_STRUCTURE_ELEMENT_COUNT = 32;

        public const uint INTEGER_DIVIDE_BY_ZERO_QUOTIENT = 0xFFFFFFFF;

        public const uint INTEGER_DIVIDE_BY_ZERO_REMAINDER = 0xFFFFFFFF;

        public const uint KEEP_RENDER_TARGETS_AND_DEPTH_STENCIL = 0xFFFFFFFF;

        public const uint KEEP_UNORDERED_ACCESS_VIEWS = 0xFFFFFFFF;

        public const float LINEAR_GAMMA = 1.0f;

        public const uint MAJOR_VERSION = 12;

        public const float MAX_BORDER_COLOR_COMPONENT = 1.0f;

        public const float MAX_DEPTH = 1.0f;

        public const uint MAX_LIVE_STATIC_SAMPLERS = 2032;

        public const uint MAX_MAXANISOTROPY = 16;

        public const uint MAX_MULTISAMPLE_SAMPLE_COUNT = 32;

        public const float MAX_POSITION_VALUE = 3.402823466e+34f;

        public const uint MAX_ROOT_COST = 64;

        public const uint MAX_SHADER_VISIBLE_DESCRIPTOR_HEAP_SIZE_TIER_1 = 1000000;

        public const uint MAX_SHADER_VISIBLE_DESCRIPTOR_HEAP_SIZE_TIER_2 = 1000000;

        public const uint MAX_SHADER_VISIBLE_SAMPLER_HEAP_SIZE = 2048;

        public const uint MAX_TEXTURE_DIMENSION_2_TO_EXP = 17;

        public const uint MINOR_VERSION = 0;

        public const float MIN_BORDER_COLOR_COMPONENT = 0.0f;

        public const float MIN_DEPTH = 0.0f;

        public const uint MIN_MAXANISOTROPY = 0;

        public const float MIP_LOD_BIAS_MAX = 15.99f;

        public const float MIP_LOD_BIAS_MIN = -16.0f;

        public const uint MIP_LOD_FRACTIONAL_BIT_COUNT = 8;

        public const uint MIP_LOD_RANGE_BIT_COUNT = 8;

        public const float MULTISAMPLE_ANTIALIAS_LINE_WIDTH = 1.4f;

        public const uint NONSAMPLE_FETCH_OUT_OF_RANGE_ACCESS_RESULT = 0;

        public const uint OS_RESERVED_REGISTER_SPACE_VALUES_END = 0xFFFFFFFF;

        public const uint OS_RESERVED_REGISTER_SPACE_VALUES_START = 0xFFFFFFF8;

        public const uint PACKED_TILE = 0xFFFFFFFF;

        public const uint PIXEL_ADDRESS_RANGE_BIT_COUNT = 15;

        public const uint PRE_SCISSOR_PIXEL_ADDRESS_RANGE_BIT_COUNT = 16;

        public const uint PS_CS_UAV_REGISTER_COMPONENTS = 1;

        public const uint PS_CS_UAV_REGISTER_COUNT = 8;

        public const uint PS_CS_UAV_REGISTER_READS_PER_INST = 1;

        public const uint PS_CS_UAV_REGISTER_READ_PORTS = 1;

        public const uint PS_FRONTFACING_DEFAULT_VALUE = 0xFFFFFFFF;

        public const uint PS_FRONTFACING_FALSE_VALUE = 0x00000000;

        public const uint PS_FRONTFACING_TRUE_VALUE = 0xFFFFFFFF;

        public const uint PS_INPUT_REGISTER_COMPONENTS = 4;

        public const uint PS_INPUT_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint PS_INPUT_REGISTER_COUNT = 32;

        public const uint PS_INPUT_REGISTER_READS_PER_INST = 2;

        public const uint PS_INPUT_REGISTER_READ_PORTS = 1;

        public const float PS_LEGACY_PIXEL_CENTER_FRACTIONAL_COMPONENT = 0.0f;

        public const uint PS_OUTPUT_DEPTH_REGISTER_COMPONENTS = 1;

        public const uint PS_OUTPUT_DEPTH_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint PS_OUTPUT_DEPTH_REGISTER_COUNT = 1;

        public const uint PS_OUTPUT_MASK_REGISTER_COMPONENTS = 1;

        public const uint PS_OUTPUT_MASK_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint PS_OUTPUT_MASK_REGISTER_COUNT = 1;

        public const uint PS_OUTPUT_REGISTER_COMPONENTS = 4;

        public const uint PS_OUTPUT_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint PS_OUTPUT_REGISTER_COUNT = 8;

        public const float PS_PIXEL_CENTER_FRACTIONAL_COMPONENT = 0.5f;

        public const uint RAW_UAV_SRV_BYTE_ALIGNMENT = 16;

        public const uint REQ_BLEND_OBJECT_COUNT_PER_DEVICE = 4096;

        public const uint REQ_BUFFER_RESOURCE_TEXEL_COUNT_2_TO_EXP = 27;

        public const uint REQ_CONSTANT_BUFFER_ELEMENT_COUNT = 4096;

        public const uint REQ_DEPTH_STENCIL_OBJECT_COUNT_PER_DEVICE = 4096;

        public const uint REQ_DRAWINDEXED_INDEX_COUNT_2_TO_EXP = 32;

        public const uint REQ_DRAW_VERTEX_COUNT_2_TO_EXP = 32;

        public const uint REQ_FILTERING_HW_ADDRESSABLE_RESOURCE_DIMENSION = 16384;

        public const uint REQ_GS_INVOCATION_32BIT_OUTPUT_COMPONENT_LIMIT = 1024;

        public const uint REQ_IMMEDIATE_CONSTANT_BUFFER_ELEMENT_COUNT = 4096;

        public const uint REQ_MAXANISOTROPY = 16;

        public const uint REQ_MIP_LEVELS = 15;

        public const uint REQ_MULTI_ELEMENT_STRUCTURE_SIZE_IN_BYTES = 2048;

        public const uint REQ_RASTERIZER_OBJECT_COUNT_PER_DEVICE = 4096;

        public const uint REQ_RENDER_TO_BUFFER_WINDOW_WIDTH = 16384;

        public const uint REQ_RESOURCE_SIZE_IN_MEGABYTES_EXPRESSION_A_TERM = 128;

        public const float REQ_RESOURCE_SIZE_IN_MEGABYTES_EXPRESSION_B_TERM = 0.25f;

        public const uint REQ_RESOURCE_SIZE_IN_MEGABYTES_EXPRESSION_C_TERM = 2048;

        public const uint REQ_RESOURCE_VIEW_COUNT_PER_DEVICE_2_TO_EXP = 20;

        public const uint REQ_SAMPLER_OBJECT_COUNT_PER_DEVICE = 4096;

        public const uint REQ_SUBRESOURCES = 30720;

        public const uint REQ_TEXTURE1D_ARRAY_AXIS_DIMENSION = 2048;

        public const uint REQ_TEXTURE1D_U_DIMENSION = 16384;

        public const uint REQ_TEXTURE2D_ARRAY_AXIS_DIMENSION = 2048;

        public const uint REQ_TEXTURE2D_U_OR_V_DIMENSION = 16384;

        public const uint REQ_TEXTURE3D_U_V_OR_W_DIMENSION = 2048;

        public const uint REQ_TEXTURECUBE_DIMENSION = 16384;

        public const uint RESINFO_INSTRUCTION_MISSING_COMPONENT_RETVAL = 0;

        public const uint RESOURCE_BARRIER_ALL_SUBRESOURCES = 0xFFFFFFFF;

        public const uint SHADER_MAJOR_VERSION = 5;

        public const uint SHADER_MAX_INSTANCES = 65535;

        public const uint SHADER_MAX_INTERFACES = 253;

        public const uint SHADER_MAX_INTERFACE_CALL_SITES = 4096;

        public const uint SHADER_MAX_TYPES = 65535;

        public const uint SHADER_MINOR_VERSION = 1;

        public const uint SHIFT_INSTRUCTION_PAD_VALUE = 0;

        public const uint SHIFT_INSTRUCTION_SHIFT_VALUE_BIT_COUNT = 5;

        public const uint SIMULTANEOUS_RENDER_TARGET_COUNT = 8;

        public const uint SMALL_MSAA_RESOURCE_PLACEMENT_ALIGNMENT = 65536;

        public const uint SMALL_RESOURCE_PLACEMENT_ALIGNMENT = 4096;

        public const uint SO_BUFFER_MAX_STRIDE_IN_BYTES = 2048;

        public const uint SO_BUFFER_MAX_WRITE_WINDOW_IN_BYTES = 512;

        public const uint SO_BUFFER_SLOT_COUNT = 4;

        public const uint SO_DDI_REGISTER_INDEX_DENOTING_GAP = 0xFFFFFFFF;

        public const uint SO_NO_RASTERIZED_STREAM = 0xFFFFFFFF;

        public const uint SO_OUTPUT_COMPONENT_COUNT = 128;

        public const uint SO_STREAM_COUNT = 4;

        public const uint SPEC_DATE_DAY = 14;

        public const uint SPEC_DATE_MONTH = 11;

        public const uint SPEC_DATE_YEAR = 2014;

        public const float SPEC_VERSION = 1.16f;

        public const float SRGB_GAMMA = 2.2f;

        public const float SRGB_TO_FLOAT_DENOMINATOR_1 = 12.92f;

        public const float SRGB_TO_FLOAT_DENOMINATOR_2 = 1.055f;

        public const float SRGB_TO_FLOAT_EXPONENT = 2.4f;

        public const float SRGB_TO_FLOAT_OFFSET = 0.055f;

        public const float SRGB_TO_FLOAT_THRESHOLD = 0.04045f;

        public const float SRGB_TO_FLOAT_TOLERANCE_IN_ULP = 0.5f;

        public const uint STANDARD_COMPONENT_BIT_COUNT = 32;

        public const uint STANDARD_COMPONENT_BIT_COUNT_DOUBLED = 64;

        public const uint STANDARD_MAXIMUM_ELEMENT_ALIGNMENT_BYTE_MULTIPLE = 4;

        public const uint STANDARD_PIXEL_COMPONENT_COUNT = 128;

        public const uint STANDARD_PIXEL_ELEMENT_COUNT = 32;

        public const uint STANDARD_VECTOR_SIZE = 4;

        public const uint STANDARD_VERTEX_ELEMENT_COUNT = 32;

        public const uint STANDARD_VERTEX_TOTAL_COMPONENT_COUNT = 64;

        public const uint SUBPIXEL_FRACTIONAL_BIT_COUNT = 8;

        public const uint SUBTEXEL_FRACTIONAL_BIT_COUNT = 8;

        public const uint SYSTEM_RESERVED_REGISTER_SPACE_VALUES_END = 0xFFFFFFFF;

        public const uint SYSTEM_RESERVED_REGISTER_SPACE_VALUES_START = 0xFFFFFFF0;

        public const uint TESSELLATOR_MAX_EVEN_TESSELLATION_FACTOR = 64;

        public const uint TESSELLATOR_MAX_ISOLINE_DENSITY_TESSELLATION_FACTOR = 64;

        public const uint TESSELLATOR_MAX_ODD_TESSELLATION_FACTOR = 63;

        public const uint TESSELLATOR_MAX_TESSELLATION_FACTOR = 64;

        public const uint TESSELLATOR_MIN_EVEN_TESSELLATION_FACTOR = 2;

        public const uint TESSELLATOR_MIN_ISOLINE_DENSITY_TESSELLATION_FACTOR = 1;

        public const uint TESSELLATOR_MIN_ODD_TESSELLATION_FACTOR = 1;

        public const uint TEXEL_ADDRESS_RANGE_BIT_COUNT = 16;

        public const uint TEXTURE_DATA_PITCH_ALIGNMENT = 256;

        public const uint TEXTURE_DATA_PLACEMENT_ALIGNMENT = 512;

        public const uint TILED_RESOURCE_TILE_SIZE_IN_BYTES = 65536;

        public const uint UAV_COUNTER_PLACEMENT_ALIGNMENT = 4096;

        public const uint UAV_SLOT_COUNT = 64;

        public const uint UNBOUND_MEMORY_ACCESS_RESULT = 0;

        public const uint VIEWPORT_AND_SCISSORRECT_MAX_INDEX = 15;

        public const uint VIEWPORT_AND_SCISSORRECT_OBJECT_COUNT_PER_PIPELINE = 16;

        public const uint VIEWPORT_BOUNDS_MAX = 32767;

        public const int VIEWPORT_BOUNDS_MIN = -32768;

        public const uint VS_INPUT_REGISTER_COMPONENTS = 4;

        public const uint VS_INPUT_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint VS_INPUT_REGISTER_COUNT = 32;

        public const uint VS_INPUT_REGISTER_READS_PER_INST = 2;

        public const uint VS_INPUT_REGISTER_READ_PORTS = 1;

        public const uint VS_OUTPUT_REGISTER_COMPONENTS = 4;

        public const uint VS_OUTPUT_REGISTER_COMPONENT_BIT_COUNT = 32;

        public const uint VS_OUTPUT_REGISTER_COUNT = 32;

        public const uint WHQL_CONTEXT_COUNT_FOR_RESOURCE_LIMIT = 10;

        public const uint WHQL_DRAWINDEXED_INDEX_COUNT_2_TO_EXP = 25;

        public const uint WHQL_DRAW_VERTEX_COUNT_2_TO_EXP = 25;

        public const uint SHADER_COMPONENT_MAPPING_MASK = 0x7;

        public const int SHADER_COMPONENT_MAPPING_SHIFT = 3;

        public const uint SHADER_COMPONENT_MAPPING_ALWAYS_SET_BIT_AVOIDING_ZEROMEM_MISTAKES = (1 << (SHADER_COMPONENT_MAPPING_SHIFT * 4));

        public const uint FILTER_REDUCTION_TYPE_MASK = 0x00000003;

        public const int FILTER_REDUCTION_TYPE_SHIFT = 7;

        public const uint FILTER_TYPE_MASK = 0x00000003;

        public const int MIN_FILTER_SHIFT = 4;

        public const int MAG_FILTER_SHIFT = 2;

        public const int MIP_FILTER_SHIFT = 0;

        public const uint ANISOTROPIC_FILTERING_BIT = 0x00000040;

        public static readonly D3D12_SHADER_COMPONENT_MAPPING DEFAULT_SHADER_4_COMPONENT_MAPPING = ENCODE_SHADER_4_COMPONENT_MAPPING(D3D12_SHADER_COMPONENT_MAPPING.FROM_MEMORY_COMPONENT_0, D3D12_SHADER_COMPONENT_MAPPING.FROM_MEMORY_COMPONENT_1, D3D12_SHADER_COMPONENT_MAPPING.FROM_MEMORY_COMPONENT_2, D3D12_SHADER_COMPONENT_MAPPING.FROM_MEMORY_COMPONENT_3);

        public static readonly Guid ExperimentalShaderModels = new Guid("76F5573E-F13A-40F5-B297-81CE9E18933F");

        public static readonly Guid DXGI_DEBUG_D3D12 = new Guid(0XCF59A98C, 0XA950, 0X4326, 0X91, 0XEF, 0X9B, 0XBA, 0XA1, 0X7B, 0XFD, 0X95);
        #endregion

        #region Methods
        public static D3D12_SHADER_COMPONENT_MAPPING ENCODE_SHADER_4_COMPONENT_MAPPING(D3D12_SHADER_COMPONENT_MAPPING Src0, D3D12_SHADER_COMPONENT_MAPPING Src1, D3D12_SHADER_COMPONENT_MAPPING Src2, D3D12_SHADER_COMPONENT_MAPPING Src3)
        {
            return (D3D12_SHADER_COMPONENT_MAPPING)(((uint)(Src0) & SHADER_COMPONENT_MAPPING_MASK)
                 | (((uint)(Src1) & SHADER_COMPONENT_MAPPING_MASK) << SHADER_COMPONENT_MAPPING_SHIFT)
                 | (((uint)(Src2) & SHADER_COMPONENT_MAPPING_MASK) << (SHADER_COMPONENT_MAPPING_SHIFT * 2))
                 | (((uint)(Src3) & SHADER_COMPONENT_MAPPING_MASK) << (SHADER_COMPONENT_MAPPING_SHIFT * 3))
                 | SHADER_COMPONENT_MAPPING_ALWAYS_SET_BIT_AVOIDING_ZEROMEM_MISTAKES);
        }

        public static D3D12_SHADER_COMPONENT_MAPPING DECODE_SHADER_4_COMPONENT_MAPPING(D3D12_SHADER_COMPONENT_MAPPING ComponentToExtract, D3D12_SHADER_COMPONENT_MAPPING Mapping)
        {
            return (D3D12_SHADER_COMPONENT_MAPPING)((uint)(Mapping) >> (SHADER_COMPONENT_MAPPING_SHIFT * (int)(ComponentToExtract)) & SHADER_COMPONENT_MAPPING_MASK);
        }

        public static D3D12_FILTER ENCODE_BASIC_FILTER(D3D12_FILTER_TYPE min, D3D12_FILTER_TYPE mag, D3D12_FILTER_TYPE mip, D3D12_FILTER_REDUCTION_TYPE reduction)
        {
            return (D3D12_FILTER)((((uint)(min) & FILTER_TYPE_MASK) << MIN_FILTER_SHIFT)
                 | (((uint)(mag) & FILTER_TYPE_MASK) << MAG_FILTER_SHIFT)
                 | (((uint)(mip) & FILTER_TYPE_MASK) << MIP_FILTER_SHIFT)
                 | (((uint)(reduction) & FILTER_REDUCTION_TYPE_MASK) << FILTER_REDUCTION_TYPE_SHIFT));
        }

        public static D3D12_FILTER ENCODE_ANISOTROPIC_FILTER(D3D12_FILTER_REDUCTION_TYPE reduction)
        {
            return (D3D12_FILTER)(ANISOTROPIC_FILTERING_BIT)
                 | ENCODE_BASIC_FILTER(D3D12_FILTER_TYPE.LINEAR, D3D12_FILTER_TYPE.LINEAR, D3D12_FILTER_TYPE.LINEAR, reduction);
        }

        public static D3D12_FILTER_TYPE DECODE_MIN_FILTER(D3D12_FILTER D3D12Filter)
        {
            return (D3D12_FILTER_TYPE)(((uint)(D3D12Filter) >> MIN_FILTER_SHIFT) & FILTER_TYPE_MASK);
        }

        public static D3D12_FILTER_TYPE DECODE_MAG_FILTER(D3D12_FILTER D3D12Filter)
        {
            return (D3D12_FILTER_TYPE)(((uint)(D3D12Filter) >> MAG_FILTER_SHIFT) & FILTER_TYPE_MASK);
        }

        public static D3D12_FILTER_TYPE DECODE_MIP_FILTER(D3D12_FILTER D3D12Filter)
        {
            return (D3D12_FILTER_TYPE)(((uint)(D3D12Filter) >> MIP_FILTER_SHIFT) & FILTER_TYPE_MASK);
        }

        public static D3D12_FILTER_REDUCTION_TYPE DECODE_FILTER_REDUCTION(D3D12_FILTER D3D12Filter)
        {
            return (D3D12_FILTER_REDUCTION_TYPE)(((uint)(D3D12Filter) >> FILTER_REDUCTION_TYPE_SHIFT) & FILTER_REDUCTION_TYPE_MASK);
        }

        public static bool DECODE_IS_COMPARISON_FILTER(D3D12_FILTER D3D12Filter)
        {
            return DECODE_FILTER_REDUCTION(D3D12Filter) == D3D12_FILTER_REDUCTION_TYPE.COMPARISON;
        }

        public static bool DECODE_IS_ANISOTROPIC_FILTER(D3D12_FILTER D3D12Filter)
        {
            return (((uint)(D3D12Filter) & ANISOTROPIC_FILTERING_BIT) != 0)
                && (D3D12_FILTER_TYPE.LINEAR == DECODE_MIN_FILTER(D3D12Filter))
                && (D3D12_FILTER_TYPE.LINEAR == DECODE_MAG_FILTER(D3D12Filter))
                && (D3D12_FILTER_TYPE.LINEAR == DECODE_MIP_FILTER(D3D12Filter));
        }

        [DllImport("D3D12", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D3D12SerializeRootSignature", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern HRESULT SerializeRootSignature(
            [In] D3D12_ROOT_SIGNATURE_DESC* pRootSignature,
            [In] D3D_ROOT_SIGNATURE_VERSION Version,
            [Out] ID3DBlob** ppBlob,
            [Out, Optional] ID3DBlob** ppErrorBlob
        );

        [DllImport("D3D12", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D3D12CreateRootSignatureDeserializer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern HRESULT CreateRootSignatureDeserializer(
            [In] void* pSrcData,
            [In] UIntPtr SrcDataSizeInBytes,
            [In] Guid* pRootSignatureDeserializerInterface,
            [Out] void** ppRootSignatureDeserializer
        );

        [DllImport("D3D12", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D3D12CreateVersionedRootSignatureDeserializer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern HRESULT SerializeVersionedRootSignature(
            [In] D3D12_VERSIONED_ROOT_SIGNATURE_DESC* pRootSignature,
            [Out] ID3DBlob** ppBlob,
            [Out, Optional] ID3DBlob** ppErrorBlob
        );

        [DllImport("D3D12", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D3D12CreateVersionedRootSignatureDeserializer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern HRESULT CreateVersionedRootSignatureDeserializer(
            [In] void* pSrcData,
            [In] UIntPtr SrcDataSizeInBytes,
            [In] Guid* pRootSignatureDeserializerInterface,
            [Out] void** ppRootSignatureDeserializer
        );

        [DllImport("D3D12", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D3D12CreateDevice", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern HRESULT CreateDevice(
            [In, Optional] IUnknown* pAdapter,
            D3D_FEATURE_LEVEL MinimumFeatureLevel,
            [In] Guid* riid,
            [Out, Optional] void** ppDevice
        );

        [DllImport("D3D12", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D3D12GetDebugInterface", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern HRESULT GetDebugInterface(
            [In] Guid* riid,
            [Out, Optional] void** ppvDebug
        );

        [DllImport("D3D12", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D3D12EnableExperimentalFeatures", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern HRESULT EnableExperimentalFeatures(
            [In] uint NumFeatures,
            [In] Guid* pIIDs,
            [In, Optional] void* pConfigurationStructs,
            [In, Optional] uint* pConfigurationStructSizes
        );
        #endregion
    }
}
