// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics;

/// <summary>
/// Creates a triangle textured with a checkerboard that moves in a circle.
/// The texturing is according to HelloTriangle.
/// The moving is via a translation matrix similar to HelloConstBuffer.
///   It has the same DX12 setup, but different translation math.
/// </summary>
public sealed class HelloTextureTransform : HelloWindow
{
    private GraphicsPrimitive _trianglePrimitive = null!;
    private GraphicsBuffer _constantBuffer = null!;
    private GraphicsBuffer _vertexBuffer = null!;
    private float _trianglePrimitiveTranslationAngle;

    public HelloTextureTransform(string name, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
    }

    public override void Cleanup()
    {
        _trianglePrimitive?.Dispose();
        _constantBuffer?.Dispose();
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
        var graphicsSwapchain = GraphicsSwapchain;
        var currentGraphicsContext = graphicsDevice.Contexts[(int)graphicsSwapchain.FramebufferIndex];

        using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
        using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024 * 4);

        _constantBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Constant, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
        _vertexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.GpuOnly, 64 * 1024);

        currentGraphicsContext.BeginFrame(graphicsSwapchain);
        _trianglePrimitive = CreateTrianglePrimitive(currentGraphicsContext, vertexStagingBuffer, textureStagingBuffer);
        currentGraphicsContext.EndFrame();

        graphicsDevice.WaitForIdle();
    }

    protected override unsafe void Update(TimeSpan delta)
    {
        const float TranslationSpeed = MathF.PI;

        var radians = _trianglePrimitiveTranslationAngle;
        {
            radians += (float)(TranslationSpeed * delta.TotalSeconds);
            radians %= MathF.Tau;
        }
        _trianglePrimitiveTranslationAngle = radians;

        var x = 0.5f * MathF.Cos(radians);
        var y = 0.5f * MathF.Sin(radians);

        ref readonly var constantBufferView = ref _trianglePrimitive.InputResourceViews[1];
        var pConstantBuffer = constantBufferView.Map<Matrix4x4>();

        // Shaders take transposed matrices, so we want to set X.W
        pConstantBuffer[0] = new Matrix4x4(
            new Vector4(1.0f, 0.0f, 0.0f, x),
            new Vector4(0.0f, 1.0f, 0.0f, y),
            new Vector4(0.0f, 0.0f, 1.0f, 0),
            new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
        );

        constantBufferView.UnmapAndWrite();
    }

    protected override void Draw(GraphicsContext graphicsContext)
    {
        graphicsContext.Draw(_trianglePrimitive);
        base.Draw(graphicsContext);
    }

    private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, GraphicsBuffer textureStagingBuffer)
    {
        var graphicsDevice = GraphicsDevice;
        var graphicsSurface = GraphicsSwapchain.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "TextureTransform", "main", "main");

        var constantBuffer = _constantBuffer;
        var vertexBuffer = _vertexBuffer;

        var vertexBufferView = CreateVertexBufferView(graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
        graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

        var inputResourceViews = new GraphicsResourceView[3] {
            CreateConstantBufferView(graphicsContext, constantBuffer, index: 0),
            CreateConstantBufferView(graphicsContext, constantBuffer, index: 1),
            CreateTextureRegion(graphicsContext, textureStagingBuffer),
        };
        return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferView, inputResourceViews: inputResourceViews);

        static GraphicsResourceView CreateConstantBufferView(GraphicsContext graphicsContext, GraphicsBuffer constantBuffer, uint index)
        {
            var constantBufferView = new GraphicsResourceView {
                Offset = 256 * index,
                Resource = constantBuffer,
                Size = SizeOf<Matrix4x4>(),
                Stride = SizeOf<Matrix4x4>(),
            };
            var pConstantBuffer = constantBufferView.Map<Matrix4x4>();

            pConstantBuffer[0] = Matrix4x4.Identity;

            constantBufferView.UnmapAndWrite();
            return constantBufferView;
        }

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
                Position = new Vector3(0.0f, 0.25f * aspectRatio, 0.0f),
                UV = new Vector2(0.5f, 0.0f)
            };

            pVertexBuffer[1] = new TextureVertex {
                Position = new Vector3(0.25f, -0.25f * aspectRatio, 0.0f),
                UV = new Vector2(1.0f, 1.0f)
            };

            pVertexBuffer[2] = new TextureVertex {
                Position = new Vector3(-0.25f, -0.25f * aspectRatio, 0.0f),
                UV = new Vector2(0.0f, 1.0f)
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
                        new GraphicsPipelineInputElement(typeof(Vector3), GraphicsPipelineInputElementKind.Position, size: 12),
                        new GraphicsPipelineInputElement(typeof(Vector2), GraphicsPipelineInputElementKind.TextureCoordinate, size: 8),
                    }
                ),
            };

            var resources = new GraphicsPipelineResource[3] {
                new GraphicsPipelineResource(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                new GraphicsPipelineResource(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                new GraphicsPipelineResource(GraphicsPipelineResourceKind.Texture, GraphicsShaderVisibility.Pixel),
            };

            return graphicsDevice.CreatePipelineSignature(inputs, resources);
        }
    }
}
