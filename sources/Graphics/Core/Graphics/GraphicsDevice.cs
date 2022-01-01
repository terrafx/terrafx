// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics device which provides state management and isolation for a graphics adapter.</summary>
public abstract partial class GraphicsDevice : GraphicsAdapterObject, IDisposable
{
    /// <summary>The information for the graphics device.</summary>
    protected GraphicsDeviceInfo DeviceInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsDevice" /> class.</summary>
    /// <param name="adapter">The underlying adapter for the device.</param>
    /// <exception cref="ArgumentNullException"><paramref name="adapter" /> is <c>null</c>.</exception>
    protected GraphicsDevice(GraphicsAdapter adapter) : base(adapter)
    {
    }

    /// <summary>Gets the compute command queue for the device.</summary>
    public GraphicsComputeCommandQueue ComputeCommandQueue => DeviceInfo.ComputeQueue;

    /// <summary>Gets the copy command queue for the device.</summary>
    public GraphicsCopyCommandQueue CopyCommandQueue => DeviceInfo.CopyQueue;

    /// <summary>Gets the render command queue for the device.</summary>
    public GraphicsRenderCommandQueue RenderCommandQueue => DeviceInfo.RenderQueue;

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
    /// <param name="isSignalled"><c>true</c> if the fence is signalled by default; otherwise, <c>false</c>.</param>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public GraphicsFence CreateFence(bool isSignalled)
    {
        ThrowIfDisposed();

        var createOptions = new GraphicsFenceCreateOptions {
            IsSignalled = isSignalled,
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
    /// <param name="inputs">The pipeline inputs for the pipeline signature or <see cref="UnmanagedArray{T}.Empty" /> if none exist.</param>
    /// <returns>A new graphics pipeline signature.</returns>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <remarks>Ownership of <paramref name="inputs" /> is given to the created pipeline signature.</remarks>
    public GraphicsPipelineSignature CreatePipelineSignature(UnmanagedArray<GraphicsPipelineInput> inputs)
    {
        ThrowIfDisposed();

        var createOptions = new GraphicsPipelineSignatureCreateOptions {
            Inputs = inputs,
            Resources = UnmanagedArray<GraphicsPipelineResource>.Empty,
            TakeInputsOwnership = true,
            TakeResourcesOwnership = true,
        };
        return CreatePipelineSignatureUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics pipeline signature.</summary>
    /// <param name="inputs">The pipeline inputs for the pipeline signature or <see cref="UnmanagedArray{T}.Empty" /> if none exist.</param>
    /// <param name="resources">The pipeline resources for the pipeline signature or <see cref="UnmanagedArray{T}.Empty" /> if none exist.</param>
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

    /// <summary>Creates a new graphics buffer.</summary>
    /// <param name="createOptions">The options to use when creating the buffer.</param>
    /// <returns>The created graphics buffer.</returns>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract GraphicsBuffer CreateBufferUnsafe(in GraphicsBufferCreateOptions createOptions);

    /// <summary>Creates a new graphics fence.</summary>
    /// <param name="createOptions">The options to use when creating the fence.</param>
    /// <returns>The created graphics fence.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract GraphicsFence CreateFenceUnsafe(in GraphicsFenceCreateOptions createOptions);

    /// <summary>Creates a new graphics pipeline signature.</summary>
    /// <param name="createOptions">The options to use when creating the pipeline signature.</param>
    /// <returns>A new graphics pipeline signature.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract GraphicsPipelineSignature CreatePipelineSignatureUnsafe(in GraphicsPipelineSignatureCreateOptions createOptions);

    /// <summary>Creates a new graphics render pass.</summary>
    /// <param name="createOptions">The options to use when creating the render pass.</param>
    /// <returns>A new graphics render pass created for the device.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsRenderPassCreateOptions.MinimumRenderTargetCount" /> is greater than the maximum number of allowed render targets.</exception>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract GraphicsRenderPass CreateRenderPassUnsafe(in GraphicsRenderPassCreateOptions createOptions);

    /// <summary>Creates a new graphics shader.</summary>
    /// <param name="createOptions">The options to use when creating the shader.</param>
    /// <returns>A new graphics shader.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract GraphicsShader CreateShaderUnsafe(in GraphicsShaderCreateOptions createOptions);

    /// <summary>Creates a new graphics texture.</summary>
    /// <param name="createOptions">The options to use when creating the buffer.</param>
    /// <returns>The created graphics texture.</returns>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract GraphicsTexture CreateTextureUnsafe(in GraphicsTextureCreateOptions createOptions);
}
