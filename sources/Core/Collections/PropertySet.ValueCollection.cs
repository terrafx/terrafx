// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Dictionary<TValue, TValue>.ValueCollection class from https://github.com/dotnet/runtime/
// The original code is Copyright Â© .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Collections;

public partial class PropertySet
{
    /// <summary>Represents the collection of keys for a property set.</summary>
    public readonly partial struct ValueCollection
        : ICollection,
          ICollection<object>,
          IEquatable<ValueCollection>,
          IReadOnlyCollection<object>
    {
        private readonly PropertySet _propertySet;

        internal ValueCollection(PropertySet propertySet)
        {
            _propertySet = propertySet;
        }

        /// <inheritdoc />
        public readonly int Count => _propertySet.Count;

        /// <summary>Compares two <see cref="ValueCollection" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="ValueCollection" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="ValueCollection" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
        public static bool operator ==(ValueCollection left, ValueCollection right) => left._propertySet == right._propertySet;

        /// <summary>Compares two <see cref="UnmanagedValueDictionary{TValue, TValue}" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="UnmanagedValueDictionary{TValue, TValue}" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="UnmanagedValueDictionary{TValue, TValue}" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(ValueCollection left, ValueCollection right) => left._propertySet != right._propertySet;

        /// <inheritdoc />
        public readonly bool Contains(object item) => _propertySet.ContainsValue(item);

        /// <summary>Copies the keys of the property set to a span.</summary>
        /// <param name="destination">The span to which the keys will be copied.</param>
        /// <exception cref="ArgumentException"><see cref="Count" /> is greater than the length of <paramref name="destination" />.</exception>
        public readonly void CopyTo(Span<object> destination)
        {
            var count = _propertySet.Count;
            ThrowIfNotInInsertBounds((uint)count, destination.Length);

            var entries = _propertySet._items._entries;

            for (var i = 0; i < count; i++)
            {
                ref var entry = ref entries.GetReferenceUnsafe((uint)i);

                if (entry.Next >= -1)
                {
                    destination.GetReferenceUnsafe((uint)i) = entry.Value;
                }
            }
        }

        /// <inheritdoc />
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is ValueCollection other) && Equals(other);

        /// <inheritdoc />
        public readonly bool Equals(ValueCollection other) => this == other;

        /// <summary>Gets an enumerator that can iterate through the keys in the property set.</summary>
        /// <returns>An enumerator that can iterate through the keys in the property set.</returns>
        public readonly ValuesEnumerator GetEnumerator() => new ValuesEnumerator(_propertySet);

        /// <inheritdoc />
        public override readonly int GetHashCode() => _propertySet.GetHashCode();

        readonly bool ICollection<object>.IsReadOnly => true;

        readonly bool ICollection.IsSynchronized => false;

        readonly object ICollection.SyncRoot => _propertySet;

        readonly void ICollection<object>.Add(object item) => ThrowForInvalidState(nameof(ICollection<>.IsReadOnly));

        readonly void ICollection<object>.Clear() => ThrowForInvalidState(nameof(ICollection<>.IsReadOnly));

        readonly void ICollection<object>.CopyTo(object[] array, int arrayIndex) => CopyTo(array.AsSpan(arrayIndex));

        readonly void ICollection.CopyTo(Array array, int index)
        {
            if (array is object[] objectArray)
            {
                CopyTo(objectArray.AsSpan(index));
            }
            else
            {
                ThrowForInvalidType(array.GetType(), typeof(object[]));
            }
        }

        readonly bool ICollection<object>.Remove(object item)
        {
            ThrowForInvalidState(nameof(ICollection<>.IsReadOnly));
            return false;
        }

        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        readonly IEnumerator<object> IEnumerable<object>.GetEnumerator() => GetEnumerator();
    }
}
