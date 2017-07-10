// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Caller-supplied implementation of an interface to receive the recorded command list.</summary>
    [Guid("54D7898A-A061-40A7-BEC7-E465BCBA2C4F")]
    unsafe public /* blittable */ struct ID2D1CommandSink
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT BeginDraw(
            [In] ID2D1CommandSink* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT EndDraw(
            [In] ID2D1CommandSink* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetAntialiasMode(
            [In] ID2D1CommandSink* This,
            [In] D2D1_ANTIALIAS_MODE antialiasMode 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetTags(
            [In] ID2D1CommandSink* This,
            [In] D2D1_TAG tag1,
            [In] D2D1_TAG tag2 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetTextAntialiasMode(
            [In] ID2D1CommandSink* This,
            [In] D2D1_TEXT_ANTIALIAS_MODE textAntialiasMode 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetTextRenderingParams(
            [In] ID2D1CommandSink* This,
            [In, Optional] IDWriteRenderingParams *textRenderingParams 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetTransform(
            [In] ID2D1CommandSink* This,
            [In] /* readonly */ D2D1_MATRIX_3X2_F *transform 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetPrimitiveBlend(
            [In] ID2D1CommandSink* This,
            [In] D2D1_PRIMITIVE_BLEND primitiveBlend 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetUnitMode(
            [In] ID2D1CommandSink* This,
            [In] D2D1_UNIT_MODE unitMode 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Clear(
            [In] ID2D1CommandSink* This,
            [In, Optional] /* readonly */ D2D1_COLOR_F *color 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DrawGlyphRun(
            [In] ID2D1CommandSink* This,
            [In] D2D1_POINT_2F baselineOrigin,
            [In] /* readonly */ DWRITE_GLYPH_RUN *glyphRun,
            [In, Optional] /* readonly */ DWRITE_GLYPH_RUN_DESCRIPTION *glyphRunDescription,
            [In] ID2D1Brush *foregroundBrush,
            [In] DWRITE_MEASURING_MODE measuringMode 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DrawLine(
            [In] ID2D1CommandSink* This,
            [In] D2D1_POINT_2F point0,
            [In] D2D1_POINT_2F point1,
            [In] ID2D1Brush *brush,
            [In] FLOAT strokeWidth,
            [In, Optional] ID2D1StrokeStyle *strokeStyle 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DrawGeometry(
            [In] ID2D1CommandSink* This,
            [In] ID2D1Geometry *geometry,
            [In] ID2D1Brush *brush,
            [In] FLOAT strokeWidth,
            [In, Optional] ID2D1StrokeStyle *strokeStyle 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DrawRectangle(
            [In] ID2D1CommandSink* This,
            [In] /* readonly */ D2D1_RECT_F *rect,
            [In] ID2D1Brush *brush,
            [In] FLOAT strokeWidth,
            [In, Optional] ID2D1StrokeStyle *strokeStyle 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DrawBitmap(
            [In] ID2D1CommandSink* This,
            [In] ID2D1Bitmap *bitmap,
            [In, Optional] /* readonly */ D2D1_RECT_F *destinationRectangle,
            [In] FLOAT opacity,
            [In] D2D1_INTERPOLATION_MODE interpolationMode,
            [In, Optional] /* readonly */ D2D1_RECT_F *sourceRectangle,
            [In, Optional] /* readonly */ D2D1_MATRIX_4X4_F *perspectiveTransform 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DrawImage(
            [In] ID2D1CommandSink* This,
            [In] ID2D1Image *image,
            [In, Optional] /* readonly */ D2D1_POINT_2F *targetOffset,
            [In, Optional] /* readonly */ D2D1_RECT_F *imageRectangle,
            [In] D2D1_INTERPOLATION_MODE interpolationMode,
            [In] D2D1_COMPOSITE_MODE compositeMode 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DrawGdiMetafile(
            [In] ID2D1CommandSink* This,
            [In] ID2D1GdiMetafile *gdiMetafile,
            [In, Optional] /* readonly */ D2D1_POINT_2F *targetOffset 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT FillMesh(
            [In] ID2D1CommandSink* This,
            [In] ID2D1Mesh *mesh,
            [In] ID2D1Brush *brush 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT FillOpacityMask(
            [In] ID2D1CommandSink* This,
            [In] ID2D1Bitmap *opacityMask,
            [In] ID2D1Brush *brush,
            [In, Optional] /* readonly */ D2D1_RECT_F *destinationRectangle,
            [In, Optional] /* readonly */ D2D1_RECT_F *sourceRectangle 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT FillGeometry(
            [In] ID2D1CommandSink* This,
            [In] ID2D1Geometry *geometry,
            [In] ID2D1Brush *brush,
            [In, Optional] ID2D1Brush *opacityBrush 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT FillRectangle(
            [In] ID2D1CommandSink* This,
            [In] /* readonly */ D2D1_RECT_F *rect,
            [In] ID2D1Brush *brush 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushAxisAlignedClip(
            [In] ID2D1CommandSink* This,
            [In] /* readonly */ D2D1_RECT_F *clipRect,
            [In] D2D1_ANTIALIAS_MODE antialiasMode 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushLayer(
            [In] ID2D1CommandSink* This,
            [In] /* readonly */ D2D1_LAYER_PARAMETERS1 *layerParameters1,
            [In, Optional] ID2D1Layer *layer 
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PopAxisAlignedClip(
            [In] ID2D1CommandSink* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PopLayer(
            [In] ID2D1CommandSink* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public BeginDraw BeginDraw;

            public EndDraw EndDraw;

            public SetAntialiasMode SetAntialiasMode;

            public SetTags SetTags;

            public SetTextAntialiasMode SetTextAntialiasMode;

            public SetTextRenderingParams SetTextRenderingParams;

            public SetTransform SetTransform;

            public SetPrimitiveBlend SetPrimitiveBlend;

            public SetUnitMode SetUnitMode;

            public Clear Clear;

            public DrawGlyphRun DrawGlyphRun;

            public DrawLine DrawLine;

            public DrawGeometry DrawGeometry;

            public DrawRectangle DrawRectangle;

            public DrawBitmap DrawBitmap;

            public DrawImage DrawImage;

            public DrawGdiMetafile DrawGdiMetafile;

            public FillMesh FillMesh;

            public FillOpacityMask FillOpacityMask;

            public FillGeometry FillGeometry;

            public FillRectangle FillRectangle;

            public PushAxisAlignedClip PushAxisAlignedClip;

            public PushLayer PushLayer;

            public PopAxisAlignedClip PopAxisAlignedClip;

            public PopLayer PopLayer;
            #endregion
        }
        #endregion
    }
}
