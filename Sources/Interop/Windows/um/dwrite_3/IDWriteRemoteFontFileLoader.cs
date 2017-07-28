// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The IDWriteRemoteFontFileLoader interface represents a font file loader that can access remote (i.e., downloadable) fonts. The IDWriteFactory5::CreateHttpFontFileLoader method returns an instance of this interface, or a client can create its own implementation.</summary>
    /// <remarks> Calls to a remote file loader or stream should never block waiting for network operations. Any call that cannot succeeded immediately using local (e.g., cached) must should return DWRITE_E_REMOTEFONT. This error signifies to DWrite that it should add requests to the font download queue.</remarks>
    [Guid("68648C83-6EDE-46C0-AB46-20083A887FDE")]
    unsafe public /* blittable */ struct IDWriteRemoteFontFileLoader
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Creates a remote font file stream object that encapsulates an open file resource and can be used to download remote file data.</summary>
        /// <param name="fontFileReferenceKey">Font file reference key that uniquely identifies the font file resource within the scope of the font loader being used.</param>
        /// <param name="fontFileReferenceKeySize">Size of font file reference key in bytes.</param>
        /// <param name="fontFileStream">Pointer to the newly created font file stream.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> Unlike CreateStreamFromKey, this method can be used to create a stream for a remote file. If the file is remote, the client must call IDWriteRemoteFontFileStream::DownloadFileInformation before the stream can be used to get the file size or access data.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateRemoteStreamFromKey(
            [In] IDWriteRemoteFontFileLoader* This,
            [In] /* readonly */ void* fontFileReferenceKey,
            [In, ComAliasName("UINT32")] uint fontFileReferenceKeySize,
            [Out] IDWriteRemoteFontFileStream** fontFileStream
        );

        /// <summary>Gets the locality of the file resource identified by the unique key.</summary>
        /// <param name="fontFileReferenceKey">Font file reference key that uniquely identifies the font file resource within the scope of the font loader being used.</param>
        /// <param name="fontFileReferenceKeySize">Size of font file reference key in bytes.</param>
        /// <param name="locality">Locality of the file.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLocalityFromKey(
            [In] IDWriteRemoteFontFileLoader* This,
            [In] /* readonly */ void* fontFileReferenceKey,
            [In, ComAliasName("UINT32")] uint fontFileReferenceKeySize,
            [Out] DWRITE_LOCALITY* locality
        );

        /// <summary>Creates a font file reference from a URL if the loader supports this capability.</summary>
        /// <param name="factory">Factory used to create the font file reference.</param>
        /// <param name="baseUrl">Optional base URL. The base URL is used to resolve the fontFileUrl if it is relative. For example, the baseUrl might be the URL of the referring document that contained the fontFileUrl.</param>
        /// <param name="fontFileUrl">URL of the font resource.</param>
        /// <param name="fontFile">Receives a pointer to the newly created font file reference.</param>
        /// <returns> Standard HRESULT error code, or E_NOTIMPL if the loader does not implement this method.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFileReferenceFromUrl(
            [In] IDWriteRemoteFontFileLoader* This,
            [In] IDWriteFactory* factory,
            [In, Optional, ComAliasName("WCHAR")] /* readonly */ char* baseUrl,
            [In, ComAliasName("WCHAR")] /* readonly */ char* fontFileUrl,
            [Out] IDWriteFontFile** fontFile
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFontFileLoader.Vtbl BaseVtbl;

            public CreateRemoteStreamFromKey CreateRemoteStreamFromKey;

            public GetLocalityFromKey GetLocalityFromKey;

            public CreateFontFileReferenceFromUrl CreateFontFileReferenceFromUrl;
            #endregion
        }
        #endregion
    }
}
