// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>This interface performs all the same functions as the ID2D1DeviceContext1 interface, plus it enables functionality such as ink rendering, gradient mesh rendering, and improved image loading.</summary>
    [Guid("394EA6A3-0C34-4321-950B-6CA20F0BE6C7")]
    public /* blittable */ unsafe struct ID2D1DeviceContext2
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateInk(
            [In] ID2D1DeviceContext2* This,
            [In] D2D1_INK_POINT* startPoint,
            [Out] ID2D1Ink** ink
        );

        /// <summary>Creates a new ink style.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateInkStyle(
            [In] ID2D1DeviceContext2* This,
            [In, Optional] D2D1_INK_STYLE_PROPERTIES* inkStyleProperties,
            [Out] ID2D1InkStyle** inkStyle
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateGradientMesh(
            [In] ID2D1DeviceContext2* This,
            [In, ComAliasName("D2D1_GRADIENT_MESH_PATCH[]")] D2D1_GRADIENT_MESH_PATCH* patches,
            [In, ComAliasName("UINT32")] uint patchesCount,
            [Out] ID2D1GradientMesh** gradientMesh
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateImageSourceFromWic(
            [In] ID2D1DeviceContext2* This,
            [In] IWICBitmapSource* wicBitmapSource,
            [In] D2D1_IMAGE_SOURCE_LOADING_OPTIONS loadingOptions,
            [In] D2D1_ALPHA_MODE alphaMode,
            [Out] ID2D1ImageSourceFromWic** imageSource
        );

        /// <summary>Creates a 3D lookup table for mapping a 3-channel input to a 3-channel output. The table data must be provided in 4-channel format.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateLookupTable3D(
            [In] ID2D1DeviceContext2* This,
            [In] D2D1_BUFFER_PRECISION precision,
            [In, ComAliasName("UINT32[]")] uint* extents,
            [In, ComAliasName("BYTE[]")] byte* data,
            [In, ComAliasName("UINT32")] uint dataCount,
            [In, ComAliasName("UINT32[]")] uint* strides,
            [Out] ID2D1LookupTable3D** lookupTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateImageSourceFromDxgi(
            [In] ID2D1DeviceContext2* This,
            [In, ComAliasName("IDXGISurface*[]")] IDXGISurface** surfaces,
            [In, ComAliasName("UINT32")] uint surfaceCount,
            [In] DXGI_COLOR_SPACE_TYPE colorSpace,
            [In] D2D1_IMAGE_SOURCE_FROM_DXGI_OPTIONS options,
            [Out] ID2D1ImageSource** imageSource
        );

        /// <summary>Retrieves the world-space bounds in DIPs of the gradient mesh using the device context DPI.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGradientMeshWorldBounds(
            [In] ID2D1DeviceContext2* This,
            [In] ID2D1GradientMesh* gradientMesh,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* pBounds
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawInk(
            [In] ID2D1DeviceContext2* This,
            [In] ID2D1Ink* ink,
            [In] ID2D1Brush* brush,
            [In] ID2D1InkStyle* inkStyle = null
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
            [In, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* destinationRectangle = null,
            [In, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* sourceRectangle = null
        );

        /// <summary>Creates an image source which shares resources with an original.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateTransformedImageSource(
            [In] ID2D1DeviceContext2* This,
            [In] ID2D1ImageSource* imageSource,
            [In] D2D1_TRANSFORMED_IMAGE_SOURCE_PROPERTIES* properties,
            [Out] ID2D1TransformedImageSource** transformedImageSource
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1DeviceContext1.Vtbl BaseVtbl;

            public IntPtr CreateInk;

            public IntPtr CreateInkStyle;

            public IntPtr CreateGradientMesh;

            public IntPtr CreateImageSourceFromWic;

            public IntPtr CreateLookupTable3D;

            public IntPtr CreateImageSourceFromDxgi;

            public IntPtr GetGradientMeshWorldBounds;

            public IntPtr DrawInk;

            public IntPtr DrawGradientMesh;

            public IntPtr DrawGdiMetafile;

            public IntPtr CreateTransformedImageSource;
            #endregion
        }
        #endregion
    }
}
