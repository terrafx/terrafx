// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ICollectionDebugView<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Diagnostics;

namespace TerraFX.Collections;

public partial struct ValueStack<T>
{
    internal sealed class DebugView
    {
        private readonly ValueStack<T> _stack;

        public DebugView(ValueStack<T> stack)
        {
            _stack = stack;
        }

        public int Capacity => _stack.Capacity;

        public int Count => _stack.Count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                ref readonly var stack = ref _stack;
                var items = GC.AllocateUninitializedArray<T>(stack.Count);

                stack.CopyTo(items);
                return items;
            }
        }
    }
}
