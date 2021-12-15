// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics;

public sealed class HelloTransform : HelloWindow
{
    private GraphicsPrimitive _trianglePrimitive = null!;
    private GraphicsBuffer _constantBuffer = null!;
    private GraphicsBuffer _vertexBuffer = null!;
    private float _trianglePrimitiveTranslationX;

    public HelloTransform(string name, ApplicationServiceProvider serviceProvider)
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
        var graphicsRenderContext = graphicsDevice.RentRenderContext(); // TODO: This could be a copy only context

        using var vertexStagingBuffer = graphicsDevice.CreateBuffer(GraphicsResourceCpuAccess.Write, GraphicsBufferKind.Default, 64 * 1024);

        _constantBuffer = graphicsDevice.CreateBuffer(GraphicsResourceCpuAccess.Write, GraphicsBufferKind.Constant, 64 * 1024);
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

    protected override unsafe void Update(TimeSpan delta)
    {
        const float TranslationSpeed = 1.0f;
        const float OffsetBounds = 1.25f;

        var trianglePrimitiveTranslationX = _trianglePrimitiveTranslationX;
        {
            trianglePrimitiveTranslationX += (float)(TranslationSpeed * delta.TotalSeconds);

            if (trianglePrimitiveTranslationX > OffsetBounds)
            {
                trianglePrimitiveTranslationX = -OffsetBounds;
            }
        }
        _trianglePrimitiveTranslationX = trianglePrimitiveTranslationX;

        var constantBufferView = _trianglePrimitive.InputResourceViews[1];
        var pConstantBuffer = constantBufferView.Map<Matrix4x4>();

        // Shaders take transposed matrices, so we want to set X.W
        pConstantBuffer[0] = Matrix4x4.Identity;
        pConstantBuffer[0].X = Vector4.Create(1.0f, 0.0f, 0.0f, trianglePrimitiveTranslationX);

        constantBufferView.UnmapAndWrite();
    }

    private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer)
    {
        var graphicsDevice = GraphicsDevice;
        var graphicsRenderPass = GraphicsRenderPass;
        var graphicsSurface = graphicsRenderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(graphicsRenderPass, "Transform", "main", "main");

        var constantBuffer = _constantBuffer;
        var vertexBuffer = _vertexBuffer;

        var vertexBufferView = CreateVertexBufferView(graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
        graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

        var inputResourceViews = new GraphicsResourceView[2] {
            CreateConstantBufferView(graphicsContext, constantBuffer, index: 0),
            CreateConstantBufferView(graphicsContext, constantBuffer, index: 1),
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

            var resources = new GraphicsPipelineResource[2] {
                new GraphicsPipelineResource(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                new GraphicsPipelineResource(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
            };

            return graphicsDevice.CreatePipelineSignature(inputs, resources);
        }
    }
}
