// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Interop.Windows;

namespace TerraFX.Interop
{
    [Guid("4B0B5BD3-0797-4549-8AC5-FE915CC53856")]
    public /* blittable */ unsafe struct IDWriteFactory4
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IDWriteFactory4* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IDWriteFactory4* This
        );
        #endregion

        #region IDWriteFactory Delegates
        /// <summary>Gets a font collection representing the set of installed fonts.</summary>
        /// <param name="fontCollection">Receives a pointer to the system font collection object, or NULL in case of failure.</param>
        /// <param name="checkForUpdates">If this parameter is nonzero, the function performs an immediate check for changes to the set of installed fonts. If this parameter is FALSE, the function will still detect changes if the font cache service is running, but there may be some latency. For example, an application might specify TRUE if it has itself just installed a font and wants to be sure the font collection contains that font.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSystemFontCollection(
            [In] IDWriteFactory4* This,
            [Out] IDWriteFontCollection** fontCollection,
            [In, ComAliasName("BOOL")] int checkForUpdates = FALSE
        );

        /// <summary>Creates a font collection using a custom font collection loader.</summary>
        /// <param name="collectionLoader">Application-defined font collection loader, which must have been previously registered using RegisterFontCollectionLoader.</param>
        /// <param name="collectionKey">Key used by the loader to identify a collection of font files.</param>
        /// <param name="collectionKeySize">Size in bytes of the collection key.</param>
        /// <param name="fontCollection">Receives a pointer to the system font collection object, or NULL in case of failure.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCustomFontCollection(
            [In] IDWriteFactory4* This,
            [In] IDWriteFontCollectionLoader* collectionLoader,
            [In] void* collectionKey,
            [In, ComAliasName("UINT32")] uint collectionKeySize,
            [Out] IDWriteFontCollection** fontCollection
        );

        /// <summary>Registers a custom font collection loader with the factory object.</summary>
        /// <param name="fontCollectionLoader">Application-defined font collection loader.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RegisterFontCollectionLoader(
            [In] IDWriteFactory4* This,
            [In] IDWriteFontCollectionLoader* fontCollectionLoader
        );

        /// <summary>Unregisters a custom font collection loader that was previously registered using RegisterFontCollectionLoader.</summary>
        /// <param name="fontCollectionLoader">Application-defined font collection loader.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int UnregisterFontCollectionLoader(
            [In] IDWriteFactory4* This,
            [In] IDWriteFontCollectionLoader* fontCollectionLoader
        );

        /// <summary>CreateFontFileReference creates a font file reference object from a local font file.</summary>
        /// <param name="filePath">Absolute file path. Subsequent operations on the conpublic /* blittable */ structed object may fail if the user provided filePath doesn't correspond to a valid file on the disk.</param>
        /// <param name="lastWriteTime">Last modified time of the input file path. If the parameter is omitted, the function will access the font file to obtain its last write time, so the clients are encouraged to specify this value to avoid extra disk access. Subsequent operations on the conpublic /* blittable */ structed object may fail if the user provided lastWriteTime doesn't match the file on the disk.</param>
        /// <param name="fontFile">Contains newly created font file reference object, or NULL in case of failure.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFileReference(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("WCHAR[]")] char* filePath,
            [In, Optional] FILETIME* lastWriteTime,
            [Out] IDWriteFontFile** fontFile
        );

        /// <summary>CreateCustomFontFileReference creates a reference to an application specific font file resource. This function enables an application or a document to use a font without having to install it on the system. The fontFileReferenceKey has to be unique only in the scope of the fontFileLoader used in this call.</summary>
        /// <param name="fontFileReferenceKey">Font file reference key that uniquely identifies the font file resource during the lifetime of fontFileLoader.</param>
        /// <param name="fontFileReferenceKeySize">Size of font file reference key in bytes.</param>
        /// <param name="fontFileLoader">Font file loader that will be used by the font system to load data from the file identified by fontFileReferenceKey.</param>
        /// <param name="fontFile">Contains the newly created font file object, or NULL in case of failure.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> This function is provided for cases when an application or a document needs to use a font without having to install it on the system. fontFileReferenceKey has to be unique only in the scope of the fontFileLoader used in this call.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCustomFontFileReference(
            [In] IDWriteFactory4* This,
            [In] void* fontFileReferenceKey,
            [In, ComAliasName("UINT32")] uint fontFileReferenceKeySize,
            [In] IDWriteFontFileLoader* fontFileLoader,
            [Out] IDWriteFontFile** fontFile
        );

        /// <summary>Creates a font face object.</summary>
        /// <param name="fontFaceType">The file format of the font face.</param>
        /// <param name="numberOfFiles">The number of font files required to represent the font face.</param>
        /// <param name="fontFiles">Font files representing the font face. Since IDWriteFontFace maintains its own references to the input font file objects, it's OK to release them after this call.</param>
        /// <param name="faceIndex">The zero based index of a font face in cases when the font files contain a collection of font faces. If the font files contain a single face, this value should be zero.</param>
        /// <param name="fontFaceSimulationFlags">Font face simulation flags for algorithmic emboldening and italicization.</param>
        /// <param name="fontFace">Contains the newly created font face object, or NULL in case of failure.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFace(
            [In] IDWriteFactory4* This,
            [In] DWRITE_FONT_FACE_TYPE fontFaceType,
            [In, ComAliasName("UINT32")] uint numberOfFiles,
            [In, ComAliasName("IDWriteFontFile*[]")] IDWriteFontFile** fontFiles,
            [In, ComAliasName("UINT32")] uint faceIndex,
            [In] DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags,
            [Out] IDWriteFontFace** fontFace
        );

        /// <summary>Creates a rendering parameters object with default settings for the primary monitor.</summary>
        /// <param name="renderingParams">Holds the newly created rendering parameters object, or NULL in case of failure.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateRenderingParams(
            [In] IDWriteFactory4* This,
            [Out] IDWriteRenderingParams** renderingParams
        );

        /// <summary>Creates a rendering parameters object with default settings for the specified monitor.</summary>
        /// <param name="monitor">The monitor to read the default values from.</param>
        /// <param name="renderingParams">Holds the newly created rendering parameters object, or NULL in case of failure.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateMonitorRenderingParams(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("HMONITOR")] IntPtr monitor,
            [Out] IDWriteRenderingParams** renderingParams
        );

        /// <summary>Creates a rendering parameters object with the specified properties.</summary>
        /// <param name="gamma">The gamma value used for gamma correction, which must be greater than zero and cannot exceed 256.</param>
        /// <param name="enhancedContrast">The amount of contrast enhancement, zero or greater.</param>
        /// <param name="clearTypeLevel">The degree of ClearType level, from 0.0f (no ClearType) to 1.0f (full ClearType).</param>
        /// <param name="pixelGeometry">The geometry of a device pixel.</param>
        /// <param name="renderingMode">Method of rendering glyphs. In most cases, this should be DWRITE_RENDERING_MODE_DEFAULT to automatically use an appropriate mode.</param>
        /// <param name="renderingParams">Holds the newly created rendering parameters object, or NULL in case of failure.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCustomRenderingParams(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("FLOAT")] float gamma,
            [In, ComAliasName("FLOAT")] float enhancedContrast,
            [In, ComAliasName("FLOAT")] float clearTypeLevel,
            [In] DWRITE_PIXEL_GEOMETRY pixelGeometry,
            [In] DWRITE_RENDERING_MODE renderingMode,
            [Out] IDWriteRenderingParams** renderingParams
        );

        /// <summary>Registers a font file loader with DirectWrite.</summary>
        /// <param name="fontFileLoader">Pointer to the implementation of the IDWriteFontFileLoader for a particular file resource type.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks> This function registers a font file loader with DirectWrite. Font file loader interface handles loading font file resources of a particular type from a key. The font file loader interface is recommended to be implemented by a singleton object. A given instance can only be registered once. Succeeding attempts will return an error that it has already been registered. IMPORTANT: font file loader implementations must not register themselves with DirectWrite inside their conpublic /* blittable */ structors and must not unregister themselves in their depublic /* blittable */ structors, because registration and unregistration operations increment and decrement the object reference count respectively. Instead, registration and unregistration of font file loaders with DirectWrite should be performed outside of the font file loader implementation as a separate step.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RegisterFontFileLoader(
            [In] IDWriteFactory4* This,
            [In] IDWriteFontFileLoader* fontFileLoader
        );

        /// <summary>Unregisters a font file loader that was previously registered with the DirectWrite font system using RegisterFontFileLoader.</summary>
        /// <param name="fontFileLoader">Pointer to the file loader that was previously registered with the DirectWrite font system using RegisterFontFileLoader.</param>
        /// <returns>This function will succeed if the user loader is requested to be removed. It will fail if the pointer to the file loader identifies a standard DirectWrite loader, or a loader that is never registered or has already been unregistered.</returns>
        /// <remarks> This function unregisters font file loader callbacks with the DirectWrite font system. The font file loader interface is recommended to be implemented by a singleton object. IMPORTANT: font file loader implementations must not register themselves with DirectWrite inside their conpublic /* blittable */ structors and must not unregister themselves in their depublic /* blittable */ structors, because registration and unregistration operations increment and decrement the object reference count respectively. Instead, registration and unregistration of font file loaders with DirectWrite should be performed outside of the font file loader implementation as a separate step.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int UnregisterFontFileLoader(
            [In] IDWriteFactory4* This,
            [In] IDWriteFontFileLoader* fontFileLoader
        );

        /// <summary>Create a text format object used for text layout.</summary>
        /// <param name="fontFamilyName">Name of the font family</param>
        /// <param name="fontCollection">Font collection. NULL indicates the system font collection.</param>
        /// <param name="fontWeight">Font weight</param>
        /// <param name="fontStyle">Font style</param>
        /// <param name="fontStretch">Font stretch</param>
        /// <param name="fontSize">Logical size of the font in DIP units. A DIP ("device-independent pixel") equals 1/96 inch.</param>
        /// <param name="localeName">Locale name</param>
        /// <param name="textFormat">Contains newly created text format object, or NULL in case of failure.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateTextFormat(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("WCHAR[]")] char* fontFamilyName,
            [In, Optional] IDWriteFontCollection* fontCollection,
            [In] DWRITE_FONT_WEIGHT fontWeight,
            [In] DWRITE_FONT_STYLE fontStyle,
            [In] DWRITE_FONT_STRETCH fontStretch,
            [In, ComAliasName("FLOAT")] float fontSize,
            [In, ComAliasName("WCHAR[]")] char* localeName,
            [Out] IDWriteTextFormat** textFormat
        );

        /// <summary>Create a typography object used in conjunction with text format for text layout.</summary>
        /// <param name="typography">Contains newly created typography object, or NULL in case of failure.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateTypography(
            [In] IDWriteFactory4* This,
            [Out] IDWriteTypography** typography
        );

        /// <summary>Create an object used for interoperability with GDI.</summary>
        /// <param name="gdiInterop">Receives the GDI interop object if successful, or NULL in case of failure.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGdiInterop(
            [In] IDWriteFactory4* This,
            [Out] IDWriteGdiInterop** gdiInterop
        );

        /// <summary>CreateTextLayout takes a string, format, and associated constraints and produces an object representing the fully analyzed and formatted result.</summary>
        /// <param name="string">The string to layout.</param>
        /// <param name="stringLength">The length of the string.</param>
        /// <param name="textFormat">The format to apply to the string.</param>
        /// <param name="maxWidth">Width of the layout box.</param>
        /// <param name="maxHeight">Height of the layout box.</param>
        /// <param name="textLayout">The resultant object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateTextLayout(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("WCHAR[]")] char* @string,
            [In, ComAliasName("UINT32")] uint stringLength,
            [In] IDWriteTextFormat* textFormat,
            [In, ComAliasName("FLOAT")] float maxWidth,
            [In, ComAliasName("FLOAT")] float maxHeight,
            [Out] IDWriteTextLayout** textLayout
        );

        /// <summary>CreateGdiCompatibleTextLayout takes a string, format, and associated constraints and produces and object representing the result formatted for a particular display resolution and measuring mode. The resulting text layout should only be used for the intended resolution, and for cases where text scalability is desired, CreateTextLayout should be used instead.</summary>
        /// <param name="string">The string to layout.</param>
        /// <param name="stringLength">The length of the string.</param>
        /// <param name="textFormat">The format to apply to the string.</param>
        /// <param name="layoutWidth">Width of the layout box.</param>
        /// <param name="layoutHeight">Height of the layout box.</param>
        /// <param name="pixelsPerDip">Number of physical pixels per DIP. For example, if rendering onto a 96 DPI device then pixelsPerDip is 1. If rendering onto a 120 DPI device then pixelsPerDip is 120/96.</param>
        /// <param name="transform">Optional transform applied to the glyphs and their positions. This transform is applied after the scaling specified the font size and pixelsPerDip.</param>
        /// <param name="useGdiNatural"> When set to FALSE, inpublic /* blittable */ structs the text layout to use the same metrics as GDI aliased text. When set to TRUE, inpublic /* blittable */ structs the text layout to use the same metrics as text measured by GDI using a font created with CLEARTYPE_NATURAL_QUALITY.</param>
        /// <param name="textLayout">The resultant object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateGdiCompatibleTextLayout(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("WCHAR[]")] char* @string,
            [In, ComAliasName("UINT32")] uint stringLength,
            [In] IDWriteTextFormat* textFormat,
            [In, ComAliasName("FLOAT")] float layoutWidth,
            [In, ComAliasName("FLOAT")] float layoutHeight,
            [In, ComAliasName("FLOAT")] float pixelsPerDip,
            [In, Optional] DWRITE_MATRIX* transform,
            [In, ComAliasName("BOOL")] int useGdiNatural,
            [Out] IDWriteTextLayout** textLayout
        );

        /// <summary>The application may call this function to create an inline object for trimming, using an ellipsis as the omission sign. The ellipsis will be created using the current settings of the format, including base font, style, and any effects. Alternate omission signs can be created by the application by implementing IDWriteInlineObject.</summary>
        /// <param name="textFormat">Text format used as a template for the omission sign.</param>
        /// <param name="trimmingSign">Created omission sign.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateEllipsisTrimmingSign(
            [In] IDWriteFactory4* This,
            [In] IDWriteTextFormat* textFormat,
            [Out] IDWriteInlineObject** trimmingSign
        );

        /// <summary>Return an interface to perform text analysis with.</summary>
        /// <param name="textAnalyzer">The resultant object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateTextAnalyzer(
            [In] IDWriteFactory4* This,
            [Out] IDWriteTextAnalyzer** textAnalyzer
        );

        /// <summary>Creates a number substitution object using a locale name, substitution method, and whether to ignore user overrides (uses NLS defaults for the given culture instead).</summary>
        /// <param name="substitutionMethod">Method of number substitution to use.</param>
        /// <param name="localeName">Which locale to obtain the digits from.</param>
        /// <param name="ignoreUserOverride">Ignore the user's settings and use the locale defaults</param>
        /// <param name="numberSubstitution">Receives a pointer to the newly created object.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateNumberSubstitution(
            [In] IDWriteFactory4* This,
            [In] DWRITE_NUMBER_SUBSTITUTION_METHOD substitutionMethod,
            [In, ComAliasName("WCHAR[]")] char* localeName,
            [In, ComAliasName("BOOL")] int ignoreUserOverride,
            [Out] IDWriteNumberSubstitution** numberSubstitution
        );

        /// <summary>Creates a glyph run analysis object, which encapsulates information used to render a glyph run.</summary>
        /// <param name="glyphRun">Structure specifying the properties of the glyph run.</param>
        /// <param name="pixelsPerDip">Number of physical pixels per DIP. For example, if rendering onto a 96 DPI bitmap then pixelsPerDip is 1. If rendering onto a 120 DPI bitmap then pixelsPerDip is 120/96.</param>
        /// <param name="transform">Optional transform applied to the glyphs and their positions. This transform is applied after the scaling specified by the emSize and pixelsPerDip.</param>
        /// <param name="renderingMode">Specifies the rendering mode, which must be one of the raster rendering modes (i.e., not default and not outline).</param>
        /// <param name="measuringMode">Specifies the method to measure glyphs.</param>
        /// <param name="baselineOriginX">Horizontal position of the baseline origin, in DIPs.</param>
        /// <param name="baselineOriginY">Vertical position of the baseline origin, in DIPs.</param>
        /// <param name="glyphRunAnalysis">Receives a pointer to the newly created object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateGlyphRunAnalysis(
            [In] IDWriteFactory4* This,
            [In] DWRITE_GLYPH_RUN* glyphRun,
            [In, ComAliasName("FLOAT")] float pixelsPerDip,
            [In, Optional] DWRITE_MATRIX* transform,
            [In] DWRITE_RENDERING_MODE renderingMode,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [Out] IDWriteGlyphRunAnalysis** glyphRunAnalysis
        );
        #endregion

        #region IDWriteFactory1 Delegates
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
            [In] IDWriteFactory4* This,
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
        public /* static */ delegate int CreateCustomRenderingParams1(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("FLOAT")] float gamma,
            [In, ComAliasName("FLOAT")] float enhancedContrast,
            [In, ComAliasName("FLOAT")] float enhancedContrastGrayscale,
            [In, ComAliasName("FLOAT")] float clearTypeLevel,
            [In] DWRITE_PIXEL_GEOMETRY pixelGeometry,
            [In] DWRITE_RENDERING_MODE renderingMode,
            [Out] IDWriteRenderingParams1** renderingParams
        );
        #endregion

        #region IDWriteFactory2 Delegates
        /// <summary>Get the system-appropriate font fallback mapping list.</summary>
        /// <param name="fontFallback">The system fallback list.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSystemFontFallback(
            [In] IDWriteFactory4* This,
            [Out] IDWriteFontFallback** fontFallback
        );

        /// <summary>Create a custom font fallback builder.</summary>
        /// <param name="fontFallbackBuilder">Empty font fallback builder.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFallbackBuilder(
            [In] IDWriteFactory4* This,
            [Out] IDWriteFontFallbackBuilder** fontFallbackBuilder
        );

        /// <summary>Translates a glyph run to a sequence of color glyph runs, which can be rendered to produce a color representation of the original "base" run.</summary>
        /// <param name="baselineOriginX">Horizontal origin of the base glyph run in pre-transform coordinates.</param>
        /// <param name="baselineOriginY">Vertical origin of the base glyph run in pre-transform coordinates.</param>
        /// <param name="glyphRun">Pointer to the original "base" glyph run.</param>
        /// <param name="glyphRunDescription">Optional glyph run description.</param>
        /// <param name="measuringMode">Measuring mode, needed to compute the origins of each glyph.</param>
        /// <param name="worldToDeviceTransform">Matrix converting from the client's coordinate space to device coordinates (pixels), i.e., the world transform multiplied by any DPI scaling.</param>
        /// <param name="colorPaletteIndex">Zero-based index of the color palette to use. Valid indices are less than the number of palettes in the font, as returned by IDWriteFontFace2::GetColorPaletteCount.</param>
        /// <param name="colorLayers">If the function succeeds, receives a pointer to an enumerator object that can be used to obtain the color glyph runs. If the base run has no color glyphs, then the output pointer is NULL and the method returns DWRITE_E_NOCOLOR.</param>
        /// <returns> Returns DWRITE_E_NOCOLOR if the font has no color information, the base glyph run does not contain any color glyphs, or the specified color palette index is out of range. In this case, the client should render the base glyph run. Otherwise, returns a standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int TranslateColorGlyphRun(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [In] DWRITE_GLYPH_RUN* glyphRun,
            [In, Optional] DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In, Optional] DWRITE_MATRIX* worldToDeviceTransform,
            [In, ComAliasName("UINT32")] uint colorPaletteIndex,
            [Out] IDWriteColorGlyphRunEnumerator** colorLayers
        );

        /// <summary>Creates a rendering parameters object with the specified properties.</summary>
        /// <param name="gamma">The gamma value used for gamma correction, which must be greater than zero and cannot exceed 256.</param>
        /// <param name="enhancedContrast">The amount of contrast enhancement, zero or greater.</param>
        /// <param name="clearTypeLevel">The degree of ClearType level, from 0.0f (no ClearType) to 1.0f (full ClearType).</param>
        /// <param name="pixelGeometry">The geometry of a device pixel.</param>
        /// <param name="renderingMode">Method of rendering glyphs. In most cases, this should be DWRITE_RENDERING_MODE_DEFAULT to automatically use an appropriate mode.</param>
        /// <param name="gridFitMode">How to grid fit glyph outlines. In most cases, this should be DWRITE_GRID_FIT_DEFAULT to automatically choose an appropriate mode.</param>
        /// <param name="renderingParams">Holds the newly created rendering parameters object, or NULL in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCustomRenderingParams2(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("FLOAT")] float gamma,
            [In, ComAliasName("FLOAT")] float enhancedContrast,
            [In, ComAliasName("FLOAT")] float grayscaleEnhancedContrast,
            [In, ComAliasName("FLOAT")] float clearTypeLevel,
            [In] DWRITE_PIXEL_GEOMETRY pixelGeometry,
            [In] DWRITE_RENDERING_MODE renderingMode,
            [In] DWRITE_GRID_FIT_MODE gridFitMode,
            [Out] IDWriteRenderingParams2** renderingParams
        );

        /// <summary>Creates a glyph run analysis object, which encapsulates information used to render a glyph run.</summary>
        /// <param name="glyphRun">Structure specifying the properties of the glyph run.</param>
        /// <param name="transform">Optional transform applied to the glyphs and their positions. This transform is applied after the scaling specified by the emSize and pixelsPerDip.</param>
        /// <param name="renderingMode">Specifies the rendering mode, which must be one of the raster rendering modes (i.e., not default and not outline).</param>
        /// <param name="measuringMode">Specifies the method to measure glyphs.</param>
        /// <param name="gridFitMode">How to grid-fit glyph outlines. This must be non-default.</param>
        /// <param name="baselineOriginX">Horizontal position of the baseline origin, in DIPs.</param>
        /// <param name="baselineOriginY">Vertical position of the baseline origin, in DIPs.</param>
        /// <param name="glyphRunAnalysis">Receives a pointer to the newly created object.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateGlyphRunAnalysis1(
            [In] IDWriteFactory4* This,
            [In] DWRITE_GLYPH_RUN* glyphRun,
            [In, Optional] DWRITE_MATRIX* transform,
            [In] DWRITE_RENDERING_MODE renderingMode,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In] DWRITE_GRID_FIT_MODE gridFitMode,
            [In] DWRITE_TEXT_ANTIALIAS_MODE antialiasMode,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [Out] IDWriteGlyphRunAnalysis** glyphRunAnalysis
        );
        #endregion

        #region IDWriteFactory3 Delegates
        /// <summary>Creates a glyph run analysis object, which encapsulates information used to render a glyph run.</summary>
        /// <param name="glyphRun">Structure specifying the properties of the glyph run.</param>
        /// <param name="transform">Optional transform applied to the glyphs and their positions. This transform is applied after the scaling specified by the emSize.</param>
        /// <param name="renderingMode">Specifies the rendering mode, which must be one of the raster rendering modes (i.e., not default and not outline).</param>
        /// <param name="measuringMode">Specifies the method to measure glyphs.</param>
        /// <param name="gridFitMode">How to grid-fit glyph outlines. This must be non-default.</param>
        /// <param name="baselineOriginX">Horizontal position of the baseline origin, in DIPs.</param>
        /// <param name="baselineOriginY">Vertical position of the baseline origin, in DIPs.</param>
        /// <param name="glyphRunAnalysis">Receives a pointer to the newly created object.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateGlyphRunAnalysis2(
            [In] IDWriteFactory4* This,
            [In] DWRITE_GLYPH_RUN* glyphRun,
            [In, Optional] DWRITE_MATRIX* transform,
            [In] DWRITE_RENDERING_MODE1 renderingMode,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In] DWRITE_GRID_FIT_MODE gridFitMode,
            [In] DWRITE_TEXT_ANTIALIAS_MODE antialiasMode,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [Out] IDWriteGlyphRunAnalysis** glyphRunAnalysis
        );

        /// <summary>Creates a rendering parameters object with the specified properties.</summary>
        /// <param name="gamma">The gamma value used for gamma correction, which must be greater than zero and cannot exceed 256.</param>
        /// <param name="enhancedContrast">The amount of contrast enhancement, zero or greater.</param>
        /// <param name="grayscaleEnhancedContrast">The amount of contrast enhancement to use for grayscale antialiasing, zero or greater.</param>
        /// <param name="clearTypeLevel">The degree of ClearType level, from 0.0f (no ClearType) to 1.0f (full ClearType).</param>
        /// <param name="pixelGeometry">The geometry of a device pixel.</param>
        /// <param name="renderingMode">Method of rendering glyphs. In most cases, this should be DWRITE_RENDERING_MODE_DEFAULT to automatically use an appropriate mode.</param>
        /// <param name="gridFitMode">How to grid fit glyph outlines. In most cases, this should be DWRITE_GRID_FIT_DEFAULT to automatically choose an appropriate mode.</param>
        /// <param name="renderingParams">Holds the newly created rendering parameters object, or NULL in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCustomRenderingParams3(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("FLOAT")] float gamma,
            [In, ComAliasName("FLOAT")] float enhancedContrast,
            [In, ComAliasName("FLOAT")] float grayscaleEnhancedContrast,
            [In, ComAliasName("FLOAT")] float clearTypeLevel,
            [In] DWRITE_PIXEL_GEOMETRY pixelGeometry,
            [In] DWRITE_RENDERING_MODE1 renderingMode,
            [In] DWRITE_GRID_FIT_MODE gridFitMode,
            [Out] IDWriteRenderingParams3** renderingParams
        );

        /// <summary>Creates a reference to a font given a full path.</summary>
        /// <param name="filePath">Absolute file path. Subsequent operations on the conpublic /* blittable */ structed object may fail if the user provided filePath doesn't correspond to a valid file on the disk.</param>
        /// <param name="lastWriteTime">Last modified time of the input file path. If the parameter is omitted, the function will access the font file to obtain its last write time, so the clients are encouraged to specify this value to avoid extra disk access. Subsequent operations on the conpublic /* blittable */ structed object may fail if the user provided lastWriteTime doesn't match the file on the disk.</param>
        /// <param name="faceIndex">The zero based index of a font face in cases when the font files contain a collection of font faces. If the font files contain a single face, this value should be zero.</param>
        /// <param name="fontSimulations">Font face simulation flags for algorithmic emboldening and italicization.</param>
        /// <param name="fontFaceReference">Contains newly created font face reference object, or nullptr in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFaceReference(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("WCHAR[]")] char* filePath,
            [In, Optional] FILETIME* lastWriteTime,
            [In, ComAliasName("UINT32")] uint faceIndex,
            [In] DWRITE_FONT_SIMULATIONS fontSimulations,
            [Out] IDWriteFontFaceReference** fontFaceReference
        );

        /// <summary>Creates a reference to a font given a file.</summary>
        /// <param name="fontFile">User provided font file representing the font face.</param>
        /// <param name="faceIndex">The zero based index of a font face in cases when the font files contain a collection of font faces. If the font files contain a single face, this value should be zero.</param>
        /// <param name="fontSimulations">Font face simulation flags for algorithmic emboldening and italicization.</param>
        /// <param name="fontFaceReference">Contains newly created font face reference object, or nullptr in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFaceReference1(
            [In] IDWriteFactory4* This,
            [In] IDWriteFontFile* fontFile,
            [In, ComAliasName("UINT32")] uint faceIndex,
            [In] DWRITE_FONT_SIMULATIONS fontSimulations,
            [Out] IDWriteFontFaceReference** fontFaceReference
        );

        /// <summary>Retrieves the list of system fonts.</summary>
        /// <param name="fontSet">Holds the newly created font set object, or NULL in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSystemFontSet(
            [In] IDWriteFactory4* This,
            [Out] IDWriteFontSet** fontSet
        );

        /// <summary>Creates an empty font set builder to add font face references and create a custom font set.</summary>
        /// <param name="fontSetBuilder">Holds the newly created font set builder object, or NULL in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontSetBuilder(
            [In] IDWriteFactory4* This,
            [Out] IDWriteFontSetBuilder** fontSetBuilder
        );

        /// <summary>Create a weight/width/slope tree from a set of fonts.</summary>
        /// <param name="fontSet">A set of fonts to use to build the collection.</param>
        /// <param name="fontCollection">Holds the newly created font collection object, or NULL in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontCollectionFromFontSet(
            [In] IDWriteFactory4* This,
            [In] IDWriteFontSet* fontSet,
            [Out] IDWriteFontCollection1** fontCollection
        );

        /// <summary>Retrieves a weight/width/slope tree of system fonts.</summary>
        /// <param name="includeDownloadableFonts">Include cloud fonts or only locally installed ones.</param>
        /// <param name="fontCollection">Holds the newly created font collection object, or NULL in case of failure.</param>
        /// <param name="checkForUpdates">If this parameter is nonzero, the function performs an immediate check for changes to the set of system fonts. If this parameter is FALSE, the function will still detect changes if the font cache service is running, but there may be some latency. For example, an application might specify TRUE if it has itself just installed a font and wants to be sure the font collection contains that font.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSystemFontCollection1(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("BOOL")] int includeDownloadableFonts,
            [Out] IDWriteFontCollection1** fontCollection,
            [In, ComAliasName("BOOL")] int checkForUpdates = FALSE
        );

        /// <summary>Gets the font download queue associated with this factory object.</summary>
        /// <param name="fontDownloadQueue">Receives a pointer to the font download queue interface.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontDownloadQueue(
            [In] IDWriteFactory4* This,
            [Out] IDWriteFontDownloadQueue** fontDownloadQueue
        );
        #endregion

        #region Delegates
        /// <summary>Translates a glyph run to a sequence of color glyph runs, which can be rendered to produce a color representation of the original "base" run.</summary>
        /// <param name="baselineOrigin">Horizontal and vertical origin of the base glyph run in pre-transform coordinates.</param>
        /// <param name="glyphRun">Pointer to the original "base" glyph run.</param>
        /// <param name="glyphRunDescription">Optional glyph run description.</param>
        /// <param name="desiredGlyphImageFormats">Which data formats TranslateColorGlyphRun should split the runs into.</param>
        /// <param name="measuringMode">Measuring mode, needed to compute the origins of each glyph.</param>
        /// <param name="worldAndDpiTransform">Matrix converting from the client's coordinate space to device coordinates (pixels), i.e., the world transform multiplied by any DPI scaling.</param>
        /// <param name="colorPaletteIndex">Zero-based index of the color palette to use. Valid indices are less than the number of palettes in the font, as returned by IDWriteFontFace2::GetColorPaletteCount.</param>
        /// <param name="colorLayers">If the function succeeds, receives a pointer to an enumerator object that can be used to obtain the color glyph runs. If the base run has no color glyphs, then the output pointer is NULL and the method returns DWRITE_E_NOCOLOR.</param>
        /// <returns> Returns DWRITE_E_NOCOLOR if the font has no color information, the glyph run does not contain any color glyphs, or the specified color palette index is out of range. In this case, the client should render the original glyph run. Otherwise, returns a standard HRESULT error code.</returns>
        /// <remarks> The old IDWriteFactory2::TranslateColorGlyphRun is equivalent to passing DWRITE_GLYPH_IMAGE_FORMATS_TRUETYPE|CFF|COLR.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int TranslateColorGlyphRun1(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F baselineOrigin,
            [In] DWRITE_GLYPH_RUN* glyphRun,
            [In, Optional] DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,
            [In] DWRITE_GLYPH_IMAGE_FORMATS desiredGlyphImageFormats,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In, Optional] DWRITE_MATRIX* worldAndDpiTransform,
            [In, ComAliasName("UINT32")] uint colorPaletteIndex,
            [Out] IDWriteColorGlyphRunEnumerator1** colorLayers
        );

        /// <summary>Converts glyph run placements to glyph origins.</summary>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> The transform and DPI have no affect on the origin scaling. They are solely used to compute glyph advances when not supplied and align glyphs in pixel aligned measuring modes.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ComputeGlyphOrigins(
            [In] IDWriteFactory4* This,
            [In] DWRITE_GLYPH_RUN* glyphRun,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F baselineOrigin,
            [In, Optional] DWRITE_MATRIX* worldAndDpiTransform,
            [Out, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F* glyphOrigins
        );

        /// <summary>Converts glyph run placements to glyph origins. This overload is for natural metrics, which includes SVG, TrueType natural modes, and bitmap placement.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ComputeGlyphOrigins1(
            [In] IDWriteFactory4* This,
            [In] DWRITE_GLYPH_RUN* glyphRun,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F baselineOrigin,
            [Out, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F* glyphOrigins
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region IDWriteFactory Fields
            public IntPtr GetSystemFontCollection;

            public IntPtr CreateCustomFontCollection;

            public IntPtr RegisterFontCollectionLoader;

            public IntPtr UnregisterFontCollectionLoader;

            public IntPtr CreateFontFileReference;

            public IntPtr CreateCustomFontFileReference;

            public IntPtr CreateFontFace;

            public IntPtr CreateRenderingParams;

            public IntPtr CreateMonitorRenderingParams;

            public IntPtr CreateCustomRenderingParams;

            public IntPtr RegisterFontFileLoader;

            public IntPtr UnregisterFontFileLoader;

            public IntPtr CreateTextFormat;

            public IntPtr CreateTypography;

            public IntPtr GetGdiInterop;

            public IntPtr CreateTextLayout;

            public IntPtr CreateGdiCompatibleTextLayout;

            public IntPtr CreateEllipsisTrimmingSign;

            public IntPtr CreateTextAnalyzer;

            public IntPtr CreateNumberSubstitution;

            public IntPtr CreateGlyphRunAnalysis;
            #endregion

            #region IDWriteFactory1 Fields
            public IntPtr GetEudcFontCollection;

            public IntPtr CreateCustomRenderingParams1;
            #endregion

            #region IDWriteFactory2 Fields
            public IntPtr GetSystemFontFallback;

            public IntPtr CreateFontFallbackBuilder;

            public IntPtr TranslateColorGlyphRun;

            public IntPtr CreateCustomRenderingParams2;

            public IntPtr CreateGlyphRunAnalysis1;
            #endregion

            #region IDWriteFactory3 Fields
            public IntPtr CreateGlyphRunAnalysis2;

            public IntPtr CreateCustomRenderingParams3;

            public IntPtr CreateFontFaceReference;

            public IntPtr CreateFontFaceReference1;

            public IntPtr GetSystemFontSet;

            public IntPtr CreateFontSetBuilder;

            public IntPtr CreateFontCollectionFromFontSet;

            public IntPtr GetSystemFontCollection1;

            public IntPtr GetFontDownloadQueue;
            #endregion

            #region Fields
            public IntPtr TranslateColorGlyphRun1;

            public IntPtr ComputeGlyphOrigins;

            public IntPtr ComputeGlyphOrigins1;
            #endregion
        }
        #endregion
    }
}
