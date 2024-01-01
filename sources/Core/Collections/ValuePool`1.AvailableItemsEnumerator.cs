// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;

namespace TerraFX.Collections;

public partial struct ValuePool<T>
{
    /// <summary>An enumerator which can iterate through the available items in a pool.</summary>
    public struct AvailableItemsEnumerator : IRefEnumerator<T>
    {
        private ValueQueue<T>.ItemsEnumerator _enumerator;

        internal AvailableItemsEnumerator(ValuePool<T> pool)
        {
            _enumerator = pool._availableItems.GetEnumerator();
        }

        /// <inheritdoc />
        public readonly T Current => _enumerator.Current;

        /// <inheritdoc />
        public ref readonly T CurrentRef => ref _enumerator.CurrentRef;

        /// <inheritdoc />
        public bool MoveNext() => _enumerator.MoveNext();

        /// <inheritdoc />
        public void Reset() => _enumerator.Reset();

        readonly object? IEnumerator.Current => Current;

        readonly void IDisposable.Dispose() { }
    }
}
