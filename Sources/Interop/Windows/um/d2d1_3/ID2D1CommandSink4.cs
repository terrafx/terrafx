// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("C78A6519-40D6-4218-B2DE-BEEEB744BB3E")]
    public /* blittable */ unsafe struct ID2D1CommandSink4
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] ID2D1CommandSink4* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] ID2D1CommandSink4* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] ID2D1CommandSink4* This
        );
        #endregion

        #region ID2D1CommandSink Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int BeginDraw(
            [In] ID2D1CommandSink4* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int EndDraw(
            [In] ID2D1CommandSink4* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetAntialiasMode(
            [In] ID2D1CommandSink4* This,
            [In] D2D1_ANTIALIAS_MODE antialiasMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetTags(
            [In] ID2D1CommandSink4* This,
            [In, ComAliasName("D2D1_TAG")] ulong tag1,
            [In, ComAliasName("D2D1_TAG")] ulong tag2
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetTextAntialiasMode(
            [In] ID2D1CommandSink4* This,
            [In] D2D1_TEXT_ANTIALIAS_MODE textAntialiasMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetTextRenderingParams(
            [In] ID2D1CommandSink4* This,
            [In] IDWriteRenderingParams* textRenderingParams = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetTransform(
            [In] ID2D1CommandSink4* This,
            [In, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* transform
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPrimitiveBlend(
            [In] ID2D1CommandSink4* This,
            [In] D2D1_PRIMITIVE_BLEND primitiveBlend
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetUnitMode(
            [In] ID2D1CommandSink4* This,
            [In] D2D1_UNIT_MODE unitMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Clear(
            [In] ID2D1CommandSink4* This,
            [In, ComAliasName("D2D1_COLOR_F")] DXGI_RGBA* color = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawGlyphRun(
            [In] ID2D1CommandSink4* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F baselineOrigin,
            [In] DWRITE_GLYPH_RUN* glyphRun,
            [In, Optional] DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,
            [In] ID2D1Brush* foregroundBrush,
            [In] DWRITE_MEASURING_MODE measuringMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawLine(
            [In] ID2D1CommandSink4* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F point0,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F point1,
            [In] ID2D1Brush* brush,
            [In, ComAliasName("FLOAT")] float strokeWidth,
            [In] ID2D1StrokeStyle* strokeStyle = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawGeometry(
            [In] ID2D1CommandSink4* This,
            [In] ID2D1Geometry* geometry,
            [In] ID2D1Brush* brush,
            [In, ComAliasName("FLOAT")] float strokeWidth,
            [In] ID2D1StrokeStyle* strokeStyle = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawRectangle(
            [In] ID2D1CommandSink4* This,
            [In, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* rect,
            [In] ID2D1Brush* brush,
            [In, ComAliasName("FLOAT")] float strokeWidth,
            [In] ID2D1StrokeStyle* strokeStyle = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawBitmap(
            [In] ID2D1CommandSink4* This,
            [In] ID2D1Bitmap* bitmap,
            [In, Optional, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* destinationRectangle,
            [In, ComAliasName("FLOAT")] float opacity,
            [In] D2D1_INTERPOLATION_MODE interpolationMode,
            [In, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* sourceRectangle = null,
            [In, ComAliasName("D2D1_MATRIX_4X4_F")] D2D_MATRIX_4X4_F* perspectiveTransform = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawImage(
            [In] ID2D1CommandSink4* This,
            [In] ID2D1Image* image,
            [In, Optional, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F* targetOffset,
            [In, Optional, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* imageRectangle,
            [In] D2D1_INTERPOLATION_MODE interpolationMode,
            [In] D2D1_COMPOSITE_MODE compositeMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawGdiMetafile(
            [In] ID2D1CommandSink4* This,
            [In] ID2D1GdiMetafile* gdiMetafile,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F* targetOffset = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int FillMesh(
            [In] ID2D1CommandSink4* This,
            [In] ID2D1Mesh* mesh,
            [In] ID2D1Brush* brush
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int FillOpacityMask(
            [In] ID2D1CommandSink4* This,
            [In] ID2D1Bitmap* opacityMask,
            [In] ID2D1Brush* brush,
            [In, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* destinationRectangle = null,
            [In, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* sourceRectangle = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int FillGeometry(
            [In] ID2D1CommandSink4* This,
            [In] ID2D1Geometry* geometry,
            [In] ID2D1Brush* brush,
            [In] ID2D1Brush* opacityBrush = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int FillRectangle(
            [In] ID2D1CommandSink4* This,
            [In, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* rect,
            [In] ID2D1Brush* brush
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushAxisAlignedClip(
            [In] ID2D1CommandSink4* This,
            [In, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* clipRect,
            [In] D2D1_ANTIALIAS_MODE antialiasMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushLayer(
            [In] ID2D1CommandSink4* This,
            [In] D2D1_LAYER_PARAMETERS1* layerParameters1,
            [In] ID2D1Layer* layer = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PopAxisAlignedClip(
            [In] ID2D1CommandSink4* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PopLayer(
            [In] ID2D1CommandSink4* This
        );
        #endregion

        #region ID2D1CommandSink1 Delegates
        /// <summary>This method is called if primitiveBlend value was added after Windows 8. SetPrimitiveBlend method is used for Win8 values (_SOURCE_OVER and _COPY).</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPrimitiveBlend1(
            [In] ID2D1CommandSink4* This,
            [In] D2D1_PRIMITIVE_BLEND primitiveBlend
        );
        #endregion

        #region ID2D1CommandSink2 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawInk(
            [In] ID2D1CommandSink4* This,
            [In] ID2D1Ink* ink,
            [In] ID2D1Brush* brush,
            [In] ID2D1InkStyle* inkStyle = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawGradientMesh(
            [In] ID2D1CommandSink4* This,
            [In] ID2D1GradientMesh* gradientMesh
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawGdiMetafile1(
            [In] ID2D1CommandSink4* This,
            [In] ID2D1GdiMetafile* gdiMetafile,
            [In, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* destinationRectangle = null,
            [In, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* sourceRectangle = null
        );
        #endregion

        #region ID2D1CommandSink3 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawSpriteBatch(
            [In] ID2D1CommandSink4* This,
            [In] ID2D1SpriteBatch* spriteBatch,
            [In, ComAliasName("UINT32")] uint startIndex,
            [In, ComAliasName("UINT32")] uint spriteCount,
            [In] ID2D1Bitmap* bitmap,
            [In] D2D1_BITMAP_INTERPOLATION_MODE interpolationMode,
            [In] D2D1_SPRITE_OPTIONS spriteOptions
        );
        #endregion

        #region Delegates
        /// <summary>A new function to set blend mode that respects the new MAX blend. Implementers of SetPrimitiveBlend2 should expect and handle blend mode: D2D1_PRIMITIVE_BLEND_MAX Implementers of SetPrimitiveBlend1 should expect and handle blend modes: D2D1_PRIMITIVE_BLEND_MIN and D2D1_PRIMITIVE_BLEND_ADD Implementers of SetPrimitiveBlend should expect and handle blend modes: D2D1_PRIMITIVE_BLEND_SOURCE_OVER and D2D1_PRIMITIVE_BLEND_COPY</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPrimitiveBlend2(
            [In] ID2D1CommandSink4* This,
            [In] D2D1_PRIMITIVE_BLEND primitiveBlend
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

            #region ID2D1CommandSink Fields
            public IntPtr BeginDraw;

            public IntPtr EndDraw;

            public IntPtr SetAntialiasMode;

            public IntPtr SetTags;

            public IntPtr SetTextAntialiasMode;

            public IntPtr SetTextRenderingParams;

            public IntPtr SetTransform;

            public IntPtr SetPrimitiveBlend;

            public IntPtr SetUnitMode;

            public IntPtr Clear;

            public IntPtr DrawGlyphRun;

            public IntPtr DrawLine;

            public IntPtr DrawGeometry;

            public IntPtr DrawRectangle;

            public IntPtr DrawBitmap;

            public IntPtr DrawImage;

            public IntPtr DrawGdiMetafile;

            public IntPtr FillMesh;

            public IntPtr FillOpacityMask;

            public IntPtr FillGeometry;

            public IntPtr FillRectangle;

            public IntPtr PushAxisAlignedClip;

            public IntPtr PushLayer;

            public IntPtr PopAxisAlignedClip;

            public IntPtr PopLayer;
            #endregion

            #region ID2D1CommandSink1 Fields
            public IntPtr SetPrimitiveBlend1;
            #endregion

            #region ID2D1CommandSink2 Fields
            public IntPtr DrawInk;

            public IntPtr DrawGradientMesh;

            public IntPtr DrawGdiMetafile1;
            #endregion

            #region ID2D1CommandSink3 Fields
            public IntPtr DrawSpriteBatch;
            #endregion

            #region Fields
            public IntPtr SetPrimitiveBlend2;
            #endregion
        }
        #endregion
    }
}
