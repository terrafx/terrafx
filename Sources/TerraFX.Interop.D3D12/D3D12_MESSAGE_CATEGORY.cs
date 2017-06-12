// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.D3D12
{
    public enum D3D12_MESSAGE_CATEGORY
    {
        APPLICATION_DEFINED = 0,

        MISCELLANEOUS = 1,

        INITIALIZATION = 2,

        CLEANUP = 3,

        COMPILATION = 4,

        STATE_CREATION = 5,

        STATE_SETTING = 6,

        STATE_GETTING = 7,

        RESOURCE_MANIPULATION = 8,

        EXECUTION = 9,

        SHADER = 10
    }
}
