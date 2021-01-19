// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Collections
{
    public partial struct UnmanagedValueStack<T>
    {
        internal sealed class DebugView
        {
            private readonly UnmanagedValueStack<T> _stack;

            public DebugView(UnmanagedValueStack<T> stack)
            {
                _stack = stack;
            }

            public nuint Count => _stack.Count;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public unsafe T[] Items
            {
                get
                {
                    var count = Min(_stack.Count, s_maxArrayLength);
                    var items = GC.AllocateUninitializedArray<T>((int)count);

                    fixed (T* pItems = items)
                    {
                        var span = new UnmanagedSpan<T>(pItems, count);
                        _stack.CopyTo(span);
                    }
                    return items;
                }
            }
        }
    }
}
