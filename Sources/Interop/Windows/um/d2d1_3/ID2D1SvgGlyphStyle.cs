// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>This object supplies the values for context-fill, context-stroke, and context-value that are used when rendering SVG glyphs.</summary>
    [Guid("AF671749-D241-4DB8-8E41-DCC2E5C1A438")]
    unsafe public /* blittable */ struct ID2D1SvgGlyphStyle
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Provides values to an SVG glyph for fill. The brush with opacity set to 1 is used as the 'context-fill'. The opacity of the brush is used as the 'context-fill-opacity' value.</summary>
        /// <param name="brush">A null brush will cause the context-fill value to come from the defaultFillBrush. If the defaultFillBrush is also null, the context-fill value will be 'none'.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetFill(
            [In] ID2D1SvgGlyphStyle* This,
            [In, Optional] ID2D1Brush* brush
        );

        /// <summary>Returns the requested fill parameters.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetFill(
            [In] ID2D1SvgGlyphStyle* This,
            [Out] ID2D1Brush** brush
        );

        /// <summary>Provides values to an SVG glyph for stroke properties. The brush with opacity set to 1 is used as the 'context-stroke'. The opacity of the brush is used as the 'context-stroke-opacity' value.</summary>
        /// <param name="brush">A null brush will cause the context-stroke value to be 'none'.</param>
        /// <param name="strokeWidth">Specifies the 'context-value' for the 'stroke-width' property.</param>
        /// <param name="dashes">Specifies the 'context-value' for the 'stroke-dasharray' property. A null value will cause the stroke-dasharray to be set to 'none'.</param>
        /// <param name="dashOffset">Specifies the 'context-value' for the 'stroke-dashoffset' property.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetStroke(
            [In] ID2D1SvgGlyphStyle* This,
            [In, Optional] ID2D1Brush* brush,
            [In, DefaultParameterValue(1.0f), ComAliasName("FLOAT")] float strokeWidth,
            [In, Optional, ComAliasName("FLOAT")] /* readonly */ float* dashes,
            [In, DefaultParameterValue(0u), ComAliasName("UINT32")] uint dashesCount,
            [In, DefaultParameterValue(1.0f), ComAliasName("FLOAT")] float dashOffset
        );

        /// <summary>Returns the number of dashes in the dash array.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetStrokeDashesCount(
            [In] ID2D1SvgGlyphStyle* This
        );

        /// <summary>Returns the requested stroke parameters.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetStroke(
            [In] ID2D1SvgGlyphStyle* This,
            [Out, Optional] ID2D1Brush** brush,
            [Out, Optional, ComAliasName("FLOAT")] float* strokeWidth,
            [Out, Optional, ComAliasName("FLOAT")] float* dashes,
            [In, DefaultParameterValue(0u), ComAliasName("UINT32")] uint dashesCount,
            [Out, Optional, ComAliasName("FLOAT")] float* dashOffset
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;

            public IntPtr SetFill;

            public IntPtr GetFill;

            public IntPtr SetStroke;

            public IntPtr GetStrokeDashesCount;

            public IntPtr GetStroke;
            #endregion
        }
        #endregion
    }
}
