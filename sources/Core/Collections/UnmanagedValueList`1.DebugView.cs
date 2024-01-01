// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Collections;

public partial struct UnmanagedValueList<T>
{
    internal sealed unsafe class DebugView(UnmanagedValueList<T> list)
    {
        private readonly UnmanagedValueList<T> _list = list;

        public nuint Count => _list._count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                ref readonly var list = ref _list;

                var count = Min(list._count, MaxArrayLength);
                var items = GC.AllocateUninitializedArray<T>((int)count);

                fixed (T* pItems = items)
                {
                    var span = new UnmanagedSpan<T>(pItems, count);
                    list.CopyTo(span);
                }
                return items;
            }
        }
    }
}
