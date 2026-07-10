// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;
using TerraFX.Threading;

namespace TerraFX.UnitTests.Threading;

/// <summary>Provides a set of tests covering the <see cref="DisposableWriterLock" /> struct.</summary>
internal static class DisposableWriterLockTests
{
    /// <summary>Provides validation that the constructor acquires a write lock and <see cref="DisposableWriterLock.Dispose()" /> releases it.</summary>
    [Test]
    public static void AcquiresAndReleasesTest()
    {
        var rwLock = new ValueReaderWriterLock();

        using (var disposableWriterLock = new DisposableWriterLock(rwLock, isExternallySynchronized: false))
        {
            // The write lock is held, so a read acquisition fails.
            Assert.That(rwLock.TryAcquireReadLock(), Is.False);
        }

        // Dispose released the write lock, so a read lock can now be acquired.
        Assert.That(rwLock.TryAcquireReadLock(), Is.True);

        rwLock.ReleaseReadLock();
        rwLock.Dispose();
    }

    /// <summary>Provides validation that an externally-synchronized wrapper does not acquire the lock.</summary>
    [Test]
    public static void ExternallySynchronizedTest()
    {
        var rwLock = new ValueReaderWriterLock();

        using (var disposableWriterLock = new DisposableWriterLock(rwLock, isExternallySynchronized: true))
        {
            // No write lock was acquired, so a read lock can be taken.
            Assert.That(rwLock.TryAcquireReadLock(), Is.True);
            rwLock.ReleaseReadLock();
        }

        rwLock.Dispose();
    }
}
