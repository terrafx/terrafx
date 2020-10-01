// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;

namespace TerraFX.Numerics
{
    /// <summary>Defines a bounding box.</summary>
    public readonly struct BoundingBox : IEquatable<BoundingBox>, IFormattable
    {
        private readonly Vector3 _center;
        private readonly Vector3 _size;

        /// <summary>Initializes a new instance of the <see cref="BoundingBox" /> struct.</summary>
        /// <param name="center">The center of the bounding box.</param>
        /// <param name="size">The size of the bounding box.</param>
        public BoundingBox(Vector3 center, Vector3 size)
        {
            _center = center;
            _size = size;
        }

        /// <summary>Gets the center of the bounding box.</summary>
        public Vector3 Center => _center;

        /// <summary>Gets the size of the bounding box.</summary>
        public Vector3 Size => _size;

        /// <summary>Compares two <see cref="BoundingBox" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="BoundingBox" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="BoundingBox" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(BoundingBox left, BoundingBox right)
        {
            return (left.Center == right.Center)
                && (left.Size == right.Size);
        }

        /// <summary>Compares two <see cref="BoundingBox" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="BoundingBox" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="BoundingBox" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(BoundingBox left, BoundingBox right)
        {
            return (left.Center != right.Center)
                || (left.Size != right.Size);
        }

        /// <summary>Creates a new <see cref="BoundingBox" /> instance with <see cref="Center" /> set to the specified value.</summary>
        /// <param name="value">The new center of the bounding box.</param>
        /// <returns>A new <see cref="BoundingBox" /> instance with <see cref="Center" /> set to <paramref name="value" />.</returns>
        public BoundingBox WithCenter(Vector3 value) => new BoundingBox(value, Size);

        /// <summary>Creates a new <see cref="BoundingBox" /> instance with <see cref="Size" /> set to the specified value.</summary>
        /// <param name="value">The new size of the bounding box.</param>
        /// <returns>A new <see cref="BoundingBox" /> instance with <see cref="Size" /> set to <paramref name="value" />.</returns>
        public BoundingBox WithY(Vector3 value) => new BoundingBox(Center, value);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is BoundingBox other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(BoundingBox other) => this == other;

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(Center);
                hashCode.Add(Size);
            }
            return hashCode.ToHashCode();
        }

        /// <inheritdoc />
        public override string ToString() => ToString(format: null, formatProvider: null);

        /// <inheritdoc />
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            return new StringBuilder(5 + separator.Length)
                .Append('<')
                .Append(Center.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(Size.ToString(format, formatProvider))
                .Append('>')
                .ToString();
        }
    }
}
