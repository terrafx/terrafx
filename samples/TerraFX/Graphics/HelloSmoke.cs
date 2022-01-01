// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics;

public sealed class HelloSmoke : HelloWindow
{
    private readonly bool _isQuickAndDirty;

    private GraphicsBuffer _constantBuffer = null!;
    private GraphicsBuffer _indexBuffer = null!;
    private GraphicsPrimitive _quadPrimitive = null!;
    private GraphicsTexture _texture3D = null!;
    private GraphicsBuffer _uploadBuffer = null!;
    private GraphicsBuffer _vertexBuffer = null!;
    private float _texturePosition;

    public HelloSmoke(string name, bool isQuickAndDirty, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
        _isQuickAndDirty = isQuickAndDirty;
    }

    public override void Cleanup()
    {
        _quadPrimitive?.Dispose();

        _constantBuffer?.Dispose();
        _indexBuffer?.Dispose();
        _texture3D?.Dispose();
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

        _constantBuffer = graphicsDevice.CreateConstantBuffer(64 * 1024, GraphicsCpuAccess.Write);
        _indexBuffer = graphicsDevice.CreateIndexBuffer(64 * 1024);

        if (_isQuickAndDirty)
        {
            _texture3D = graphicsDevice.CreateTexture3D(GraphicsFormat.R8G8B8A8_UNORM, 64, 64, 64);
        }
        else
        {
            _texture3D = graphicsDevice.CreateTexture3D(GraphicsFormat.R8G8B8A8_UNORM, 256, 256, 256);
        }

        _uploadBuffer = graphicsDevice.CreateUploadBuffer(128 * 1024 * 1024);
        _vertexBuffer = graphicsDevice.CreateVertexBuffer(64 * 1024);

        var copyCommandQueue = graphicsDevice.CopyCommandQueue;
        var copyContext = copyCommandQueue.RentContext();
        {
            copyContext.Reset();
            {
                _quadPrimitive = CreateQuadPrimitive(copyContext);
            }
            copyContext.Close();
            copyContext.Execute();
        }
        copyCommandQueue.ReturnContext(copyContext);

        _uploadBuffer.DisposeAllViews();
    }

    protected override unsafe void Update(TimeSpan delta)
    {
        const float TranslationSpeed = 0.05f;

        var dydz = _texturePosition;
        {
            dydz += (float)(TranslationSpeed * delta.TotalSeconds);
            dydz %= 1.0f;
        }
        _texturePosition = dydz;

        var constantBufferView = _quadPrimitive.PipelineDescriptorSet!.ResourceViews[1].As<GraphicsBufferView>();
        var constantBufferSpan = constantBufferView.Map<Matrix4x4>();
        {
            // Shaders take transposed matrices, so we want to set X.W
            constantBufferSpan[0] = Matrix4x4.Create(
                Vector4.Create(0.5f, 0.0f, 0.0f, 0.5f),      // *0.5f and +0.5f since the input vertex coordinates are in range [-1, 1]  but output texture coordinates needs to be [0, 1]
                Vector4.Create(0.0f, 0.5f, 0.0f, 0.5f - dydz), // *0.5f and +0.5f as above, -dydz to slide the view of the texture vertically each frame
                Vector4.Create(0.0f, 0.0f, 0.5f, dydz / 5.0f), // +dydz to slide the start of the compositing ray in depth each frame
                Vector4.UnitW
            );
        }
        constantBufferView.UnmapAndWrite();
    }

    protected override void Draw(GraphicsRenderContext graphicsRenderContext)
    {
        _quadPrimitive.Draw(graphicsRenderContext);
        base.Draw(graphicsRenderContext);
    }

    private unsafe GraphicsPrimitive CreateQuadPrimitive(GraphicsCopyContext copyContext)
    {
        var renderPass = RenderPass;
        var surface = renderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(renderPass, "Smoke", "main", "main");

        var constantBuffer = _constantBuffer;
        var uploadBuffer = _uploadBuffer;

        return new GraphicsPrimitive(
            graphicsPipeline,
            CreateVertexBufferView(copyContext, _vertexBuffer, uploadBuffer, aspectRatio: surface.PixelWidth / surface.PixelHeight),
            CreateIndexBufferView(copyContext, _indexBuffer, uploadBuffer),
            new GraphicsResourceView[3] {
                CreateConstantBufferView(constantBuffer),
                CreateConstantBufferView(constantBuffer),
                CreateTexture3DView(copyContext, _texture3D, uploadBuffer, _isQuickAndDirty),
            }
        );

        static GraphicsBufferView CreateConstantBufferView(GraphicsBuffer constantBuffer)
        {
            var constantBufferView = constantBuffer.CreateBufferView<Matrix4x4>(1);
            var constantBufferSpan = constantBufferView.Map<Matrix4x4>();
            {
                constantBufferSpan[0] = Matrix4x4.Identity;
            }
            constantBufferView.UnmapAndWrite();
            return constantBufferView;
        }

        static GraphicsBufferView CreateIndexBufferView(GraphicsCopyContext copyContext, GraphicsBuffer indexBuffer, GraphicsBuffer uploadBuffer)
        {
            var uploadBufferView = uploadBuffer.CreateBufferView<ushort>(6);
            var indexBufferSpan = uploadBufferView.Map<ushort>();
            {
                // clockwise when looking at the triangle from the outside

                indexBufferSpan[0] = 0;
                indexBufferSpan[1] = 1;
                indexBufferSpan[2] = 2;

                indexBufferSpan[3] = 0;
                indexBufferSpan[4] = 2;
                indexBufferSpan[5] = 3;
            }
            uploadBufferView.UnmapAndWrite();

            var indexBufferView = indexBuffer.CreateBufferView<ushort>(6);
            copyContext.Copy(indexBufferView, uploadBufferView);
            return indexBufferView;
        }

        static GraphicsTextureView CreateTexture3DView(GraphicsCopyContext copyContext, GraphicsTexture texture3D, GraphicsBuffer uploadBuffer, bool isQuickAndDirty)
        {
            var uploadBufferView = uploadBuffer.CreateBufferView<byte>(checked((uint)texture3D.ByteLength));
            var textureDataSpan = uploadBufferView.Map<byte>();
            {
                var random = new Random(Seed: 20170526);
                var isOnBlurring = true;

                var width = texture3D.PixelWidth;

                var height = texture3D.PixelHeight;
                var bytesPerRow = texture3D.BytesPerRow;

                var depth = texture3D.PixelDepth;
                var bytesPerLayer = texture3D.BytesPerLayer;

                // start with random speckles
                for (var z = 0u; z < depth; z++)
                {
                    var layerIndex = z * bytesPerLayer;

                    for (var y = 0u; y < height; y++)
                    {
                        var rowIndex = layerIndex + (y * bytesPerRow);
                        var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                        for (var x = 0u; x < width; x++)
                        {
                            // convert indices to fractions in the range [0, 1)
                            var fx = (float)x / width;
                            var fz = (float)z / depth;

                            // make x,z relative to texture center
                            fx -= 0.5f;
                            fz -= 0.5f;

                            // get radius from center, clamped to 0.5
                            var radius = MathF.Abs(fx); // MathF.Sqrt(fx * fx + fz * fz);

                            if (radius > 0.5f)
                            {
                                radius = 0.5f;
                            }

                            // scale as 1 in center, tapering off to the edge
                            var scale = 2 * MathF.Abs(0.5f - radius);

                            // random value scaled by the above
                            var rand = random.NextSingle();

                            if (isOnBlurring && (rand < 0.99))
                            {
                                rand = 0;
                            }

                            uint value = (byte)(rand * scale * 255);
                            row[x] = value | (value << 8) | (value << 16) | (value << 24);
                        }
                    }
                }

                if (isOnBlurring)
                {
                    // now smear them out to smooth smoke splotches
                    var falloffFactor = isQuickAndDirty ? 0.9f : 0.95f;

                    for (var z = 0u; z < depth; z++)
                    {
                        var layerIndex = z * bytesPerLayer;

                        for (var y = 0u; y < height; y++)
                        {
                            var rowIndex = layerIndex + (y * bytesPerRow);
                            var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                            for (var x = 1u; x < width; x++)
                            {
                                if ((row[x] & 0xFF) < falloffFactor * (row[x - 1] & 0xFF))
                                {
                                    uint value = (byte)(falloffFactor * (row[x - 1] & 0xFF));
                                    row[x] = value | (value << 8) | (value << 16) | (value << 24);
                                }
                            }

                            for (var x = width - 2; x != uint.MaxValue; x = unchecked(x - 1))
                            {
                                if ((row[x] & 0xFF) < falloffFactor * (row[x + 1] & 0xFF))
                                {
                                    uint value = (byte)(falloffFactor * (row[x + 1] & 0xFF));
                                    row[x] = value | (value << 8) | (value << 16) | (value << 24);
                                }
                            }
                        }
                    }

                    for (var z = 0u; z < depth; z++)
                    {
                        var layerIndex = z * bytesPerLayer;

                        for (var x = 0u; x < width; x++)
                        {
                            for (var y = 1u; y < height; y++)
                            {
                                var rowIndex = layerIndex + (y * bytesPerRow);
                                var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                                var previousRowIndex = rowIndex - bytesPerRow;
                                var previousRow = (uint*)textureDataSpan.GetPointer(previousRowIndex);

                                if ((row[x] & 0xFF) < falloffFactor * (previousRow[x] & 0xFF))
                                {
                                    uint value = (byte)(falloffFactor * (previousRow[x] & 0xFF));
                                    row[x] = value | (value << 8) | (value << 16) | (value << 24);
                                }
                            }

                            for (var y = 0u; y <= 0; y++)
                            {
                                var rowIndex = layerIndex + (y * bytesPerRow);
                                var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                                var previousRowOfNextLayerIndex = rowIndex + bytesPerLayer - bytesPerRow;
                                var previousRowOfNextLayer = (uint*)textureDataSpan.GetPointer(previousRowOfNextLayerIndex);

                                if ((row[x] & 0xFF) < falloffFactor * (previousRowOfNextLayer[x] & 0xFF))
                                {
                                    uint value = (byte)(falloffFactor * (previousRowOfNextLayer[x] & 0xFF));
                                    row[x] = value | (value << 8) | (value << 16) | (value << 24);
                                }
                            }

                            for (var y = 1u; y < height; y++)
                            {
                                var rowIndex = layerIndex + (y * bytesPerRow);
                                var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                                var previousRowIndex = rowIndex - bytesPerRow;
                                var previousRow = (uint*)textureDataSpan.GetPointer(previousRowIndex);

                                if ((row[x] & 0xFF) < falloffFactor * (previousRow[x] & 0xFF))
                                {
                                    uint value = (byte)(falloffFactor * (previousRow[x] & 0xFF));
                                    row[x] = value | (value << 8) | (value << 16) | (value << 24);
                                }
                            }

                            for (var y = height - 2; y != uint.MaxValue; y = unchecked(y - 1))
                            {
                                var rowIndex = layerIndex + (y * bytesPerRow);
                                var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                                var nextRowIndex = rowIndex + bytesPerRow;
                                var nextRow = (uint*)textureDataSpan.GetPointer(nextRowIndex);

                                if ((row[x] & 0xFF) < falloffFactor * (nextRow[x] & 0xFF))
                                {
                                    uint value = (byte)(falloffFactor * (nextRow[x] & 0xFF));
                                    row[x] = value | (value << 8) | (value << 16) | (value << 24);
                                }
                            }

                            for (var y = height - 1; y >= height - 1; y--)
                            {
                                var rowIndex = layerIndex + (y * bytesPerRow);
                                var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                                var nextRowOfPreviousLayerIndex = rowIndex + bytesPerRow - bytesPerLayer;
                                var nextRowOfPreviousLayer = (uint*)textureDataSpan.GetPointer(nextRowOfPreviousLayerIndex);

                                if ((row[x] & 0xFF) < falloffFactor * (nextRowOfPreviousLayer[x] & 0xFF))
                                {
                                    uint value = (byte)(falloffFactor * (nextRowOfPreviousLayer[x] & 0xFF));
                                    row[x] = value | (value << 8) | (value << 16) | (value << 24);
                                }
                            }

                            for (var y = height - 2; y != uint.MaxValue; y = unchecked(y - 1))
                            {
                                var rowIndex = layerIndex + (y * bytesPerRow);
                                var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                                var nextRowIndex = rowIndex + bytesPerRow;
                                var nextRow = (uint*)textureDataSpan.GetPointer(nextRowIndex);

                                if ((row[x] & 0xFF) < falloffFactor * (nextRow[x] & 0xFF))
                                {
                                    uint value = (byte)(falloffFactor * (nextRow[x] & 0xFF));
                                    row[x] = value | (value << 8) | (value << 16) | (value << 24);
                                }
                            }
                        }
                    }

                    for (var y = 0u; y < height; y++)
                    {
                        for (var x = 0u; x < width; x++)
                        {
                            if (x != 0)
                            {
                                for (var z = 1u; z < depth; z++)
                                {
                                    var layerIndex = z * bytesPerLayer;

                                    var rowIndex = layerIndex + (y * bytesPerRow);
                                    var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                                    var sameRowOfPreviousLayerIndex = rowIndex - bytesPerLayer;
                                    var sameRowOfPreviousLayer = (uint*)textureDataSpan.GetPointer(sameRowOfPreviousLayerIndex);

                                    if ((row[x] & 0xFF) < falloffFactor * (sameRowOfPreviousLayer[x] & 0xFF))
                                    {
                                        uint value = (byte)(falloffFactor * (row[x - 1] & 0xFF));
                                        row[x] = value | (value << 8) | (value << 16) | (value << 24);
                                    }
                                }
                            }

                            if (x != (width - 1))
                            {
                                for (var z = depth - 1u; z != uint.MaxValue; z = unchecked(z - 1))
                                {
                                    var layerIndex = z * bytesPerLayer;

                                    var rowIndex = layerIndex + (y * bytesPerRow);
                                    var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                                    if ((row[x] & 0xFF) < falloffFactor * (row[x + bytesPerLayer] & 0xFF))
                                    {
                                        uint value = (byte)(falloffFactor * (row[x + 1] & 0xFF));
                                        row[x] = value | (value << 8) | (value << 16) | (value << 24);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            uploadBufferView.UnmapAndWrite();

            var texture3DView = texture3D.CreateView(0, 1);
            copyContext.Copy(texture3DView, uploadBufferView);
            return texture3DView;
        }

        static GraphicsBufferView CreateVertexBufferView(GraphicsCopyContext copyContext, GraphicsBuffer vertexBuffer, GraphicsBuffer uploadBuffer, float aspectRatio)
        {
            var uploadBufferView = uploadBuffer.CreateBufferView<Texture3DVertex>(4);
            var vertexBufferSpan = uploadBufferView.Map<Texture3DVertex>();
            {
                var y = 1.0f;
                var x = y / aspectRatio;

                vertexBufferSpan[0] = new Texture3DVertex {                //
                    Position = Vector3.Create(-x, y, 0.0f),             //   y          in this setup
                    UVW = Vector3.Create(0, 0, 0),                      //   ^     z    the origin o
                };                                                      //   |   /      is in the middle
                                                                        //   | /        of the rendered scene
                vertexBufferSpan[1] = new Texture3DVertex {                //   o------>x
                    Position = Vector3.Create(x, y, 0.0f),              //
                    UVW = Vector3.Create(1, 0, 0),                      //   0 ----- 1
                };                                                      //   | \     |
                                                                        //   |   \   |
                vertexBufferSpan[2] = new Texture3DVertex {                //   |     \ |
                    Position = Vector3.Create(x, -y, 0.0f),             //   3-------2
                    UVW = Vector3.Create(1, 1, 0),                      //
                };

                vertexBufferSpan[3] = new Texture3DVertex {
                    Position = Vector3.Create(-x, -y, 0),
                    UVW = Vector3.Create(0, 1, 0),
                };
            }
            uploadBufferView.UnmapAndWrite();

            var vertexBufferView = vertexBuffer.CreateBufferView<Texture3DVertex>(4);
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
                    ByteLength = 12,
                    Format = GraphicsFormat.R32G32B32_SFLOAT,
                    Kind = GraphicsPipelineInputKind.TextureCoordinate,
                    ShaderVisibility = GraphicsShaderVisibility.Vertex,
                },
            };

            var resources = new UnmanagedArray<GraphicsPipelineResource>(3) {
                [0] = new GraphicsPipelineResource {
                    BindingIndex = 0,
                    Kind = GraphicsPipelineResourceKind.ConstantBuffer,
                    ShaderVisibility = GraphicsShaderVisibility.Vertex,
                },
                [1] = new GraphicsPipelineResource {
                    BindingIndex = 1,
                    Kind = GraphicsPipelineResourceKind.ConstantBuffer,
                    ShaderVisibility = GraphicsShaderVisibility.Vertex,
                },
                [2] = new GraphicsPipelineResource {
                    BindingIndex = 2,
                    Kind = GraphicsPipelineResourceKind.Texture,
                    ShaderVisibility = GraphicsShaderVisibility.Pixel,
                },
            };

            return graphicsDevice.CreatePipelineSignature(inputs, resources);
        }
    }
}
