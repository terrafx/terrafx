// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Collections;

public partial struct UnmanagedValueQueue<T>
{
    internal sealed class DebugView
    {
        private readonly UnmanagedValueQueue<T> _queue;

        public DebugView(UnmanagedValueQueue<T> queue)
        {
            _queue = queue;
        }

        public nuint Count => _queue._count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public unsafe T[] Items
        {
            get
            {
                ref readonly var queue = ref _queue;

                var count = Min(queue._count, MaxArrayLength);
                var items = GC.AllocateUninitializedArray<T>((int)count);

                fixed (T* pItems = items)
                {
                    var span = new UnmanagedSpan<T>(pItems, count);
                    queue.CopyTo(span);
                }
                return items;
            }
        }
    }
}
