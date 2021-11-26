// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX;

public partial struct UnmanagedReadOnlySpan<T>
{
    internal sealed class DebugView
    {
        private readonly UnmanagedReadOnlySpan<T> _span;

        public DebugView(UnmanagedReadOnlySpan<T> span)
        {
            _span = span;
        }

        public bool IsEmpty => _span.IsEmpty;

        public nuint Length => _span.Length;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public unsafe T[] Items
        {
            get
            {
                var count = Min(_span.Length, s_maxArrayLength);
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
