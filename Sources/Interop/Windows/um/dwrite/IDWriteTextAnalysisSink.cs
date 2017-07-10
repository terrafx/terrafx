// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface implemented by the text analyzer's client to receive the output of a given text analysis. The Text analyzer disregards any current state of the analysis sink, therefore a Set method call on a range overwrites the previously set analysis result of the same range. </summary>
    [Guid("5810CD44-0CA0-4701-B3FA-BEC5182AE4F6")]
    unsafe public /* blittable */ struct IDWriteTextAnalysisSink
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Report script analysis for the text range.</summary>
        /// <param name="textPosition">Starting position to report from.</param>
        /// <param name="textLength">Number of UTF16 units of the reported range.</param>
        /// <param name="scriptAnalysis">Script analysis of characters in range.</param>
        /// <returns>A successful code or error code to abort analysis.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetScriptAnalysis(
            [In] IDWriteTextAnalysisSink* This,
            [In] UINT32 textPosition,
            [In] UINT32 textLength,
            [In] /* readonly */ DWRITE_SCRIPT_ANALYSIS* scriptAnalysis
        );

        /// <summary>Report line-break opportunities for each character, starting from the specified position.</summary>
        /// <param name="textPosition">Starting position to report from.</param>
        /// <param name="textLength">Number of UTF16 units of the reported range.</param>
        /// <param name="lineBreakpoints">Breaking conditions for each character.</param>
        /// <returns>A successful code or error code to abort analysis.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetLineBreakpoints(
            [In] IDWriteTextAnalysisSink* This,
            [In] UINT32 textPosition,
            [In] UINT32 textLength,
            [In] /* readonly */ DWRITE_LINE_BREAKPOINT* lineBreakpoints
        );

        /// <summary>Set bidirectional level on the range, called once per each level run change (either explicit or resolved implicit).</summary>
        /// <param name="textPosition">Starting position to report from.</param>
        /// <param name="textLength">Number of UTF16 units of the reported range.</param>
        /// <param name="explicitLevel">Explicit level from embedded control codes RLE/RLO/LRE/LRO/PDF, determined before any additional rules.</param>
        /// <param name="resolvedLevel">Final implicit level considering the explicit level and characters' natural directionality, after all Bidi rules have been applied.</param>
        /// <returns>A successful code or error code to abort analysis.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetBidiLevel(
            [In] IDWriteTextAnalysisSink* This,
            [In] UINT32 textPosition,
            [In] UINT32 textLength,
            [In] UINT8 explicitLevel,
            [In] UINT8 resolvedLevel
        );

        /// <summary>Set number substitution on the range.</summary>
        /// <param name="textPosition">Starting position to report from.</param>
        /// <param name="textLength">Number of UTF16 units of the reported range.</param>
        /// <param name="numberSubstitution">The number substitution applicable to the returned range of text. The sink callback may hold onto it by incrementing its ref count.</param>
        /// <returns>A successful code or error code to abort analysis.</returns>
        /// <remark> Unlike script and bidi analysis, where every character passed to the analyzer has a result, this will only be called for those ranges where substitution is applicable. For any other range, you will simply not be called.</remark>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetNumberSubstitution(
            [In] IDWriteTextAnalysisSink* This,
            [In] UINT32 textPosition,
            [In] UINT32 textLength,
            [In] IDWriteNumberSubstitution* numberSubstitution
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public SetScriptAnalysis SetScriptAnalysis;

            public SetLineBreakpoints SetLineBreakpoints;

            public SetBidiLevel SetBidiLevel;

            public SetNumberSubstitution SetNumberSubstitution;
            #endregion
        }
        #endregion
    }
}
