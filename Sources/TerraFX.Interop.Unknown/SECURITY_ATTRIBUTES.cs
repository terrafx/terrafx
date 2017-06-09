// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wtypesbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)] // Size = 12 or 20
    public struct SECURITY_ATTRIBUTES
    {
        #region Fields
        public uint nLength;

        public IntPtr lpSecurityDescriptor;

        public int /* BOOL */ bInheritHandle;
        #endregion
    }
}
