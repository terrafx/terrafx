// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;

namespace TerraFX.Samples.Graphics;

public sealed class HelloTexture : HelloWindow
{
    private GraphicsPrimitive _trianglePrimitive = null!;
    private GraphicsTexture _texture2D = null!;
    private GraphicsBuffer _uploadBuffer = null!;
    private GraphicsBuffer _vertexBuffer = null!;

    public HelloTexture(string name, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
    }

    public override void Cleanup()
    {
        _trianglePrimitive?.Dispose();

        _texture2D?.Dispose();
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
        var graphicsRenderContext = graphicsDevice.RentRenderContext(); // TODO: This could be a copy only context

        _texture2D = graphicsDevice.CreateTexture2D(GraphicsFormat.R8G8B8A8_UNORM, 256, 256);
        _uploadBuffer = graphicsDevice.CreateUploadBuffer(1 * 1024 * 1024);
        _vertexBuffer = graphicsDevice.CreateVertexBuffer(64 * 1024);

        graphicsRenderContext.Reset();
        _trianglePrimitive = CreateTrianglePrimitive(graphicsRenderContext);
        graphicsRenderContext.Flush();

        graphicsDevice.WaitForIdle();
        graphicsDevice.ReturnRenderContext(graphicsRenderContext);

        _uploadBuffer.DisposeAllViews();
    }

    protected override void Draw(GraphicsRenderContext graphicsRenderContext)
    {
        graphicsRenderContext.Draw(_trianglePrimitive);
        base.Draw(graphicsRenderContext);
    }

    private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsContext graphicsContext)
    {
        var graphicsRenderPass = GraphicsRenderPass;
        var graphicsSurface = graphicsRenderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(graphicsRenderPass, "Texture", "main", "main");
        var uploadBuffer = _uploadBuffer;

        return GraphicsDevice.CreatePrimitive(
            graphicsPipeline,
            CreateVertexBufferView(graphicsContext, _vertexBuffer, uploadBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height),
            inputResourceViews: new GraphicsResourceView[1] {
                CreateTexture2DView(graphicsContext, _texture2D, uploadBuffer)
            }
        );

        static GraphicsTextureView CreateTexture2DView(GraphicsContext graphicsContext, GraphicsTexture texture2D, GraphicsBuffer uploadBuffer)
        {
            var uploadBufferView = uploadBuffer.CreateView<byte>(checked((uint)texture2D.Size));
            var textureDataSpan = uploadBufferView.Map<byte>();
            {
                var width = texture2D.Width;

                var height = texture2D.Height;
                var rowPitch = texture2D.RowPitch;

                var cellWidth = width / 8;
                var cellHeight = height / 8;

                for (var y = 0u; y < height; y++)
                {
                    var rowIndex = y * rowPitch;
                    var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                    for (var x = 0u; x < width; x++)
                    {
                        if ((x / cellWidth % 2) == (y / cellHeight % 2))
                        {
                            row[x] = 0xFF000000;
                        }
                        else
                        {
                            row[x] = 0xFFFFFFFF;
                        }
                    }
                }
            }
            uploadBufferView.UnmapAndWrite();

            var texture2DView = texture2D.CreateView(0, 1);
            graphicsContext.Copy(texture2DView, uploadBufferView);
            return texture2DView;
        }

        static GraphicsBufferView CreateVertexBufferView(GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer uploadBuffer, float aspectRatio)
        {
            var uploadBufferView = uploadBuffer.CreateView<TextureVertex>(3);
            var vertexBufferSpan = uploadBufferView.Map<TextureVertex>();
            {
                vertexBufferSpan[0] = new TextureVertex {
                    Position = Vector3.Create(0.0f, 0.25f * aspectRatio, 0.0f),
                    UV = Vector2.Create(0.5f, 0.0f)
                };

                vertexBufferSpan[1] = new TextureVertex {
                    Position = Vector3.Create(0.25f, -0.25f * aspectRatio, 0.0f),
                    UV = Vector2.Create(1.0f, 1.0f)
                };

                vertexBufferSpan[2] = new TextureVertex {
                    Position = Vector3.Create(-0.25f, -0.25f * aspectRatio, 0.0f),
                    UV = Vector2.Create(0.0f, 1.0f)
                };
            }
            uploadBufferView.UnmapAndWrite();

            var vertexBufferView = vertexBuffer.CreateView<TextureVertex>(3);
            graphicsContext.Copy(vertexBufferView, uploadBufferView);
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
