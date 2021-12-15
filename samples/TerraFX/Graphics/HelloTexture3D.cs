// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics;

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
    private const uint TEXTURE3D_SIDE_LENGTH = 64; // this results in a RowPitch of 256, the minimum allowed

    public HelloTexture3D(string name, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
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
        var graphicsRenderContext = graphicsDevice.RentRenderContext(); // TODO: This could be a copy only context

        var textureSize = 4u * TEXTURE3D_SIDE_LENGTH * TEXTURE3D_SIDE_LENGTH * TEXTURE3D_SIDE_LENGTH;
        using var vertexStagingBuffer = graphicsDevice.CreateBuffer(GraphicsResourceCpuAccess.Write, GraphicsBufferKind.Default, 64 * 1024);
        using var indexStagingBuffer = graphicsDevice.CreateBuffer(GraphicsResourceCpuAccess.Write, GraphicsBufferKind.Default, 64 * 1024);
        using var textureStagingBuffer = graphicsDevice.CreateBuffer(GraphicsResourceCpuAccess.Write, GraphicsBufferKind.Default, textureSize);

        _constantBuffer = graphicsDevice.CreateBuffer(GraphicsResourceCpuAccess.Write, GraphicsBufferKind.Constant, 64 * 1024);
        _indexBuffer = graphicsDevice.CreateBuffer(GraphicsResourceCpuAccess.None, GraphicsBufferKind.Index, 64 * 1024);
        _vertexBuffer = graphicsDevice.CreateBuffer(GraphicsResourceCpuAccess.None, GraphicsBufferKind.Vertex, 64 * 1024);

        graphicsRenderContext.Reset();
        _quadPrimitive = CreateQuadPrimitive(graphicsRenderContext, vertexStagingBuffer, indexStagingBuffer, textureStagingBuffer);
        graphicsRenderContext.Flush();

        graphicsDevice.WaitForIdle();
        graphicsDevice.ReturnRenderContext(graphicsRenderContext);
    }

    protected override unsafe void Update(TimeSpan delta)
    {
        var graphicsDevice = GraphicsDevice;
        var graphicsSurface = GraphicsRenderPass.Surface;
        var scale255_256 = 255f / 256f;
        var aspectRatio = graphicsSurface.Width / graphicsSurface.Height;
        var scaleX = scale255_256;
        var scaleY = scale255_256 / aspectRatio;
        var scaleZ = scale255_256;

        const float TranslationSpeed = MathF.PI;

        var radians = _texturePosition;
        {
            radians += (float)(TranslationSpeed * delta.TotalSeconds);
            radians %= MathF.Tau;
        }
        _texturePosition = radians;
        var z = scaleZ * (0.5f + (0.5f * MathF.Cos(radians)));

        var constantBufferView = _quadPrimitive.InputResourceViews[1];
        var constantBuffer = _constantBuffer;
        var pConstantBuffer = constantBufferView.Map<Matrix4x4>();

        // Shaders take transposed matrices, so we want to set X.W
        pConstantBuffer[0] = Matrix4x4.Create(
            Vector4.Create(scaleX, 0.0f, 0.0f, 0.5f), // +0.5 since the input coordinates are in range [-.5, .5]  but output needs to be [0, 1]
            Vector4.Create(0.0f, scaleY, 0.0f, 0.5f), // +0.5 since the input coordinates are in range [-.5, .5]  but output needs to be [0, 1]
            Vector4.Create(0.0f, 0.0f, 1.0f, z),
            Vector4.UnitW
        );

        constantBufferView.UnmapAndWrite();
    }

    protected override void Draw(GraphicsRenderContext graphicsRenderContext)
    {
        graphicsRenderContext.Draw(_quadPrimitive);
        base.Draw(graphicsRenderContext);
    }

    private unsafe GraphicsPrimitive CreateQuadPrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, GraphicsBuffer indexStagingBuffer, GraphicsBuffer textureStagingBuffer)
    {
        var graphicsDevice = GraphicsDevice;
        var graphicsRenderPass = GraphicsRenderPass;
        var graphicsSurface = graphicsRenderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(graphicsRenderPass, "Texture3D", "main", "main");

        var constantBuffer = _constantBuffer;
        var indexBuffer = _indexBuffer;
        var vertexBuffer = _vertexBuffer;

        var vertexBufferView = CreateVertexBufferView(graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
        graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

        var indexBufferView = CreateIndexBufferView(graphicsContext, indexBuffer, indexStagingBuffer);
        graphicsContext.Copy(indexBuffer, indexStagingBuffer);

        var inputResourceViews = new GraphicsResourceView[3] {
            CreateConstantBufferView(graphicsContext, constantBuffer, index: 0),
            CreateConstantBufferView(graphicsContext, constantBuffer, index: 1),
            CreateTextureView(graphicsContext, textureStagingBuffer),
        };
        return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferView, indexBufferView, inputResourceViews);

        static GraphicsResourceView CreateConstantBufferView(GraphicsContext graphicsContext, GraphicsBuffer constantBuffer, uint index)
        {
            var constantBufferView = new GraphicsResourceView {
                Offset = 256 * index,
                Resource = constantBuffer,
                Size = SizeOf<Matrix4x4>(),
                Stride = SizeOf<Matrix4x4>(),
            };
            var pConstantBuffer = constantBufferView.Map<Matrix4x4>();

            pConstantBuffer[0] = Matrix4x4.Identity;

            constantBufferView.UnmapAndWrite();
            return constantBufferView;
        }

        static GraphicsResourceView CreateIndexBufferView(GraphicsContext graphicsContext, GraphicsBuffer indexBuffer, GraphicsBuffer indexStagingBuffer)
        {
            var indexBufferView = new GraphicsResourceView {
                Offset = 0,
                Resource = indexBuffer,
                Size = SizeOf<ushort>() * 6,
                Stride = SizeOf<ushort>(),
            };
            var pIndexBuffer = indexStagingBuffer.Map<ushort>(indexBufferView.Offset, indexBufferView.Size);

            // clockwise when looking at the triangle from the outside

            pIndexBuffer[0] = 0;
            pIndexBuffer[1] = 1;
            pIndexBuffer[2] = 2;

            pIndexBuffer[3] = 0;
            pIndexBuffer[4] = 2;
            pIndexBuffer[5] = 3;

            indexStagingBuffer.UnmapAndWrite(indexBufferView.Offset, indexBufferView.Size);
            return indexBufferView;
        }

        static GraphicsResourceView CreateTextureView(GraphicsContext graphicsContext, GraphicsBuffer textureStagingBuffer)
        {
            const uint TextureWidth = TEXTURE3D_SIDE_LENGTH;
            const uint TextureHeight = TEXTURE3D_SIDE_LENGTH;
            const uint TextureDepth = TEXTURE3D_SIDE_LENGTH;
            const uint TextureDz = TextureWidth * TextureHeight;
            const uint TexturePixels = TextureDz * TextureDepth;

            var texture = graphicsContext.Device.CreateTexture(GraphicsResourceCpuAccess.None, GraphicsTextureKind.ThreeDimensional, GraphicsFormat.R8G8B8A8_UNORM, TextureWidth, TextureHeight, (ushort)TextureDepth);
            var textureView = new GraphicsResourceView {
                Offset = 0,
                Resource = texture,
                Size = checked((uint)texture.Size),
                Stride = SizeOf<uint>(),
            };
            var pTextureData = textureStagingBuffer.Map<uint>(textureView.Offset, textureView.Size);

            for (uint n = 0; n < TexturePixels; n++)
            {
                var x = n % TextureWidth;
                var y = n % TextureDz / TextureWidth;
                var z = n / TextureDz;
                x = x * 256 / TextureWidth;
                y = y * 256 / TextureHeight;
                z = z * 256 / TextureDepth;

                pTextureData[n] = 0xFF000000 | (z << 16) | (y << 8) | (x << 0);
            }

            textureStagingBuffer.UnmapAndWrite(textureView.Offset, textureView.Size);
            graphicsContext.Copy(texture, textureStagingBuffer);

            return textureView;
        }

        static GraphicsResourceView CreateVertexBufferView(GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
        {
            var vertexBufferView = new GraphicsResourceView {
                Offset = 0,
                Resource = vertexBuffer,
                Size = SizeOf<Texture3DVertex>() * 4,
                Stride = SizeOf<Texture3DVertex>(),
            };
            var pVertexBuffer = vertexStagingBuffer.Map<Texture3DVertex>(vertexBufferView.Offset, vertexBufferView.Size);

            pVertexBuffer[0] = new Texture3DVertex {             //
                Position = Vector3.Create(-0.5f, 0.5f, 0.0f),       //   y          in this setup
                UVW = Vector3.Create(0, 1, 0.5f),                   //   ^     z    the origin o
            };                                                   //   |   /      is in the middle
                                                                 //   | /        of the rendered scene
            pVertexBuffer[1] = new Texture3DVertex {             //   o------>x
                Position = Vector3.Create(0.5f, 0.5f, 0.0f),        //
                UVW = Vector3.Create(1, 1, 0.5f),                   //   0 ----- 1
            };                                                   //   | \     |
                                                                 //   |   \   |
            pVertexBuffer[2] = new Texture3DVertex {             //   |     \ |
                Position = Vector3.Create(0.5f, -0.5f, 0.0f),       //   3-------2
                UVW = Vector3.Create(1, 0, 0.5f),                   //
            };

            pVertexBuffer[3] = new Texture3DVertex {
                Position = Vector3.Create(-0.5f, -0.5f, 0.0f),
                UVW = Vector3.Create(0, 0, 0.5f),
            };

            vertexStagingBuffer.UnmapAndWrite(vertexBufferView.Offset, vertexBufferView.Size);
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
                    new GraphicsPipelineInputElement[2] {
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.Position, GraphicsFormat.R32G32B32_SFLOAT, size: 12, alignment: 4),
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.TextureCoordinate, GraphicsFormat.R32G32B32_SFLOAT, size: 12, alignment: 4),
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
