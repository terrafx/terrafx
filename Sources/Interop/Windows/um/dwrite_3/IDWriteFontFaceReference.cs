// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>A uniquely identifying reference to a font, from which you can create a font face to query font metrics and use for rendering. A font face reference consists of a font file, font face index, and font face simulation. The file data may or may not be physically present on the local machine yet.</summary>
    [Guid("5E7FA7CA-DDE3-424C-89F0-9FCD6FED58CD")]
    public /* blittable */ unsafe struct IDWriteFontFaceReference
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Creates a font face from the reference for use with layout, shaping, or rendering.</summary>
        /// <param name="fontFace">Newly created font face object, or nullptr in the case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> This function can fail with DWRITE_E_REMOTEFONT if the font is not local.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFace(
            [In] IDWriteFontFaceReference* This,
            [Out] IDWriteFontFace3** fontFace
        );

        /// <summary>Creates a font face with alternate font simulations, for example, to explicitly simulate a bold font face out of a regular variant.</summary>
        /// <param name="fontFaceSimulationFlags">Font face simulation flags for algorithmic emboldening and italicization.</param>
        /// <param name="fontFace">Newly created font face object, or nullptr in the case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> This function can fail with DWRITE_E_REMOTEFONT if the font is not local.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFaceWithSimulations(
            [In] IDWriteFontFaceReference* This,
            [In] DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags,
            [Out] IDWriteFontFace3** fontFace
        );

        /// <summary>Compares two instances of a font face references for equality.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _Equals(
            [In] IDWriteFontFaceReference* This,
            [In] IDWriteFontFaceReference* fontFaceReference
        );

        /// <summary>Obtains the zero-based index of the font face in its font file or files. If the font files contain a single face, the return value is zero.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetFontFaceIndex(
            [In] IDWriteFontFaceReference* This
        );

        /// <summary>Obtains the algorithmic style simulation flags of a font face.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_SIMULATIONS GetSimulations(
            [In] IDWriteFontFaceReference* This
        );

        /// <summary>Obtains the font file representing a font face.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontFile(
            [In] IDWriteFontFaceReference* This,
            [Out] IDWriteFontFile** fontFile
        );

        /// <summary>Get the local size of the font face in bytes.</summary>
        /// <remarks> The value returned by GetLocalFileSize will always be less than or equal to the value returned by GetFullSize. If the locality is remote, the GetLocalFileSize value is zero. If the locality is local, this value will equal the value returned by GetFileSize. If the locality is partial, this value will equal the size of the portions of the font data that have been downloaded, which will be greater than zero and less than or equal to the GetFileSize value.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetLocalFileSize(
            [In] IDWriteFontFaceReference* This
        );

        /// <summary>Get the total size of the font face in bytes.</summary>
        /// <remarks> If the locality is remote, this value is unknown and will be zero. If the locality is partial or local, the value is the full size of the font face.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetFileSize(
            [In] IDWriteFontFaceReference* This
        );

        /// <summary>Get the last modified date.</summary>
        /// <remarks> The time may be zero if the font file loader does not expose file time.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFileTime(
            [In] IDWriteFontFaceReference* This,
            [Out] FILETIME* lastWriteTime
        );

        /// <summary>Get the locality of this font face reference. You can always successfully create a font face from a fully local font. Attempting to create a font face on a remote or partially local font may fail with DWRITE_E_REMOTEFONT. This function may change between calls depending on background downloads and whether cached data expires.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_LOCALITY GetLocality(
            [In] IDWriteFontFaceReference* This
        );

        /// <summary>Adds a request to the font download queue (IDWriteFontDownloadQueue).</summary>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int EnqueueFontDownloadRequest(
            [In] IDWriteFontFaceReference* This
        );

        /// <summary>Adds a request to the font download queue (IDWriteFontDownloadQueue).</summary>
        /// <param name="characters">Array of characters to download.</param>
        /// <param name="characterCount">The number of elements in the character array.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> Downloading a character involves downloading every glyph it depends on directly or indirectly, via font tables (cmap, GSUB, COLR, glyf).</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int EnqueueCharacterDownloadRequest(
            [In] IDWriteFontFaceReference* This,
            [In, ComAliasName("WCHAR")] /* readonly */ char* characters,
            [In, ComAliasName("UINT32")] uint characterCount
        );

        /// <summary>Adds a request to the font download queue (IDWriteFontDownloadQueue).</summary>
        /// <param name="glyphIndices">Array of glyph indices to download.</param>
        /// <param name="glyphCount">The number of elements in the glyph index array.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> Downloading a glyph involves downloading any other glyphs it depends on from the font tables (GSUB, COLR, glyf).</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int EnqueueGlyphDownloadRequest(
            [In] IDWriteFontFaceReference* This,
            [In, ComAliasName("UINT16")] /* readonly */ ushort* glyphIndices,
            [In, ComAliasName("UINT32")] uint glyphCount
        );

        /// <summary>Adds a request to the font download queue (IDWriteFontDownloadQueue).</summary>
        /// <param name="fileOffset">Offset of the fragment from the beginning of the font file.</param>
        /// <param name="fragmentSize">Size of the fragment in bytes.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int EnqueueFileFragmentDownloadRequest(
            [In] IDWriteFontFaceReference* This,
            [In, ComAliasName("UINT64")] ulong fileOffset,
            [In, ComAliasName("UINT64")] ulong fragmentSize
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr CreateFontFace;

            public IntPtr CreateFontFaceWithSimulations;

            public IntPtr _Equals;

            public IntPtr GetFontFaceIndex;

            public IntPtr GetSimulations;

            public IntPtr GetFontFile;

            public IntPtr GetLocalFileSize;

            public IntPtr GetFileSize;

            public IntPtr GetFileTime;

            public IntPtr GetLocality;

            public IntPtr EnqueueFontDownloadRequest;

            public IntPtr EnqueueCharacterDownloadRequest;

            public IntPtr EnqueueGlyphDownloadRequest;

            public IntPtr EnqueueFileFragmentDownloadRequest;
            #endregion
        }
        #endregion
    }
}
