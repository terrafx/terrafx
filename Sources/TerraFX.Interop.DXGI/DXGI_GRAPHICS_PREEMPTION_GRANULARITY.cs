// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.DXGI
{
    public enum DXGI_GRAPHICS_PREEMPTION_GRANULARITY
    {
        DMA_BUFFER_BOUNDARY = 0,

        PRIMITIVE_BOUNDARY = 1,

        TRIANGLE_BOUNDARY = 2,

        PIXEL_BOUNDARY = 3,

        INSTRUCTION_BOUNDARY = 4
    }
}
