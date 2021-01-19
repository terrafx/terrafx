// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX
{
    public readonly partial struct UnmanagedArray<T>
    {
        internal sealed class DebugView
        {
            private readonly UnmanagedArray<T> _array;

            public DebugView(UnmanagedArray<T> array)
            {
                _array = array;
            }

            public nuint Alignment => _array.Alignment;

            public bool IsNull => _array.IsNull;

            public nuint Length => _array.Length;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public unsafe T[] Items
            {
                get
                {
                    var count = Max(_array.Length, s_maxArrayLength);
                    var items = GC.AllocateUninitializedArray<T>((int)count);

                    fixed (T* pItems = items)
                    {
                        var span = new UnmanagedSpan<T>(pItems, count);
                        _array.CopyTo(span);
                    }
                    return items;
                }
            }
        }
    }
}
