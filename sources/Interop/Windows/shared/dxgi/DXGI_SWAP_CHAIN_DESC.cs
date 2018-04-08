// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ unsafe struct DXGI_SWAP_CHAIN_DESC
    {
        #region Fields
        public DXGI_MODE_DESC BufferDesc;

        public DXGI_SAMPLE_DESC SampleDesc;

        [ComAliasName("DXGI_USAGE")]
        public uint BufferUsage;

        [ComAliasName("UINT")]
        public uint BufferCount;

        [ComAliasName("HWND")]
        public IntPtr OutputWindow;

        [ComAliasName("BOOL")]
        public int Windowed;

        public DXGI_SWAP_EFFECT SwapEffect;

        [ComAliasName("UINT")]
        public uint Flags;
        #endregion
    }
}
