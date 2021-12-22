// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.	

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using TerraFX.Graphics;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.DirectX.DXGI_FORMAT;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

[assembly: SupportedOSPlatform("windows10.0")]

namespace TerraFX.Utilities;

internal static unsafe partial class D3D12Utilities
{
    private static readonly DXGI_FORMAT[] s_dxgiFormatMap = new DXGI_FORMAT[] {
        DXGI_FORMAT_UNKNOWN,                    // Unknown

        DXGI_FORMAT_R32G32B32A32_FLOAT,         // R32G32B32A32_SFLOAT
        DXGI_FORMAT_R32G32B32A32_UINT,          // R32G32B32A32_UINT
        DXGI_FORMAT_R32G32B32A32_SINT,          // R32G32B32A32_SINT

        DXGI_FORMAT_R32G32B32_FLOAT,            // R32G32B32_SFLOAT
        DXGI_FORMAT_R32G32B32_UINT,             // R32G32B32_UINT
        DXGI_FORMAT_R32G32B32_SINT,             // R32G32B32_SINT

        DXGI_FORMAT_R16G16B16A16_FLOAT,         // R16G16B16A16_SFLOAT
        DXGI_FORMAT_R16G16B16A16_UNORM,         // R16G16B16A16_UNORM
        DXGI_FORMAT_R16G16B16A16_UINT,          // R16G16B16A16_UINT 
        DXGI_FORMAT_R16G16B16A16_SNORM,         // R16G16B16A16_SNORM
        DXGI_FORMAT_R16G16B16A16_SINT,          // R16G16B16A16_SINT

        DXGI_FORMAT_R32G32_FLOAT,               // R32G32_SFLOAT
        DXGI_FORMAT_R32G32_UINT,                // R32G32_UINT
        DXGI_FORMAT_R32G32_SINT,                // R32G32_SINT

        DXGI_FORMAT_D32_FLOAT_S8X24_UINT,       // D32_SFLOAT_S8X24_UINT

        DXGI_FORMAT_R10G10B10A2_UNORM,          // R10G10B10A2_UNORM
        DXGI_FORMAT_R10G10B10A2_UINT,           // R10G10B10A2_UINT

        DXGI_FORMAT_R11G11B10_FLOAT,            // R11G11B10_UFLOAT

        DXGI_FORMAT_R8G8B8A8_UNORM,             // R8G8B8A8_UNORM
        DXGI_FORMAT_R8G8B8A8_UNORM_SRGB,        // R8G8B8A8_SRGB
        DXGI_FORMAT_R8G8B8A8_UINT,              // R8G8B8A8_UINT
        DXGI_FORMAT_R8G8B8A8_SNORM,             // R8G8B8A8_SNORM
        DXGI_FORMAT_R8G8B8A8_SINT,              // R8G8B8A8_SINT

        DXGI_FORMAT_R16G16_FLOAT,               // R16G16_SFLOAT
        DXGI_FORMAT_R16G16_UNORM,               // R16G16_UNORM
        DXGI_FORMAT_R16G16_UINT,                // R16G16_UINT
        DXGI_FORMAT_R16G16_SNORM,               // R16G16_SNORM
        DXGI_FORMAT_R16G16_SINT,                // R16G16_SINT

        DXGI_FORMAT_D32_FLOAT,                  // D32_SFLOAT

        DXGI_FORMAT_R32_FLOAT,                  // R32_SFLOAT
        DXGI_FORMAT_R32_UINT,                   // R32_UINT
        DXGI_FORMAT_R32_SINT,                   // R32_SINT

        DXGI_FORMAT_D24_UNORM_S8_UINT,          // D24_UNORM_S8_UINT

        DXGI_FORMAT_R8G8_UNORM,                 // R8G8_UNORM
        DXGI_FORMAT_R8G8_UINT,                  // R8G8_UINT
        DXGI_FORMAT_R8G8_SNORM,                 // R8G8_SNORM
        DXGI_FORMAT_R8G8_SINT,                  // R8G8_SINT

        DXGI_FORMAT_D16_UNORM,                  // D16_UNORM

        DXGI_FORMAT_R16_FLOAT,                  // R16_SFLOAT
        DXGI_FORMAT_R16_UNORM,                  // R16_UNORM
        DXGI_FORMAT_R16_UINT,                   // R16_UINT 
        DXGI_FORMAT_R16_SNORM,                  // R16_SNORM
        DXGI_FORMAT_R16_SINT,                   // R16_SINT

        DXGI_FORMAT_R8_UNORM,                   // R8_UNORM
        DXGI_FORMAT_R8_UINT,                    // R8_UINT
        DXGI_FORMAT_R8_SNORM,                   // R8_SNORM
        DXGI_FORMAT_R8_SINT,                    // R8_SINT

        DXGI_FORMAT_R9G9B9E5_SHAREDEXP,         // R9G9B9E5_UFLOAT

        DXGI_FORMAT_R8G8_B8G8_UNORM,            // R8G8B8G8_UNORM
        DXGI_FORMAT_G8R8_G8B8_UNORM,            // G8R8G8B8_UNORM

        DXGI_FORMAT_BC1_UNORM,                  // BC1_UNORM
        DXGI_FORMAT_BC1_UNORM_SRGB,             // BC1_UNORM_SRGB

        DXGI_FORMAT_BC2_UNORM,                  // BC2_UNORM
        DXGI_FORMAT_BC2_UNORM_SRGB,             // BC2_UNORM_SRGB

        DXGI_FORMAT_BC3_UNORM,                  // BC3_UNORM
        DXGI_FORMAT_BC3_UNORM_SRGB,             // BC3_UNORM_SRGB

        DXGI_FORMAT_BC4_UNORM,                  // BC4_UNORM
        DXGI_FORMAT_BC4_SNORM,                  // BC4_SNORM

        DXGI_FORMAT_BC5_UNORM,                  // BC5_UNORM
        DXGI_FORMAT_BC5_SNORM,                  // BC5_SNORM

        DXGI_FORMAT_B5G6R5_UNORM,               // B5G6R5_UNORM

        DXGI_FORMAT_B5G5R5A1_UNORM,             // B5G5R5A1_UNORM

        DXGI_FORMAT_B8G8R8A8_UNORM,             // B8G8R8A8_UNORM
        DXGI_FORMAT_B8G8R8A8_UNORM_SRGB,        // B8G8R8A8_SRGB

        DXGI_FORMAT_BC6H_UF16,                  // BC6H_UFLOAT
        DXGI_FORMAT_BC6H_SF16,                  // BC6H_SFLOAT

        DXGI_FORMAT_BC7_UNORM,                  // BC7_UNORM
        DXGI_FORMAT_BC7_UNORM_SRGB,             // BC7_SRGB

        DXGI_FORMAT_NV12,                       // NV12
        DXGI_FORMAT_YUY2,                       // YUY2

        DXGI_FORMAT_B4G4R4A4_UNORM,             // B4G4R4A4_UNORM
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DXGI_FORMAT AsDxgiFormat(this GraphicsFormat format)
    {
        Assert(AssertionsEnabled && (s_dxgiFormatMap.Length == Enum.GetValues<GraphicsFormat>().Length));
        return s_dxgiFormatMap[(uint)format];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReleaseIfNotNull<TUnknown>(Pointer<TUnknown> unknown)
        where TUnknown : unmanaged => ReleaseIfNotNull(unknown.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReleaseIfNotNull<TUnknown>(TUnknown* unknown)
        where TUnknown : unmanaged
    {
        if (unknown != null)
        {
            _ = ((IUnknown*)unknown)->Release();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowExternalExceptionIfFailed(HRESULT value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value.FAILED)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, value);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetD3D12Name<TD3D12Object>(ref this TD3D12Object self, string name, [CallerArgumentExpression("self")] string component = "")
        where TD3D12Object : unmanaged, ID3D12Object.Interface
    {
        if (GraphicsService.EnableDebugMode)
        {
            var componentName = $"{name}: {component}";

            fixed (char* pName = componentName)
            {
                _ = self.SetName((ushort*)pName);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetDxgiName<TDXGIObject>(ref this TDXGIObject self, string name, [CallerArgumentExpression("self")] string component = "")
        where TDXGIObject : unmanaged, IDXGIObject.Interface
    {
        if (GraphicsService.EnableDebugMode)
        {
            var componentName = $"{name}: {component}";

            fixed (char* pName = componentName)
            {
                _ = self.SetPrivateData(AsReadonlyPointer(in WKPDID_D3DDebugObjectName), (uint)componentName.Length, (ushort*)pName);
            }
        }
    }
}
