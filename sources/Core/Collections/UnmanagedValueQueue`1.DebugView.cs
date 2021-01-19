// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Collections
{
    public partial struct UnmanagedValueQueue<T>
    {
        internal sealed class DebugView
        {
            private readonly UnmanagedValueQueue<T> _list;

            public DebugView(UnmanagedValueQueue<T> list)
            {
                _list = list;
            }

            public nuint Count => _list.Count;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public unsafe T[] Items
            {
                get
                {
                    var count = Max(_list.Count, s_maxArrayLength);
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
}
