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
    public sealed class HelloTexture : Sample
    {
        private GraphicsDevice _graphicsDevice = null!;
        private GraphicsHeap _graphicsDeviceHeap = null!;
        private GraphicsPrimitive _trianglePrimitive = null!;
        private Window _window = null!;
        private TimeSpan _elapsedTime;

        public HelloTexture(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void Cleanup()
        {
            _trianglePrimitive?.Dispose();
            _graphicsDeviceHeap?.Dispose();
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
            _graphicsDeviceHeap = graphicsDevice.CreateGraphicsHeap(64 * 1024 * 5, GraphicsHeapCpuAccess.None);

            using (var graphicsStagingHeap = graphicsDevice.CreateGraphicsHeap(64 * 1024 * 5, GraphicsHeapCpuAccess.Write))
            using (var vertexStagingBuffer = graphicsStagingHeap.CreateGraphicsBuffer(GraphicsBufferKind.Staging, 64 * 1024, sizeof(byte)))
            using (var textureStagingBuffer = graphicsStagingHeap.CreateGraphicsBuffer(GraphicsBufferKind.Staging, 64 * 1024 * 4, sizeof(byte)))
            {
                var currentGraphicsContext = graphicsDevice.CurrentGraphicsContext;
                currentGraphicsContext.BeginFrame();

                _trianglePrimitive = CreateTrianglePrimitive(currentGraphicsContext, vertexStagingBuffer, textureStagingBuffer);

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

        private unsafe GraphicsPrimitive CreateTrianglePrimitive(GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, GraphicsBuffer textureStagingBuffer)
        {
            var graphicsDevice = _graphicsDevice;
            var graphicsSurface = graphicsDevice.GraphicsSurface;

            var graphicsBufferHeap = _graphicsDeviceHeap;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Texture", "main", "main");
            var vertexBuffer = CreateVertexBuffer(graphicsContext, graphicsBufferHeap, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);

            var inputResources = new GraphicsResource[1] {
                CreateTexture2D(graphicsContext, graphicsBufferHeap, textureStagingBuffer),
            };
            return graphicsDevice.CreateGraphicsPrimitive(graphicsPipeline, vertexBuffer, indexBuffer: null, inputResources);

            static GraphicsTexture CreateTexture2D(GraphicsContext graphicsContext, GraphicsHeap graphicsHeap, GraphicsBuffer textureStagingBuffer)
            {
                const uint TextureWidth = 256;
                const uint TextureHeight = 256;
                const uint TextureSize = TextureWidth * TextureHeight;
                const uint CellWidth = TextureWidth / 8;
                const uint CellHeight = TextureHeight / 8;

                var texture2D = graphicsHeap.CreateGraphicsTexture(GraphicsTextureKind.TwoDimensional, TextureWidth, TextureHeight);
                var pTextureData = textureStagingBuffer.Map<uint>();

                for (uint n = 0; n < TextureSize; n++)
                {
                    var x = n % TextureWidth;
                    var y = n / TextureWidth;

                    if ((x / CellWidth % 2) == (y / CellHeight % 2))
                    {
                        pTextureData[n] = 0xFF000000;
                    }
                    else
                    {
                        pTextureData[n] = 0xFFFFFFFF;
                    }
                }

                textureStagingBuffer.Unmap(0..(int)TextureSize);
                graphicsContext.Copy(texture2D, textureStagingBuffer);

                return texture2D;
            }

            static GraphicsBuffer CreateVertexBuffer(GraphicsContext graphicsContext, GraphicsHeap graphicsHeap, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBuffer = graphicsHeap.CreateGraphicsBuffer(GraphicsBufferKind.Vertex, (ulong)(sizeof(TextureVertex) * 3), (ulong)sizeof(TextureVertex));
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

                return graphicsDevice.CreateGraphicsPipeline(signature, vertexShader, pixelShader);
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
        }
    }
}
