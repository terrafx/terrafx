// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D_FEATURE_LEVEL;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_FEATURE;
using static TerraFX.Interop.DirectX.D3D12_HEAP_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_HEAP_TIER;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsDevice : GraphicsDevice
{
    private readonly uint _d3d12CbvSrvUavDescriptorHandleIncrementSize;
    private readonly ID3D12CommandQueue* _d3d12CommandQueue;
    private readonly ID3D12Device* _d3d12Device;
    private readonly D3D12_FEATURE_DATA_D3D12_OPTIONS _d3d12Options;
    private readonly uint _d3d12RtvDescriptorHandleIncrementSize;
    private readonly bool _d3d12SupportsResourceHeapTier2;
    private readonly D3D12GraphicsMemoryManager[] _memoryManagers;
    private readonly D3D12GraphicsFence _waitForIdleFence;

    private string _name = null!;
    private ContextPool<D3D12GraphicsDevice, D3D12GraphicsRenderContext> _renderContextPool;
    private VolatileState _state;

    internal D3D12GraphicsDevice(D3D12GraphicsAdapter adapter, delegate*<GraphicsDeviceObject, ulong, GraphicsMemoryAllocator> createMemoryAllocator)
        : base(adapter)
    {
        var d3d12Device = CreateD3D12Device(adapter);

        _d3d12CommandQueue = CreateD3D12CommandQueue(d3d12Device);
        _d3d12Device = d3d12Device;
        _d3d12Options = GetD3D12Options(d3d12Device);

        _d3d12CbvSrvUavDescriptorHandleIncrementSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV);
        _d3d12RtvDescriptorHandleIncrementSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);

        var d3d12SupportsResourceHeapTier2 = D3D12Options.ResourceHeapTier >= D3D12_RESOURCE_HEAP_TIER_2;
        _d3d12SupportsResourceHeapTier2 = d3d12SupportsResourceHeapTier2;

        _memoryManagers = CreateMemoryManagers(this, createMemoryAllocator, d3d12SupportsResourceHeapTier2);
        // TODO: UpdateBudget
  
        _waitForIdleFence = CreateFence(isSignalled: false);
        _renderContextPool = new ContextPool<D3D12GraphicsDevice, D3D12GraphicsRenderContext>();

        _ = _state.Transition(to: Initialized);
        Name = nameof(D3D12GraphicsDevice);

        static ID3D12CommandQueue* CreateD3D12CommandQueue(ID3D12Device* d3d12Device)
        {
            ID3D12CommandQueue* d3d12CommandQueue;
            var commandQueueDesc = new D3D12_COMMAND_QUEUE_DESC();

            ThrowExternalExceptionIfFailed(d3d12Device->CreateCommandQueue(&commandQueueDesc, __uuidof<ID3D12CommandQueue>(), (void**)&d3d12CommandQueue));
            return d3d12CommandQueue;
        }

        static ID3D12Device* CreateD3D12Device(D3D12GraphicsAdapter adapter)
        {
            ID3D12Device* d3d12Device;
            ThrowExternalExceptionIfFailed(D3D12CreateDevice((IUnknown*)adapter.DxgiAdapter, D3D_FEATURE_LEVEL_11_0, __uuidof<ID3D12Device>(), (void**)&d3d12Device));
            return d3d12Device;
        }

        static D3D12_FEATURE_DATA_D3D12_OPTIONS GetD3D12Options(ID3D12Device* d3d12Device)
        {
            D3D12_FEATURE_DATA_D3D12_OPTIONS d3d12Options;
            ThrowExternalExceptionIfFailed(d3d12Device->CheckFeatureSupport(D3D12_FEATURE_D3D12_OPTIONS, &d3d12Options, SizeOf<D3D12_FEATURE_DATA_D3D12_OPTIONS>()));
            return d3d12Options;
        }

        static D3D12GraphicsMemoryManager[] CreateMemoryManagers(D3D12GraphicsDevice device, delegate*<GraphicsDeviceObject, ulong, GraphicsMemoryAllocator> createMemoryAllocator, bool d3d12SupportsResourceHeapTier2)
        {
            D3D12GraphicsMemoryManager[] memoryManagers;

            if (d3d12SupportsResourceHeapTier2)
            {
                memoryManagers = new D3D12GraphicsMemoryManager[3] {
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_DEFAULT),
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_UPLOAD),
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_READBACK),
                };
            }
            else
            {
                memoryManagers = new D3D12GraphicsMemoryManager[9] {
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_ALLOW_ONLY_BUFFERS, D3D12_HEAP_TYPE_DEFAULT),
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_ALLOW_ONLY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_DEFAULT),
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_ALLOW_ONLY_RT_DS_TEXTURES, D3D12_HEAP_TYPE_DEFAULT),

                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_ALLOW_ONLY_BUFFERS, D3D12_HEAP_TYPE_UPLOAD),
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_ALLOW_ONLY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_UPLOAD),
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_ALLOW_ONLY_RT_DS_TEXTURES, D3D12_HEAP_TYPE_UPLOAD),

                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_ALLOW_ONLY_BUFFERS, D3D12_HEAP_TYPE_READBACK),
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_ALLOW_ONLY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_READBACK),
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_ALLOW_ONLY_RT_DS_TEXTURES, D3D12_HEAP_TYPE_READBACK),
                };
            }

            return memoryManagers;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsDevice" /> class.</summary>
    ~D3D12GraphicsDevice() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the descriptor handle increment size for <see cref="D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV" />.</summary>
    public uint D3D12CbvSrvUavDescriptorHandleIncrementSize => _d3d12CbvSrvUavDescriptorHandleIncrementSize;

    /// <summary>Gets the <see cref="ID3D12CommandQueue" /> used by the device.</summary>
    public ID3D12CommandQueue* D3D12CommandQueue
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12CommandQueue;
        }
    }

    /// <summary>Gets the underlying <see cref="ID3D12Device" /> for the device.</summary>
    public ID3D12Device* D3D12Device
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12Device;
        }
    }

    /// <summary>Gets the <see cref="D3D12_FEATURE_DATA_D3D12_OPTIONS" /> for the device.</summary>
    public ref readonly D3D12_FEATURE_DATA_D3D12_OPTIONS D3D12Options => ref _d3d12Options;

    /// <summary>Gets the descriptor handle increment size for <see cref="D3D12_DESCRIPTOR_HEAP_TYPE_RTV" />.</summary>
    public uint D3D12RtvDescriptorHandleIncrementSize => _d3d12RtvDescriptorHandleIncrementSize;

    /// <summary>Gets <c>true</c> if the device supports <see cref="D3D12_RESOURCE_HEAP_TIER_2" />; otherwise, <c>false</c>.</summary>
    public bool D3D12SupportsResourceHeapTier2 => _d3d12SupportsResourceHeapTier2;

    /// <summary>Gets or sets the name for the device.</summary>
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = D3D12Device->UpdateD3D12Name(value);
            _ = D3D12CommandQueue->UpdateD3D12Name(value);
        }
    }

    /// <inheritdoc cref="GraphicsAdapterObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <summary>Gets a fence that is used to wait for the device to become idle.</summary>
    public D3D12GraphicsFence WaitForIdleFence => _waitForIdleFence;

    /// <inheritdoc />
    public override D3D12GraphicsBuffer CreateBuffer(in GraphicsBufferCreateInfo bufferCreateInfo)
    {
        var d3d12Device = D3D12Device;

        var d3d12ResourceDesc = GetD3D12ResourceDesc(bufferCreateInfo.Size, D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT);
        var d3d12ResourceAllocationInfo = d3d12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &d3d12ResourceDesc);

        var memoryManagerIndex = GetMemoryManagerIndex(bufferCreateInfo.CpuAccess, 0);
        var memoryRegion = _memoryManagers[memoryManagerIndex].Allocate(d3d12ResourceAllocationInfo.SizeInBytes, d3d12ResourceAllocationInfo.Alignment, GraphicsMemoryAllocationFlags.None);

        var d3d12ResourceState = GetD3D12ResourceState(bufferCreateInfo.CpuAccess, bufferCreateInfo.Kind);
        var d3d12Resource = CreateD3D12Resource(d3d12Device, &d3d12ResourceDesc, in memoryRegion, d3d12ResourceState);

        var createInfo = new D3D12GraphicsBuffer.CreateInfo {
            D3D12Resource = d3d12Resource,
            D3D12ResourceState = d3d12ResourceState,
            MemoryRegion = memoryRegion,
            Kind = bufferCreateInfo.Kind,
            ResourceInfo = new GraphicsResourceInfo {
                Alignment = d3d12ResourceAllocationInfo.Alignment,
                CpuAccess = bufferCreateInfo.CpuAccess,
                Size = d3d12ResourceAllocationInfo.SizeInBytes,
            },
        };
        return new D3D12GraphicsBuffer(this, in createInfo);

        static ID3D12Resource* CreateD3D12Resource(ID3D12Device* d3d12Device, D3D12_RESOURCE_DESC* d3d12ResourceDesc, in GraphicsMemoryRegion memoryRegion, D3D12_RESOURCE_STATES d3d12ResourceState)
        {
            ID3D12Resource* d3d12Resource;

            ThrowExternalExceptionIfFailed(d3d12Device->CreatePlacedResource(
                memoryRegion.Allocator.DeviceObject.As<D3D12GraphicsMemoryHeap>().D3D12Heap,
                memoryRegion.Offset,
                d3d12ResourceDesc,
                d3d12ResourceState,
                pOptimizedClearValue: null,
                __uuidof<ID3D12Resource>(),
                (void**)&d3d12Resource
            ));

            return d3d12Resource;
        }

        static D3D12_RESOURCE_DESC GetD3D12ResourceDesc(ulong size, ulong alignment)
        {
            return D3D12_RESOURCE_DESC.Buffer(size, alignment: alignment);
        }

        static D3D12_RESOURCE_STATES GetD3D12ResourceState(GraphicsResourceCpuAccess cpuAccess, GraphicsBufferKind kind)
        {
            return cpuAccess switch {
                GraphicsResourceCpuAccess.Read => D3D12_RESOURCE_STATE_COPY_DEST,
                GraphicsResourceCpuAccess.Write => D3D12_RESOURCE_STATE_GENERIC_READ,
                _ => kind switch {
                    GraphicsBufferKind.Vertex => D3D12_RESOURCE_STATE_VERTEX_AND_CONSTANT_BUFFER | D3D12_RESOURCE_STATE_NON_PIXEL_SHADER_RESOURCE | D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE,
                    GraphicsBufferKind.Index => D3D12_RESOURCE_STATE_INDEX_BUFFER | D3D12_RESOURCE_STATE_NON_PIXEL_SHADER_RESOURCE | D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE,
                    GraphicsBufferKind.Constant => D3D12_RESOURCE_STATE_VERTEX_AND_CONSTANT_BUFFER | D3D12_RESOURCE_STATE_NON_PIXEL_SHADER_RESOURCE | D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE,
                    _ => default,
                },
            };
        }
    }

    /// <inheritdoc />
    public override D3D12GraphicsFence CreateFence(bool isSignalled)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return new D3D12GraphicsFence(this, isSignalled);
    }

    /// <inheritdoc />
    public override D3D12GraphicsPipelineSignature CreatePipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResource> resources = default)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return new D3D12GraphicsPipelineSignature(this, inputs, resources);
    }

    /// <inheritdoc />
    public override D3D12GraphicsPrimitive CreatePrimitive(GraphicsPipeline pipeline, in GraphicsResourceView vertexBufferView, in GraphicsResourceView indexBufferView = default, ReadOnlySpan<GraphicsResourceView> inputResourceViews = default)
        => CreatePrimitive((D3D12GraphicsPipeline)pipeline, in vertexBufferView, in indexBufferView, inputResourceViews);

    /// <inheritdoc cref="CreatePrimitive(GraphicsPipeline, in GraphicsResourceView, in GraphicsResourceView, ReadOnlySpan{GraphicsResourceView})" />
    private D3D12GraphicsPrimitive CreatePrimitive(D3D12GraphicsPipeline pipeline, in GraphicsResourceView vertexBufferView, in GraphicsResourceView indexBufferView = default, ReadOnlySpan<GraphicsResourceView> inputResourceViews = default)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return new D3D12GraphicsPrimitive(this, pipeline, in vertexBufferView, in indexBufferView, inputResourceViews);
    }

    /// <inheritdoc />
    public override D3D12GraphicsShader CreateShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return new D3D12GraphicsShader(this, kind, bytecode, entryPointName);
    }

    /// <inheritdoc />
    public override D3D12GraphicsRenderPass CreateRenderPass(IGraphicsSurface surface, GraphicsFormat renderTargetFormat, uint minimumRenderTargetCount = 0)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return new D3D12GraphicsRenderPass(this, surface, renderTargetFormat, minimumRenderTargetCount);
    }

    /// <inheritdoc />
    public override D3D12GraphicsTexture CreateTexture(in GraphicsTextureCreateInfo textureCreateInfo)
    {
        var d3d12Device = D3D12Device;

        var d3d12ResourceDesc = GetD3D12ResourceDesc(textureCreateInfo.Kind, textureCreateInfo.Format.AsDxgiFormat(), textureCreateInfo.Width, textureCreateInfo.Height, textureCreateInfo.Depth, D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT);
        var d3d12ResourceAllocationInfo = d3d12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &d3d12ResourceDesc);

        var memoryManagerIndex = GetMemoryManagerIndex(textureCreateInfo.CpuAccess, 1);
        var memoryRegion = _memoryManagers[memoryManagerIndex].Allocate(d3d12ResourceAllocationInfo.SizeInBytes, d3d12ResourceAllocationInfo.Alignment, GraphicsMemoryAllocationFlags.None);

        var d3d12ResourceState = GetD3D12ResourceState(textureCreateInfo.CpuAccess, textureCreateInfo.Kind);
        var d3d12Resource = CreateD3D12Resource(d3d12Device, &d3d12ResourceDesc, in memoryRegion, d3d12ResourceState);

        var createInfo = new D3D12GraphicsTexture.CreateInfo {
            D3D12Resource = d3d12Resource,
            D3D12ResourceState = d3d12ResourceState,
            MemoryRegion = memoryRegion,
            ResourceInfo = new GraphicsResourceInfo {
                Alignment = d3d12ResourceAllocationInfo.Alignment,
                CpuAccess = textureCreateInfo.CpuAccess,
                Size = d3d12ResourceAllocationInfo.SizeInBytes,
            },
            TextureInfo = new GraphicsTextureInfo {
                Depth = textureCreateInfo.Depth,
                Format = textureCreateInfo.Format,
                Height = textureCreateInfo.Height,
                Kind = textureCreateInfo.Kind,
                Width = textureCreateInfo.Width,
            },
        };
        return new D3D12GraphicsTexture(this, in createInfo);

        static ID3D12Resource* CreateD3D12Resource(ID3D12Device* d3d12Device, D3D12_RESOURCE_DESC* d3d12ResourceDesc, in GraphicsMemoryRegion memoryRegion, D3D12_RESOURCE_STATES d3d12ResourceState)
        {
            ID3D12Resource* d3d12Resource;

            ThrowExternalExceptionIfFailed(d3d12Device->CreatePlacedResource(
                memoryRegion.Allocator.DeviceObject.As<D3D12GraphicsMemoryHeap>().D3D12Heap,
                memoryRegion.Offset,
                d3d12ResourceDesc,
                d3d12ResourceState,
                pOptimizedClearValue: null,
                __uuidof<ID3D12Resource>(),
                (void**)&d3d12Resource
            ));

            return d3d12Resource;
        }

        static D3D12_RESOURCE_DESC GetD3D12ResourceDesc(GraphicsTextureKind kind, DXGI_FORMAT format, uint width, uint height, ushort depth, ulong alignment)
        {
            Unsafe.SkipInit(out D3D12_RESOURCE_DESC d3d12ResourceDesc);

            switch (kind)
            {
                case GraphicsTextureKind.OneDimensional:
                {
                    d3d12ResourceDesc = D3D12_RESOURCE_DESC.Tex1D(format, width, mipLevels: 1, alignment: alignment);
                    break;
                }

                case GraphicsTextureKind.TwoDimensional:
                {
                    d3d12ResourceDesc = D3D12_RESOURCE_DESC.Tex2D(format, width, height, mipLevels: 1, alignment: alignment);
                    break;
                }

                case GraphicsTextureKind.ThreeDimensional:
                {
                    d3d12ResourceDesc = D3D12_RESOURCE_DESC.Tex3D(format, width, height, depth, mipLevels: 1, alignment: alignment);
                    break;
                }

                default:
                {
                    ThrowForInvalidKind(kind);
                    break;
                }
            }

            return d3d12ResourceDesc;
        }

        static D3D12_RESOURCE_STATES GetD3D12ResourceState(GraphicsResourceCpuAccess cpuAccess, GraphicsTextureKind kind)
        {
            return cpuAccess switch {
                GraphicsResourceCpuAccess.Read => D3D12_RESOURCE_STATE_COPY_DEST,
                GraphicsResourceCpuAccess.Write => D3D12_RESOURCE_STATE_GENERIC_READ,
                _ => D3D12_RESOURCE_STATE_NON_PIXEL_SHADER_RESOURCE | D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE,
            };
        }
    }

    /// <inheritdoc />
    public override GraphicsMemoryBudget GetMemoryBudget(GraphicsMemoryManager memoryManager)
        => GetMemoryBudget((D3D12GraphicsMemoryManager)memoryManager);

    /// <inheritdoc cref="GetMemoryBudget(GraphicsMemoryManager)" />
    public GraphicsMemoryBudget GetMemoryBudget(D3D12GraphicsMemoryManager memoryManager) => new GraphicsMemoryBudget {
        EstimatedBudget = ulong.MaxValue,
        EstimatedUsage = 0,
        TotalAllocatedMemoryRegionSize = 0,
        TotalAllocatorSize = 0,
    };

    /// <inheritdoc />
    public override D3D12GraphicsRenderContext RentRenderContext()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return _renderContextPool.Rent(this, &CreateRenderContext);

        static D3D12GraphicsRenderContext CreateRenderContext(D3D12GraphicsDevice device)
        {
            AssertNotNull(device);
            return new D3D12GraphicsRenderContext(device);
        }
    }

    /// <inheritdoc />
    public override void ReturnRenderContext(GraphicsRenderContext renderContext)
        => ReturnRenderContext((D3D12GraphicsRenderContext)renderContext);

    /// <inheritdoc cref="ReturnRenderContext(GraphicsRenderContext)" />
    public void ReturnRenderContext(D3D12GraphicsRenderContext renderContext)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        ThrowIfNull(renderContext);

        if (renderContext.Device != this)
        {
            ThrowForInvalidParent(renderContext.Device);
        }
        _renderContextPool.Return(renderContext);
    }

    /// <inheritdoc />
    public override void Signal(GraphicsFence fence)
        => Signal((D3D12GraphicsFence)fence);

    /// <inheritdoc cref="Signal(GraphicsFence)" />
    public void Signal(D3D12GraphicsFence fence)
        => ThrowExternalExceptionIfFailed(D3D12CommandQueue->Signal(fence.D3D12Fence, fence.D3D12FenceSignalValue));

    /// <inheritdoc />
    public override void WaitForIdle()
    {
        var waitForIdleFence = WaitForIdleFence;

        ThrowExternalExceptionIfFailed(_d3d12CommandQueue->Signal(waitForIdleFence.D3D12Fence, waitForIdleFence.D3D12FenceSignalValue));

        waitForIdleFence.Wait();
        waitForIdleFence.Reset();
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            WaitForIdle();

            if (isDisposing)
            {
                _renderContextPool.Dispose();
                _waitForIdleFence?.Dispose();
            }

            foreach (var memoryManager in _memoryManagers)
            {
                memoryManager.Dispose();
            }

            ReleaseIfNotNull(_d3d12CommandQueue);
            ReleaseIfNotNull(_d3d12Device);
        }

        _state.EndDispose();
    }

    private int GetMemoryManagerIndex(GraphicsResourceCpuAccess cpuAccess, int kind)
    {
        var memoryManagerIndex = cpuAccess switch {
            GraphicsResourceCpuAccess.None => 0,    // DEFAULT
            GraphicsResourceCpuAccess.Read => 2,    // READBACK
            GraphicsResourceCpuAccess.Write => 1,   // UPLOAD
            _ => -1,
        };

        Assert(AssertionsEnabled && (memoryManagerIndex >= 0));

        if (!_d3d12SupportsResourceHeapTier2)
        {
            // Scale to account for resource kind
            memoryManagerIndex *= 3;
            memoryManagerIndex += kind;
        }

        return memoryManagerIndex;
    }
}
