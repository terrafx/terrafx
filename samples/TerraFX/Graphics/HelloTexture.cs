// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.Graphics
{
    public sealed class HelloTexture : HelloWindow
    {
        private GraphicsPrimitive _trianglePrimitive = null!;

        public HelloTexture(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void Cleanup()
        {
            _trianglePrimitive?.Dispose();
            base.Cleanup();
        }

        public override void Initialize(Application application)
        {
            base.Initialize(application);

            var graphicsDevice = GraphicsDevice;
            var currentGraphicsContext = graphicsDevice.CurrentContext;

            using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, 64 * 1024);
            using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.Write, 64 * 1024 * 4);

            currentGraphicsContext.BeginFrame();
            _trianglePrimitive = CreateTrianglePrimitive(currentGraphicsContext, vertexStagingBuffer, textureStagingBuffer);
            currentGraphicsContext.EndFrame();

            graphicsDevice.Signal(currentGraphicsContext.Fence);
            graphicsDevice.WaitForIdle();
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

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Texture", "main", "main");

            var vertexBuffer = CreateVertexBuffer(graphicsContext, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            var vertexBufferRegion = vertexBuffer.Allocate(vertexBuffer.Size, alignment: 1, stride: SizeOf<TextureVertex>());

            var inputResources = new IGraphicsResource[1] {
                CreateTexture2D(graphicsContext, textureStagingBuffer),
            };

            var inputResourceRegions = new GraphicsMemoryRegion<IGraphicsResource>[1] {
                inputResources[0].Allocate(inputResources[0].Size, alignment: 1, stride: 1),
            };

            return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferRegion, indexBufferRegion: default, inputResourceRegions);

            static IGraphicsTexture CreateTexture2D(GraphicsContext graphicsContext, IGraphicsBuffer textureStagingBuffer)
            {
                const uint TextureWidth = 256;
                const uint TextureHeight = 256;
                const uint TexturePixels = TextureWidth * TextureHeight;
                const uint TextureSize = TexturePixels * 4;
                const uint CellWidth = TextureWidth / 8;
                const uint CellHeight = TextureHeight / 8;

                var texture2D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.TwoDimensional, GraphicsResourceCpuAccess.None, TextureWidth, TextureHeight);
                var pTextureData = textureStagingBuffer.Map<uint>();

                for (uint n = 0; n < TexturePixels; n++)
                {
                    var x = n % TextureWidth;
                    var y = n / TextureWidth;

                    pTextureData[n] = (x / CellWidth % 2) == (y / CellHeight % 2)
                                    ? 0xFF000000 : 0xFFFFFFFF;
                }

                textureStagingBuffer.Unmap(0..(int)TextureSize);
                graphicsContext.Copy(texture2D, textureStagingBuffer);

                return texture2D;
            }

            static IGraphicsBuffer CreateVertexBuffer(GraphicsContext graphicsContext, IGraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.None, (ulong)(sizeof(TextureVertex) * 3));
                var pVertexBuffer = vertexStagingBuffer.Map<TextureVertex>();

                pVertexBuffer[0] = new TextureVertex {
                    Position = new Vector3(0.0f, 0.25f * aspectRatio, 0.0f),
                    UV = new Vector2(0.5f, 1.0f),
                };

                pVertexBuffer[1] = new TextureVertex {
                    Position = new Vector3(0.25f, -0.25f * aspectRatio, 0.0f),
                    UV = new Vector2(1.0f, 0.0f),
                };

                pVertexBuffer[2] = new TextureVertex {
                    Position = new Vector3(-0.25f, -0.25f * aspectRatio, 0.0f),
                    UV = new Vector2(0.0f, 0.0f),
                };

                vertexStagingBuffer.Unmap(0..(sizeof(TextureVertex) * 3));
                graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

                return vertexBuffer;
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

                var resources = new GraphicsPipelineResource[1] {
                    new GraphicsPipelineResource(GraphicsPipelineResourceKind.Texture, GraphicsShaderVisibility.Pixel),
                };

                return graphicsDevice.CreatePipelineSignature(inputs, resources);
            }
        }
    }
}
