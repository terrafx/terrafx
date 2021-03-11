// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using TerraFX.Graphics;
using TerraFX.Numerics;
using TerraFX.Utilities;

namespace TerraFX.Samples
{
    /// <summary>Defines a Apperance via its T, ToWorld and AppearanceStyle.</summary>
    public struct Appearance<T> : IEquatable<Appearance<T>>, IEqualEstimate<Appearance<T>>, IFormattable where T : IEquatable<T>, IEqualEstimate<T>, IFormattable
    {
        private readonly T _model;
        private readonly AppearanceStyle _style;

        /// <summary>Initializes a new instance of the Apperance struct.</summary>
        /// <param name="model">The model for the new instance.</param>
        /// <param name="style">The <see cref="Style" /> for this instance.</param>
        public Appearance(T model, AppearanceStyle style)
        {
            _model = model;
            _style = style;
        }

        /// <summary>Gets the model of the instance.</summary>
        public T Model => _model;

        /// <summary>The  <see cref="Style" /> of this instance.</summary>
        public AppearanceStyle Style => _style;

        /// <summary>Compares two Apperance instances to determine equality.</summary>
        /// <param name="left">The Apperance to compare with <paramref name="right" />.</param>
        /// <param name="right">The Apperance to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Appearance<T> left, Appearance<T> right) => left.Model.Equals(right.Model) && left.Style == right.Style;

        /// <summary>Compares two Apperance instances to determine inequality.</summary>
        /// <param name="left">The Apperance to compare with <paramref name="right" />.</param>
        /// <param name="right">The Apperance to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Appearance<T> left, Appearance<T> right) => !(left == right);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Appearance<T> other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Appearance<T> other) => this == other;

        /// <summary>Tests if two Apperance instances (this and right) have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="right">The right instance to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns><c>true</c> if similar, <c>false</c> otherwise.</returns>
        public bool EqualEstimate(Appearance<T> right, Appearance<T> epsilon)
        {
            return Model.EqualEstimate(right.Model, epsilon.Model)
                && Style.EqualEstimate(right.Style, epsilon.Style);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(Model);
                hashCode.Add(Style);
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
                .Append("Model" + Model.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append("GfxStyle" + Style.ToString(format, formatProvider))
                .Append('>')
                .ToString();
        }

        /// <summary>Creates a new Apperance instance with <see cref="Size" /> set to the specified value.</summary>
        /// <param name="value">The new size of the instance.</param>
        /// <returns>A new Apperance instance with <see cref="Size" /> set to <paramref name="value" />.</returns>
        public Appearance<T> WithModel(T value) => new Appearance<T>(value, Style);

        /// <summary>Creates a new Apperance instance with <see cref="Size" /> set to the specified value.</summary>
        /// <param name="value">The new size of the instance.</param>
        /// <returns>A new Appearance instance with <see cref="Size" /> set to <paramref name="value" />.</returns>
        public Appearance<T> WithGfxStyle(AppearanceStyle value) => new Appearance<T>(Model, value);
    }
}
