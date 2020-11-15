// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Numerics;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.Graphics
{
    public class HelloSierpinski : HelloWindow
    {
        private GraphicsPrimitive _pyramid = null!;
        private float _texturePosition;
        private int _recursionDepth;

        public HelloSierpinski(string name, int recursionDepth, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
            _recursionDepth = recursionDepth;
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

            using (var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, 64 * 1024)) // 2^16, minimum page size
            using (var indexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, 64 * 1024))
            using (var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, 64 * 1024 * 1024)) // 2^26, same as 4 * 256 * 256 * 256
            {
                var currentGraphicsContext = graphicsDevice.CurrentContext;

                currentGraphicsContext.BeginFrame();
                _pyramid = CreateGraphicsPrimitive(currentGraphicsContext, vertexStagingBuffer, indexStagingBuffer, textureStagingBuffer);
                currentGraphicsContext.EndFrame();

                graphicsDevice.Signal(currentGraphicsContext.Fence);
                graphicsDevice.WaitForIdle();
            }
        }

        protected override unsafe void Update(TimeSpan delta)
        {
            //var graphicsDevice = GraphicsDevice;
            //var graphicsSurface = graphicsDevice.Surface;
            //var aspectRatio = graphicsSurface.Width / graphicsSurface.Height;

            const float translationSpeed = 1;

            float radians = _texturePosition;
            {
                radians += (float)(translationSpeed * delta.TotalSeconds);
                radians = radians % (2 * MathF.PI);
            }
            _texturePosition = radians;
            float sin = MathF.Sin(radians);
            float cos = MathF.Cos(radians);

            var constantBuffer = (GraphicsBuffer)_pyramid.InputResources[0];
            var pConstantBuffer = constantBuffer.Map<Matrix4x4>();


            // Shaders take transposed matrices, so we want to mirror along the diagonal
            // rotate around y axis
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

        private unsafe GraphicsPrimitive CreateGraphicsPrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, GraphicsBuffer indexStagingBuffer, GraphicsBuffer textureStagingBuffer)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "PositionTransformUvwFixed", "main", "main");
            (List<Vector3> vertices, List<ushort[]> indices) = SierpinskiPyramid.CreateMesh(_recursionDepth);
            var vertexBuffer = CreateVertexBuffer(vertices, graphicsContext, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            var indexBuffer = CreateIndexBuffer(indices, graphicsContext, indexStagingBuffer);

            var inputResources = new GraphicsResource[3] {
                CreateConstantBuffer(graphicsContext),
                CreateConstantBuffer(graphicsContext),
                CreateTexture3D(graphicsContext, textureStagingBuffer),
            };
            return graphicsDevice.CreatePrimitive(graphicsPipeline, new GraphicsBufferView(vertexBuffer, vertexBuffer.Size, SizeOf<Texture3DVertex>()), new GraphicsBufferView(indexBuffer, indexBuffer.Size, sizeof(ushort)), inputResources);

            static GraphicsBuffer CreateVertexBuffer(List<Vector3> vertices, GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                int size = sizeof(Texture3DVertex) * vertices.Count;
                var vertexBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.None, (ulong)size);
                var pVertexBuffer = vertexStagingBuffer.Map<Texture3DVertex>();

                // assumes the vertices are in a box from (-1,-1,-1) to (1,1,1)
                var offset3D = new Vector3(1, 1, 1); // to move lower left corner to (0,0,0)
                var scale3D = new Vector3(0.5f, 0.5f, 0.5f); // to scale to side length 1
                for (int i = 0; i < vertices.Count; i++)
                {
                    var xyz = vertices[i];                // position
                    var uvw = (xyz + offset3D) * scale3D; // texture coordinate
                    pVertexBuffer[i] = new Texture3DVertex {
                        Position = xyz,
                        UVW = uvw
                    };
                }

                vertexStagingBuffer.Unmap(0..size);
                graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

                return vertexBuffer;
            }

            static GraphicsBuffer CreateIndexBuffer(List<ushort[]> indices, GraphicsContext graphicsContext, GraphicsBuffer indexStagingBuffer)
            {
                int size = sizeof(ushort) * 3 * indices.Count;
                var indexBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Index, GraphicsResourceCpuAccess.None, (ulong)size);
                var pIndexBuffer = indexStagingBuffer.Map<ushort>();

                for (int i = 0; i < indices.Count; i++)
                {
                    for (int j = 0; j < 3; j++)
                        pIndexBuffer[j + 3 * i] = indices[i][j];
                }

                indexStagingBuffer.Unmap(0..size);
                graphicsContext.Copy(indexBuffer, indexStagingBuffer);

                return indexBuffer;
            }

            static GraphicsBuffer CreateConstantBuffer(GraphicsContext graphicsContext)
            {
                var constantBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Constant, GraphicsResourceCpuAccess.Write, 256);

                var pConstantBuffer = constantBuffer.Map<Matrix4x4>();
                pConstantBuffer[0] = Matrix4x4.Identity;
                constantBuffer.Unmap(0..sizeof(Matrix4x4));

                return constantBuffer;
            }

            static GraphicsTexture CreateTexture3D(GraphicsContext graphicsContext, GraphicsBuffer textureStagingBuffer)
            {
                const uint TextureWidth = 256;
                const uint TextureHeight = 256;
                const ushort TextureDepth = 256;
                const uint TextureDz = TextureWidth * TextureHeight;
                const uint TexturePixels = TextureDz * TextureDepth;
                const uint TextureSize = TexturePixels * 4;

                var texture3D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.ThreeDimensional, GraphicsResourceCpuAccess.None
                    , TextureWidth, TextureHeight, TextureDepth);
                var pTextureData = textureStagingBuffer.Map<uint>();

                for (uint n = 0; n < TexturePixels; n++)
                {
                    var x = n % TextureWidth;
                    var y = (n % TextureDz) / TextureWidth;
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
                        new GraphicsPipelineInputElement[2] {
                            new GraphicsPipelineInputElement(typeof(Vector3), GraphicsPipelineInputElementKind.Position, size: 12),
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
