// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The root factory interface for all DWrite objects.</summary>
    [Guid("958DB99A-BE2A-4F09-AF7D-65189803D1D3")]
    unsafe public /* blittable */ struct IDWriteFactory5
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Creates an empty font set builder to add font face references and create a custom font set.</summary>
        /// <param name="fontSetBuilder">Holds the newly created font set builder object, or NULL in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateFontSetBuilder(
            [In] IDWriteFactory5* This,
            [Out] IDWriteFontSetBuilder1** fontSetBuilder
        );

        /// <summary>The CreateInMemoryFontFileLoader method creates a loader object that can be used to create font file references to in-memory fonts. The caller is responsible for registering and unregistering the loader.</summary>
        /// <param name="newLoader">Receives a pointer to the newly-created loader object.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateInMemoryFontFileLoader(
            [In] IDWriteFactory5* This,
            [Out] IDWriteInMemoryFontFileLoader** newLoader
        );

        /// <summary>The CreateHttpFontFileLoader function creates a remote font file loader that can create font file references from HTTP or HTTPS URLs. The caller is responsible for registering and unregistering the loader.</summary>
        /// <param name="referrerUrl">Optional referrer URL for HTTP requests.</param>
        /// <param name="extraHeaders">Optional additional header fields to include in HTTP requests. Each header field consists of a name followed by a colon (":") and the field value, as specified by RFC 2616. Multiple header fields may be separated by newlines.</param>
        /// <param name="newLoader">Receives a pointer to the newly-created loader object.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateHttpFontFileLoader(
            [In] IDWriteFactory5* This,
            [In, Optional] /* readonly */ WCHAR* referrerUrl,
            [In, Optional] /* readonly */ WCHAR* extraHeaders,
            [Out] IDWriteRemoteFontFileLoader** newLoader
            );

        /// <summary>The AnalyzeContainerType method analyzes the specified file data to determine whether it is a known font container format (e.g., WOFF or WOFF2).</summary>
        /// <returns> Returns the container type if recognized. DWRITE_CONTAINER_TYPE_UNKOWNN is returned for all other files, including uncompressed font files.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_CONTAINER_TYPE AnalyzeContainerType(
            [In] IDWriteFactory5* This,
            [In] /* readonly */ void* fileData,
            [In] UINT32 fileDataSize
        );

        /// <summary>The UnpackFontFile method unpacks font data from a container file (WOFF or WOFF2) and returns the unpacked font data in the form of a font file stream.</summary>
        /// <param name="containerType">Container type returned by AnalyzeContainerType.</param>
        /// <param name="fileData">Pointer to the compressed data.</param>
        /// <param name="fileDataSize">Size of the compressed data, in bytes.</param>
        /// <param name="unpackedFontStream">Receives a pointer to a newly created font file stream containing the uncompressed data.</param>
        /// <returns> Standard HRESULT error code. The return value is E_INVALIDARG if the container type is DWRITE_CONTAINER_TYPE_UNKNOWN.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT UnpackFontFile(
            [In] IDWriteFactory5* This,
            [In] DWRITE_CONTAINER_TYPE containerType,
            [In] /* readonly */ void* fileData,
            [In] UINT32 fileDataSize,
            [Out] IDWriteFontFileStream** unpackedFontStream
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFactory4.Vtbl BaseVtbl;

            public CreateFontSetBuilder CreateFontSetBuilder;

            public CreateInMemoryFontFileLoader CreateInMemoryFontFileLoader;

            public CreateHttpFontFileLoader CreateHttpFontFileLoader;

            public AnalyzeContainerType AnalyzeContainerType;

            public UnpackFontFile UnpackFontFile;
            #endregion
        }
        #endregion
    }
}
