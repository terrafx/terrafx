// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;

namespace TerraFX.Collections;

public partial struct UnmanagedValueList<T>
{
    /// <summary>An enumerator which can iterate through the items in a list.</summary>
    public struct ItemsEnumerator : IRefEnumerator<T>
    {
        private readonly UnmanagedValueList<T> _list;
        private nuint _index;

        internal ItemsEnumerator(UnmanagedValueList<T> list)
        {
            _list = list;
            _index = nuint.MaxValue;
        }

        /// <inheritdoc />
        public T Current => CurrentRef;

        /// <inheritdoc />
        public ref readonly T CurrentRef => ref _list.GetReferenceUnsafe(_index);

        /// <inheritdoc />
        public bool MoveNext()
        {
            var succeeded = true;
            var index = unchecked(_index + 1);

            if (index == _list._count)
            {
                index--;
                succeeded = false;
            }

            _index = index;
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
