// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ICollectionDebugView<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Collections;

public partial struct UnmanagedValueLinkedList<T>
{
    internal sealed unsafe class DebugView
    {
        private readonly UnmanagedValueLinkedList<T> _linkedList;

        public DebugView(UnmanagedValueLinkedList<T> linkedList)
        {
            _linkedList = linkedList;
        }

        public nuint Count => _linkedList._count;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                ref readonly var linkedList = ref _linkedList;

                var count = Min(linkedList._count, MaxArrayLength);
                var items = GC.AllocateUninitializedArray<T>((int)count);

                fixed (T* pItems = items)
                {
                    var span = new UnmanagedSpan<T>(pItems, count);
                    linkedList.CopyTo(span);
                }
                return items;
            }
        }
    }
}
