// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the IDictionaryDebugView<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Collections;

public unsafe partial struct UnmanagedValueDictionary<TKey, TValue>
{
    internal sealed class DebugView(UnmanagedValueDictionary<TKey, TValue> dictionary)
    {
        private readonly UnmanagedValueDictionary<TKey, TValue> _dictionary = dictionary;

        public nuint Capacity => _dictionary.Capacity;

        public nuint Count => (uint)_dictionary._count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<TKey, TValue>[] Items
        {
            get
            {
                ref readonly var dictionary = ref _dictionary;

                var count = Min((uint)dictionary._count, MaxArrayLength);
                var items = GC.AllocateUninitializedArray<KeyValuePair<TKey, TValue>>((int)count);

                fixed (KeyValuePair<TKey, TValue>* pItems = items)
                {
                    var span = new UnmanagedSpan<KeyValuePair<TKey, TValue>>(pItems, count);
                    dictionary.CopyTo(span);
                }
                return items;
            }
        }
    }
}
