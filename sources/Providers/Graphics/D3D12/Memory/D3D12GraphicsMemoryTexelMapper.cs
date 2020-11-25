// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using TerraFX.Interop;
using static TerraFX.Interop.DXGI_FORMAT;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public class D3D12GraphicsMemoryTexelMapper
    {
        /// <summary>Maps from a texel format to the DXGI_FORMAT</summary>
        /// <param name="texelFormat">The texel format to map.</param>
        /// <returns></returns>
        public static DXGI_FORMAT Map(TexelFormat texelFormat) => texelFormat switch {
            TexelFormat.R8G8B8A8_UNORM => DXGI_FORMAT_R8G8B8A8_UNORM,
            TexelFormat.R16_SINT => DXGI_FORMAT_R16_SINT,
            TexelFormat.R16G16UINT => DXGI_FORMAT_R16G16_UINT,
            _ => DXGI_FORMAT_UNKNOWN,
        };
    }
}
