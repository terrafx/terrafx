// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Threading;

/// <summary>Provides a simple wrapper over a <see cref="ValueMutex" /> so a lock can be acquired and released via a using statement.</summary>
public readonly struct DisposableMutex : IDisposable
{
    private readonly ValueMutex _mutex;

    /// <summary>Initializes a new instance of the <see cref="DisposableMutex" /> struct.</summary>
    /// <param name="mutex">The mutex on which a lock should be acquired.</param>
    /// <param name="isExternallySynchronized"><c>false</c> if a lock on <paramref name="mutex" /> should be acquired; otherwise, <c>true</c>.</param>
    public DisposableMutex(ValueMutex mutex, bool isExternallySynchronized)
    {
        Assert(!mutex.IsNull);

        if (!isExternallySynchronized)
        {
            mutex.AcquireLock();
            _mutex = mutex;
        }
        else
        {
            _mutex = default;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (!_mutex.IsNull)
        {
            _mutex.ReleaseLock();
        }
    }
}
