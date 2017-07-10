// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct WICImageParameters
    {
        #region Fields
        public D2D1_PIXEL_FORMAT pixelFormat;

        public FLOAT dpiX;

        public FLOAT dpiY;

        public FLOAT Top;

        public FLOAT Left;

        public UINT32 PixelWidth;

        public UINT32 PixelHeight;
        #endregion
    }
}
