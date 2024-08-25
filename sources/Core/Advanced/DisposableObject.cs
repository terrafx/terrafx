// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Advanced;

/// <summary>An object which is disposable.</summary>
public abstract class DisposableObject : IDisposable, INameable
{
    private string _name;
    private volatile uint _isDisposed;

    /// <summary>Initializes a new instance of the <see cref="DisposableObject" /> class.</summary>
    /// <param name="name">The name of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    protected DisposableObject(string? name)
    {
        _name = name ?? GetType().Name;
        _isDisposed = 0;
    }

    /// <summary>Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.</summary>
    public bool IsDisposed => _isDisposed != 0;

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
            SetNameUnsafe(_name);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <inheritdoc />
    public override string ToString() => _name;

    /// <summary>Asserts that the object has not been disposed.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void AssertNotDisposed() => Assert(_isDisposed == 0);

    /// <inheritdoc cref="Dispose()" />
    /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void Dispose(bool isDisposing);

    /// <summary>Sets the name of the object.</summary>
    /// <param name="value">The new name of the object.</param>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void SetNameUnsafe(string value);

    /// <summary>Throws an exception if the object has been disposed.</summary>
    /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void ThrowIfDisposed()
    {
        if (_isDisposed != 0)
        {
            ThrowObjectDisposedException(_name);
        }
    }

    /// <summary>Marks the object as being disposed.</summary>
    protected void MarkDisposed() => Interlocked.Exchange(ref _isDisposed, 1);
}
