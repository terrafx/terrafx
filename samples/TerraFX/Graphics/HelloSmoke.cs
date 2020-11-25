// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.Graphics
{
    public sealed class HelloSmoke : HelloWindow
    {
        private readonly bool _isQuickAndDirty;

        private GraphicsPrimitive _quadPrimitive = null!;
        private float _texturePosition;

        public HelloSmoke(string name, bool isQuickAndDirty, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
            _isQuickAndDirty = isQuickAndDirty;
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
            using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, 4 * 256 * 256 * 256);

            currentGraphicsContext.BeginFrame();
            _quadPrimitive = CreateQuadPrimitive(currentGraphicsContext, vertexStagingBuffer, indexStagingBuffer, textureStagingBuffer);
            currentGraphicsContext.EndFrame();

            graphicsDevice.Signal(currentGraphicsContext.Fence);
            graphicsDevice.WaitForIdle();
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

            var constantBuffer = (GraphicsBuffer)_quadPrimitive.InputResources[0];
            var pConstantBuffer = constantBuffer.Map<Matrix4x4>();

            // Shaders take transposed matrices, so we want to set X.W
            pConstantBuffer[0] = new Matrix4x4(
                new Vector4(0.5f, 0.0f, 0.0f, 0.5f),      // *0.5f and +0.5f since the input vertex coordinates are in range [-1, 1]  but output texture coordinates needs to be [0, 1]
                new Vector4(0.0f, 0.5f, 0.0f, 0.5f-dydz), // *0.5f and +0.5f as above, -dydz to slide the view of the texture vertically each frame
                new Vector4(0.0f, 0.0f, 0.5f, dydz/5.0f), // +dydz to slide the start of the compositing ray in depth each frame
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

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Smoke", "main", "main");
            var vertexBuffer = CreateVertexBuffer(graphicsContext, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            var indexBuffer = CreateIndexBuffer(graphicsContext, indexStagingBuffer);

            var inputResources = new GraphicsResource[3] {
                CreateConstantBuffer(graphicsContext),
                CreateConstantBuffer(graphicsContext),
                CreateTexture3D(graphicsContext, textureStagingBuffer, _isQuickAndDirty),
            };
            return graphicsDevice.CreatePrimitive(graphicsPipeline, new GraphicsBufferView(vertexBuffer, vertexBuffer.Size, SizeOf<Texture3DVertex>()), new GraphicsBufferView(indexBuffer, indexBuffer.Size, sizeof(ushort)), inputResources);

            static GraphicsBuffer CreateVertexBuffer(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.None, (ulong)(sizeof(Texture3DVertex) * 4));
                var pVertexBuffer = vertexStagingBuffer.Map<Texture3DVertex>();

                var y = 1.0f;
                var x = y / aspectRatio;
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

            static GraphicsTexture CreateTexture3D(GraphicsContext graphicsContext, GraphicsBuffer textureStagingBuffer, bool isQuickAndDirty)
            {
                const uint TextureWidth = 256;
                const uint TextureHeight = 256;
                const ushort TextureDepth = 256;
                const uint TextureDz = TextureWidth * TextureHeight;
                const uint TexturePixels = TextureDz * TextureDepth;
                const uint TextureSize = TexturePixels * 4;

                var texelFormat = TexelFormat.R8G8B8A8_UNORM;
                var texture3D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.ThreeDimensional, GraphicsResourceCpuAccess.None, TextureWidth, TextureHeight, TextureDepth, texelFormat: texelFormat);
                var pTextureData = textureStagingBuffer.Map<uint>();

                var random = new Random(Seed: 1);

                // start with random speckles
                for (uint n = 0; n < TexturePixels; n++)
                {
                    // convert n to indices
                    float x = n % TextureWidth;
                    float y = n % TextureDz / TextureWidth;
                    float z = n / TextureDz;

                    // convert indices to fractions in the range [0, 1)
                    x /= TextureWidth;
                    y /= TextureHeight;
                    z /= TextureHeight;

                    // make x,z relative to texture center
                    x -= 0.5f;
                    z -= 0.5f;

                    // get radius from center, clamped to 0.5
                    var radius = MathF.Abs(x); // MathF.Sqrt(x * x + z * z);
                    if (radius > 0.5f)
                    {
                        radius = 0.5f;
                    }

                    // scale as 1 in center, tapering off to the edge
                    var scale = 2 * MathF.Abs(0.5f - radius);

                    // random value scaled by the above
                    var rand = (float)random.NextDouble();
                    if (!isQuickAndDirty && (rand < 0.99))
                    {
                        rand = 0;
                    }
                    uint value = (byte)(rand * scale * 255);
                    pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                }

                if (!isQuickAndDirty)
                {
                    // now smear them out to smooth smoke splotches

                    const uint Dy = TextureWidth;
                    const uint Dz = Dy * TextureHeight;

                    for (var z = 0; z < TextureDepth; z++)
                    {
                        for (var y = 0; y < TextureHeight; y++)
                        {
                            for (var x = 1; x < TextureWidth; x++)
                            {
                                var n = x + (y * Dy) + (z * Dz);
                                if ((pTextureData[n] & 0xFF) < 0.9f * (pTextureData[n - 1] & 0xFF))
                                {
                                    uint value = (byte)(0.9f * (pTextureData[n - 1] & 0xFF));
                                    pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                                }
                            }
                            for (var x = (int)TextureWidth - 2; x >= 0; x--)
                            {
                                var n = x + (y * Dy) + (z * Dz);
                                if ((pTextureData[n] & 0xFF) < 0.9f * (pTextureData[n + 1] & 0xFF))
                                {
                                    uint value = (byte)(0.9f * (pTextureData[n + 1] & 0xFF));
                                    pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                                }
                            }
                        }
                    }
                    for (var z = 0; z < TextureDepth; z++)
                    {
                        for (var x = 0; x < TextureWidth; x++)
                        {
                            for (var y = 1; y < TextureHeight; y++)
                            {
                                var n = x + (y * Dy) + (z * Dz);
                                if ((pTextureData[n] & 0xFF) < 0.9f * (pTextureData[n - Dy] & 0xFF))
                                {
                                    uint value = (byte)(0.9f * (pTextureData[n - Dy] & 0xFF));
                                    pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                                }
                            }
                            for (var y = 0; y <= 0; y++)
                            {
                                var n = x + (y * Dy) + (z * Dz);
                                if ((pTextureData[n] & 0xFF) < 0.9f * (pTextureData[n - Dy + Dz] & 0xFF))
                                {
                                    uint value = (byte)(0.9f * (pTextureData[n - Dy + Dz] & 0xFF));
                                    pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                                }
                            }
                            for (var y = 1; y < TextureHeight; y++)
                            {
                                var n = x + (y * Dy) + (z * Dz);
                                if ((pTextureData[n] & 0xFF) < 0.9f * (pTextureData[n - Dy] & 0xFF))
                                {
                                    uint value = (byte)(0.9f * (pTextureData[n - Dy] & 0xFF));
                                    pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                                }
                            }
                            for (var y = (int)TextureHeight - 2; y >= 0; y--)
                            {
                                var n = x + (y * Dy) + (z * Dz);
                                if ((pTextureData[n] & 0xFF) < 0.9f * (pTextureData[n + Dy] & 0xFF))
                                {
                                    uint value = (byte)(0.9f * (pTextureData[n + Dy] & 0xFF));
                                    pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                                }
                            }
                            for (var y = (int)TextureHeight - 1; y >= (int)TextureHeight - 1; y--)
                            {
                                var n = x + (y * Dy) + (z * Dz);
                                if ((pTextureData[n] & 0xFF) < 0.9f * (pTextureData[n + Dy - Dz] & 0xFF))
                                {
                                    uint value = (byte)(0.9f * (pTextureData[n + Dy - Dz] & 0xFF));
                                    pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                                }
                            }
                            for (var y = (int)TextureHeight - 2; y >= 0; y--)
                            {
                                var n = x + (y * Dy) + (z * Dz);
                                if ((pTextureData[n] & 0xFF) < 0.9f * (pTextureData[n + Dy] & 0xFF))
                                {
                                    uint value = (byte)(0.9f * (pTextureData[n + Dy] & 0xFF));
                                    pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                                }
                            }
                        }
                    }
                    for (var y = 0; y < TextureHeight; y++)
                    {
                        for (var x = 0; x < TextureWidth; x++)
                        {
                            for (var z = 1; z < TextureDepth; z++)
                            {
                                var n = x + (y * Dy) + (z * Dz);
                                if ((pTextureData[n] & 0xFF) < 0.9f * (pTextureData[n - Dz] & 0xFF))
                                {
                                    uint value = (byte)(0.9f * (pTextureData[n - 1] & 0xFF));
                                    pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                                }
                            }
                            for (var z = (int)TextureDepth - 0; z >= 0; z--)
                            {
                                var n = x + (y * Dy) + (z * Dz);
                                if ((pTextureData[n] & 0xFF) < 0.9f * (pTextureData[n + Dz] & 0xFF))
                                {
                                    uint value = (byte)(0.9f * (pTextureData[n + 1] & 0xFF));
                                    pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                                }
                            }
                        }
                    }
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
