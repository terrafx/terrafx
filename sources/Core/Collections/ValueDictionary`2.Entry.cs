
// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ValueDictionary<TKey, TValue> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).


namespace TerraFX.Collections;

public partial struct ValueDictionary<TKey, TValue>
{
    private struct Entry
    {
        public int HashCode;
        public int Next;
        public TKey Key;
        public TValue Value;
    }
}
