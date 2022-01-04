// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D_FEATURE_LEVEL;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_FEATURE;
using static TerraFX.Interop.DirectX.D3D12_HEAP_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_HEAP_TIER;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsDevice : GraphicsDevice
{
    private ComPtr<ID3D12Device> _d3d12Device;
    private readonly uint _d3d12DeviceVersion;

    private readonly uint _cbvSrvUavDescriptorSize;
    private readonly uint _dsvDescriptorSize;
    private readonly uint _rtvDescriptorSize;
    private readonly uint _samplerDescriptorSize;

    private readonly D3D12GraphicsMemoryManager[] _memoryManagers;

    private ValueList<D3D12GraphicsBuffer> _buffers;
    private readonly ValueMutex _buffersMutex;

    private ValueList<D3D12GraphicsFence> _fences;
    private readonly ValueMutex _fencesMutex;

    private ValueList<D3D12GraphicsPipelineSignature> _pipelineSignatures;
    private readonly ValueMutex _pipelineSignaturesMutex;

    private ValueList<D3D12GraphicsRenderPass> _renderPasses;
    private readonly ValueMutex _renderPassesMutex;

    private ValueList<D3D12GraphicsShader> _shaders;
    private readonly ValueMutex _shadersMutex;

    private ValueList<D3D12GraphicsTexture> _textures;
    private readonly ValueMutex _texturesMutex;

    private MemoryBudgetInfo _memoryBudgetInfo;

    internal D3D12GraphicsDevice(D3D12GraphicsAdapter adapter, in GraphicsDeviceCreateOptions createOptions) : base(adapter)
    {
        adapter.AddDevice(this);

        var d3d12Device = CreateD3D12Device(out _d3d12DeviceVersion);
        _d3d12Device = d3d12Device;

        _buffers = new ValueList<D3D12GraphicsBuffer>();
        _buffersMutex = new ValueMutex();

        _fences = new ValueList<D3D12GraphicsFence>();
        _fencesMutex = new ValueMutex();

        _pipelineSignatures = new ValueList<D3D12GraphicsPipelineSignature>();
        _pipelineSignaturesMutex = new ValueMutex();

        _renderPasses = new ValueList<D3D12GraphicsRenderPass>();
        _renderPassesMutex = new ValueMutex();

        _shaders = new ValueList<D3D12GraphicsShader>();
        _shadersMutex = new ValueMutex();

        _textures = new ValueList<D3D12GraphicsTexture>();
        _texturesMutex = new ValueMutex();

        DeviceInfo.ComputeQueue = new D3D12GraphicsComputeCommandQueue(this);
        DeviceInfo.CopyQueue = new D3D12GraphicsCopyCommandQueue(this);
        DeviceInfo.RenderQueue = new D3D12GraphicsRenderCommandQueue(this);

        _cbvSrvUavDescriptorSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV);
        _dsvDescriptorSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_DSV);
        _rtvDescriptorSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);
        _samplerDescriptorSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_SAMPLER);

        _memoryManagers = CreateMemoryManagers(createOptions.CreateMemoryAllocator);

        _memoryBudgetInfo = new MemoryBudgetInfo();
        UpdateMemoryBudgetInfo(ref _memoryBudgetInfo, totalOperationCount: 0);

        SetNameUnsafe(Name);

        ID3D12Device* CreateD3D12Device(out uint d3d12DeviceVersion)
        {
            ID3D12Device* d3d12Device;
            ThrowExternalExceptionIfFailed(D3D12CreateDevice((IUnknown*)Adapter.DxgiAdapter, D3D_FEATURE_LEVEL_11_0, __uuidof<ID3D12Device>(), (void**)&d3d12Device));
            return GetLatestD3D12Device(d3d12Device, out d3d12DeviceVersion);
        }

        D3D12GraphicsMemoryManager[] CreateMemoryManagers(GraphicsMemoryAllocatorCreateFunc memoryAllocatorCreateFunc)
        {
            D3D12GraphicsMemoryManager[] memoryManagers;

            D3D12_FEATURE_DATA_D3D12_OPTIONS d3d12Options;

            if (_d3d12Device.Get()->CheckFeatureSupport(D3D12_FEATURE_D3D12_OPTIONS, &d3d12Options, SizeOf<D3D12_FEATURE_DATA_D3D12_OPTIONS>()).SUCCEEDED && (d3d12Options.ResourceHeapTier >= D3D12_RESOURCE_HEAP_TIER_2))
            {
                memoryManagers = new D3D12GraphicsMemoryManager[MaxMemoryManagerKinds];

                var createOptions = new D3D12GraphicsMemoryManagerCreateOptions {
                    CreateMemoryAllocator = memoryAllocatorCreateFunc,
                    D3D12HeapFlags = D3D12_HEAP_FLAG_NONE,
                };

                createOptions.D3D12HeapType = D3D12_HEAP_TYPE_DEFAULT;
                memoryManagers[0] = new D3D12GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapType = D3D12_HEAP_TYPE_UPLOAD;
                memoryManagers[1] = new D3D12GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapType = D3D12_HEAP_TYPE_READBACK;
                memoryManagers[2] = new D3D12GraphicsMemoryManager(this, in createOptions);
            }
            else
            {
                memoryManagers = new D3D12GraphicsMemoryManager[MaxMemoryManagerCount];

                var createOptions = new D3D12GraphicsMemoryManagerCreateOptions {
                    CreateMemoryAllocator = memoryAllocatorCreateFunc,
                };

                createOptions.D3D12HeapType = D3D12_HEAP_TYPE_DEFAULT;

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_BUFFERS;
                memoryManagers[0] = new D3D12GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_NON_RT_DS_TEXTURES;
                memoryManagers[1] = new D3D12GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_RT_DS_TEXTURES;
                memoryManagers[2] = new D3D12GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapType = D3D12_HEAP_TYPE_UPLOAD;

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_BUFFERS;
                memoryManagers[3] = new D3D12GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_NON_RT_DS_TEXTURES;
                memoryManagers[4] = new D3D12GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_RT_DS_TEXTURES;
                memoryManagers[5] = new D3D12GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapType = D3D12_HEAP_TYPE_READBACK;

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_BUFFERS;
                memoryManagers[6] = new D3D12GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_NON_RT_DS_TEXTURES;
                memoryManagers[7] = new D3D12GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_RT_DS_TEXTURES;
                memoryManagers[8] = new D3D12GraphicsMemoryManager(this, in createOptions);
            }

            return memoryManagers;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsDevice" /> class.</summary>
    ~D3D12GraphicsDevice() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the descriptor size for <see cref="D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV" />.</summary>
    public uint CbvSrvUavDescriptorSize => _cbvSrvUavDescriptorSize;

    /// <inheritdoc cref="GraphicsDevice.ComputeCommandQueue" />
    public new D3D12GraphicsComputeCommandQueue ComputeCommandQueue => base.ComputeCommandQueue.As<D3D12GraphicsComputeCommandQueue>();

    /// <inheritdoc cref="GraphicsDevice.CopyCommandQueue" />
    public new D3D12GraphicsCopyCommandQueue CopyCommandQueue => base.CopyCommandQueue.As<D3D12GraphicsCopyCommandQueue>();

    /// <summary>Gets the descriptor size for <see cref="D3D12_DESCRIPTOR_HEAP_TYPE_DSV" />.</summary>
    public uint DsvDescriptorSize => _dsvDescriptorSize;

    /// <summary>Gets the underlying <see cref="ID3D12Device" /> for the device.</summary>
    public ID3D12Device* D3D12Device => _d3d12Device;

    /// <summary>Gets the interface version of <see cref="D3D12Device" />.</summary>
    public uint D3D12DeviceVersion => _d3d12DeviceVersion;

    /// <inheritdoc cref="GraphicsDevice.RenderCommandQueue" />
    public new D3D12GraphicsRenderCommandQueue RenderCommandQueue => base.RenderCommandQueue.As<D3D12GraphicsRenderCommandQueue>();

    /// <summary>Gets the descriptor size for <see cref="D3D12_DESCRIPTOR_HEAP_TYPE_RTV" />.</summary>
    public uint RtvDescriptorSize => _rtvDescriptorSize;

    /// <summary>Gets the descriptor size for <see cref="D3D12_DESCRIPTOR_HEAP_TYPE_SAMPLER" />.</summary>
    public uint SamplerDescriptorSize => _samplerDescriptorSize;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override D3D12GraphicsBuffer CreateBufferUnsafe(in GraphicsBufferCreateOptions createOptions)
    {
        return new D3D12GraphicsBuffer(this, in createOptions);
    }

    /// <inheritdoc />
    protected override D3D12GraphicsFence CreateFenceUnsafe(in GraphicsFenceCreateOptions createOptions)
    {
        return new D3D12GraphicsFence(this, in createOptions);
    }

    /// <inheritdoc />
    protected override D3D12GraphicsPipelineSignature CreatePipelineSignatureUnsafe(in GraphicsPipelineSignatureCreateOptions createOptions)
    {
        return new D3D12GraphicsPipelineSignature(this, in createOptions);
    }

    /// <inheritdoc />
    protected override D3D12GraphicsRenderPass CreateRenderPassUnsafe(in GraphicsRenderPassCreateOptions createOptions)
    {
        return new D3D12GraphicsRenderPass(this, in createOptions);
    }

    /// <inheritdoc />
    protected override D3D12GraphicsShader CreateShaderUnsafe(in GraphicsShaderCreateOptions createOptions)
    {
        return new D3D12GraphicsShader(this, in createOptions);
    }

    /// <inheritdoc />
    protected override D3D12GraphicsTexture CreateTextureUnsafe(in GraphicsTextureCreateOptions createOptions)
    {
        return new D3D12GraphicsTexture(this, in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _buffers.Dispose();
            _pipelineSignatures.Dispose();
            _renderPasses.Dispose();
            _shaders.Dispose();
            _textures.Dispose();

            DeviceInfo.ComputeQueue.Dispose();
            DeviceInfo.CopyQueue.Dispose();
            DeviceInfo.RenderQueue.Dispose();

            foreach (var memoryManager in _memoryManagers)
            {
                memoryManager.Dispose();
            }

            _fences.Dispose();
        }

        _ = _d3d12Device.Reset();
        _ = Adapter.RemoveDevice(this);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12Device->SetD3D12Name(value);
    }

    internal void AddBuffer(D3D12GraphicsBuffer buffer)
    {
        _buffers.Add(buffer, _buffersMutex);
    }

    internal void AddFence(D3D12GraphicsFence fence)
    {
        _fences.Add(fence, _fencesMutex);
    }

    internal void AddPipelineSignature(D3D12GraphicsPipelineSignature pipelineSignature)
    {
        _pipelineSignatures.Add(pipelineSignature, _pipelineSignaturesMutex);
    }

    internal void AddRenderPass(D3D12GraphicsRenderPass renderPass)
    {
        _renderPasses.Add(renderPass, _renderPassesMutex);
    }

    internal void AddShader(D3D12GraphicsShader shader)
    {
        _shaders.Add(shader, _shadersMutex);
    }

    internal void AddTexture(D3D12GraphicsTexture texture)
    {
        _textures.Add(texture, _texturesMutex);
    }

    internal GraphicsMemoryBudget GetMemoryBudget(D3D12GraphicsMemoryManager memoryManager)
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
        return GetMemoryBudgetNoLock(memoryManager, in memoryBudgetInfo);
    }

    internal D3D12GraphicsMemoryManager GetMemoryManager(GraphicsCpuAccess cpuAccess, GraphicsResourceKind resourceKind)
    {
        var memoryManagerIndex = GetMemoryManagerIndex(cpuAccess, resourceKind);
        return _memoryManagers[memoryManagerIndex];
    }

    internal bool RemoveBuffer(D3D12GraphicsBuffer buffer)
    {
        return IsDisposed || _buffers.Remove(buffer, _buffersMutex);
    }

    internal bool RemoveFence(D3D12GraphicsFence fence)
    {
        return IsDisposed || _fences.Remove(fence, _fencesMutex);
    }

    internal bool RemovePipelineSignature(D3D12GraphicsPipelineSignature pipelineSignature)
    {
        return IsDisposed || _pipelineSignatures.Remove(pipelineSignature, _pipelineSignaturesMutex);
    }

    internal bool RemoveRenderPass(D3D12GraphicsRenderPass renderPass)
    {
        return IsDisposed || _renderPasses.Remove(renderPass, _renderPassesMutex);
    }

    internal bool RemoveShader(D3D12GraphicsShader shader)
    {
        return IsDisposed || _shaders.Remove(shader, _shadersMutex);
    }

    internal bool RemoveTexture(D3D12GraphicsTexture texture)
    {
        return IsDisposed || _textures.Remove(texture, _texturesMutex);
    }

    private GraphicsMemoryBudget GetMemoryBudgetNoLock(D3D12GraphicsMemoryManager memoryManager, in MemoryBudgetInfo memoryBudgetInfo)
    {
        var estimatedMemoryBudget = 0UL;
        var estimatedMemoryUsage = 0UL;
        var totalFreeMemoryRegionSize = 0UL;
        var totalSize = 0UL;
        var totalSizeAtLastUpdate = 0UL;

        switch (memoryManager.D3D12HeapType)
        {
            case D3D12_HEAP_TYPE_DEFAULT:
            {
                estimatedMemoryBudget = memoryBudgetInfo.DxgiQueryLocalVideoMemoryInfo.Budget;
                estimatedMemoryUsage = memoryBudgetInfo.DxgiQueryLocalVideoMemoryInfo.CurrentUsage;

                var memoryManagerKindIndex = (int)(D3D12_HEAP_TYPE_DEFAULT - 1);

                totalFreeMemoryRegionSize = GetTotalFreeMemoryRegionByteLength(memoryManagerKindIndex);
                totalSize = GetTotalByteLength(memoryManagerKindIndex);
                totalSizeAtLastUpdate = memoryBudgetInfo.GetTotalByteLengthAtLastUpdate(memoryManagerKindIndex);
                break;
            }

            case D3D12_HEAP_TYPE_UPLOAD:
            case D3D12_HEAP_TYPE_READBACK:
            {
                estimatedMemoryBudget = memoryBudgetInfo.DxgiQueryNonLocalVideoMemoryInfo.Budget;
                estimatedMemoryUsage = memoryBudgetInfo.DxgiQueryNonLocalVideoMemoryInfo.CurrentUsage;

                var memoryManagerKindIndex = (int)(D3D12_HEAP_TYPE_UPLOAD - 1);

                totalFreeMemoryRegionSize = GetTotalFreeMemoryRegionByteLength(memoryManagerKindIndex);
                totalSize = GetTotalByteLength(memoryManagerKindIndex);
                totalSizeAtLastUpdate = memoryBudgetInfo.GetTotalByteLengthAtLastUpdate(memoryManagerKindIndex);

                memoryManagerKindIndex = (int)(D3D12_HEAP_TYPE_READBACK - 1);

                totalFreeMemoryRegionSize += GetTotalFreeMemoryRegionByteLength(memoryManagerKindIndex);
                totalSize += GetTotalByteLength(memoryManagerKindIndex);
                totalSizeAtLastUpdate += memoryBudgetInfo.GetTotalByteLengthAtLastUpdate(memoryManagerKindIndex);
                break;
            }

            default:
            {
                ThrowForInvalidKind(memoryManager.D3D12HeapType);
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
            EstimatedMemoryByteBudget = estimatedMemoryBudget,
            EstimatedMemoryByteUsage = estimatedMemoryUsage,
            TotalFreeMemoryRegionByteLength = totalFreeMemoryRegionSize,
            TotalByteLength = totalSize,
        };
    }

    private int GetMemoryManagerIndex(GraphicsCpuAccess cpuAccess, GraphicsResourceKind resourceKind)
    {
        var memoryManagerIndex = cpuAccess switch {
            GraphicsCpuAccess.None => 0,    // DEFAULT
            GraphicsCpuAccess.Read => 2,    // READBACK
            GraphicsCpuAccess.Write => 1,   // UPLOAD
            _ => -1,
        };

        Assert(AssertionsEnabled && (memoryManagerIndex >= 0));
        Assert(AssertionsEnabled && (resourceKind is GraphicsResourceKind.Buffer or GraphicsResourceKind.Texture));

        if (_memoryManagers.Length != MaxMemoryManagerKinds)
        {
            // Scale to account for resource kind
            memoryManagerIndex *= 3;
            memoryManagerIndex += resourceKind switch {
                GraphicsResourceKind.Buffer => 0,
                GraphicsResourceKind.Texture => 1,
                _ => -1
            };
        }

        return memoryManagerIndex;
    }

    private ulong GetTotalFreeMemoryRegionByteLength(int memoryManagerKindIndex)
    {
        Assert(AssertionsEnabled && ((uint)memoryManagerKindIndex < MaxMemoryManagerKinds));

        if (_memoryManagers.Length == MaxMemoryManagerKinds)
        {
            return _memoryManagers[memoryManagerKindIndex].TotalFreeMemoryRegionByteLength;
        }
        else
        {
            memoryManagerKindIndex *= 3;

            return _memoryManagers[memoryManagerKindIndex + 0].TotalFreeMemoryRegionByteLength
                 + _memoryManagers[memoryManagerKindIndex + 1].TotalFreeMemoryRegionByteLength
                 + _memoryManagers[memoryManagerKindIndex + 2].TotalFreeMemoryRegionByteLength;
        }
    }

    private ulong GetTotalOperationCount(int memoryManagerKindIndex)
    {
        Assert(AssertionsEnabled && ((uint)memoryManagerKindIndex < MaxMemoryManagerKinds));

        if (_memoryManagers.Length == MaxMemoryManagerKinds)
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

    private ulong GetTotalByteLength(int memoryManagerKindIndex)
    {
        Assert(AssertionsEnabled && ((uint)memoryManagerKindIndex < MaxMemoryManagerKinds));

        if (_memoryManagers.Length == MaxMemoryManagerKinds)
        {
            return _memoryManagers[memoryManagerKindIndex].ByteLength;
        }
        else
        {
            memoryManagerKindIndex *= 3;

            return _memoryManagers[memoryManagerKindIndex + 0].ByteLength
                 + _memoryManagers[memoryManagerKindIndex + 1].ByteLength
                 + _memoryManagers[memoryManagerKindIndex + 2].ByteLength;
        }
    }

    private void UpdateMemoryBudgetInfo(ref MemoryBudgetInfo memoryBudgetInfo, ulong totalOperationCount)
    {
        using var writerLock = new DisposableWriterLock(memoryBudgetInfo.RWLock, isExternallySynchronized: false);
        UpdateMemoryBudgetInfoNoLock(ref memoryBudgetInfo, totalOperationCount);
    }

    private void UpdateMemoryBudgetInfoNoLock(ref MemoryBudgetInfo memoryBudgetInfo, ulong totalOperationCount)
    {
        DXGI_QUERY_VIDEO_MEMORY_INFO dxgiQueryLocalVideoMemoryInfo;

        if (Adapter.TryQueryLocalVideoMemoryInfo(&dxgiQueryLocalVideoMemoryInfo))
        {
            memoryBudgetInfo.DxgiQueryLocalVideoMemoryInfo = dxgiQueryLocalVideoMemoryInfo;
        }

        DXGI_QUERY_VIDEO_MEMORY_INFO dxgiQueryNonLocalVideoMemoryInfo;

        if (Adapter.TryQueryNonLocalVideoMemoryInfo(&dxgiQueryNonLocalVideoMemoryInfo))
        {
            memoryBudgetInfo.DxgiQueryNonLocalVideoMemoryInfo = dxgiQueryNonLocalVideoMemoryInfo;
        }

        memoryBudgetInfo.TotalOperationCountAtLastUpdate = totalOperationCount;

        for (var memoryManagerKindIndex = 0; memoryManagerKindIndex < MaxMemoryManagerKinds; memoryManagerKindIndex++)
        {
            memoryBudgetInfo.SetTotalFreeMemoryRegionByteLengthAtLastUpdate(memoryManagerKindIndex, GetTotalFreeMemoryRegionByteLength(memoryManagerKindIndex));
            memoryBudgetInfo.SetTotalSizeAtLastUpdate(memoryManagerKindIndex, GetTotalByteLength(memoryManagerKindIndex));
        }
    }
}
