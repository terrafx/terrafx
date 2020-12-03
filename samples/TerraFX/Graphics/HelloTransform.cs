// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.Graphics
{
    public sealed class HelloTransform : HelloWindow
    {
        private GraphicsPrimitive _trianglePrimitive = null!;
        private float _trianglePrimitiveTranslationX;

        public HelloTransform(string name, params Assembly[] compositionAssemblies)
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

        protected override unsafe void Update(TimeSpan delta)
        {
            const float TranslationSpeed = 1.0f;
            const float OffsetBounds = 1.25f;

            var trianglePrimitiveTranslationX = _trianglePrimitiveTranslationX;
            {
                trianglePrimitiveTranslationX += (float)(TranslationSpeed * delta.TotalSeconds);

                if (trianglePrimitiveTranslationX > OffsetBounds)
                {
                    trianglePrimitiveTranslationX = -OffsetBounds;
                }
            }
            _trianglePrimitiveTranslationX = trianglePrimitiveTranslationX;

            var constantBuffer = (IGraphicsBuffer)_trianglePrimitive.InputResourceRegions[0].Parent;
            var pConstantBuffer = constantBuffer.Map<Matrix4x4>();

            // Shaders take transposed matrices, so we want to set X.W
            pConstantBuffer[0] = Matrix4x4.Identity.WithX(
                new Vector4(1.0f, 0.0f, 0.0f, trianglePrimitiveTranslationX)
            );

            constantBuffer.Unmap(0..sizeof(Matrix4x4));
        }

        private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsContext graphicsContext, IGraphicsBuffer vertexStagingBuffer)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Transform", "main", "main");

            var vertexBuffer = CreateVertexBuffer(graphicsContext, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            var vertexBufferRegion = vertexBuffer.Allocate(vertexBuffer.Size, alignment: 1, stride: SizeOf<IdentityVertex>());

            var inputResources = new IGraphicsResource[2] {
                CreateConstantBuffer(graphicsContext),
                CreateConstantBuffer(graphicsContext),
            };

            var inputResourceRegions = new GraphicsMemoryRegion<IGraphicsResource>[2] {
                inputResources[0].Allocate(inputResources[0].Size, alignment: 1, stride: SizeOf<Matrix4x4>()),
                inputResources[1].Allocate(inputResources[1].Size, alignment: 1, stride: SizeOf<Matrix4x4>()),
            };

            return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferRegion, indexBufferRegion: default, inputResourceRegions);

            static IGraphicsBuffer CreateConstantBuffer(GraphicsContext graphicsContext)
            {
                var constantBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Constant, GraphicsResourceCpuAccess.Write, 256);

                var pConstantBuffer = constantBuffer.Map<Matrix4x4>();
                pConstantBuffer[0] = Matrix4x4.Identity;
                constantBuffer.Unmap(0..sizeof(Matrix4x4));

                return constantBuffer;
            }

            static IGraphicsBuffer CreateVertexBuffer(GraphicsContext graphicsContext, IGraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBuffer = graphicsContext.Device.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.None, (ulong)(sizeof(IdentityVertex) * 3));
                var pVertexBuffer = vertexStagingBuffer.Map<IdentityVertex>();

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

                vertexStagingBuffer.Unmap(0..(sizeof(IdentityVertex) * 3));
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
                            new GraphicsPipelineInputElement(typeof(Vector4), GraphicsPipelineInputElementKind.Color, size: 16),
                        }
                    ),
                };

                var resources = new GraphicsPipelineResource[2] {
                    new GraphicsPipelineResource(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                    new GraphicsPipelineResource(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                };

                return graphicsDevice.CreatePipelineSignature(inputs, resources);
            }
        }
    }
}
