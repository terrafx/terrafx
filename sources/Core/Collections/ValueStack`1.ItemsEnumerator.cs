// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;

namespace TerraFX.Collections;

public partial struct ValueStack<T>
{
    /// <summary>An enumerator which can iterate through the items in a stack.</summary>
    public struct ItemsEnumerator : IRefEnumerator<T>
    {
        private readonly ValueStack<T> _stack;
        private int _index;

        internal ItemsEnumerator(ValueStack<T> stack)
        {
            _stack = stack;
            _index = -1;
        }

        /// <inheritdoc />
        public T Current => CurrentRef;

        /// <inheritdoc />
        public ref readonly T CurrentRef => ref _stack.GetReferenceUnsafe(_index);

        /// <inheritdoc />
        public bool MoveNext()
        {
            var succeeded = true;
            _index++;

            if (_index == _stack.Count)
            {
                _index--;
                succeeded = false;
            }

            return succeeded;
        }

        /// <inheritdoc />
        public void Reset() => _index = -1;

        object? IEnumerator.Current => Current;

        void IDisposable.Dispose() { }
    }
}
