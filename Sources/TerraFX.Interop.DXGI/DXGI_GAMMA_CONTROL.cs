// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_GAMMA_CONTROL
    {
        #region Fields
        public DXGI_RGB Scale;

        public DXGI_RGB Offset;

        // TODO: Change to a fixed-sized array when C# supports it (https://github.com/dotnet/csharplang/issues/78)
        //       Additionally, set Size = 12324
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1025)]
        public DXGI_RGB[] GammaCurve;
        #endregion
    }
}
