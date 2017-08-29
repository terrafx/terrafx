// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("F67E0EDD-9E3D-4ECC-8C32-4183253DFE70")]
    public /* blittable */ unsafe struct IDWriteTextFormat2
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IDWriteTextFormat2* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IDWriteTextFormat2* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IDWriteTextFormat2* This
        );
        #endregion

        #region IDWriteTextFormat Delegates
        /// <summary>Set alignment option of text relative to layout box's leading and trailing edge.</summary>
        /// <param name="textAlignment">Text alignment option</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetTextAlignment(
            [In] IDWriteTextFormat2* This,
            [In] DWRITE_TEXT_ALIGNMENT textAlignment
        );

        /// <summary>Set alignment option of paragraph relative to layout box's top and bottom edge.</summary>
        /// <param name="paragraphAlignment">Paragraph alignment option</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetParagraphAlignment(
            [In] IDWriteTextFormat2* This,
            [In] DWRITE_PARAGRAPH_ALIGNMENT paragraphAlignment
        );

        /// <summary>Set word wrapping option.</summary>
        /// <param name="wordWrapping">Word wrapping option</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetWordWrapping(
            [In] IDWriteTextFormat2* This,
            [In] DWRITE_WORD_WRAPPING wordWrapping
        );

        /// <summary>Set paragraph reading direction.</summary>
        /// <param name="readingDirection">Text reading direction</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> The flow direction must be perpendicular to the reading direction. Setting both to a vertical direction or both to horizontal yields DWRITE_E_FLOWDIRECTIONCONFLICTS when calling GetMetrics or Draw.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetReadingDirection(
            [In] IDWriteTextFormat2* This,
            [In] DWRITE_READING_DIRECTION readingDirection
        );

        /// <summary>Set paragraph flow direction.</summary>
        /// <param name="flowDirection">Paragraph flow direction</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> The flow direction must be perpendicular to the reading direction. Setting both to a vertical direction or both to horizontal yields DWRITE_E_FLOWDIRECTIONCONFLICTS when calling GetMetrics or Draw.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetFlowDirection(
            [In] IDWriteTextFormat2* This,
            [In] DWRITE_FLOW_DIRECTION flowDirection
        );

        /// <summary>Set incremental tab stop position.</summary>
        /// <param name="incrementalTabStop">The incremental tab stop value</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetIncrementalTabStop(
            [In] IDWriteTextFormat2* This,
            [In, ComAliasName("FLOAT")] float incrementalTabStop
        );

        /// <summary>Set trimming options for any trailing text exceeding the layout width or for any far text exceeding the layout height.</summary>
        /// <param name="trimmingOptions">Text trimming options.</param>
        /// <param name="trimmingSign">Application-defined omission sign. This parameter may be NULL if no trimming sign is desired.</param>
        /// <remarks> Any inline object can be used for the trimming sign, but CreateEllipsisTrimmingSign provides a typical ellipsis symbol. Trimming is also useful vertically for hiding partial lines.</remarks>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetTrimming(
            [In] IDWriteTextFormat2* This,
            [In] DWRITE_TRIMMING* trimmingOptions,
            [In] IDWriteInlineObject* trimmingSign = null
        );

        /// <summary>Set line spacing.</summary>
        /// <param name="lineSpacingMethod">How to determine line height.</param>
        /// <param name="lineSpacing">The line height, or rather distance between one baseline to another.</param>
        /// <param name="baseline">Distance from top of line to baseline. A reasonable ratio to lineSpacing is 80%.</param>
        /// <remarks> For the default method, spacing depends solely on the content. For uniform spacing, the given line height will override the content.</remarks>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetLineSpacing(
            [In] IDWriteTextFormat2* This,
            [In] DWRITE_LINE_SPACING_METHOD lineSpacingMethod,
            [In, ComAliasName("FLOAT")] float lineSpacing,
            [In, ComAliasName("FLOAT")] float baseline
        );

        /// <summary>Get alignment option of text relative to layout box's leading and trailing edge.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_TEXT_ALIGNMENT GetTextAlignment(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Get alignment option of paragraph relative to layout box's top and bottom edge.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_PARAGRAPH_ALIGNMENT GetParagraphAlignment(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Get word wrapping option.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_WORD_WRAPPING GetWordWrapping(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Get paragraph reading direction.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_READING_DIRECTION GetReadingDirection(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Get paragraph flow direction.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FLOW_DIRECTION GetFlowDirection(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Get incremental tab stop position.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("FLOAT")]
        public /* static */ delegate float GetIncrementalTabStop(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Get trimming options for text overflowing the layout width.</summary>
        /// <param name="trimmingOptions">Text trimming options.</param>
        /// <param name="trimmingSign">Trimming omission sign. This parameter may be NULL.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTrimming(
            [In] IDWriteTextFormat2* This,
            [Out] DWRITE_TRIMMING* trimmingOptions,
            [Out] IDWriteInlineObject** trimmingSign
        );

        /// <summary>Get line spacing.</summary>
        /// <param name="lineSpacingMethod">How line height is determined.</param>
        /// <param name="lineSpacing">The line height, or rather distance between one baseline to another.</param>
        /// <param name="baseline">Distance from top of line to baseline.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLineSpacing(
            [In] IDWriteTextFormat2* This,
            [Out] DWRITE_LINE_SPACING_METHOD* lineSpacingMethod,
            [Out, ComAliasName("FLOAT")] float* lineSpacing,
            [Out, ComAliasName("FLOAT")] float* baseline
        );

        /// <summary>Get the font collection.</summary>
        /// <param name="fontCollection">The current font collection.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontCollection(
            [In] IDWriteTextFormat2* This,
            [Out] IDWriteFontCollection** fontCollection
        );

        /// <summary>Get the length of the font family name, in characters, not including the terminating NULL character.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetFontFamilyNameLength(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Get a copy of the font family name.</summary>
        /// <param name="fontFamilyName">Character array that receives the current font family name</param>
        /// <param name="nameSize">Size of the character array in character count including the terminated NULL character.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontFamilyName(
            [In] IDWriteTextFormat2* This,
            [Out, ComAliasName("WCHAR[]")] char* fontFamilyName,
            [In, ComAliasName("UINT32")] uint nameSize
        );

        /// <summary>Get the font weight.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_WEIGHT GetFontWeight(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Get the font style.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_STYLE GetFontStyle(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Get the font stretch.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_STRETCH GetFontStretch(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Get the font em height.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("FLOAT")]
        public /* static */ delegate float GetFontSize(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Get the length of the locale name, in characters, not including the terminating NULL character.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetLocaleNameLength(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Get a copy of the locale name.</summary>
        /// <param name="localeName">Character array that receives the current locale name</param>
        /// <param name="nameSize">Size of the character array in character count including the terminated NULL character.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLocaleName(
            [In] IDWriteTextFormat2* This,
            [Out, ComAliasName("WCHAR[]")] char* localeName,
            [In, ComAliasName("UINT32")] uint nameSize
        );
        #endregion

        #region IDWriteTextFormat1 Delegates
        /// <summary>Set the preferred orientation of glyphs when using a vertical reading direction.</summary>
        /// <param name="glyphOrientation">Preferred glyph orientation.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetVerticalGlyphOrientation(
            [In] IDWriteTextFormat2* This,
            [In] DWRITE_VERTICAL_GLYPH_ORIENTATION glyphOrientation
        );

        /// <summary>Get the preferred orientation of glyphs when using a vertical reading direction.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_VERTICAL_GLYPH_ORIENTATION GetVerticalGlyphOrientation(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Set whether or not the last word on the last line is wrapped.</summary>
        /// <param name="isLastLineWrappingEnabled">Line wrapping option.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetLastLineWrapping(
            [In] IDWriteTextFormat2* This,
            [In, ComAliasName("BOOL")] int isLastLineWrappingEnabled
        );

        /// <summary>Get whether or not the last word on the last line is wrapped.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int GetLastLineWrapping(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Set how the glyphs align to the edges the margin. Default behavior is to align glyphs using their default glyphs metrics which include side bearings.</summary>
        /// <param name="opticalAlignment">Optical alignment option.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetOpticalAlignment(
            [In] IDWriteTextFormat2* This,
            [In] DWRITE_OPTICAL_ALIGNMENT opticalAlignment
        );

        /// <summary>Get how the glyphs align to the edges the margin.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_OPTICAL_ALIGNMENT GetOpticalAlignment(
            [In] IDWriteTextFormat2* This
        );

        /// <summary>Apply a custom font fallback onto layout. If none is specified, layout uses the system fallback list.</summary>
        /// <param name="fontFallback">Custom font fallback created from IDWriteFontFallbackBuilder::CreateFontFallback or from IDWriteFactory2::GetSystemFontFallback.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetFontFallback(
            [In] IDWriteTextFormat2* This,
            [In] IDWriteFontFallback* fontFallback
        );

        /// <summary>Get the current font fallback object.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontFallback(
            [In] IDWriteTextFormat2* This,
            [Out] IDWriteFontFallback** fontFallback
        );
        #endregion

        #region Delegates
        /// <summary>Set line spacing.</summary>
        /// <param name="lineSpacingOptions">How to manage space between lines.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetLineSpacing1(
            [In] IDWriteTextFormat2* This,
            [In] DWRITE_LINE_SPACING* lineSpacingOptions
        );

        /// <summary>Get line spacing.</summary>
        /// <param name="lineSpacingOptions">How to manage space between lines.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLineSpacing1(
            [In] IDWriteTextFormat2* This,
            [Out] DWRITE_LINE_SPACING* lineSpacingOptions
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

            #region IDWriteTextFormat Fields
            public IntPtr SetTextAlignment;

            public IntPtr SetParagraphAlignment;

            public IntPtr SetWordWrapping;

            public IntPtr SetReadingDirection;

            public IntPtr SetFlowDirection;

            public IntPtr SetIncrementalTabStop;

            public IntPtr SetTrimming;

            public IntPtr SetLineSpacing;

            public IntPtr GetTextAlignment;

            public IntPtr GetParagraphAlignment;

            public IntPtr GetWordWrapping;

            public IntPtr GetReadingDirection;

            public IntPtr GetFlowDirection;

            public IntPtr GetIncrementalTabStop;

            public IntPtr GetTrimming;

            public IntPtr GetLineSpacing;

            public IntPtr GetFontCollection;

            public IntPtr GetFontFamilyNameLength;

            public IntPtr GetFontFamilyName;

            public IntPtr GetFontWeight;

            public IntPtr GetFontStyle;

            public IntPtr GetFontStretch;

            public IntPtr GetFontSize;

            public IntPtr GetLocaleNameLength;

            public IntPtr GetLocaleName;
            #endregion

            #region IDWriteTextFormat1 Fields
            public IntPtr SetVerticalGlyphOrientation;

            public IntPtr GetVerticalGlyphOrientation;

            public IntPtr SetLastLineWrapping;

            public IntPtr GetLastLineWrapping;

            public IntPtr SetOpticalAlignment;

            public IntPtr GetOpticalAlignment;

            public IntPtr SetFontFallback;

            public IntPtr GetFontFallback;
            #endregion

            #region Fields
            public IntPtr SetLineSpacing1;

            public IntPtr GetLineSpacing1;
            #endregion
        }
        #endregion
    }
}
