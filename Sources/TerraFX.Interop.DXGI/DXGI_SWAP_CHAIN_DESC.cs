// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)] // Size = 60 or 68
    public struct DXGI_SWAP_CHAIN_DESC
    {
        #region Constants
        public const uint MAX_SWAP_CHAIN_BUFFERS = 16;
        #endregion

        #region Fields
        public DXGI_MODE_DESC BufferDesc;

        public DXGI_SAMPLE_DESC SampleDesc;

        public DXGI_USAGE BufferUsage;

        public uint BufferCount;

        public IntPtr /* HWND */ OutputWindow;

        public int /* BOOL */ Windowed;

        public DXGI_SWAP_EFFECT SwapEffect;

        public DXGI_SWAP_CHAIN_FLAG Flags;
        #endregion
    }
}
