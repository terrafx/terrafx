// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Threading;

/// <summary>Provides a simple wrapper over a <see cref="ValueReaderWriterLock" /> so a read lock can be acquired and released via a using statement.</summary>
public readonly struct DisposableWriterLock
    : IDisposable,
      IEquatable<DisposableWriterLock>
{
    private readonly ValueReaderWriterLock _rwLock;

    /// <summary>Initializes a new instance of the <see cref="DisposableWriterLock" /> struct.</summary>
    /// <param name="rwLock">The reader-writer lock on which a write lock should be acquired.</param>
    /// <param name="isExternallySynchronized"><c>false</c> if a write lock on <paramref name="rwLock" /> should be acquired; otherwise, <c>true</c>.</param>
    public DisposableWriterLock(ValueReaderWriterLock rwLock, bool isExternallySynchronized)
    {
        Assert(!rwLock.IsNull);

        if (!isExternallySynchronized)
        {
            rwLock.AcquireWriteLock();
            _rwLock = rwLock;
        }
        else
        {
            _rwLock = default;
        }
    }

    /// <summary>Compares two <see cref="DisposableWriterLock" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="DisposableWriterLock" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="DisposableWriterLock" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(DisposableWriterLock left, DisposableWriterLock right) => left._rwLock == right._rwLock;

    /// <summary>Compares two <see cref="DisposableWriterLock" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="DisposableWriterLock" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="DisposableWriterLock" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(DisposableWriterLock left, DisposableWriterLock right) => left._rwLock != right._rwLock;

    /// <inheritdoc />
    public void Dispose()
    {
        if (!_rwLock.IsNull)
        {
            _rwLock.ReleaseWriteLock();
        }
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => (obj is DisposableWriterLock other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(DisposableWriterLock other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode() => _rwLock.GetHashCode();
}
