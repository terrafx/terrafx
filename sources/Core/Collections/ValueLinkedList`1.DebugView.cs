// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ICollectionDebugView<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Diagnostics;

namespace TerraFX.Collections;

public partial struct ValueLinkedList<T>
{
    internal sealed class DebugView
    {
        private readonly ValueLinkedList<T> _linkedList;

        public DebugView(ValueLinkedList<T> linkedList)
        {
            _linkedList = linkedList;
        }

        public int Count => _linkedList._count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                ref readonly var linkedList = ref _linkedList;
                var items = GC.AllocateUninitializedArray<T>(linkedList._count);

                linkedList.CopyTo(items);
                return items;
            }
        }
    }
}
