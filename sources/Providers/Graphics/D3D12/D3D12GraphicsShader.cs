// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Threading;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsShader : GraphicsShader
    {
        private readonly D3D12_SHADER_BYTECODE _d3d12ShaderBytecode;

        private VolatileState _state;

        internal D3D12GraphicsShader(D3D12GraphicsDevice device, GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
            : base(device, kind, entryPointName)
        {
            var bytecodeLength = (nuint)bytecode.Length;

            _d3d12ShaderBytecode.pShaderBytecode = Allocate(bytecodeLength);
            _d3d12ShaderBytecode.BytecodeLength = bytecodeLength;

            var destination = new Span<byte>(_d3d12ShaderBytecode.pShaderBytecode, (int)bytecodeLength);
            bytecode.CopyTo(destination);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsShader" /> class.</summary>
        ~D3D12GraphicsShader() => Dispose(isDisposing: false);

        /// <inheritdoc />
        public override ReadOnlySpan<byte> Bytecode => new ReadOnlySpan<byte>(_d3d12ShaderBytecode.pShaderBytecode, (int)_d3d12ShaderBytecode.BytecodeLength);

        /// <summary>Gets the underlying <see cref="D3D12_SHADER_BYTECODE" /> for the shader.</summary>
        public ref readonly D3D12_SHADER_BYTECODE D3D12ShaderBytecode => ref _d3d12ShaderBytecode;

        /// <inheritdoc cref="GraphicsShader.Device" />
        public new D3D12GraphicsDevice Device => (D3D12GraphicsDevice)base.Device;

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
            Free(shaderBytecode);
        }
    }
}
