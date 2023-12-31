// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX;

public unsafe partial struct UnmanagedArray<T, TData>
{
    internal sealed class DebugView(UnmanagedArray<T, TData> array)
    {
        private readonly UnmanagedArray<T, TData> _array = array;

        public nuint Alignment => _array.Alignment;

        public TData Data => _array.Data;

        public bool IsNull => _array.IsNull;

        public nuint Length => _array.Length;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                var array = _array;

                var count = Min(array.Length, MaxArrayLength);
                var items = GC.AllocateUninitializedArray<T>((int)count);

                fixed (T* pItems = items)
                {
                    var span = new UnmanagedSpan<T>(pItems, count);
                    array.CopyTo(span);
                }
                return items;
            }
        }
    }
}
