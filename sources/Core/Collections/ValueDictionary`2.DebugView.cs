// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the IDictionaryDebugView<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TerraFX.Collections;

public partial struct ValueDictionary<TKey, TValue>
{
    internal sealed class DebugView(ValueDictionary<TKey, TValue> dictionary)
    {
        private readonly ValueDictionary<TKey, TValue> _dictionary = dictionary;

        public int Capacity => _dictionary.Capacity;

        public int Count => _dictionary._count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<TKey, TValue>[] Items
        {
            get
            {
                ref readonly var dictionary = ref _dictionary;
                var items = GC.AllocateUninitializedArray<KeyValuePair<TKey, TValue>>(dictionary._count);

                dictionary.CopyTo(items);
                return items;
            }
        }
    }
}
