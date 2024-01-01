// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Collections;

public partial struct UnmanagedValueStack<T>
{
    internal sealed class DebugView(UnmanagedValueStack<T> stack)
    {
        private readonly UnmanagedValueStack<T> _stack = stack;

        public nuint Count => _stack._count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public unsafe T[] Items
        {
            get
            {
                ref readonly var stack = ref _stack;

                var count = Min(stack._count, MaxArrayLength);
                var items = GC.AllocateUninitializedArray<T>((int)count);

                fixed (T* pItems = items)
                {
                    var span = new UnmanagedSpan<T>(pItems, count);
                    stack.CopyTo(span);
                }
                return items;
            }
        }
    }
}
