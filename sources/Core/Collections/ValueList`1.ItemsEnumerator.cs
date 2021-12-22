// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;

namespace TerraFX.Collections;

public partial struct ValueList<T>
{
    /// <summary>An enumerator which can iterate through the items in a list.</summary>
    public struct ItemsEnumerator : IRefEnumerator<T>
    {
        private readonly ValueList<T> _list;
        private int _index;

        internal ItemsEnumerator(ValueList<T> list)
        {
            _list = list;
            _index = -1;
        }

        /// <inheritdoc />
        public T Current => CurrentRef;

        /// <inheritdoc />
        public ref readonly T CurrentRef => ref _list.GetReferenceUnsafe(_index);

        /// <inheritdoc />
        public bool MoveNext()
        {
            var succeeded = true;
            _index++;

            if (_index == _list.Count)
            {
                _index--;
                succeeded = false;
            }

            return succeeded;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _index = -1;
        }

        object? IEnumerator.Current => Current;

        void IDisposable.Dispose() { }
    }
}
