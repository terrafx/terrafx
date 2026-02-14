// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Graphics.Advanced;
using TerraFX.Threading;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.GraphicsUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A view of memory in a graphics resource.</summary>
public abstract unsafe class GraphicsResourceView : IDisposable, INameable
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsDevice _device;
    private readonly GraphicsResource _resource;
    private readonly GraphicsService _service;

    private readonly nuint _byteOffset;
    private readonly nuint _byteLength;
    private readonly uint _bytesPerElement;

    private readonly uint _d3d12SubresourceIndex;

    private readonly GraphicsResourceKind _kind;

    private string _name;
    private VolatileState _state;

    private protected GraphicsResourceView(GraphicsBuffer buffer, in GraphicsBufferViewCreateOptions createOptions, in GraphicsMemoryRegion memoryRegion) : this(buffer)
    {
        _byteOffset = memoryRegion.ByteOffset;
        _byteLength = memoryRegion.ByteLength;
        _bytesPerElement = createOptions.BytesPerElement;

        _d3d12SubresourceIndex = 0;
    }

    private protected GraphicsResourceView(GraphicsTexture texture, in GraphicsTextureViewCreateOptions createOptions) : this(texture)
    {
        var d3d12ResourceDesc = texture.D3D12Resource->GetDesc();
        var d3d12SubresourceIndex = d3d12ResourceDesc.CalcSubresource(createOptions.MipLevelStart, ArraySlice: 0, PlaneSlice: 0);

        var d3d12PlacedSubresourceFootprints = texture.D3D12PlacedSubresourceFootprints.Slice(createOptions.MipLevelStart, createOptions.MipLevelCount);
        var byteLength = 0UL;

        Device.D3D12Device->GetCopyableFootprints(
            &d3d12ResourceDesc,
            FirstSubresource: d3d12SubresourceIndex,
            NumSubresources: createOptions.MipLevelCount,
            BaseOffset: 0,
            pLayouts: null,
            pNumRows: null,
            pRowSizeInBytes: null,
            pTotalBytes: &byteLength
        );

        ref readonly var d3d12PlacedSubresourceFootprint = ref d3d12PlacedSubresourceFootprints.GetReferenceUnsafe(0);

        _byteLength = (nuint)byteLength;
        _byteOffset = (nuint)d3d12PlacedSubresourceFootprint.Offset;
        _bytesPerElement = texture.PixelFormat.GetSize();

        _d3d12SubresourceIndex = d3d12SubresourceIndex;
    }

    private GraphicsResourceView(GraphicsResource resource)
    {
        AssertNotNull(resource);
        _resource = resource;

        var device = resource.Device;
        _device = device;

        var adapter = device.Adapter;
        _adapter = adapter;

        var service = adapter.Service;
        _service = service;

        _kind = resource.Kind;

        _name = GetType().Name;
        _ = _state.Transition(VolatileState.Initialized);
    }

    /// <summary>Gets the adapter for which the object was created.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the length, in bytes, of the resource view.</summary>
    public nuint ByteLength => _byteLength;

    /// <summary>Gets the offset, in bytes, of the resource view.</summary>
    public nuint ByteOffset => _byteOffset;

    /// <summary>Gets the number of bytes per element in the resource view.</summary>
    public uint BytesPerElement => _bytesPerElement;

    /// <summary>Gets the device for which the object was created.</summary>
    public GraphicsDevice Device => _device;

    /// <summary>Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.</summary>
    public bool IsDisposed => _state.IsDisposedOrDisposing;

    /// <summary>Gets the resource view kind.</summary>
    public GraphicsResourceKind Kind => _kind;

    /// <inheritdoc />
    [AllowNull]
    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value ?? GetType().Name;
        }
    }

    /// <summary>Gets the resource for which the object was created.</summary>
    public GraphicsResource Resource => _resource;

    /// <summary>Gets the service for which the object was created.</summary>
    public GraphicsService Service => _service;

    internal uint D3D12SubresourceIndex => _d3d12SubresourceIndex;

    /// <inheritdoc />
    public void Dispose()
    {
        _ = _state.BeginDispose();
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        _state.EndDispose();
    }

    /// <summary>Maps the resource view into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer resource.</typeparam>
    /// <returns>An unmanaged span that represents the mapped buffer resource.</returns>
    /// <exception cref="ObjectDisposedException">The resource view has been disposed.</exception>
    public UnmanagedSpan<T> Map<T>()
        where T : unmanaged
    {
        ThrowIfDisposedOrDisposing(_state, _name);

        var mappedAddress = MapUnsafe();
        return new UnmanagedSpan<T>((T*)mappedAddress, ByteLength / SizeOf<T>());
    }

    /// <summary>Maps the buffer view into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <returns>An unmanaged span that represents the mapped resource.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer view has been disposed.</exception>
    public UnmanagedSpan<T> Map<T>(nuint start)
        where T : unmanaged
    {
        ThrowIfDisposedOrDisposing(_state, _name);

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = ByteLength - byteStart;

        var mappedAddress = MapUnsafe();
        return new UnmanagedSpan<T>((T*)(mappedAddress + byteStart), byteLength / SizeOf<T>());
    }

    /// <summary>Maps the buffer view into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <param name="length">The number of elements in the mapping.</param>
    /// <returns>An unmanaged span that represents the mapped buffer view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(<paramref name="length" /> + <paramref name="start" />) * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer view has been disposed.</exception>
    public UnmanagedSpan<T> Map<T>(nuint start, nuint length)
        where T : unmanaged
    {
        ThrowIfDisposedOrDisposing(_state, _name);

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = SizeOf<T>() * length;
        ThrowIfNotInInsertBounds(byteLength, ByteLength - byteStart);

        var mappedAddress = MapUnsafe();
        return new UnmanagedSpan<T>((T*)(mappedAddress + byteStart), byteLength / SizeOf<T>());
    }

    /// <summary>Maps the resource view into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the resource view.</typeparam>
    /// <returns>An unmanaged span that represents the mapped resource view.</returns>
    /// <exception cref="ObjectDisposedException">The resource view has been disposed.</exception>
    public UnmanagedSpan<T> MapForRead<T>()
        where T : unmanaged
    {
        ThrowIfDisposedOrDisposing(_state, _name);

        var mappedAddress = MapForReadUnsafe();
        return new UnmanagedSpan<T>((T*)mappedAddress, ByteLength / SizeOf<T>());
    }

    /// <summary>Maps the buffer view into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <returns>An unmanaged span that represents the mapped buffer view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer view has been disposed.</exception>
    public UnmanagedSpan<T> MapForRead<T>(nuint start)
        where T : unmanaged
    {
        ThrowIfDisposedOrDisposing(_state, _name);

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = ByteLength - byteStart;

        var mappedAddress = MapForReadUnsafe(byteStart, byteLength);
        return new UnmanagedSpan<T>((T*)(mappedAddress + byteStart), byteLength / SizeOf<T>());
    }

    /// <summary>Maps the buffer view into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <param name="length">The number of elements in the mapping.</param>
    /// <returns>An unmanaged span that represents the mapped buffer view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(<paramref name="length" /> + <paramref name="start" />) * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer view has been disposed.</exception>
    public UnmanagedSpan<T> MapForRead<T>(nuint start, nuint length)
        where T : unmanaged
    {
        ThrowIfDisposedOrDisposing(_state, _name);

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = SizeOf<T>() * length;
        ThrowIfNotInInsertBounds(byteLength, ByteLength - byteStart);

        var mappedAddress = MapForReadUnsafe();
        return new UnmanagedSpan<T>((T*)(mappedAddress + byteStart), byteLength / SizeOf<T>());
    }

    /// <inheritdoc />
    public override string ToString() => _name;

    /// <summary>Unmaps the resource view from CPU memory.</summary>
    /// <remarks>This overload should be used when no memory was written.</remarks>
    /// <exception cref="InvalidOperationException">The resource view is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The resource view has been disposed.</exception>
    public void Unmap()
    {
        ThrowIfDisposedOrDisposing(_state, _name);
        UnmapUnsafe();
    }

    /// <summary>Unmaps the resource view from CPU memory and writes the entire mapped region.</summary>
    /// <remarks>This overload should be used when all memory was written.</remarks>
    /// <exception cref="InvalidOperationException">The resource view is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The resource view has been disposed.</exception>
    public void UnmapAndWrite()
    {
        ThrowIfDisposedOrDisposing(_state, _name);
        UnmapAndWriteUnsafe();
    }

    /// <summary>Unmaps the buffer view from CPU memory and writes a region of memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="InvalidOperationException">The buffer view is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The buffer view has been disposed.</exception>
    public void UnmapAndWrite<T>(nuint start)
        where T : unmanaged
    {
        ThrowIfDisposedOrDisposing(_state, _name);

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = ByteLength - byteStart;

        UnmapAndWriteUnsafe(byteStart, byteLength);
    }

    /// <summary>Unmaps the buffer view from CPU memory and writes a region of memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <param name="length">The number of elements in the mapping.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(<paramref name="length" /> + <paramref name="start" />) * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="InvalidOperationException">The buffer view is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The buffer view has been disposed.</exception>
    public void UnmapAndWrite<T>(nuint start, nuint length)
        where T : unmanaged
    {
        ThrowIfDisposedOrDisposing(_state, _name);

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = SizeOf<T>() * length;
        ThrowIfNotInInsertBounds(byteLength, ByteLength - byteStart);

        UnmapAndWriteUnsafe();
    }

    /// <inheritdoc />
    protected abstract void Dispose(bool isDisposing);

    private protected abstract byte* MapUnsafe();

    private protected abstract byte* MapForReadUnsafe();

    private protected abstract byte* MapForReadUnsafe(nuint byteStart, nuint byteLength);

    private protected abstract void UnmapUnsafe();

    private protected abstract void UnmapAndWriteUnsafe();

    private protected abstract void UnmapAndWriteUnsafe(nuint byteStart, nuint byteLength);
}
