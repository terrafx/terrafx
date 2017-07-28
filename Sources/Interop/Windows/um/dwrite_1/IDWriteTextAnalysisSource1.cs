// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface implemented by the client to provide needed information to the text analyzer, such as the text and associated text properties. If any of these callbacks returns an error, the analysis functions will stop prematurely and return a callback error.</summary>
    [Guid("639CFAD8-0FB4-4B21-A58A-067920120009")]
    unsafe public /* blittable */ struct IDWriteTextAnalysisSource1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>The text analyzer calls back to this to get the desired glyph orientation and resolved bidi level, which it uses along with the script properties of the text to determine the actual orientation of each character, which it reports back to the client via the sink SetGlyphOrientation method.</summary>
        /// <param name="textPosition">First position of the piece to obtain. All positions are in UTF-16 code-units, not whole characters, which matters when supplementary characters are used.</param>
        /// <param name="textLength">Number of UTF-16 units of the retrieved chunk. The returned length is not the length of the block, but the length remaining in the block, from the given position until its end. So querying for a position that is 75 positions into a 100 postition block would return 25.</param>
        /// <param name="glyphOrientation">The type of glyph orientation the client wants for this range, up to the returned text length.</param>
        /// <param name="bidiLevel">The bidi level for this range up to the returned text length, which comes from an earlier bidirectional analysis.</param>
        /// <returns> Standard HRESULT error code. Returning an error will abort the analysis.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetVerticalGlyphOrientation(
            [In] IDWriteTextAnalysisSource1* This,
            [In] UINT32 textPosition,
            [Out] UINT32* textLength,
            [Out] DWRITE_VERTICAL_GLYPH_ORIENTATION* glyphOrientation,
            [Out] UINT8* bidiLevel
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteTextAnalysisSource.Vtbl BaseVtbl;

            public GetVerticalGlyphOrientation GetVerticalGlyphOrientation;
            #endregion
        }
        #endregion
    }
}
