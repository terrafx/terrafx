// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;

namespace TerraFX.Graphics;

public partial class GraphicsDevice
{
    [Flags]
    private enum FeatureFlags
    {
        None = 0,

        Uma = 1 << 1,

        CacheCoherentUma = 1 << 2,
    }
}
