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

        public int AvailableCount => _pool.AvailableCount;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] AvailableItems
        {
            get
            {
                var availableItems = GC.AllocateUninitializedArray<T>(_pool.AvailableCount);
                _pool._availableItems.CopyTo(availableItems);
                return availableItems;
            }
        }

        public int Capacity => _pool.Capacity;

        public int Count => _pool.Count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                var items = GC.AllocateUninitializedArray<T>(_pool.Count);
                _pool._items.CopyTo(items);
                return items;
            }
        }
    }
}
