// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;

namespace TerraFX.Samples.Graphics;

public sealed class HelloQuad(string name) : HelloWindow(name)
{
    private GraphicsBuffer _indexBuffer = null!;
    private GraphicsPrimitive _quadPrimitive = null!;
    private GraphicsBuffer _uploadBuffer = null!;
    private GraphicsBuffer _vertexBuffer = null!;

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

        _indexBuffer = graphicsDevice.CreateIndexBuffer(64 * 1024);
        _uploadBuffer = graphicsDevice.CreateUploadBuffer(64 * 1024);
        _vertexBuffer = graphicsDevice.CreateVertexBuffer(64 * 1024);

        var copyCommandQueue = graphicsDevice.CopyCommandQueue;
        var copyContext = copyCommandQueue.RentContext();
        {
            copyContext.Reset();
            {
                _quadPrimitive = CreateQuadPrimitive(copyContext);
            }
            copyContext.Close();
            copyContext.Execute();
        }
        copyCommandQueue.ReturnContext(copyContext);

        _uploadBuffer.DisposeAllViews();
    }

    protected override void Draw(GraphicsRenderContext renderContext)
    {
        _quadPrimitive.Draw(renderContext);
        base.Draw(renderContext);
    }

    private unsafe GraphicsPrimitive CreateQuadPrimitive(GraphicsCopyContext copyContext)
    {
        var renderPass = RenderPass;
        var surface = renderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(renderPass, "Identity", "main", "main");
        var uploadBuffer = _uploadBuffer;

        return new GraphicsPrimitive(
            graphicsPipeline,
            CreateVertexBufferView(copyContext, _vertexBuffer, uploadBuffer, aspectRatio: surface.PixelWidth / surface.PixelHeight),
            CreateIndexBufferView(copyContext, _indexBuffer, uploadBuffer)
        );

        static GraphicsBufferView CreateIndexBufferView(GraphicsCopyContext copyContext, GraphicsBuffer indexBuffer, GraphicsBuffer uploadBuffer)
        {
            var uploadBufferView = uploadBuffer.CreateBufferView<ushort>(6);
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

            var indexBufferView = indexBuffer.CreateBufferView<ushort>(6);
            copyContext.Copy(indexBufferView, uploadBufferView);
            return indexBufferView;
        }

        static GraphicsBufferView CreateVertexBufferView(GraphicsCopyContext copyContext, GraphicsBuffer vertexBuffer, GraphicsBuffer uploadBuffer, float aspectRatio)
        {
            var uploadBufferView = uploadBuffer.CreateBufferView<IdentityVertex>(4);
            var vertexBufferSpan = uploadBufferView.Map<IdentityVertex>();
            {
                vertexBufferSpan[0] = new IdentityVertex {                          //
                    Color = Colors.Red,                                             //   y          in this setup
                    Position = Vector3.Create(-0.25f, 0.25f * aspectRatio, 0.0f),   //   ^     z    the origin o
                };                                                                  //   |   /      is in the middle
                                                                                    //   | /        of the rendered scene
                vertexBufferSpan[1] = new IdentityVertex {                          //   o------>x
                    Color = Colors.Blue,                                            //
                    Position = Vector3.Create(0.25f, 0.25f * aspectRatio, 0.0f),    //   0 ----- 1
                };                                                                  //   | \     |
                                                                                    //   |   \   |
                vertexBufferSpan[2] = new IdentityVertex {                          //   |     \ |
                    Color = Colors.Lime,                                            //   3-------2
                    Position = Vector3.Create(0.25f, -0.25f * aspectRatio, 0.0f),   //
                };

                vertexBufferSpan[3] = new IdentityVertex {
                    Color = Colors.Blue,
                    Position = Vector3.Create(-0.25f, -0.25f * aspectRatio, 0.0f),
                };
            }
            uploadBufferView.UnmapAndWrite();

            var vertexBufferView = vertexBuffer.CreateBufferView<IdentityVertex>(4);
            copyContext.Copy(vertexBufferView, uploadBufferView);
            return vertexBufferView;
        }

        GraphicsPipeline CreateGraphicsPipeline(GraphicsRenderPass renderPass, string shaderName, string vertexShaderEntryPoint, string pixelShaderEntryPoint)
        {
            var graphicsDevice = renderPass.Device;

            var pipelineCreateOptions = new GraphicsPipelineCreateOptions {
                Signature = CreateGraphicsPipelineSignature(graphicsDevice),
                PixelShader = CompileShader(graphicsDevice, GraphicsShaderKind.Pixel, shaderName, pixelShaderEntryPoint),
                VertexShader = CompileShader(graphicsDevice, GraphicsShaderKind.Vertex, shaderName, vertexShaderEntryPoint),
            };

            return renderPass.CreatePipeline(in pipelineCreateOptions);
        }

        GraphicsPipelineSignature CreateGraphicsPipelineSignature(GraphicsDevice graphicsDevice)
        {
            var inputs = new UnmanagedArray<GraphicsPipelineInput>(2) {
                [0] = new GraphicsPipelineInput {
                    BindingIndex = 0,
                    ByteAlignment = 16,
                    ByteLength = 16,
                    Format = GraphicsFormat.R32G32B32A32_SFLOAT,
                    Kind = GraphicsPipelineInputKind.Color,
                    ShaderVisibility = GraphicsShaderVisibility.Vertex,
                },
                [1] = new GraphicsPipelineInput {
                    BindingIndex = 1,
                    ByteAlignment = 4,
                    ByteLength = 12,
                    Format = GraphicsFormat.R32G32B32_SFLOAT,
                    Kind = GraphicsPipelineInputKind.Position,
                    ShaderVisibility = GraphicsShaderVisibility.Vertex,
                },
            };

            return graphicsDevice.CreatePipelineSignature(inputs);
        }
    }
}
