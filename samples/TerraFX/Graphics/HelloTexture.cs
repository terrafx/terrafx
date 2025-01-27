// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Samples.Graphics;

internal sealed class HelloTexture(string name) : HelloWindow(name)
{
    private GraphicsPrimitive _trianglePrimitive = null!;
    private GraphicsTexture _texture2D = null!;
    private GraphicsBuffer _uploadBuffer = null!;
    private GraphicsBuffer _vertexBuffer = null!;

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

        _texture2D = graphicsDevice.CreateTexture2D(GraphicsFormat.R8G8B8A8_UNORM, 256, 256);
        _uploadBuffer = graphicsDevice.CreateUploadBuffer(1 * 1024 * 1024);
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

    protected override void Draw(GraphicsRenderContext renderContext)
    {
        ThrowIfNull(renderContext);
        _trianglePrimitive.Draw(renderContext);
        base.Draw(renderContext);
    }

    private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsCopyContext copyContext)
    {
        var renderPass = RenderPass;
        var surface = renderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(renderPass, "Texture", "main", "main");
        var uploadBuffer = _uploadBuffer;

        return new GraphicsPrimitive(
            graphicsPipeline,
            CreateVertexBufferView(copyContext, _vertexBuffer, uploadBuffer, aspectRatio: surface.PixelWidth / surface.PixelHeight),
            resourceViews: [
                CreateTexture2DView(copyContext, _texture2D, uploadBuffer)
            ]
        );

        static GraphicsTextureView CreateTexture2DView(GraphicsCopyContext copyContext, GraphicsTexture texture2D, GraphicsBuffer uploadBuffer)
        {
            var uploadBufferView = uploadBuffer.CreateBufferView<byte>(checked((uint)texture2D.ByteLength));
            var textureDataSpan = uploadBufferView.Map<byte>();
            {
                var width = texture2D.PixelWidth;

                var height = texture2D.PixelHeight;
                var bytesPerRow = texture2D.BytesPerRow;

                var cellWidth = width / 8;
                var cellHeight = height / 8;

                for (var y = 0u; y < height; y++)
                {
                    var rowIndex = y * bytesPerRow;
                    var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                    for (var x = 0u; x < width; x++)
                    {
                        row[x] = ((x / cellWidth % 2) == (y / cellHeight % 2)) ? 0xFF000000 : 0xFFFFFFFF;
                    }
                }
            }
            uploadBufferView.UnmapAndWrite();

            var texture2DView = texture2D.CreateView(0, 1);
            copyContext.Copy(texture2DView, uploadBufferView);
            return texture2DView;
        }

        static GraphicsBufferView CreateVertexBufferView(GraphicsCopyContext copyContext, GraphicsBuffer vertexBuffer, GraphicsBuffer uploadBuffer, float aspectRatio)
        {
            var uploadBufferView = uploadBuffer.CreateBufferView<TextureVertex>(3);
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

            var vertexBufferView = vertexBuffer.CreateBufferView<TextureVertex>(3);
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
                    ByteAlignment = 4,
                    ByteLength = 12,
                    Format = GraphicsFormat.R32G32B32_SFLOAT,
                    Kind = GraphicsPipelineInputKind.Position,
                    ShaderVisibility = GraphicsShaderVisibility.Vertex,
                },
                [1] = new GraphicsPipelineInput {
                    BindingIndex = 1,
                    ByteAlignment = 4,
                    ByteLength = 8,
                    Format = GraphicsFormat.R32G32_SFLOAT,
                    Kind = GraphicsPipelineInputKind.TextureCoordinate,
                    ShaderVisibility = GraphicsShaderVisibility.Vertex,
                },
            };

            var resources = new UnmanagedArray<GraphicsPipelineResource>(1) {
                [0] = new GraphicsPipelineResource {
                    BindingIndex = 0,
                    Kind = GraphicsPipelineResourceKind.Texture,
                    ShaderVisibility = GraphicsShaderVisibility.Pixel,
                },
            };

            return graphicsDevice.CreatePipelineSignature(inputs, resources);
        }
    }
}
