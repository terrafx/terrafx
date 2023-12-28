// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Threading;

/// <summary>Provides a simple wrapper over a <see cref="ValueReaderWriterLock" /> so a read lock can be acquired and released via a using statement.</summary>
public readonly struct DisposableReaderLock
    : IDisposable,
      IEquatable<DisposableReaderLock>
{
    private readonly ValueReaderWriterLock _rwLock;

    /// <summary>Initializes a new instance of the <see cref="DisposableReaderLock" /> struct.</summary>
    /// <param name="rwLock">The reader-writer lock on which a read lock should be acquired.</param>
    /// <param name="isExternallySynchronized"><c>false</c> if a read lock on <paramref name="rwLock" /> should be acquired; otherwise, <c>true</c>.</param>
    public DisposableReaderLock(ValueReaderWriterLock rwLock, bool isExternallySynchronized)
    {
        Assert(!rwLock.IsNull);

        if (!isExternallySynchronized)
        {
            rwLock.AcquireReadLock();
            _rwLock = rwLock;
        }
        else
        {
            _rwLock = default;
        }
    }

    /// <summary>Compares two <see cref="DisposableReaderLock" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="DisposableReaderLock" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="DisposableReaderLock" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
    public static bool operator ==(DisposableReaderLock left, DisposableReaderLock right) => left._rwLock == right._rwLock;

    /// <summary>Compares two <see cref="DisposableReaderLock" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="DisposableReaderLock" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="DisposableReaderLock" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(DisposableReaderLock left, DisposableReaderLock right) => left._rwLock != right._rwLock;

    /// <inheritdoc />
    public void Dispose()
    {
        if (!_rwLock.IsNull)
        {
            _rwLock.ReleaseReadLock();
        }
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => (obj is DisposableReaderLock other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(DisposableReaderLock other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode() => _rwLock.GetHashCode();
}
