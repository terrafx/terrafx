// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics
{
    /// <summary>Defines the CPU access capabilities for a <see cref="IGraphicsResource" />.</summary>
    public enum GraphicsResourceCpuAccess
    {
        /// <summary>The graphics resource has no CPU access, it is only accessible from the GPU.</summary>
        /// <remarks>This is an alternative name for <see cref="GpuOnly" />.</remarks>
        None = 0,

        /// <summary>The graphics resource has no CPU access, it is only accessible from the GPU.</summary>
        /// <remarks>This is an alternative name for <see cref="None" />.</remarks>
        GpuOnly = None,

        /// <summary>The graphics resource has CPU read access and can be used to transfer data from the GPU to the CPU.</summary>
        /// <remarks>This is an alternative name for <see cref="GpuToCpu" />.</remarks>
        Read = 1,

        /// <summary>The graphics resource has CPU read access and can be used to transfer data from the GPU to the CPU.</summary>
        /// <remarks>This is an alternative name for <see cref="Read" />.</remarks>
        GpuToCpu = Read,

        /// <summary>The graphics resource has CPU write access and can be used to transfer data from the CPU to the GPU.</summary>
        /// <remarks>This is an alternative name for <see cref="CpuToGpu" />.</remarks>
        Write = 2,

        /// <summary>The graphics resource has CPU write access and can be used to transfer data from the CPU to the GPU.</summary>
        /// <remarks>This is an alternative name for <see cref="CpuToGpu" />.</remarks>
        CpuToGpu = Write,
    }
}
