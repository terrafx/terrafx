// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI;

/// <summary>Defines a graphics surface usable by WinForms.</summary>
public sealed unsafe class WinFormsGraphicsSurface : IGraphicsSurface
{
    private static readonly IntPtr s_entryPointModule = Marshal.GetHINSTANCE(Assembly.GetEntryAssembly()!.Modules.First());

    private readonly Control _control;
    private Vector2 _size;

    /// <summary>Initializes a new instance of the <see cref="WinFormsGraphicsSurface" /> class.</summary>
    /// <param name="control">The control that will be used as the underlying surface.</param>
    public WinFormsGraphicsSurface(Control control)
    {
        ThrowIfNull(control);

        _control = control;
        _control.ClientSizeChanged += HandleControlClientSizeChanged;
    }

    /// <inheritdoc />
    public event EventHandler<PropertyChangedEventArgs<Vector2>>? SizeChanged;

    /// <inheritdoc />
    public IntPtr ContextHandle => s_entryPointModule;

    /// <inheritdoc />
    public IntPtr Handle => _control.Handle;

    /// <inheritdoc />
    public GraphicsSurfaceKind Kind => GraphicsSurfaceKind.Win32;

    /// <inheritdoc />
    public Vector2 Size => _size;

    /// <inheritdoc />
    public void Dispose() => _control?.Dispose();

    private void HandleControlClientSizeChanged(object? sender, EventArgs eventArgs)
    {
        var controlClientSize = _control.ClientSize;
        var currentSize = new Vector2(controlClientSize.Width, controlClientSize.Height);

        var previousSize = _size;
        _size = currentSize;

        OnSizeChanged(previousSize, currentSize);
    }

    private void OnSizeChanged(Vector2 previousSize, Vector2 currentSize)
    {
        if (SizeChanged is not null)
        {
            var eventArgs = new PropertyChangedEventArgs<Vector2>(previousSize, currentSize);
            SizeChanged(this, eventArgs);
        }
    }
}
