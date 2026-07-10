// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;
using TerraFX.Threading;

namespace TerraFX.UnitTests.Threading;

/// <summary>Provides a set of tests covering the <see cref="ValueMutex" /> struct.</summary>
internal static class ValueMutexTests
{
    /// <summary>Provides validation that a default instance is <c>null</c> and a constructed instance is not.</summary>
    [Test]
    public static void IsNullTest()
    {
        Assert.That(default(ValueMutex).IsNull, Is.True);

        var mutex = new ValueMutex();

        Assert.That(mutex.IsNull, Is.False);

        mutex.Dispose();
    }

    /// <summary>Provides validation of the <see cref="ValueMutex.AcquireLock()" />, <see cref="ValueMutex.TryAcquireLock()" />, and <see cref="ValueMutex.ReleaseLock()" /> methods.</summary>
    [Test]
    public static void AcquireReleaseTest()
    {
        var mutex = new ValueMutex();

        mutex.AcquireLock();

        // The lock is held exclusively, so a non-blocking attempt from this thread fails.
        Assert.That(mutex.TryAcquireLock(), Is.False);

        mutex.ReleaseLock();

        // The lock is now free, so it can be re-acquired.
        Assert.That(mutex.TryAcquireLock(), Is.True);

        mutex.ReleaseLock();
        mutex.Dispose();
    }

    /// <summary>Provides validation of the <see cref="ValueMutex.op_Equality" /> and <see cref="ValueMutex.op_Inequality" /> methods.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        ValueMutex nullMutex = default;
        ValueMutex otherNullMutex = default;

        Assert.That(nullMutex == otherNullMutex, Is.True);

        var mutex = new ValueMutex();
        var copy = mutex;

        Assert.That(mutex == copy, Is.True);
        Assert.That(mutex != nullMutex, Is.True);
        Assert.That(mutex.Equals(copy), Is.True);
        Assert.That(mutex.GetHashCode(), Is.EqualTo(copy.GetHashCode()));

        mutex.Dispose();
    }
}
