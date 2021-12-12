// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics;

public sealed class HelloQuad : HelloWindow
{
    private GraphicsPrimitive _quadPrimitive = null!;
    private GraphicsBuffer _indexBuffer = null!;
    private GraphicsBuffer _vertexBuffer = null!;

    public HelloQuad(string name, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
    }

    public override void Cleanup()
    {
        _quadPrimitive?.Dispose();
        _indexBuffer?.Dispose();
        _vertexBuffer?.Dispose();
        base.Cleanup();
    }

    /// <summary>Initializes the GUI for this sample.</summary>
    /// <param name="application">The hosting <see cref="Application" />.</param>
    /// <param name="timeout">The <see cref="TimeSpan" /> after which this sample should stop running.</param>
    /// <param name="windowLocation">The <see cref="Vector2" /> that defines the initial window location.</param>
    /// <param name="windowSize">The <see cref="Vector2" /> that defines the initial window client rectangle size.</param>
    public override void Initialize(Application application, TimeSpan timeout, Vector2? windowLocation, Vector2? windowSize)
    {
        base.Initialize(application, timeout, windowLocation, windowSize);

        var graphicsDevice = GraphicsDevice;
        var graphicsRenderContext = graphicsDevice.RentRenderContext(); // TODO: This could be a copy only context

        using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
        _vertexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.GpuOnly, 64 * 1024);

        using var indexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
        _indexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Index, GraphicsResourceCpuAccess.GpuOnly, 64 * 1024);

        graphicsRenderContext.Reset();
        _quadPrimitive = CreateQuadPrimitive(graphicsRenderContext, vertexStagingBuffer, indexStagingBuffer);
        graphicsRenderContext.Flush();

        graphicsDevice.WaitForIdle();
        graphicsDevice.ReturnRenderContext(graphicsRenderContext);
    }

    protected override void Draw(GraphicsRenderContext graphicsRenderContext)
    {
        graphicsRenderContext.Draw(_quadPrimitive);
        base.Draw(graphicsRenderContext);
    }

    private unsafe GraphicsPrimitive CreateQuadPrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, GraphicsBuffer indexStagingBuffer)
    {
        var graphicsDevice = GraphicsDevice;
        var graphicsSurface = GraphicsSwapchain.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Identity", "main", "main");

        var indexBuffer = _indexBuffer;
        var vertexBuffer = _vertexBuffer;

        var vertexBufferView = CreateVertexBufferView(graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
        graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

        var indexBufferView = CreateIndexBufferView(graphicsContext, indexBuffer, indexStagingBuffer);
        graphicsContext.Copy(indexBuffer, indexStagingBuffer);

        return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferView, indexBufferView);

        static GraphicsResourceView CreateIndexBufferView(GraphicsContext graphicsContext, GraphicsBuffer indexBuffer, GraphicsBuffer indexStagingBuffer)
        {
            var indexBufferView = new GraphicsResourceView {
                Offset = 0,
                Resource = indexBuffer,
                Size = SizeOf<ushort>() * 6,
                Stride = SizeOf<ushort>(),
            };
            var pIndexBuffer = indexStagingBuffer.Map<ushort>(indexBufferView.Offset, indexBufferView.Size);

            // clockwise when looking at the triangle from the outside

            pIndexBuffer[0] = 0;
            pIndexBuffer[1] = 1;
            pIndexBuffer[2] = 2;

            pIndexBuffer[3] = 0;
            pIndexBuffer[4] = 2;
            pIndexBuffer[5] = 3;

            indexStagingBuffer.UnmapAndWrite(indexBufferView.Offset, indexBufferView.Size);
            return indexBufferView;
        }

        static GraphicsResourceView CreateVertexBufferView(GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
        {
            var vertexBufferView = new GraphicsResourceView {
                Offset = 0,
                Resource = vertexBuffer,
                Size = SizeOf<IdentityVertex>() * 4,
                Stride = SizeOf<IdentityVertex>(),
            };
            var pVertexBuffer = vertexStagingBuffer.Map<IdentityVertex>(vertexBufferView.Offset, vertexBufferView.Size);

            pVertexBuffer[0] = new IdentityVertex {                         //
                Color = Colors.Red,                                         //   y          in this setup
                Position = Vector3.Create(-0.25f, 0.25f * aspectRatio, 0.0f),  //   ^     z    the origin o
            };                                                              //   |   /      is in the middle
                                                                            //   | /        of the rendered scene
            pVertexBuffer[1] = new IdentityVertex {                         //   o------>x
                Color = Colors.Lime,                                        //
                Position = Vector3.Create(0.25f, 0.25f * aspectRatio, 0.0f),   //   0 ----- 1
            };                                                              //   | \     |
                                                                            //   |   \   |
            pVertexBuffer[2] = new IdentityVertex {                         //   |     \ |
                Color = Colors.Blue,                                        //   3-------2
                Position = Vector3.Create(0.25f, -0.25f * aspectRatio, 0.0f),  //
            };

            pVertexBuffer[3] = new IdentityVertex {
                Color = Colors.Lime,
                Position = Vector3.Create(-0.25f, -0.25f * aspectRatio, 0.0f),
            };

            vertexStagingBuffer.UnmapAndWrite(vertexBufferView.Offset, vertexBufferView.Size);
            return vertexBufferView;
        }

        GraphicsPipeline CreateGraphicsPipeline(GraphicsDevice graphicsDevice, string shaderName, string vertexShaderEntryPoint, string pixelShaderEntryPoint)
        {
            var signature = CreateGraphicsPipelineSignature(graphicsDevice);
            var vertexShader = CompileShader(graphicsDevice, GraphicsShaderKind.Vertex, shaderName, vertexShaderEntryPoint);
            var pixelShader = CompileShader(graphicsDevice, GraphicsShaderKind.Pixel, shaderName, pixelShaderEntryPoint);

            return graphicsDevice.CreatePipeline(signature, vertexShader, pixelShader);
        }

        GraphicsPipelineSignature CreateGraphicsPipelineSignature(GraphicsDevice graphicsDevice)
        {
            var inputs = new GraphicsPipelineInput[1] {
                new GraphicsPipelineInput(
                    new GraphicsPipelineInputElement[2] {
                        new GraphicsPipelineInputElement(typeof(ColorRgba), GraphicsPipelineInputElementKind.Color, size: 16),
                        new GraphicsPipelineInputElement(typeof(Vector3), GraphicsPipelineInputElementKind.Position, size: 12),
                    }
                ),
            };

            return graphicsDevice.CreatePipelineSignature(inputs);
        }
    }
}
