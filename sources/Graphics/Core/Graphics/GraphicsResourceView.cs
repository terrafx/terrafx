// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A view into a graphics resource.</summary>
[StructLayout(LayoutKind.Auto)]
public readonly unsafe struct GraphicsResourceView
{
    /// <summary>Gets the offset of the view, in bytes.</summary>
    public nuint Offset { get; init; }

    /// <summary>Gets the resource which contains the view.</summary>
    public GraphicsResource? Resource { get; init; }

    /// <summary>Gets the size of the view, in bytes.</summary>
    public uint Size { get; init; }

    /// <summary>Gets the stride of the view, in bytes.</summary>
    public uint Stride { get; init; }

    /// <summary>Maps the resource view into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource view.</typeparam>
    /// <returns>A pointer to the mapped resource view.</returns>
    public T* Map<T>()
        where T : unmanaged
    {
        ThrowIfNull(Resource);
        return Resource.Map<T>(Offset, Size);
    }

    /// <summary>Maps the resource view into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource view .</typeparam>
    /// <returns>A pointer to the mapped resource view .</returns>
    public T* MapForRead<T>()
        where T : unmanaged
    {
        ThrowIfNull(Resource);
        return Resource.MapForRead<T>(Offset, Size);
    }

    /// <summary>Unmaps the resource view from CPU memory.</summary>
    /// <remarks>This overload should be used when no memory was written.</remarks>
    public void Unmap()
    {
        ThrowIfNull(Resource);
        Resource.Unmap();
    }

    /// <summary>Unmaps the resource view from CPU memory.</summary>
    public void UnmapAndWrite()
    {

        ThrowIfNull(Resource);
        Resource.UnmapAndWrite(Offset, Size);
    }
}
