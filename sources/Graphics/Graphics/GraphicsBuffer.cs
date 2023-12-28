// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Advanced;
using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics buffer which can hold data for a graphics device.</summary>
public sealed unsafe class GraphicsBuffer : GraphicsResource
{
    private ValueList<GraphicsBufferView> _bufferViews;
    private readonly ValueMutex _bufferViewsMutex;

    private ulong _d3d12GpuVirtualAddress;

    private readonly GraphicsBufferKind _kind;

    private GraphicsMemoryAllocator _memoryAllocator;

    internal GraphicsBuffer(GraphicsDevice device, in GraphicsBufferCreateOptions createOptions) : base(device, in createOptions)
    {
        device.AddBuffer(this);

        _bufferViews = new ValueList<GraphicsBufferView>();
        _bufferViewsMutex = new ValueMutex();

        _d3d12GpuVirtualAddress = D3D12Resource->GetGPUVirtualAddress();

        _kind = createOptions.Kind;

        var memoryAllocatorCreateOptions = new GraphicsMemoryAllocatorCreateOptions {
            ByteLength = MemoryRegion.ByteLength,
            IsDedicated = false,
            OnFree = default,
        };

        _memoryAllocator = createOptions.CreateMemorySuballocator.IsNotNull
                         ? createOptions.CreateMemorySuballocator.Invoke(this, in memoryAllocatorCreateOptions)
                         : GraphicsMemoryAllocator.CreateDefault(this, in memoryAllocatorCreateOptions);
    }

    /// <summary>Gets the buffer kind.</summary>
    public new GraphicsBufferKind Kind => _kind;

    internal ulong D3D12GpuVirtualAddress => _d3d12GpuVirtualAddress;

    /// <summary>Creates a view of the buffer.</summary>
    /// <param name="createOptions">The options to use when creating the buffer view.</param>
    /// <returns>The created buffer view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferViewCreateOptions.BytesPerElement" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferViewCreateOptions.ElementCount" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and <see cref="GraphicsBufferViewCreateOptions.BytesPerElement" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public GraphicsBufferView CreateBufferView(in GraphicsBufferViewCreateOptions createOptions)
    {
        if (!TryCreateBufferView(in createOptions, out var bufferView))
        {
            ThrowOutOfMemoryException(createOptions.ElementCount, createOptions.BytesPerElement);
        }
        return bufferView;
    }

    /// <summary>Creates a view of the buffer.</summary>
    /// <param name="elementCount">The number of elements in the buffer view.</param>
    /// <param name="bytesPerElement">The number of bytes per element in the buffer view.</param>
    /// <returns>The created buffer view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="bytesPerElement" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="elementCount" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and <paramref name="bytesPerElement" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBufferView CreateBufferView(uint elementCount, uint bytesPerElement)
    {
        if (!TryCreateBufferView(elementCount, bytesPerElement, out var bufferView))
        {
            ThrowOutOfMemoryException(elementCount, bytesPerElement);
        }
        return bufferView;
    }

    /// <summary>Creates a view of the buffer.</summary>
    /// <typeparam name="T">The type used to compute the number of bytes per element in the buffer view.</typeparam>
    /// <param name="elementCount">The number of elements, of type <typeparamref name="T" />, in the buffer view.</param>
    /// <returns>The created buffer view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="elementCount" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and the size of <typeparamref name="T" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBufferView CreateBufferView<T>(uint elementCount)
    {
        if (!TryCreateBufferView<T>(elementCount, out var bufferView))
        {
            ThrowOutOfMemoryException(elementCount);
        }
        return bufferView;
    }

    /// <summary>Disposes all buffer views allocated by the buffer.</summary>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public void DisposeAllViews()
    {
        ThrowIfDisposed();
        DisposeAllViewsUnsafe();
    }

    /// <summary>Tries to creates a view of the buffer.</summary>
    /// <param name="createOptions">The options to use when creating the buffer view.</param>
    /// <param name="bufferView">On return, contains the buffer view if it was successfully created; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the view was successfully created; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferViewCreateOptions.BytesPerElement" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferViewCreateOptions.ElementCount" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and <see cref="GraphicsBufferViewCreateOptions.BytesPerElement" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public bool TryCreateBufferView(in GraphicsBufferViewCreateOptions createOptions, [NotNullWhen(true)] out GraphicsBufferView? bufferView)
    {
        ThrowIfDisposed();

        ThrowIfZero(createOptions.BytesPerElement);
        ThrowIfZero(createOptions.ElementCount);

        if ((Kind == GraphicsBufferKind.Index) && (createOptions.BytesPerElement != 2) && (createOptions.BytesPerElement != 4))
        {
            ThrowForInvalidKind(Kind);
        }

        return TryCreateBufferViewUnsafe(in createOptions, out bufferView);
    }

    /// <summary>Tries to creates a view of the buffer.</summary>
    /// <param name="elementCount">The number of elements in the buffer view.</param>
    /// <param name="bytesPerElement">The number of bytes per element in the buffer view.</param>
    /// <param name="bufferView">On return, contains the buffer view if it was successfully created; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the buffer view was successfully created; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="bytesPerElement" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="elementCount" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and <paramref name="bytesPerElement" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public bool TryCreateBufferView(uint elementCount, uint bytesPerElement, [NotNullWhen(true)] out GraphicsBufferView? bufferView)
    {
        ThrowIfDisposed();

        ThrowIfZero(bytesPerElement);
        ThrowIfZero(elementCount);

        if ((Kind == GraphicsBufferKind.Index) && (bytesPerElement != 2) && (bytesPerElement != 4))
        {
            ThrowForInvalidKind(Kind);
        }

        var createOptions = new GraphicsBufferViewCreateOptions {
            BytesPerElement = bytesPerElement,
            ElementCount = elementCount,
        };
        return TryCreateBufferViewUnsafe(in createOptions, out bufferView);
    }

    /// <summary>Tries to creates a view of the buffer.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in the buffer view.</typeparam>
    /// <param name="elementCount">The number of elements, of type <typeparamref name="T" />, in the buffer view.</param>
    /// <param name="bufferView">On return, contains the buffer view if it was successfully created; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the view was successfully created; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="elementCount" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and the size of <typeparamref name="T" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public bool TryCreateBufferView<T>(uint elementCount, [NotNullWhen(true)] out GraphicsBufferView? bufferView)
    {
        ThrowIfDisposed();

        ThrowIfZero(elementCount);

        var bytesPerElement = SizeOf<T>();

        if ((Kind == GraphicsBufferKind.Index) && (bytesPerElement != 2) && (bytesPerElement != 4))
        {
            ThrowForInvalidKind(Kind);
        }

        var createOptions = new GraphicsBufferViewCreateOptions {
            BytesPerElement = bytesPerElement,
            ElementCount = elementCount,
        };
        return TryCreateBufferViewUnsafe(in createOptions, out bufferView);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            DisposeAllViewsUnsafe();

            _memoryAllocator.Clear();
            _memoryAllocator = null!;
        }
        _bufferViewsMutex.Dispose();

        base.Dispose(isDisposing);
        _d3d12GpuVirtualAddress = 0;

        _ = Device.RemoveBuffer(this);
    }

    internal void AddBufferView(GraphicsBufferView bufferView) => _bufferViews.Add(bufferView, _bufferViewsMutex);

    internal bool RemoveBufferView(GraphicsBufferView bufferView) => IsDisposed || _bufferViews.Remove(bufferView, _bufferViewsMutex);

    private void DisposeAllViewsUnsafe() => _bufferViews.Dispose();

    private bool TryCreateBufferViewUnsafe(in GraphicsBufferViewCreateOptions createOptions, [NotNullWhen(true)] out GraphicsBufferView? bufferView)
    {
        nuint byteLength = createOptions.BytesPerElement;
        byteLength *= createOptions.ElementCount;
        nuint byteAlignment = 0;

        if (Kind == GraphicsBufferKind.Index)
        {
            byteAlignment = createOptions.BytesPerElement;
            byteLength = AlignUp(byteLength, createOptions.BytesPerElement);
        }
        else if (Kind == GraphicsBufferKind.Constant)
        {
            byteAlignment = D3D12_CONSTANT_BUFFER_DATA_PLACEMENT_ALIGNMENT;
            byteLength = AlignUp(byteLength, D3D12_CONSTANT_BUFFER_DATA_PLACEMENT_ALIGNMENT);
        }
        else if (Kind == GraphicsBufferKind.Default)
        {
            byteAlignment = D3D12_TEXTURE_DATA_PLACEMENT_ALIGNMENT;
        }

        var succeeded = _memoryAllocator.TryAllocate(byteLength, byteAlignment, out var memoryRegion);
        bufferView = succeeded ? new GraphicsBufferView(this, in createOptions, in memoryRegion) : null;

        return succeeded;
    }   
}
