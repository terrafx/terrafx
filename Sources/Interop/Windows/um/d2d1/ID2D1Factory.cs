// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The root factory interface for all of D2D's objects.</summary>
    [Guid("06152247-6F50-465A-9245-118BFD3B6007")]
    public /* blittable */ unsafe struct ID2D1Factory
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Cause the factory to refresh any system metrics that it might have been snapped on factory creation.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ReloadSystemMetrics(
            [In] ID2D1Factory* This
        );

        /// <summary>Retrieves the current desktop DPI. To refresh this, call ReloadSystemMetrics.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDesktopDpi(
            [In] ID2D1Factory* This,
            [Out, ComAliasName("FLOAT")] float* dpiX,
            [Out, ComAliasName("FLOAT")] float* dpiY
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateRectangleGeometry(
            [In] ID2D1Factory* This,
            [In, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* rectangle,
            [Out] ID2D1RectangleGeometry** rectangleGeometry
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateRoundedRectangleGeometry(
            [In] ID2D1Factory* This,
            [In] /* readonly */ D2D1_ROUNDED_RECT* roundedRectangle,
            [Out] ID2D1RoundedRectangleGeometry** roundedRectangleGeometry
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateEllipseGeometry(
            [In] ID2D1Factory* This,
            [In] /* readonly */ D2D1_ELLIPSE* ellipse,
            [Out] ID2D1EllipseGeometry** ellipseGeometry
        );

        /// <summary>Create a geometry which holds other geometries.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateGeometryGroup(
            [In] ID2D1Factory* This,
            [In] D2D1_FILL_MODE fillMode,
            [In] ID2D1Geometry** geometries,
            [In, ComAliasName("UINT32")] uint geometriesCount,
            [Out] ID2D1GeometryGroup** geometryGroup
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateTransformedGeometry(
            [In] ID2D1Factory* This,
            [In] ID2D1Geometry* sourceGeometry,
            [In, ComAliasName("D2D1_MATRIX_3X2_F")] /* readonly */ D2D_MATRIX_3X2_F* transform,
            [Out] ID2D1TransformedGeometry** transformedGeometry
        );

        /// <summary>Returns an initially empty path geometry interface. A geometry sink is created off the interface to populate it.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreatePathGeometry(
            [In] ID2D1Factory* This,
            [Out] ID2D1PathGeometry** pathGeometry
        );

        /// <summary>Allows a non-default stroke style to be specified for a given geometry at draw time.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateStrokeStyle(
            [In] ID2D1Factory* This,
            [In] /* readonly */ D2D1_STROKE_STYLE_PROPERTIES* strokeStyleProperties,
            [In, Optional, ComAliasName("FLOAT")] /* readonly */ float* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [Out] ID2D1StrokeStyle** strokeStyle
        );

        /// <summary>Creates a new drawing state block, this can be used in subsequent SaveDrawingState and RestoreDrawingState operations on the render target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDrawingStateBlock(
            [In] ID2D1Factory* This,
            [In, Optional] /* readonly */ D2D1_DRAWING_STATE_DESCRIPTION* drawingStateDescription,
            [In, Optional] IDWriteRenderingParams* textRenderingParams,
            [Out] ID2D1DrawingStateBlock** drawingStateBlock
        );

        /// <summary>Creates a render target which is a source of bitmaps.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateWicBitmapRenderTarget(
            [In] ID2D1Factory* This,
            [In] IWICBitmap* target,
            [In] /* readonly */ D2D1_RENDER_TARGET_PROPERTIES* renderTargetProperties,
            [Out] ID2D1RenderTarget** renderTarget
        );

        /// <summary>Creates a render target that appears on the display.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateHwndRenderTarget(
            [In] ID2D1Factory* This,
            [In] /* readonly */ D2D1_RENDER_TARGET_PROPERTIES* renderTargetProperties,
            [In] /* readonly */ D2D1_HWND_RENDER_TARGET_PROPERTIES* hwndRenderTargetProperties,
            [Out] ID2D1HwndRenderTarget** hwndRenderTarget
        );

        /// <summary>Creates a render target that draws to a DXGI Surface. The device that owns the surface is used for rendering.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDxgiSurfaceRenderTarget(
            [In] ID2D1Factory* This,
            [In] IDXGISurface* dxgiSurface,
            [In] /* readonly */ D2D1_RENDER_TARGET_PROPERTIES* renderTargetProperties,
            [Out] ID2D1RenderTarget** renderTarget
        );

        /// <summary>Creates a render target that draws to a GDI device context.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDCRenderTarget(
            [In] ID2D1Factory* This,
            [In] /* readonly */ D2D1_RENDER_TARGET_PROPERTIES* renderTargetProperties,
            [Out] ID2D1DCRenderTarget** dcRenderTarget
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr ReloadSystemMetrics;

            public IntPtr GetDesktopDpi;

            public IntPtr CreateRectangleGeometry;

            public IntPtr CreateRoundedRectangleGeometry;

            public IntPtr CreateEllipseGeometry;

            public IntPtr CreateGeometryGroup;

            public IntPtr CreateTransformedGeometry;

            public IntPtr CreatePathGeometry;

            public IntPtr CreateStrokeStyle;

            public IntPtr CreateDrawingStateBlock;

            public IntPtr CreateWicBitmapRenderTarget;

            public IntPtr CreateHwndRenderTarget;

            public IntPtr CreateDxgiSurfaceRenderTarget;

            public IntPtr CreateDCRenderTarget;
            #endregion
        }
        #endregion
    }
}
