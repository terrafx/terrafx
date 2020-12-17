// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using TerraFX.Graphics;
using TerraFX.Numerics;
using TerraFX.Utilities;

namespace TerraFX.Samples
{
    /// <summary>Defines a GfxStyle.</summary>
    public readonly struct AppearanceStyle : IEquatable<AppearanceStyle>, IEqualEstimate<AppearanceStyle>, IFormattable
    {
        /// <summary>Defines a <see cref="AppearanceStyle" /> in standard red.</summary>
        public static readonly AppearanceStyle Red = new AppearanceStyle(Colors.Red);
        /// <summary>Defines a <see cref="AppearanceStyle" /> in standard green.</summary>
        public static readonly AppearanceStyle Green = new AppearanceStyle(Colors.Green);
        /// <summary>Defines a <see cref="AppearanceStyle" /> in standard blue.</summary>
        public static readonly AppearanceStyle Blue = new AppearanceStyle(Colors.Blue);

        private readonly ColorRgba _faceColorRgba;
        private readonly ColorRgba _lineColorRgba;
        private readonly float _lineThickness;
        private readonly bool _isVisible;
        private readonly bool _isEnabled;

        /// <summary>Initializes a new instance of the <see cref="AppearanceStyle" /> struct.</summary>
        /// <param name="faceColorRgba"></param>
        /// <param name="lineColorRgba"></param>
        /// <param name="lineThickness"></param>
        /// <param name="isVisible"></param>
        /// <param name="inEnabled"></param>
        public AppearanceStyle(
            ColorRgba faceColorRgba = default,
            ColorRgba lineColorRgba = default,
            float lineThickness = 1.0f,
            bool isVisible = true,
            bool inEnabled = true)
        {
            _faceColorRgba = faceColorRgba;
            _lineColorRgba = lineColorRgba;
            _lineThickness = lineThickness;
            _isVisible = isVisible;
            _isEnabled = inEnabled;
        }

        /// <summary>Gets the value of the face color rgba.</summary>
        public ColorRgba FaceColorRgba => _faceColorRgba;

        /// <summary>Gets the value of the line color rgba.</summary>
        public ColorRgba LineColorRgba => _lineColorRgba;

        /// <summary>Gets the value of the line thickness.</summary>
        public float LineThickness => _lineThickness;

        /// <summary>Gets the visibility status.</summary>
        public bool IsVisible => _isVisible;

        /// <summary>Gets the enabled status.</summary>
        public bool IsEnabled => _isEnabled;

        /// <summary>Compares two <see cref="AppearanceStyle" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="AppearanceStyle" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="AppearanceStyle" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(AppearanceStyle left, AppearanceStyle right)
        {
            return (left.FaceColorRgba == right.FaceColorRgba)
                && (left.LineColorRgba == right.LineColorRgba)
                && (left.LineThickness == right.LineThickness)
                && (left.IsVisible == right.IsVisible)
                && (left.IsEnabled == right.IsEnabled);
        }

        /// <summary>Compares two <see cref="AppearanceStyle" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="AppearanceStyle" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="AppearanceStyle" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(AppearanceStyle left, AppearanceStyle right) => !(left == right);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is AppearanceStyle other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(AppearanceStyle other) => this == other;

        /// <summary>Tests if two <see cref="AppearanceStyle" /> instances (this and right) have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="right">The right <see cref="AppearanceStyle" /> to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns><c>true</c> if similar, <c>false</c> otherwise.</returns>
        public bool EqualEstimate(AppearanceStyle right, AppearanceStyle epsilon)
        {
            return FaceColorRgba.EqualEstimate(right.FaceColorRgba, epsilon.FaceColorRgba)
                && LineColorRgba.EqualEstimate(right.LineColorRgba, epsilon.LineColorRgba)
                && LineThickness.EqualEstimate(right.LineThickness, epsilon.LineThickness)
                && IsEnabled.Equals(right.IsEnabled)
                && IsVisible.Equals(right.IsVisible);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(FaceColorRgba);
                hashCode.Add(LineColorRgba);
                hashCode.Add(LineThickness);
                hashCode.Add(IsVisible);
                hashCode.Add(IsEnabled);
            }
            return hashCode.ToHashCode();
        }

        /// <inheritdoc />
        public override string ToString() => ToString(format: null, formatProvider: null);

        /// <inheritdoc />
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            return new StringBuilder(12 + (separator.Length * 4))
                .Append('<')
                .Append(FaceColorRgba.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(LineColorRgba.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(LineThickness.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(IsVisible ? "Visible":"Hidden")
                .Append(separator)
                .Append(' ')
                .Append(IsEnabled ? "Enabled":"Disabled")
                .Append('>')
                .ToString();
        }

        /// <summary>Creates a new <see cref="AppearanceStyle" /> instance with <see cref="FaceColorRgba" /> set to the specified value.</summary>
        /// <param name="value">The new value to use.</param>
        /// <returns>A new <see cref="AppearanceStyle" /> instance with <see cref="FaceColorRgba" /> set to <paramref name="value" />.</returns>
        public AppearanceStyle WithFaceColorRgba(ColorRgba value) => new AppearanceStyle(value, LineColorRgba, LineThickness, IsVisible, IsEnabled);

        /// <summary>Creates a new <see cref="AppearanceStyle" /> instance with <see cref="FaceColorRgba" /> set to the specified value.</summary>
        /// <param name="value">The new value to use.</param>
        /// <returns>A new <see cref="AppearanceStyle" /> instance with <see cref="FaceColorRgba" /> set to <paramref name="value" />.</returns>
        public AppearanceStyle WithLineColorRgba(ColorRgba value) => new AppearanceStyle(FaceColorRgba, value, LineThickness, IsVisible, IsEnabled);

        /// <summary>Creates a new <see cref="AppearanceStyle" /> instance with <see cref="FaceColorRgba" /> set to the specified value.</summary>
        /// <param name="value">The new value to use.</param>
        /// <returns>A new <see cref="AppearanceStyle" /> instance with <see cref="FaceColorRgba" /> set to <paramref name="value" />.</returns>
        public AppearanceStyle WithLineThickness(float value) => new AppearanceStyle(FaceColorRgba, LineColorRgba, value, IsVisible, IsEnabled);

        /// <summary>Creates a new <see cref="AppearanceStyle" /> instance with <see cref="FaceColorRgba" /> set to the specified value.</summary>
        /// <param name="value">The new value to use.</param>
        /// <returns>A new <see cref="AppearanceStyle" /> instance with <see cref="FaceColorRgba" /> set to <paramref name="value" />.</returns>
        public AppearanceStyle WithIsVisible(bool value) => new AppearanceStyle(FaceColorRgba, LineColorRgba, LineThickness, value, IsEnabled);

        /// <summary>Creates a new <see cref="AppearanceStyle" /> instance with <see cref="FaceColorRgba" /> set to the specified value.</summary>
        /// <param name="value">The new value to use.</param>
        /// <returns>A new <see cref="AppearanceStyle" /> instance with <see cref="FaceColorRgba" /> set to <paramref name="value" />.</returns>
        public AppearanceStyle WithIsEnabled(bool value) => new AppearanceStyle(FaceColorRgba, LineColorRgba, LineThickness, IsVisible, value);
    }
}
