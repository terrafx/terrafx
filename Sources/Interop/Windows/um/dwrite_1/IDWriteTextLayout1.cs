// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The IDWriteTextLayout1 interface represents a block of text after it has been fully analyzed and formatted.
    /// All coordinates are in device independent pixels (DIPs).</summary>
    [Guid("9064D822-80A7-465C-A986-DF65F78B8FEB")]
    public /* blittable */ unsafe struct IDWriteTextLayout1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Enables/disables pair-kerning on the given range.</summary>
        /// <param name="isPairKerningEnabled">The Boolean flag indicates whether text is pair-kerned.</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPairKerning(
            [In] IDWriteTextLayout1* This,
            [In, ComAliasName("BOOL")] int isPairKerningEnabled,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Get whether or not pair-kerning is enabled at given position.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="isPairKerningEnabled">The Boolean flag indicates whether text is pair-kerned.</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPairKerning(
            [In] IDWriteTextLayout1* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out, ComAliasName("BOOL")] int* isPairKerningEnabled,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );

        /// <summary>Sets the spacing between characters.</summary>
        /// <param name="leadingSpacing">The spacing before each character, in reading order.</param>
        /// <param name="trailingSpacing">The spacing after each character, in reading order.</param>
        /// <param name="minimumAdvanceWidth">The minimum advance of each character, to prevent characters from becoming too thin or zero-width. This must be zero or greater.</param>
        /// <param name="textRange">Text range to which this change applies.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetCharacterSpacing(
            [In] IDWriteTextLayout1* This,
            [In, ComAliasName("FLOAT")] float leadingSpacing,
            [In, ComAliasName("FLOAT")] float trailingSpacing,
            [In, ComAliasName("FLOAT")] float minimumAdvanceWidth,
            [In] DWRITE_TEXT_RANGE textRange
        );

        /// <summary>Gets the spacing between characters.</summary>
        /// <param name="currentPosition">The current text position.</param>
        /// <param name="leadingSpacing">The spacing before each character, in reading order.</param>
        /// <param name="trailingSpacing">The spacing after each character, in reading order.</param>
        /// <param name="minimumAdvanceWidth">The minimum advance of each character, to prevent characters from becoming too thin or zero-width. This must be zero or greater.</param>
        /// <param name="textRange">The position range of the current format.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetCharacterSpacing(
            [In] IDWriteTextLayout1* This,
            [In, ComAliasName("UINT32")] uint currentPosition,
            [Out, ComAliasName("FLOAT")] float* leadingSpacing,
            [Out, ComAliasName("FLOAT")] float* trailingSpacing,
            [Out, ComAliasName("FLOAT")] float* minimumAdvanceWidth,
            [Out] DWRITE_TEXT_RANGE* textRange = null
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteTextLayout.Vtbl BaseVtbl;

            public IntPtr SetPairKerning;

            public IntPtr GetPairKerning;

            public IntPtr SetCharacterSpacing;

            public IntPtr GetCharacterSpacing;
            #endregion
        }
        #endregion
    }
}
