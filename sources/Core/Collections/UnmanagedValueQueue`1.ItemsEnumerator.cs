// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;

namespace TerraFX.Collections;

public partial struct UnmanagedValueQueue<T>
{
    /// <summary>An enumerator which can iterate through the items in a queue.</summary>
    public struct ItemsEnumerator : IRefEnumerator<T>
    {
        private readonly UnmanagedValueQueue<T> _queue;
        private nuint _index;

        internal ItemsEnumerator(UnmanagedValueQueue<T> queue)
        {
            _queue = queue;
            _index = nuint.MaxValue;
        }

        /// <inheritdoc />
        public readonly T Current => CurrentRef;

        /// <inheritdoc />
        public readonly ref readonly T CurrentRef => ref _queue.GetReferenceUnsafe(_index);

        /// <inheritdoc />
        public bool MoveNext()
        {
            var succeeded = true;
            var index = unchecked(_index + 1);

            if (index == _queue._count)
            {
                index--;
                succeeded = false;
            }

            _index = index;
            return succeeded;
        }

        /// <inheritdoc />
        public void Reset() => _index = nuint.MaxValue;

        readonly object IEnumerator.Current => Current;

        readonly void IDisposable.Dispose() { }
    }
}
