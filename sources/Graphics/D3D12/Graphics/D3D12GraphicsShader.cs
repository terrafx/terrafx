// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsShader : GraphicsShader
{
    internal D3D12GraphicsShader(D3D12GraphicsDevice device, in GraphicsShaderCreateOptions createOptions) : base(device)
    {
        device.AddShader(this);

        if (createOptions.TakeBytecodeOwnership)
        {
            ShaderInfo.Bytecode = createOptions.Bytecode;
        }
        else
        {
            ShaderInfo.Bytecode = new UnmanagedArray<byte>(createOptions.Bytecode.Length, createOptions.Bytecode.Alignment);
            createOptions.Bytecode.CopyTo(ShaderInfo.Bytecode);
        }

        ShaderInfo.EntryPointName = createOptions.EntryPointName;
        ShaderInfo.Kind = createOptions.ShaderKind;
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsShader" /> class.</summary>
    ~D3D12GraphicsShader() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        ShaderInfo.Bytecode.Dispose();

        _ = Device.RemoveShader(this);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }
}
