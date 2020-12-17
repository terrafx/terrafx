// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics
{
    /// <summary>
    /// Demonstrates the use of
    /// * Quads (Vertex buffer + Index Buffer, see HelloQuad)
    /// * Texture3D (256x256x256, representing the RGB cube, extension of HelloTexture)
    /// * ConstBuffer (transformation matrix as in HelloConstBuffer, but here to animate the 3D texture coordinates)
    /// Will show a quad cutting through the RGB cube and being animated to move back and forth in texture coordinate space.
    /// </summary>
    public class HelloTexture3D : HelloWindow
    {
        private GraphicsPrimitive _quadPrimitive = null!;
        private GraphicsBuffer _constantBuffer = null!;
        private GraphicsBuffer _indexBuffer = null!;
        private GraphicsBuffer _vertexBuffer = null!;
        private float _texturePosition;

        public HelloTexture3D(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void Cleanup()
        {
            _quadPrimitive?.Dispose();
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

            ulong texture3DSize = sizeof(uint) * 256 * 256 * 256;
            using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, 64 * 1024);
            using var indexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, 64 * 1024);
            using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, texture3DSize);

            currentGraphicsContext.BeginFrame();
            _quadPrimitive = CreateQuadPrimitive(currentGraphicsContext, vertexStagingBuffer, indexStagingBuffer, textureStagingBuffer);
            currentGraphicsContext.EndFrame();

            _constantBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Constant, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
            _indexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Index, GraphicsResourceCpuAccess.GpuOnly, 64 * 1024);
            _vertexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.GpuOnly, 64 * 1024);

            currentGraphicsContext.BeginFrame();
            _quadPrimitive = CreateQuadPrimitive(currentGraphicsContext, vertexStagingBuffer, indexStagingBuffer, textureStagingBuffer);
            currentGraphicsContext.EndFrame();

            graphicsDevice.Signal(currentGraphicsContext.Fence);
            graphicsDevice.WaitForIdle();
        }

        protected override unsafe void Update(TimeSpan delta)
        {
            const float TranslationSpeed = MathF.PI;
            var radians = _texturePosition;
            {
                radians += (float)(TranslationSpeed * delta.TotalSeconds);
                radians %= MathF.Tau;
            }
            _texturePosition = radians;
            var z = 0.5f + (0.5f * MathF.Cos(radians));

            var constantBufferRegion = _quadPrimitive.InputResourceRegions[1];
            var constantBuffer = _constantBuffer;
            var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

            // Shaders take transposed matrices, so we want to set X.W
            pConstantBuffer[0] = new Matrix4x4(
                new Vector4(0.5f, 0.0f, 0.0f, 0.5f), // *0.5f and +0.5f since the input vertex coordinates are in range [-1, 1]  but output texture coordinates needs to be [0, 1]
                new Vector4(0.0f, 0.5f, 0.0f, 0.5f), // *0.5f and +0.5f as above
                new Vector4(0.0f, 0.0f, 0.5f, z),    // z to slide the depth of the sampling plane each frame
                new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            );

            constantBuffer.UnmapAndWrite(in constantBufferRegion);
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

            var constantBuffer = _constantBuffer;
            var indexBuffer = _indexBuffer;
            var vertexBuffer = _vertexBuffer;

            var vertexBufferRegion = CreateVertexBufferRegion(graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

            var indexBufferRegion = CreateIndexBufferRegion(graphicsContext, indexBuffer, indexStagingBuffer);
            graphicsContext.Copy(indexBuffer, indexStagingBuffer);

            var inputResourceRegions = new GraphicsMemoryRegion<GraphicsResource>[3] {
                CreateConstantBufferRegion(graphicsContext, constantBuffer),
                CreateConstantBufferRegion(graphicsContext, constantBuffer),
                CreateTexture3DRegion(graphicsContext, textureStagingBuffer),
            };
            return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferRegion, SizeOf<Texture3DVertex>(), indexBufferRegion, SizeOf<ushort>(), inputResourceRegions);

            static GraphicsMemoryRegion<GraphicsResource> CreateConstantBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer constantBuffer)
            {
                var constantBufferRegion = constantBuffer.Allocate(SizeOf<Matrix4x4>(), alignment: 256);
                var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

                pConstantBuffer[0] = Matrix4x4.Identity;

                constantBuffer.UnmapAndWrite(in constantBufferRegion);
                return constantBufferRegion;
            }

            static GraphicsMemoryRegion<GraphicsResource> CreateIndexBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer indexBuffer, GraphicsBuffer indexStagingBuffer)
            {
                var indexBufferRegion = indexBuffer.Allocate(SizeOf<ushort>() * 6, alignment: 2);
                var pIndexBuffer = indexStagingBuffer.Map<ushort>(in indexBufferRegion);

                // clockwise when looking at the triangle from the outside

                pIndexBuffer[0] = 0;
                pIndexBuffer[1] = 1;
                pIndexBuffer[2] = 2;

                pIndexBuffer[3] = 0;
                pIndexBuffer[4] = 2;
                pIndexBuffer[5] = 3;

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

                    pTextureData[n]
                        = (UInt32)(x << 0)     // r
                        | (UInt32)(y << 8)     // g
                        | (UInt32)(z << 16)    // b
                        | 0xFF00_0000;         // a
                }

                textureStagingBuffer.UnmapAndWrite(in texture3DRegion);
                graphicsContext.Copy(texture3D, textureStagingBuffer);

                return texture3DRegion;
            }

            static GraphicsMemoryRegion<GraphicsResource> CreateVertexBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBufferRegion = vertexBuffer.Allocate(SizeOf<Texture3DVertex>() * 4, alignment: 16);
                var pVertexBuffer = vertexStagingBuffer.Map<Texture3DVertex>(in vertexBufferRegion);

                pVertexBuffer[0] = new Texture3DVertex {             //
                    Position = new Vector3(-0.5f, 0.5f, 0.0f),       //   y          in this setup
                    UVW = new Vector3(0, 1, 0.5f),                   //   ^     z    the origin o
                };                                                   //   |   /      is in the middle
                                                                     //   | /        of the rendered scene
                pVertexBuffer[1] = new Texture3DVertex {             //   o------>x
                    Position = new Vector3(0.5f, 0.5f, 0.0f),        //
                    UVW = new Vector3(1, 1, 0.5f),                   //   0 ----- 1
                };                                                   //   | \     |
                                                                     //   |   \   |
                pVertexBuffer[2] = new Texture3DVertex {             //   |     \ |
                    Position = new Vector3(0.5f, -0.5f, 0.0f),       //   3-------2
                    UVW = new Vector3(1, 0, 0.5f),                   //
                };

                pVertexBuffer[3] = new Texture3DVertex {
                    Position = new Vector3(-0.5f, -0.5f, 0.0f),
                    UVW = new Vector3(0, 0, 0.5f),
                };

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
