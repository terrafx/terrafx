// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.CompilerServices;
using TerraFX.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D_FEATURE_LEVEL;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_LIST_TYPE;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_FEATURE;
using static TerraFX.Interop.DirectX.D3D12_HEAP_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_HEAP_TIER;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.DirectX.D3D12_TEXTURE_LAYOUT;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsDevice : GraphicsDevice
{
    private readonly uint _d3d12CbvSrvUavDescriptorHandleIncrementSize;
    private readonly ID3D12CommandQueue* _d3d12ComputeCommandQueue;
    private readonly ID3D12CommandQueue* _d3d12CopyCommandQueue;
    private readonly ID3D12CommandQueue* _d3d12DirectCommandQueue;
    private readonly ID3D12Device* _d3d12Device;
    private readonly D3D12_FEATURE_DATA_D3D12_OPTIONS _d3d12Options;
    private readonly uint _d3d12RtvDescriptorHandleIncrementSize;
    private readonly bool _d3d12SupportsResourceHeapTier2;
    private readonly D3D12GraphicsMemoryManager[] _memoryManagers;
    private readonly D3D12GraphicsFence _waitForIdleFence;

    private ContextPool<D3D12GraphicsDevice, D3D12GraphicsComputeContext> _computeContextPool;
    private ContextPool<D3D12GraphicsDevice, D3D12GraphicsCopyContext> _copyContextPool;
    private MemoryBudgetInfo _memoryBudgetInfo;
    private ContextPool<D3D12GraphicsDevice, D3D12GraphicsRenderContext> _renderContextPool;

    internal D3D12GraphicsDevice(D3D12GraphicsAdapter adapter, GraphicsMemoryAllocatorCreateFunc createMemoryAllocator)
        : base(adapter)
    {
        var d3d12Device = CreateD3D12Device(adapter);

        _d3d12ComputeCommandQueue = CreateD3D12CommandQueue(d3d12Device, D3D12_COMMAND_LIST_TYPE_COMPUTE);
        _d3d12CopyCommandQueue = CreateD3D12CommandQueue(d3d12Device, D3D12_COMMAND_LIST_TYPE_COPY);
        _d3d12DirectCommandQueue = CreateD3D12CommandQueue(d3d12Device, D3D12_COMMAND_LIST_TYPE_DIRECT);
        _d3d12Device = d3d12Device;
        _d3d12Options = GetD3D12Options(d3d12Device);

        _d3d12CbvSrvUavDescriptorHandleIncrementSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV);
        _d3d12RtvDescriptorHandleIncrementSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);

        var d3d12SupportsResourceHeapTier2 = D3D12Options.ResourceHeapTier >= D3D12_RESOURCE_HEAP_TIER_2;
        _d3d12SupportsResourceHeapTier2 = d3d12SupportsResourceHeapTier2;

        _memoryManagers = CreateMemoryManagers(this, createMemoryAllocator, d3d12SupportsResourceHeapTier2);
        _waitForIdleFence = CreateFence(isSignalled: false);

        _computeContextPool = new ContextPool<D3D12GraphicsDevice, D3D12GraphicsComputeContext>();
        _copyContextPool = new ContextPool<D3D12GraphicsDevice, D3D12GraphicsCopyContext>();
        _renderContextPool = new ContextPool<D3D12GraphicsDevice, D3D12GraphicsRenderContext>();

        _memoryBudgetInfo = new MemoryBudgetInfo();
        UpdateMemoryBudgetInfo(ref _memoryBudgetInfo, totalOperationCount: 0);

        static ID3D12CommandQueue* CreateD3D12CommandQueue(ID3D12Device* d3d12Device, D3D12_COMMAND_LIST_TYPE d3d12CommandListType)
        {
            ID3D12CommandQueue* d3d12CommandQueue;
            var commandQueueDesc = new D3D12_COMMAND_QUEUE_DESC {
                Type = d3d12CommandListType,
            };

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

        static D3D12GraphicsMemoryManager[] CreateMemoryManagers(D3D12GraphicsDevice device, GraphicsMemoryAllocatorCreateFunc createMemoryAllocator, bool d3d12SupportsResourceHeapTier2)
        {
            D3D12GraphicsMemoryManager[] memoryManagers;

            if (d3d12SupportsResourceHeapTier2)
            {
                memoryManagers = new D3D12GraphicsMemoryManager[MaxMemoryManagerKinds] {
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_DEFAULT),
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_UPLOAD),
                    new D3D12GraphicsMemoryManager(device, createMemoryAllocator, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_READBACK),
                };
            }
            else
            {
                memoryManagers = new D3D12GraphicsMemoryManager[MaxMemoryManagerCount] {
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

    /// <summary>Gets the <see cref="ID3D12CommandQueue" /> used by the device for compute commands.</summary>
    public ID3D12CommandQueue* D3D12ComputeCommandQueue
    {
        get
        {
            AssertNotDisposed();
            return _d3d12ComputeCommandQueue;
        }
    }

    /// <summary>Gets the <see cref="ID3D12CommandQueue" /> used by the device for copy commands.</summary>
    public ID3D12CommandQueue* D3D12CopyCommandQueue
    {
        get
        {
            AssertNotDisposed();
            return _d3d12CopyCommandQueue;
        }
    }

    /// <summary>Gets the <see cref="ID3D12CommandQueue" /> used by the device for rendering commands.</summary>
    public ID3D12CommandQueue* D3D12DirectCommandQueue
    {
        get
        {
            AssertNotDisposed();
            return _d3d12DirectCommandQueue;
        }
    }

    /// <summary>Gets the underlying <see cref="ID3D12Device" /> for the device.</summary>
    public ID3D12Device* D3D12Device
    {
        get
        {
            AssertNotDisposed();
            return _d3d12Device;
        }
    }

    /// <summary>Gets the <see cref="D3D12_FEATURE_DATA_D3D12_OPTIONS" /> for the device.</summary>
    public ref readonly D3D12_FEATURE_DATA_D3D12_OPTIONS D3D12Options => ref _d3d12Options;

    /// <summary>Gets the descriptor handle increment size for <see cref="D3D12_DESCRIPTOR_HEAP_TYPE_RTV" />.</summary>
    public uint D3D12RtvDescriptorHandleIncrementSize => _d3d12RtvDescriptorHandleIncrementSize;

    /// <summary>Gets <c>true</c> if the device supports <see cref="D3D12_RESOURCE_HEAP_TIER_2" />; otherwise, <c>false</c>.</summary>
    public bool D3D12SupportsResourceHeapTier2 => _d3d12SupportsResourceHeapTier2;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
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
        var memoryRegion = _memoryManagers[memoryManagerIndex].Allocate(checked((nuint)d3d12ResourceAllocationInfo.SizeInBytes), checked((nuint)d3d12ResourceAllocationInfo.Alignment), bufferCreateInfo.AllocationFlags);

        var d3d12ResourceState = GetD3D12ResourceState(bufferCreateInfo.CpuAccess, bufferCreateInfo.Kind);
        var d3d12Resource = CreateD3D12Resource(d3d12Device, &d3d12ResourceDesc, in memoryRegion, d3d12ResourceState);

        var createInfo = new D3D12GraphicsBuffer.CreateInfo {
            CpuAccess = bufferCreateInfo.CpuAccess,
            CreateMemoryAllocator = bufferCreateInfo.CreateMemoryAllocator.IsValid ? bufferCreateInfo.CreateMemoryAllocator : new GraphicsMemoryAllocatorCreateFunc(&GraphicsMemoryAllocator.CreateDefault),
            D3D12Resource = d3d12Resource,
            D3D12ResourceState = d3d12ResourceState,
            Kind = bufferCreateInfo.Kind,
            MemoryRegion = memoryRegion,
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

        static D3D12_RESOURCE_DESC GetD3D12ResourceDesc(nuint size, nuint alignment)
        {
            return D3D12_RESOURCE_DESC.Buffer(size, alignment: alignment);
        }

        static D3D12_RESOURCE_STATES GetD3D12ResourceState(GraphicsResourceCpuAccess cpuAccess, GraphicsBufferKind kind)
        {
            return cpuAccess switch {
                GraphicsResourceCpuAccess.Read => D3D12_RESOURCE_STATE_COPY_DEST,
                GraphicsResourceCpuAccess.Write => D3D12_RESOURCE_STATE_GENERIC_READ,
                _ => D3D12_RESOURCE_STATE_COMMON,
            };
        }
    }

    /// <inheritdoc />
    public override D3D12GraphicsFence CreateFence(bool isSignalled)
    {
        ThrowIfDisposed();
        return new D3D12GraphicsFence(this, isSignalled);
    }

    /// <inheritdoc />
    public override D3D12GraphicsPipelineSignature CreatePipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResourceInfo> resources = default)
    {
        ThrowIfDisposed();
        return new D3D12GraphicsPipelineSignature(this, inputs, resources);
    }

    /// <inheritdoc />
    public override D3D12GraphicsShader CreateShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
    {
        ThrowIfDisposed();
        return new D3D12GraphicsShader(this, kind, bytecode, entryPointName);
    }

    /// <inheritdoc />
    public override D3D12GraphicsRenderPass CreateRenderPass(IGraphicsSurface surface, GraphicsFormat renderTargetFormat, uint minimumRenderTargetCount = 0)
    {
        ThrowIfDisposed();
        return new D3D12GraphicsRenderPass(this, surface, renderTargetFormat, minimumRenderTargetCount);
    }

    /// <inheritdoc />
    public override D3D12GraphicsTexture CreateTexture(in GraphicsTextureCreateInfo textureCreateInfo)
    {
        var d3d12Device = D3D12Device;

        var d3d12ResourceDesc = GetD3D12ResourceDesc(in textureCreateInfo, D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT);
        var d3d12ResourceAllocationInfo = d3d12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &d3d12ResourceDesc);

        var memoryManagerIndex = GetMemoryManagerIndex(textureCreateInfo.CpuAccess, 1);
        var memoryRegion = _memoryManagers[memoryManagerIndex].Allocate(checked((nuint)d3d12ResourceAllocationInfo.SizeInBytes), checked((nuint)d3d12ResourceAllocationInfo.Alignment), textureCreateInfo.AllocationFlags);

        var d3d12ResourceState = GetD3D12ResourceState(textureCreateInfo.CpuAccess, textureCreateInfo.Kind);
        var d3d12Resource = CreateD3D12Resource(d3d12Device, &d3d12ResourceDesc, in memoryRegion, d3d12ResourceState);

        var d3d12PlacedSubresourceFootprints = GetD3D12PlacedSubresourceFootprints(d3d12Device, &d3d12ResourceDesc, textureCreateInfo.MipLevelCount);

        var format = textureCreateInfo.Format;

        var width = textureCreateInfo.Width;
        var height = textureCreateInfo.Height;
        var depth = textureCreateInfo.Depth;

        ref readonly var d3d12PlacedSubresourceFootprint = ref d3d12PlacedSubresourceFootprints.GetReferenceUnsafe(0);

        var rowPitch = d3d12PlacedSubresourceFootprint.Footprint.RowPitch;
        var slicePitch = rowPitch * height;

        var createInfo = new D3D12GraphicsTexture.CreateInfo {
            CpuAccess = textureCreateInfo.CpuAccess,
            D3D12PlacedSubresourceFootprints = d3d12PlacedSubresourceFootprints,
            D3D12Resource = d3d12Resource,
            D3D12ResourceState = d3d12ResourceState,
            MemoryRegion = memoryRegion,
            TextureInfo = new GraphicsTextureInfo {
                Depth = depth,
                Format = format,
                Height = height,
                Kind = textureCreateInfo.Kind,
                MipLevelCount = textureCreateInfo.MipLevelCount,
                RowPitch = rowPitch,
                SlicePitch = slicePitch,
                Width = width,
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

        static UnmanagedArray<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> GetD3D12PlacedSubresourceFootprints(ID3D12Device* d3d12Device, D3D12_RESOURCE_DESC* d3d12ResourceDesc, ushort mipLevelCount)
        {
            var d3d12PlacedSubresourceFootprints = new UnmanagedArray<D3D12_PLACED_SUBRESOURCE_FOOTPRINT>(mipLevelCount);

            d3d12Device->GetCopyableFootprints(
                d3d12ResourceDesc,
                FirstSubresource: 0,
                NumSubresources: mipLevelCount,
                BaseOffset: 0,
                d3d12PlacedSubresourceFootprints.GetPointerUnsafe(0),
                pNumRows: null,
                pRowSizeInBytes: null,
                pTotalBytes: null
            );

            return d3d12PlacedSubresourceFootprints;
        }

        static D3D12_RESOURCE_DESC GetD3D12ResourceDesc(in GraphicsTextureCreateInfo textureCreateInfo, nuint alignment)
        {
            Unsafe.SkipInit(out D3D12_RESOURCE_DESC d3d12ResourceDesc);

            switch (textureCreateInfo.Kind)
            {
                case GraphicsTextureKind.OneDimensional:
                {
                    d3d12ResourceDesc = D3D12_RESOURCE_DESC.Tex1D(
                        textureCreateInfo.Format.AsDxgiFormat(),
                        textureCreateInfo.Width,
                        arraySize: 1,
                        textureCreateInfo.MipLevelCount,
                        D3D12_RESOURCE_FLAG_NONE,
                        D3D12_TEXTURE_LAYOUT_UNKNOWN,
                        alignment
                    );
                    break;
                }

                case GraphicsTextureKind.TwoDimensional:
                {
                    d3d12ResourceDesc = D3D12_RESOURCE_DESC.Tex2D(
                        textureCreateInfo.Format.AsDxgiFormat(),
                        textureCreateInfo.Width,
                        textureCreateInfo.Height,
                        arraySize: 1,
                        textureCreateInfo.MipLevelCount,
                        sampleCount: 1,
                        sampleQuality: 0,
                        D3D12_RESOURCE_FLAG_NONE,
                        D3D12_TEXTURE_LAYOUT_UNKNOWN,
                        alignment
                    );
                    break;
                }

                case GraphicsTextureKind.ThreeDimensional:
                {
                    d3d12ResourceDesc = D3D12_RESOURCE_DESC.Tex3D(
                        textureCreateInfo.Format.AsDxgiFormat(),
                        textureCreateInfo.Width,
                        textureCreateInfo.Height,
                        textureCreateInfo.Depth,
                        textureCreateInfo.MipLevelCount,
                        D3D12_RESOURCE_FLAG_NONE,
                        D3D12_TEXTURE_LAYOUT_UNKNOWN,
                        alignment
                    );
                    break;
                }

                default:
                {
                    ThrowForInvalidKind(textureCreateInfo.Kind);
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
                _ => D3D12_RESOURCE_STATE_COMMON,
            };
        }
    }

    /// <inheritdoc />
    public override GraphicsMemoryBudget GetMemoryBudget(GraphicsMemoryManager memoryManager)
        => GetMemoryBudget((D3D12GraphicsMemoryManager)memoryManager);

    /// <inheritdoc cref="GetMemoryBudget(GraphicsMemoryManager)" />
    public GraphicsMemoryBudget GetMemoryBudget(D3D12GraphicsMemoryManager memoryManager)
    {
        ulong totalOperationCount;

        if (memoryManager.D3D12HeapType == D3D12_HEAP_TYPE_DEFAULT)
        {
            var memoryManagerKindIndex = (int)(D3D12_HEAP_TYPE_DEFAULT - 1);
            totalOperationCount = GetTotalOperationCount(memoryManagerKindIndex);
        }
        else
        {
            var memoryManagerKindIndex = (int)(D3D12_HEAP_TYPE_UPLOAD - 1);
            totalOperationCount = GetTotalOperationCount(memoryManagerKindIndex);

            memoryManagerKindIndex = (int)(D3D12_HEAP_TYPE_READBACK - 1);
            totalOperationCount += GetTotalOperationCount(memoryManagerKindIndex);
        }

        ref var memoryBudgetInfo = ref _memoryBudgetInfo;

        if ((totalOperationCount - memoryBudgetInfo.TotalOperationCountAtLastUpdate) >= 30)
        {
            UpdateMemoryBudgetInfo(ref memoryBudgetInfo, totalOperationCount);
        }

        using var readerLock = new DisposableReaderLock(memoryBudgetInfo.RWLock, isExternallySynchronized: false);
        return GetMemoryBudgetInternal(memoryManager.D3D12HeapType, ref memoryBudgetInfo);
    }

    /// <inheritdoc />
    public override D3D12GraphicsComputeContext RentComputeContext()
    {
        ThrowIfDisposed();
        return _computeContextPool.Rent(this, &CreateComputeContext);

        static D3D12GraphicsComputeContext CreateComputeContext(D3D12GraphicsDevice device)
        {
            AssertNotNull(device);
            return new D3D12GraphicsComputeContext(device);
        }
    }

    /// <inheritdoc />
    public override D3D12GraphicsCopyContext RentCopyContext()
    {
        ThrowIfDisposed();
        return _copyContextPool.Rent(this, &CreateCopyContext);

        static D3D12GraphicsCopyContext CreateCopyContext(D3D12GraphicsDevice device)
        {
            AssertNotNull(device);
            return new D3D12GraphicsCopyContext(device);
        }
    }

    /// <inheritdoc />
    public override D3D12GraphicsRenderContext RentRenderContext()
    {
        ThrowIfDisposed();
        return _renderContextPool.Rent(this, &CreateRenderContext);

        static D3D12GraphicsRenderContext CreateRenderContext(D3D12GraphicsDevice device)
        {
            AssertNotNull(device);
            return new D3D12GraphicsRenderContext(device);
        }
    }

    /// <inheritdoc />
    public override void ReturnContext(GraphicsComputeContext computeContext)
        => ReturnContext((D3D12GraphicsComputeContext)computeContext);

    /// <inheritdoc />
    public override void ReturnContext(GraphicsCopyContext copyContext)
        => ReturnContext((D3D12GraphicsCopyContext)copyContext);

    /// <inheritdoc />
    public override void ReturnContext(GraphicsRenderContext renderContext)
        => ReturnContext((D3D12GraphicsRenderContext)renderContext);

    /// <inheritdoc cref="ReturnContext(GraphicsComputeContext)" />
    public void ReturnContext(D3D12GraphicsComputeContext computeContext)
    {
        ThrowIfDisposed();
        ThrowIfNull(computeContext);

        if (computeContext.Device != this)
        {
            ThrowForInvalidParent(computeContext.Device);
        }
        _computeContextPool.Return(computeContext);
    }

    /// <inheritdoc cref="ReturnContext(GraphicsCopyContext)" />
    public void ReturnContext(D3D12GraphicsCopyContext copyContext)
    {
        ThrowIfDisposed();
        ThrowIfNull(copyContext);

        if (copyContext.Device != this)
        {
            ThrowForInvalidParent(copyContext.Device);
        }
        _copyContextPool.Return(copyContext);
    }

    /// <inheritdoc cref="ReturnContext(GraphicsRenderContext)" />
    public void ReturnContext(D3D12GraphicsRenderContext renderContext)
    {
        ThrowIfDisposed();
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
        => ThrowExternalExceptionIfFailed(D3D12DirectCommandQueue->Signal(fence.D3D12Fence, fence.D3D12FenceSignalValue));

    /// <inheritdoc />
    public override void WaitForIdle()
    {
        var waitForIdleFence = WaitForIdleFence;

        ThrowExternalExceptionIfFailed(_d3d12DirectCommandQueue->Signal(waitForIdleFence.D3D12Fence, waitForIdleFence.D3D12FenceSignalValue));

        waitForIdleFence.Wait();
        waitForIdleFence.Reset();
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        WaitForIdle();

        if (isDisposing)
        {
            _computeContextPool.Dispose();
            _copyContextPool.Dispose();
            _renderContextPool.Dispose();
            _waitForIdleFence?.Dispose();
        }

        foreach (var memoryManager in _memoryManagers)
        {
            memoryManager.Dispose();
        }

        ReleaseIfNotNull(_d3d12ComputeCommandQueue);
        ReleaseIfNotNull(_d3d12CopyCommandQueue);
        ReleaseIfNotNull(_d3d12DirectCommandQueue);
        ReleaseIfNotNull(_d3d12Device);
    }

    /// <inheritdoc />
    protected override void SetNameInternal(string value)
    {
        D3D12Device->SetD3D12Name(value);
        D3D12ComputeCommandQueue->SetD3D12Name(value);
        D3D12CopyCommandQueue->SetD3D12Name(value);
        D3D12DirectCommandQueue->SetD3D12Name(value);
    }

    private GraphicsMemoryBudget GetMemoryBudgetInternal(D3D12_HEAP_TYPE d3d12HeapType, ref MemoryBudgetInfo memoryBudgetInfo)
    {
        // This method should only be called under the reader lock

        var estimatedMemoryBudget = 0UL;
        var estimatedMemoryUsage = 0UL;
        var totalFreeMemoryRegionSize = 0UL;
        var totalSize = 0UL;
        var totalSizeAtLastUpdate = 0UL;

        switch (d3d12HeapType)
        {
            case D3D12_HEAP_TYPE_DEFAULT:
            {
                estimatedMemoryBudget = memoryBudgetInfo.DxgiQueryLocalVideoMemoryInfo.Budget;
                estimatedMemoryUsage = memoryBudgetInfo.DxgiQueryLocalVideoMemoryInfo.CurrentUsage;

                var memoryManagerKindIndex = (int)(D3D12_HEAP_TYPE_DEFAULT - 1);

                totalFreeMemoryRegionSize = GetTotalFreeMemoryRegionSize(memoryManagerKindIndex);
                totalSize = GetTotalSize(memoryManagerKindIndex);
                totalSizeAtLastUpdate = memoryBudgetInfo.GetTotalSizeAtLastUpdate(memoryManagerKindIndex);
                break;
            }

            case D3D12_HEAP_TYPE_UPLOAD:
            case D3D12_HEAP_TYPE_READBACK:
            {
                estimatedMemoryBudget = memoryBudgetInfo.DxgiQueryNonLocalVideoMemoryInfo.Budget;
                estimatedMemoryUsage = memoryBudgetInfo.DxgiQueryNonLocalVideoMemoryInfo.CurrentUsage;

                var memoryManagerKindIndex = (int)(D3D12_HEAP_TYPE_UPLOAD - 1);

                totalFreeMemoryRegionSize = GetTotalFreeMemoryRegionSize(memoryManagerKindIndex);
                totalSize = GetTotalSize(memoryManagerKindIndex);
                totalSizeAtLastUpdate = memoryBudgetInfo.GetTotalSizeAtLastUpdate(memoryManagerKindIndex);

                memoryManagerKindIndex = (int)(D3D12_HEAP_TYPE_READBACK - 1);

                totalFreeMemoryRegionSize += GetTotalFreeMemoryRegionSize(memoryManagerKindIndex);
                totalSize += GetTotalSize(memoryManagerKindIndex);
                totalSizeAtLastUpdate += memoryBudgetInfo.GetTotalSizeAtLastUpdate(memoryManagerKindIndex);
                break;
            }

            default:
            {
                ThrowForInvalidKind(d3d12HeapType);
                break;
            }
        }

        if ((estimatedMemoryUsage + totalSize) > totalSizeAtLastUpdate)
        {
            estimatedMemoryUsage = estimatedMemoryUsage + totalSize - totalSizeAtLastUpdate;
        }
        else
        {
            estimatedMemoryUsage = 0;
        }

        return new GraphicsMemoryBudget {
            EstimatedMemoryBudget = estimatedMemoryBudget,
            EstimatedMemoryUsage = estimatedMemoryUsage,
            TotalFreeMemoryRegionSize = totalFreeMemoryRegionSize,
            TotalSize = totalSize,
        };
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

    private void UpdateMemoryBudgetInfo(ref MemoryBudgetInfo memoryBudgetInfo, ulong totalOperationCount)
    {
        using var writerLock = new DisposableWriterLock(memoryBudgetInfo.RWLock, isExternallySynchronized: false);
        UpdateMemoryBudgetInfoInternal(ref memoryBudgetInfo, totalOperationCount);
    }

    private void UpdateMemoryBudgetInfoInternal(ref MemoryBudgetInfo memoryBudgetInfo, ulong totalOperationCount)
    {
        // This method should only be called under the writer lock

        DXGI_QUERY_VIDEO_MEMORY_INFO dxgiQueryLocalVideoMemoryInfo;

        if (Adapter.TryGetDxgiQueryLocalVideoMemoryInfo(&dxgiQueryLocalVideoMemoryInfo))
        {
            memoryBudgetInfo.DxgiQueryLocalVideoMemoryInfo = dxgiQueryLocalVideoMemoryInfo;
        }

        DXGI_QUERY_VIDEO_MEMORY_INFO dxgiQueryNonLocalVideoMemoryInfo;

        if (Adapter.TryGetDxgiQueryNonLocalVideoMemoryInfo(&dxgiQueryNonLocalVideoMemoryInfo))
        {
            memoryBudgetInfo.DxgiQueryNonLocalVideoMemoryInfo = dxgiQueryNonLocalVideoMemoryInfo;
        }

        memoryBudgetInfo.TotalOperationCountAtLastUpdate = totalOperationCount;

        for (var memoryManagerKindIndex = 0; memoryManagerKindIndex < MaxMemoryManagerKinds; memoryManagerKindIndex++)
        {
            memoryBudgetInfo.SetTotalFreeMemoryRegionSizeAtLastUpdate(memoryManagerKindIndex, GetTotalFreeMemoryRegionSize(memoryManagerKindIndex));
            memoryBudgetInfo.SetTotalSizeAtLastUpdate(memoryManagerKindIndex, GetTotalSize(memoryManagerKindIndex));
        }
    }

    private ulong GetTotalOperationCount(int memoryManagerKindIndex)
    {
        Assert(AssertionsEnabled && ((uint)memoryManagerKindIndex < MaxMemoryManagerKinds));

        if (D3D12SupportsResourceHeapTier2)
        {
            return _memoryManagers[memoryManagerKindIndex].OperationCount;
        }
        else
        {
            memoryManagerKindIndex *= 3;

            return _memoryManagers[memoryManagerKindIndex + 0].OperationCount
                 + _memoryManagers[memoryManagerKindIndex + 1].OperationCount
                 + _memoryManagers[memoryManagerKindIndex + 2].OperationCount;
        }
    }

    private ulong GetTotalFreeMemoryRegionSize(int memoryManagerKindIndex)
    {
        Assert(AssertionsEnabled && ((uint)memoryManagerKindIndex < MaxMemoryManagerKinds));


        if (D3D12SupportsResourceHeapTier2)
        {
            return _memoryManagers[memoryManagerKindIndex].TotalFreeMemoryRegionSize;
        }
        else
        {
            memoryManagerKindIndex *= 3;

            return _memoryManagers[memoryManagerKindIndex + 0].TotalFreeMemoryRegionSize
                 + _memoryManagers[memoryManagerKindIndex + 1].TotalFreeMemoryRegionSize
                 + _memoryManagers[memoryManagerKindIndex + 2].TotalFreeMemoryRegionSize;
        }
    }

    private ulong GetTotalSize(int memoryManagerKindIndex)
    {
        Assert(AssertionsEnabled && ((uint)memoryManagerKindIndex < MaxMemoryManagerKinds));

        if (D3D12SupportsResourceHeapTier2)
        {
            return _memoryManagers[memoryManagerKindIndex].Size;
        }
        else
        {
            memoryManagerKindIndex *= 3;

            return _memoryManagers[memoryManagerKindIndex + 0].Size
                 + _memoryManagers[memoryManagerKindIndex + 1].Size
                 + _memoryManagers[memoryManagerKindIndex + 2].Size;
        }
    }
}
