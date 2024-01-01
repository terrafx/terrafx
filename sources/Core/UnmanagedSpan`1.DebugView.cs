// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX;

public readonly partial struct UnmanagedSpan<T>
{
    internal sealed class DebugView(UnmanagedSpan<T> span)
    {
        private readonly UnmanagedSpan<T> _span = span;

        public bool IsEmpty => _span.IsEmpty;

        public nuint Length => _span._length;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public unsafe T[] Items
        {
            get
            {
                var count = Min(_span._length, MaxArrayLength);
                var items = GC.AllocateUninitializedArray<T>((int)count);

                fixed (T* pItems = items)
                {
                    var span = new UnmanagedSpan<T>(pItems, count);
                    _span.CopyTo(span);
                }
                return items;
            }
        }
    }
}
