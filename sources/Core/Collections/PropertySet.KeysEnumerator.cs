// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Dictionary<TKey, TValue>.KeyCollection class from https://github.com/dotnet/runtime/
// The original code is Copyright Â© .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using TerraFX.Utilities;

namespace TerraFX.Collections;

public partial class PropertySet
{
    /// <summary>An enumerator which can iterate through the keys in a property set.</summary>
    public struct KeysEnumerator : IEnumerator<string?>
    {
        private readonly PropertySet _propertySet;
        private int _index;
        private string? _current;

        internal KeysEnumerator(PropertySet propertySet)
        {
            _propertySet = propertySet;
            _index = -1;
        }

        /// <inheritdoc />
        public readonly string? Current => _current;

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
                        _current = entry.Key;
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
