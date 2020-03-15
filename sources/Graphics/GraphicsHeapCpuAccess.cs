// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics
{
    /// <summary>Defines the CPU access capabilities for a <see cref="GraphicsHeap" />.</summary>
    public enum GraphicsHeapCpuAccess
    {
        /// <summary>The graphics heap has no CPU access.</summary>
        None = 0,

        /// <summary>The graphics heap has CPU read access.</summary>
        Read = 1,

        /// <summary>The graphics heap has CPU write access.</summary>
        Write = 2,
    }
}
