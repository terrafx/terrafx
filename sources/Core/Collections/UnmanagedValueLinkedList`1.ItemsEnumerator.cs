// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Collections;

public partial struct UnmanagedValueLinkedList<T>
{
    /// <summary>An enumerator which can iterate through the items in a linked list.</summary>
    public unsafe struct ItemsEnumerator : IRefEnumerator<T>
    {
        private readonly UnmanagedValueLinkedList<T> _linkedList;
        private Node* _current;
        private nuint _index;

        internal ItemsEnumerator(UnmanagedValueLinkedList<T> linkedList)
        {
            _linkedList = linkedList;
            _current = linkedList.Last;
            _index = nuint.MaxValue;
        }

        /// <inheritdoc />
        public T Current
        {
            get
            {
                AssertNotNull(_current);
                return _current->_value;
            }
        }

        /// <inheritdoc />
        public ref readonly T CurrentRef
        {
            get
            {
                AssertNotNull(_current);
                return ref _current->_value;
            }
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            var succeeded = true;
            var index = unchecked(_index + 1);

            if (index == _linkedList._count)
            {
                index--;
                succeeded = false;
            }
            else
            {
                AssertNotNull(_current);
                _current = _current->_next;
            }

            _index = index;
            return succeeded;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _current = null;
            _index = nuint.MaxValue;
        }

        object? IEnumerator.Current => Current;

        void IDisposable.Dispose() { }
    }
}
