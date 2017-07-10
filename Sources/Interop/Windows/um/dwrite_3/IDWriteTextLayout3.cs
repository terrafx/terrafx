// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("07DDCD52-020E-4DE8-AC33-6C953D83F92D")]
    unsafe public /* blittable */ struct IDWriteTextLayout3
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Invalidates the layout, forcing layout to remeasure before calling the metrics or drawing functions. This is useful if the locality of a font changes, and layout should be redrawn, or if the size of a client implemented IDWriteInlineObject changes.</summary>>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT InvalidateLayout(
            [In] IDWriteTextLayout3* This
        );

        /// <summary>Set line spacing.</summary>>
        /// <param name="lineSpacingOptions">How to manage space between lines.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetLineSpacing(
            [In] IDWriteTextLayout3* This,
            [In] /* readonly */ DWRITE_LINE_SPACING* lineSpacingOptions
        );

        /// <summary>Get line spacing.</summary>>
        /// <param name="lineSpacingOptions">How to manage space between lines.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetLineSpacing(
            [In] IDWriteTextLayout3* This,
            [Out] DWRITE_LINE_SPACING* lineSpacingOptions
        );

        /// <summary>GetLineMetrics returns properties of each line.</summary>>
        /// <param name="lineMetrics">The array to fill with line information.</param>
        /// <param name="maxLineCount">The maximum size of the lineMetrics array.</param>
        /// <param name="actualLineCount">The actual size of the lineMetrics array that is needed.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> If maxLineCount is not large enough E_NOT_SUFFICIENT_BUFFER, which is equivalent to HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER), is returned and *actualLineCount is set to the number of lines needed.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetLineMetrics(
            [In] IDWriteTextLayout3* This,
            [Out, Optional] DWRITE_LINE_METRICS1* lineMetrics,
            [In] UINT32 maxLineCount,
            [Out] UINT32* actualLineCount
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteTextLayout2.Vtbl BaseVtbl;

            public InvalidateLayout InvalidateLayout;

            public SetLineSpacing SetLineSpacing;

            public GetLineSpacing GetLineSpacing;

            public GetLineMetrics GetLineMetrics;
            #endregion
        }
        #endregion
    }
}
