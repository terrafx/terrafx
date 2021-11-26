// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.	

using System.Runtime.Versioning;
using TerraFX.Graphics;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.DirectX.DXGI_FORMAT;
using static TerraFX.Utilities.ExceptionUtilities;

[assembly: SupportedOSPlatform("windows10.0")]

namespace TerraFX.Utilities;

internal static unsafe partial class D3D12Utilities
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

    public static void ReleaseIfNotNull<TUnknown>(Pointer<TUnknown> unknown)
        where TUnknown : unmanaged => ReleaseIfNotNull(unknown.Value);

    public static void ReleaseIfNotNull<TUnknown>(TUnknown* unknown)
        where TUnknown : unmanaged
    {
        if (unknown != null)
        {
            _ = ((IUnknown*)unknown)->Release();
        }
    }

    public static void ThrowExternalExceptionIfFailed(HRESULT hr, string methodName)
    {
        if (hr.FAILED)
        {
            ThrowExternalException(methodName, hr);
        }
    }
}
