// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using TerraFX.Collections;

namespace TerraFX;

public partial struct UnmanagedSpan<T>
{
    /// <summary>An enumerator which can iterate through the items in a span.</summary>
    public struct Enumerator : IRefEnumerator<T>
    {
        private readonly UnmanagedSpan<T> _span;
        private nuint _index;

        internal Enumerator(UnmanagedSpan<T> span)
        {
            _span = span;
            _index = nuint.MaxValue;
        }

        /// <inheritdoc />
        public T Current => CurrentRef;

        /// <inheritdoc />
        public ref readonly T CurrentRef => ref _span.GetReferenceUnsafe(_index);

        /// <inheritdoc />
        public bool MoveNext()
        {
            var succeeded = true;
            _index++;

            if (_index == _span._length)
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
