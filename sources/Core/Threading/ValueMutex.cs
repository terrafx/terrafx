// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.LibC;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.LibC.LibC;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;

namespace TerraFX.Threading;

/// <summary>Defines a lightweight mutual exclusion lock suitable for use in multimedia based applications.</summary>
public readonly unsafe partial struct ValueMutex : IDisposable
{
    // This is a SRWLOCK* on Windows and pthread_mutex_t* on Linux
    private readonly void* _value;

    /// <summary>Initializes a new instance of the <see cref="ValueMutex" /> struct.</summary>
    public ValueMutex()
    {
        if (OperatingSystem.IsWindowsVersionAtLeast(6, 1))
        {
            var value = Allocate<SRWLOCK>();
            InitializeSRWLock(value);
            _value = value;
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = Allocate<pthread_mutex_t>();
            value[0] = PTHREAD_MUTEX_INITIALIZER();
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
        if (OperatingSystem.IsWindowsVersionAtLeast(6, 1))
        {
            var value = (SRWLOCK*)_value;
            Free(value);
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = (pthread_mutex_t*)_value;
            ThrowForLastErrorIfNotZero(pthread_mutex_destroy(value));
            Free(value);
        }
        else
        {
            ThrowNotImplementedException();
        }
    }

    /// <summary>Acquires a lock on the mutex.</summary>
    public void AcquireLock()
    {
        if (OperatingSystem.IsWindowsVersionAtLeast(6, 1))
        {
            var value = (SRWLOCK*)_value;
            AcquireSRWLockExclusive(value);
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = (pthread_mutex_t*)_value;
            ThrowForLastErrorIfNotZero(pthread_mutex_lock(value));
        }
        else
        {
            ThrowNotImplementedException();
        }
    }

    /// <summary>Attempts to acquire a lock on the mutex.</summary>
    /// <returns><c>true</c> if the lock was successfully acquired; otherwise, <c>false</c>.</returns>
    public bool TryAcquireLock()
    {
        if (OperatingSystem.IsWindowsVersionAtLeast(6, 1))
        {
            var value = (SRWLOCK*)_value;
            return TryAcquireSRWLockExclusive(value) != 0; 
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = (pthread_mutex_t*)_value;
            return pthread_mutex_trylock(value) == 0;
        }
        else
        {
            ThrowNotImplementedException();
            return false;
        }
    }

    /// <summary>Releases a lock on the mutex.</summary>
    public void ReleaseLock()
    {
        if (OperatingSystem.IsWindowsVersionAtLeast(6, 1))
        {
            var value = (SRWLOCK*)_value;
            ReleaseSRWLockExclusive(value);
        }
        else if (OperatingSystem.IsLinux())
        {
            var value = (pthread_mutex_t*)_value;
            ThrowForLastErrorIfNotZero(pthread_mutex_unlock(value));
        }
        else
        {
            ThrowNotImplementedException();
        }
    }
}
