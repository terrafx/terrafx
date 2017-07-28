// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents an collection of gradient stops that can then be the source resource for either a linear or radial gradient brush.</summary>
    [Guid("AE1572F4-5DD0-4777-998B-9279472AE63B")]
    unsafe public /* blittable */ struct ID2D1GradientStopCollection1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Copies the gradient stops from the collection into the caller's memory. If this object was created using ID2D1DeviceContext::CreateGradientStopCollection, this method returns the same values as were specified in the creation method. If this object was created using ID2D1RenderTarget::CreateGradientStopCollection, the stops returned here will first be transformed into the gamma space specified by the colorInterpolationGamma parameter.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetGradientStops1(
            [In] ID2D1GradientStopCollection1* This,
            [Out] D2D1_GRADIENT_STOP* gradientStops,
            [In, ComAliasName("UINT32")] uint gradientStopsCount
        );

        /// <summary>Returns the color space in which interpolation occurs. If this object was created using ID2D1RenderTarget::CreateGradientStopCollection, this method returns the color space related to the color interpolation gamma.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_COLOR_SPACE GetPreInterpolationSpace(
            [In] ID2D1GradientStopCollection1* This
        );

        /// <summary>Returns the color space colors will be converted to after interpolation occurs. If this object was created using ID2D1RenderTarget::CreateGradientStopCollection, this method returns D2D1_COLOR_SPACE_SRGB.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_COLOR_SPACE GetPostInterpolationSpace(
            [In] ID2D1GradientStopCollection1* This
        );

        /// <summary>Returns the buffer precision of this gradient. If this object was created using ID2D1RenderTarget::CreateGradientStopCollection, this method returns D2D1_BUFFER_PRECISION_8BPC_UNORM.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_BUFFER_PRECISION GetBufferPrecision(
            [In] ID2D1GradientStopCollection1* This
        );

        /// <summary>Returns the interpolation mode used to interpolate colors in the gradient.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_COLOR_INTERPOLATION_MODE GetColorInterpolationMode(
            [In] ID2D1GradientStopCollection1* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1GradientStopCollection.Vtbl BaseVtbl;

            public GetGradientStops1 GetGradientStops1;

            public GetPreInterpolationSpace GetPreInterpolationSpace;

            public GetPostInterpolationSpace GetPostInterpolationSpace;

            public GetBufferPrecision GetBufferPrecision;

            public GetColorInterpolationMode GetColorInterpolationMode;
            #endregion
        }
        #endregion
    }
}
