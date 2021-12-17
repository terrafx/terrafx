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

        _constantBuffer = graphicsDevice.CreateConstantBuffer(64 * 1024, GraphicsResourceCpuAccess.Write);
        _indexBuffer = graphicsDevice.CreateIndexBuffer(verticeCount * SizeOf<uint>());
        _texture3D = graphicsDevice.CreateTexture3D(GraphicsFormat.R8G8B8A8_UNORM, 256, 256, 256);
        _uploadBuffer = graphicsDevice.CreateUploadBuffer(128 * 1024 * 1024);
        _vertexBuffer = graphicsDevice.CreateVertexBuffer(verticeCount * SizeOf<PosNormTex3DVertex>());

        var graphicsCopyContext = graphicsDevice.RentCopyContext();
        {
            graphicsCopyContext.Reset();
            _sierpinskiPrimitive = CreateSierpinskiPrimitive(graphicsCopyContext);

            graphicsCopyContext.Flush();
            graphicsDevice.WaitForIdle();
        }
        graphicsDevice.ReturnContext(graphicsCopyContext);

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

        var constantBufferView = _sierpinskiPrimitive.InputResourceViews[1].As<GraphicsBufferView>();
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
        graphicsRenderContext.Draw(_sierpinskiPrimitive);
        base.Draw(graphicsRenderContext);
    }

    private unsafe GraphicsPrimitive CreateSierpinskiPrimitive(GraphicsCopyContext graphicsCopyContext)
    {
        var graphicsRenderPass = GraphicsRenderPass;
        var graphicsSurface = graphicsRenderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(graphicsRenderPass, "Sierpinski", "main", "main");

        var constantBuffer = _constantBuffer;
        var uploadBuffer = _uploadBuffer;

        (var vertices, var indices) = (_sierpinskiShape == SierpinskiShape.Pyramid) ? SierpinskiPyramid.CreateMeshTetrahedron(_recursionDepth) : SierpinskiPyramid.CreateMeshQuad(_recursionDepth);
        var normals = SierpinskiPyramid.MeshNormals(in vertices);

        var sierpinskiPrimitive = GraphicsDevice.CreatePrimitive(
            graphicsPipeline,
            CreateVertexBufferView(graphicsCopyContext, _vertexBuffer, uploadBuffer, in vertices, in normals),
            CreateIndexBufferView(graphicsCopyContext, _indexBuffer, uploadBuffer, in indices),
            new GraphicsResourceView[3] {
                CreateConstantBufferView(graphicsCopyContext, constantBuffer),
                CreateConstantBufferView(graphicsCopyContext, constantBuffer),
                CreateTexture3DView(graphicsCopyContext, _texture3D, uploadBuffer),
            }
        );

        normals.Dispose();
        indices.Dispose();
        vertices.Dispose();

        return sierpinskiPrimitive;

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

        static GraphicsBufferView CreateIndexBufferView(GraphicsCopyContext graphicsCopyContext, GraphicsBuffer indexBuffer, GraphicsBuffer uploadBuffer, in UnmanagedValueList<uint> indices)
        {
            var uploadBufferView = uploadBuffer.CreateView<uint>(checked((uint)indices.Count));
            var indexBufferSpan = uploadBufferView.Map<uint>();
            {
                indices.CopyTo(indexBufferSpan);
            }
            uploadBufferView.UnmapAndWrite();

            var indexBufferView = indexBuffer.CreateView<uint>(checked((uint)indices.Count));
            graphicsCopyContext.Copy(indexBufferView, uploadBufferView);
            return indexBufferView;
        }

        static GraphicsTextureView CreateTexture3DView(GraphicsCopyContext graphicsCopyContext, GraphicsTexture texture3D, GraphicsBuffer uploadBuffer)
        {
            var uploadBufferView = uploadBuffer.CreateView<byte>(checked((uint)texture3D.Size));
            var textureDataSpan = uploadBufferView.Map<byte>();
            {
                var width = texture3D.Width;

                var height = texture3D.Height;
                var rowPitch = texture3D.RowPitch;

                var depth = texture3D.Depth;
                var slicePitch = texture3D.SlicePitch;

                for (var z = 0u; z < depth; z++)
                {
                    var sliceIndex = z * slicePitch;

                    for (var y = 0u; y < height; y++)
                    {
                        var rowIndex = sliceIndex + (y * rowPitch);
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
            graphicsCopyContext.Copy(texture3DView, uploadBufferView);
            return texture3DView;
        }

        static GraphicsBufferView CreateVertexBufferView(GraphicsCopyContext graphicsCopyContext, GraphicsBuffer vertexBuffer, GraphicsBuffer uploadBuffer, in UnmanagedValueList<Vector3> vertices, in UnmanagedValueList<Vector3> normals)
        {
            var uploadBufferView = uploadBuffer.CreateView<PosNormTex3DVertex>(checked((uint)vertices.Count));
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

            var vertexBufferView = vertexBuffer.CreateView<PosNormTex3DVertex>(checked((uint)vertices.Count));
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
                    new GraphicsPipelineInputElement[3] {
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.Position, GraphicsFormat.R32G32B32_SFLOAT, size: 12, alignment: 4),
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.Normal, GraphicsFormat.R32G32B32_SFLOAT, size: 12, alignment: 4),
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.TextureCoordinate, GraphicsFormat.R32G32B32_SFLOAT, size: 12, alignment: 4),
                    }
                ),
            };

            var resources = new GraphicsPipelineResourceInfo[3] {
                new GraphicsPipelineResourceInfo(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                new GraphicsPipelineResourceInfo(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                new GraphicsPipelineResourceInfo(GraphicsPipelineResourceKind.Texture, GraphicsShaderVisibility.Pixel),
            };

            return graphicsDevice.CreatePipelineSignature(inputs, resources);
        }
    }
}
