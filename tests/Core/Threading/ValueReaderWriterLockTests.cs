// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;
using TerraFX.Threading;

namespace TerraFX.UnitTests.Threading;

/// <summary>Provides a set of tests covering the <see cref="ValueReaderWriterLock" /> struct.</summary>
internal static class ValueReaderWriterLockTests
{
    /// <summary>Provides validation that a default instance is <c>null</c> and a constructed instance is not.</summary>
    [Test]
    public static void IsNullTest()
    {
        Assert.That(default(ValueReaderWriterLock).IsNull, Is.True);

        var rwLock = new ValueReaderWriterLock();

        Assert.That(rwLock.IsNull, Is.False);

        rwLock.Dispose();
    }

    /// <summary>Provides validation of the read-lock acquire/release contract.</summary>
    [Test]
    public static void ReadLockTest()
    {
        var rwLock = new ValueReaderWriterLock();

        rwLock.AcquireReadLock();

        // Read locks are shared, so another read acquisition succeeds...
        Assert.That(rwLock.TryAcquireReadLock(), Is.True);

        // ...but a write acquisition fails while any read lock is held.
        Assert.That(rwLock.TryAcquireWriteLock(), Is.False);

        rwLock.ReleaseReadLock();
        rwLock.ReleaseReadLock();

        // With all read locks released, the write lock can be acquired.
        Assert.That(rwLock.TryAcquireWriteLock(), Is.True);

        rwLock.ReleaseWriteLock();
        rwLock.Dispose();
    }

    /// <summary>Provides validation of the write-lock acquire/release contract.</summary>
    [Test]
    public static void WriteLockTest()
    {
        var rwLock = new ValueReaderWriterLock();

        rwLock.AcquireWriteLock();

        // The write lock is exclusive, so neither a write nor a read acquisition succeeds.
        Assert.That(rwLock.TryAcquireWriteLock(), Is.False);
        Assert.That(rwLock.TryAcquireReadLock(), Is.False);

        rwLock.ReleaseWriteLock();

        // With the write lock released, a read lock can be acquired.
        Assert.That(rwLock.TryAcquireReadLock(), Is.True);

        rwLock.ReleaseReadLock();
        rwLock.Dispose();
    }

    /// <summary>Provides validation of the <see cref="ValueReaderWriterLock.op_Equality" /> and <see cref="ValueReaderWriterLock.op_Inequality" /> methods.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        ValueReaderWriterLock nullLock = default;
        ValueReaderWriterLock otherNullLock = default;

        Assert.That(nullLock == otherNullLock, Is.True);

        var rwLock = new ValueReaderWriterLock();
        var copy = rwLock;

        Assert.That(rwLock == copy, Is.True);
        Assert.That(rwLock != nullLock, Is.True);
        Assert.That(rwLock.Equals(copy), Is.True);
        Assert.That(rwLock.GetHashCode(), Is.EqualTo(copy.GetHashCode()));

        rwLock.Dispose();
    }
}
