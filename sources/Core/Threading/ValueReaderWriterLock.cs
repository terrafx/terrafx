// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Threading;

/// <summary>Defines a lightweight reader-writer lock suitable for use in multimedia based applications.</summary>
public readonly unsafe partial struct ValueReaderWriterLock : IDisposable
{
    private readonly SRWLOCK* _value;

    /// <summary>Initializes a new instance of the <see cref="ValueReaderWriterLock" /> struct.</summary>
    public ValueReaderWriterLock()
    {
        var value = Allocate<SRWLOCK>();
        InitializeSRWLock(value);
        _value = value;
    }

    /// <summary><c>true</c> if the reader-writer lock is <c>null</c>; otherwise, <c>false</c>.</summary>
    public bool IsNull => _value is null;

    /// <inheritdoc />
    public void Dispose()
    {
        Free(_value);
    }

    /// <summary>Acquires a read lock on the mutex.</summary>
    public void AcquireReadLock()
    {
        AcquireSRWLockShared(_value);
    }

    /// <summary>Acquires a write lock on the mutex.</summary>
    public void AcquireWriteLock()
    {
        AcquireSRWLockExclusive(_value);
    }

    /// <summary>Attempts to acquire a read lock on the mutex.</summary>
    /// <returns><c>true</c> if the lock was successfully acquired; otherwise, <c>false</c>.</returns>
    public bool TryAcquireReadLock()
    {
        return TryAcquireSRWLockShared(_value) != 0;
    }

    /// <summary>Attempts to acquire a write lock on the mutex.</summary>
    /// <returns><c>true</c> if the lock was successfully acquired; otherwise, <c>false</c>.</returns>
    public bool TryAcquireWriteLock()
    {
        return TryAcquireSRWLockExclusive(_value) != 0; 
    }

    /// <summary>Releases a read lock on the mutex.</summary>
    public void ReleaseReadLock()
    {
        ReleaseSRWLockShared(_value);
    }

    /// <summary>Releases a write lock on the mutex.</summary>
    public void ReleaseWriteLock()
    {
        ReleaseSRWLockExclusive(_value);
    }
}
