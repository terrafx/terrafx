// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Collections;

public partial struct UnmanagedValueList<T>
{
    internal sealed unsafe class DebugView
    {
        private readonly UnmanagedValueList<T> _list;

        public DebugView(UnmanagedValueList<T> list)
        {
            _list = list;
        }

        public nuint Count => _list.Count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                var count = Min(_list.Count, s_maxArrayLength);
                var items = GC.AllocateUninitializedArray<T>((int)count);

                fixed (T* pItems = items)
                {
                    var span = new UnmanagedSpan<T>(pItems, count);
                    _list.CopyTo(span);
                }
                return items;
            }
        }
    }
}
