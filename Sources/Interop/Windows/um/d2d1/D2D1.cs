// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Interop.Windows;

namespace TerraFX.Interop
{
    unsafe public static partial class D2D1
    {
        #region Constants
        public const ulong D2D1_INVALID_TAG = ULONGLONG_MAX;

        public const float D2D1_DEFAULT_FLATTENING_TOLERANCE = 0.25f;

        #region D2D1_INTERPOLATION_MODE_*
        // This defines the superset of interpolation mode supported by D2D APIs and built-in effects

        public const int D2D1_INTERPOLATION_MODE_DEFINITION_NEAREST_NEIGHBOR = 0;

        public const int D2D1_INTERPOLATION_MODE_DEFINITION_LINEAR = 1;

        public const int D2D1_INTERPOLATION_MODE_DEFINITION_CUBIC = 2;

        public const int D2D1_INTERPOLATION_MODE_DEFINITION_MULTI_SAMPLE_LINEAR = 3;

        public const int D2D1_INTERPOLATION_MODE_DEFINITION_ANISOTROPIC = 4;

        public const int D2D1_INTERPOLATION_MODE_DEFINITION_HIGH_QUALITY_CUBIC = 5;

        public const int D2D1_INTERPOLATION_MODE_DEFINITION_FANT = 6;

        public const int D2D1_INTERPOLATION_MODE_DEFINITION_MIPMAP_LINEAR = 7;
        #endregion
        #endregion

        #region Methods
        [DllImport("D2D1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D2D1CreateFactory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern HRESULT D2D1CreateFactory(
            [In] D2D1_FACTORY_TYPE factoryType,
            [In] REFIID riid,
            [In, Optional] /* readonly */ D2D1_FACTORY_OPTIONS* pFactoryOptions,
            [Out] void** ppIFactory
        );

        [DllImport("D2D1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D2D1MakeRotateMatrix", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void D2D1MakeRotateMatrix(
            [In] FLOAT angle,
            [In] D2D1_POINT_2F center,
            [Out] D2D1_MATRIX_3X2_F* matrix
        );

        [DllImport("D2D1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D2D1MakeSkewMatrix", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void D2D1MakeSkewMatrix(
            [In] FLOAT angleX,
            [In] FLOAT angleY,
            [In] D2D1_POINT_2F center,
            [Out] D2D1_MATRIX_3X2_F* matrix
        );

        [DllImport("D2D1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D2D1IsMatrixInvertible", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern BOOL D2D1IsMatrixInvertible(
            [In] /* readonly */ D2D1_MATRIX_3X2_F* matrix
        );

        [DllImport("D2D1", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "D2D1InvertMatrix", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern BOOL D2D1InvertMatrix(
            [In, Out] D2D1_MATRIX_3X2_F* matrix
        );
        #endregion
    }
}
