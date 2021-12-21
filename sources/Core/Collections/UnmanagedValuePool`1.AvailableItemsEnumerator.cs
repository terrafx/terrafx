// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;

namespace TerraFX.Collections;

public partial struct UnmanagedValuePool<T>
{
    /// <summary>An enumerator which can iterate through the available items in a pool.</summary>
    public struct AvailableItemsEnumerator : IRefEnumerator<T>
    {
        private UnmanagedValueQueue<T>.ItemsEnumerator _enumerator;

        internal AvailableItemsEnumerator(UnmanagedValuePool<T> pool)
        {
            _enumerator = pool._availableItems.GetEnumerator();
        }

        /// <inheritdoc />
        public T Current => CurrentRef;

        /// <inheritdoc />
        public ref readonly T CurrentRef => ref _enumerator.CurrentRef;

        /// <inheritdoc />
        public bool MoveNext() => _enumerator.MoveNext();

        /// <inheritdoc />
        public void Reset() => _enumerator.Reset();

        object? IEnumerator.Current => Current;

        void IDisposable.Dispose() { }
    }
}
