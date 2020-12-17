// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Graphics.Geometry2D;
using TerraFX.Numerics;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.Graphics
{
    /// <summary>
    /// Demonstrates the use of
    /// * Quads (Vertex buffer + Index Buffer, see HelloQuad)
    /// * Texture1D (4096, representing an RGB lookup table)
    /// * Texture2D (256x256, representing texturing using an image, similar to HelloTexture)
    /// * Texture3D (256x256x256, representing the RGB cube, extension of HelloTexture)
    /// * ConstBuffer (transformation matrix as in HelloConstBuffer, but here to animate the 3D texture coordinates)
    /// Will show a quad cutting through the RGB cube and being animated to move back and forth in texture coordinate space
    /// and having an image blended on top
    /// plus the diagonal blended with the lookup table result.
    /// </summary>
    public sealed class HelloTexture1D2D3D : HelloWindow
    {
        private GraphicsPrimitive _quadPrimitive = null!;
        private GraphicsBuffer _constantBuffer = null!;
        private GraphicsBuffer _vertexBuffer = null!;
        private GraphicsBuffer _indexBuffer = null!;
        private float _texturePosition;

        public HelloTexture1D2D3D(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        /// <summary> Dispose resources as needed. </summary>
        public override void Cleanup()
        {
            _quadPrimitive?.Dispose();
            base.Cleanup();
        }

        public override void Initialize(Application application, TimeSpan timeout)
        {
            base.Initialize(application, timeout);

            var graphicsDevice = GraphicsDevice;
            var currentGraphicsContext = graphicsDevice.CurrentContext;

            ulong vertexBufferSize = 64 * 1024; // 2^16, minimum page size
            ulong indexBufferSize = 64 * 1024; // 2^16, minimum page size
            ulong texture1DSize = 4 * 4096; // that is the max possible size for 1D
            ulong texture2DSize = 4 * 256 * 256;
            ulong texture3DSize = 4 * 256 * 256 * 256;
            var textureBufferSize = texture1DSize + texture2DSize + texture3DSize;
            using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, vertexBufferSize);
            using var indexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, indexBufferSize);
            using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, textureBufferSize);

            _constantBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Constant, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
            _vertexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.GpuOnly, vertexBufferSize);
            _indexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Index, GraphicsResourceCpuAccess.GpuOnly, indexBufferSize);

            currentGraphicsContext.BeginFrame();
            _quadPrimitive = CreateQuadPrimitive(currentGraphicsContext, vertexStagingBuffer, indexStagingBuffer, textureStagingBuffer);
            currentGraphicsContext.EndFrame();

            graphicsDevice.Signal(currentGraphicsContext.Fence);
            graphicsDevice.WaitForIdle();
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.Draw(_quadPrimitive);
            base.Draw(graphicsContext);
        }

        /// <summary>Animate the location where the quad pixel shader is supposed to sample from the 3D texture.</summary>
        /// <param name="delta">The time since the last call to this method.</param>
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

            var constantBufferRegion = _quadPrimitive.InputResourceRegions[0];
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

        private unsafe GraphicsPrimitive CreateQuadPrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, GraphicsBuffer indexStagingBuffer, GraphicsBuffer textureStagingBuffer)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Texture1D2D3D", "main", "main");

            var constantBuffer = _constantBuffer;
            var indexBuffer = _indexBuffer;
            var vertexBuffer = _vertexBuffer;

            var vertexBufferRegion = CreateVertexBufferRegion(graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

            var indexBufferRegion = CreateIndexBufferRegion(graphicsContext, indexBuffer, indexStagingBuffer);
            graphicsContext.Copy(indexBuffer, indexStagingBuffer);

            var inputResourceRegions = new GraphicsMemoryRegion<GraphicsResource>[5] {
                CreateConstantBufferRegion(graphicsContext, constantBuffer),
                CreateConstantBufferRegion(graphicsContext, constantBuffer),
                CreateTexture1DRegion(graphicsContext, textureStagingBuffer),
                CreateTexture2DRegion(graphicsContext, textureStagingBuffer),
                CreateTexture3DRegion(graphicsContext, textureStagingBuffer),
            };
            return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferRegion, SizeOf<Texture3DVertex>(), inputResourceRegions: inputResourceRegions);

            GraphicsMemoryRegion<GraphicsResource> CreateIndexBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer indexBuffer, GraphicsBuffer indexStagingBuffer)
            {
                var indexBufferRegion = indexBuffer.Allocate(SizeOf<ushort>() * 6, alignment: 2);
                var pIndexBuffer = indexStagingBuffer.Map<ushort>(in indexBufferRegion);

                pIndexBuffer[0] = 0;
                pIndexBuffer[1] = 1;
                pIndexBuffer[2] = 2;

                pIndexBuffer[3] = 0;
                pIndexBuffer[4] = 2;
                pIndexBuffer[5] = 3;

                indexStagingBuffer.UnmapAndWrite(in indexBufferRegion);
                return indexBufferRegion;
            }

            GraphicsMemoryRegion<GraphicsResource> CreateVertexBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBufferRegion = vertexBuffer.Allocate(SizeOf<Texture3DVertex>() * 3, alignment: 16);
                var pVertexBuffer = vertexStagingBuffer.Map<Texture3DVertex>(in vertexBufferRegion);

                var y = 1.0f;
                var x = 1.0f;
                var t = 1f;
                pVertexBuffer[0] = new Texture3DVertex {       //  
                    Position = new Vector3(-x, y, 0.0f),       //   y          Vertex position space: 
                    UVW = new Vector3(0, 0, 0),                //   ^     z    the origin o is
                };                                             //   |   /      in the middle
                                                               //   | /        of the rendered scene
                pVertexBuffer[1] = new Texture3DVertex {       //   o------>x  here the range is [-1,1] but x is shortened by aspectRatio
                    Position = new Vector3(x, y, 0.0f),        //  
                    UVW = new Vector3(t, 0, 0),                //   o------>x  Texture coordinate space:
                };                                             //   | \        the origin o is 
                                                               //   |   \      at the top left corner
                pVertexBuffer[2] = new Texture3DVertex {       //   v     z    and at the beginning of the texture memory
                    Position = new Vector3(x, -y, 0.0f),       //   y          here the range is [0,1] for x and y
                    UVW = new Vector3(t, t, 0),                //  
                };                                             //   0 ----- 1  the numbers at the corners 
                                                               //   | \     |  are the indices into the 
                pVertexBuffer[3] = new Texture3DVertex {       //   |   \   |  vertex array
                    Position = new Vector3(-x, -y, 0.0f),      //   |     \ |  
                    UVW = new Vector3(0, t, 0),                //   3-------2  
                };                                             //

                vertexStagingBuffer.UnmapAndWrite(in vertexBufferRegion);
                return vertexBufferRegion;
            }

            static GraphicsMemoryRegion<GraphicsResource> CreateConstantBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer constantBuffer)
            {
                var constantBufferRegion = constantBuffer.Allocate(SizeOf<Matrix4x4>(), alignment: 256);
                var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

                pConstantBuffer[0] = Matrix4x4.Identity;

                constantBuffer.UnmapAndWrite(in constantBufferRegion);
                return constantBufferRegion;
            }

            static GraphicsMemoryRegion<GraphicsResource> CreateTexture1DRegion(GraphicsContext graphicsContext, GraphicsBuffer textureStagingBuffer)
            {
                const uint TextureWidth = 4096;
                const uint TexturePixels = TextureWidth;

                var texture1D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.OneDimensional, GraphicsResourceCpuAccess.None, TextureWidth);
                var texture1DRegion = texture1D.Allocate(texture1D.Size, alignment: 4);
                var pTextureData = textureStagingBuffer.Map<uint>(in texture1DRegion);

                for (uint n = 0; n < TexturePixels; n++)
                {
                    var frac = n / (float)TexturePixels;
                    unchecked
                    {
                        pTextureData[n] = ((uint)(255 * frac) << 0)         // r
                                        | ((uint)(255 * (1 - frac)) << 8)   // g
                                        | ((uint)(255 * frac * frac) << 16) // b
                                        | ((uint)(0xFF << 24));             // a
                    }
                }
                textureStagingBuffer.UnmapAndWrite(in texture1DRegion);
                graphicsContext.Copy(texture1D, textureStagingBuffer);

                return texture1DRegion;
            }

            static GraphicsMemoryRegion<GraphicsResource> CreateTexture2DRegion(GraphicsContext graphicsContext, GraphicsBuffer textureStagingBuffer)
            {
                const uint TextureWidth = 256;
                const uint TextureHeight = 256;
                const uint TexturePixels = TextureWidth * TextureHeight;

                var texture2D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.TwoDimensional, GraphicsResourceCpuAccess.None, TextureWidth, TextureHeight);
                var texture2DRegion = texture2D.Allocate(texture2D.Size, alignment: 4);
                var pTextureData = textureStagingBuffer.Map<uint>(in texture2DRegion);

                for (uint n = 0; n < TexturePixels; n++)
                {
                    // x,y in range [-1,1]
                    var x = (n % TextureWidth * 2.0f / TextureWidth) - 1;
                    var y = (n / TextureWidth * 2.0f / TextureWidth) - 1;

                    var r = MathF.Sqrt((x * x) + (y * y));
                    if (r < 1)
                    {
                        pTextureData[n]
                            = ((uint)(255 * r) << 0)         // r
                            | ((uint)(255 * (1 - r)) << 8)   // g
                            | ((uint)(255 * r * r) << 16)    // b
                            | ((uint)(255 * (1 - r)) << 24); // a
                    }
                }
                textureStagingBuffer.UnmapAndWrite(in texture2DRegion);
                graphicsContext.Copy(texture2D, textureStagingBuffer);

                return texture2DRegion;
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
                        = (uint)(x << 0)     // r
                        | (uint)(y << 8)     // g
                        | (uint)(z << 16)    // b
                        | 0xFF00_0000;         // a
                }
                textureStagingBuffer.UnmapAndWrite(in texture3DRegion);
                graphicsContext.Copy(texture3D, textureStagingBuffer);

                return texture3DRegion;
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

                var resources = new GraphicsPipelineResource[5] {
                    new GraphicsPipelineResource(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                    new GraphicsPipelineResource(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                    new GraphicsPipelineResource(GraphicsPipelineResourceKind.Texture, GraphicsShaderVisibility.Pixel),
                    new GraphicsPipelineResource(GraphicsPipelineResourceKind.Texture, GraphicsShaderVisibility.Pixel),
                    new GraphicsPipelineResource(GraphicsPipelineResourceKind.Texture, GraphicsShaderVisibility.Pixel),
                };

                return graphicsDevice.CreatePipelineSignature(inputs, resources);
            }
        }
    }
}
