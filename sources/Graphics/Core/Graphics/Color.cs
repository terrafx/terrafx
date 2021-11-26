// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;

namespace TerraFX.Graphics;

/// <summary>Defines a red-green-blue color triple.</summary>
public readonly struct Color : IEquatable<Color>, IFormattable
{
    private readonly float _red;
    private readonly float _green;
    private readonly float _blue;

    /// <summary>Initializes a new instance of the <see cref="Color" /> struct.</summary>
    /// <param name="red">The value of the red component.</param>
    /// <param name="green">The value of the green component.</param>
    /// <param name="blue">The value of the blue component.</param>
    public Color(float red, float green, float blue)
    {
        _red = red;
        _green = green;
        _blue = blue;
    }

    /// <summary>Gets the value of the blue component.</summary>
    public float Blue => _blue;

    /// <summary>Gets the value of the green component.</summary>
    public float Green => _green;

    /// <summary>Gets the value of the red component.</summary>
    public float Red => _red;

    /// <summary>Compares two <see cref="Color" /> instances to determine equality.</summary>
    /// <param name="left">The <see cref="Color" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="Color" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Color left, Color right)
    {
        return (left.Red == right.Red)
            && (left.Green == right.Green)
            && (left.Blue == right.Blue);
    }

    /// <summary>Compares two <see cref="Color" /> instances to determine inequality.</summary>
    /// <param name="left">The <see cref="Color" /> to compare with <paramref name="right" />.</param>
    /// <param name="right">The <see cref="Color" /> to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Color left, Color right)
    {
        return (left.Red != right.Red)
            || (left.Green != right.Green)
            || (left.Blue != right.Blue);
    }

    /// <summary>Creates a new <see cref="Color" /> instance with <see cref="Blue" /> set to the specified value.</summary>
    /// <param name="value">The new value of the blue component.</param>
    /// <returns>A new <see cref="Color" /> instance with <see cref="Blue" /> set to <paramref name="value" />.</returns>
    public Color WithBlue(float value) => new Color(Red, Green, value);

    /// <summary>Creates a new <see cref="Color" /> instance with <see cref="Green" /> set to the specified value.</summary>
    /// <param name="value">The new value of the green component.</param>
    /// <returns>A new <see cref="Color" /> instance with <see cref="Green" /> set to <paramref name="value" />.</returns>
    public Color WithGreen(float value) => new Color(Red, value, Blue);

    /// <summary>Creates a new <see cref="Color" /> instance with <see cref="Red" /> set to the specified value.</summary>
    /// <param name="value">The new value of the red component.</param>
    /// <returns>A new <see cref="Color" /> instance with <see cref="Red" /> set to <paramref name="value" />.</returns>
    public Color WithRed(float value) => new Color(value, Green, Blue);

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Color other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(Color other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        {
            hashCode.Add(Red);
            hashCode.Add(Green);
            hashCode.Add(Blue);
        }
        return hashCode.ToHashCode();
    }

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return new StringBuilder(7 + (separator.Length * 2))
            .Append('<')
            .Append(Red.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append(Green.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append(Blue.ToString(format, formatProvider))
            .Append('>')
            .ToString();
    }
}
