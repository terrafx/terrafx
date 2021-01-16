// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ICollectionDebugView<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System.Diagnostics;

namespace TerraFX.Collections
{
    public partial struct ValueList<T>
    {
        internal sealed class DebugView
        {
            private readonly ValueList<T> _list;

            public DebugView(ValueList<T> list)
            {
                _list = list;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public T[] Items
            {
                get
                {
                    var items = new T[_list.Count];
                    _list.CopyTo(items);
                    return items;
                }
            }
        }
    }
}
