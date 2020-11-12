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
using ChannelType = TerraFX.Graphics.TexelFormat.Channel.Type;
using ChannelKind = TerraFX.Graphics.TexelFormat.Channel.Kind;

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
            if (texelFormat.Channels == null || texelFormat.Channels.Length == 0)
                return DXGI_FORMAT.DXGI_FORMAT_UNKNOWN;
            var c = texelFormat.Channels;

            // first quickly check for some common formats
            if (c.Length == 4
                && c[0].ChannelType == ChannelType.UInt8 && c[0].ChannelKind == ChannelKind.R
                && c[1].ChannelType == ChannelType.UInt8 && c[1].ChannelKind == ChannelKind.G
                && c[2].ChannelType == ChannelType.UInt8 && c[2].ChannelKind == ChannelKind.B
                && c[3].ChannelType == ChannelType.UInt8 && c[3].ChannelKind == ChannelKind.A
                )
                    return DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM;

            if (c.Length == 1 && c[0].ChannelKind == ChannelKind.R)
            {
                if (c[0].ChannelType == ChannelType.UInt8)
                    return DXGI_FORMAT.DXGI_FORMAT_R8_UNORM;

                if (c[0].ChannelType == ChannelType.UInt16)
                    return DXGI_FORMAT.DXGI_FORMAT_R16_UINT;

                if (c[0].ChannelType == ChannelType.SInt8)
                    return DXGI_FORMAT.DXGI_FORMAT_R16_SINT;

                if (c[0].ChannelType == ChannelType.SInt16)
                    return DXGI_FORMAT.DXGI_FORMAT_R16_SINT;

                if (c[0].ChannelType == ChannelType.Float16)
                    return DXGI_FORMAT.DXGI_FORMAT_R16_FLOAT;

                if (c[0].ChannelType == ChannelType.Float32)
                    return DXGI_FORMAT.DXGI_FORMAT_R32_FLOAT;
            }

            // for the remainder do a name match
            string formatName = "DXGI_FORMAT_";
            var channelType = c[0].ChannelType;
            var channelKind = c[0].ChannelKind;
            string bitsStr = "";
            for (int i = 0; i < c.Length; i++)
            {
                if (channelType != c[i].ChannelType)
                    break;
                channelKind = c[i].ChannelKind;
                channelType = c[i].ChannelType;
                formatName += channelKind.ToString();
                bitsStr = TexelFormat.Channel.BitsUsed(channelType).ToString();
                formatName += bitsStr;
            }
            formatName += "_" + channelType.ToString().Replace(bitsStr, "").ToUpper();
            var formats = (DXGI_FORMAT[])Enum.GetValues(typeof(DXGI_FORMAT));
            foreach (var format in formats)
            {
                if (formatName == format.ToString())
                    return format;
            }

            // to do: NORM, SRGB, TYPELESS

            // nothing matched, return unknown
            return DXGI_FORMAT.DXGI_FORMAT_UNKNOWN;
        }
    }
}
