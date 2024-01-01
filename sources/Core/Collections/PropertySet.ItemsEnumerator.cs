// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Collections;

public partial class PropertySet
{
    /// <summary>An enumerator which can iterate through the items in a property set.</summary>
    public struct ItemsEnumerator : IRefEnumerator<KeyValuePair<string, object>>
    {
        private readonly PropertySet _propertySet;
        private int _index;
        private KeyValuePair<string, object> _current;

        internal ItemsEnumerator(PropertySet propertySet)
        {
            _propertySet = propertySet;
            _index = -1;
        }

        /// <inheritdoc />
        public readonly KeyValuePair<string, object> Current => CurrentRef;

        /// <inheritdoc />
        public readonly ref readonly KeyValuePair<string, object> CurrentRef => ref Unsafe.AsRef(in _current);

        /// <inheritdoc />
        public bool MoveNext()
        {
            var succeeded = true;

            var index = unchecked(_index + 1);
            var count = _propertySet._items._count;

            if (index == count)
            {
                index--;
                succeeded = false;
            }
            else
            {
                var entries = _propertySet._items._entries;

                while (index < count)
                {
                    ref var entry = ref entries.GetReferenceUnsafe(index);

                    if (entry.Next >= -1)
                    {
                        _current = new KeyValuePair<string, object>(entry.Key, entry.Value);
                        break;
                    }

                    index++;
                }
            }

            _index = index;
            return succeeded;
        }

        /// <inheritdoc />
        public void Reset() => _index = -1;

        readonly object? IEnumerator.Current => Current;

        readonly void IDisposable.Dispose() { }
    }
}
