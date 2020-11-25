// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Utilities
{
    /// <summary>Provides a simple wrapper over a <see cref="ReaderWriterLockSlim" /> so a read lock can be acquired and released via a using statement.</summary>
    public readonly struct ReaderLockSlim : IDisposable
    {
        private readonly ReaderWriterLockSlim? _mutex;

        /// <summary>Initializes a new instance of the <see cref="ReaderLockSlim" /> struct.</summary>
        /// <param name="mutex">The mutex on which a read lock should be acquired.</param>
        /// <param name="isExternallySynchronized"><c>false</c> if a read lock on <paramref name="mutex" /> should be acquired; otherwise, <c>true</c>.</param>
        public ReaderLockSlim(ReaderWriterLockSlim mutex, bool isExternallySynchronized)
        {
            AssertNotNull(mutex, nameof(mutex));

            if (!isExternallySynchronized)
            {
                mutex.EnterReadLock();
                _mutex = mutex;
            }
            else
            {
                _mutex = null;
            }
        }

        /// <inheritdoc />
        public void Dispose() => _mutex?.ExitReadLock();
    }
}
