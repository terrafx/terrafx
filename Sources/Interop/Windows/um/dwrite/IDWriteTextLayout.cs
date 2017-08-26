// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The IDWriteTextLayout interface represents a block of text after it has been fully analyzed and formatted. All coordinates are in device independent pixels (DIPs).</summary>
    [Guid("53737037-6D14-410B-9BFE-0B182BB70961")]
    public /* blittable */ unsafe struct IDWriteTextLayout
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Set layout maximum width</summary>
        /// <param name="maxWidth">Layout maximum width</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetMaxWidth(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("FLOAT")] float maxWidth
        );

        /// <summary>Set layout maximum height</summary>
        /// <param name="maxHeight">Layout maximum height</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetMaxHeight(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("FLOAT")] float maxHeight
        );

        /// <summary>Set the font collection.</summary>
        /// <param name="fontCollection">The font collection to set</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetFontCollection(
            [In] IDWriteTextLayout* This,
            [In] IDWriteFontCollection* fontCollection,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Set null-terminated font family name.</summary>
        /// <param name="fontFamilyName">Font family name</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetFontFamilyName(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("WCHAR")] /* readonly */ char* fontFamilyName,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Set font weight.</summary>
        /// <param name="fontWeight">Font weight</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetFontWeight(
            [In] IDWriteTextLayout* This,
            [In] DWRITE_FONT_WEIGHT fontWeight,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Set font style.</summary>
        /// <param name="fontStyle">Font style</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetFontStyle(
            [In] IDWriteTextLayout* This,
            [In] DWRITE_FONT_STYLE fontStyle,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Set font stretch.</summary>
        /// <param name="fontStretch">font stretch</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetFontStretch(
            [In] IDWriteTextLayout* This,
            [In] DWRITE_FONT_STRETCH fontStretch,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Set font em height.</summary>
        /// <param name="fontSize">Font em height</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetFontSize(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("FLOAT")] float fontSize,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Set underline.</summary>
        /// <param name="hasUnderline">The Boolean flag indicates whether underline takes place</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetUnderline(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("BOOL")] int hasUnderline,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Set strikethrough.</summary>
        /// <param name="hasStrikethrough">The Boolean flag indicates whether strikethrough takes place</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetStrikethrough(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("BOOL")] int hasStrikethrough,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Set application-defined drawing effect.</summary>
        /// <param name="drawingEffect">Pointer to an application-defined drawing effect.</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> This drawing effect is associated with the specified range and will be passed back to the application via the callback when the range is drawn at drawing time.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetDrawingEffect(
            [In] IDWriteTextLayout* This,
            [In] IUnknown* drawingEffect,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Set inline object.</summary>
        /// <param name="inlineObject">Pointer to an application-implemented inline object.</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> This inline object applies to the specified range and will be passed back to the application via the DrawInlineObject callback when the range is drawn. Any text in that range will be suppressed.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetInlineObject(
            [In] IDWriteTextLayout* This,
            [In] IDWriteInlineObject* inlineObject,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Set font typography features.</summary>
        /// <param name="typography">Pointer to font typography setting.</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetTypography(
            [In] IDWriteTextLayout* This,
            [In] IDWriteTypography* typography,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Set locale name.</summary>
        /// <param name="localeName">Locale name</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetLocaleName(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("WCHAR")] /* readonly */ char* localeName,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Get layout maximum width</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("FLOAT")]
        public /* static */ delegate float GetMaxWidth(
            [In] IDWriteTextLayout* This
        );

        /// <summary>Get layout maximum height</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("FLOAT")]
        public /* static */ delegate float GetMaxHeight(
            [In] IDWriteTextLayout* This
        );

        /// <summary>Get the font collection where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="fontCollection">The current font collection</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontCollection(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out] IDWriteFontCollection** fontCollection,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Get the length of the font family name where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="nameLength">Size of the character array in character count not including the terminated NULL character.</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontFamilyNameLength(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out, ComAliasName("UINT32")] uint* nameLength,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Copy the font family name where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="fontFamilyName">Character array that receives the current font family name</param>
        /// <param name="nameSize">Size of the character array in character count including the terminated NULL character.</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontFamilyName(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out, ComAliasName("WCHAR")] char* fontFamilyName,
            [In, ComAliasName("UINT32")] uint nameSize,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Get the font weight where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="fontWeight">The current font weight</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontWeight(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out] DWRITE_FONT_WEIGHT* fontWeight,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Get the font style where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="fontStyle">The current font style</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontStyle(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out] DWRITE_FONT_STYLE* fontStyle,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Get the font stretch where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="fontStretch">The current font stretch</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontStretch(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out] DWRITE_FONT_STRETCH* fontStretch,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Get the font em height where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="fontSize">The current font em height</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontSize(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out, ComAliasName("FLOAT")] float* fontSize,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Get the underline presence where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="hasUnderline">The Boolean flag indicates whether text is underlined.</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetUnderline(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out, ComAliasName("BOOL")] int* hasUnderline,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Get the strikethrough presence where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="hasStrikethrough">The Boolean flag indicates whether text has strikethrough.</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetStrikethrough(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out, ComAliasName("BOOL")] int* hasStrikethrough,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Get the application-defined drawing effect where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="drawingEffect">The current application-defined drawing effect.</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDrawingEffect(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out] IUnknown** drawingEffect,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Get the inline object at the given position.</summary>
        /// <param name="currentPosition">The given text position.</param>
        /// <param name="inlineObject">The inline object.</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetInlineObject(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out] IDWriteInlineObject** inlineObject,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Get the typography setting where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="typography">The current typography setting.</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTypography(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out] IDWriteTypography** typography,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Get the length of the locale name where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="nameLength">Size of the character array in character count not including the terminated NULL character.</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLocaleNameLength(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out, ComAliasName("UINT32")] uint* nameLength,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Get the locale name where the current position is at.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="localeName">Character array that receives the current locale name</param>
        /// <param name="nameSize">Size of the character array in character count including the terminated NULL character.</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLocaleName(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out, ComAliasName("WCHAR")] char* localeName,
            [In, ComAliasName("UINT32")] uint nameSize,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Initiate drawing of the text.</summary>
        /// <param name="clientDrawingContext">An application defined value included in rendering callbacks.</param>
        /// <param name="renderer">The set of application-defined callbacks that do the actual rendering.</param>
        /// <param name="originX">X-coordinate of the layout's left side.</param>
        /// <param name="originY">Y-coordinate of the layout's top side.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Draw(
            [In] IDWriteTextLayout* This,
            [In, Optional] void* clientDrawingContext,
            [In] IDWriteTextRenderer* renderer,
            [In, ComAliasName("FLOAT")] float originX,
            [In, ComAliasName("FLOAT")] float originY
        );

        /// <summary>GetLineMetrics returns properties of each line.</summary>
        /// <param name="lineMetrics">The array to fill with line information.</param>
        /// <param name="maxLineCount">The maximum size of the lineMetrics array.</param>
        /// <param name="actualLineCount">The actual size of the lineMetrics array that is needed.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> If maxLineCount is not large enough E_NOT_SUFFICIENT_BUFFER, which is equivalent to HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER), is returned and *actualLineCount is set to the number of lines needed.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLineMetrics(
            [In] IDWriteTextLayout* This,
            [Out, Optional] DWRITE_LINE_METRICS* lineMetrics,
            [In, ComAliasName("UINT32")] uint maxLineCount,
            [Out, ComAliasName("UINT32")] uint* actualLineCount
        );

        /// <summary>GetMetrics retrieves overall metrics for the formatted string.</summary>
        /// <param name="textMetrics">The returned metrics.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> Drawing effects like underline and strikethrough do not contribute to the text size, which is essentially the sum of advance widths and line heights. Additionally, visible swashes and other graphic adornments may extend outside the returned width and height.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetMetrics(
            [In] IDWriteTextLayout* This,
            [Out] DWRITE_TEXT_METRICS* textMetrics
        );

        /// <summary>GetOverhangMetrics returns the overhangs (in DIPs) of the layout and all objects contained in it, including text glyphs and inline objects.</summary>
        /// <param name="overhangs">Overshoots of visible extents (in DIPs) outside the layout.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> Any underline and strikethrough do not contribute to the black box determination, since these are actually drawn by the renderer, which is allowed to draw them in any variety of styles.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetOverhangMetrics(
            [In] IDWriteTextLayout* This,
            [Out] DWRITE_OVERHANG_METRICS* overhangs
        );

        /// <summary>Retrieve logical properties and measurement of each cluster.</summary>
        /// <param name="clusterMetrics">The array to fill with cluster information.</param>
        /// <param name="maxClusterCount">The maximum size of the clusterMetrics array.</param>
        /// <param name="actualClusterCount">The actual size of the clusterMetrics array that is needed.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> If maxClusterCount is not large enough E_NOT_SUFFICIENT_BUFFER, which is equivalent to HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER), is returned and *actualClusterCount is set to the number of clusters needed.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetClusterMetrics(
            [In] IDWriteTextLayout* This,
            [Out, Optional] DWRITE_CLUSTER_METRICS* clusterMetrics,
            [In, ComAliasName("UINT32")] uint maxClusterCount,
            [Out, ComAliasName("UINT32")] uint* actualClusterCount
        );

        /// <summary>Determines the minimum possible width the layout can be set to without emergency breaking between the characters of whole words.</summary>
        /// <param name="minWidth">Minimum width.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DetermineMinWidth(
            [In] IDWriteTextLayout* This,
            [Out, ComAliasName("FLOAT")] float* minWidth
        );

        /// <summary>Given a coordinate (in DIPs) relative to the top-left of the layout box, this returns the corresponding hit-test metrics of the text string where the hit-test has occurred. This is useful for mapping mouse clicks to caret positions. When the given coordinate is outside the text string, the function sets the output value *isInside to false but returns the nearest character position.</summary>
        /// <param name="pointX">X coordinate to hit-test, relative to the top-left location of the layout box.</param>
        /// <param name="pointY">Y coordinate to hit-test, relative to the top-left location of the layout box.</param>
        /// <param name="isTrailingHit">Output flag indicating whether the hit-test location is at the leading or the trailing side of the character. When the output *isInside value is set to false, this value is set according to the output *position value to represent the edge closest to the hit-test location. </param>
        /// <param name="isInside">Output flag indicating whether the hit-test location is inside the text string. When false, the position nearest the text's edge is returned.</param>
        /// <param name="hitTestMetrics">Output geometry fully enclosing the hit-test location. When the output *isInside value is set to false, this public structure represents the geometry enclosing the edge closest to the hit-test location.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int HitTestPoint(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("FLOAT")] float pointX,
            [In, ComAliasName("FLOAT")] float pointY,
            [Out, ComAliasName("BOOL")] int* isTrailingHit,
            [Out, ComAliasName("BOOL")] int* isInside,
            [Out] DWRITE_HIT_TEST_METRICS* hitTestMetrics
        );

        /// <summary>Given a text position and whether the caret is on the leading or trailing edge of that position, this returns the corresponding coordinate (in DIPs) relative to the top-left of the layout box. This is most useful for drawing the caret's current position, but it could also be used to anchor an IME to the typed text or attach a floating menu near the point of interest. It may also be used to programmatically obtain the geometry of a particular text position for UI automation.</summary>
        /// <param name="textPosition">Text position to get the coordinate of.</param>
        /// <param name="isTrailingHit">Flag indicating whether the location is of the leading or the trailing side of the specified text position. </param>
        /// <param name="pointX">Output caret X, relative to the top-left of the layout box.</param>
        /// <param name="pointY">Output caret Y, relative to the top-left of the layout box.</param>
        /// <param name="hitTestMetrics">Output geometry fully enclosing the specified text position.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> When drawing a caret at the returned X,Y, it should be centered on X and drawn from the Y coordinate down. The height will be the size of the hit-tested text (which can vary in size within a line). Reading direction also affects which side of the character the caret is drawn. However, the returned X coordinate will be correct for either case. You can get a text length back that is larger than a single character. This happens for complex scripts when multiple characters form a single cluster, when diacritics join their base character, or when you test a surrogate pair.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int HitTestTextPosition(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint textPosition,
            [In, ComAliasName("BOOL")] int isTrailingHit,
            [Out, ComAliasName("FLOAT")] float* pointX,
            [Out, ComAliasName("FLOAT")] float* pointY,
            [Out] DWRITE_HIT_TEST_METRICS* hitTestMetrics
        );

        /// <summary>The application calls this function to get a set of hit-test metrics corresponding to a range of text positions. The main usage for this is to draw highlighted selection of the text string. The function returns E_NOT_SUFFICIENT_BUFFER, which is equivalent to HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER), when the buffer size of hitTestMetrics is too small to hold all the regions calculated by the function. In such situation, the function sets the output value *actualHitTestMetricsCount to the number of geometries calculated. The application is responsible to allocate a new buffer of greater size and call the function again. A good value to use as an initial value for maxHitTestMetricsCount may be calculated from the following equation: maxHitTestMetricsCount = lineCount * maxBidiReorderingDepth where lineCount is obtained from the value of the output argument *actualLineCount from the function IDWriteTextLayout::GetLineMetrics, and the maxBidiReorderingDepth value from the DWRITE_TEXT_METRICS public structure of the output argument *textMetrics from the function IDWriteFactory::CreateTextLayout.</summary>
        /// <param name="textPosition">First text position of the specified range.</param>
        /// <param name="textLength">Number of positions of the specified range.</param>
        /// <param name="originX">Offset of the X origin (left of the layout box) which is added to each of the hit-test metrics returned.</param>
        /// <param name="originY">Offset of the Y origin (top of the layout box) which is added to each of the hit-test metrics returned.</param>
        /// <param name="hitTestMetrics">Pointer to a buffer of the output geometry fully enclosing the specified position range.</param>
        /// <param name="maxHitTestMetricsCount">Maximum number of distinct metrics it could hold in its buffer memory.</param>
        /// <param name="actualHitTestMetricsCount">Actual number of metrics returned or needed.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> There are no gaps in the returned metrics. While there could be visual gaps, depending on bidi ordering, each range is contiguous and reports all the text, including any hidden characters and trimmed text. The height of each returned range will be the same within each line, regardless of how the font sizes vary.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int HitTestTextRange(
            [In] IDWriteTextLayout* This,
            [In, ComAliasName("UINT32")] uint textPosition,
            [In, ComAliasName("UINT32")] uint textLength,
            [In, ComAliasName("FLOAT")] float originX,
            [In, ComAliasName("FLOAT")] float originY,
            [Out, Optional] DWRITE_HIT_TEST_METRICS* hitTestMetrics,
            [In, ComAliasName("UINT32")] uint maxHitTestMetricsCount,
            [Out, ComAliasName("UINT32")] uint* actualHitTestMetricsCount
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteTextFormat.Vtbl BaseVtbl;

            public IntPtr SetMaxWidth;

            public IntPtr SetMaxHeight;

            public IntPtr SetFontCollection;

            public IntPtr SetFontFamilyName;

            public IntPtr SetFontWeight;

            public IntPtr SetFontStyle;

            public IntPtr SetFontStretch;

            public IntPtr SetFontSize;

            public IntPtr SetUnderline;

            public IntPtr SetStrikethrough;

            public IntPtr SetDrawingEffect;

            public IntPtr SetInlineObject;

            public IntPtr SetTypography;

            public IntPtr SetLocaleName;

            public IntPtr GetMaxWidth;

            public IntPtr GetMaxHeight;

            public IntPtr GetFontCollection;

            public IntPtr GetFontFamilyNameLength;

            public IntPtr GetFontFamilyName;

            public IntPtr GetFontWeight;

            public IntPtr GetFontStyle;

            public IntPtr GetFontStretch;

            public IntPtr GetFontSize;

            public IntPtr GetUnderline;

            public IntPtr GetStrikethrough;

            public IntPtr GetDrawingEffect;

            public IntPtr GetInlineObject;

            public IntPtr GetTypography;

            public IntPtr GetLocaleNameLength;

            public IntPtr GetLocaleName;

            public IntPtr Draw;

            public IntPtr GetLineMetrics;

            public IntPtr GetMetrics;

            public IntPtr GetOverhangMetrics;

            public IntPtr GetClusterMetrics;

            public IntPtr DetermineMinWidth;

            public IntPtr HitTestPoint;

            public IntPtr HitTestTextPosition;

            public IntPtr HitTestTextRange;
            #endregion
        }
        #endregion
    }
}
