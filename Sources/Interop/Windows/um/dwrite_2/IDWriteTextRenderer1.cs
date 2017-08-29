// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The text renderer interface represents a set of application-defined callbacks that perform rendering of text, inline objects, and decorations such as underlines.</summary>
    [Guid("D3E0E934-22A0-427E-AAE4-7D9574B59DB1")]
    public /* blittable */ unsafe struct IDWriteTextRenderer1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IDWriteTextRenderer1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IDWriteTextRenderer1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IDWriteTextRenderer1* This
        );
        #endregion

        #region IDWritePixelSnapping Delegates
        /// <summary>Determines whether pixel snapping is disabled. The recommended default is FALSE, unless doing animation that requires subpixel vertical placement.</summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="isDisabled">Receives TRUE if pixel snapping is disabled or FALSE if it not.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int IsPixelSnappingDisabled(
            [In] IDWriteTextRenderer1* This,
            [In, Optional] void* clientDrawingContext,
            [Out, ComAliasName("BOOL")] int* isDisabled
        );

        /// <summary>Gets the current transform that maps abstract coordinates to DIPs, which may disable pixel snapping upon any rotation or shear.</summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="transform">Receives the transform.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetCurrentTransform(
            [In] IDWriteTextRenderer1* This,
            [In, Optional] void* clientDrawingContext,
            [Out] DWRITE_MATRIX* transform
        );

        /// <summary>Gets the number of physical pixels per DIP. A DIP (device-independent pixel) is 1/96 inch, so the pixelsPerDip value is the number of logical pixels per inch divided by 96 (yielding a value of 1 for 96 DPI and 1.25 for 120).</summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="pixelsPerDip">Receives the number of physical pixels per DIP.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPixelsPerDip(
            [In] IDWriteTextRenderer1* This,
            [In, Optional] void* clientDrawingContext,
            [Out, ComAliasName("FLOAT")] float* pixelsPerDip
        );
        #endregion

        #region IDWriteTextRenderer Delegates
        /// <summary>IDWriteTextLayout::Draw calls this function to instruct the client to render a run of glyphs.</summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="baselineOriginX">X-coordinate of the baseline.</param>
        /// <param name="baselineOriginY">Y-coordinate of the baseline.</param>
        /// <param name="measuringMode">Specifies measuring mode for glyphs in the run. Renderer implementations may choose different rendering modes for given measuring modes, but best results are seen when the rendering mode matches the corresponding measuring mode: DWRITE_RENDERING_MODE_CLEARTYPE_NATURAL for DWRITE_MEASURING_MODE_NATURAL DWRITE_RENDERING_MODE_CLEARTYPE_GDI_CLASSIC for DWRITE_MEASURING_MODE_GDI_CLASSIC DWRITE_RENDERING_MODE_CLEARTYPE_GDI_NATURAL for DWRITE_MEASURING_MODE_GDI_NATURAL</param>
        /// <param name="glyphRun">The glyph run to draw.</param>
        /// <param name="glyphRunDescription">Properties of the characters associated with this run.</param>
        /// <param name="clientDrawingEffect">The drawing effect set in IDWriteTextLayout::SetDrawingEffect.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawGlyphRun(
            [In] IDWriteTextRenderer1* This,
            [In, Optional] void* clientDrawingContext,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In] DWRITE_GLYPH_RUN* glyphRun,
            [In] DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,
            [In] IUnknown* clientDrawingEffect = null
        );

        /// <summary>IDWriteTextLayout::Draw calls this function to instruct the client to draw an underline.</summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="baselineOriginX">X-coordinate of the baseline.</param>
        /// <param name="baselineOriginY">Y-coordinate of the baseline.</param>
        /// <param name="underline">Underline logical information.</param>
        /// <param name="clientDrawingEffect">The drawing effect set in IDWriteTextLayout::SetDrawingEffect.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> A single underline can be broken into multiple calls, depending on how the formatting changes attributes. If font sizes/styles change within an underline, the thickness and offset will be averaged weighted according to characters. To get the correct top coordinate of the underline rect, add underline::offset to the baseline's Y. Otherwise the underline will be immediately under the text. The x coordinate will always be passed as the left side, regardless of text directionality. This simplifies drawing and reduces the problem of round-off that could potentially cause gaps or a double stamped alpha blend. To avoid alpha overlap, round the end points to the nearest device pixel.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawUnderline(
            [In] IDWriteTextRenderer1* This,
            [In, Optional] void* clientDrawingContext,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [In] DWRITE_UNDERLINE* underline,
            [In] IUnknown* clientDrawingEffect = null
        );

        /// <summary>IDWriteTextLayout::Draw calls this function to instruct the client to draw a strikethrough.</summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="baselineOriginX">X-coordinate of the baseline.</param>
        /// <param name="baselineOriginY">Y-coordinate of the baseline.</param>
        /// <param name="strikethrough">Strikethrough logical information.</param>
        /// <param name="clientDrawingEffect">The drawing effect set in IDWriteTextLayout::SetDrawingEffect.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> A single strikethrough can be broken into multiple calls, depending on how the formatting changes attributes. Strikethrough is not averaged across font sizes/styles changes. To get the correct top coordinate of the strikethrough rect, add strikethrough::offset to the baseline's Y. Like underlines, the x coordinate will always be passed as the left side, regardless of text directionality.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawStrikethrough(
            [In] IDWriteTextRenderer1* This,
            [In, Optional] void* clientDrawingContext,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [In] DWRITE_STRIKETHROUGH* strikethrough,
            [In] IUnknown* clientDrawingEffect = null
        );

        /// <summary>IDWriteTextLayout::Draw calls this application callback when it needs to draw an inline object.</summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="originX">X-coordinate at the top-left corner of the inline object.</param>
        /// <param name="originY">Y-coordinate at the top-left corner of the inline object.</param>
        /// <param name="inlineObject">The object set using IDWriteTextLayout::SetInlineObject.</param>
        /// <param name="isSideways">The object should be drawn on its side.</param>
        /// <param name="isRightToLeft">The object is in an right-to-left context and should be drawn flipped.</param>
        /// <param name="clientDrawingEffect">The drawing effect set in IDWriteTextLayout::SetDrawingEffect.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> The right-to-left flag is a hint for those cases where it would look strange for the image to be shown normally (like an arrow pointing to right to indicate a submenu).</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawInlineObject(
            [In] IDWriteTextRenderer1* This,
            [In, Optional] void* clientDrawingContext,
            [In, ComAliasName("FLOAT")] float originX,
            [In, ComAliasName("FLOAT")] float originY,
            [In] IDWriteInlineObject* inlineObject,
            [In, ComAliasName("BOOL")] int isSideways,
            [In, ComAliasName("BOOL")] int isRightToLeft,
            [In] IUnknown* clientDrawingEffect = null
        );
        #endregion

        #region Delegates
        /// <summary>IDWriteTextLayout::Draw calls this function to inpublic /* blittable */ struct the client to render a run of glyphs.</summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="baselineOriginX">X-coordinate of the baseline.</param>
        /// <param name="baselineOriginY">Y-coordinate of the baseline.</param>
        /// <param name="orientationAngle">Orientation of the glyph run.</param>
        /// <param name="measuringMode">Specifies measuring method for glyphs in the run. Renderer implementations may choose different rendering modes for given measuring methods, but best results are seen when the rendering mode matches the corresponding measuring mode: DWRITE_RENDERING_MODE_CLEARTYPE_NATURAL for DWRITE_MEASURING_MODE_NATURAL DWRITE_RENDERING_MODE_CLEARTYPE_GDI_CLASSIC for DWRITE_MEASURING_MODE_GDI_CLASSIC DWRITE_RENDERING_MODE_CLEARTYPE_GDI_NATURAL for DWRITE_MEASURING_MODE_GDI_NATURAL</param>
        /// <param name="glyphRun">The glyph run to draw.</param>
        /// <param name="glyphRunDescription">Properties of the characters associated with this run.</param>
        /// <param name="clientDrawingEffect">The drawing effect set in IDWriteTextLayout::SetDrawingEffect.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> If a non-identity orientation is passed, the glyph run should be rotated around the given baseline x and y coordinates. The function IDWriteAnalyzer2::GetGlyphOrientationTransform will return the necessary transform for you, which can be combined with any existing world transform on the drawing context.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawGlyphRun1(
            [In] IDWriteTextRenderer1* This,
            [In, Optional] void* clientDrawingContext,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [In] DWRITE_GLYPH_ORIENTATION_ANGLE orientationAngle,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In] DWRITE_GLYPH_RUN* glyphRun,
            [In] DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,
            [In] IUnknown* clientDrawingEffect = null
        );

        /// <summary>IDWriteTextLayout::Draw calls this function to inpublic /* blittable */ struct the client to draw an underline.</summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="baselineOriginX">X-coordinate of the baseline.</param>
        /// <param name="baselineOriginY">Y-coordinate of the baseline.</param>
        /// <param name="orientationAngle">Orientation of the underline.</param>
        /// <param name="underline">Underline logical information.</param>
        /// <param name="clientDrawingEffect">The drawing effect set in IDWriteTextLayout::SetDrawingEffect.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> A single underline can be broken into multiple calls, depending on how the formatting changes attributes. If font sizes/styles change within an underline, the thickness and offset will be averaged weighted according to characters.
        /// To get the correct top coordinate of the underline rect, add underline::offset to the baseline's Y. Otherwise the underline will be immediately under the text. The x coordinate will always be passed as the left side, regardless of text directionality. This simplifies drawing and reduces the problem of round-off that could potentially cause gaps or a double stamped alpha blend. To avoid alpha overlap, round the end points to the nearest device pixel.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawUnderline1(
            [In] IDWriteTextRenderer1* This,
            [In, Optional] void* clientDrawingContext,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [In] DWRITE_GLYPH_ORIENTATION_ANGLE orientationAngle,
            [In] DWRITE_UNDERLINE* underline,
            [In] IUnknown* clientDrawingEffect = null
        );

        /// <summary>IDWriteTextLayout::Draw calls this function to inpublic /* blittable */ struct the client to draw a strikethrough.</summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="baselineOriginX">X-coordinate of the baseline.</param>
        /// <param name="baselineOriginY">Y-coordinate of the baseline.</param>
        /// <param name="orientationAngle">Orientation of the strikethrough.</param>
        /// <param name="strikethrough">Strikethrough logical information.</param>
        /// <param name="clientDrawingEffect">The drawing effect set in IDWriteTextLayout::SetDrawingEffect.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> A single strikethrough can be broken into multiple calls, depending on how the formatting changes attributes. Strikethrough is not averaged across font sizes/styles changes. To get the correct top coordinate of the strikethrough rect, add strikethrough::offset to the baseline's Y. Like underlines, the x coordinate will always be passed as the left side, regardless of text directionality.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawStrikethrough1(
            [In] IDWriteTextRenderer1* This,
            [In, Optional] void* clientDrawingContext,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [In] DWRITE_GLYPH_ORIENTATION_ANGLE orientationAngle,
            [In] DWRITE_STRIKETHROUGH* strikethrough,
            [In] IUnknown* clientDrawingEffect = null
        );

        /// <summary>IDWriteTextLayout::Draw calls this application callback when it needs to draw an inline object.</summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="originX">X-coordinate at the top-left corner of the inline object.</param>
        /// <param name="originY">Y-coordinate at the top-left corner of the inline object.</param>
        /// <param name="orientationAngle">Orientation of the inline object.</param>
        /// <param name="inlineObject">The object set using IDWriteTextLayout::SetInlineObject.</param>
        /// <param name="isSideways">The object should be drawn on its side.</param>
        /// <param name="isRightToLeft">The object is in an right-to-left context and should be drawn flipped.</param>
        /// <param name="clientDrawingEffect">The drawing effect set in IDWriteTextLayout::SetDrawingEffect.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> The right-to-left flag is a hint to draw the appropriate visual for that reading direction. For example, it would look strange to draw an arrow pointing to the right to indicate a submenu. The sideways flag similarly hints that the object is drawn in a different orientation. If a non-identity orientation is passed, the top left of the inline object should be rotated around the given x and y coordinates. IDWriteAnalyzer2::GetGlyphOrientationTransform returns the necessary transform for this.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawInlineObject1(
            [In] IDWriteTextRenderer1* This,
            [In, Optional] void* clientDrawingContext,
            [In, ComAliasName("FLOAT")] float originX,
            [In, ComAliasName("FLOAT")] float originY,
            [In] DWRITE_GLYPH_ORIENTATION_ANGLE orientationAngle,
            [In] IDWriteInlineObject* inlineObject,
            [In, ComAliasName("BOOL")] int isSideways,
            [In, ComAliasName("BOOL")] int isRightToLeft,
            [In] IUnknown* clientDrawingEffect = null
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

            #region IDWritePixelSnapping Fields
            public IntPtr IsPixelSnappingDisabled;

            public IntPtr GetCurrentTransform;

            public IntPtr GetPixelsPerDip;
            #endregion

            #region IDWriteTextRenderer Fields
            public IntPtr DrawGlyphRun;

            public IntPtr DrawUnderline;

            public IntPtr DrawStrikethrough;

            public IntPtr DrawInlineObject;
            #endregion

            #region Fields
            public IntPtr DrawGlyphRun1;

            public IntPtr DrawUnderline1;

            public IntPtr DrawStrikethrough1;

            public IntPtr DrawInlineObject1;
            #endregion
        }
        #endregion
    }
}
