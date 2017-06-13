// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgicommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public struct DXGI_SAMPLE_DESC
    {
        #region Constants
        public const uint STANDARD_MULTISAMPLE_QUALITY_PATTERN = 0xFFFFFFFF;

        public const uint CENTER_MULTISAMPLE_QUALITY_PATTERN = 0xFFFFFFFE;
        #endregion

        #region Fields
        public uint Count;

        public uint Quality;
        #endregion
    }
}
