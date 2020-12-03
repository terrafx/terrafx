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
    /// Creates a triangle textured with a checkerboard that moves in a circle.
    /// The texturing is according to HelloTriangle.
    /// The moving is via a translation matrix similar to HelloConstBuffer.
    ///   It has the same DX12 setup, but different translation math.
    /// </summary>
    public sealed class HelloTextureTransform : HelloWindow
    {
        private GraphicsPrimitive _trianglePrimitive = null!;
        private IGraphicsBuffer _constantBuffer = null!;
        private IGraphicsBuffer _vertexBuffer = null!;
        private float _trianglePrimitiveTranslationAngle;

        public HelloTextureTransform(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void Cleanup()
        {
            _trianglePrimitive?.Dispose();
            _constantBuffer?.Dispose();
            _vertexBuffer?.Dispose();
            base.Cleanup();
        }

        public override void Initialize(Application application)
        {
            base.Initialize(application);

            var graphicsDevice = GraphicsDevice;
            var currentGraphicsContext = graphicsDevice.CurrentContext;

            using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
            using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024 * 4);

            _constantBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Constant, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
            _vertexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.GpuOnly, 64 * 1024);

            currentGraphicsContext.BeginFrame();
            _trianglePrimitive = CreateTrianglePrimitive(currentGraphicsContext, vertexStagingBuffer, textureStagingBuffer);
            currentGraphicsContext.EndFrame();

            graphicsDevice.Signal(currentGraphicsContext.Fence);
            graphicsDevice.WaitForIdle();
        }

        protected override unsafe void Update(TimeSpan delta)
        {
            const float TranslationSpeed = MathF.PI;

            var radians = _trianglePrimitiveTranslationAngle;
            {
                radians += (float)(TranslationSpeed * delta.TotalSeconds);
                radians %= MathF.Tau;
            }
            _trianglePrimitiveTranslationAngle = radians;

            var x = 0.5f * MathF.Cos(radians);
            var y = 0.5f * MathF.Sin(radians);

            var constantBufferRegion = _trianglePrimitive.InputResourceRegions[1];
            var constantBuffer = _constantBuffer;
            var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

            // Shaders take transposed matrices, so we want to set X.W
            pConstantBuffer[0] = new Matrix4x4(
                new Vector4(1.0f, 0.0f, 0.0f, x),
                new Vector4(0.0f, 1.0f, 0.0f, y),
                new Vector4(0.0f, 0.0f, 1.0f, 0),
                new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            );

            constantBuffer.UnmapAndWrite(in constantBufferRegion);
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.Draw(_trianglePrimitive);
            base.Draw(graphicsContext);
        }

        private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsContext graphicsContext, IGraphicsBuffer vertexStagingBuffer, IGraphicsBuffer textureStagingBuffer)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "TextureTransform", "main", "main");

            var constantBuffer = _constantBuffer;
            var vertexBuffer = _vertexBuffer;

            var vertexBufferRegion = CreateVertexBufferRegion(graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

            var inputResourceRegions = new GraphicsMemoryRegion<IGraphicsResource>[3] {
                CreateConstantBufferRegion(graphicsContext, constantBuffer),
                CreateConstantBufferRegion(graphicsContext, constantBuffer),
                CreateTexture2DRegion(graphicsContext, textureStagingBuffer),
            };
            return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferRegion, indexBufferRegion: default, inputResourceRegions);

            static GraphicsMemoryRegion<IGraphicsResource> CreateConstantBufferRegion(GraphicsContext graphicsContext, IGraphicsBuffer constantBuffer)
            {
                var constantBufferRegion = constantBuffer.Allocate(SizeOf<Matrix4x4>(), alignment: 256, stride: SizeOf<Matrix4x4>());
                var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

                pConstantBuffer[0] = Matrix4x4.Identity;

                constantBuffer.UnmapAndWrite(in constantBufferRegion);
                return constantBufferRegion;
            }

            static GraphicsMemoryRegion<IGraphicsResource> CreateTexture2DRegion(GraphicsContext graphicsContext, IGraphicsBuffer textureStagingBuffer)
            {
                const uint TextureWidth = 256;
                const uint TextureHeight = 256;
                const uint TexturePixels = TextureWidth * TextureHeight;
                const uint CellWidth = TextureWidth / 8;
                const uint CellHeight = TextureHeight / 8;

                var texture2D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.TwoDimensional, GraphicsResourceCpuAccess.None, TextureWidth, TextureHeight);
                var texture2DRegion = texture2D.Allocate(texture2D.Size, alignment: 4, stride: sizeof(uint));
                var pTextureData = textureStagingBuffer.Map<uint>(in texture2DRegion);

                for (uint n = 0; n < TexturePixels; n++)
                {
                    var x = n % TextureWidth;
                    var y = n / TextureWidth;

                    pTextureData[n] = (x / CellWidth % 2) == (y / CellHeight % 2)
                                    ? 0xFF000000 : 0xFFFFFFFF;
                }

                textureStagingBuffer.UnmapAndWrite(in texture2DRegion);
                graphicsContext.Copy(texture2D, textureStagingBuffer);

                return texture2DRegion;
            }

            static GraphicsMemoryRegion<IGraphicsResource> CreateVertexBufferRegion(GraphicsContext graphicsContext, IGraphicsBuffer vertexBuffer, IGraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBufferRegion = vertexBuffer.Allocate(SizeOf<TextureVertex>() * 3, alignment: 16, stride: SizeOf<TextureVertex>());
                var pVertexBuffer = vertexStagingBuffer.Map<TextureVertex>(in vertexBufferRegion);

                pVertexBuffer[0] = new TextureVertex {
                    Position = new Vector3(0.0f, 0.25f * aspectRatio, 0.0f),
                    UV = new Vector2(1.0f, 0.0f)
                };

                pVertexBuffer[1] = new TextureVertex {
                    Position = new Vector3(0.25f, -0.25f * aspectRatio, 0.0f),
                    UV = new Vector2(0.0f, 1.0f)
                };

                pVertexBuffer[2] = new TextureVertex {
                    Position = new Vector3(-0.25f, -0.25f * aspectRatio, 0.0f),
                    UV = new Vector2(0.0f, 0.0f)
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
                            new GraphicsPipelineInputElement(typeof(Vector2), GraphicsPipelineInputElementKind.TextureCoordinate, size: 8),
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
