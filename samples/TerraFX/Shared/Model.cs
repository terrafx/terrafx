// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using TerraFX.Numerics;
using TerraFX.Utilities;

namespace TerraFX.Samples
{
    /// <summary>Defines a 3D Tetrahedron that has a Model matrix in form of a ToWorld Transform.</summary>
    public struct Model<T> : IEquatable<Model<T>>, IEqualEstimate<Model<T>>, IFormattable where T : IEquatable<T>, IEqualEstimate<T>, IFormattable
    {
        private readonly T _shape;
        private readonly Transform _toWorld;

        /// <summary>Initializes a new instance of the Model struct.</summary>
        /// <param name="shape">The Tetrahedron for the new instance.</param>
        /// <param name="toWorld">The <see cref="Transform" /> positioning and orienting this Model in the world coordinate system.</param>
        public Model(T shape, Transform toWorld)
        {
            _shape = shape;
            _toWorld = toWorld;
        }

        /// <summary>Gets the geometry of the instance.</summary>
        public T Shape => _shape;

        /// <summary>Gets the ToWorld <see cref="Transform" /> of the instance.
        /// It maps the center of the Tetrahedron.</summary>
        public Transform ToWorld => _toWorld;

        /// <summary>Compares two Model instances to determine equality.</summary>
        /// <param name="left">The Model to compare with <paramref name="right" />.</param>
        /// <param name="right">The Model to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Model<T> left, Model<T> right) => left.Shape.Equals(right.Shape) && left.ToWorld == right.ToWorld;

        /// <summary>Compares two Model instances to determine inequality.</summary>
        /// <param name="left">The Model to compare with <paramref name="right" />.</param>
        /// <param name="right">The Model to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Model<T> left, Model<T> right) => !(left == right);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Model<T> other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Model<T> other) => this == other;

        /// <summary>Tests if two Model instances (this and right) have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="right">The right instance to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns>True if similar, false otherwise.</returns>
        public bool EqualEstimate(Model<T> right, Model<T> epsilon)
        {
            return Shape.EqualEstimate(right.Shape, epsilon.Shape)
                && ToWorld.EqualEstimate(right.ToWorld, epsilon.ToWorld);
        }



        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(Shape);
                hashCode.Add(ToWorld);
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
                .Append("Geom" + Shape.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append("ToWorld" + ToWorld.ToString(format, formatProvider))
                .Append('>')
                .ToString();
        }

        // -- new instance with some member changed (With*) --

        /// <summary>Creates a new Model instance with <see cref="Size" /> set to the specified value.</summary>
        /// <param name="value">The new size of the instance.</param>
        /// <returns>A new Model instance with <see cref="Size" /> set to <paramref name="value" />.</returns>
        public Model<T> WithGeometry(T value) => new Model<T>(value, ToWorld);

        /// <summary>Creates a new Model instance with <see cref="Size" /> set to the specified value.</summary>
        /// <param name="value">The new size of the instance.</param>
        /// <returns>A new Model instance with <see cref="Size" /> set to <paramref name="value" />.</returns>
        public Model<T> WithToWorld(Transform value) => new Model<T>(Shape, value);

        // -- math --

        /// <summary>Computes if there is any intersection/overlap between this Model and the other.</summary>
        /// <param name="other">The other Model to check against.</param>
        /// <returns><c>true</c> if intersecting, <c>false</c> otherwise.</returns>
        public bool IsIntersecting(Model<T> other) => throw new NotImplementedException();
    }
}
