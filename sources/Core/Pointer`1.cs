// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX
{
    /// <summary>Wraps a <typeparamref name="T" /> pointer so it can be used as a generic type parameter.</summary>
    /// <typeparam name="T">The type of the pointer being wrapped.</typeparam>
    public unsafe struct Pointer<T> : IEquatable<Pointer<T>>
        where T : unmanaged
    {
        /// <summary>The pointer value wrapped by the instance.</summary>
        public T* Value;

        /// <summary>Initializes a new instance of <see cref="Pointer{T}" />.</summary>
        /// <param name="value">The pointer to be wrapped by the instance.</param>
        public Pointer(T* value)
        {
            Value = value;
        }

        /// <summary>Implicitly converts a <typeparamref name="T" /> pointer to a new <see cref="Pointer{T}" /> instance.</summary>
        /// <param name="value">The <typeparamref name="T" /> pointer for which to wrap.</param>
        public static implicit operator Pointer<T>(T* value) => new Pointer<T>(value);

        /// <summary>Implicitly converts a <see cref="Pointer{T}" /> instance to the <typeparamref name="T" /> pointer it wraps.</summary>
        /// <param name="value">The <see cref="Pointer{T}" /> for which to get the wrapped value.</param>
        public static implicit operator T*(Pointer<T> value) => value.Value;

        /// <summary>Compares two <see cref="Pointer{T}" /> instances for equality.</summary>
        /// <param name="l">The <see cref="Pointer{T}" /> to compare with <paramref name="r" />.</param>
        /// <param name="r">The <see cref="Pointer{T}" /> to compare with <paramref name="l" />.</param>
        /// <returns><c>true</c> if <paramref name="l" /> and <paramref name="r" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Pointer<T> l, Pointer<T> r) => l.Value == r.Value;

        /// <summary>Compares two <see cref="Pointer{T}" /> instances for inequality.</summary>
        /// <param name="l">The <see cref="Pointer{T}" /> to compare with <paramref name="r" />.</param>
        /// <param name="r">The <see cref="Pointer{T}" /> to compare with <paramref name="l" />.</param>
        /// <returns><c>true</c> if <paramref name="l" /> and <paramref name="r" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Pointer<T> l, Pointer<T> r) => !(l == r);

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Pointer<T> other && Equals(other);

        /// <inheritdoc />
        public bool Equals(Pointer<T> other) => this == other;

        /// <inheritdoc />
        public override int GetHashCode() => ((IntPtr)Value).GetHashCode();
    }
}
