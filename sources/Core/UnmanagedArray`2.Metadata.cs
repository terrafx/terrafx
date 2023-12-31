// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.CompilerServices;

namespace TerraFX;

public unsafe partial struct UnmanagedArray<T, TData>
{
    internal struct Metadata
    {
        public static readonly nuint OffsetOf_Item = GetOffsetOf_Item();

        public nuint Length;
        public nuint Alignment;
        public TData Data;
        public T Item;

        private static nuint GetOffsetOf_Item()
        {
            Unsafe.SkipInit<Metadata>(out var metadata);
            return (nuint)((byte*)&metadata.Item - (byte*)&metadata);
        }
    }
}
