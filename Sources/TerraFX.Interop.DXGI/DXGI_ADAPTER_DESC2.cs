// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    unsafe public struct DXGI_ADAPTER_DESC2
    {
        #region Fields
        public DXGI_ADAPTER_DESC1 BaseValue;

        public DXGI_GRAPHICS_PREEMPTION_GRANULARITY GraphicsPreemptionGranularity;

        public DXGI_COMPUTE_PREEMPTION_GRANULARITY ComputePreemptionGranularity;
        #endregion
    }
}
