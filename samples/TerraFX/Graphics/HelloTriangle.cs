// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;

namespace TerraFX.Samples.Graphics;

public sealed class HelloTriangle : HelloWindow
{
    private GraphicsPrimitive _trianglePrimitive = null!;
    private GraphicsBuffer _uploadBuffer = null!;
    private GraphicsBuffer _vertexBuffer = null!;

    public HelloTriangle(string name, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
    }

    public override void Cleanup()
    {
        _trianglePrimitive?.Dispose();

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
        
        _uploadBuffer = graphicsDevice.CreateUploadBuffer(64 * 1024);
        _vertexBuffer = graphicsDevice.CreateVertexBuffer(64 * 1024);

        var copyCommandQueue = graphicsDevice.CopyCommandQueue;
        var copyContext = copyCommandQueue.RentContext();
        {
            copyContext.Reset();
            {
                _trianglePrimitive = CreateTrianglePrimitive(copyContext);
            }
            copyContext.Close();
            copyContext.Execute();
        }
        copyCommandQueue.ReturnContext(copyContext);

        _uploadBuffer.DisposeAllViews();
    }

    protected override void Draw(GraphicsRenderContext graphicsRenderContext)
    {
        _trianglePrimitive.Draw(graphicsRenderContext);
        base.Draw(graphicsRenderContext);
    }

    private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsCopyContext copyContext)
    {
        var renderPass = RenderPass;
        var surface = renderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(renderPass, "Identity", "main", "main");
        var uploadBuffer = _uploadBuffer;

        return new GraphicsPrimitive(
            graphicsPipeline,
            CreateVertexBufferView(copyContext, _vertexBuffer, uploadBuffer, aspectRatio: surface.PixelWidth / surface.PixelHeight)
        );

        static GraphicsBufferView CreateVertexBufferView(GraphicsCopyContext copyContext, GraphicsBuffer vertexBuffer, GraphicsBuffer uploadBuffer, float aspectRatio)
        {
            var uploadBufferView = uploadBuffer.CreateBufferView<IdentityVertex>(3);
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

            var vertexBufferView = vertexBuffer.CreateBufferView<IdentityVertex>(3);
            copyContext.Copy(vertexBufferView, uploadBufferView);
            return vertexBufferView;
        }

        GraphicsPipeline CreateGraphicsPipeline(GraphicsRenderPass renderPass, string shaderName, string vertexShaderEntryPoint, string pixelShaderEntryPoint)
        {
            var graphicsDevice = renderPass.Device;

            var pipelineCreateOptions = new GraphicsPipelineCreateOptions {
                Signature = CreateGraphicsPipelineSignature(graphicsDevice),
                PixelShader = CompileShader(graphicsDevice, GraphicsShaderKind.Pixel, shaderName, pixelShaderEntryPoint),
                VertexShader = CompileShader(graphicsDevice, GraphicsShaderKind.Vertex, shaderName, vertexShaderEntryPoint),
            };

            return renderPass.CreatePipeline(in pipelineCreateOptions);
        }

        static GraphicsPipelineSignature CreateGraphicsPipelineSignature(GraphicsDevice graphicsDevice)
        {
            var inputs = new UnmanagedArray<GraphicsPipelineInput>(2) {
                [0] = new GraphicsPipelineInput {
                    BindingIndex = 0,
                    ByteAlignment = 16,
                    ByteLength = 16,
                    Format = GraphicsFormat.R32G32B32A32_SFLOAT,
                    Kind = GraphicsPipelineInputKind.Color,
                    ShaderVisibility = GraphicsShaderVisibility.Vertex,
                },
                [1] = new GraphicsPipelineInput {
                    BindingIndex = 1,
                    ByteAlignment = 4,
                    ByteLength = 12,
                    Format = GraphicsFormat.R32G32B32_SFLOAT,
                    Kind = GraphicsPipelineInputKind.Position,
                    ShaderVisibility = GraphicsShaderVisibility.Vertex,
                },
            };

            return graphicsDevice.CreatePipelineSignature(inputs);
        }
    }
}
