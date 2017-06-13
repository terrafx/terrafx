// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    public struct DXGI_DEBUG_ID
    {
        public static readonly Guid ALL = new Guid(0xE48AE283, 0xDA80, 0x490B, 0x87, 0xE6, 0x43, 0xE9, 0xA9, 0xCF, 0xDA, 0x08);
                                    
        public static readonly Guid DX = new Guid(0x35CDD7FC, 0x13B2, 0x421D, 0xA5, 0xD7, 0x7E, 0x44, 0x51, 0x28, 0x7D, 0x64);
                                    
        public static readonly Guid DXGI = new Guid(0x25CDDAA4, 0xB1C6, 0x47E1, 0xAC, 0x3E, 0x98, 0x87, 0x5B, 0x5A, 0x2E, 0x2A);
                                    
        public static readonly Guid APP = new Guid(0x06CD6E01, 0x4219, 0x4EBD, 0x87, 0x09, 0x27, 0xED, 0x23, 0x36, 0x0C, 0x62);

        #region Fields
        public Guid Value;
        #endregion
    }
}
