// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2derr.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using static TerraFX.Interop.Windows;

namespace TerraFX.Interop
{
    public static partial class D2D1
    {
        #region Constants
        public const int FACILITY_D2D = 0x899;

        #region D2DERR_*
        public static readonly HRESULT D2DERR_UNSUPPORTED_PIXEL_FORMAT = WINCODEC_ERR_UNSUPPORTEDPIXELFORMAT;

        public static readonly HRESULT D2DERR_INSUFFICIENT_BUFFER = (HRESULT)(ERROR_INSUFFICIENT_BUFFER | (FACILITY_WIN32 << 16) | 0x80000000);

        public static readonly HRESULT D2DERR_FILE_NOT_FOUND = (HRESULT)(ERROR_FILE_NOT_FOUND | (FACILITY_WIN32 << 16) | 0x80000000);
        #endregion
        #endregion
    }
}
