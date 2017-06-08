// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 24)]
    public struct DXGI_OUTDUPL_POINTER_SHAPE_INFO
    {
        #region Fields
        public uint Type;

        public uint Width;

        public uint Height;

        public uint Pitch;

        public POINT HotSpot;
        #endregion
    }
}
