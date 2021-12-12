// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.LibC;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.LibC.LibC;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Threading;

/// <summary>Defines a lightweight reader-writer lock suitable for use in multimedia based applications.</summary>
public readonly unsafe partial struct ValueReaderWriterLock : IDisposable
{
    // This is a SRWLOCK* on Windows and pthread_rwlock_t* on Linux
    private readonly void* _value;

    /// <summary>Initializes a new instance of the <see cref="ValueReaderWriterLock" /> struct.</summary>
    public ValueReaderWriterLock()
    {
        if (OperatingSystem.IsWindows())
        {
            var value = Allocate<SRWLOCK>();
            InitializeSRWLock(value);
            _value = value;
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = Allocate<pthread_rwlock_t>();
            value[0] = PTHREAD_RWLOCK_INITIALIZER();
            _value = value;
        }
        else
        {
            ThrowNotImplementedException();
            _value = null;
        }
    }

    /// <summary><c>true</c> if the reader-writer lock is <c>null</c>; otherwise, <c>false</c>.</summary>
    public bool IsNull => _value is null;

    /// <inheritdoc />
    public void Dispose()
    {
        if (OperatingSystem.IsWindows())
        {
            var value = (SRWLOCK*)_value;
            Free(value);
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = (pthread_rwlock_t*)_value;
            ThrowForLastErrorIfNotZero(pthread_rwlock_destroy(value));
            Free(value);
        }
        else
        {
            ThrowNotImplementedException();
        }
    }

    /// <summary>Acquires a read lock on the mutex.</summary>
    public void AcquireReadLock()
    {
        if (OperatingSystem.IsWindows())
        {
            var value = (SRWLOCK*)_value;
            AcquireSRWLockShared(value);
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = (pthread_rwlock_t*)_value;
            ThrowForLastErrorIfNotZero(pthread_rwlock_rdlock(value));
        }
        else
        {
            ThrowNotImplementedException();
        }
    }

    /// <summary>Acquires a write lock on the mutex.</summary>
    public void AcquireWriteLock()
    {
        if (OperatingSystem.IsWindows())
        {
            var value = (SRWLOCK*)_value;
            AcquireSRWLockExclusive(value);
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = (pthread_rwlock_t*)_value;
            ThrowForLastErrorIfNotZero(pthread_rwlock_wrlock(value));
        }
        else
        {
            ThrowNotImplementedException();
        }
    }

    /// <summary>Attempts to acquire a read lock on the mutex.</summary>
    /// <returns><c>true</c> if the lock was succesfully acquired; otherwise, <c>false</c>.</returns>
    public bool TryAcquireReadLock()
    {
        if (OperatingSystem.IsWindows())
        {
            var value = (SRWLOCK*)_value;
            return TryAcquireSRWLockShared(value) != 0;
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = (pthread_rwlock_t*)_value;
            return pthread_rwlock_tryrdlock(value) == 0;
        }
        else
        {
            ThrowNotImplementedException();
            return false;
        }
    }

    /// <summary>Attempts to acquire a write lock on the mutex.</summary>
    /// <returns><c>true</c> if the lock was succesfully acquired; otherwise, <c>false</c>.</returns>
    public bool TryAcquireWriteLock()
    {
        if (OperatingSystem.IsWindows())
        {
            var value = (SRWLOCK*)_value;
            return TryAcquireSRWLockExclusive(value) != 0; 
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = (pthread_rwlock_t*)_value;
            return pthread_rwlock_trywrlock(value) == 0;
        }
        else
        {
            ThrowNotImplementedException();
            return false;
        }
    }

    /// <summary>Releases a read lock on the mutex.</summary>
    public void ReleaseReadLock()
    {
        if (OperatingSystem.IsWindows())
        {
            var value = (SRWLOCK*)_value;
            ReleaseSRWLockShared(value);
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = (pthread_rwlock_t*)_value;
            ThrowForLastErrorIfNotZero(pthread_rwlock_unlock(value));
        }
        else
        {
            ThrowNotImplementedException();
        }
    }

    /// <summary>Releases a write lock on the mutex.</summary>
    public void ReleaseWriteLock()
    {
        if (OperatingSystem.IsWindows())
        {
            var value = (SRWLOCK*)_value;
            ReleaseSRWLockExclusive(value);
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = (pthread_rwlock_t*)_value;
            ThrowForLastErrorIfNotZero(pthread_rwlock_unlock(value));
        }
        else
        {
            ThrowNotImplementedException();
        }
    }
}
