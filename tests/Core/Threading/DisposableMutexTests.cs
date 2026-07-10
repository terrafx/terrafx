// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;
using TerraFX.Threading;

namespace TerraFX.UnitTests.Threading;

/// <summary>Provides a set of tests covering the <see cref="DisposableMutex" /> struct.</summary>
internal static class DisposableMutexTests
{
    /// <summary>Provides validation that the constructor acquires the lock and <see cref="DisposableMutex.Dispose()" /> releases it.</summary>
    [Test]
    public static void AcquiresAndReleasesTest()
    {
        var mutex = new ValueMutex();

        using (var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: false))
        {
            // The wrapper acquired the lock, so a non-blocking attempt from this thread fails.
            Assert.That(mutex.TryAcquireLock(), Is.False);
        }

        // Dispose released the lock, so it can now be acquired.
        Assert.That(mutex.TryAcquireLock(), Is.True);

        mutex.ReleaseLock();
        mutex.Dispose();
    }

    /// <summary>Provides validation that an externally-synchronized wrapper does not acquire the lock.</summary>
    [Test]
    public static void ExternallySynchronizedTest()
    {
        var mutex = new ValueMutex();

        using (var disposableMutex = new DisposableMutex(mutex, isExternallySynchronized: true))
        {
            // The wrapper did not acquire the lock, so it remains free.
            Assert.That(mutex.TryAcquireLock(), Is.True);
            mutex.ReleaseLock();
        }

        mutex.Dispose();
    }
}
