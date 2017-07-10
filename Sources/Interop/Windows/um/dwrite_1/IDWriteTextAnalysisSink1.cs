// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface implemented by the client to receive the output of the text analyzers.</summary>
    [Guid("B0D941A0-85E7-4D8B-9FD3-5CED9934482A")]
    unsafe public /* blittable */ struct IDWriteTextAnalysisSink1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>The text analyzer calls back to this to report the actual orientation of each character for shaping and drawing.</summary>>
        /// <param name="textPosition">Starting position to report from.</param>
        /// <param name="textLength">Number of UTF-16 units of the reported range.</param>
        /// <param name="glyphOrientationAngle">Angle of the glyphs within the text range (pass to GetGlyphOrientationTransform to get the world relative transform).</param>
        /// <param name="adjustedBidiLevel">The adjusted bidi level to be used by the client layout for reordering runs. This will differ from the resolved bidi level retrieved from the source for cases such as Arabic stacked top-to-bottom, where the glyphs are still shaped as RTL, but the runs are TTB along with any CJK or Latin.</param>
        /// <param name="isSideways">Whether the glyphs are rotated on their side, which is the default case for CJK and the case stacked Latin</param>
        /// <param name="isRightToLeft">Whether the script should be shaped as right-to-left. For Arabic stacked top-to-bottom, even when the adjusted bidi level is coerced to an even level, this will still be true.</param>
        /// <returns> A successful code or error code to abort analysis.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetGlyphOrientation(
            [In] IDWriteTextAnalysisSink1* This,
            [In] UINT32 textPosition,
            [In] UINT32 textLength,
            [In] DWRITE_GLYPH_ORIENTATION_ANGLE glyphOrientationAngle,
            [In] UINT8 adjustedBidiLevel,
            [In] BOOL isSideways,
            [In] BOOL isRightToLeft
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteTextAnalysisSink.Vtbl BaseVtbl;

            public SetGlyphOrientation SetGlyphOrientation;
            #endregion
        }
        #endregion
    }
}
