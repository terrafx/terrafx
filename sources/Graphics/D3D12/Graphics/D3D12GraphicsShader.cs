// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Advanced;
using TerraFX.Interop.DirectX;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsShader : GraphicsShader
{
    private readonly D3D12_SHADER_BYTECODE _d3d12ShaderBytecode;

    internal D3D12GraphicsShader(D3D12GraphicsDevice device, GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
        : base(device, kind, entryPointName)
    {
        _d3d12ShaderBytecode.pShaderBytecode = Allocate((uint)bytecode.Length);
        _d3d12ShaderBytecode.BytecodeLength = (uint)bytecode.Length;

        var destination = new Span<byte>(_d3d12ShaderBytecode.pShaderBytecode, bytecode.Length);
        bytecode.CopyTo(destination);
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsShader" /> class.</summary>
    ~D3D12GraphicsShader() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <inheritdoc />
    public override UnmanagedReadOnlySpan<byte> Bytecode => new UnmanagedReadOnlySpan<byte>((byte*)_d3d12ShaderBytecode.pShaderBytecode, _d3d12ShaderBytecode.BytecodeLength);

    /// <summary>Gets the underlying <see cref="D3D12_SHADER_BYTECODE" /> for the shader.</summary>
    public ref readonly D3D12_SHADER_BYTECODE D3D12ShaderBytecode => ref _d3d12ShaderBytecode;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        Free(_d3d12ShaderBytecode.pShaderBytecode);
    }
}
