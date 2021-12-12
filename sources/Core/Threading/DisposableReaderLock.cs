// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Threading;

/// <summary>Provides a simple wrapper over a <see cref="ValueReaderWriterLock" /> so a read lock can be acquired and released via a using statement.</summary>
public readonly struct DisposableReaderLock : IDisposable
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

    /// <inheritdoc />
    public void Dispose()
    {
        if (!_rwLock.IsNull)
        {
            _rwLock.ReleaseReadLock();
        }
    }
}
