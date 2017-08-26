// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The format of text used for text layout.</summary>
    /// <remarks> This object may not be thread-safe and it may carry the state of text format change.</remarks>
    [Guid("5F174B49-0D8B-4CFB-8BCA-F1CCE9D06C67")]
    unsafe public /* blittable */ struct IDWriteTextFormat1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Set the preferred orientation of glyphs when using a vertical reading direction.</summary>
        /// <param name="glyphOrientation">Preferred glyph orientation.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetVerticalGlyphOrientation(
            [In] IDWriteTextFormat1* This,
            [In] DWRITE_VERTICAL_GLYPH_ORIENTATION glyphOrientation
        );

        /// <summary>Get the preferred orientation of glyphs when using a vertical reading direction.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_VERTICAL_GLYPH_ORIENTATION GetVerticalGlyphOrientation(
            [In] IDWriteTextFormat1* This
        );

        /// <summary>Set whether or not the last word on the last line is wrapped.</summary>
        /// <param name="isLastLineWrappingEnabled">Line wrapping option.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetLastLineWrapping(
            [In] IDWriteTextFormat1* This,
            [In, ComAliasName("BOOL")] int isLastLineWrappingEnabled
        );

        /// <summary>Get whether or not the last word on the last line is wrapped.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int GetLastLineWrapping(
            [In] IDWriteTextFormat1* This
        );

        /// <summary>Set how the glyphs align to the edges the margin. Default behavior is to align glyphs using their default glyphs metrics which include side bearings.</summary>
        /// <param name="opticalAlignment">Optical alignment option.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetOpticalAlignment(
            [In] IDWriteTextFormat1* This,
            [In] DWRITE_OPTICAL_ALIGNMENT opticalAlignment
        );

        /// <summary>Get how the glyphs align to the edges the margin.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_OPTICAL_ALIGNMENT GetOpticalAlignment(
            [In] IDWriteTextFormat1* This
        );

        /// <summary>Apply a custom font fallback onto layout. If none is specified, layout uses the system fallback list.</summary>
        /// <param name="fontFallback">Custom font fallback created from IDWriteFontFallbackBuilder::CreateFontFallback or from IDWriteFactory2::GetSystemFontFallback.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetFontFallback(
            [In] IDWriteTextFormat1* This,
            [In] IDWriteFontFallback* fontFallback
        );

        /// <summary>Get the current font fallback object.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontFallback(
            [In] IDWriteTextFormat1* This,
            [Out] IDWriteFontFallback** fontFallback
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteTextFormat.Vtbl BaseVtbl;

            public IntPtr SetVerticalGlyphOrientation;

            public IntPtr GetVerticalGlyphOrientation;

            public IntPtr SetLastLineWrapping;

            public IntPtr GetLastLineWrapping;

            public IntPtr SetOpticalAlignment;

            public IntPtr GetOpticalAlignment;

            public IntPtr SetFontFallback;

            public IntPtr GetFontFallback;
            #endregion
        }
        #endregion
    }
}
