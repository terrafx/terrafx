// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using static TerraFX.Utilities.HashUtilities;
using static TerraFX.Utilities.InteropUtilities;

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
        public Vector3 Center
        {
            get
            {
                return _center;
            }
        }

        /// <summary>Gets the size of the bounding box.</summary>
        public Vector3 Size
        {
            get
            {
                return _size;
            }
        }

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
        public BoundingBox WithCenter(Vector3 value)
        {
            return new BoundingBox(value, Size);
        }

        /// <summary>Creates a new <see cref="BoundingBox" /> instance with <see cref="Size" /> set to the specified value.</summary>
        /// <param name="value">The new size of the bounding box.</param>
        /// <returns>A new <see cref="BoundingBox" /> instance with <see cref="Size" /> set to <paramref name="value" />.</returns>
        public BoundingBox WithY(Vector3 value)
        {
            return new BoundingBox(Center, value);
        }

        /// <summary>Compares a <see cref="BoundingBox" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="BoundingBox" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(BoundingBox other)
        {
            return this == other;
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
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

        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="BoundingBox" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            return (obj is BoundingBox other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            var combinedValue = 0;
            {
                combinedValue = CombineValue(Center.GetHashCode(), combinedValue);
                combinedValue = CombineValue(Size.GetHashCode(), combinedValue);
            }
            return FinalizeValue(combinedValue, SizeOf<Vector3>() * 3);
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString()
        {
            return ToString(format: null, formatProvider: null);
        }
    }
}
