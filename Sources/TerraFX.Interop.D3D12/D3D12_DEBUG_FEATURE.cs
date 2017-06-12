// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.D3D12
{
    [Flags]
    public enum D3D12_DEBUG_FEATURE
    {
        NONE = 0,

        ALLOW_BEHAVIOR_CHANGING_DEBUG_AIDS = 1,

        CONSERVATIVE_RESOURCE_STATE_TRACKING = 2,

        DISABLE_VIRTUALIZED_BUNDLES_VALIDATION = 4,

        VALID_MASK = 7
    }
}
