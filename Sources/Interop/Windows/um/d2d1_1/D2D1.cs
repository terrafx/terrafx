// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    unsafe public static partial class D2D1
    {
        #region Constants
        public const uint D2D1_INVALID_PROPERTY_INDEX = uint.MaxValue;
        #endregion

        #region External Methods
        [DllImport("D2D1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D2D1CreateDevice", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern HRESULT D2D1CreateDevice(
            [In] IDXGIDevice* dxgiDevice,
            [In, Optional] /* readonly */ D2D1_CREATION_PROPERTIES* creationProperties,
            [Out] ID2D1Device** d2dDevice
        );

        [DllImport("D2D1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D2D1CreateDeviceContext", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern HRESULT D2D1CreateDeviceContext(
            [In] IDXGISurface* dxgiSurface,
            [In, Optional] /* readonly */ D2D1_CREATION_PROPERTIES* creationProperties,
            [Out] ID2D1DeviceContext** d2dDeviceContext
        );

        [DllImport("D2D1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D2D1ConvertColorSpace", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern D2D1_COLOR_F D2D1ConvertColorSpace(
            [In] D2D1_COLOR_SPACE sourceColorSpace,
            [In] D2D1_COLOR_SPACE destinationColorSpace,
            [In] /* readonly */ D2D1_COLOR_F* color
        );

        [DllImport("D2D1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D2D1SinCos", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void D2D1SinCos(
          [In] FLOAT angle,
          [Out] FLOAT* s,
          [Out] FLOAT* c
        );


        [DllImport("D2D1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D2D1Tan", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern FLOAT D2D1Tan(
            [In] FLOAT angle
        );

        [DllImport("D2D1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D2D1Vec3Length", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern FLOAT D2D1Vec3Length(
          [In] FLOAT x,
          [In] FLOAT y,
          [In] FLOAT z
        );
        #endregion
    }
}
