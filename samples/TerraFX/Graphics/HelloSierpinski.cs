// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics
{
    public class HelloSierpinski : HelloWindow
    {
        private readonly int _recursionDepth;
        private readonly SierpinskiShape _sierpinskiShape;

        private GraphicsPrimitive _pyramid = null!;
        private GraphicsBuffer _constantBuffer = null!;
        private GraphicsBuffer _indexBuffer = null!;
        private GraphicsBuffer _vertexBuffer = null!;
        private float _texturePosition;

        public HelloSierpinski(string name, int recursionDepth, SierpinskiShape shape, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
            _recursionDepth = recursionDepth;
            _sierpinskiShape = shape;
        }

        public override void Cleanup()
        {
            _pyramid?.Dispose();
            _constantBuffer?.Dispose();
            _indexBuffer?.Dispose();
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
            var currentGraphicsContext = graphicsDevice.CurrentContext;

            var vertices = 2 * 12 * (ulong)MathF.Pow(4, _recursionDepth);
            var vertexBufferSize = vertices * SizeOf<PosNormTex3DVertex>();
            var indexBufferSize = vertices * SizeOf<uint>(); // matches vertices count because vertices are replicated, three unique ones per triangle

            using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, vertexBufferSize);
            using var indexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, indexBufferSize);
            using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024 * 1024);

            _constantBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Constant, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
            _indexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Index, GraphicsResourceCpuAccess.GpuOnly, indexBufferSize);
            _vertexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.GpuOnly, vertexBufferSize);

            currentGraphicsContext.BeginFrame();
            _pyramid = CreateGraphicsPrimitive(currentGraphicsContext, vertexStagingBuffer, indexStagingBuffer, textureStagingBuffer);
            currentGraphicsContext.EndFrame();

            graphicsDevice.Signal(currentGraphicsContext.Fence);
            graphicsDevice.WaitForIdle();
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

            var constantBufferRegion = _pyramid.InputResourceRegions[1];
            var constantBuffer = _constantBuffer;
            var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

            // Shaders take transposed matrices, so we want to mirror along the diagonal
            pConstantBuffer[0] = new Matrix4x4(
                new Vector4(+cos, 0.0f, -sin, 0.0f),
                new Vector4(0.0f, 1.0f, 0.0f, 0.0f),
                new Vector4(+sin, 0.0f, +cos, 0.0f),
                new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            );

            constantBuffer.UnmapAndWrite(in constantBufferRegion);
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.Draw(_pyramid);
            base.Draw(graphicsContext);
        }

        private unsafe GraphicsPrimitive CreateGraphicsPrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, GraphicsBuffer indexStagingBuffer, GraphicsBuffer textureStagingBuffer)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Sierpinski", "main", "main");

            var constantBuffer = _constantBuffer;
            var indexBuffer = _indexBuffer;
            var vertexBuffer = _vertexBuffer;

            (var vertices, var indices) = (_sierpinskiShape == SierpinskiShape.Pyramid) ? SierpinskiPyramid.CreateMeshTetrahedron(_recursionDepth) : SierpinskiPyramid.CreateMeshQuad(_recursionDepth);
            var normals = SierpinskiPyramid.MeshNormals(vertices);

            var vertexBufferRegion = CreateVertexBufferRegion(graphicsContext, vertexBuffer, vertexStagingBuffer, vertices, normals);
            graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

            var indexBufferRegion = CreateIndexBufferRegion(graphicsContext, indexBuffer, indexStagingBuffer, indices);
            graphicsContext.Copy(indexBuffer, indexStagingBuffer);

            var inputResourceRegions = new GraphicsMemoryRegion<GraphicsResource>[3] {
                CreateConstantBufferRegion(graphicsContext, constantBuffer),
                CreateConstantBufferRegion(graphicsContext, constantBuffer),
                CreateTexture3DRegion(graphicsContext, textureStagingBuffer),
            };
            return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferRegion, SizeOf<PosNormTex3DVertex>(), indexBufferRegion, SizeOf<uint>(), inputResourceRegions);

            static GraphicsMemoryRegion<GraphicsResource> CreateConstantBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer constantBuffer)
            {
                var constantBufferRegion = constantBuffer.Allocate(SizeOf<Matrix4x4>(), alignment: 256);
                var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

                pConstantBuffer[0] = Matrix4x4.Identity;

                constantBuffer.UnmapAndWrite(in constantBufferRegion);
                return constantBufferRegion;
            }

            static GraphicsMemoryRegion<GraphicsResource> CreateIndexBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer indexBuffer, GraphicsBuffer indexStagingBuffer, List<uint> indices)
            {
                var indexBufferRegion = indexBuffer.Allocate(SizeOf<uint>() * (uint)indices.Count, alignment: 4);
                var pIndexBuffer = indexStagingBuffer.Map<uint>(in indexBufferRegion);

                for (var i = 0; i < indices.Count; i++)
                {
                    pIndexBuffer[i] = indices[i];
                }

                indexStagingBuffer.UnmapAndWrite(in indexBufferRegion);
                return indexBufferRegion;
            }

            static GraphicsMemoryRegion<GraphicsResource> CreateTexture3DRegion(GraphicsContext graphicsContext, GraphicsBuffer textureStagingBuffer)
            {
                const uint TextureWidth = 256;
                const uint TextureHeight = 256;
                const ushort TextureDepth = 256;
                const uint TextureDz = TextureWidth * TextureHeight;
                const uint TexturePixels = TextureDz * TextureDepth;

                var texture3D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.ThreeDimensional, GraphicsResourceCpuAccess.None, TextureWidth, TextureHeight, TextureDepth);
                var texture3DRegion = texture3D.Allocate(texture3D.Size, alignment: 4);
                var pTextureData = textureStagingBuffer.Map<uint>(in texture3DRegion);

                for (uint n = 0; n < TexturePixels; n++)
                {
                    var x = n % TextureWidth;
                    var y = n % TextureDz / TextureWidth;
                    var z = n / TextureDz;
                    {
                        pTextureData[n] = (uint)(x << 0)      // r
                                        | (uint)(y << 8)      // g
                                        | (uint)(z << 16)     // b
                                        | (uint)(0xFFu << 24);// a
                    }
                }

                textureStagingBuffer.UnmapAndWrite(in texture3DRegion);
                graphicsContext.Copy(texture3D, textureStagingBuffer);

                return texture3DRegion;
            }

            static GraphicsMemoryRegion<GraphicsResource> CreateVertexBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer vertexStagingBuffer, List<Vector3> vertices, List<Vector3> normals)
            {
                var vertexBufferRegion = vertexBuffer.Allocate(SizeOf<PosNormTex3DVertex>() * (uint)vertices.Count, alignment: 16);
                var pVertexBuffer = vertexStagingBuffer.Map<PosNormTex3DVertex>(in vertexBufferRegion);

                // assumes the vertices are in a box from (-1,-1,-1) to (1,1,1)

                var offset3D = new Vector3(1, 1, 1); // to move lower left corner to (0,0,0)
                var scale3D = new Vector3(0.5f, 0.5f, 0.5f); // to scale to side length 1

                for (var i = 0; i < vertices.Count; i++)
                {
                    var xyz = vertices[i];                // position
                    var normal = normals[i];              // normal
                    var uvw = (xyz + offset3D) * scale3D; // texture coordinate

                    pVertexBuffer[i] = new PosNormTex3DVertex {
                        Position = xyz,
                        Normal = normal,
                        UVW = uvw
                    };
                }

                vertexStagingBuffer.UnmapAndWrite(in vertexBufferRegion);
                return vertexBufferRegion;
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
                        new GraphicsPipelineInputElement[3] {
                            new GraphicsPipelineInputElement(typeof(Vector3), GraphicsPipelineInputElementKind.Position, size: 12),
                            new GraphicsPipelineInputElement(typeof(Vector3), GraphicsPipelineInputElementKind.Normal, size: 12),
                            new GraphicsPipelineInputElement(typeof(Vector3), GraphicsPipelineInputElementKind.TextureCoordinate, size: 12),
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
}
