// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <summary>A graphics shader which performs a transformation for a graphics device.</summary>
public sealed unsafe class GraphicsShader : GraphicsDeviceObject
{
    private readonly UnmanagedArray<byte> _bytecode;

    private readonly string _entryPointName;

    private readonly GraphicsShaderKind _kind;

    internal GraphicsShader(GraphicsDevice device, in GraphicsShaderCreateOptions createOptions) : base(device)
    {
        device.AddShader(this);

        if (createOptions.TakeBytecodeOwnership)
        {
            _bytecode = createOptions.Bytecode;
        }
        else
        {
            _bytecode = new UnmanagedArray<byte>(createOptions.Bytecode.Length, createOptions.Bytecode.Alignment);
            createOptions.Bytecode.CopyTo(_bytecode);
        }

        _entryPointName = createOptions.EntryPointName;
        _kind = createOptions.ShaderKind;
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsShader" /> class.</summary>
    ~GraphicsShader() => Dispose(isDisposing: false);

    /// <summary>Gets the bytecode for the shader.</summary>
    public UnmanagedReadOnlySpan<byte> Bytecode => _bytecode;

    /// <summary>Gets the entry point name for the shader.</summary>
    public string EntryPointName => _entryPointName;

    /// <summary>Gets the shader kind.</summary>
    public GraphicsShaderKind Kind => _kind;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        _bytecode.Dispose();

        _ = Device.RemoveShader(this);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }
}
