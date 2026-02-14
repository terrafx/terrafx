// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Graphics.Advanced;
using TerraFX.Threading;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics shader which performs a transformation for a graphics device.</summary>
public sealed class GraphicsShader : IDisposable, INameable
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsDevice _device;
    private readonly GraphicsService _service;

    private readonly UnmanagedArray<byte> _bytecode;

    private readonly string _entryPointName;

    private readonly GraphicsShaderKind _kind;

    private string _name;
    private VolatileState _state;

    internal GraphicsShader(GraphicsDevice device, in GraphicsShaderCreateOptions createOptions)
    {
        AssertNotNull(device);
        _device = device;

        var adapter = device.Adapter;
        _adapter = adapter;

        var service = adapter.Service;
        _service = service;

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

        _name = GetType().Name;
        _ = _state.Transition(VolatileState.Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsShader" /> class.</summary>
    ~GraphicsShader() => Dispose(isDisposing: false);

    /// <summary>Gets the adapter for which the object was created.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the bytecode for the shader.</summary>
    public UnmanagedReadOnlySpan<byte> Bytecode => _bytecode;

    /// <summary>Gets the device for which the object was created.</summary>
    public GraphicsDevice Device => _device;

    /// <summary>Gets the entry point name for the shader.</summary>
    public string EntryPointName => _entryPointName;

    /// <summary>Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.</summary>
    public bool IsDisposed => _state.IsDisposedOrDisposing;

    /// <summary>Gets the shader kind.</summary>
    public GraphicsShaderKind Kind => _kind;

    /// <inheritdoc />
    [AllowNull]
    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value ?? GetType().Name;
        }
    }

    /// <summary>Gets the service for which the object was created.</summary>
    public GraphicsService Service => _service;

    /// <inheritdoc />
    public void Dispose()
    {
        _ = _state.BeginDispose();
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        _state.EndDispose();
    }

    /// <inheritdoc />
    public override string ToString() => _name;

    private void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            // Nothing to handle
        }

        _bytecode.Dispose();
        _ = Device.RemoveShader(this);
    }
}
