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
    private GraphicsBuffer _constantBuffer = null!;
    private GraphicsPrimitive _trianglePrimitive = null!;
    private GraphicsTexture _texture2D = null!;
    private GraphicsBuffer _uploadBuffer = null!;
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

        _constantBuffer = graphicsDevice.CreateConstantBuffer(64 * 1024, GraphicsResourceCpuAccess.Write);
        _texture2D = graphicsDevice.CreateTexture2D(GraphicsFormat.R8G8B8A8_UNORM, 256, 256);
        _uploadBuffer = graphicsDevice.CreateUploadBuffer(1 * 1024 * 1024);
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

        var constantBufferView = _trianglePrimitive.InputResourceViews[1].As<GraphicsBufferView>();
        var constantBufferSpan = constantBufferView.Map<Matrix4x4>();
        {
            // Shaders take transposed matrices, so we want to set X.W
            constantBufferSpan[0] = Matrix4x4.Create(
                Vector4.Create(1.0f, 0.0f, 0.0f, x),
                Vector4.Create(0.0f, 1.0f, 0.0f, y),
                Vector4.Create(0.0f, 0.0f, 1.0f, 0),
                Vector4.UnitW
            );
        }
        constantBufferView.UnmapAndWrite();
    }

    protected override void Draw(GraphicsRenderContext graphicsRenderContext)
    {
        graphicsRenderContext.Draw(_trianglePrimitive);
        base.Draw(graphicsRenderContext);
    }

    private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsCopyContext graphicsCopyContext)
    {
        var graphicsRenderPass = GraphicsRenderPass;
        var graphicsSurface = graphicsRenderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(graphicsRenderPass, "TextureTransform", "main", "main");

        var constantBuffer = _constantBuffer;
        var uploadBuffer = _uploadBuffer;

        return GraphicsDevice.CreatePrimitive(
            graphicsPipeline,
            CreateVertexBufferView(graphicsCopyContext, _vertexBuffer, uploadBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height),
            inputResourceViews: new GraphicsResourceView[3] {
                CreateConstantBufferView(graphicsCopyContext, constantBuffer),
                CreateConstantBufferView(graphicsCopyContext, constantBuffer),
                CreateTexture2DView(graphicsCopyContext, _texture2D, uploadBuffer),
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

        static GraphicsTextureView CreateTexture2DView(GraphicsCopyContext graphicsCopyContext, GraphicsTexture texture2D, GraphicsBuffer uploadBuffer)
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
            graphicsCopyContext.Copy(texture2DView, uploadBufferView);
            return texture2DView;
        }

        static GraphicsBufferView CreateVertexBufferView(GraphicsCopyContext graphicsCopyContext, GraphicsBuffer vertexBuffer, GraphicsBuffer uploadBuffer, float aspectRatio)
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
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.Position, GraphicsFormat.R32G32B32_SFLOAT, size: 12, alignment: 4),
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.TextureCoordinate, GraphicsFormat.R32G32_SFLOAT, size: 8, alignment: 4),
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
