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

        public nuint AvailableCount => _pool.AvailableCount;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] AvailableItems
        {
            get
            {
                var availableCount = Min(_pool._availableItems.Count, MaxArrayLength);
                var availableItems = GC.AllocateUninitializedArray<T>((int)availableCount);

                fixed (T* pItems = availableItems)
                {
                    var span = new UnmanagedSpan<T>(pItems, availableCount);
                    _pool._availableItems.CopyTo(span);
                }
                return availableItems;
            }
        }

        public nuint Capacity => _pool.Capacity;

        public nuint Count => _pool.Count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                var count = Min(_pool._items.Count, MaxArrayLength);
                var items = GC.AllocateUninitializedArray<T>((int)count);

                fixed (T* pItems = items)
                {
                    var span = new UnmanagedSpan<T>(pItems, count);
                    _pool._items.CopyTo(span);
                }
                return items;
            }
        }
    }
}
