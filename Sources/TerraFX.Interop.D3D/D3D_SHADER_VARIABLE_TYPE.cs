// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum D3D_SHADER_VARIABLE_TYPE
    {
        VOID = 0,

        BOOL = 1,

        INT = 2,

        FLOAT = 3,

        STRING = 4,

        TEXTURE = 5,

        TEXTURE1D = 6,

        TEXTURE2D = 7,

        TEXTURE3D = 8,

        TEXTURECUBE = 9,

        SAMPLER = 10,

        SAMPLER1D = 11,

        SAMPLER2D = 12,

        SAMPLER3D = 13,

        SAMPLERCUBE = 14,

        PIXELSHADER = 15,

        VERTEXSHADER = 16,

        PIXELFRAGMENT = 17,

        VERTEXFRAGMENT = 18,

        UINT = 19,

        UINT8 = 20,

        GEOMETRYSHADER = 21,

        RASTERIZER = 22,

        DEPTHSTENCIL = 23,

        BLEND = 24,

        BUFFER = 25,

        CBUFFER = 26,

        TBUFFER = 27,

        TEXTURE1DARRAY = 28,

        TEXTURE2DARRAY = 29,

        RENDERTARGETVIEW = 30,

        DEPTHSTENCILVIEW = 31,

        TEXTURE2DMS = 32,

        TEXTURE2DMSARRAY = 33,

        TEXTURECUBEARRAY = 34,

        HULLSHADER = 35,

        DOMAINSHADER = 36,

        INTERFACE_POINTER = 37,

        COMPUTESHADER = 38,

        DOUBLE = 39,

        RWTEXTURE1D = 40,

        RWTEXTURE1DARRAY = 41,

        RWTEXTURE2D = 42,

        RWTEXTURE2DARRAY = 43,

        RWTEXTURE3D = 44,

        RWBUFFER = 45,

        BYTEADDRESS_BUFFER = 46,

        RWBYTEADDRESS_BUFFER = 47,

        STRUCTURED_BUFFER = 48,

        RWSTRUCTURED_BUFFER = 49,

        APPEND_STRUCTURED_BUFFER = 50,

        CONSUME_STRUCTURED_BUFFER = 51,

        MIN8FLOAT = 52,

        MIN10FLOAT = 53,

        MIN16FLOAT = 54,

        MIN12INT = 55,

        MIN16INT = 56,

        MIN16UINT = 57,

        FORCE_DWORD = 0x7FFFFFFF
    }
}
