// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct WICBitmapPlane
    {
        #region Fields
        [ComAliasName("WICPixelFormatGUID")]
        public Guid Format;

        [ComAliasName("BYTE[]")]
        public byte* pbBuffer;

        [ComAliasName("UINT")]
        public uint cbStride;

        [ComAliasName("UINT")]
        public uint cbBufferSize;
        #endregion
    }
}
