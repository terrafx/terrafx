// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.Graphics
{
    /// <summary>
    /// Demonstrates the use of
    /// * Quads (Vertex buffer + Index Buffer, see HelloQuad)
    /// * Texture3D (256x256x256, representing the RGB cube, extension of HelloTexture)
    /// * ConstBuffer (transformation matrix as in HelloConstBuffer, but here to animate the 3D texture coordinates)
    /// Will show a quad cutting through the RGB cube and being animated to move back and forth in texture coordinate space.
    /// </summary>
    public sealed class HelloTexture3D : HelloWindow
    {
        private GraphicsPrimitive _quadPrimitive = null!;
        private float _texturePosition;

        public HelloTexture3D(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void Cleanup()
        {
            _quadPrimitive?.Dispose();
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
                _quadPrimitive = CreateQuadPrimitive(currentGraphicsContext, vertexStagingBuffer, indexStagingBuffer, textureStagingBuffer);
                currentGraphicsContext.EndFrame();

                graphicsDevice.Signal(currentGraphicsContext.Fence);
                graphicsDevice.WaitForIdle();
            }
        }

        protected override unsafe void Update(TimeSpan delta)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;
            var scale255_256 = 255f / 256f;
            var aspectRatio = graphicsSurface.Width / graphicsSurface.Height;
            var scaleY = 1 / aspectRatio * scale255_256;
            var scaleX = scale255_256;
            var scaleZ = scale255_256;

            const float translationSpeed = MathF.PI;

            float radians = _texturePosition;
            {
                radians += (float)(translationSpeed * delta.TotalSeconds);
                radians = radians % (2 * MathF.PI); 
            }
            _texturePosition = radians;
            float z = scaleZ * (0.5f + 0.5f * MathF.Cos(radians));

            var constantBuffer = (GraphicsBuffer)_quadPrimitive.InputResources[0];
            var pConstantBuffer = constantBuffer.Map<Matrix4x4>();


            // Shaders take transposed matrices, so we want to set X.W
            pConstantBuffer[0] = new Matrix4x4(
                new Vector4(scaleX, 0.0f, 0.0f, 0.5f), // +0.5 since the input coordinates are in range [-.5, .5]  but output needs to be [0, 1]
                new Vector4(0.0f, scaleY, 0.0f, 0.5f), // +0.5 since the input coordinates are in range [-.5, .5]  but output needs to be [0, 1]
                new Vector4(0.0f, 0.0f, 1.0f, z),
                new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            );

            constantBuffer.Unmap(0..sizeof(Matrix4x4));
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.Draw(_quadPrimitive);
            base.Draw(graphicsContext);
        }

        private unsafe GraphicsPrimitive CreateQuadPrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, GraphicsBuffer indexStagingBuffer, GraphicsBuffer textureStagingBuffer)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Texture3D", "main", "main");
            var vertexBuffer = CreateVertexBuffer(graphicsContext, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            var indexBuffer = CreateIndexBuffer(graphicsContext, indexStagingBuffer);

            var inputResources = new GraphicsResource[3] {
                CreateConstantBuffer(graphicsContext),
                CreateConstantBuffer(graphicsContext),
                CreateTexture3D(graphicsContext, textureStagingBuffer),
            };
            return graphicsDevice.CreatePrimitive(graphicsPipeline, new GraphicsBufferView(vertexBuffer, vertexBuffer.Size, SizeOf<Texture3DVertex>()), new GraphicsBufferView(indexBuffer, indexBuffer.Size, sizeof(ushort)), inputResources);

            static GraphicsBuffer CreateVertexBuffer(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.None, (ulong)(sizeof(Texture3DVertex) * 4));
                var pVertexBuffer = vertexStagingBuffer.Map<Texture3DVertex>();

                pVertexBuffer[0] = new Texture3DVertex {
                    Position = new Vector3(0.5f, 0.5f * aspectRatio, 0.0f),
                    UVW = new Vector3(1.0f, 0.0f, 0.5f),
                };

                pVertexBuffer[1] = new Texture3DVertex {
                    Position = new Vector3(0.5f, -0.5f * aspectRatio, 0.0f),
                    UVW = new Vector3(0.0f, 1.0f, 0.5f),
                };

                pVertexBuffer[2] = new Texture3DVertex {
                    Position = new Vector3(-0.5f, -0.5f * aspectRatio, 0.0f),
                    UVW = new Vector3(0.0f, 0.0f, 0.5f),
                };

                pVertexBuffer[3] = new Texture3DVertex {
                    Position = new Vector3(-0.5f, 0.5f * aspectRatio, 0.0f),
                    UVW = new Vector3(0.0f, 1.0f, 0.5f),
                };

                vertexStagingBuffer.Unmap(0..(sizeof(Texture3DVertex) * 4));
                graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

                return vertexBuffer;
            }

            static GraphicsBuffer CreateIndexBuffer(GraphicsContext graphicsContext, GraphicsBuffer indexStagingBuffer)
            {
                var indexBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Index, GraphicsResourceCpuAccess.None, sizeof(ushort) * 6);
                var pIndexBuffer = indexStagingBuffer.Map<ushort>();

                pIndexBuffer[0] = 0;
                pIndexBuffer[1] = 1;
                pIndexBuffer[2] = 2;

                pIndexBuffer[3] = 0;
                pIndexBuffer[4] = 2;
                pIndexBuffer[5] = 3;

                indexStagingBuffer.Unmap(0..(sizeof(ushort) * 6));
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

                var texture3D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.ThreeDimensional, GraphicsResourceCpuAccess.None, TextureWidth, TextureHeight, TextureDepth);
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
