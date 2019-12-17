// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsShader : GraphicsShader
    {
        private readonly D3D12_SHADER_BYTECODE _d3d12ShaderBytecode;

        private State _state;

        internal D3D12GraphicsShader(D3D12GraphicsDevice graphicsDevice, GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
            : base(graphicsDevice, kind, entryPointName)
        {
            var bytecodeLength = bytecode.Length;

            _d3d12ShaderBytecode.pShaderBytecode = Allocate(bytecodeLength);
            _d3d12ShaderBytecode.BytecodeLength = (UIntPtr)bytecodeLength;

            var destination = new Span<byte>(_d3d12ShaderBytecode.pShaderBytecode, bytecodeLength);
            bytecode.CopyTo(destination);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsShader" /> class.</summary>
        ~D3D12GraphicsShader()
        {
            Dispose(isDisposing: false);
        }

        /// <inheritdoc />
        public override ReadOnlySpan<byte> Bytecode => new ReadOnlySpan<byte>(_d3d12ShaderBytecode.pShaderBytecode, (int)_d3d12ShaderBytecode.BytecodeLength);

        /// <inheritdoc cref="GraphicsShader.GraphicsDevice" />
        public D3D12GraphicsDevice D3D12GraphicsDevice => (D3D12GraphicsDevice)GraphicsDevice;

        /// <summary>Gets the underlying <see cref="D3D12_SHADER_BYTECODE" /> for the shader.</summary>
        public ref readonly D3D12_SHADER_BYTECODE D3D12ShaderBytecode => ref _d3d12ShaderBytecode;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeD3D12ShaderBytecode();
            }

            _state.EndDispose();
        }

        private void DisposeD3D12ShaderBytecode()
        {
            var shaderBytecode = _d3d12ShaderBytecode.pShaderBytecode;

            if (shaderBytecode != null)
            {
                Free(shaderBytecode);
            }
        }
    }
}
