// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;

namespace TerraFX.Collections;

public partial struct ValuePool<T>
{
    internal sealed class DebugView
    {
        private readonly ValuePool<T> _pool;

        public DebugView(ValuePool<T> pool)
        {
            _pool = pool;
        }

        public int AvailableCount => _pool._availableItems.Count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] AvailableItems
        {
            get
            {
                ref readonly var poolAvailableItems = ref _pool._availableItems;
                var availableItems = GC.AllocateUninitializedArray<T>(poolAvailableItems.Count);

                poolAvailableItems.CopyTo(availableItems);
                return availableItems;
            }
        }

        public int Capacity => _pool._items.Capacity;

        public int Count => _pool._items.Count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                ref readonly var poolItems = ref _pool._items;
                var items = GC.AllocateUninitializedArray<T>(poolItems.Count);

                poolItems.CopyTo(items);
                return items;
            }
        }
    }
}
