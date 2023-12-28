// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX;

public partial struct UnmanagedArray<T>
{
    internal struct Metadata
    {
        public nuint Length;
        public nuint Alignment;
        public T Item;
    }
}
