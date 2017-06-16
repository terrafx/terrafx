// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    unsafe public struct DXGI_GAMMA_CONTROL_CAPABILITIES
    {
        #region Fields
        public BOOL ScaleAndOffsetSupported;

        public float MaxConvertedValue;

        public float MinConvertedValue;

        public uint NumGammaControlPoints;

        public fixed float ControlPointPositions[1025];
        #endregion
    }
}
