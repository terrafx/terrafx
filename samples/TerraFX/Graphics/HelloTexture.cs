// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics;

public sealed class HelloTexture : HelloWindow
{
    private GraphicsPrimitive _trianglePrimitive = null!;
    private GraphicsBuffer _vertexBuffer = null!;

    public HelloTexture(string name, ApplicationServiceProvider serviceProvider)
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

        using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
        using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024 * 4);

        _vertexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.GpuOnly, 64 * 1024);

        graphicsRenderContext.Reset();
        _trianglePrimitive = CreateTrianglePrimitive(graphicsRenderContext, vertexStagingBuffer, textureStagingBuffer);
        graphicsRenderContext.Flush();

        graphicsDevice.WaitForIdle();
        graphicsDevice.ReturnRenderContext(graphicsRenderContext);
    }

    protected override void Draw(GraphicsRenderContext graphicsRenderContext)
    {
        graphicsRenderContext.Draw(_trianglePrimitive);
        base.Draw(graphicsRenderContext);
    }

    private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, GraphicsBuffer textureStagingBuffer)
    {
        var graphicsDevice = GraphicsDevice;
        var graphicsSurface = GraphicsSwapchain.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Texture", "main", "main");

        var vertexBuffer = _vertexBuffer;

        var vertexBufferView = CreateVertexBufferView(graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
        graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

        var inputResourceViews = new GraphicsResourceView[1] {
            CreateTextureRegion(graphicsContext, textureStagingBuffer)
        };
        return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferView, inputResourceViews: inputResourceViews);

        static GraphicsResourceView CreateTextureRegion(GraphicsContext graphicsContext, GraphicsBuffer textureStagingBuffer)
        {
            const uint TextureWidth = 256;
            const uint TextureHeight = 256;
            const uint TexturePixels = TextureWidth * TextureHeight;
            const uint CellWidth = TextureWidth / 8;
            const uint CellHeight = TextureHeight / 8;

            var texture = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.TwoDimensional, GraphicsResourceCpuAccess.None, TextureWidth, TextureHeight);
            var textureView = new GraphicsResourceView {
                Offset = 0,
                Resource = texture,
                Size = checked((uint)texture.Size),
                Stride = SizeOf<uint>(),
            };
            var pTextureData = textureStagingBuffer.Map<uint>(textureView.Offset, textureView.Size);

            for (uint n = 0; n < TexturePixels; n++)
            {
                var x = n % TextureWidth;
                var y = n / TextureWidth;

                pTextureData[n] = (x / CellWidth % 2) == (y / CellHeight % 2)
                                ? 0xFF000000 : 0xFFFFFFFF;
            }

            textureStagingBuffer.UnmapAndWrite(textureView.Offset, textureView.Size);
            graphicsContext.Copy(texture, textureStagingBuffer);

            return textureView;
        }

        static GraphicsResourceView CreateVertexBufferView(GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
        {
            var vertexBufferView = new GraphicsResourceView {
                Offset = 0,
                Resource = vertexBuffer,
                Size = SizeOf<TextureVertex>() * 3,
                Stride = SizeOf<TextureVertex>(),
            };
            var pVertexBuffer = vertexStagingBuffer.Map<TextureVertex>(vertexBufferView.Offset, vertexBufferView.Size);

            pVertexBuffer[0] = new TextureVertex {
                Position = Vector3.Create(0.0f, 0.25f * aspectRatio, 0.0f),
                UV = Vector2.Create(0.5f, 0.0f)
            };

            pVertexBuffer[1] = new TextureVertex {
                Position = Vector3.Create(0.25f, -0.25f * aspectRatio, 0.0f),
                UV = Vector2.Create(1.0f, 1.0f)
            };

            pVertexBuffer[2] = new TextureVertex {
                Position = Vector3.Create(-0.25f, -0.25f * aspectRatio, 0.0f),
                UV = Vector2.Create(0.0f, 1.0f)
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

        static GraphicsPipelineSignature CreateGraphicsPipelineSignature(GraphicsDevice graphicsDevice)
        {
            var inputs = new GraphicsPipelineInput[1] {
                new GraphicsPipelineInput(
                    new GraphicsPipelineInputElement[2] {
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.Position, GraphicsFormat.R32G32B32_SFLOAT, size: 12, alignment: 4),
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.TextureCoordinate, GraphicsFormat.R32G32_SFLOAT, size: 8, alignment: 4),
                    }
                ),
            };

            var resources = new GraphicsPipelineResource[1] {
                new GraphicsPipelineResource(GraphicsPipelineResourceKind.Texture, GraphicsShaderVisibility.Pixel),
            };

            return graphicsDevice.CreatePipelineSignature(inputs, resources);
        }
    }
}
