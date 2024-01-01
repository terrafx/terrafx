// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Collections;

public partial struct ValueLinkedList<T>
{
    /// <summary>An enumerator which can iterate through the items in a linked list.</summary>
    public struct ItemsEnumerator : IRefEnumerator<T>
    {
        private readonly ValueLinkedList<T> _linkedList;
        private ValueLinkedListNode<T>? _current;
        private int _index;

        internal ItemsEnumerator(ValueLinkedList<T> linkedList)
        {
            _linkedList = linkedList;
            _current = linkedList.Last;
            _index = -1;
        }

        /// <inheritdoc />
        public readonly T Current => CurrentRef;

        /// <inheritdoc />
        public readonly ref readonly T CurrentRef
        {
            get
            {
                AssertNotNull(_current);
                return ref _current.ValueRef;
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
                _current = _current.Next;
            }

            _index = index;
            return succeeded;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _current = null;
            _index = -1;
        }

        readonly object? IEnumerator.Current => Current;

        readonly void IDisposable.Dispose() { }
    }
}
