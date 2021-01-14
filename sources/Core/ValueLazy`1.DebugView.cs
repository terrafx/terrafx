// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the LazyDebugView<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

namespace TerraFX
{
    public partial struct ValueLazy<T>
    {
        internal sealed class DebugView
        {
            private readonly ValueLazy<T> _value;

            public DebugView(ValueLazy<T> value)
            {
                _value = value;
            }

            public bool IsValueCreated => _value.IsValueCreated;

            public bool IsValueFaulted => _value.IsValueFaulted;

            public T? Value => _value.ValueOrDefault;
        }
    }
}
