// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Dictionary<TKey, TValue>.KeyCollection class from https://github.com/dotnet/runtime/
// The original code is Copyright Â© .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Collections;

#pragma warning disable CA1034 // Nested types should not be visible

public partial class PropertySet
{
    /// <summary>Represents the collection of keys for a property set.</summary>
    public readonly partial struct KeyCollection
        : ICollection,
          ICollection<string>,
          IEquatable<KeyCollection>,
          IReadOnlyCollection<string>
    {
        private readonly PropertySet _propertySet;

        internal KeyCollection(PropertySet propertySet)
        {
            _propertySet = propertySet;
        }

        /// <inheritdoc />
        public readonly int Count => _propertySet.Count;

        /// <summary>Compares two <see cref="KeyCollection" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="KeyCollection" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="KeyCollection" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, false.</returns>
        public static bool operator ==(KeyCollection left, KeyCollection right) => left._propertySet == right._propertySet;

        /// <summary>Compares two <see cref="UnmanagedValueDictionary{TKey, TValue}" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="UnmanagedValueDictionary{TKey, TValue}" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="UnmanagedValueDictionary{TKey, TValue}" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(KeyCollection left, KeyCollection right) => left._propertySet != right._propertySet;

        /// <inheritdoc />
        public readonly bool Contains(string item) => _propertySet.ContainsKey(item);

        /// <summary>Copies the keys of the property set to a span.</summary>
        /// <param name="destination">The span to which the keys will be copied.</param>
        /// <exception cref="ArgumentException"><see cref="Count" /> is greater than the length of <paramref name="destination" />.</exception>
        public readonly void CopyTo(Span<string> destination)
        {
            var count = _propertySet.Count;
            ThrowIfNotInInsertBounds((uint)count, destination.Length);

            var entries = _propertySet._items._entries;

            for (var i = 0; i < count; i++)
            {
                ref var entry = ref entries.GetReferenceUnsafe((uint)i);

                if (entry.Next >= -1)
                {
                    destination.GetReferenceUnsafe((uint)i) = entry.Key;
                }
            }
        }

        /// <inheritdoc />
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is KeyCollection other) && Equals(other);

        /// <inheritdoc />
        public readonly bool Equals(KeyCollection other) => this == other;

        /// <summary>Gets an enumerator that can iterate through the keys in the property set.</summary>
        /// <returns>An enumerator that can iterate through the keys in the property set.</returns>
        public readonly KeysEnumerator GetEnumerator() => new KeysEnumerator(_propertySet);

        /// <inheritdoc />
        public override readonly int GetHashCode() => _propertySet.GetHashCode();

        readonly bool ICollection<string>.IsReadOnly => true;

        readonly bool ICollection.IsSynchronized => false;

        readonly object ICollection.SyncRoot => _propertySet;

        readonly void ICollection<string>.Add(string item) => ThrowForInvalidState(nameof(ICollection<string>.IsReadOnly));

        readonly void ICollection<string>.Clear() => ThrowForInvalidState(nameof(ICollection<string>.IsReadOnly));

        readonly void ICollection<string>.CopyTo(string[] array, int arrayIndex) => CopyTo(array.AsSpan(arrayIndex));

        readonly void ICollection.CopyTo(Array array, int index)
        {
            if (array is string[] stringArray)
            {
                CopyTo(stringArray.AsSpan(index));
            }
            else
            {
                ThrowForInvalidType(array.GetType(), typeof(string[]));
            }
        }

        readonly bool ICollection<string>.Remove(string item)
        {
            ThrowForInvalidState(nameof(ICollection<string>.IsReadOnly));
            return false;
        }

        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        readonly IEnumerator<string> IEnumerable<string>.GetEnumerator() => GetEnumerator();
    }
}
