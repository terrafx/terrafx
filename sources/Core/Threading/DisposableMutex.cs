// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Threading;

/// <summary>Provides a simple wrapper over a <see cref="ValueMutex" /> so a lock can be acquired and released via a using statement.</summary>
public readonly struct DisposableMutex
    : IDisposable,
      IEquatable<DisposableMutex>
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

    /// <summary>Compares two <see cref="DisposableMutex" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="DisposableMutex" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="DisposableMutex" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(DisposableMutex left, DisposableMutex right) => left._mutex == right._mutex;

    /// <summary>Compares two <see cref="DisposableMutex" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="DisposableMutex" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="DisposableMutex" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(DisposableMutex left, DisposableMutex right) => left._mutex != right._mutex;

    /// <inheritdoc />
    public void Dispose()
    {
        if (!_mutex.IsNull)
        {
            _mutex.ReleaseLock();
        }
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => (obj is DisposableMutex other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(DisposableMutex other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode() => _mutex.GetHashCode();
}
