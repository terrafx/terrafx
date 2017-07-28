// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>A font fallback definition used for mapping characters to fonts capable of supporting them.</summary>
    [Guid("EFA008F9-F7A1-48BF-B05C-F224713CC0FF")]
    unsafe public /* blittable */ struct IDWriteFontFallback
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Determines an appropriate font to use to render the range of text.</summary>
        /// <param name="analysisSource">The text source implementation holds the text and locale.</param>
        /// <param name="textLength">Length of the text to analyze.</param>
        /// <param name="baseFontCollection">Default font collection to use.</param>
        /// <param name="baseFamilyName">Family name of the base font. If you pass null, no matching will be done against the family.</param>
        /// <param name="baseWeight">Desired weight.</param>
        /// <param name="baseStyle">Desired style.</param>
        /// <param name="baseStretch">Desired stretch.</param>
        /// <param name="mappedLength">Length of text mapped to the mapped font. This will always be less or equal to the input text length and greater than zero (if the text length is non-zero) so that the caller advances at least one character each call.</param>
        /// <param name="mappedFont">The font that should be used to render the first mappedLength characters of the text. If it returns NULL, then no known font can render the text, and mappedLength is the number of unsupported characters to skip.</param>
        /// <param name="scale">Scale factor to multiply the em size of the returned font by.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT MapCharacters(
            [In] IDWriteFontFallback* This,
            [In] IDWriteTextAnalysisSource* analysisSource,
            [In] UINT32 textPosition,
            [In] UINT32 textLength,
            [In, Optional] IDWriteFontCollection* baseFontCollection,
            [In, Optional] /* readonly */ WCHAR* baseFamilyName,
            [In] DWRITE_FONT_WEIGHT baseWeight,
            [In] DWRITE_FONT_STYLE baseStyle,
            [In] DWRITE_FONT_STRETCH baseStretch,
            [Out] UINT32* mappedLength,
            [Out] IDWriteFont** mappedFont,
            [Out] FLOAT* scale
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public MapCharacters MapCharacters;
            #endregion
        }
        #endregion
    }
}
