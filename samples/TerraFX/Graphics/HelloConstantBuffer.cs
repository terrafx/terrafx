// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Linq;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using TerraFX.UI;
using TerraFX.Utilities;

namespace TerraFX.Samples.Graphics
{
    public sealed class HelloConstantBuffer : Sample
    {
        private GraphicsDevice _graphicsDevice = null!;
        private GraphicsHeap _graphicsBufferHeap = null!;
        private GraphicsHeap _graphicsConstantBufferHeap = null!;
        private GraphicsPrimitive _trianglePrimitive = null!;
        private Window _window = null!;

        private TimeSpan _elapsedTime;
        private float _trianglePrimitiveTranslationX;

        public HelloConstantBuffer(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void Cleanup()
        {
            _trianglePrimitive?.Dispose();
            _graphicsConstantBufferHeap?.Dispose();
            _graphicsBufferHeap?.Dispose();
            _graphicsDevice?.Dispose();
            _window?.Dispose();

            base.Cleanup();
        }

        public override void Initialize(Application application)
        {
            ExceptionUtilities.ThrowIfNull(application, nameof(application));

            var windowProvider = application.GetService<WindowProvider>();
            _window = windowProvider.CreateWindow();
            _window.Show();

            var graphicsProvider = application.GetService<GraphicsProvider>();
            var graphicsAdapter = graphicsProvider.GraphicsAdapters.First();

            var graphicsDevice = graphicsAdapter.CreateGraphicsDevice(_window, graphicsContextCount: 2);

            _graphicsDevice = graphicsDevice;
            _graphicsBufferHeap = graphicsDevice.CreateGraphicsHeap(64 * 1024 , GraphicsHeapCpuAccess.None);
            _graphicsConstantBufferHeap = graphicsDevice.CreateGraphicsHeap(64 * 1024 * 2, GraphicsHeapCpuAccess.Write);

            using (var graphicsStagingHeap = graphicsDevice.CreateGraphicsHeap(64 * 1024, GraphicsHeapCpuAccess.Write))
            using (var vertexStagingBuffer = graphicsStagingHeap.CreateGraphicsBuffer(GraphicsBufferKind.Staging, 64 * 1024, sizeof(byte)))
            {
                var currentGraphicsContext = graphicsDevice.CurrentGraphicsContext;
                currentGraphicsContext.BeginFrame();

                _trianglePrimitive = CreateTrianglePrimitive(currentGraphicsContext, vertexStagingBuffer);

                currentGraphicsContext.EndFrame();

                graphicsDevice.Signal(currentGraphicsContext.GraphicsFence);
                graphicsDevice.WaitForIdle();
            }

            base.Initialize(application);
        }

        protected override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
        {
            ExceptionUtilities.ThrowIfNull(sender, nameof(sender));

            _elapsedTime += eventArgs.Delta;

            if (_elapsedTime.TotalSeconds >= 2.5)
            {
                var application = (Application)sender;
                application.RequestExit();
            }

            if (_window.IsVisible)
            {
                var currentGraphicsContext = _graphicsDevice.CurrentGraphicsContext;
                currentGraphicsContext.BeginFrame();

                Update(eventArgs.Delta);
                Render();

                currentGraphicsContext.EndFrame();
                Present();
            }
        }

        private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer)
        {
            var graphicsDevice = _graphicsDevice;
            var graphicsSurface = graphicsDevice.GraphicsSurface;

            var graphicsBufferHeap = _graphicsBufferHeap;
            var graphicsConstantBufferHeap = _graphicsConstantBufferHeap;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Transform", "main", "main");
            var vertexBuffer = CreateVertexBuffer(graphicsContext, graphicsBufferHeap, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);

            var inputBuffers = new GraphicsBuffer[2] {
                CreateConstantBuffer(graphicsConstantBufferHeap),
                CreateConstantBuffer(graphicsConstantBufferHeap),
            };
            return graphicsDevice.CreateGraphicsPrimitive(graphicsPipeline, vertexBuffer, inputBuffers: inputBuffers);

            static GraphicsBuffer CreateConstantBuffer(GraphicsHeap graphicsHeap)
            {
                var constantBuffer = graphicsHeap.CreateGraphicsBuffer(GraphicsBufferKind.Constant, 256, 64);

                var pConstantBuffer = constantBuffer.Map<Matrix4x4>();
                pConstantBuffer[0] = Matrix4x4.Identity;
                constantBuffer.Unmap(0..sizeof(Matrix4x4));

                return constantBuffer;
            }

            static GraphicsBuffer CreateVertexBuffer(GraphicsContext graphicsContext, GraphicsHeap graphicsHeap, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBuffer = graphicsHeap.CreateGraphicsBuffer(GraphicsBufferKind.Vertex, (ulong)(sizeof(IdentityVertex) * 3), (ulong)sizeof(IdentityVertex));
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

                return graphicsDevice.CreateGraphicsPipeline(signature, vertexShader, pixelShader);
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

                return graphicsDevice.CreateGraphicsPipelineSignature(inputs, resources);
            }
        }

        private void Present() => _graphicsDevice.PresentFrame();

        private void Render()
        {
            var graphicsDevice = _graphicsDevice;
            var graphicsContext = graphicsDevice.CurrentGraphicsContext;

            var backgroundColor = new ColorRgba(red: 100.0f / 255.0f, green: 149.0f / 255.0f, blue: 237.0f / 255.0f, alpha: 1.0f);

            graphicsContext.BeginDrawing(backgroundColor);
            graphicsContext.Draw(_trianglePrimitive);
            graphicsContext.EndDrawing();
        }

        private unsafe void Update(TimeSpan delta)
        {
            const float translationSpeed = 1.0f;
            const float offsetBounds = 1.25f;

            var trianglePrimitiveTranslationX = _trianglePrimitiveTranslationX;
            {
                trianglePrimitiveTranslationX += (float)(translationSpeed * delta.TotalSeconds);

                if (trianglePrimitiveTranslationX > offsetBounds)
                {
                    trianglePrimitiveTranslationX = -offsetBounds;
                }
            }
            _trianglePrimitiveTranslationX = trianglePrimitiveTranslationX;

            var constantBuffer = _trianglePrimitive.ConstantBuffers[0];
            var pConstantBuffer = constantBuffer.Map<Matrix4x4>();

            // Shaders take transposed matrices, so we want to set X.W
            pConstantBuffer[0] = Matrix4x4.Identity.WithX(
                new Vector4(1.0f, 0.0f, 0.0f, trianglePrimitiveTranslationX)
            );

            constantBuffer.Unmap(0..sizeof(Matrix4x4));
        }
    }
}
