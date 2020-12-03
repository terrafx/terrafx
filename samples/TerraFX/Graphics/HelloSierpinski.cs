// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.Graphics
{
    public enum SierpinskiShape
    {
        Pyramid,
        Quad,
    }

    public class HelloSierpinskiPyramid : HelloSierpinski
    {
        public HelloSierpinskiPyramid(string name, int recursionDepth, params Assembly[] compositionAssemblies)
            : base(name, recursionDepth, SierpinskiShape.Pyramid, compositionAssemblies)
        {
        }
    }

    public class HelloSierpinskiQuad : HelloSierpinski
    {
        public HelloSierpinskiQuad(string name, int recursionDepth, params Assembly[] compositionAssemblies)
            : base(name, recursionDepth, SierpinskiShape.Quad, compositionAssemblies)
        {
        }
    }

    public class HelloSierpinski : HelloWindow
    {
        private readonly int _recursionDepth;
        private readonly SierpinskiShape _sierpinskiShape;

        private GraphicsPrimitive _pyramid = null!;
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
            base.Cleanup();
        }

        public override void Initialize(Application application)
        {
            base.Initialize(application);

            var graphicsDevice = GraphicsDevice;
            var currentGraphicsContext = graphicsDevice.CurrentContext;

            var vertices = 2 * 12 * (ulong)MathF.Pow(4, _recursionDepth);
            var vertexBufferSize = vertices * SizeOf<PosNormTex3DVertex>();
            var indexBufferSize = vertices * SizeOf<uint>(); // matches vertices count because vertices are replicated, three unique ones per triangle

            using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, vertexBufferSize);
            using var indexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, indexBufferSize);
            using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, 64 * 1024 * 1024);

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
                radians %= 2 * MathF.PI;
            }
            _texturePosition = radians;
            var sin = MathF.Sin(radians);
            var cos = MathF.Cos(radians);

            var constantBuffer = (IGraphicsBuffer)_pyramid.InputResourceRegions[0].Parent;
            var pConstantBuffer = constantBuffer.Map<Matrix4x4>();

            // Shaders take transposed matrices, so we want to mirror along the diagonal
            pConstantBuffer[0] = new Matrix4x4(
                new Vector4(+cos, 0.0f, -sin, 0.0f),
                new Vector4(0.0f, 1.0f, 0.0f, 0.0f),
                new Vector4(+sin, 0.0f, +cos, 0.0f),
                new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            );

            constantBuffer.Unmap(0..sizeof(Matrix4x4));
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.Draw(_pyramid);
            base.Draw(graphicsContext);
        }

        private unsafe GraphicsPrimitive CreateGraphicsPrimitive(GraphicsContext graphicsContext, IGraphicsBuffer vertexStagingBuffer, IGraphicsBuffer indexStagingBuffer, IGraphicsBuffer textureStagingBuffer)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Sierpinski", "main", "main");

            (var vertices, var indices) = (_sierpinskiShape == SierpinskiShape.Pyramid) ? SierpinskiPyramid.CreateMeshTetrahedron(_recursionDepth) : SierpinskiPyramid.CreateMeshQuad(_recursionDepth);
            var normals = SierpinskiPyramid.MeshNormals(vertices);

            var vertexBuffer = CreateVertexBuffer(vertices, normals, graphicsContext, vertexStagingBuffer);
            var vertexBufferRegion = vertexBuffer.Allocate(vertexBuffer.Size, alignment: 1, stride: SizeOf<PosNormTex3DVertex>());

            var indexBuffer = CreateIndexBuffer(indices, graphicsContext, indexStagingBuffer);
            var indexBufferRegion = indexBuffer.Allocate(indexBuffer.Size, alignment: 1, stride: sizeof(uint));

            var inputResources = new IGraphicsResource[3] {
                CreateConstantBuffer(graphicsContext),
                CreateConstantBuffer(graphicsContext),
                CreateTexture3D(graphicsContext, textureStagingBuffer),
            };

            var inputResourceRegions = new GraphicsMemoryRegion<IGraphicsResource>[3] {
                inputResources[0].Allocate(inputResources[0].Size, alignment: 1, stride: SizeOf<Matrix4x4>()),
                inputResources[1].Allocate(inputResources[1].Size, alignment: 1, stride: SizeOf<Matrix4x4>()),
                inputResources[2].Allocate(inputResources[2].Size, alignment: 1, stride: 1),
            };

            return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferRegion, indexBufferRegion, inputResourceRegions);

            static IGraphicsBuffer CreateVertexBuffer(List<Vector3> vertices, List<Vector3> normals, GraphicsContext graphicsContext, IGraphicsBuffer vertexStagingBuffer)
            {
                var size = sizeof(PosNormTex3DVertex) * vertices.Count;
                var vertexBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.None, (ulong)size);
                var pVertexBuffer = vertexStagingBuffer.Map<PosNormTex3DVertex>();

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

                vertexStagingBuffer.Unmap(0..size);
                graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

                return vertexBuffer;
            }

            static IGraphicsBuffer CreateIndexBuffer(List<uint> indices, GraphicsContext graphicsContext, IGraphicsBuffer indexStagingBuffer)
            {
                var size = sizeof(uint) * indices.Count;
                var indexBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Index, GraphicsResourceCpuAccess.None, (ulong)size);
                var pIndexBuffer = indexStagingBuffer.Map<uint>();

                for (var i = 0; i < indices.Count; i++)
                {
                    pIndexBuffer[i] = indices[i];
                }

                indexStagingBuffer.Unmap(0..size);
                graphicsContext.Copy(indexBuffer, indexStagingBuffer);

                return indexBuffer;
            }

            static IGraphicsBuffer CreateConstantBuffer(GraphicsContext graphicsContext)
            {
                var constantBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Constant, GraphicsResourceCpuAccess.Write, 256);

                var pConstantBuffer = constantBuffer.Map<Matrix4x4>();
                pConstantBuffer[0] = Matrix4x4.Identity;
                constantBuffer.Unmap(0..sizeof(Matrix4x4));

                return constantBuffer;
            }

            static IGraphicsTexture CreateTexture3D(GraphicsContext graphicsContext, IGraphicsBuffer textureStagingBuffer)
            {
                const uint TextureWidth = 256;
                const uint TextureHeight = 256;
                const ushort TextureDepth = 256;
                const uint TextureDz = TextureWidth * TextureHeight;
                const uint TexturePixels = TextureDz * TextureDepth;
                const uint TextureSize = TexturePixels * 4;

                var texture3D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.ThreeDimensional, GraphicsResourceCpuAccess.None, TextureWidth, TextureHeight, TextureDepth);
                var pTextureData = textureStagingBuffer.Map<uint>();

                for (uint n = 0; n < TexturePixels; n++)
                {
                    var x = n % TextureWidth;
                    var y = n % TextureDz / TextureWidth;
                    var z = n / TextureDz;

                    pTextureData[n] = 0xFF000000 | (z << 16) | (y << 8) | (x << 0);
                }
                textureStagingBuffer.Unmap(0..(int)TextureSize);
                graphicsContext.Copy(texture3D, textureStagingBuffer);

                return texture3D;
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
