// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.	

using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics.Providers.D3D12
{
    internal static unsafe partial class HelperUtilities
    {
        public static void ReleaseIfCreated<TUnknown>(ValueLazy<Pointer<TUnknown>> unknown)
            where TUnknown : unmanaged
        {
            if (unknown.IsCreated)
            {
                ReleaseIfNotNull<TUnknown>(unknown.Value);
            }
        }

        public static void ReleaseIfNotNull<TUnknown>(TUnknown* unknown)
            where TUnknown : unmanaged
        {
            if (unknown != null)
            {
                _ = ((IUnknown*)unknown)->Release();
            }
        }

        public static void ThrowExternalExceptionIfFailed(string methodName, int hr)
        {
            if (FAILED(hr))
            {
                ThrowExternalException(methodName, hr);
            }
        }
    }
}
