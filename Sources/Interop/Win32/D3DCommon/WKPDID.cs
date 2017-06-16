// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\D3DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    public static class WKPDID
    {
        #region Constants
        public static readonly Guid D3DDebugObjectName = new Guid(0x429B8C22, 0x9188, 0x4B0C, 0x87, 0x42, 0xAC, 0xB0, 0xBF, 0x85, 0xC2, 0x00);

        public static readonly Guid D3DDebugObjectNameW = new Guid(0x4CCA5FD8, 0x921F, 0x42C8, 0x85, 0x66, 0x70, 0xCA, 0xF2, 0xA9, 0xB7, 0x41);

        public static readonly Guid CommentStringW = new Guid(0xD0149DC0, 0x90E8, 0x4EC8, 0x81, 0x44, 0xE9, 0x00, 0xAD, 0x26, 0x6B, 0xB2);
        #endregion
    }
}
