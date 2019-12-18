// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using TerraFX.UI;
using TerraFX.Utilities;

namespace TerraFX.Samples.Graphics
{
    public sealed class HelloTriangle : Sample
    {
        private GraphicsDevice _graphicsDevice = null!;
        private GraphicsPrimitive _trianglePrimitive = null!;
        private Window _window = null!;
        private TimeSpan _elapsedTime;

        public HelloTriangle(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void Cleanup()
        {
            _trianglePrimitive?.Dispose();
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

            _graphicsDevice = graphicsAdapter.CreateGraphicsDevice(_window, graphicsContextCount: 2);
            _trianglePrimitive = CreateTrianglePrimitive();
            
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
                var graphicsDevice = _graphicsDevice;
                var graphicsContext = graphicsDevice.GraphicsContexts[graphicsDevice.GraphicsContextIndex];

                var backgroundColor = new ColorRgba(red: 100.0f / 255.0f, green: 149.0f / 255.0f, blue: 237.0f / 255.0f, alpha: 1.0f);
                graphicsContext.BeginFrame(backgroundColor);

                graphicsContext.Draw(_trianglePrimitive);

                graphicsContext.EndFrame();
                graphicsDevice.PresentFrame();
            }
        }

        private unsafe GraphicsPrimitive CreateTrianglePrimitive()
        {
            var graphicsDevice = _graphicsDevice;
            var graphicsSurface = graphicsDevice.GraphicsSurface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Identity", "VSMain", "PSMain");
            var vertexBuffer = CreateVertexBuffer(graphicsDevice, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);

            return graphicsDevice.CreateGraphicsPrimitive(graphicsPipeline, vertexBuffer);

            static GraphicsBuffer CreateVertexBuffer(GraphicsDevice graphicsDevice, float aspectRatio)
            {
                var vertexBuffer = graphicsDevice.CreateGraphicsBuffer(GraphicsBufferKind.Vertex, (ulong)(sizeof(IdentityVertex) * 3), (ulong)sizeof(IdentityVertex));

                ReadOnlySpan<IdentityVertex> vertices = stackalloc IdentityVertex[3] {
                    new IdentityVertex {
                        Position = new Vector3(0.0f, 0.25f * aspectRatio, 0.0f),
                        Color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
                    },
                    new IdentityVertex {
                        Position = new Vector3(0.25f, -0.25f * aspectRatio, 0.0f),
                        Color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new IdentityVertex {
                        Position = new Vector3(-0.25f, -0.25f * aspectRatio, 0.0f),
                        Color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
                    },
                };

                vertexBuffer.Write(MemoryMarshal.AsBytes(vertices));
                return vertexBuffer;
            }

            GraphicsPipeline CreateGraphicsPipeline(GraphicsDevice graphicsDevice, string shaderName, string vertexShaderEntryPoint, string pixelShaderEntryPoint)
            {
                var vertexShader = CompileShader(graphicsDevice, GraphicsShaderKind.Vertex, "Identity", "main");
                var pixelShader = CompileShader(graphicsDevice, GraphicsShaderKind.Pixel, "Identity", "main");

                var inputElements = new GraphicsPipelineInputElement[2] {
                    new GraphicsPipelineInputElement(typeof(Vector3), GraphicsPipelineInputElementKind.Position, size: 12),
                    new GraphicsPipelineInputElement(typeof(Vector4), GraphicsPipelineInputElementKind.Color, size: 16),
                };

                return graphicsDevice.CreateGraphicsPipeline(vertexShader, inputElements, pixelShader);
            }
        }
    }
}
