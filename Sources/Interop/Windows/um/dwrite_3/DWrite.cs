// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public static partial class DWrite
    {
        #region Constants
        /// <summary>A font resource could not be accessed because it was remote. This can happen when calling CreateFontFace on a non-local font or trying to measure/draw glyphs that are not downloaded yet.</summary>
        public static readonly HRESULT DWRITE_E_REMOTEFONT = (HRESULT)(0x8898500D);

        /// <summary>The download was canceled, which happens if the application calls IDWriteFontDownloadQueue::CancelDownload before they finish.</summary>
        public static readonly HRESULT DWRITE_E_DOWNLOADCANCELLED = (HRESULT)(0x8898500E);

        /// <summary>The download failed to complete because the remote resource is missing or the network is down.</summary>
        public static readonly HRESULT DWRITE_E_DOWNLOADFAILED = (HRESULT)(0x8898500F);

        /// <summary>A download request was not added or a download failed because there are too many active downloads.</summary>
        public static readonly HRESULT DWRITE_E_TOOMANYDOWNLOADS = (HRESULT)(0x88985010);
        #endregion
    }
}
