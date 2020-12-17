// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Graphics.Geometry3D;
using TerraFX.Numerics;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.Graphics
{
    public sealed class HelloModelViewProjection : HelloWindow
    {
        private readonly List<GraphicsPrimitive> _meshPrimitives = null!;
        private GraphicsBuffer _constantBuffer = null!;
        private GraphicsBuffer _indexBuffer = null!;
        private GraphicsBuffer _vertexBuffer = null!;

        private readonly List<Mesh> _meshes = null!;
        private readonly List<AppearanceStyle> _styles = null!;

        private Appearance<Model<Box>> _box;
        private Appearance<Model<Tetrahedron>> _tetrahedron;

        private Camera _camera;
        private float _meshRotationAngle;

        public HelloModelViewProjection(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
            var boxTransform = Transform
                .Identity
                .WithScale(new Vector3(0.5f, 0.5f, 0.5f))
                .WithTranslation(new Vector3(0.5f, 0, 0.5f));
            boxTransform = Transform.AddRotationAroundTargetX(boxTransform, 0.1f);

            var tetrahedronTransform = Transform
                .Identity
                .WithScale(new Vector3(0.6f, 0.6f, 0.6f))
                .WithTranslation(new Vector3(-0.5f, 0, 0.5f));
            tetrahedronTransform = Transform.AddRotationAroundTargetX(tetrahedronTransform, 0.1f);
            tetrahedronTransform = Transform.AddRotationAroundTargetY(tetrahedronTransform, 0.1f);

            _box = new Appearance<Model<Box>>(new Model<Box>(Box.One, boxTransform), AppearanceStyle.Green);
            _tetrahedron = new Appearance<Model<Tetrahedron>>(new Model<Tetrahedron>(Tetrahedron.One, tetrahedronTransform), AppearanceStyle.Red);
            _meshPrimitives = new List<GraphicsPrimitive>();
            _meshes = new List<Mesh>();
            _styles = new List<AppearanceStyle>();
        }

        public override void Cleanup()
        {
            for (var i = 0; i < _meshPrimitives.Count; i++)
            {
                _meshPrimitives[i]?.Dispose();
            }

            _constantBuffer?.Dispose();
            _vertexBuffer?.Dispose();
            _indexBuffer?.Dispose();
            base.Cleanup();
        }

        public override void Initialize(Application application, TimeSpan timeout, Vector2? windowLocation, Vector2? windowSize)
        {
            base.Initialize(application, timeout, windowLocation, windowSize);
            _camera = new Camera(Transform.Identity, Window.ClientSize, new Vector2(0, 1), ProjectionMode.Orthogonal);

            var graphicsDevice = GraphicsDevice;
            var currentGraphicsContext = graphicsDevice.CurrentContext;

            _meshes.Add(_box.Model.Shape.ToMesh());
            _meshes.Add(_tetrahedron.Model.Shape.ToMesh());
            _styles.Add(_box.Style);
            _styles.Add(_tetrahedron.Style);

            currentGraphicsContext.BeginFrame();
            ulong vertexBufferSize = 0;
            ulong indexBufferSize = 0;

            for (var i = 0; i < _meshes.Count; ++i)
            {
                var mesh = _meshes[i];
                vertexBufferSize += (ulong)(SizeOf<PosNormColorVertex>() * mesh.Vertices.Length);
                indexBufferSize += (ulong)(SizeOf<uint>() * 3 * mesh.TriangleIndices.Length);
            }

            using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, vertexBufferSize);
            using var indexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, indexBufferSize);

            _constantBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Constant, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
            _vertexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.GpuOnly, vertexBufferSize);
            _indexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Index, GraphicsResourceCpuAccess.GpuOnly, indexBufferSize);

            for (var i = 0; i < _meshes.Count; ++i)
            {
                _meshPrimitives.Add(CreateMeshPrimitive(i, currentGraphicsContext, vertexStagingBuffer, indexStagingBuffer));
            }

            currentGraphicsContext.EndFrame();

            graphicsDevice.Signal(currentGraphicsContext.Fence);
            graphicsDevice.WaitForIdle();
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            foreach (var meshPrimitive in _meshPrimitives)
            {
                graphicsContext.Draw(meshPrimitive);
            }

            base.Draw(graphicsContext);
        }

        protected override unsafe void Update(TimeSpan delta)
        {
            const float RotationSpeed = 1f;

            var angle = _meshRotationAngle;
            var angleDelta = (float)(RotationSpeed * delta.TotalSeconds);
            angle += angleDelta;
            angle %= MathF.Tau;

            _meshRotationAngle = angle;
            var sin = MathF.Sin(angle);
            var cos = MathF.Cos(angle);

            var camToWorld = Transform.AddRotationAroundSourceY(Transform.Identity, angle)
                .WithTranslation(new Vector3(cos, 0, sin));
            _camera = _camera.WithToWorld(camToWorld);

            { // box
                var constantBufferRegion = _meshPrimitives[0].InputResourceRegions[0];
                var constantBuffer = _constantBuffer;
                var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

                var boxToWorld = Transform.AddRotationAroundTargetY(_box.Model.ToWorld, angleDelta);
                _box = _box.WithModel(_box.Model.WithToWorld(boxToWorld));
                pConstantBuffer[0] = Matrix4x4.Transpose(Transform.ToMatrix4x4(_box.Model.ToWorld)); // Transposed because shaders store matrices in column major order
                pConstantBuffer[1] = _camera.ViewProjectionMatrixColumnMajor();

                constantBuffer.UnmapAndWrite(in constantBufferRegion);
            }

            { // tetrahedron
                var constantBufferRegion = _meshPrimitives[1].InputResourceRegions[0];
                var constantBuffer = _constantBuffer;
                var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

                var tetraToWorld = Transform.AddRotationAroundTargetX(_tetrahedron.Model.ToWorld, angleDelta);
                _tetrahedron = _tetrahedron.WithModel(_tetrahedron.Model.WithToWorld(tetraToWorld));
                pConstantBuffer[0] = Matrix4x4.Transpose(Transform.ToMatrix4x4(_tetrahedron.Model.ToWorld)); // Transposed because shaders store matrices in column major order
                pConstantBuffer[1] = _camera.ViewProjectionMatrixColumnMajor();

                constantBuffer.UnmapAndWrite(in constantBufferRegion);
            }
        }

        private unsafe GraphicsPrimitive CreateMeshPrimitive(int primitiveIndex, GraphicsContext graphicsContext, GraphicsBuffer vertexStagingBuffer, GraphicsBuffer indexStagingBuffer)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "ColorTransformShading", "main", "main");

            var constantBuffer = _constantBuffer;
            var indexBuffer = _indexBuffer;
            var vertexBuffer = _vertexBuffer;

            var vertexBufferRegion = CreateVertexBufferRegion(primitiveIndex, graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

            var indexBufferRegion = CreateIndexBufferRegion(primitiveIndex, graphicsContext, indexBuffer, indexStagingBuffer);
            graphicsContext.Copy(indexBuffer, indexStagingBuffer);

            var inputResourceRegions = new GraphicsMemoryRegion<GraphicsResource>[2] {
                CreateConstantBufferRegion(graphicsContext, constantBuffer),
                CreateConstantBufferRegion(graphicsContext, constantBuffer),
            };
            return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferRegion, SizeOf<PosNormColorVertex>(), indexBufferRegion, SizeOf<uint>(), inputResourceRegions: inputResourceRegions);

            static GraphicsMemoryRegion<GraphicsResource> CreateConstantBufferRegion(GraphicsContext graphicsContext, GraphicsBuffer constantBuffer)
            {
                var constantBufferRegion = constantBuffer.Allocate(SizeOf<Matrix4x4>(), alignment: 256);
                var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

                pConstantBuffer[0] = Matrix4x4.Identity;

                constantBuffer.UnmapAndWrite(in constantBufferRegion);
                return constantBufferRegion;
            }

            GraphicsMemoryRegion<GraphicsResource> CreateIndexBufferRegion(int primitiveIndex, GraphicsContext graphicsContext, GraphicsBuffer indexBuffer, GraphicsBuffer indexStagingBuffer)
            {
                var indices = _meshes[primitiveIndex].TriangleIndices;

                var indexBufferRegion = indexBuffer.Allocate((ulong)(SizeOf<uint>() * 3 * indices.Length), alignment: 2);
                var pIndexBuffer = indexStagingBuffer.Map<uint>(in indexBufferRegion);

                for (var i = 0; i < indices.Length; i++)
                {
                    var t = indices[i];
                    pIndexBuffer[(3 * i) + 0] = t.Indices.Item1;
                    pIndexBuffer[(3 * i) + 1] = t.Indices.Item2;
                    pIndexBuffer[(3 * i) + 2] = t.Indices.Item3;
                }

                indexStagingBuffer.UnmapAndWrite(in indexBufferRegion);
                return indexBufferRegion;
            }

            GraphicsMemoryRegion<GraphicsResource> CreateVertexBufferRegion(int primitiveIndex, GraphicsContext graphicsContext, GraphicsBuffer vertexBuffer, GraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertices = _meshes[primitiveIndex].Vertices;
                var normals = _meshes[primitiveIndex].Normals;
                var vertexBufferRegion = vertexBuffer.Allocate((ulong)(SizeOf<PosNormColorVertex>() * vertices.Length), alignment: 16);
                var pVertexBuffer = vertexStagingBuffer.Map<PosNormColorVertex>(in vertexBufferRegion);

                var color = ColorRgba.ToVector4(_styles[primitiveIndex].FaceColorRgba);
                for (var i = 0; i < vertices.Length; i++)
                {
                    var v = vertices[i];
                    var n = normals[i];
                    pVertexBuffer[i] = new PosNormColorVertex {
                        Position = v,
                        Normal = n,
                        Color = color,
                    };
                }

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
                        new GraphicsPipelineInputElement[3] {
                            new GraphicsPipelineInputElement(typeof(Vector3), GraphicsPipelineInputElementKind.Position, size: 12),
                            new GraphicsPipelineInputElement(typeof(Vector3), GraphicsPipelineInputElementKind.Normal, size: 12),
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
