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
    public class HelloTexture3D : HelloWindow
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
            var currentGraphicsContext = graphicsDevice.CurrentContext;

            using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, 64 * 1024);
            using var indexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, 64 * 1024);
            using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, 64 * 1024 * 1024);

            currentGraphicsContext.BeginFrame();
            _quadPrimitive = CreateQuadPrimitive(currentGraphicsContext, vertexStagingBuffer, indexStagingBuffer, textureStagingBuffer);
            currentGraphicsContext.EndFrame();

            graphicsDevice.Signal(currentGraphicsContext.Fence);
            graphicsDevice.WaitForIdle();
        }

        protected override unsafe void Update(TimeSpan delta)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;
            var scale255_256 = 255f / 256f;
            var aspectRatio = graphicsSurface.Width / graphicsSurface.Height;
            var scaleX = scale255_256;
            var scaleY = scale255_256 / aspectRatio;
            var scaleZ = scale255_256;

            const float TranslationSpeed = MathF.PI;

            var radians = _texturePosition;
            {
                radians += (float)(TranslationSpeed * delta.TotalSeconds);
                radians %= 2 * MathF.PI;
            }
            _texturePosition = radians;
            var z = scaleZ * (0.5f + (0.5f * MathF.Cos(radians)));

            var constantBuffer = (IGraphicsBuffer)_quadPrimitive.InputResourceRegions[0].Parent;
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

        private unsafe GraphicsPrimitive CreateQuadPrimitive(GraphicsContext graphicsContext, IGraphicsBuffer vertexStagingBuffer, IGraphicsBuffer indexStagingBuffer, IGraphicsBuffer textureStagingBuffer)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Texture3D", "main", "main");

            var vertexBuffer = CreateVertexBuffer(graphicsContext, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            var vertexBufferRegion = vertexBuffer.Allocate(vertexBuffer.Size, alignment: 1, stride: SizeOf<Texture3DVertex>());

            var indexBuffer = CreateIndexBuffer(graphicsContext, indexStagingBuffer);
            var indexBufferRegion = indexBuffer.Allocate(indexBuffer.Size, alignment: 1, stride: sizeof(ushort));

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

            static IGraphicsBuffer CreateVertexBuffer(GraphicsContext graphicsContext, IGraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.None, (ulong)(sizeof(Texture3DVertex) * 4));
                var pVertexBuffer = vertexStagingBuffer.Map<Texture3DVertex>();

                var a = new Texture3DVertex {                        //
                    Position = new Vector3(-0.5f, 0.5f, 0.0f),       //   y          in this setup
                    UVW = new Vector3(0, 1, 0.5f),                   //   ^     z    the origin o
                };                                                   //   |   /      is in the middle
                                                                     //   | /        of the rendered scene
                var b = new Texture3DVertex {                        //   o------>x
                    Position = new Vector3(0.5f, 0.5f, 0.0f),        //
                    UVW = new Vector3(1, 1, 0.5f),                   //   a ----- b
                };                                                   //   | \     |
                                                                     //   |   \   |
                var c = new Texture3DVertex {                        //   |     \ |
                    Position = new Vector3(0.5f, -0.5f, 0.0f),       //   d-------c
                    UVW = new Vector3(1, 0, 0.5f),                   //
                };                                                   //   0 ----- 1
                                                                     //   | \     |
                var d = new Texture3DVertex {                        //   |   \   |
                    Position = new Vector3(-0.5f, -0.5f, 0.0f),      //   |     \ |
                    UVW = new Vector3(0, 0, 0.5f),                   //   3-------2
                };                                                   //
                pVertexBuffer[0] = a;
                pVertexBuffer[1] = b;
                pVertexBuffer[2] = c;
                pVertexBuffer[3] = d;

                vertexStagingBuffer.Unmap(0..(sizeof(Texture3DVertex) * 4));
                graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

                return vertexBuffer;
            }

            static IGraphicsBuffer CreateIndexBuffer(GraphicsContext graphicsContext, IGraphicsBuffer indexStagingBuffer)
            {
                var indexBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Index, GraphicsResourceCpuAccess.None, sizeof(ushort) * 6);
                var pIndexBuffer = indexStagingBuffer.Map<ushort>();

                pIndexBuffer[0] = 0; // a clockwise when looking at the triangle from the outside
                pIndexBuffer[1] = 1; // b
                pIndexBuffer[2] = 2; // d

                pIndexBuffer[3] = 0; // b
                pIndexBuffer[4] = 2; // c
                pIndexBuffer[5] = 3; // d

                indexStagingBuffer.Unmap(0..(sizeof(ushort) * 6));
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

                var texture3D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.ThreeDimensional, GraphicsResourceCpuAccess.None
                    , TextureWidth, TextureHeight, TextureDepth);
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
