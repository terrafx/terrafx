// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ struct WICImageParameters
    {
        #region Fields
        public D2D1_PIXEL_FORMAT pixelFormat;

        [ComAliasName("FLOAT")]
        public float dpiX;

        [ComAliasName("FLOAT")]
        public float dpiY;

        [ComAliasName("FLOAT")]
        public float Top;

        [ComAliasName("FLOAT")]
        public float Left;

        [ComAliasName("UINT32")]
        public uint PixelWidth;

        [ComAliasName("UINT32")]
        public uint PixelHeight;
        #endregion
    }
}
