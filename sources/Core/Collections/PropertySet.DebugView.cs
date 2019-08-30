// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Collections
{
    public partial class PropertySet
    {
        /// <summary>Represents a debug-view for an <see cref="IPropertySet" /> instance.</summary>
        private sealed class DebugView
        {
            private readonly IPropertySet _propertySet;

            /// <summary>Initializes a new instance of the <see cref="DebugView" /> class.</summary>
            /// <param name="propertySet">The <see cref="IPropertySet" /> which the instance represents.</param>
            /// <exception cref="ArgumentNullException"><paramref name="propertySet" /> is <c>null</c>.</exception>
            public DebugView(IPropertySet propertySet)
            {
                ThrowIfNull(propertySet, nameof(propertySet));
                _propertySet = propertySet;
            }

            /// <summary>Gets the items contained by the underlying <see cref="IPropertySet" />.</summary>
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public KeyValuePair<string, object>[] Items
            {
                get
                {
                    var items = new KeyValuePair<string, object>[_propertySet.Count];
                    _propertySet.CopyTo(items, 0);
                    return items;
                }
            }
        }
    }
}
