// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Interop.Windows;

namespace TerraFX.Interop
{
    /// <summary>The root factory interface for all DWrite objects.</summary>
    [Guid("30572F99-DAC6-41DB-A16E-0486307E606A")]
    unsafe public /* blittable */ struct IDWriteFactory1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Gets a font collection representing the set of end-user defined custom fonts.</summary>
        /// <param name="fontCollection">Receives a pointer to the EUDC font collection object, or NULL in case of failure.</param>
        /// <param name="checkForUpdates">If this parameter is nonzero, the function performs an immediate check for changes to the set of EUDC fonts. If this parameter is FALSE, the function will still detect changes, but there may be some latency. For example, an application might specify TRUE if it has itself just modified a font and wants to be sure the font collection contains that font.</param>
        /// <returns> Standard HRESULT error code. Note that if no EUDC is set on the system, the returned collection will be empty, meaning it will return success but GetFontFamilyCount will be zero.</returns>
        /// <remarks> Querying via IDWriteFontCollection::FindFamilyName for a specific family (like MS Gothic) will return the matching family-specific EUDC font if one exists. Querying for "" will return the global EUDC font. For example, if you were matching an EUDC character within a run of the base font PMingLiu, you would retrieve the corresponding EUDC font face using GetEudcFontCollection, then FindFamilyName with "PMingLiu", followed by GetFontFamily and CreateFontFace.
        /// Be aware that eudcedit.exe can create placeholder empty glyphs that have zero advance width and no glyph outline. Although they are present in the font (HasCharacter returns true), you are best to ignore these and continue on with font fallback in your layout if the metrics for the glyph are zero.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetEudcFontCollection(
            [In] IDWriteFactory1* This,
            [Out] IDWriteFontCollection** fontCollection,
            [In, ComAliasName("BOOL")] int checkForUpdates = FALSE
    );

        /// <summary>Creates a rendering parameters object with the specified properties.</summary>
        /// <param name="gamma">The gamma value used for gamma correction, which must be greater than zero and cannot exceed 256.</param>
        /// <param name="enhancedContrast">The amount of contrast enhancement, zero or greater.</param>
        /// <param name="enhancedContrastGrayscale">The amount of contrast enhancement to use for grayscale antialiasing, zero or greater.</param>
        /// <param name="clearTypeLevel">The degree of ClearType level, from 0.0f (no ClearType) to 1.0f (full ClearType).</param>
        /// <param name="pixelGeometry">The geometry of a device pixel.</param>
        /// <param name="renderingMode">Method of rendering glyphs. In most cases, this should be DWRITE_RENDERING_MODE_DEFAULT to automatically use an appropriate mode.</param>
        /// <param name="renderingParams">Holds the newly created rendering parameters object, or NULL in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCustomRenderingParams(
            [In] IDWriteFactory1* This,
            [In, ComAliasName("FLOAT")] float gamma,
            [In, ComAliasName("FLOAT")] float enhancedContrast,
            [In, ComAliasName("FLOAT")] float enhancedContrastGrayscale,
            [In, ComAliasName("FLOAT")] float clearTypeLevel,
            [In] DWRITE_PIXEL_GEOMETRY pixelGeometry,
            [In] DWRITE_RENDERING_MODE renderingMode,
            [Out] IDWriteRenderingParams1** renderingParams
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFactory.Vtbl BaseVtbl;

            public IntPtr GetEudcFontCollection;

            public IntPtr CreateCustomRenderingParams;
            #endregion
        }
        #endregion
    }
}
