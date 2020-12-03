// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics
{
    /// <summary>Defines a single block of memory which can contain allocated or free regions.</summary>
    public interface IGraphicsMemoryBlock : IGraphicsMemoryRegionCollection<IGraphicsMemoryBlock>, IDisposable
    {
        /// <summary>Gets the block collection which contains the memory block.</summary>
        GraphicsMemoryBlockCollection Collection { get; }
    }
}
