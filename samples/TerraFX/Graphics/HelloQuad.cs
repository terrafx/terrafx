// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;

namespace TerraFX.Samples.Graphics;

public sealed class HelloQuad : HelloWindow
{
    private GraphicsBuffer _indexBuffer = null!;
    private GraphicsPrimitive _quadPrimitive = null!;
    private GraphicsBuffer _uploadBuffer = null!;
    private GraphicsBuffer _vertexBuffer = null!;

    public HelloQuad(string name, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
    }

    public override void Cleanup()
    {
        _quadPrimitive?.Dispose();

        _indexBuffer?.Dispose();
        _uploadBuffer?.Dispose();
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

        _indexBuffer = graphicsDevice.CreateIndexBuffer(64 * 1024);
        _uploadBuffer = graphicsDevice.CreateUploadBuffer(64 * 1024);
        _vertexBuffer = graphicsDevice.CreateVertexBuffer(64 * 1024);

        graphicsRenderContext.Reset();
        _quadPrimitive = CreateQuadPrimitive(graphicsRenderContext);
        graphicsRenderContext.Flush();

        graphicsDevice.WaitForIdle();
        graphicsDevice.ReturnRenderContext(graphicsRenderContext);

        // _uploadBuffer.DisposeAllViews();
    }

    protected override void Draw(GraphicsRenderContext graphicsRenderContext)
    {
        graphicsRenderContext.Draw(_quadPrimitive);
        base.Draw(graphicsRenderContext);
    }

    private unsafe GraphicsPrimitive CreateQuadPrimitive(GraphicsContext graphicsContext)
    {
        var graphicsRenderPass = GraphicsRenderPass;
        var graphicsSurface = graphicsRenderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(graphicsRenderPass, "Identity", "main", "main");
        var uploadBuffer = _uploadBuffer;

        return GraphicsDevice.CreatePrimitive(
            graphicsPipeline,
            CreateVertexBufferView(graphicsContext, _vertexBuffer, uploadBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height),
            CreateIndexBufferView(graphicsContext, _indexBuffer, uploadBuffer)
        );

        static GraphicsBufferView CreateIndexBufferView(GraphicsContext graphicsContext, GraphicsBuffer indexBuffer, GraphicsBuffer uploadBuffer)
        {
            var uploadBufferView = uploadBuffer.CreateView<ushort>(6);
            var indexBufferSpan = uploadBufferView.Map<ushort>();
            {
                // clockwise when looking at the triangle from the outside

                indexBufferSpan[0] = 0;
                indexBufferSpan[1] = 1;
                indexBufferSpan[2] = 2;

                indexBufferSpan[3] = 0;
                indexBufferSpan[4] = 2;
                indexBufferSpan[5] = 3;
            }
            uploadBufferView.UnmapAndWrite();

            var indexBufferView = indexBuffer.CreateView<ushort>(6);
            graphicsContext.Copy(indexBufferView, uploadBufferView);
            return indexBufferView;
        }

        static GraphicsBufferView CreateVertexBufferView(GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer uploadBuffer, float aspectRatio)
        {
            var uploadBufferView = uploadBuffer.CreateView<IdentityVertex>(4);
            var vertexBufferSpan = uploadBufferView.Map<IdentityVertex>();
            {
                vertexBufferSpan[0] = new IdentityVertex {                             //
                    Color = Colors.Red,                                             //   y          in this setup
                    Position = Vector3.Create(-0.25f, 0.25f * aspectRatio, 0.0f),   //   ^     z    the origin o
                };                                                                  //   |   /      is in the middle
                                                                                    //   | /        of the rendered scene
                vertexBufferSpan[1] = new IdentityVertex {                             //   o------>x
                    Color = Colors.Blue,                                            //
                    Position = Vector3.Create(0.25f, 0.25f * aspectRatio, 0.0f),    //   0 ----- 1
                };                                                                  //   | \     |
                                                                                    //   |   \   |
                vertexBufferSpan[2] = new IdentityVertex {                             //   |     \ |
                    Color = Colors.Lime,                                            //   3-------2
                    Position = Vector3.Create(0.25f, -0.25f * aspectRatio, 0.0f),   //
                };

                vertexBufferSpan[3] = new IdentityVertex {
                    Color = Colors.Blue,
                    Position = Vector3.Create(-0.25f, -0.25f * aspectRatio, 0.0f),
                };
            }
            uploadBufferView.UnmapAndWrite();

            var vertexBufferView = vertexBuffer.CreateView<IdentityVertex>(4);
            graphicsContext.Copy(vertexBufferView, uploadBufferView);
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

        GraphicsPipelineSignature CreateGraphicsPipelineSignature(GraphicsDevice graphicsDevice)
        {
            var inputs = new GraphicsPipelineInput[1] {
                new GraphicsPipelineInput(
                    new GraphicsPipelineInputElement[2] {
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.Color, GraphicsFormat.R32G32B32A32_SFLOAT, size: 16, alignment: 16),
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.Position, GraphicsFormat.R32G32B32_SFLOAT, size: 12, alignment: 4),
                    }
                ),
            };

            return graphicsDevice.CreatePipelineSignature(inputs);
        }
    }
}
