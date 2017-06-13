// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_6.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    unsafe public struct DXGI_OUTPUT_DESC1
    {
        #region Fields
        public DXGI_OUTPUT_DESC BaseValue;

        public uint BitsPerColor;

        public DXGI_COLOR_SPACE_TYPE ColorSpace;

        public fixed float RedPrimary[2];

        public fixed float GreenPrimary[2];

        public fixed float BluePrimary[2];

        public fixed float WhitePoint[2];

        public float MinLuminance;

        public float MaxLuminance;

        public float MaxFullFrameLuminance;
        #endregion
    }
}
