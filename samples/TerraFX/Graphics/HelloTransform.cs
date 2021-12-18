// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics;

public sealed class HelloTransform : HelloWindow
{
    private GraphicsBuffer _constantBuffer = null!;
    private GraphicsPrimitive _trianglePrimitive = null!;
    private GraphicsBuffer _uploadBuffer = null!;
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
        _uploadBuffer?.Dispose();
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

        _constantBuffer = graphicsDevice.CreateConstantBuffer(64 * 1024, GraphicsResourceCpuAccess.Write);
        _uploadBuffer = graphicsDevice.CreateUploadBuffer(64 * 1024);
        _vertexBuffer = graphicsDevice.CreateVertexBuffer(64 * 1024);

        var graphicsCopyContext = graphicsDevice.RentCopyContext();
        {
            graphicsCopyContext.Reset();
            _trianglePrimitive = CreateTrianglePrimitive(graphicsCopyContext);

            graphicsCopyContext.Flush();
            graphicsDevice.WaitForIdle();
        }
        graphicsDevice.ReturnContext(graphicsCopyContext);

        _uploadBuffer.DisposeAllViews();
    }

    protected override void Draw(GraphicsRenderContext graphicsRenderContext)
    {
        _trianglePrimitive.Draw(graphicsRenderContext);
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

        var constantBufferView = _trianglePrimitive.PipelineResourceViews![1].As<GraphicsBufferView>();
        var constantBufferSpan = constantBufferView.Map<Matrix4x4>();
        {
            // Shaders take transposed matrices, so we want to set X.W
            constantBufferSpan[0] = Matrix4x4.Identity;
            constantBufferSpan[0].X = Vector4.Create(1.0f, 0.0f, 0.0f, trianglePrimitiveTranslationX);
        }
        constantBufferView.UnmapAndWrite();
    }

    private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsCopyContext graphicsCopyContext)
    {
        var graphicsRenderPass = GraphicsRenderPass;
        var graphicsSurface = graphicsRenderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(graphicsRenderPass, "Transform", "main", "main");

        var constantBuffer = _constantBuffer;
        var uploadBuffer = _uploadBuffer;

        return new GraphicsPrimitive(
            graphicsPipeline,
            CreateVertexBufferView(graphicsCopyContext, _vertexBuffer, uploadBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height),
            resourceViews: new GraphicsResourceView[2] {
                CreateConstantBufferView(graphicsCopyContext, constantBuffer),
                CreateConstantBufferView(graphicsCopyContext, constantBuffer),
            }
        );

        static GraphicsBufferView CreateConstantBufferView(GraphicsCopyContext graphicsCopyContext, GraphicsBuffer constantBuffer)
        {
            var constantBufferView = constantBuffer.CreateView<Matrix4x4>(1);
            var constantBufferSpan = constantBufferView.Map<Matrix4x4>();
            {
                constantBufferSpan[0] = Matrix4x4.Identity;
            }
            constantBufferView.UnmapAndWrite();
            return constantBufferView;
        }

        static GraphicsBufferView CreateVertexBufferView(GraphicsCopyContext graphicsCopyContext, GraphicsBuffer vertexBuffer, GraphicsBuffer uploadBuffer, float aspectRatio)
        {
            var uploadBufferView = uploadBuffer.CreateView<IdentityVertex>(3);
            var vertexBufferSpan = uploadBufferView.Map<IdentityVertex>();
            {
                vertexBufferSpan[0] = new IdentityVertex {
                    Color = Colors.Red,
                    Position = Vector3.Create(0.0f, 0.25f * aspectRatio, 0.0f),
                };

                vertexBufferSpan[1] = new IdentityVertex {
                    Color = Colors.Lime,
                    Position = Vector3.Create(0.25f, -0.25f * aspectRatio, 0.0f),
                };

                vertexBufferSpan[2] = new IdentityVertex {
                    Color = Colors.Blue,
                    Position = Vector3.Create(-0.25f, -0.25f * aspectRatio, 0.0f),
                };
            }
            uploadBufferView.UnmapAndWrite();

            var vertexBufferView = vertexBuffer.CreateView<IdentityVertex>(3);
            graphicsCopyContext.Copy(vertexBufferView, uploadBufferView);
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

            var resources = new GraphicsPipelineResourceInfo[2] {
                new GraphicsPipelineResourceInfo(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                new GraphicsPipelineResourceInfo(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
            };

            return graphicsDevice.CreatePipelineSignature(inputs, resources);
        }
    }
}
