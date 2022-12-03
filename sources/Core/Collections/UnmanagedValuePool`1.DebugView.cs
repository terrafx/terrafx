// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Collections;

public partial struct UnmanagedValuePool<T>
{
    internal sealed unsafe class DebugView
    {
        private readonly UnmanagedValuePool<T> _pool;

        public DebugView(UnmanagedValuePool<T> pool)
        {
            _pool = pool;
        }

        public nuint AvailableCount => _pool._availableItems.Count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] AvailableItems
        {
            get
            {
                ref readonly var poolAvailableItems = ref _pool._availableItems;

                var availableCount = Min(poolAvailableItems.Count, MaxArrayLength);
                var availableItems = GC.AllocateUninitializedArray<T>((int)availableCount);

                fixed (T* pItems = availableItems)
                {
                    var span = new UnmanagedSpan<T>(pItems, availableCount);
                    poolAvailableItems.CopyTo(span);
                }
                return availableItems;
            }
        }

        public nuint Capacity => _pool._items.Capacity;

        public nuint Count => _pool._items.Count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                ref readonly var poolItems = ref _pool._items;

                var count = Min(poolItems.Count, MaxArrayLength);
                var items = GC.AllocateUninitializedArray<T>((int)count);

                fixed (T* pItems = items)
                {
                    var span = new UnmanagedSpan<T>(pItems, count);
                    poolItems.CopyTo(span);
                }
                return items;
            }
        }
    }
}
