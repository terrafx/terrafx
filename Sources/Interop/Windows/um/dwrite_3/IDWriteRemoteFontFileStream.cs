// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>IDWriteRemoteFontFileStream represents a font file stream parts of which may be non-local. Non-local data must be downloaded before it can be accessed using ReadFragment. The interface exposes methods to download font data and query the locality of font data.</summary>
    /// <remarks> For more information, see the description of IDWriteRemoteFontFileLoader.</remarks>
    [Guid("4DB3757A-2C72-4ED9-B2B6-1ABABE1AFF9C")]
    public /* blittable */ unsafe struct IDWriteRemoteFontFileStream
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IDWriteRemoteFontFileStream* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IDWriteRemoteFontFileStream* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IDWriteRemoteFontFileStream* This
        );
        #endregion

        #region IDWriteFontFileStream Delegates
        /// <summary>Reads a fragment from a file.</summary>
        /// <param name="fragmentStart">Receives the pointer to the start of the font file fragment.</param>
        /// <param name="fileOffset">Offset of the fragment from the beginning of the font file.</param>
        /// <param name="fragmentSize">Size of the fragment in bytes.</param>
        /// <param name="fragmentContext">The client defined context to be passed to the ReleaseFileFragment.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks>IMPORTANT: ReadFileFragment() implementations must check whether the requested file fragment is within the file bounds. Otherwise, an error should be returned from ReadFileFragment.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ReadFileFragment(
            [In] IDWriteRemoteFontFileStream* This,
            [Out] void** fragmentStart,
            [In, ComAliasName("UINT64")] ulong fileOffset,
            [In, ComAliasName("UINT64")] ulong fragmentSize,
            [Out] void** fragmentContext
        );

        /// <summary>Releases a fragment from a file.</summary>
        /// <param name="fragmentContext">The client defined context of a font fragment returned from ReadFileFragment.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ReleaseFileFragment(
            [In] IDWriteRemoteFontFileStream* This,
            void* fragmentContext
        );

        /// <summary>Obtains the total size of a file.</summary>
        /// <param name="fileSize">Receives the total size of the file.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks>Implementing GetFileSize() for asynchronously loaded font files may require downloading the complete file contents, therefore this method should only be used for operations that either require complete font file to be loaded (e.g., copying a font file) or need to make decisions based on the value of the file size (e.g., validation against a persisted file size).</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFileSize(
            [In] IDWriteRemoteFontFileStream* This,
            [Out, ComAliasName("UINT64")] ulong* fileSize
        );

        /// <summary>Obtains the last modified time of the file. The last modified time is used by DirectWrite font selection algorithms to determine whether one font resource is more up to date than another one.</summary>
        /// <param name="lastWriteTime">Receives the last modified time of the file in the format that represents the number of 100-nanosecond intervals since January 1, 1601 (UTC).</param>
        /// <returns>Standard HRESULT error code. For resources that don't have a concept of the last modified time, the implementation of GetLastWriteTime should return E_NOTIMPL.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLastWriteTime(
            [In] IDWriteRemoteFontFileStream* This,
            [Out, ComAliasName("UINT64")] ulong* lastWriteTime
        );
        #endregion

        #region Delegates
        /// <summary>GetLocalFileSize returns the number of bytes of the font file that are currently local, which should always be less than or equal to the full file size returned by GetFileSize. If the locality is remote, the return value is zero. If the file is fully local, the return value must be the same as GetFileSize.</summary>
        /// <param name="localFileSize">Receives the local size of the file.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLocalFileSize(
            [Out, ComAliasName("UINT64")] ulong* localFileSize
        );

        /// <summary>GetFileFragmentLocality returns information about the locality of a byte range (i.e., font fragment) within the font file stream.</summary>
        /// <param name="fileOffset">Offset of the fragment from the beginning of the font file.</param>
        /// <param name="fragmentSize">Size of the fragment in bytes.</param>
        /// <param name="isLocal">Receives TRUE if the first byte of the fragment is local, FALSE if not.</param>
        /// <param name="partialSize">Receives the number of contiguous bytes from the start of the fragment that have the same locality as the first byte.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFileFragmentLocality(
            [In, ComAliasName("UINT64")] ulong fileOffset,
            [In, ComAliasName("UINT64")] ulong fragmentSize,
            [Out, ComAliasName("BOOL")] int* isLocal,
            [Out, ComAliasName("UINT64")] ulong* partialSize
        );

        /// <summary>Gets the current locality of the file.</summary>
        /// <returns> Returns the locality enumeration (i.e., remote, partial, or local).</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_LOCALITY GetLocality(
        );

        /// <summary>BeginDownload begins downloading all or part of the font file.</summary>
        /// <param name="fileFragments">Array of public structures, each specifying a byte range to download.</param>
        /// <param name="fragmentCount">Number of elements in the fileFragments array. This can be zero to just download file information, such as the size.</param>
        /// <param name="asyncResult">Receives an object that can be used to wait for the asynchronous download to complete and to get the download result upon completion. The result may be NULL if the download completes synchronously. For example, this can happen if method determines that the requested data is already local.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int BeginDownload(
            [In, ComAliasName("GUID")] Guid* downloadOperationID,
            [In, ComAliasName("DWRITE_FILE_FRAGMENT[]")] DWRITE_FILE_FRAGMENT* fileFragments,
            [In, ComAliasName("UINT32")] uint fragmentCount,
            [Out] IDWriteAsyncResult** asyncResult
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

            #region IDWriteFontFileStream Fields
            public IntPtr ReadFileFragment;

            public IntPtr ReleaseFileFragment;

            public IntPtr GetFileSize;

            public IntPtr GetLastWriteTime;
            #endregion

            #region Fields
            public IntPtr GetLocalFileSize;

            public IntPtr GetFileFragmentLocality;

            public IntPtr GetLocality;

            public IntPtr BeginDownload;
            #endregion
        }
        #endregion
    }
}
