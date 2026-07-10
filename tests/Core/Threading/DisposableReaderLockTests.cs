// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;
using TerraFX.Threading;

namespace TerraFX.UnitTests.Threading;

/// <summary>Provides a set of tests covering the <see cref="DisposableReaderLock" /> struct.</summary>
internal static class DisposableReaderLockTests
{
    /// <summary>Provides validation that the constructor acquires a read lock and <see cref="DisposableReaderLock.Dispose()" /> releases it.</summary>
    [Test]
    public static void AcquiresAndReleasesTest()
    {
        var rwLock = new ValueReaderWriterLock();

        using (var disposableReaderLock = new DisposableReaderLock(rwLock, isExternallySynchronized: false))
        {
            // A read lock is held, so a write acquisition fails.
            Assert.That(rwLock.TryAcquireWriteLock(), Is.False);
        }

        // Dispose released the read lock, so the write lock can now be acquired.
        Assert.That(rwLock.TryAcquireWriteLock(), Is.True);

        rwLock.ReleaseWriteLock();
        rwLock.Dispose();
    }

    /// <summary>Provides validation that an externally-synchronized wrapper does not acquire the lock.</summary>
    [Test]
    public static void ExternallySynchronizedTest()
    {
        var rwLock = new ValueReaderWriterLock();

        using (var disposableReaderLock = new DisposableReaderLock(rwLock, isExternallySynchronized: true))
        {
            // No read lock was acquired, so a write lock can be taken.
            Assert.That(rwLock.TryAcquireWriteLock(), Is.True);
            rwLock.ReleaseWriteLock();
        }

        rwLock.Dispose();
    }
}
