// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Collections;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics;

public class HelloSierpinski : HelloWindow
{
    private readonly int _recursionDepth;
    private readonly SierpinskiShape _sierpinskiShape;

    private GraphicsBuffer _constantBuffer = null!;
    private GraphicsBuffer _indexBuffer = null!;
    private GraphicsPrimitive _sierpinskiPrimitive = null!;
    private GraphicsTexture _texture3D = null!;
    private GraphicsBuffer _uploadBuffer = null!;
    private GraphicsBuffer _vertexBuffer = null!;
    private float _texturePosition;

    public HelloSierpinski(string name, int recursionDepth, SierpinskiShape shape, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
        _recursionDepth = recursionDepth;
        _sierpinskiShape = shape;
    }

    public override void Cleanup()
    {
        _sierpinskiPrimitive?.Dispose();

        _constantBuffer?.Dispose();
        _indexBuffer?.Dispose();
        _texture3D.Dispose();
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

        var verticeCount = 2 * 12 * (nuint)MathF.Pow(4, _recursionDepth);

        _constantBuffer = graphicsDevice.CreateConstantBuffer(64 * 1024, GraphicsCpuAccess.Write);
        _indexBuffer = graphicsDevice.CreateIndexBuffer(verticeCount * SizeOf<uint>());
        _texture3D = graphicsDevice.CreateTexture3D(GraphicsFormat.R8G8B8A8_UNORM, 256, 256, 256);
        _uploadBuffer = graphicsDevice.CreateUploadBuffer(128 * 1024 * 1024);
        _vertexBuffer = graphicsDevice.CreateVertexBuffer(verticeCount * SizeOf<PosNormTex3DVertex>());

        var copyCommandQueue = graphicsDevice.CopyCommandQueue;
        var copyContext = copyCommandQueue.RentContext();
        {
            copyContext.Reset();
            {
                _sierpinskiPrimitive = CreateSierpinskiPrimitive(copyContext);
            }
            copyContext.Close();
            copyContext.Execute();
        }
        copyCommandQueue.ReturnContext(copyContext);

        _uploadBuffer.DisposeAllViews();
    }

    protected override unsafe void Update(TimeSpan delta)
    {
        const float RotationSpeed = 0.5f;

        var radians = _texturePosition;
        {
            radians += (float)(RotationSpeed * delta.TotalSeconds);
            radians %= MathF.Tau;
        }
        _texturePosition = radians;

        var sin = MathF.Sin(radians);
        var cos = MathF.Cos(radians);

        var constantBufferView = _sierpinskiPrimitive.PipelineDescriptorSet!.ResourceViews[1].As<GraphicsBufferView>();
        var constantBufferSpan = constantBufferView.Map<Matrix4x4>();
        {
            // Shaders take transposed matrices, so we want to mirror along the diagonal
            constantBufferSpan[0] = Matrix4x4.Create(
                Vector4.Create(+cos, 0.0f, -sin, 0.0f),
                Vector4.UnitY,
                Vector4.Create(+sin, 0.0f, +cos, 0.0f),
                Vector4.UnitW
            );
        }
        constantBufferView.UnmapAndWrite();
    }

    protected override void Draw(GraphicsRenderContext graphicsRenderContext)
    {
        _sierpinskiPrimitive.Draw(graphicsRenderContext);
        base.Draw(graphicsRenderContext);
    }

    private unsafe GraphicsPrimitive CreateSierpinskiPrimitive(GraphicsCopyContext copyContext)
    {
        var renderPass = RenderPass;
        var surface = renderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(renderPass, "Sierpinski", "main", "main");

        var constantBuffer = _constantBuffer;
        var uploadBuffer = _uploadBuffer;

        (var vertices, var indices) = (_sierpinskiShape == SierpinskiShape.Pyramid) ? SierpinskiPyramid.CreateMeshTetrahedron(_recursionDepth) : SierpinskiPyramid.CreateMeshQuad(_recursionDepth);
        var normals = SierpinskiPyramid.MeshNormals(in vertices);

        var sierpinskiPrimitive = new GraphicsPrimitive(
            graphicsPipeline,
            CreateVertexBufferView(copyContext, _vertexBuffer, uploadBuffer, in vertices, in normals),
            CreateIndexBufferView(copyContext, _indexBuffer, uploadBuffer, in indices),
            new GraphicsResourceView[3] {
                CreateConstantBufferView(copyContext, constantBuffer),
                CreateConstantBufferView(copyContext, constantBuffer),
                CreateTexture3DView(copyContext, _texture3D, uploadBuffer),
            }
        );

        normals.Dispose();
        indices.Dispose();
        vertices.Dispose();

        return sierpinskiPrimitive;

        static GraphicsBufferView CreateConstantBufferView(GraphicsCopyContext copyContext, GraphicsBuffer constantBuffer)
        {
            var constantBufferView = constantBuffer.CreateBufferView<Matrix4x4>(1);
            var constantBufferSpan = constantBufferView.Map<Matrix4x4>();
            {
                constantBufferSpan[0] = Matrix4x4.Identity;
            }
            constantBufferView.UnmapAndWrite();
            return constantBufferView;
        }

        static GraphicsBufferView CreateIndexBufferView(GraphicsCopyContext copyContext, GraphicsBuffer indexBuffer, GraphicsBuffer uploadBuffer, in UnmanagedValueList<uint> indices)
        {
            var uploadBufferView = uploadBuffer.CreateBufferView<uint>(checked((uint)indices.Count));
            var indexBufferSpan = uploadBufferView.Map<uint>();
            {
                indices.CopyTo(indexBufferSpan);
            }
            uploadBufferView.UnmapAndWrite();

            var indexBufferView = indexBuffer.CreateBufferView<uint>(checked((uint)indices.Count));
            copyContext.Copy(indexBufferView, uploadBufferView);
            return indexBufferView;
        }

        static GraphicsTextureView CreateTexture3DView(GraphicsCopyContext copyContext, GraphicsTexture texture3D, GraphicsBuffer uploadBuffer)
        {
            var uploadBufferView = uploadBuffer.CreateBufferView<byte>(checked((uint)texture3D.ByteLength));
            var textureDataSpan = uploadBufferView.Map<byte>();
            {
                var width = texture3D.PixelWidth;

                var height = texture3D.PixelHeight;
                var bytesPerRow = texture3D.BytesPerRow;

                var depth = texture3D.PixelDepth;
                var bytesPerLayer = texture3D.BytesPerLayer;

                for (var z = 0u; z < depth; z++)
                {
                    var layerIndex = z * bytesPerLayer;

                    for (var y = 0u; y < height; y++)
                    {
                        var rowIndex = layerIndex + (y * bytesPerRow);
                        var row = (uint*)textureDataSpan.GetPointer(rowIndex);

                        for (var x = 0u; x < width; x++)
                        {
                            var red = x % 0xFFu;
                            var blue = y % 0xFFu;
                            var green = z % 0xFFu;
                            var alpha = 0xFFu;

                            row[x] = (alpha << 24) | (green << 16) | (blue << 8) | (red << 0);
                        }
                    }
                }
            }
            uploadBufferView.UnmapAndWrite();

            var texture3DView = texture3D.CreateView(0, 1);
            copyContext.Copy(texture3DView, uploadBufferView);
            return texture3DView;
        }

        static GraphicsBufferView CreateVertexBufferView(GraphicsCopyContext copyContext, GraphicsBuffer vertexBuffer, GraphicsBuffer uploadBuffer, in UnmanagedValueList<Vector3> vertices, in UnmanagedValueList<Vector3> normals)
        {
            var uploadBufferView = uploadBuffer.CreateBufferView<PosNormTex3DVertex>(checked((uint)vertices.Count));
            var vertexBufferSpan = uploadBufferView.Map<PosNormTex3DVertex>();
            {
                // assumes the vertices are in a box from (-1,-1,-1) to (1,1,1)

                var offset3D = Vector3.Create(1, 1, 1); // to move lower left corner to (0,0,0)
                var scale3D = Vector3.Create(0.5f, 0.5f, 0.5f); // to scale to side length 1

                for (nuint i = 0; i < vertices.Count; i++)
                {
                    var xyz = vertices[i];                // position
                    var normal = normals[i];              // normal
                    var uvw = (xyz + offset3D) * scale3D; // texture coordinate

                    vertexBufferSpan[i] = new PosNormTex3DVertex {
                        Position = xyz,
                        Normal = normal,
                        UVW = uvw
                    };
                }
            }
            uploadBufferView.UnmapAndWrite();

            var vertexBufferView = vertexBuffer.CreateBufferView<PosNormTex3DVertex>(checked((uint)vertices.Count));
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
            var inputs = new UnmanagedArray<GraphicsPipelineInput>(3) {
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
                    Kind = GraphicsPipelineInputKind.Normal,
                    ShaderVisibility = GraphicsShaderVisibility.Vertex,
                },
                [2] = new GraphicsPipelineInput {
                    BindingIndex = 2,
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
