// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using TerraFX.Collections;

namespace TerraFX;

public unsafe partial struct UnmanagedArray<T>
{
    /// <summary>An enumerator which can iterate through the items in an array.</summary>
    public struct Enumerator : IRefEnumerator<T>
    {
        private readonly UnmanagedArray<T> _array;
        private nuint _index;

        internal Enumerator(UnmanagedArray<T> array)
        {
            _array = array;
            _index = nuint.MaxValue;
        }

        /// <inheritdoc />
        public T Current => CurrentRef;

        /// <inheritdoc />
        public ref readonly T CurrentRef => ref _array.GetReferenceUnsafe(_index);

        /// <inheritdoc />
        public bool MoveNext()
        {
            var succeeded = true;
            var index = unchecked(_index + 1);

            if (index == _array._data->Length)
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
