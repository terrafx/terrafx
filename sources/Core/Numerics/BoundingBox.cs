// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

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

        /// <summary>Compares two bounding boxes to determine equality.</summary>
        /// <param name="left">The bounding box to compare with <paramref name="right" />.</param>
        /// <param name="right">The bounding box to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(BoundingBox left, BoundingBox right)
            => (left.Center == right.Center)
            && (left.Size == right.Size);

        /// <summary>Compares two bounding boxes instances to determine inequality.</summary>
        /// <param name="left">The bounding box to compare with <paramref name="right" />.</param>
        /// <param name="right">The bounding box to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(BoundingBox left, BoundingBox right)
            => (left.Center != right.Center)
            || (left.Size != right.Size);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is BoundingBox other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(BoundingBox other) => this == other;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Center, Size);

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

        /// <summary>Creates a new <see cref="BoundingBox" /> instance with <see cref="Center" /> set to the specified value.</summary>
        /// <param name="center">The new center of the bounding box.</param>
        /// <returns>A new <see cref="BoundingBox" /> instance with <see cref="Center" /> set to <paramref name="center" />.</returns>
        public BoundingBox WithCenter(Vector3 center) => new BoundingBox(center, Size);

        /// <summary>Creates a new <see cref="BoundingBox" /> instance with <see cref="Size" /> set to the specified value.</summary>
        /// <param name="size">The new size of the bounding box.</param>
        /// <returns>A new <see cref="BoundingBox" /> instance with <see cref="Size" /> set to <paramref name="size" />.</returns>
        public BoundingBox WithSize(Vector3 size) => new BoundingBox(Center, size);
    }
}
