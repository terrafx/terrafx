// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Creates Direct2D resources. This interface also enables the creation of ID2D1Device1 objects.</summary>
    [Guid("94F81A73-9212-4376-9C58-B16A3A0D3992")]
    public /* blittable */ unsafe struct ID2D1Factory2
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] ID2D1Factory2* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] ID2D1Factory2* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] ID2D1Factory2* This
        );
        #endregion

        #region ID2D1Factory Delegates
        /// <summary>Cause the factory to refresh any system metrics that it might have been snapped on factory creation.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ReloadSystemMetrics(
            [In] ID2D1Factory2* This
        );

        /// <summary>Retrieves the current desktop DPI. To refresh this, call ReloadSystemMetrics.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDesktopDpi(
            [In] ID2D1Factory2* This,
            [Out, ComAliasName("FLOAT")] float* dpiX,
            [Out, ComAliasName("FLOAT")] float* dpiY
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateRectangleGeometry(
            [In] ID2D1Factory2* This,
            [In, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* rectangle,
            [Out] ID2D1RectangleGeometry** rectangleGeometry
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateRoundedRectangleGeometry(
            [In] ID2D1Factory2* This,
            [In] D2D1_ROUNDED_RECT* roundedRectangle,
            [Out] ID2D1RoundedRectangleGeometry** roundedRectangleGeometry
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateEllipseGeometry(
            [In] ID2D1Factory2* This,
            [In] D2D1_ELLIPSE* ellipse,
            [Out] ID2D1EllipseGeometry** ellipseGeometry
        );

        /// <summary>Create a geometry which holds other geometries.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateGeometryGroup(
            [In] ID2D1Factory2* This,
            [In] D2D1_FILL_MODE fillMode,
            [In, ComAliasName("ID2D1Geometry*[]")] ID2D1Geometry** geometries,
            [In, ComAliasName("UINT32")] uint geometriesCount,
            [Out] ID2D1GeometryGroup** geometryGroup
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateTransformedGeometry(
            [In] ID2D1Factory2* This,
            [In] ID2D1Geometry* sourceGeometry,
            [In, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* transform,
            [Out] ID2D1TransformedGeometry** transformedGeometry
        );

        /// <summary>Returns an initially empty path geometry interface. A geometry sink is created off the interface to populate it.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreatePathGeometry(
            [In] ID2D1Factory2* This,
            [Out] ID2D1PathGeometry** pathGeometry
        );

        /// <summary>Allows a non-default stroke style to be specified for a given geometry at draw time.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateStrokeStyle(
            [In] ID2D1Factory2* This,
            [In] D2D1_STROKE_STYLE_PROPERTIES* strokeStyleProperties,
            [In, Optional, ComAliasName("FLOAT[]")] float* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [Out] ID2D1StrokeStyle** strokeStyle
        );

        /// <summary>Creates a new drawing state block, this can be used in subsequent SaveDrawingState and RestoreDrawingState operations on the render target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDrawingStateBlock(
            [In] ID2D1Factory2* This,
            [In, Optional] D2D1_DRAWING_STATE_DESCRIPTION* drawingStateDescription,
            [In, Optional] IDWriteRenderingParams* textRenderingParams,
            [Out] ID2D1DrawingStateBlock** drawingStateBlock
        );

        /// <summary>Creates a render target which is a source of bitmaps.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateWicBitmapRenderTarget(
            [In] ID2D1Factory2* This,
            [In] IWICBitmap* target,
            [In] D2D1_RENDER_TARGET_PROPERTIES* renderTargetProperties,
            [Out] ID2D1RenderTarget** renderTarget
        );

        /// <summary>Creates a render target that appears on the display.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateHwndRenderTarget(
            [In] ID2D1Factory2* This,
            [In] D2D1_RENDER_TARGET_PROPERTIES* renderTargetProperties,
            [In] D2D1_HWND_RENDER_TARGET_PROPERTIES* hwndRenderTargetProperties,
            [Out] ID2D1HwndRenderTarget** hwndRenderTarget
        );

        /// <summary>Creates a render target that draws to a DXGI Surface. The device that owns the surface is used for rendering.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDxgiSurfaceRenderTarget(
            [In] ID2D1Factory2* This,
            [In] IDXGISurface* dxgiSurface,
            [In] D2D1_RENDER_TARGET_PROPERTIES* renderTargetProperties,
            [Out] ID2D1RenderTarget** renderTarget
        );

        /// <summary>Creates a render target that draws to a GDI device context.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDCRenderTarget(
            [In] ID2D1Factory2* This,
            [In] D2D1_RENDER_TARGET_PROPERTIES* renderTargetProperties,
            [Out] ID2D1DCRenderTarget** dcRenderTarget
        );
        #endregion

        #region ID2D1Factory1 Delegates
        /// <summary>This creates a new Direct2D device from the given IDXGIDevice.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDevice(
            [In] ID2D1Factory2* This,
            [In] IDXGIDevice* dxgiDevice,
            [Out] ID2D1Device** d2dDevice
        );

        /// <summary>This creates a stroke style with the ability to preserve stroke width in various ways.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateStrokeStyle1(
            [In] ID2D1Factory2* This,
            [In] D2D1_STROKE_STYLE_PROPERTIES1* strokeStyleProperties,
            [In, Optional, ComAliasName("FLOAT[]")] float* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [Out] ID2D1StrokeStyle1** strokeStyle
        );

        /// <summary>Creates a path geometry with new operational methods.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreatePathGeometry1(
            [In] ID2D1Factory2* This,
            [Out] ID2D1PathGeometry1** pathGeometry
        );

        /// <summary>Creates a new drawing state block, this can be used in subsequent SaveDrawingState and RestoreDrawingState operations on the render target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDrawingStateBlock1(
            [In] ID2D1Factory2* This,
            [In, Optional] D2D1_DRAWING_STATE_DESCRIPTION1* drawingStateDescription,
            [In, Optional] IDWriteRenderingParams* textRenderingParams,
            [Out] ID2D1DrawingStateBlock1** drawingStateBlock
        );

        /// <summary>Creates a new GDI metafile.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateGdiMetafile(
            [In] ID2D1Factory2* This,
            [In] IStream* metafileStream,
            [Out] ID2D1GdiMetafile** metafile
        );

        /// <summary>This globally registers the given effect. The effect can later be instantiated by using the registered class id. The effect registration is reference counted.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RegisterEffectFromStream(
            [In] ID2D1Factory2* This,
            [In, ComAliasName("REFCLSID")] Guid* classId,
            [In] IStream* propertyXml,
            [In, Optional] D2D1_PROPERTY_BINDING* bindings,
            [In, ComAliasName("UINT32")] uint bindingsCount,
            [In] PD2D1_EFFECT_FACTORY effectFactory
        );

        /// <summary>This globally registers the given effect. The effect can later be instantiated by using the registered class id. The effect registration is reference counted.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RegisterEffectFromString(
            [In] ID2D1Factory2* This,
            [In, ComAliasName("REFCLSID")] Guid* classId,
            [In, ComAliasName("PCWSTR")] char* propertyXml,
            [In, Optional, ComAliasName("D2D1_PROPERTY_BINDING[]")] D2D1_PROPERTY_BINDING* bindings,
            [In, ComAliasName("UINT32")] uint bindingsCount,
            [In] PD2D1_EFFECT_FACTORY effectFactory
        );

        /// <summary>This unregisters the given effect by its class id, you need to call UnregisterEffect for every call to ID2D1Factory1::RegisterEffectFromStream and ID2D1Factory1::RegisterEffectFromString to completely unregister it.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int UnregisterEffect(
            [In] ID2D1Factory2* This,
            [In, ComAliasName("REFCLSID")] Guid* classId
        );

        /// <summary>This returns all of the registered effects in the process, including any built-in effects.</summary>
        /// <param name="effectsReturned">The number of effects returned into the passed in effects array.</param>
        /// <param name="effectsRegistered">The number of effects currently registered in the system.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetRegisteredEffects(
            [In] ID2D1Factory2* This,
            [Out, Optional, ComAliasName("CLSID[]")] Guid* effects,
            [In, ComAliasName("UINT32")] uint effectsCount,
            [Out, ComAliasName("UINT32")] uint* effectsReturned = null,
            [Out, ComAliasName("UINT32")] uint* effectsRegistered = null
        );

        /// <summary>This retrieves the effect properties for the given effect, all of the effect properties will be set to a default value since an effect is not instantiated to implement the returned property interface.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetEffectProperties(
            [In] ID2D1Factory2* This,
            [In, ComAliasName("REFCLSID")] Guid* effectId,
            [Out] ID2D1Properties** properties
        );
        #endregion

        #region Delegates
        /// <summary>This creates a new Direct2D device from the given IDXGIDevice.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDevice1(
            [In] ID2D1Factory2* This,
            [In] IDXGIDevice* dxgiDevice,
            [Out] ID2D1Device1** d2dDevice1
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region ID2D1Factory Fields
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

            #region ID2D1Factory1 Fields
            public IntPtr CreateDevice;

            public IntPtr CreateStrokeStyle1;

            public IntPtr CreatePathGeometry1;

            public IntPtr CreateDrawingStateBlock1;

            public IntPtr CreateGdiMetafile;

            public IntPtr RegisterEffectFromStream;

            public IntPtr RegisterEffectFromString;

            public IntPtr UnregisterEffect;

            public IntPtr GetRegisteredEffects;

            public IntPtr GetEffectProperties;
            #endregion

            #region Fields
            public IntPtr CreateDevice1;
            #endregion
        }
        #endregion
    }
}
