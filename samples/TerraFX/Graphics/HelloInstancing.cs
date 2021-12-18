// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Samples.Graphics;

public sealed class HelloInstancing : HelloWindow
{
    private const uint InstanceCount = 128;

    private readonly Random _rng;

    private float _aspectRatio = 1.0f;
    private GraphicsBuffer _constantBuffer = null!;
    private GraphicsPrimitive _trianglePrimitive = null!;
    private UnmanagedArray<AffineTransform> _trianglePrimitiveTransforms;
    private UnmanagedArray<AffineTransform> _trianglePrimitivePerSecondDelta;
    private GraphicsBuffer _uploadBuffer = null!;
    private GraphicsBuffer _vertexBuffer = null!;

    public HelloInstancing(string name, ApplicationServiceProvider serviceProvider)
        : base(name, serviceProvider)
    {
        _rng = new Random(Seed: 20170526);
    }

    public override void Cleanup()
    {
        _trianglePrimitive?.Dispose();

        _trianglePrimitiveTransforms.Dispose();
        _trianglePrimitivePerSecondDelta.Dispose();

        _constantBuffer?.Dispose();
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

        _constantBuffer = graphicsDevice.CreateConstantBuffer(64 * 1024, GraphicsResourceCpuAccess.Write);
        _uploadBuffer = graphicsDevice.CreateUploadBuffer(64 * 1024);
        _vertexBuffer = graphicsDevice.CreateVertexBuffer(64 * 1024);

        var graphicsCopyContext = graphicsDevice.RentCopyContext();
        {
            graphicsCopyContext.Reset();

            _trianglePrimitive = CreateInstancedTrianglePrimitive(graphicsCopyContext, InstanceCount);
            _trianglePrimitiveTransforms = new UnmanagedArray<AffineTransform>(InstanceCount);
            _trianglePrimitivePerSecondDelta = new UnmanagedArray<AffineTransform>(InstanceCount);

            var triangleTransform = AffineTransform.Identity;

            for (var index = 0u; index < InstanceCount; index++)
            {
                triangleTransform.Translation = Vector3.Create(
                    GetRandomInBoundsSingle(),
                    GetRandomInBoundsSingle(),
                    0.0f
                );

                _trianglePrimitiveTransforms[index] = triangleTransform;

                triangleTransform.Translation = Vector3.Create(
                    GetRandomInBoundsSingle(),
                    GetRandomInBoundsSingle(),
                    0.0f
                );

                triangleTransform.Scale = Vector3.One;

                triangleTransform.Rotation = Quaternion.CreateFromAxisAngle(
                    Vector3.Create(GetRandomInBoundsSingle(), GetRandomInBoundsSingle(), 0.0f),
                    _rng.NextSingle()
                );

                _trianglePrimitivePerSecondDelta[index] = triangleTransform;
            }

            graphicsCopyContext.Flush();
            graphicsDevice.WaitForIdle();
        }
        graphicsDevice.ReturnContext(graphicsCopyContext);

        _uploadBuffer.DisposeAllViews();
    }

    protected override void Draw(GraphicsRenderContext graphicsRenderContext)
    {
        _trianglePrimitive.Draw(graphicsRenderContext, InstanceCount);
        base.Draw(graphicsRenderContext);
    }

    protected override unsafe void Update(TimeSpan delta)
    {
        var offsetBoundsX = 1.25f;
        var offsetBoundsY = _aspectRatio / 1.25f;

        var constantBufferView = _trianglePrimitive.PipelineResourceViews![1].As<GraphicsBufferView>();
        var constantBufferSpan = constantBufferView.Map<Matrix4x4>();
        {
            for (var index = 0u; index < InstanceCount; index++)
            {
                var trianglePrimitiveTransform = _trianglePrimitiveTransforms[index];
                {
                    var translation = trianglePrimitiveTransform.Translation + (_trianglePrimitivePerSecondDelta[index].Translation * (float)delta.TotalSeconds);

                    if (translation.X > offsetBoundsX)
                    {
                        translation = translation.WithX(-offsetBoundsX);
                    }
                    else if (translation.X < -offsetBoundsX)
                    {
                        translation = translation.WithX(offsetBoundsX);
                    }

                    if (translation.Y > offsetBoundsY)
                    {
                        translation = translation.WithY(-offsetBoundsY);
                    }
                    else if (translation.Y < -offsetBoundsY)
                    {
                        translation = translation.WithY(offsetBoundsY);
                    }

                    trianglePrimitiveTransform.Translation = translation;
                }
                _trianglePrimitiveTransforms[index] = trianglePrimitiveTransform;

                var matrix = Matrix4x4.CreateFromAffineTransform(trianglePrimitiveTransform);
                constantBufferSpan[index] = Matrix4x4.Transpose(matrix);
            }
        }
        constantBufferView.UnmapAndWrite();
    }

    private unsafe GraphicsPrimitive CreateInstancedTrianglePrimitive(GraphicsCopyContext graphicsCopyContext, uint instanceCount)
    {
        var graphicsRenderPass = GraphicsRenderPass;
        var graphicsSurface = graphicsRenderPass.Surface;

        var graphicsPipeline = CreateGraphicsPipeline(graphicsRenderPass, "Instancing", "main", "main");

        var constantBuffer = _constantBuffer;
        var uploadBuffer = _uploadBuffer;

        _aspectRatio = graphicsSurface.Width / graphicsSurface.Height;

        return new GraphicsPrimitive(
            graphicsPipeline,
            CreateVertexBufferView(graphicsCopyContext, _vertexBuffer, uploadBuffer, _aspectRatio),
            resourceViews: new GraphicsResourceView[2] {
                CreateConstantBufferView(constantBuffer, 1),
                CreateConstantBufferView(constantBuffer, instanceCount),
            }
        );

        static GraphicsBufferView CreateConstantBufferView(GraphicsBuffer constantBuffer, uint instanceCount)
        {
            var constantBufferView = constantBuffer.CreateView<Matrix4x4>(instanceCount);
            var constantBufferSpan = constantBufferView.Map<Matrix4x4>();
            {
                for (var index = 0u; index < instanceCount; index++)
                {
                    constantBufferSpan[index] = Matrix4x4.Identity;
                }
            }
            constantBufferView.UnmapAndWrite();
            return constantBufferView;
        }

        static GraphicsBufferView CreateVertexBufferView(GraphicsCopyContext graphicsCopyContext, GraphicsBuffer vertexBuffer, GraphicsBuffer uploadBuffer, float aspectRatio)
        {
            var uploadBufferView = uploadBuffer.CreateView<IdentityVertex>(3);
            var vertexBufferSpan = uploadBufferView.Map<IdentityVertex>();
            {
                vertexBufferSpan[0] = new IdentityVertex {
                    Color = Colors.Red,
                    Position = Vector3.Create(0.0f, 0.25f * aspectRatio, 0.0f),
                };

                vertexBufferSpan[1] = new IdentityVertex {
                    Color = Colors.Lime,
                    Position = Vector3.Create(0.25f, -0.25f * aspectRatio, 0.0f),
                };

                vertexBufferSpan[2] = new IdentityVertex {
                    Color = Colors.Blue,
                    Position = Vector3.Create(-0.25f, -0.25f * aspectRatio, 0.0f),
                };
            }
            uploadBufferView.UnmapAndWrite();

            var vertexBufferView = vertexBuffer.CreateView<IdentityVertex>(3);
            graphicsCopyContext.Copy(vertexBufferView, uploadBufferView);
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

        static GraphicsPipelineSignature CreateGraphicsPipelineSignature(GraphicsDevice graphicsDevice)
        {
            var inputs = new GraphicsPipelineInput[1] {
                new GraphicsPipelineInput(
                    new GraphicsPipelineInputElement[2] {
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.Color, GraphicsFormat.R32G32B32A32_SFLOAT, size: 16, alignment: 16),
                        new GraphicsPipelineInputElement(GraphicsPipelineInputElementKind.Position, GraphicsFormat.R32G32B32_SFLOAT, size: 12, alignment: 4),
                    }
                ),
            };

            var resources = new GraphicsPipelineResourceInfo[2] {
                new GraphicsPipelineResourceInfo(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                new GraphicsPipelineResourceInfo(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
            };

            return graphicsDevice.CreatePipelineSignature(inputs, resources);
        }
    }

    private float GetRandomInBoundsSingle()
    {
        return _rng.NextSingle() + _rng.NextSingle() - 1.0f;
    }
}
