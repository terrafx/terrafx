// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using TerraFX.Utilities;

namespace TerraFX.Collections
{
    public partial class PropertySet
    {
        /// <summary>Represents a debug-view for an <see cref="IPropertySet" /> instance.</summary>
        internal sealed class DebugView
        {
            #region Fields
            private IPropertySet _propertySet;
            #endregion

            #region Constructors
            /// <summary>Initializes a new instance of the <see cref="DebugView" /> class.</summary>
            /// <param name="propertySet">The <see cref="IPropertySet" /> which the instance represents.</param>
            /// <exception cref="ArgumentNullException"><paramref name="propertySet" /> is <c>null</c>.</exception>
            public DebugView(IPropertySet propertySet)
            {
                if (propertySet is null)
                {
                    ExceptionUtilities.ThrowArgumentNullException(nameof(propertySet));
                }

                _propertySet = propertySet;
            }
            #endregion

            #region Properties
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
            #endregion
        }
    }
}
