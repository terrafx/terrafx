// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>This interface performs all the same functions as the ID2D1DeviceContext1 interface, plus it enables functionality such as ink rendering, gradient mesh rendering, and improved image loading.</summary>
    [Guid("394EA6A3-0C34-4321-950B-6CA20F0BE6C7")]
    unsafe public /* blittable */ struct ID2D1DeviceContext2
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateInk(
            [In] ID2D1DeviceContext2* This,
            [In] /* readonly */ D2D1_INK_POINT* startPoint,
            [Out] ID2D1Ink** ink
        );

        /// <summary>Creates a new ink style.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateInkStyle(
            [In] ID2D1DeviceContext2* This,
            [In, Optional] /* readonly */ D2D1_INK_STYLE_PROPERTIES* inkStyleProperties,
            [Out] ID2D1InkStyle** inkStyle
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateGradientMesh(
            [In] ID2D1DeviceContext2* This,
            [In] /* readonly */ D2D1_GRADIENT_MESH_PATCH* patches,
            [In] UINT32 patchesCount,
            [Out] ID2D1GradientMesh** gradientMesh
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateImageSourceFromWic(
            [In] ID2D1DeviceContext2* This,
            [In] IWICBitmapSource* wicBitmapSource,
            [In] D2D1_IMAGE_SOURCE_LOADING_OPTIONS loadingOptions,
            [In] D2D1_ALPHA_MODE alphaMode,
            [Out] ID2D1ImageSourceFromWic** imageSource
        );

        /// <summary>Creates a 3D lookup table for mapping a 3-channel input to a 3-channel output. The table data must be provided in 4-channel format.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateLookupTable3D(
            [In] ID2D1DeviceContext2* This,
            [In] D2D1_BUFFER_PRECISION precision,
            [In] /* readonly */ UINT32* extents,
            [In] /* readonly */ BYTE* data,
            [In] UINT32 dataCount,
            [In] /* readonly */ UINT32* strides,
            [Out] ID2D1LookupTable3D** lookupTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateImageSourceFromDxgi(
            [In] ID2D1DeviceContext2* This,
            [In] IDXGISurface** surfaces,
            [In] UINT32 surfaceCount,
            [In] DXGI_COLOR_SPACE_TYPE colorSpace,
            [In] D2D1_IMAGE_SOURCE_FROM_DXGI_OPTIONS options,
            [Out] ID2D1ImageSource** imageSource
        );

        /// <summary>Retrieves the world-space bounds in DIPs of the gradient mesh using the device context DPI.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetGradientMeshWorldBounds(
            [In] ID2D1DeviceContext2* This,
            [In] ID2D1GradientMesh* gradientMesh,
            [Out] D2D1_RECT_F* pBounds
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawInk(
            [In] ID2D1DeviceContext2* This,
            [In] ID2D1Ink* ink,
            [In] ID2D1Brush* brush,
            [In, Optional] ID2D1InkStyle* inkStyle
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawGradientMesh(
            [In] ID2D1DeviceContext2* This,
            [In] ID2D1GradientMesh* gradientMesh
        );

        /// <summary>Draw a metafile to the device context.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawGdiMetafile(
            [In] ID2D1DeviceContext2* This,
            [In] ID2D1GdiMetafile* gdiMetafile,
            [In, Optional] /* readonly */ D2D1_RECT_F* destinationRectangle,
            [In] /* readonly */ D2D1_RECT_F* sourceRectangle = null
        );

        /// <summary>Creates an image source which shares resources with an original.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateTransformedImageSource(
            [In] ID2D1DeviceContext2* This,
            [In] ID2D1ImageSource* imageSource,
            [In] /* readonly */ D2D1_TRANSFORMED_IMAGE_SOURCE_PROPERTIES* properties,
            [Out] ID2D1TransformedImageSource** transformedImageSource
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1DeviceContext1.Vtbl BaseVtbl;

            public CreateInk CreateInk;

            public CreateInkStyle CreateInkStyle;

            public CreateGradientMesh CreateGradientMesh;

            public CreateImageSourceFromWic CreateImageSourceFromWic;

            public CreateLookupTable3D CreateLookupTable3D;

            public CreateImageSourceFromDxgi CreateImageSourceFromDxgi;

            public GetGradientMeshWorldBounds GetGradientMeshWorldBounds;

            public DrawInk DrawInk;

            public DrawGradientMesh DrawGradientMesh;

            public DrawGdiMetafile DrawGdiMetafile;

            public CreateTransformedImageSource CreateTransformedImageSource;
            #endregion
        }
        #endregion
    }
}
