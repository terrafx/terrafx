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
        public T Current => CurrentRef;

        /// <inheritdoc />
        public ref readonly T CurrentRef => ref _queue.GetReferenceUnsafe(_index);

        /// <inheritdoc />
        public bool MoveNext()
        {
            var succeeded = true;
            _index++;

            if (_index == _queue.Count)
            {
                _index--;
                succeeded = false;
            }

            return succeeded;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _index = nuint.MaxValue;
        }

        object IEnumerator.Current => Current;

        void IDisposable.Dispose() { }
    }
}
