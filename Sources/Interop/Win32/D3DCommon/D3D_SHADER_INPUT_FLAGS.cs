// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\D3DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum D3D_SHADER_INPUT_FLAGS : uint
    {
        NONE = 0x00000000,

        USERPACKED = 0x00000001,

        COMPARISON_SAMPLER = 0x00000002,    // is this a comparison sampler?

        TEXTURE_COMPONENT_0 = 0x00000004,   // this 2-but value encodes c - 1, where c

        TEXTURE_COMPONENT_1 = 0x00000008,   // is the number of components in the texture

        TEXTURE_COMPONENTS = (TEXTURE_COMPONENT_0 | TEXTURE_COMPONENT_1),

        UNUSED = 0x00000010
    }
}
