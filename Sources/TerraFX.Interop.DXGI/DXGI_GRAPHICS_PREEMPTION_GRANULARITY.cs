// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.DXGI
{
    public enum DXGI_GRAPHICS_PREEMPTION_GRANULARITY
    {
        DXGI_GRAPHICS_PREEMPTION_DMA_BUFFER_BOUNDARY,
        DXGI_GRAPHICS_PREEMPTION_PRIMITIVE_BOUNDARY,
        DXGI_GRAPHICS_PREEMPTION_TRIANGLE_BOUNDARY,
        DXGI_GRAPHICS_PREEMPTION_PIXEL_BOUNDARY,
        DXGI_GRAPHICS_PREEMPTION_INSTRUCTION_BOUNDARY,
    }
}
