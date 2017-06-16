// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    unsafe public struct DXGI_ADAPTER_DESC1
    {
        #region Fields
        public DXGI_ADAPTER_DESC BaseValue;

        public DXGI_ADAPTER_FLAG Flags;
        #endregion
    }
}
