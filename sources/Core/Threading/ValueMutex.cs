// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Threading;

/// <summary>Defines a lightweight mutual exclusion lock suitable for use in multimedia based applications.</summary>
public readonly unsafe partial struct ValueMutex : IDisposable
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

    /// <inheritdoc />
    public void Dispose() => Free(_value);

    /// <summary>Acquires a lock on the mutex.</summary>
    public void AcquireLock() => AcquireSRWLockExclusive(_value);

    /// <summary>Attempts to acquire a lock on the mutex.</summary>
    /// <returns><c>true</c> if the lock was successfully acquired; otherwise, <c>false</c>.</returns>
    public bool TryAcquireLock() => TryAcquireSRWLockExclusive(_value) != 0;

    /// <summary>Releases a lock on the mutex.</summary>
    public void ReleaseLock() => ReleaseSRWLockExclusive(_value);
}
