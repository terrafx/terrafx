// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Threading;

/// <summary>Defines a lightweight mutual exclusion lock suitable for use in multimedia based applications.</summary>
public readonly unsafe partial struct ValueMutex
    : IDisposable,
      IEquatable<ValueMutex>
{
    private readonly SRWLOCK* _value;

    /// <summary>Initializes a new instance of the <see cref="ValueMutex" /> struct.</summary>
    public ValueMutex()
    {
        var value = Allocate<SRWLOCK>();
        InitializeSRWLock(value);
        _value = value;
    }

    /// <summary><c>true</c> if the reader-writer lock is <c>null</c>; otherwise, <c>false</c>.</summary>
    public bool IsNull => _value is null;

    /// <summary>Compares two <see cref="ValueMutex" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="ValueMutex" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="ValueMutex" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(ValueMutex left, ValueMutex right) => left._value == right._value;

    /// <summary>Compares two <see cref="ValueMutex" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="ValueMutex" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="ValueMutex" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(ValueMutex left, ValueMutex right) => left._value != right._value;

    /// <inheritdoc />
    public void Dispose() => Free(_value);

    /// <summary>Acquires a lock on the mutex.</summary>
    public void AcquireLock() => AcquireSRWLockExclusive(_value);

    /// <summary>Attempts to acquire a lock on the mutex.</summary>
    /// <returns><c>true</c> if the lock was successfully acquired; otherwise, <c>false</c>.</returns>
    public bool TryAcquireLock() => TryAcquireSRWLockExclusive(_value) != 0;

    /// <summary>Releases a lock on the mutex.</summary>
    public void ReleaseLock() => ReleaseSRWLockExclusive(_value);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => (obj is ValueMutex other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(ValueMutex other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode() => ((nuint)_value).GetHashCode();
}
