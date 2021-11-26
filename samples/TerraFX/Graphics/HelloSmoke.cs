// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics;

public sealed class HelloSmoke : HelloWindow
{
    private readonly bool _isQuickAndDirty;

    private GraphicsPrimitive _quadPrimitive = null!;
    private GraphicsBuffer _constantBuffer = null!;
    private GraphicsBuffer _indexBuffer = null!;
    private GraphicsBuffer _vertexBuffer = null!;
    private float _texturePosition;

    public HelloSmoke(string name, bool isQuickAndDirty, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
        _isQuickAndDirty = isQuickAndDirty;
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
        var textureSize = 64 * 1024 * 16 * (_isQuickAndDirty ? 1 : 64);

        using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
        using var indexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
        using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, (ulong)textureSize);

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
        const float TranslationSpeed = 0.05f;

        var dydz = _texturePosition;
        {
            dydz += (float)(TranslationSpeed * delta.TotalSeconds);
            dydz %= 1.0f;
        }
        _texturePosition = dydz;

        var constantBufferRegion = _quadPrimitive.InputResourceRegions[1];
        var constantBuffer = _constantBuffer;
        var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

        // Shaders take transposed matrices, so we want to set X.W
        pConstantBuffer[0] = new Matrix4x4(
            new Vector4(0.5f, 0.0f, 0.0f, 0.5f),      // *0.5f and +0.5f since the input vertex coordinates are in range [-1, 1]  but output texture coordinates needs to be [0, 1]
            new Vector4(0.0f, 0.5f, 0.0f, 0.5f - dydz), // *0.5f and +0.5f as above, -dydz to slide the view of the texture vertically each frame
            new Vector4(0.0f, 0.0f, 0.5f, dydz / 5.0f), // +dydz to slide the start of the compositing ray in depth each frame
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

        var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Smoke", "main", "main");

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
                CreateTexture3DRegion(graphicsContext, textureStagingBuffer, _isQuickAndDirty),
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

        GraphicsMemoryRegion<GraphicsResource> CreateTexture3DRegion(GraphicsContext graphicsContext, GraphicsBuffer textureStagingBuffer, bool isQuickAndDirty)
        {
            var textureWidth = isQuickAndDirty ? 64u : 256u;
            var textureHeight = isQuickAndDirty ? 64u : 256u;
            var textureDepth = isQuickAndDirty ? (ushort)64 : (ushort)256;
            var textureDz = textureWidth * textureHeight;
            var texturePixels = textureDz * textureDepth;

            var texture3D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.ThreeDimensional, GraphicsResourceCpuAccess.None, textureWidth, textureHeight, textureDepth, texelFormat: TexelFormat.R8G8B8A8_UNORM);
            var texture3DRegion = texture3D.Allocate(texture3D.Size, alignment: 4);
            var pTextureData = textureStagingBuffer.Map<uint>(in texture3DRegion);

            var random = new Random(Seed: 1);

            var isOnBlurring = true;
            // start with random speckles
            for (uint n = 0; n < texturePixels; n++)
            {
                // convert n to indices
                float x = n % textureWidth;
                float y = n % textureDz / textureWidth;
                float z = n / textureDz;

                // convert indices to fractions in the range [0, 1)
                x /= textureWidth;
                y /= textureHeight;
                z /= textureHeight;

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
                if (isOnBlurring && (rand < 0.99))
                {
                    rand = 0;
                }
                uint value = (byte)(rand * scale * 255);
                pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
            }

            if (isOnBlurring)
            {
                // now smear them out to smooth smoke splotches

                var dy = textureWidth;
                var dz = dy * textureHeight;
                var falloffFactor = _isQuickAndDirty ? 0.9f : 0.95f;

                for (var z = 0; z < textureDepth; z++)
                {
                    for (var y = 0; y < textureHeight; y++)
                    {
                        for (var x = 1; x < textureWidth; x++)
                        {
                            var n = x + (y * dy) + (z * dz);
                            if ((pTextureData[n] & 0xFF) < falloffFactor * (pTextureData[n - 1] & 0xFF))
                            {
                                uint value = (byte)(falloffFactor * (pTextureData[n - 1] & 0xFF));
                                pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                            }
                        }
                        for (var x = (int)textureWidth - 2; x >= 0; x--)
                        {
                            var n = x + (y * dy) + (z * dz);
                            if ((pTextureData[n] & 0xFF) < falloffFactor * (pTextureData[n + 1] & 0xFF))
                            {
                                uint value = (byte)(falloffFactor * (pTextureData[n + 1] & 0xFF));
                                pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                            }
                        }
                    }
                }
                for (var z = 0; z < textureDepth; z++)
                {
                    for (var x = 0; x < textureWidth; x++)
                    {
                        for (var y = 1; y < textureHeight; y++)
                        {
                            var n = x + (y * dy) + (z * dz);
                            if ((pTextureData[n] & 0xFF) < falloffFactor * (pTextureData[n - dy] & 0xFF))
                            {
                                uint value = (byte)(falloffFactor * (pTextureData[n - dy] & 0xFF));
                                pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                            }
                        }
                        for (var y = 0; y <= 0; y++)
                        {
                            var n = x + (y * dy) + (z * dz);
                            if ((pTextureData[n] & 0xFF) < falloffFactor * (pTextureData[n - dy + dz] & 0xFF))
                            {
                                uint value = (byte)(falloffFactor * (pTextureData[n - dy + dz] & 0xFF));
                                pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                            }
                        }
                        for (var y = 1; y < textureHeight; y++)
                        {
                            var n = x + (y * dy) + (z * dz);
                            if ((pTextureData[n] & 0xFF) < falloffFactor * (pTextureData[n - dy] & 0xFF))
                            {
                                uint value = (byte)(falloffFactor * (pTextureData[n - dy] & 0xFF));
                                pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                            }
                        }
                        for (var y = (int)textureHeight - 2; y >= 0; y--)
                        {
                            var n = x + (y * dy) + (z * dz);
                            if ((pTextureData[n] & 0xFF) < falloffFactor * (pTextureData[n + dy] & 0xFF))
                            {
                                uint value = (byte)(falloffFactor * (pTextureData[n + dy] & 0xFF));
                                pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                            }
                        }
                        for (var y = (int)textureHeight - 1; y >= (int)textureHeight - 1; y--)
                        {
                            var n = x + (y * dy) + (z * dz);
                            if ((pTextureData[n] & 0xFF) < falloffFactor * (pTextureData[n + dy - dz] & 0xFF))
                            {
                                uint value = (byte)(falloffFactor * (pTextureData[n + dy - dz] & 0xFF));
                                pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                            }
                        }
                        for (var y = (int)textureHeight - 2; y >= 0; y--)
                        {
                            var n = x + (y * dy) + (z * dz);
                            if ((pTextureData[n] & 0xFF) < falloffFactor * (pTextureData[n + dy] & 0xFF))
                            {
                                uint value = (byte)(falloffFactor * (pTextureData[n + dy] & 0xFF));
                                pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                            }
                        }
                    }
                }
                for (var y = 0; y < textureHeight; y++)
                {
                    for (var x = 0; x < textureWidth; x++)
                    {
                        for (var z = 1; z < textureDepth; z++)
                        {
                            var n = x + (y * dy) + (z * dz);
                            if ((pTextureData[n] & 0xFF) < falloffFactor * (pTextureData[n - dz] & 0xFF))
                            {
                                uint value = (byte)(falloffFactor * (pTextureData[n - 1] & 0xFF));
                                pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                            }
                        }
                        for (var z = (int)textureDepth - 0; z >= 0; z--)
                        {
                            var n = x + (y * dy) + (z * dz);
                            if ((pTextureData[n] & 0xFF) < falloffFactor * (pTextureData[n + dz] & 0xFF))
                            {
                                uint value = (byte)(falloffFactor * (pTextureData[n + 1] & 0xFF));
                                pTextureData[n] = (uint)(value | (value << 8) | (value << 16) | (value << 24));
                            }
                        }
                    }
                }
            }

            textureStagingBuffer.UnmapAndWrite(in texture3DRegion);
            graphicsContext.Copy(texture3D, textureStagingBuffer);

            return texture3DRegion;
        }

        static GraphicsMemoryRegion<GraphicsResource> CreateVertexBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
        {
            var vertexBufferRegion = vertexBuffer.Allocate(SizeOf<Texture3DVertex>() * 4, alignment: 16);
            var pVertexBuffer = vertexStagingBuffer.Map<Texture3DVertex>(in vertexBufferRegion);

            var y = 1.0f;
            var x = y / aspectRatio;

            pVertexBuffer[0] = new Texture3DVertex {             //
                Position = new Vector3(-x, y, 0.0f),             //   y          in this setup
                UVW = new Vector3(0, 0, 0),                      //   ^     z    the origin o
            };                                                   //   |   /      is in the middle
                                                                 //   | /        of the rendered scene
            pVertexBuffer[1] = new Texture3DVertex {             //   o------>x
                Position = new Vector3(x, y, 0.0f),              //
                UVW = new Vector3(1, 0, 0),                      //   0 ----- 1
            };                                                   //   | \     |
                                                                 //   |   \   |
            pVertexBuffer[2] = new Texture3DVertex {             //   |     \ |
                Position = new Vector3(x, -y, 0.0f),             //   3-------2
                UVW = new Vector3(1, 1, 0),                      //
            };

            pVertexBuffer[3] = new Texture3DVertex {
                Position = new Vector3(-x, -y, 0),
                UVW = new Vector3(0, 1, 0),
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
