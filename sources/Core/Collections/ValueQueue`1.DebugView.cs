// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ICollectionDebugView<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Diagnostics;

namespace TerraFX.Collections
{
    public partial struct ValueQueue<T>
    {
        internal sealed class DebugView
        {
            private readonly ValueQueue<T> _queue;

            public DebugView(ValueQueue<T> queue)
            {
                _queue = queue;
            }

            public int Capacity => _queue.Capacity;

            public int Count => _queue.Count;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public T[] Items
            {
                get
                {
                    var items = GC.AllocateUninitializedArray<T>(_queue.Count);
                    _queue.CopyTo(items);
                    return items;
                }
            }
        }
    }
}
