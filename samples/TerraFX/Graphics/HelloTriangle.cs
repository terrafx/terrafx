// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics;

public sealed class HelloTriangle : HelloWindow
{
    private GraphicsPrimitive _trianglePrimitive = null!;
    private GraphicsBuffer _vertexBuffer = null!;

    public HelloTriangle(string name, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
    }

    public override void Cleanup()
    {
        _trianglePrimitive?.Dispose();
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

        using var vertexStagingBuffer = graphicsDevice.CreateBuffer(GraphicsResourceCpuAccess.Write, GraphicsBufferKind.Default, 64 * 1024);

        _vertexBuffer = graphicsDevice.CreateBuffer(GraphicsResourceCpuAccess.None, GraphicsBufferKind.Vertex, 64 * 1024);

        graphicsRenderContext.Reset();
        _trianglePrimitive = CreateTrianglePrimitive(graphicsRenderContext, vertexStagingBuffer);
        graphicsRenderContext.Flush();

        graphicsDevice.WaitForIdle();
        graphicsDevice.ReturnRenderContext(graphicsRenderContext);
    }

    protected override void Draw(GraphicsRenderContext graphicsRenderContext)
    {
        graphicsRenderContext.Draw(_trianglePrimitive);
        base.Draw(graphicsRenderContext);
    }

    private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer)
    {
        var graphicsDevice = GraphicsDevice;
        var graphicsRenderPass = GraphicsRenderPass;
        var graphicsSurface = graphicsRenderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(graphicsRenderPass, "Identity", "main", "main");
        var vertexBuffer = _vertexBuffer;

        var vertexBufferView = CreateVertexBufferView(graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
        graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

        return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferView);

        static GraphicsResourceView CreateVertexBufferView(GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
        {
            var vertexBufferView = new GraphicsResourceView {
                Offset = 0,
                Resource = vertexBuffer,
                Size = SizeOf<IdentityVertex>() * 3,
                Stride = SizeOf<IdentityVertex>(),
            };
            var pVertexBuffer = vertexStagingBuffer.Map<IdentityVertex>(vertexBufferView.Offset, vertexBufferView.Size);

            pVertexBuffer[0] = new IdentityVertex {
                Color = Colors.Red,
                Position = Vector3.Create(0.0f, 0.25f * aspectRatio, 0.0f),
            };

            pVertexBuffer[1] = new IdentityVertex {
                Color = Colors.Lime,
                Position = Vector3.Create(0.25f, -0.25f * aspectRatio, 0.0f),
            };

            pVertexBuffer[2] = new IdentityVertex {
                Color = Colors.Blue,
                Position = Vector3.Create(-0.25f, -0.25f * aspectRatio, 0.0f),
            };

            vertexStagingBuffer.UnmapAndWrite(vertexBufferView.Offset, vertexBufferView.Size);
            return vertexBufferView;
        }

        GraphicsPipeline CreateGraphicsPipeline(GraphicsRenderPass graphicsRenderPass, string shaderName, string vertexShaderEntryPoint, string pixelShaderEntryPoint)
        {
            var graphicsDevice = graphicsRenderPass.Device;

            var signature = CreateGraphicsPipelineSignature(graphicsDevice);
            var vertexShader = CompileShader(graphicsDevice, GraphicsShaderKind.Vertex, shaderName, vertexShaderEntryPoint);
            var pixelShader = CompileShader(graphicsDevice, GraphicsShaderKind.Pixel, shaderName, pixelShaderEntryPoint);

            return graphicsRenderPass.CreatePipeline(signature, vertexShader, pixelShader);
        }

        static GraphicsPipelineSignature CreateGraphicsPipelineSignature(GraphicsDevice graphicsDevice)
        {
            var inputs = new GraphicsPipelineInput[1] {
                new GraphicsPipelineInput(
                    new GraphicsPipelineInputElement[2] {
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.Color, GraphicsFormat.R32G32B32A32_SFLOAT, size: 16, alignment: 16),
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.Position, GraphicsFormat.R32G32B32_SFLOAT, size: 12, alignment: 4),
                    }
                ),
            };

            return graphicsDevice.CreatePipelineSignature(inputs);
        }
    }
}
