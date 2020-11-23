// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.D3D12_HEAP_FLAGS;
using static TerraFX.Interop.D3D12_HEAP_TYPE;
using static TerraFX.Interop.D3D12_RESOURCE_FLAGS;
using static TerraFX.Interop.D3D12_RESOURCE_HEAP_TIER;
using static TerraFX.Interop.DXGI_FORMAT;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public class D3D12GraphicsMemoryTexelMapper
    {
        /// <summary>
        /// Maps from the TerraFX TexelFormat to the DXGI_FORMAT
        /// </summary>
        /// <param name="texelFormat">The TerraFX TexelFormat to map.</param>
        /// <returns></returns>
        public static TerraFX.Interop.DXGI_FORMAT Map(TexelFormat texelFormat)
        {
            switch (texelFormat)
            {
                case TexelFormat.RGBA4xByte:
                    return DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM;
                case TexelFormat.XSInt16:
                    return DXGI_FORMAT.DXGI_FORMAT_R16_SINT;
                default:
                    return DXGI_FORMAT.DXGI_FORMAT_UNKNOWN;
            }
        }
    }
}
