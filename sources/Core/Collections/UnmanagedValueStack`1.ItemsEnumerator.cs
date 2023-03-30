// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;

namespace TerraFX.Collections;

public partial struct UnmanagedValueStack<T>
{
    /// <summary>An enumerator which can iterate through the items in a stack.</summary>
    public struct ItemsEnumerator : IRefEnumerator<T>
    {
        private readonly UnmanagedValueStack<T> _stack;
        private nuint _index;

        internal ItemsEnumerator(UnmanagedValueStack<T> stack)
        {
            _stack = stack;
            _index = nuint.MaxValue;
        }

        /// <inheritdoc />
        public T Current => CurrentRef;

        /// <inheritdoc />
        public ref readonly T CurrentRef => ref _stack.GetReferenceUnsafe(_index);

        /// <inheritdoc />
        public bool MoveNext()
        {
            var succeeded = true;
            var index = unchecked(_index + 1);

            if (index == _stack._count)
            {
                index--;
                succeeded = false;
            }

            _index = index;
            return succeeded;
        }

        /// <inheritdoc />
        public void Reset() => _index = nuint.MaxValue;

        object IEnumerator.Current => Current;

        void IDisposable.Dispose() { }
    }
}
