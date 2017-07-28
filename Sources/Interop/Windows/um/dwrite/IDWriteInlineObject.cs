// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The IDWriteInlineObject interface wraps an application defined inline graphic, allowing DWrite to query metrics as if it was a glyph inline with the text.</summary>
    [Guid("8339FDE3-106F-47AB-8373-1C6295EB10B3")]
    unsafe public /* blittable */ struct IDWriteInlineObject
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>The application implemented rendering callback (IDWriteTextRenderer::DrawInlineObject) can use this to draw the inline object without needing to cast or query the object type. The text layout does not call this method directly.</summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <param name="renderer">The renderer passed to IDWriteTextLayout::Draw as the object's containing parent.</param>
        /// <param name="originX">X-coordinate at the top-left corner of the inline object.</param>
        /// <param name="originY">Y-coordinate at the top-left corner of the inline object.</param>
        /// <param name="isSideways">The object should be drawn on its side.</param>
        /// <param name="isRightToLeft">The object is in an right-to-left context and should be drawn flipped.</param>
        /// <param name="clientDrawingEffect">The drawing effect set in IDWriteTextLayout::SetDrawingEffect.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Draw(
            [In] IDWriteInlineObject* This,
            [In, Optional] void* clientDrawingContext,
            [In] IDWriteTextRenderer* renderer,
            [In, ComAliasName("FLOAT")] float originX,
            [In, ComAliasName("FLOAT")] float originY,
            [In, ComAliasName("BOOL")] int isSideways,
            [In, ComAliasName("BOOL")] int isRightToLeft,
            [In, Optional] IUnknown* clientDrawingEffect
        );

        /// <summary>TextLayout calls this callback function to get the measurement of the inline object.</summary>
        /// <param name="metrics">Returned metrics</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetMetrics(
            [In] IDWriteInlineObject* This,
            [Out] DWRITE_INLINE_OBJECT_METRICS* metrics
        );

        /// <summary>TextLayout calls this callback function to get the visible extents (in DIPs) of the inline object. In the case of a simple bitmap, with no padding and no overhang, all the overhangs will simply be zeroes.</summary>
        /// <param name="overhangs">Overshoot of visible extents (in DIPs) outside the object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> The overhangs should be returned relative to the reported size of the object (DWRITE_INLINE_OBJECT_METRICS::width/height), and should not be baseline adjusted. If you have an image that is actually 100x100 DIPs, but you want it slightly inset (perhaps it has a glow) by 20 DIPs on each side, you would return a width/height of 60x60 and four overhangs of 20 DIPs.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetOverhangMetrics(
            [In] IDWriteInlineObject* This,
            [Out] DWRITE_OVERHANG_METRICS* overhangs
        );

        /// <summary>Layout uses this to determine the line breaking behavior of the inline object amidst the text.</summary>
        /// <param name="breakConditionBefore">Line-breaking condition between the object and the content immediately preceding it.</param>
        /// <param name="breakConditionAfter" >Line-breaking condition between the object and the content immediately following it.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetBreakConditions(
            [In] IDWriteInlineObject* This,
            [Out] DWRITE_BREAK_CONDITION* breakConditionBefore,
            [Out] DWRITE_BREAK_CONDITION* breakConditionAfter
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public Draw Draw;

            public GetMetrics GetMetrics;

            public GetOverhangMetrics GetOverhangMetrics;

            public GetBreakConditions GetBreakConditions;
            #endregion
        }
        #endregion
    }
}
