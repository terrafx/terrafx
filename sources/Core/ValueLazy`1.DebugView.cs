// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the LazyDebugView<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

namespace TerraFX;

public partial struct ValueLazy<T>
{
    internal sealed class DebugView
    {
        private readonly ValueLazy<T> _lazy;

        public DebugView(ValueLazy<T> lazy)
        {
            _lazy = lazy;
        }

        public bool IsValueCreated => _lazy.IsValueCreated;

        public bool IsValueFaulted => _lazy.IsValueFaulted;

        public T? Value => _lazy.ValueOrDefault;
    }
}
