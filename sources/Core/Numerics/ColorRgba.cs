// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;
using static TerraFX.Utilities.VectorUtilities;

namespace TerraFX.Numerics;

/// <summary>Defines a red-green-blue-alpha color triple.</summary>
public readonly struct ColorRgba
    : IEquatable<ColorRgba>,
      IFormattable,
      ISpanFormattable,
      IUtf8SpanFormattable
{
    private readonly Vector128<float> _value;

    /// <summary>Initializes a new instance of the <see cref="ColorRgba" /> struct.</summary>
    /// <param name="red">The value of the red component.</param>
    /// <param name="green">The value of the green component.</param>
    /// <param name="blue">The value of the blue component.</param>
    /// <param name="alpha">The value of the alpha component.</param>
    public ColorRgba(float red, float green, float blue, float alpha)
    {
        _value = Vector128.Create(red, green, blue, alpha);
    }

    /// <summary>Initializes a new instance of the <see cref="ColorRgba" /> struct.</summary>
    /// <param name="value">The value of the vector.</param>
    public ColorRgba(Vector128<float> value)
    {
        _value = value;
    }

    /// <summary>Gets the value of the red component.</summary>
    public float Red
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.ToScalar();
        }
    }

    /// <summary>Gets the value of the green component.</summary>
    public float Green
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.GetElement(1);
        }
    }

    /// <summary>Gets the value of the blue component.</summary>
    public float Blue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.GetElement(2);
        }
    }

    /// <summary>Gets the value of the alpha component.</summary>
    public float Alpha
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _value.GetElement(3);
        }
    }

    /// <summary>Compares two colors to determine equality.</summary>
    /// <param name="left">The color to compare with <paramref name="right" />.</param>
    /// <param name="right">The color to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(ColorRgba left, ColorRgba right) => CompareEqualAll(left._value, right._value);

    /// <summary>Compares two colors to determine equality.</summary>
    /// <param name="left">The color to compare with <paramref name="right" />.</param>
    /// <param name="right">The color to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(ColorRgba left, ColorRgba right) => CompareNotEqualAny(left._value, right._value);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ColorRgba other && Equals(other);

    /// <inheritdoc />
    public bool Equals(ColorRgba other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Red, Green, Blue, Alpha);

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format = null, IFormatProvider? formatProvider = null)
        => $"ColorRgba {{ Red = {Red.ToString(format, formatProvider)}, Green = {Green.ToString(format, formatProvider)}, Blue = {Blue.ToString(format, formatProvider)}, Alpha = {Alpha.ToString(format, formatProvider)} }}";

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"ColorRgba { Red = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        var partLength = "ColorRgba { Red = ".Length;

        numWritten += partLength;
        destination = destination.Slice(numWritten);

        if (!Red.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!", Green = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = ", Green = ".Length;

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!Green.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }


        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!", Blue = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = ", Blue = ".Length;

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!Blue.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!", Alpha = ".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = ", Alpha = ".Length;

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!Alpha.TryFormat(destination, out partLength, format, provider))
        {
            charsWritten = 0;
            return false;
        }

        numWritten += partLength;
        destination = destination.Slice(partLength);

        if (!" }".TryCopyTo(destination))
        {
            charsWritten = 0;
            return false;
        }
        partLength = " }".Length;

        charsWritten = numWritten + partLength;
        return true;
    }

    /// <inheritdoc />
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        var numWritten = 0;

        if (!"ColorRgba { Red = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        var partLength = "ColorRgba { Red = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(numWritten);

        if (!Red.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!", Green = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = ", Green = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!Green.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!", Blue = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = ", Blue = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!Blue.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!", Alpha = "u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = ", Alpha = "u8.Length;

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!Alpha.TryFormat(utf8Destination, out partLength, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        numWritten += partLength;
        utf8Destination = utf8Destination.Slice(partLength);

        if (!" }"u8.TryCopyTo(utf8Destination))
        {
            bytesWritten = 0;
            return false;
        }
        partLength = " }"u8.Length;

        bytesWritten = numWritten + partLength;
        return true;
    }

    /// <summary>Creates a new <see cref="ColorRgba" /> instance with <see cref="Red" /> set to the specified value.</summary>
    /// <param name="red">The new value of the red component.</param>
    /// <returns>A new <see cref="ColorRgba" /> instance with <see cref="Red" /> set to <paramref name="red" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ColorRgba WithRed(float red)
    {
        var result = _value.WithElement(0, red);
        return new ColorRgba(result);
    }

    /// <summary>Creates a new <see cref="ColorRgba" /> instance with <see cref="Green" /> set to the specified value.</summary>
    /// <param name="green">The new value of the green component.</param>
    /// <returns>A new <see cref="ColorRgba" /> instance with <see cref="Green" /> set to <paramref name="green" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ColorRgba WithGreen(float green)
    {
        var result = _value.WithElement(0, green);
        return new ColorRgba(result);
    }

    /// <summary>Creates a new <see cref="ColorRgba" /> instance with <see cref="Blue" /> set to the specified value.</summary>
    /// <param name="blue">The new value of the blue component.</param>
    /// <returns>A new <see cref="ColorRgba" /> instance with <see cref="Blue" /> set to <paramref name="blue" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ColorRgba WithBlue(float blue)
    {
        var result = _value.WithElement(0, blue);
        return new ColorRgba(result);
    }

    /// <summary>Creates a new <see cref="ColorRgba" /> instance with <see cref="Alpha" /> set to the specified value.</summary>
    /// <param name="alpha">The new value of the alpha component.</param>
    /// <returns>A new <see cref="ColorRgba" /> instance with <see cref="Alpha" /> set to <paramref name="alpha" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ColorRgba WithAlpha(float alpha)
    {
        var result = _value.WithElement(0, alpha);
        return new ColorRgba(result);
    }
}
