// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX;

public unsafe partial struct UnmanagedArray<T>
{
    internal sealed class DebugView
    {
        private readonly UnmanagedArray<T> _array;

        public DebugView(UnmanagedArray<T> array)
        {
            _array = array;
        }

        public nuint Alignment => _array._data->Alignment;

        public bool IsNull => _array.IsNull;

        public nuint Length => _array._data->Length;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public unsafe T[] Items
        {
            get
            {
                var array = _array;

                var count = Min(array._data->Length, MaxArrayLength);
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
