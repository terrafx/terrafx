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
    public sealed class HelloMirror : HelloWindow
    {
        private GraphicsPrimitive _mirrorPrimitive = null!;
        private GraphicsBuffer _indexBuffer = null!;
        private GraphicsBuffer _vertexBuffer = null!;

        public HelloMirror(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void Cleanup()
        {
            _mirrorPrimitive?.Dispose();
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

            using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
            using var indexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
            using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024 * 4);

            _vertexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.GpuOnly, 64 * 1024);
            _indexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Index, GraphicsResourceCpuAccess.GpuOnly, 64 * 1024);

            currentGraphicsContext.BeginFrame();
            _mirrorPrimitive = CreateMirrorPrimitive(currentGraphicsContext, vertexStagingBuffer, indexStagingBuffer, textureStagingBuffer);
            currentGraphicsContext.EndFrame();

            graphicsDevice.Signal(currentGraphicsContext.Fence);
            graphicsDevice.WaitForIdle();
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.Draw(_mirrorPrimitive);
            base.Draw(graphicsContext);
        }

        private unsafe GraphicsPrimitive CreateMirrorPrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, GraphicsBuffer indexStagingBuffer, GraphicsBuffer textureStagingBuffer)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Texture", "main", "main");

            var vertexBuffer = _vertexBuffer;
            var indexBuffer = _indexBuffer;

            var vertexBufferRegion = CreateVertexBufferRegion(graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

            var indexBufferRegion = CreateIndexBufferRegion(graphicsContext, indexBuffer, indexStagingBuffer);
            graphicsContext.Copy(indexBuffer, indexStagingBuffer);

            var inputResourceRegions = new GraphicsMemoryRegion<GraphicsResource>[1] {
                CreateTexture2DRegion(graphicsContext, textureStagingBuffer)
            };
            return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferRegion, SizeOf<TextureVertex>(), indexBufferRegion, SizeOf<ushort>(), inputResourceRegions: inputResourceRegions);

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

            static GraphicsMemoryRegion<GraphicsResource> CreateTexture2DRegion(GraphicsContext graphicsContext, GraphicsBuffer textureStagingBuffer)
            {
                const uint TextureWidth = 256;
                const uint TextureHeight = 256;
                const uint TexturePixels = TextureWidth * TextureHeight;
                const uint CellWidth = TextureWidth / 8;
                const uint CellHeight = TextureHeight / 8;

                var texture2D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.TwoDimensional, GraphicsResourceCpuAccess.None, TextureWidth, TextureHeight);
                var texture2DRegion = texture2D.Allocate(texture2D.Size, alignment: 4);
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

            static GraphicsMemoryRegion<GraphicsResource> CreateVertexBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBufferRegion = vertexBuffer.Allocate(SizeOf<TextureVertex>() * 4, alignment: 16);
                var pVertexBuffer = vertexStagingBuffer.Map<TextureVertex>(in vertexBufferRegion);

                pVertexBuffer[0] = new TextureVertex {                          //
                    Position = new Vector3(-0.25f, 0.25f * aspectRatio, 0.0f),  //   y          in this setup
                    UV = new Vector2(0.0f, 0.0f),                               //   ^     z    the origin o
                };                                                              //   |   /      is in the middle
                                                                                //   | /        of the rendered scene
                pVertexBuffer[1] = new TextureVertex {                          //   o------>x
                    Position = new Vector3(0.25f, 0.25f * aspectRatio, 0.5f),   //
                    UV = new Vector2(1.0f, 0.0f),                               //   0 ----- 1
                };                                                              //   | \     |
                                                                                //   |   \   |
                pVertexBuffer[2] = new TextureVertex {                          //   |     \ |
                    Position = new Vector3(0.25f, -0.25f * aspectRatio, 0.5f),  //   3-------2
                    UV = new Vector2(1.0f, 1.0f),                               //
                };

                pVertexBuffer[3] = new TextureVertex {
                    Position = new Vector3(-0.25f, -0.25f * aspectRatio, 0.0f),
                    UV = new Vector2(0.0f, 1.0f),
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

                var resources = new GraphicsPipelineResource[1] {
                    new GraphicsPipelineResource(GraphicsPipelineResourceKind.Texture, GraphicsShaderVisibility.Pixel),
                };

                return graphicsDevice.CreatePipelineSignature(inputs, resources);
            }
        }
    }
}
