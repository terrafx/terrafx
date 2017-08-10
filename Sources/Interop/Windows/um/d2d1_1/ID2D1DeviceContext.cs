// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Interop.D2D1_COMPOSITE_MODE;
using static TerraFX.Interop.D2D1_INTERPOLATION_MODE;
using static TerraFX.Interop.DWRITE_MEASURING_MODE;

namespace TerraFX.Interop
{
    /// <summary>The device context represents a set of state and a command buffer that is used to render to a target bitmap.</summary>
    [Guid("E8F7FE7A-191C-466D-AD95-975678BDA998")]
    unsafe public /* blittable */ struct ID2D1DeviceContext
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Creates a bitmap with extended bitmap properties, potentially from a block of memory.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmap(
            [In] ID2D1DeviceContext* This,
            [In, ComAliasName("D2D1_SIZE_U")] D2D_SIZE_U size,
            [In, Optional] /* readonly */ void* sourceData,
            [In, ComAliasName("UINT32")] uint pitch,
            [In] /* readonly */ D2D1_BITMAP_PROPERTIES1* bitmapProperties,
            [Out] ID2D1Bitmap1** bitmap
        );

        /// <summary>Create a D2D bitmap by copying a WIC bitmap.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFromWicBitmap(
            [In] ID2D1DeviceContext* This,
            [In] IWICBitmapSource* wicBitmapSource,
            [In, Optional] /* readonly */ D2D1_BITMAP_PROPERTIES1* bitmapProperties,
            [Out] ID2D1Bitmap1** bitmap
        );

        /// <summary>Creates a color context from a color space.  If the space is Custom, the context is initialized from the profile/profileSize arguments.  Otherwise the context is initialized with the profile bytes associated with the space and profile/profileSize are ignored.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateColorContext(
            [In] ID2D1DeviceContext* This,
            [In] D2D1_COLOR_SPACE space,
            [In, ComAliasName("BYTE")] /* readonly */ byte* profile,
            [In, ComAliasName("UINT32")] uint profileSize,
            [Out] ID2D1ColorContext** colorContext
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateColorContextFromFilename(
            [In] ID2D1DeviceContext* This,
            [In, ComAliasName("PCWSTR")] /* readonly */ char* filename,
            [Out] ID2D1ColorContext** colorContext
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateColorContextFromWicColorContext(
            [In] ID2D1DeviceContext* This,
            [In] IWICColorContext* wicColorContext,
            [Out] ID2D1ColorContext** colorContext
        );

        /// <summary>Creates a bitmap from a DXGI surface with a set of extended properties.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFromDxgiSurface(
            [In] ID2D1DeviceContext* This,
            [In] IDXGISurface* surface,
            [In, Optional] /* readonly */ D2D1_BITMAP_PROPERTIES1* bitmapProperties,
            [Out] ID2D1Bitmap1** bitmap
        );

        /// <summary>Create a new effect, the effect must either be built in or previously registered through ID2D1Factory1::RegisterEffectFromStream or ID2D1Factory1::RegisterEffectFromString.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateEffect(
            [In] ID2D1DeviceContext* This,
            [In, ComAliasName("REFCLSID")] /* readonly */ Guid* effectId,
            [Out] ID2D1Effect** effect
        );

        /// <summary>A gradient stop collection represents a set of stops in an ideal unit length. This is the source resource for a linear gradient and radial gradient brush.</summary>
        /// <param name="preInterpolationSpace">Specifies both the input color space and the space in which the color interpolation occurs.</param>
        /// <param name="postInterpolationSpace">Specifies the color space colors will be converted to after interpolation occurs.</param>
        /// <param name="bufferPrecision">Specifies the precision in which the gradient buffer will be held.</param>
        /// <param name="extendMode">Specifies how the gradient will be extended outside of the unit length.</param>
        /// <param name="colorInterpolationMode">Determines if colors will be interpolated in straight alpha or premultiplied alpha space.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateGradientStopCollection(
            [In] ID2D1DeviceContext* This,
            [In] /* readonly */ D2D1_GRADIENT_STOP* straightAlphaGradientStops,
            [In, ComAliasName("UINT32")] uint straightAlphaGradientStopsCount,
            [In] D2D1_COLOR_SPACE preInterpolationSpace,
            [In] D2D1_COLOR_SPACE postInterpolationSpace,
            [In] D2D1_BUFFER_PRECISION bufferPrecision,
            [In] D2D1_EXTEND_MODE extendMode,
            [In] D2D1_COLOR_INTERPOLATION_MODE colorInterpolationMode,
            [Out] ID2D1GradientStopCollection1** gradientStopCollection1
        );

        /// <summary>Creates an image brush, the input image can be any type of image, including a bitmap, effect and a command list.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateImageBrush(
            [In] ID2D1DeviceContext* This,
            [In, Optional] ID2D1Image* image,
            [In] /* readonly */ D2D1_IMAGE_BRUSH_PROPERTIES* imageBrushProperties,
            [In, Optional] /* readonly */ D2D1_BRUSH_PROPERTIES* brushProperties,
            [Out] ID2D1ImageBrush** imageBrush
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapBrush(
            [In] ID2D1DeviceContext* This,
            [In, Optional] ID2D1Bitmap* bitmap,
            [In, Optional] /* readonly */ D2D1_BITMAP_BRUSH_PROPERTIES1* bitmapBrushProperties,
            [In, Optional] /* readonly */ D2D1_BRUSH_PROPERTIES* brushProperties,
            [Out] ID2D1BitmapBrush1** bitmapBrush
        );

        /// <summary>Creates a new command list.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCommandList(
            [In] ID2D1DeviceContext* This,
            [Out] ID2D1CommandList** commandList
        );

        /// <summary>Indicates whether the format is supported by D2D.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsDxgiFormatSupported(
            [In] ID2D1DeviceContext* This,
            [In] DXGI_FORMAT format
        );

        /// <summary>Indicates whether the buffer precision is supported by D2D.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsBufferPrecisionSupported(
            [In] ID2D1DeviceContext* This,
            [In] D2D1_BUFFER_PRECISION bufferPrecision
        );

        /// <summary>This retrieves the local-space bounds in DIPs of the current image using the device context DPI.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetImageLocalBounds(
            [In] ID2D1DeviceContext* This,
            [In] ID2D1Image* image,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* localBounds
        );

        /// <summary>This retrieves the world-space bounds in DIPs of the current image using the device context DPI.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetImageWorldBounds(
            [In] ID2D1DeviceContext* This,
            [In] ID2D1Image* image,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* worldBounds
        );

        /// <summary>Retrieves the world-space bounds in DIPs of the glyph run using the device context DPI.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGlyphRunWorldBounds(
            [In] ID2D1DeviceContext* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F baselineOrigin,
            [In] /* readonly */ DWRITE_GLYPH_RUN* glyphRun,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* bounds
        );

        /// <summary>Retrieves the device associated with this device context.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDevice(
            [In] ID2D1DeviceContext* This,
            [Out] ID2D1Device** device
        );

        /// <summary>Sets the target for this device context to point to the given image. The image can be a command list or a bitmap created with the D2D1_BITMAP_OPTIONS_TARGET flag.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetTarget(
            [In] ID2D1DeviceContext* This,
            [In] ID2D1Image* image = null
        );

        /// <summary>Gets the target that this device context is currently pointing to.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetTarget(
            [In] ID2D1DeviceContext* This,
            [Out] ID2D1Image** image
        );

        /// <summary>Sets tuning parameters for internal rendering inside the device context.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetRenderingControls(
            [In] ID2D1DeviceContext* This,
            [In] /* readonly */ D2D1_RENDERING_CONTROLS* renderingControls
        );

        /// <summary>This retrieves the rendering controls currently selected into the device context.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetRenderingControls(
            [In] ID2D1DeviceContext* This,
            [Out] D2D1_RENDERING_CONTROLS* renderingControls
        );

        /// <summary>Changes the primitive blending mode for all of the rendering operations.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetPrimitiveBlend(
            [In] ID2D1DeviceContext* This,
            [In] D2D1_PRIMITIVE_BLEND primitiveBlend
        );

        /// <summary>Returns the primitive blend currently selected into the device context.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_PRIMITIVE_BLEND GetPrimitiveBlend(
            [In] ID2D1DeviceContext* This
        );

        /// <summary>Changes the units used for all of the rendering operations.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetUnitMode(
            [In] ID2D1DeviceContext* This,
            [In] D2D1_UNIT_MODE unitMode
        );

        /// <summary>Returns the unit mode currently set on the device context.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_UNIT_MODE GetUnitMode(
            [In] ID2D1DeviceContext* This
        );

        /// <summary>Draws the glyph run with an extended description to describe the glyphs.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawGlyphRun(
            [In] ID2D1DeviceContext* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F baselineOrigin,
            [In] /* readonly */ DWRITE_GLYPH_RUN* glyphRun,
            [In, Optional] /* readonly */ DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,
            [In] ID2D1Brush* foregroundBrush,
            [In] DWRITE_MEASURING_MODE measuringMode = DWRITE_MEASURING_MODE_NATURAL
        );

        /// <summary>Draw an image to the device context. The image represents either a concrete bitmap or the output of an effect graph.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawImage(
            [In] ID2D1DeviceContext* This,
            [In] ID2D1Image* image,
            [In, ComAliasName("D2D1_POINT_2F")] /* readonly */ D2D_POINT_2F* targetOffset = null,
            [In, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* imageRectangle = null,
            [In] D2D1_INTERPOLATION_MODE interpolationMode = D2D1_INTERPOLATION_MODE_LINEAR,
            [In] D2D1_COMPOSITE_MODE compositeMode = D2D1_COMPOSITE_MODE_SOURCE_OVER
        );

        /// <summary>Draw a metafile to the device context.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawGdiMetafile(
            [In] ID2D1DeviceContext* This,
            [In] ID2D1GdiMetafile* gdiMetafile,
            [In, ComAliasName("D2D1_POINT_2F")] /* readonly */ D2D_POINT_2F* targetOffset = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawBitmap(
            [In] ID2D1DeviceContext* This,
            [In] ID2D1Bitmap* bitmap,
            [In, Optional, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* destinationRectangle,
            [In, ComAliasName("FLOAT")] float opacity,
            [In] D2D1_INTERPOLATION_MODE interpolationMode,
            [In, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* sourceRectangle = null,
            [In, ComAliasName("D2D1_MATRIX_4X4_F")] /* readonly */ D2D_MATRIX_4X4_F* perspectiveTransform = null
        );

        /// <summary>Push a layer on the device context.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void PushLayer(
            [In] ID2D1DeviceContext* This,
            [In] /* readonly */ D2D1_LAYER_PARAMETERS1* layerParameters,
            [In] ID2D1Layer* layer = null
        );

        /// <summary>This indicates that a portion of an effect's input is invalid. This method can be called many times.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int InvalidateEffectInputRectangle(
            [In] ID2D1DeviceContext* This,
            [In] ID2D1Effect* effect,
            [In, ComAliasName("UINT32")] uint input,
            [In, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* inputRectangle
        );

        /// <summary>Gets the number of invalid ouptut rectangles that have accumulated at the effect.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetEffectInvalidRectangleCount(
            [In] ID2D1DeviceContext* This,
            [In] ID2D1Effect* effect,
            [Out, ComAliasName("UINT32")] uint* rectangleCount
        );

        /// <summary>Gets the invalid rectangles that are at the output of the effect.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetEffectInvalidRectangles(
            [In] ID2D1DeviceContext* This,
            [In] ID2D1Effect* effect,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* rectangles,
            [In, ComAliasName("UINT32")] uint rectanglesCount
        );

        /// <summary>Gets the maximum region of each specified input which would be used during a subsequent rendering operation</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetEffectRequiredInputRectangles(
            [In] ID2D1DeviceContext* This,
            [In] ID2D1Effect* renderEffect,
            [In, Optional, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* renderImageRectangle,
            [In] /* readonly */ D2D1_EFFECT_INPUT_DESCRIPTION* inputDescriptions,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* requiredInputRects,
            [In, ComAliasName("UINT32")] uint inputCount
        );

        /// <summary>Fill using the alpha channel of the supplied opacity mask bitmap. The brush opacity will be modulated by the mask. The render target antialiasing mode must be set to aliased.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void FillOpacityMask(
            [In] ID2D1DeviceContext* This,
            [In] ID2D1Bitmap* opacityMask,
            [In] ID2D1Brush* brush,
            [In, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* destinationRectangle = null,
            [In, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* sourceRectangle = null
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1RenderTarget.Vtbl BaseVtbl;

            public IntPtr CreateBitmap;

            public IntPtr CreateBitmapFromWicBitmap;

            public IntPtr CreateColorContext;

            public IntPtr CreateColorContextFromFilename;

            public IntPtr CreateColorContextFromWicColorContext;

            public IntPtr CreateBitmapFromDxgiSurface;

            public IntPtr CreateEffect;

            public IntPtr CreateGradientStopCollection;

            public IntPtr CreateImageBrush;

            public IntPtr CreateBitmapBrush;

            public IntPtr CreateCommandList;

            public IntPtr IsDxgiFormatSupported;

            public IntPtr IsBufferPrecisionSupported;

            public IntPtr GetImageLocalBounds;

            public IntPtr GetImageWorldBounds;

            public IntPtr GetGlyphRunWorldBounds;

            public IntPtr GetDevice;

            public IntPtr SetTarget;

            public IntPtr GetTarget;

            public IntPtr SetRenderingControls;

            public IntPtr GetRenderingControls;

            public IntPtr SetPrimitiveBlend;

            public IntPtr GetPrimitiveBlend;

            public IntPtr SetUnitMode;

            public IntPtr GetUnitMode;

            public IntPtr DrawGlyphRun;

            public IntPtr DrawImage;

            public IntPtr DrawGdiMetafile;

            public IntPtr DrawBitmap;

            public IntPtr PushLayer;

            public IntPtr InvalidateEffectInputRectangle;

            public IntPtr GetEffectInvalidRectangleCount;

            public IntPtr GetEffectInvalidRectangles;

            public IntPtr GetEffectRequiredInputRectangles;

            public IntPtr FillOpacityMask;
            #endregion
        }
        #endregion
    }
}
