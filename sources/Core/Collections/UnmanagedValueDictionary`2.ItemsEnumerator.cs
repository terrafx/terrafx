// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the UnmanagedValueDictionary<TKey, TValue> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace TerraFX.Collections;

public partial struct UnmanagedValueDictionary<TKey, TValue>
{
    /// <summary>An enumerator which can iterate through the items in a dictionary.</summary>
    public struct ItemsEnumerator : IRefEnumerator<(TKey Key, TValue Value)>
    {
        private readonly UnmanagedValueDictionary<TKey, TValue> _dictionary;
        private int _index;
        private (TKey Key, TValue Value) _current;

        internal ItemsEnumerator(UnmanagedValueDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
            _index = -1;
        }

        /// <inheritdoc />
        public readonly (TKey Key, TValue Value) Current => CurrentRef;

        /// <inheritdoc />
        public readonly ref readonly (TKey Key, TValue Value) CurrentRef => ref Unsafe.AsRef(in _current);

        /// <inheritdoc />
        public bool MoveNext()
        {
            var succeeded = true;

            var index = unchecked(_index + 1);
            var count = _dictionary._count;

            if (index == count)
            {
                index--;
                succeeded = false;
            }
            else
            {
                var entries = _dictionary._entries;

                while (index < count)
                {
                    ref var entry = ref entries.GetReferenceUnsafe((uint)index);

                    if (entry.Next >= -1)
                    {
                        _current = (entry.Key, entry.Value);
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
