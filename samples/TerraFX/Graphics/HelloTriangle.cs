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
    public sealed class HelloTriangle : HelloWindow
    {
        private GraphicsPrimitive _trianglePrimitive = null!;
        private GraphicsBuffer _vertexBuffer = null!;

        public HelloTriangle(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void Cleanup()
        {
            _trianglePrimitive?.Dispose();
            _vertexBuffer?.Dispose();
            base.Cleanup();
        }

        /// <summary>Initializes the GUI for this sample.</summary>
        /// <param name="application">The hosting <see cref="Application" />.</param>
        /// <param name="timeout">The <see cref="TimeSpan" /> after which this sample should stop running.</param>
        /// <param name="windowBounds">The <see cref="Rectangle" /> that defines the initial window bounds.
        /// Note that it is a mix of outer window location and inner client rect size.</param>
        public override void Initialize(Application application, TimeSpan timeout, Rectangle windowBounds = default)
        {
            base.Initialize(application, timeout, windowBounds);

            var graphicsDevice = GraphicsDevice;
            var currentGraphicsContext = graphicsDevice.CurrentContext;

            using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);

            _vertexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.GpuOnly, 64 * 1024);

            currentGraphicsContext.BeginFrame();
            _trianglePrimitive = CreateTrianglePrimitive(currentGraphicsContext, vertexStagingBuffer);
            currentGraphicsContext.EndFrame();

            graphicsDevice.Signal(currentGraphicsContext.Fence);
            graphicsDevice.WaitForIdle();
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.Draw(_trianglePrimitive);
            base.Draw(graphicsContext);
        }

        private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Identity", "main", "main");
            var vertexBuffer = _vertexBuffer;

            var vertexBufferRegion = CreateVertexBufferRegion(graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

            return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferRegion, SizeOf<IdentityVertex>());

            static GraphicsMemoryRegion<GraphicsResource> CreateVertexBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBufferRegion = vertexBuffer.Allocate(SizeOf<IdentityVertex>() * 3, alignment: 16);
                var pVertexBuffer = vertexStagingBuffer.Map<IdentityVertex>(in vertexBufferRegion);

                pVertexBuffer[0] = new IdentityVertex {
                    Position = new Vector3(0.0f, 0.25f * aspectRatio, 0.0f),
                    Color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
                };

                pVertexBuffer[1] = new IdentityVertex {
                    Position = new Vector3(0.25f, -0.25f * aspectRatio, 0.0f),
                    Color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                };

                pVertexBuffer[2] = new IdentityVertex {
                    Position = new Vector3(-0.25f, -0.25f * aspectRatio, 0.0f),
                    Color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
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
                            new GraphicsPipelineInputElement(typeof(Vector4), GraphicsPipelineInputElementKind.Color, size: 16),
                        }
                    ),
                };

                return graphicsDevice.CreatePipelineSignature(inputs);
            }
        }
    }
}
