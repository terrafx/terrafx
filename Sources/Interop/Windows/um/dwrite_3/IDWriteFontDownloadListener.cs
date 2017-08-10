// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Application-defined callback interface that receives notifications from the font download queue (IDWriteFontDownloadQueue interface). Callbacks will occur on the downloading thread, and objects must be prepared to handle calls on their methods from other threads at any time.</summary>
    [Guid("B06FE5B9-43EC-4393-881B-DBE4DC72FDA7")]
    unsafe public /* blittable */ struct IDWriteFontDownloadListener
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>The DownloadCompleted method is called back on an arbitrary thread when a download operation ends.</summary>
        /// <param name="downloadQueue">Pointer to the download queue interface on which the BeginDownload method was called.</param>
        /// <param name="context">Optional context object that was passed to BeginDownload. AddRef is called on the context object by BeginDownload and Release is called after the DownloadCompleted method returns.</param>
        /// <param name="downloadResult">Result of the download operation.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DownloadCompleted(
            [In] IDWriteFontDownloadListener* This,
            [In] IDWriteFontDownloadQueue* downloadQueue,
            [In, Optional] IUnknown* context,
            [In, ComAliasName("HRESULT")] int downloadResult
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr DownloadCompleted;
            #endregion
        }
        #endregion
    }
}
