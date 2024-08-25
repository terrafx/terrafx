// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
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
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics device which provides state management and isolation for a graphics adapter.</summary>
public sealed unsafe partial class GraphicsDevice : GraphicsAdapterObject
{
    private ValueList<GraphicsBuffer> _buffers;
    private readonly ValueMutex _buffersMutex;

    private readonly GraphicsComputeCommandQueue _computeQueue;
    private readonly GraphicsCopyCommandQueue _copyQueue;
    private readonly GraphicsRenderCommandQueue _renderQueue;

    private ComPtr<ID3D12Device> _d3d12Device;
    private readonly uint _d3d12DeviceVersion;

    private readonly uint _d3d12CbvSrvUavDescriptorSize;
    private readonly uint _d3d12DsvDescriptorSize;
    private readonly uint _d3d12RtvDescriptorSize;
    private readonly uint _d3d12SamplerDescriptorSize;

    private readonly D3D12_RESOURCE_HEAP_TIER _d3d12ResourceHeapTier;
    private readonly FeatureFlags _featureFlags;

    private ValueList<GraphicsFence> _fences;
    private readonly ValueMutex _fencesMutex;

    private MemoryBudgetInfo _memoryBudgetInfo;
    private readonly GraphicsMemoryManager[] _memoryManagers;

    private ValueList<GraphicsPipelineSignature> _pipelineSignatures;
    private readonly ValueMutex _pipelineSignaturesMutex;

    private ValueList<GraphicsRenderPass> _renderPasses;
    private readonly ValueMutex _renderPassesMutex;

    private ValueList<GraphicsShader> _shaders;
    private readonly ValueMutex _shadersMutex;

    private ValueList<GraphicsTexture> _textures;
    private readonly ValueMutex _texturesMutex;

    internal GraphicsDevice(GraphicsAdapter adapter, in GraphicsDeviceCreateOptions createOptions) : base(adapter)
    {
        adapter.AddDevice(this);

        var d3d12Device = CreateD3D12Device(out _d3d12DeviceVersion);
        _d3d12Device.Attach(d3d12Device);

        _buffers = [];
        _buffersMutex = new ValueMutex();

        _fences = [];
        _fencesMutex = new ValueMutex();

        _pipelineSignatures = [];
        _pipelineSignaturesMutex = new ValueMutex();

        _renderPasses = [];
        _renderPassesMutex = new ValueMutex();

        _shaders = [];
        _shadersMutex = new ValueMutex();

        _textures = [];
        _texturesMutex = new ValueMutex();

        _computeQueue = new GraphicsComputeCommandQueue(this);
        _copyQueue = new GraphicsCopyCommandQueue(this);
        _renderQueue = new GraphicsRenderCommandQueue(this);

        _d3d12CbvSrvUavDescriptorSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV);
        _d3d12DsvDescriptorSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_DSV);
        _d3d12RtvDescriptorSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);
        _d3d12SamplerDescriptorSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_SAMPLER);

        var featureFlags = FeatureFlags.None;
        var d3d12Options = new D3D12_FEATURE_DATA_D3D12_OPTIONS();

        if (d3d12Device->CheckFeatureSupport(D3D12_FEATURE_D3D12_OPTIONS, &d3d12Options, SizeOf<D3D12_FEATURE_DATA_D3D12_OPTIONS>()).SUCCEEDED)
        {
            _d3d12ResourceHeapTier = d3d12Options.ResourceHeapTier;
        }

        var d3d12Architecture = new D3D12_FEATURE_DATA_ARCHITECTURE {
            NodeIndex = 0,
        };

        if (d3d12Device->CheckFeatureSupport(D3D12_FEATURE_ARCHITECTURE, &d3d12Architecture, SizeOf<D3D12_FEATURE_DATA_ARCHITECTURE>()).SUCCEEDED)
        {
            if (d3d12Architecture.UMA != 0)
            {
                featureFlags |= FeatureFlags.Uma;
            }

            if (d3d12Architecture.CacheCoherentUMA != 0)
            {
                featureFlags |= FeatureFlags.CacheCoherentUma;
            }
        }

        _featureFlags = featureFlags;
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

        GraphicsMemoryManager[] CreateMemoryManagers(GraphicsMemoryAllocatorCreateFunc memoryAllocatorCreateFunc)
        {
            GraphicsMemoryManager[] memoryManagers;

            if (_d3d12ResourceHeapTier >= D3D12_RESOURCE_HEAP_TIER_2)
            {
                memoryManagers = new GraphicsMemoryManager[MaxMemoryManagerKinds];

                var createOptions = new GraphicsMemoryManagerCreateOptions {
                    CreateMemoryAllocator = memoryAllocatorCreateFunc,
                    D3D12HeapFlags = D3D12_HEAP_FLAG_NONE,
                    D3D12HeapType = D3D12_HEAP_TYPE_DEFAULT
                };
                memoryManagers[0] = new GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapType = D3D12_HEAP_TYPE_UPLOAD;
                memoryManagers[1] = new GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapType = D3D12_HEAP_TYPE_READBACK;
                memoryManagers[2] = new GraphicsMemoryManager(this, in createOptions);
            }
            else
            {
                memoryManagers = new GraphicsMemoryManager[MaxMemoryManagerCount];

                var createOptions = new GraphicsMemoryManagerCreateOptions {
                    CreateMemoryAllocator = memoryAllocatorCreateFunc,
                    D3D12HeapType = D3D12_HEAP_TYPE_DEFAULT,
                    D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_BUFFERS,
                };
                memoryManagers[0] = new GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_NON_RT_DS_TEXTURES;
                memoryManagers[1] = new GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_RT_DS_TEXTURES;
                memoryManagers[2] = new GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapType = D3D12_HEAP_TYPE_UPLOAD;

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_BUFFERS;
                memoryManagers[3] = new GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_NON_RT_DS_TEXTURES;
                memoryManagers[4] = new GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_RT_DS_TEXTURES;
                memoryManagers[5] = new GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapType = D3D12_HEAP_TYPE_READBACK;

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_BUFFERS;
                memoryManagers[6] = new GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_NON_RT_DS_TEXTURES;
                memoryManagers[7] = new GraphicsMemoryManager(this, in createOptions);

                createOptions.D3D12HeapFlags = D3D12_HEAP_FLAG_ALLOW_ONLY_RT_DS_TEXTURES;
                memoryManagers[8] = new GraphicsMemoryManager(this, in createOptions);
            }

            return memoryManagers;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsDevice" /> class.</summary>
    ~GraphicsDevice() => Dispose(isDisposing: false);

    /// <summary>Gets the compute command queue for the device.</summary>
    public GraphicsComputeCommandQueue ComputeCommandQueue => _computeQueue;

    /// <summary>Gets the copy command queue for the device.</summary>
    public GraphicsCopyCommandQueue CopyCommandQueue => _copyQueue;

    /// <summary>Gets <c>true</c> if the hardware is using a cache-coherent Unified Memory Architecture; otherwise, <c>false</c>.</summary>
    public bool IsCacheCoherentUma => _featureFlags.HasFlag(FeatureFlags.CacheCoherentUma);

    /// <summary>Gets <c>true</c> if the hardware is using a Unified Memory Architecture; otherwise, <c>false</c>.</summary>
    public bool IsUma => _featureFlags.HasFlag(FeatureFlags.Uma);

    /// <summary>Gets the render command queue for the device.</summary>
    public GraphicsRenderCommandQueue RenderCommandQueue => _renderQueue;

    internal uint D3D12CbvSrvUavDescriptorSize => _d3d12CbvSrvUavDescriptorSize;

    internal ID3D12Device* D3D12Device => _d3d12Device;

    internal uint D3D12DeviceVersion => _d3d12DeviceVersion;

    internal uint D3D12DsvDescriptorSize => _d3d12DsvDescriptorSize;

    internal uint D3D12RtvDescriptorSize => _d3d12RtvDescriptorSize;

    internal uint D3D12SamplerDescriptorSize => _d3d12SamplerDescriptorSize;

    /// <summary>Creates a new graphics buffer.</summary>
    /// <param name="createOptions">The options to use when creating the buffer.</param>
    /// <returns>The created graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferCreateOptions.AllocationFlags"/> is unsupported.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferCreateOptions.Kind"/> is unsupported.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferCreateOptions.CpuAccess"/> is unsupported.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferCreateOptions.ByteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBuffer CreateBuffer(in GraphicsBufferCreateOptions createOptions)
    {
        ThrowIfDisposed();

        ThrowIfNotDefined(createOptions.AllocationFlags);
        ThrowIfNotDefined(createOptions.Kind);
        ThrowIfNotDefined(createOptions.CpuAccess);
        ThrowIfZero(createOptions.ByteLength);

        return CreateBufferUnsafe(in createOptions);
    }

    /// <summary>Creates a new constant graphics buffer.</summary>
    /// <param name="byteLength">The length, in bytes, of the graphics buffer.</param>
    /// <returns>A created constant graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="byteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBuffer CreateConstantBuffer(nuint byteLength)
    {
        ThrowIfDisposed();
        ThrowIfZero(byteLength);

        var createOptions = new GraphicsBufferCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            Kind = GraphicsBufferKind.Constant,
            CreateMemorySuballocator = default,
            CpuAccess = GraphicsCpuAccess.None,
            ByteLength = byteLength,
        };
        return CreateBufferUnsafe(in createOptions);
    }

    /// <summary>Creates a new constant graphics buffer.</summary>
    /// <param name="byteLength">The length, in bytes, of the graphics buffer.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the buffer.</param>
    /// <returns>A created constant graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="byteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="cpuAccess" /> is not defined.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBuffer CreateConstantBuffer(nuint byteLength, GraphicsCpuAccess cpuAccess)
    {
        ThrowIfDisposed();

        ThrowIfNotDefined(cpuAccess);
        ThrowIfZero(byteLength);

        var createOptions = new GraphicsBufferCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            Kind = GraphicsBufferKind.Constant,
            CreateMemorySuballocator = default,
            CpuAccess = cpuAccess,
            ByteLength = byteLength,
        };
        return CreateBufferUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics fence.</summary>
    /// <param name="isSignaled"><c>true</c> if the fence is signaled by default; otherwise, <c>false</c>.</param>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public GraphicsFence CreateFence(bool isSignaled)
    {
        ThrowIfDisposed();

        var createOptions = new GraphicsFenceCreateOptions {
            IsSignaled = isSignaled,
        };
        return CreateFenceUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics fence.</summary>
    /// <param name="createOptions">The options to use when creating the fence.</param>
    /// <returns>The created graphics fence.</returns>
    public GraphicsFence CreateFence(in GraphicsFenceCreateOptions createOptions)
    {
        ThrowIfDisposed();
        return CreateFenceUnsafe(in createOptions);
    }

    /// <summary>Creates a new index graphics buffer.</summary>
    /// <param name="byteLength">The length, in bytes, of the graphics buffer.</param>
    /// <returns>A created index graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="byteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBuffer CreateIndexBuffer(nuint byteLength)
    {
        ThrowIfDisposed();
        ThrowIfZero(byteLength);

        var createOptions = new GraphicsBufferCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            Kind = GraphicsBufferKind.Index,
            CreateMemorySuballocator = default,
            CpuAccess = GraphicsCpuAccess.None,
            ByteLength = byteLength,
        };
        return CreateBufferUnsafe(in createOptions);
    }

    /// <summary>Creates a new index graphics buffer.</summary>
    /// <param name="byteLength">The length, in bytes, of the graphics buffer.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the buffer.</param>
    /// <returns>A created index graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="byteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBuffer CreateIndexBuffer(nuint byteLength, GraphicsCpuAccess cpuAccess)
    {
        ThrowIfDisposed();

        ThrowIfNotDefined(cpuAccess);
        ThrowIfZero(byteLength);

        var createOptions = new GraphicsBufferCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            Kind = GraphicsBufferKind.Index,
            CreateMemorySuballocator = default,
            CpuAccess = cpuAccess,
            ByteLength = byteLength,
        };
        return CreateBufferUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics pipeline signature.</summary>
    /// <param name="inputs">The pipeline inputs for the pipeline signature or <see cref="UnmanagedArray.Empty{T}()" /> if none exist.</param>
    /// <returns>A new graphics pipeline signature.</returns>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <remarks>Ownership of <paramref name="inputs" /> is given to the created pipeline signature.</remarks>
    public GraphicsPipelineSignature CreatePipelineSignature(UnmanagedArray<GraphicsPipelineInput> inputs)
    {
        ThrowIfDisposed();

        var createOptions = new GraphicsPipelineSignatureCreateOptions {
            Inputs = inputs,
            Resources = [],
            TakeInputsOwnership = true,
            TakeResourcesOwnership = true,
        };
        return CreatePipelineSignatureUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics pipeline signature.</summary>
    /// <param name="inputs">The pipeline inputs for the pipeline signature or <see cref="UnmanagedArray.Empty{T}()" /> if none exist.</param>
    /// <param name="resources">The pipeline resources for the pipeline signature or <see cref="UnmanagedArray.Empty{T}()" /> if none exist.</param>
    /// <returns>A new graphics pipeline signature.</returns>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <remarks>Ownership of <paramref name="inputs" /> is given to the created pipeline signature.</remarks>
    public GraphicsPipelineSignature CreatePipelineSignature(UnmanagedArray<GraphicsPipelineInput> inputs, UnmanagedArray<GraphicsPipelineResource> resources)
    {
        ThrowIfDisposed();

        var createOptions = new GraphicsPipelineSignatureCreateOptions {
            Inputs = inputs,
            Resources = resources,
            TakeInputsOwnership = true,
            TakeResourcesOwnership = true,
        };
        return CreatePipelineSignatureUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics pipeline signature.</summary>
    /// <param name="createOptions">The options to use when creating the pipeline signature.</param>
    /// <returns>A new graphics pipeline signature.</returns>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public GraphicsPipelineSignature CreatePipelineSignature(in GraphicsPipelineSignatureCreateOptions createOptions)
    {
        ThrowIfDisposed();
        return CreatePipelineSignatureUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics pixel shader.</summary>
    /// <param name="bytecode">The options to use when creating the shader.</param>
    /// <param name="entryPointName">The entry point name for the shader.</param>
    /// <returns>A new graphics pixel shader.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="bytecode" /> is <c>empty</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="entryPointName" /> is <c>null</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <remarks>Ownership of <paramref name="bytecode" /> is given to the created pixel shader.</remarks>
    public GraphicsShader CreatePixelShader(UnmanagedArray<byte> bytecode, string entryPointName)
    {
        ThrowIfDisposed();

        ThrowIfZero(bytecode.Length);
        ThrowIfNull(entryPointName);

        var createOptions = new GraphicsShaderCreateOptions {
            Bytecode = bytecode,
            EntryPointName = entryPointName,
            ShaderKind = GraphicsShaderKind.Pixel,
            TakeBytecodeOwnership = true,
        };
        return CreateShaderUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics render pass.</summary>
    /// <param name="renderTargetFormat">The format of the render targets created for the render pass.</param>
    /// <param name="surface">The surface for the render pass.</param>
    /// <returns>A new graphics render pass created for the device.</returns>
    /// <exception cref="ArgumentNullException"><see cref="GraphicsRenderPassCreateOptions.Surface" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsRenderPassCreateOptions.MinimumRenderTargetCount" /> is greater than the maximum number of allowed render targets.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsRenderPassCreateOptions.RenderTargetFormat" /> is not defined.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public GraphicsRenderPass CreateRenderPass(IGraphicsSurface surface, GraphicsFormat renderTargetFormat)
    {
        ThrowIfDisposed();

        ThrowIfNotDefined(renderTargetFormat);
        ThrowIfNull(surface);

        var createOptions = new GraphicsRenderPassCreateOptions {
            MinimumRenderTargetCount = 0,
            RenderTargetFormat = renderTargetFormat,
            Surface = surface,
        };
        return CreateRenderPassUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics render pass.</summary>
    /// <param name="createOptions">The options to use when creating the render pass.</param>
    /// <returns>A new graphics render pass created for the device.</returns>
    /// <exception cref="ArgumentNullException"><see cref="GraphicsRenderPassCreateOptions.Surface" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsRenderPassCreateOptions.MinimumRenderTargetCount" /> is greater than the maximum number of allowed render targets.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsRenderPassCreateOptions.RenderTargetFormat" /> is not defined.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public GraphicsRenderPass CreateRenderPass(in GraphicsRenderPassCreateOptions createOptions)
    {
        ThrowIfDisposed();

        ThrowIfNotDefined(createOptions.RenderTargetFormat);
        ThrowIfNull(createOptions.Surface);

        return CreateRenderPassUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics shader.</summary>
    /// <param name="createOptions">The options to use when creating the shader.</param>
    /// <returns>A new graphics shader.</returns>
    /// <exception cref="ArgumentNullException"><see cref="GraphicsShaderCreateOptions.Bytecode" /> is <c>empty</c>.</exception>
    /// <exception cref="ArgumentNullException"><see cref="GraphicsShaderCreateOptions.EntryPointName" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><see cref="GraphicsShaderCreateOptions.ShaderKind" /> is not defined.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public GraphicsShader CreateShader(in GraphicsShaderCreateOptions createOptions)
    {
        ThrowIfDisposed();

        ThrowIfZero(createOptions.Bytecode.Length);
        ThrowIfNull(createOptions.EntryPointName);
        ThrowIfNotDefined(createOptions.ShaderKind);

        return CreateShaderUnsafe(in createOptions);
    }

    /// <summary>Creates a new one-dimensional graphics texture.</summary>
    /// <param name="format">The format of the texture.</param>
    /// <param name="pixelWidth">The width, in pixels, of the texture.</param>
    /// <returns>A created one-dimensional graphics texture.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="format"/> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelWidth" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsTexture CreateTexture1D(GraphicsFormat format, uint pixelWidth)
    {
        ThrowIfDisposed();

        ThrowIfNotDefined(format);
        ThrowIfZero(pixelWidth);

        var createOptions = new GraphicsTextureCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            CpuAccess = GraphicsCpuAccess.None,
            PixelDepth = 1,
            PixelFormat = format,
            PixelHeight = 1,
            MipLevelCount = 0,
            Kind = GraphicsTextureKind.OneDimensional,
            PixelWidth = pixelWidth,
        };
        return CreateTexture(in createOptions);
    }

    /// <summary>Creates a new one-dimensional graphics texture.</summary>
    /// <param name="format">The format of the texture.</param>
    /// <param name="pixelWidth">The width, in pixels, of the texture.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the texture.</param>
    /// <returns>A created one-dimensional graphics texture.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="cpuAccess" /> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="format"/> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelWidth" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsTexture CreateTexture1D(GraphicsFormat format, uint pixelWidth, GraphicsCpuAccess cpuAccess)
    {
        ThrowIfDisposed();

        ThrowIfNotDefined(cpuAccess);
        ThrowIfNotDefined(format);
        ThrowIfZero(pixelWidth);

        var createOptions = new GraphicsTextureCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            CpuAccess = cpuAccess,
            PixelDepth = 1,
            PixelFormat = format,
            PixelHeight = 1,
            MipLevelCount = 0,
            Kind = GraphicsTextureKind.OneDimensional,
            PixelWidth = pixelWidth,
        };
        return CreateTexture(in createOptions);
    }

    /// <summary>Creates a new two-dimensional graphics texture.</summary>
    /// <param name="format">The format of the texture.</param>
    /// <param name="pixelWidth">The width, in pixels, of the texture.</param>
    /// <param name="pixelHeight">The height, in pixels, of the texture.</param>
    /// <returns>A created graphics texture.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="format"/> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelHeight" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelWidth" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsTexture CreateTexture2D(GraphicsFormat format, uint pixelWidth, uint pixelHeight)
    {
        ThrowIfDisposed();

        ThrowIfNotDefined(format);
        ThrowIfZero(pixelHeight);
        ThrowIfZero(pixelWidth);

        var createOptions = new GraphicsTextureCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            CpuAccess = GraphicsCpuAccess.None,
            PixelDepth = 1,
            PixelFormat = format,
            PixelHeight = pixelHeight,
            MipLevelCount = 0,
            Kind = GraphicsTextureKind.TwoDimensional,
            PixelWidth = pixelWidth,
        };
        return CreateTexture(in createOptions);
    }

    /// <summary>Creates a new two-dimensional graphics texture.</summary>
    /// <param name="format">The format of the texture.</param>
    /// <param name="pixelWidth">The width, in pixels, of the texture.</param>
    /// <param name="pixelHeight">The height, in pixels, of the texture.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the texture.</param>
    /// <returns>A created graphics texture.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="cpuAccess" /> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="format"/> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelHeight" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelWidth" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsTexture CreateTexture2D(GraphicsFormat format, uint pixelWidth, uint pixelHeight, GraphicsCpuAccess cpuAccess)
    {
        ThrowIfDisposed();

        ThrowIfNotDefined(cpuAccess);
        ThrowIfNotDefined(format);
        ThrowIfZero(pixelHeight);
        ThrowIfZero(pixelWidth);

        var createOptions = new GraphicsTextureCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            CpuAccess = cpuAccess,
            PixelDepth = 1,
            PixelFormat = format,
            PixelHeight = pixelHeight,
            MipLevelCount = 0,
            Kind = GraphicsTextureKind.TwoDimensional,
            PixelWidth = pixelWidth,
        };
        return CreateTexture(in createOptions);
    }

    /// <summary>Creates a new three-dimensional graphics texture.</summary>
    /// <param name="format">The format of the texture.</param>
    /// <param name="pixelWidth">The width, in pixels, of the texture.</param>
    /// <param name="pixelHeight">The height, in pixels, of the texture.</param>
    /// <param name="pixelDepth">The depth, in pixels, of the texture.</param>
    /// <returns>A created graphics texture.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelDepth" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="format"/> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelHeight" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelWidth" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsTexture CreateTexture3D(GraphicsFormat format, uint pixelWidth, uint pixelHeight, ushort pixelDepth)
    {
        ThrowIfDisposed();

        ThrowIfZero(pixelDepth);
        ThrowIfNotDefined(format);
        ThrowIfZero(pixelHeight);
        ThrowIfZero(pixelWidth);

        var createOptions = new GraphicsTextureCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            CpuAccess = GraphicsCpuAccess.None,
            PixelDepth = pixelDepth,
            PixelFormat = format,
            PixelHeight = pixelHeight,
            MipLevelCount = 0,
            Kind = GraphicsTextureKind.ThreeDimensional,
            PixelWidth = pixelWidth,
        };
        return CreateTexture(in createOptions);
    }

    /// <summary>Creates a new three-dimensional graphics texture.</summary>
    /// <param name="format">The format of the texture.</param>
    /// <param name="pixelWidth">The width, in pixels, of the texture.</param>
    /// <param name="pixelHeight">The height, in pixels, of the texture.</param>
    /// <param name="pixelDepth">The depth, in pixels, of the texture.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the texture.</param>
    /// <returns>A created graphics texture.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="cpuAccess" /> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelDepth" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="format"/> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelHeight" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelWidth" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsTexture CreateTexture3D(GraphicsFormat format, uint pixelWidth, uint pixelHeight, ushort pixelDepth, GraphicsCpuAccess cpuAccess)
    {
        ThrowIfDisposed();

        ThrowIfNotDefined(cpuAccess);
        ThrowIfZero(pixelDepth);
        ThrowIfNotDefined(format);
        ThrowIfZero(pixelHeight);
        ThrowIfZero(pixelWidth);

        var createOptions = new GraphicsTextureCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            CpuAccess = cpuAccess,
            PixelDepth = pixelDepth,
            PixelFormat = format,
            PixelHeight = pixelHeight,
            MipLevelCount = 0,
            Kind = GraphicsTextureKind.ThreeDimensional,
            PixelWidth = pixelWidth,
        };
        return CreateTexture(in createOptions);
    }

    /// <summary>Creates a new graphics texture.</summary>
    /// <param name="createOptions">The options to use when creating the buffer.</param>
    /// <returns>The created graphics texture.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureCreateOptions.AllocationFlags" /> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureCreateOptions.CpuAccess" /> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureCreateOptions.PixelDepth" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureCreateOptions.PixelFormat" /> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureCreateOptions.PixelHeight" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureCreateOptions.Kind" /> is not defined.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureCreateOptions.PixelWidth" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsTexture CreateTexture(in GraphicsTextureCreateOptions createOptions)
    {
        ThrowIfDisposed();

        ThrowIfNotDefined(createOptions.AllocationFlags);
        ThrowIfNotDefined(createOptions.CpuAccess);
        ThrowIfZero(createOptions.PixelDepth);
        ThrowIfNotDefined(createOptions.PixelFormat);
        ThrowIfZero(createOptions.PixelHeight);
        ThrowIfNotDefined(createOptions.Kind);
        ThrowIfZero(createOptions.PixelWidth);

        return CreateTextureUnsafe(in createOptions);
    }

    /// <summary>Creates a new upload graphics buffer.</summary>
    /// <param name="byteLength">The length, in bytes, of the graphics buffer.</param>
    /// <returns>A created upload graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="byteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBuffer CreateUploadBuffer(nuint byteLength)
    {
        ThrowIfDisposed();
        ThrowIfZero(byteLength);

        var createOptions = new GraphicsBufferCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            Kind = GraphicsBufferKind.Default,
            CreateMemorySuballocator = default,
            CpuAccess = GraphicsCpuAccess.Write,
            ByteLength = byteLength,
        };
        return CreateBufferUnsafe(in createOptions);
    }

    /// <summary>Creates a new vertex graphics buffer.</summary>
    /// <param name="byteLength">The length, in bytes, of the graphics buffer.</param>
    /// <returns>A created index graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="byteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBuffer CreateVertexBuffer(nuint byteLength)
    {
        ThrowIfDisposed();
        ThrowIfZero(byteLength);

        var createOptions = new GraphicsBufferCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            Kind = GraphicsBufferKind.Vertex,
            CreateMemorySuballocator = default,
            CpuAccess = GraphicsCpuAccess.None,
            ByteLength = byteLength,
        };
        return CreateBufferUnsafe(in createOptions);
    }

    /// <summary>Creates a new vertex graphics buffer.</summary>
    /// <param name="byteLength">The length, in bytes, of the graphics buffer.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the buffer.</param>
    /// <returns>A created vertex graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="byteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBuffer CreateVertexBuffer(nuint byteLength, GraphicsCpuAccess cpuAccess)
    {
        ThrowIfDisposed();

        ThrowIfNotDefined(cpuAccess);
        ThrowIfZero(byteLength);

        var createOptions = new GraphicsBufferCreateOptions {
            AllocationFlags = GraphicsMemoryAllocationFlags.None,
            Kind = GraphicsBufferKind.Vertex,
            CreateMemorySuballocator = default,
            CpuAccess = cpuAccess,
            ByteLength = byteLength,
        };
        return CreateBufferUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics vertex shader.</summary>
    /// <param name="bytecode">The options to use when creating the shader.</param>
    /// <param name="entryPointName">The entry point name for the shader.</param>
    /// <returns>A new graphics vertex shader.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="bytecode" /> is <c>empty</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="entryPointName" /> is <c>null</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <remarks>Ownership of <paramref name="bytecode" /> is given to the created vertex shader.</remarks>
    public GraphicsShader CreateVertexShader(UnmanagedArray<byte> bytecode, string entryPointName)
    {
        ThrowIfDisposed();

        ThrowIfZero(bytecode.Length);
        ThrowIfNull(entryPointName);

        var createOptions = new GraphicsShaderCreateOptions {
            Bytecode = bytecode,
            EntryPointName = entryPointName,
            ShaderKind = GraphicsShaderKind.Vertex,
            TakeBytecodeOwnership = true,
        };
        return CreateShaderUnsafe(in createOptions);
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

            _computeQueue.Dispose();
            _copyQueue.Dispose();
            _renderQueue.Dispose();

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
    protected override void SetNameUnsafe(string value) => D3D12Device->SetD3D12Name(value);

    internal void AddBuffer(GraphicsBuffer buffer) => _buffers.Add(buffer, _buffersMutex);

    internal void AddFence(GraphicsFence fence) => _fences.Add(fence, _fencesMutex);

    internal void AddPipelineSignature(GraphicsPipelineSignature pipelineSignature) => _pipelineSignatures.Add(pipelineSignature, _pipelineSignaturesMutex);

    internal void AddRenderPass(GraphicsRenderPass renderPass) => _renderPasses.Add(renderPass, _renderPassesMutex);

    internal void AddShader(GraphicsShader shader) => _shaders.Add(shader, _shadersMutex);

    internal void AddTexture(GraphicsTexture texture) => _textures.Add(texture, _texturesMutex);

    internal GraphicsMemoryBudget GetMemoryBudget(GraphicsMemoryManager memoryManager)
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

    internal GraphicsMemoryManager GetMemoryManager(GraphicsCpuAccess cpuAccess, GraphicsResourceKind resourceKind)
    {
        var memoryManagerIndex = GetMemoryManagerIndex(cpuAccess, resourceKind);
        return _memoryManagers[memoryManagerIndex];
    }

    internal bool RemoveBuffer(GraphicsBuffer buffer) => IsDisposed || _buffers.Remove(buffer, _buffersMutex);

    internal bool RemoveFence(GraphicsFence fence) => IsDisposed || _fences.Remove(fence, _fencesMutex);

    internal bool RemovePipelineSignature(GraphicsPipelineSignature pipelineSignature) => IsDisposed || _pipelineSignatures.Remove(pipelineSignature, _pipelineSignaturesMutex);

    internal bool RemoveRenderPass(GraphicsRenderPass renderPass) => IsDisposed || _renderPasses.Remove(renderPass, _renderPassesMutex);

    internal bool RemoveShader(GraphicsShader shader) => IsDisposed || _shaders.Remove(shader, _shadersMutex);

    internal bool RemoveTexture(GraphicsTexture texture) => IsDisposed || _textures.Remove(texture, _texturesMutex);

    private GraphicsBuffer CreateBufferUnsafe(in GraphicsBufferCreateOptions createOptions) => new GraphicsBuffer(this, in createOptions);

    private GraphicsFence CreateFenceUnsafe(in GraphicsFenceCreateOptions createOptions) => new GraphicsFence(this, in createOptions);

    private GraphicsPipelineSignature CreatePipelineSignatureUnsafe(in GraphicsPipelineSignatureCreateOptions createOptions) => new GraphicsPipelineSignature(this, in createOptions);

    private GraphicsRenderPass CreateRenderPassUnsafe(in GraphicsRenderPassCreateOptions createOptions) => new GraphicsRenderPass(this, in createOptions);

    private GraphicsShader CreateShaderUnsafe(in GraphicsShaderCreateOptions createOptions) => new GraphicsShader(this, in createOptions);

    private GraphicsTexture CreateTextureUnsafe(in GraphicsTextureCreateOptions createOptions) => new GraphicsTexture(this, in createOptions);

    private GraphicsMemoryBudget GetMemoryBudgetNoLock(GraphicsMemoryManager memoryManager, in MemoryBudgetInfo memoryBudgetInfo)
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
                if (IsUma)
                {
                    estimatedMemoryBudget = memoryBudgetInfo.DxgiQueryLocalVideoMemoryInfo.Budget;
                    estimatedMemoryUsage = memoryBudgetInfo.DxgiQueryLocalVideoMemoryInfo.CurrentUsage;
                }
                else
                {
                    estimatedMemoryBudget = memoryBudgetInfo.DxgiQueryNonLocalVideoMemoryInfo.Budget;
                    estimatedMemoryUsage = memoryBudgetInfo.DxgiQueryNonLocalVideoMemoryInfo.CurrentUsage;
                }

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
            case D3D12_HEAP_TYPE_CUSTOM:
            case D3D12_HEAP_TYPE_GPU_UPLOAD:
            {
                ThrowForInvalidKind(memoryManager.D3D12HeapType);
                break;
            }
        }

        estimatedMemoryUsage = ((estimatedMemoryUsage + totalSize) > totalSizeAtLastUpdate) ? (estimatedMemoryUsage + totalSize - totalSizeAtLastUpdate) : 0;

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

        Assert(memoryManagerIndex >= 0);
        Assert(resourceKind is GraphicsResourceKind.Buffer or GraphicsResourceKind.Texture);

        if (_memoryManagers.Length != MaxMemoryManagerKinds)
        {
            // Scale to account for resource kind
            memoryManagerIndex *= 3;
            memoryManagerIndex += resourceKind switch {
                GraphicsResourceKind.Buffer => 0,
                GraphicsResourceKind.Texture => 1,
                GraphicsResourceKind.Unknown => -1,
                _ => -1
            };
        }

        return memoryManagerIndex;
    }

    private ulong GetTotalAllocatedMemoryRegionByteLength(int memoryManagerKindIndex)
    {
        Assert((uint)memoryManagerKindIndex < MaxMemoryManagerKinds);

        if (_memoryManagers.Length == MaxMemoryManagerKinds)
        {
            return _memoryManagers[memoryManagerKindIndex].TotalAllocatedMemoryRegionByteLength;
        }
        else
        {
            memoryManagerKindIndex *= 3;

            return _memoryManagers[memoryManagerKindIndex + 2].TotalAllocatedMemoryRegionByteLength
                 + _memoryManagers[memoryManagerKindIndex + 1].TotalAllocatedMemoryRegionByteLength
                 + _memoryManagers[memoryManagerKindIndex + 0].TotalAllocatedMemoryRegionByteLength;
        }
    }

    private ulong GetTotalFreeMemoryRegionByteLength(int memoryManagerKindIndex)
    {
        Assert((uint)memoryManagerKindIndex < MaxMemoryManagerKinds);

        if (_memoryManagers.Length == MaxMemoryManagerKinds)
        {
            return _memoryManagers[memoryManagerKindIndex].TotalFreeMemoryRegionByteLength;
        }
        else
        {
            memoryManagerKindIndex *= 3;

            return _memoryManagers[memoryManagerKindIndex + 2].TotalFreeMemoryRegionByteLength
                 + _memoryManagers[memoryManagerKindIndex + 1].TotalFreeMemoryRegionByteLength
                 + _memoryManagers[memoryManagerKindIndex + 0].TotalFreeMemoryRegionByteLength;
        }
    }

    private ulong GetTotalOperationCount(int memoryManagerKindIndex)
    {
        Assert((uint)memoryManagerKindIndex < MaxMemoryManagerKinds);

        if (_memoryManagers.Length == MaxMemoryManagerKinds)
        {
            return _memoryManagers[memoryManagerKindIndex].OperationCount;
        }
        else
        {
            memoryManagerKindIndex *= 3;

            return _memoryManagers[memoryManagerKindIndex + 2].OperationCount
                 + _memoryManagers[memoryManagerKindIndex + 1].OperationCount
                 + _memoryManagers[memoryManagerKindIndex + 0].OperationCount;
        }
    }

    private ulong GetTotalByteLength(int memoryManagerKindIndex)
    {
        Assert((uint)memoryManagerKindIndex < MaxMemoryManagerKinds);

        if (_memoryManagers.Length == MaxMemoryManagerKinds)
        {
            return _memoryManagers[memoryManagerKindIndex].ByteLength;
        }
        else
        {
            memoryManagerKindIndex *= 3;

            return _memoryManagers[memoryManagerKindIndex + 2].ByteLength
                 + _memoryManagers[memoryManagerKindIndex + 1].ByteLength
                 + _memoryManagers[memoryManagerKindIndex + 0].ByteLength;
        }
    }

    private void UpdateMemoryBudgetInfo(ref MemoryBudgetInfo memoryBudgetInfo, ulong totalOperationCount)
    {
        using var writerLock = new DisposableWriterLock(memoryBudgetInfo.RWLock, isExternallySynchronized: false);
        UpdateMemoryBudgetInfoNoLock(ref memoryBudgetInfo, totalOperationCount);
    }

    private void UpdateMemoryBudgetInfoNoLock(ref MemoryBudgetInfo memoryBudgetInfo, ulong totalOperationCount)
    {
        var adapter = Adapter;

        DXGI_QUERY_VIDEO_MEMORY_INFO dxgiQueryLocalVideoMemoryInfo;

        if (!adapter.TryQueryLocalVideoMemoryInfo(&dxgiQueryLocalVideoMemoryInfo))
        {
            dxgiQueryLocalVideoMemoryInfo = new DXGI_QUERY_VIDEO_MEMORY_INFO {
                Budget = adapter.DxgiDedicatedVideoMemory,
                CurrentUsage = GetTotalFreeMemoryRegionByteLength((int)(D3D12_HEAP_TYPE_DEFAULT - 1)),
            };

            if (IsUma)
            {
                dxgiQueryLocalVideoMemoryInfo.Budget += adapter.DxgiSharedSystemMemory;
            }

            // Fallback to an 80% heuristic for available budget

            dxgiQueryLocalVideoMemoryInfo.Budget *= 8;
            dxgiQueryLocalVideoMemoryInfo.Budget /= 10;
        }

        DXGI_QUERY_VIDEO_MEMORY_INFO dxgiQueryNonLocalVideoMemoryInfo;

        if (!adapter.TryQueryNonLocalVideoMemoryInfo(&dxgiQueryNonLocalVideoMemoryInfo))
        {
            memoryBudgetInfo.DxgiQueryNonLocalVideoMemoryInfo = new DXGI_QUERY_VIDEO_MEMORY_INFO {
                Budget = adapter.DxgiSharedSystemMemory,
                CurrentUsage = GetTotalFreeMemoryRegionByteLength((int)(D3D12_HEAP_TYPE_UPLOAD - 1))
                             + GetTotalFreeMemoryRegionByteLength((int)(D3D12_HEAP_TYPE_READBACK - 1)),
            };

            if (IsUma)
            {
                dxgiQueryNonLocalVideoMemoryInfo.Budget = 0;
            }

            // Fallback to an 80% heuristic for available budget

            dxgiQueryNonLocalVideoMemoryInfo.Budget *= 8;
            dxgiQueryNonLocalVideoMemoryInfo.Budget /= 10;
        }

        memoryBudgetInfo.DxgiQueryLocalVideoMemoryInfo = dxgiQueryLocalVideoMemoryInfo;
        memoryBudgetInfo.DxgiQueryNonLocalVideoMemoryInfo = dxgiQueryNonLocalVideoMemoryInfo;
        memoryBudgetInfo.TotalOperationCountAtLastUpdate = totalOperationCount;

        for (var memoryManagerKindIndex = 0; memoryManagerKindIndex < MaxMemoryManagerKinds; memoryManagerKindIndex++)
        {
            memoryBudgetInfo.SetTotalFreeMemoryRegionByteLengthAtLastUpdate(memoryManagerKindIndex, GetTotalFreeMemoryRegionByteLength(memoryManagerKindIndex));
            memoryBudgetInfo.SetTotalSizeAtLastUpdate(memoryManagerKindIndex, GetTotalByteLength(memoryManagerKindIndex));
        }
    }
}
