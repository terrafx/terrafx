// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the IDictionaryDebugView<K, V> class from https://github.com/dotnet/runtime/
// The original code is Copyright Â© .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System.Collections.Generic;
using System.Diagnostics;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Collections;

public partial class PropertySet
{
    internal sealed class DebugView
    {
        private readonly PropertySet _propertySet;

        public DebugView(PropertySet propertySet)
        {
            ThrowIfNull(propertySet);
            _propertySet = propertySet;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<string, object>[] Items
        {
            get
            {
                var items = new KeyValuePair<string, object>[_propertySet.Count];
                _propertySet._items.CopyTo(items);
                return items;
            }
        }
    }
}
