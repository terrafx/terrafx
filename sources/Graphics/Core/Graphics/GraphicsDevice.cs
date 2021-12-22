// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics device which provides state management and isolation for a graphics adapter.</summary>
public abstract partial class GraphicsDevice : GraphicsAdapterObject, IDisposable
{
    /// <summary>Initializes a new instance of the <see cref="GraphicsDevice" /> class.</summary>
    /// <param name="adapter">The underlying adapter for the device.</param>
    /// <exception cref="ArgumentNullException"><paramref name="adapter" /> is <c>null</c>.</exception>
    protected GraphicsDevice(GraphicsAdapter adapter)
        : base(adapter)
    {
        ThrowIfNull(adapter);
    }

    /// <summary>Creates a new graphics buffer.</summary>
    /// <param name="bufferCreateInfo">The creation info describing the buffer.</param>
    /// <returns>The created graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferCreateInfo.Kind" /> of <paramref name="bufferCreateInfo" /> is unsupported.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferCreateInfo.Size" /> of <paramref name="bufferCreateInfo" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public abstract GraphicsBuffer CreateBuffer(in GraphicsBufferCreateInfo bufferCreateInfo);

    /// <summary>Creates a new constant graphics buffer.</summary>
    /// <param name="size">The size, in bytes, of the graphics buffer.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the buffer.</param>
    /// <returns>A created constant graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    /// <remarks>This is an alternative name for <see cref="CreateUniformBuffer(nuint, GraphicsResourceCpuAccess)" />.</remarks>
    public GraphicsBuffer CreateConstantBuffer(nuint size, GraphicsResourceCpuAccess cpuAccess = GraphicsResourceCpuAccess.None)
    {
        var bufferCreateInfo = new GraphicsBufferCreateInfo {
            CpuAccess = cpuAccess,
            Kind = GraphicsBufferKind.Constant,
            Size = size,
        };
        return CreateBuffer(in bufferCreateInfo);
    }

    /// <summary>Creates a new graphics fence for the device.</summary>
    /// <param name="isSignalled">The default state of <see cref="GraphicsFence.IsSignalled" /> for the created fence.</param>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsFence CreateFence(bool isSignalled);

    /// <summary>Creates a new index graphics buffer.</summary>
    /// <param name="size">The size, in bytes, of the graphics buffer.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the buffer.</param>
    /// <returns>A created index graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBuffer CreateIndexBuffer(nuint size, GraphicsResourceCpuAccess cpuAccess = GraphicsResourceCpuAccess.None)
    {
        var bufferCreateInfo = new GraphicsBufferCreateInfo {
            CpuAccess = cpuAccess,
            Kind = GraphicsBufferKind.Index,
            Size = size,
        };
        return CreateBuffer(in bufferCreateInfo);
    }

    /// <summary>Creates a new graphics pipeline signature for the device.</summary>
    /// <param name="inputs">The inputs given to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</param>
    /// <param name="resources">The info about resources available to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</param>
    /// <returns>A new graphics pipeline signature created for the device.</returns>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsPipelineSignature CreatePipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResourceInfo> resources = default);

    /// <summary>Creates a new graphics shader for the device.</summary>
    /// <param name="kind">The kind of graphics shader to create.</param>
    /// <param name="bytecode">The underlying bytecode for the graphics shader.</param>
    /// <param name="entryPointName">The name of the entry point for the graphics shader.</param>
    /// <returns>A new graphics shader created for the device.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entryPointName" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind" /> is unsupported.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsShader CreateShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName);

    /// <summary>Creates a new graphics render pass for the device.</summary>
    /// <param name="surface">The surface used by the render pass.</param>
    /// <param name="renderTargetFormat">The format of render targets used by the render pass.</param>
    /// <param name="minimumRenderTargetCount">The minimum number of render targets to create or <c>zero</c> to use the system default.</param>
    /// <returns>A new graphics render pass created for the device.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="surface" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minimumRenderTargetCount" /> is greater than the maximum number of allowed render targets.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsRenderPass CreateRenderPass(IGraphicsSurface surface, GraphicsFormat renderTargetFormat, uint minimumRenderTargetCount = 0);

    /// <summary>Creates a new staging graphics buffer.</summary>
    /// <param name="size">The size, in bytes, of the graphics buffer.</param>
    /// <returns>A created staging graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    /// <remarks>This is an alternative name for <see cref="CreateUploadBuffer(nuint)" />.</remarks>
    public GraphicsBuffer CreateStagingBuffer(nuint size) => CreateUploadBuffer(size);

    /// <summary>Creates a new one-dimensional graphics texture.</summary>
    /// <param name="format">The format of the texture.</param>
    /// <param name="width">The width, in pixels, of the texture.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the texture.</param>
    /// <returns>A created one-dimensional graphics texture.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="width" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsTexture CreateTexture1D(GraphicsFormat format, uint width, GraphicsResourceCpuAccess cpuAccess = GraphicsResourceCpuAccess.None)
    {
        var textureCreateInfo = new GraphicsTextureCreateInfo {
            CpuAccess = cpuAccess,
            Depth = 1,
            Format = format,
            Height = 1,
            Kind = GraphicsTextureKind.OneDimensional,
            MipLevelCount = 1,
            Width = width,
        };
        return CreateTexture(in textureCreateInfo);
    }

    /// <summary>Creates a new two-dimensional graphics texture.</summary>
    /// <param name="format">The format of the texture.</param>
    /// <param name="width">The width, in pixels, of the texture.</param>
    /// <param name="height">The height, in pixels, of the texture.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the texture.</param>
    /// <returns>A created graphics texture.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="width" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="height" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsTexture CreateTexture2D(GraphicsFormat format, uint width, uint height, GraphicsResourceCpuAccess cpuAccess = GraphicsResourceCpuAccess.None)
    {
        var textureCreateInfo = new GraphicsTextureCreateInfo {
            CpuAccess = cpuAccess,
            Depth = 1,
            Format = format,
            Height = height,
            Kind = GraphicsTextureKind.TwoDimensional,
            MipLevelCount = 1,
            Width = width,
        };
        return CreateTexture(in textureCreateInfo);
    }

    /// <summary>Creates a new three-dimensional graphics texture.</summary>
    /// <param name="format">The format of the texture.</param>
    /// <param name="width">The width, in pixels, of the texture.</param>
    /// <param name="height">The height, in pixels, of the texture.</param>
    /// <param name="depth">The depth, in pixels, of the texture.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the texture.</param>
    /// <returns>A created graphics texture.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="width" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="height" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="depth" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsTexture CreateTexture3D(GraphicsFormat format, uint width, uint height, ushort depth, GraphicsResourceCpuAccess cpuAccess = GraphicsResourceCpuAccess.None)
    {
        var textureCreateInfo = new GraphicsTextureCreateInfo {
            CpuAccess = cpuAccess,
            Depth = depth,
            Format = format,
            Height = height,
            Kind = GraphicsTextureKind.ThreeDimensional,
            MipLevelCount = 1,
            Width = width,
        };
        return CreateTexture(in textureCreateInfo);
    }

    /// <summary>Creates a new graphics texture.</summary>
    /// <param name="textureCreateInfo">The creation info describing the texture.</param>
    /// <returns>The created graphics texture.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureCreateInfo.Kind" /> of <paramref name="textureCreateInfo" /> is unsupported.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureCreateInfo.Width" /> of <paramref name="textureCreateInfo" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureCreateInfo.Height" /> of <paramref name="textureCreateInfo" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureCreateInfo.Depth" /> of <paramref name="textureCreateInfo" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public abstract GraphicsTexture CreateTexture(in GraphicsTextureCreateInfo textureCreateInfo);

    /// <summary>Creates a new upload graphics buffer.</summary>
    /// <param name="size">The size, in bytes, of the graphics buffer.</param>
    /// <returns>A created upload graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    /// <remarks>This is an alternative name for <see cref="CreateStagingBuffer(nuint)" />.</remarks>
    public GraphicsBuffer CreateUploadBuffer(nuint size)
    {
        var bufferCreateInfo = new GraphicsBufferCreateInfo {
            CpuAccess = GraphicsResourceCpuAccess.Write,
            Kind = GraphicsBufferKind.Default,
            Size = size,
        };
        return CreateBuffer(in bufferCreateInfo);
    }

    /// <summary>Creates a new uniform graphics buffer.</summary>
    /// <param name="size">The size, in bytes, of the graphics buffer.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the buffer.</param>
    /// <returns>A created uniform graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    /// <remarks>This is an alternative name for <see cref="CreateConstantBuffer(nuint, GraphicsResourceCpuAccess)" />.</remarks>
    public GraphicsBuffer CreateUniformBuffer(nuint size, GraphicsResourceCpuAccess cpuAccess = GraphicsResourceCpuAccess.None) => CreateConstantBuffer(size, cpuAccess);

    /// <summary>Creates a new vertex graphics buffer.</summary>
    /// <param name="size">The size, in bytes, of the graphics buffer.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the buffer.</param>
    /// <returns>A created vertex graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBuffer CreateVertexBuffer(nuint size, GraphicsResourceCpuAccess cpuAccess = GraphicsResourceCpuAccess.None)
    {
        var bufferCreateInfo = new GraphicsBufferCreateInfo {
            CpuAccess = cpuAccess,
            Kind = GraphicsBufferKind.Vertex,
            Size = size,
        };
        return CreateBuffer(in bufferCreateInfo);
    }

    /// <summary>Gets the memory budget for a given memory manager.</summary>
    /// <param name="memoryManager">The memory manager for which to get its budget.</param>
    /// <returns>The memory budget for <paramref name="memoryManager" />.</returns>
    public abstract GraphicsMemoryBudget GetMemoryBudget(GraphicsMemoryManager memoryManager);

    /// <summary>Rents a compute context from the device.</summary>
    /// <returns>A compute context for the device.</returns>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsComputeContext RentComputeContext();

    /// <summary>Rents a copy context from the device.</summary>
    /// <returns>A copy context for the device.</returns>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsCopyContext RentCopyContext();

    /// <summary>Rents a render context from the device.</summary>
    /// <returns>A render context for the device.</returns>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsRenderContext RentRenderContext();

    /// <summary>Returns a compute context to the device for further use.</summary>
    /// <param name="computeContext">The compute context that should be returned.</param>
    /// <exception cref="ArgumentNullException"><paramref name="computeContext" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="computeContext" /> is not owned by the device.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract void ReturnContext(GraphicsComputeContext computeContext);

    /// <summary>Returns a copy context to the device for further use.</summary>
    /// <param name="copyContext">The copy context that should be returned.</param>
    /// <exception cref="ArgumentNullException"><paramref name="copyContext" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="copyContext" /> is not owned by the device.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract void ReturnContext(GraphicsCopyContext copyContext);

    /// <summary>Returns a render context to the device for further use.</summary>
    /// <param name="renderContext">The render context that should be returned.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderContext" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="renderContext" /> is not owned by the device.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract void ReturnContext(GraphicsRenderContext renderContext);

    /// <summary>Signals a graphics fence.</summary>
    /// <param name="fence">The fence to be signalled</param>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract void Signal(GraphicsFence fence);

    /// <summary>Waits for the device to become idle.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract void WaitForIdle();
}
