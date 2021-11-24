// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Text;

namespace TerraFX.Numerics;

/// <summary>Defines a transform.</summary>
public readonly struct Transform : IEquatable<Transform>, IFormattable
{
    /// <summary>Defines the identity transform.</summary>
    public static readonly Transform Identity = new Transform(Quaternion.Identity, Vector3.One, Vector3.Zero);

    /// <summary>Defines the all zeros transform.</summary>
    public static readonly Transform Zero = new Transform(Quaternion.Zero, Vector3.Zero, Vector3.Zero);

    /// <summary>Defines the all ones transform.</summary>
    public static readonly Transform One = new Transform(Quaternion.One, Vector3.One, Vector3.One);

    private readonly Quaternion _rotation;
    private readonly Vector3 _scale;
    private readonly Vector3 _translation;

    /// <summary>Initializes a new instance of the <see cref="Transform" /> struct.</summary>
    /// <param name="rotation">The rotation of the transform.</param>
    /// <param name="scale">The scale of the transform.</param>
    /// <param name="translation">The translation of the transform.</param>
    public Transform(Quaternion rotation, Vector3 scale, Vector3 translation)
    {
        _rotation = Quaternion.Normalize(rotation);
        _scale = scale;
        _translation = translation;
    }

    /// <summary>Gets the rotation of the transform.</summary>
    public Quaternion Rotation => _rotation;

    /// <summary>Gets the scale of the transform.</summary>
    public Vector3 Scale => _scale;

    /// <summary>Gets the translation of the transform.</summary>
    public Vector3 Translation => _translation;

    /// <summary>Compares two transforms to determine equality.</summary>
    /// <param name="left">The transform to compare with <paramref name="right" />.</param>
    /// <param name="right">The transform to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Transform left, Transform right)
        => (left.Scale == right.Scale)
        && (left.Rotation == right.Rotation)
        && (left.Translation == right.Translation);

    /// <summary>Compares two transforms to determine inequality.</summary>
    /// <param name="left">The transform to compare with <paramref name="right" />.</param>
    /// <param name="right">The transform to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Transform left, Transform right)
        => (left.Scale != right.Scale)
        || (left.Rotation != right.Rotation)
        || (left.Translation != right.Translation);

    /// <summary>Computes the value of a transform.</summary>
    /// <param name="value">The transform.</param>
    /// <returns><paramref name="value" /></returns>
    public static Transform operator +(Transform value) => value;

    /// <summary>Computes the negation of a transform.</summary>
    /// <param name="value">The transform to negate.</param>
    /// <returns>The negation of <paramref name="value" />.</returns>
    public static Transform operator -(Transform value) => new Transform(
        -value.Rotation,
        -value.Scale,
        -value.Translation
    );

    /// <summary>Computes the sum of two transforms.</summary>
    /// <param name="left">The transform to which to add <paramref name="right" />.</param>
    /// <param name="right">The transform which is added to <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="right" /> added to <paramref name="left" />.</returns>
    public static Transform operator +(Transform left, Transform right) => new Transform(
        left.Rotation + right.Rotation,
        left.Scale + right.Scale,
        left.Translation + right.Translation
    );

    /// <summary>Computes the difference of two transforms.</summary>
    /// <param name="left">The transform from which to subtract <paramref name="right" />.</param>
    /// <param name="right">The transform which is subtracted from <paramref name="left" />.</param>
    /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
    public static Transform operator -(Transform left, Transform right) => new Transform(
        left.Rotation - right.Rotation,
        left.Scale - right.Scale,
        left.Translation - right.Translation
    );

    /// <summary>Computes the product of a transform and a float.</summary>
    /// <param name="left">The transform to multiply by <paramref name="right" />.</param>
    /// <param name="right">The float which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    public static Transform operator *(Transform left, float right) => new Transform(
        left.Rotation * right,
        left.Scale * right,
        left.Translation * right
    );

    /// <summary>Computes the product of two transforms.</summary>
    /// <param name="left">The transform to multiply by <paramref name="right" />.</param>
    /// <param name="right">The transform which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    public static Transform operator *(Transform left, Transform right) => new Transform(
        left.Rotation * right.Rotation,
        left.Scale * right.Scale,
        left.Translation * right.Translation
    );

    /// <summary>Computes the quotient of two transforms.</summary>
    /// <param name="left">The transform which is divied by <paramref name="right" />.</param>
    /// <param name="right">The transform which divides <paramref name="left" />.</param>
    /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    public static Transform operator /(Transform left, Transform right) => new Transform(
        left.Rotation / right.Rotation,
        left.Scale / right.Scale,
        left.Translation / right.Translation
    );

    /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the given axis in the target coordinate system.</summary>
    /// <param name="value">The transform for this operation.</param>
    /// <param name="axisInDst">The rotation axis in the destinatian space. Does not have to be normalized.</param>
    /// <param name="radians">The desired rotation angle in radians.</param>
    /// <returns>A new Transform with the rotation appended.</returns>
    public static Transform AddRotationAroundTargetAxis(Transform value, Vector3 axisInDst, float radians) => value.WithRotation(Quaternion.Concatenate(value.Rotation, Quaternion.CreateFromAxisAngle(axisInDst, radians)));

    /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the source X axis.</summary>
    /// <param name="value">The transform for this operation.</param>
    /// <param name="radians">The desired rotation angle in radians.</param>
    /// <returns>A new Transform with the rotation appended.</returns>
    public static Transform AddRotationAroundSourceX(Transform value, float radians) => AddRotationAroundTargetAxis(value, MapDirection(value, new Vector3(1, 0, 0)), radians);

    /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the source Y axis.</summary>
    /// <param name="value">The transform for this operation.</param>
    /// <param name="radians">The desired rotation angle in radians.</param>
    /// <returns>A new Transform with the rotation appended.</returns>
    public static Transform AddRotationAroundSourceY(Transform value, float radians) => AddRotationAroundTargetAxis(value, MapDirection(value, new Vector3(0, 1, 0)), radians);

    /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the source Z axis.</summary>
    /// <param name="value">The transform for this operation.</param>
    /// <param name="radians">The desired rotation angle in radians.</param>
    /// <returns>A new Transform with the rotation appended.</returns>
    public static Transform AddRotationAroundSourceZ(Transform value, float radians) => AddRotationAroundTargetAxis(value, MapDirection(value, new Vector3(0, 0, 1)), radians);

    /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the target X axis.</summary>
    /// <param name="value">The transform for this operation.</param>
    /// <param name="radians">The desired rotation angle in radians.</param>
    /// <returns>A new Transform with the rotation appended.</returns>
    public static Transform AddRotationAroundTargetX(Transform value, float radians) => AddRotationAroundTargetAxis(value, new Vector3(1, 0, 0), radians);

    /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the target Y axis.</summary>
    /// <param name="value">The transform for this operation.</param>
    /// <param name="radians">The desired rotation angle in radians.</param>
    /// <returns>A new Transform with the rotation appended.</returns>
    public static Transform AddRotationAroundTargetY(Transform value, float radians) => AddRotationAroundTargetAxis(value, new Vector3(0, 1, 0), radians);

    /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the target Z axis.</summary>
    /// <param name="value">The transform for this operation.</param>
    /// <param name="radians">The desired rotation angle in radians.</param>
    /// <returns>A new Transform with the rotation appended.</returns>
    public static Transform AddRotationAroundTargetZ(Transform value, float radians) => AddRotationAroundTargetAxis(value, new Vector3(0, 0, 1), radians);

    /// <summary>Computes the concatenation of two transforms.</summary>
    /// <param name="left">The transform on which to concatenate <paramref name="right" />.</param>
    /// <param name="right">The transform which is concatenated onto <paramref name="left" />.</param>
    /// <returns>The concatenation of <paramref name="right" /> onto <paramref name="left" />.</returns>
    /// <remarks>This methods is the same as computing <c><paramref name="right" /> * <paramref name="left" /></c>.</remarks>
    public static Transform Concatenate(Transform left, Transform right)
    {
        var newRotation = Quaternion.Concatenate(left.Rotation, right.Rotation);
        var newScale = left.Scale * right.Scale;
        var newTranslation = (Vector3.Rotate(left.Translation, right.Rotation) * right.Scale) + right.Translation;
        return new Transform(newRotation, newScale, newTranslation);
    }

    /// <summary>Compares two transforms to determine approximate equality.</summary>
    /// <param name="left">The transform to compare with <paramref name="right" />.</param>
    /// <param name="right">The transform to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (exclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    public static bool EqualsEstimate(Transform left, Transform right, Transform epsilon)
        => Quaternion.EqualsEstimate(left._rotation, right._rotation, epsilon._rotation)
        && Vector3.EqualsEstimate(left._scale, right._scale, epsilon._scale)
        && Vector3.EqualsEstimate(left._translation, right._translation, epsilon._translation);

    /// <summary>Computes the inverse of a transform.</summary>
    /// <param name="value">The transform to invert.</param>
    /// <returns>The inverse of <paramref name="value" />.</returns>
    public static Transform Invert(Transform value)
    {
        var inverseRotation = Quaternion.Invert(value.Rotation);
        var inverseScale = new Vector3(1.0f / value.Scale.X, 1.0f / value.Scale.Y, 1.0f / value.Scale.Z);
        var inverseTranslation = -value.Translation;
        return new Transform(inverseRotation, inverseScale, inverseTranslation);
    }

    /// <summary>
    /// Maps a Vector3 direction from source to target space.
    /// Applies only the rotation part of the transformation.
    /// If the input is a normalized vector, then so will be the output.
    /// </summary>
    /// <param name="value">The transform for this operation.</param>
    /// <param name="directionSrc">The direction vector to map.</param>
    /// <returns>The resulting target direction vector.</returns>
    public static Vector3 MapDirection(Transform value, Vector3 directionSrc)
    {
        var rotated = Vector3.Rotate(directionSrc, value.Rotation);
        return rotated;
    }

    /// <summary>
    /// Maps a Vector3 direction from target to source space.
    /// Applies only the rotation part of the transformation.
    /// If the input is a normalized vector, then so will be the output.
    /// </summary>
    /// <param name="value">The transform for this operation.</param>
    /// <param name="directionDst">The direction vector to map.</param>
    /// <returns>The resulting target direction vector.</returns>
    public static Vector3 MapBackDirection(Transform value, Vector3 directionDst)
    {
        var rotated = Vector3.RotateInverse(directionDst, value.Rotation);
        return rotated;
    }

    /// <summary>
    /// Maps a Vector3 position from source to target space.
    /// Applies the full transformation of translation, scaling and rotation.
    /// </summary>
    /// <param name="value">The transform for this operation.</param>
    /// <param name="positionSource">The source position to map.</param>
    /// <returns>The resulting target position.</returns>
    public static Vector3 MapPosition(Transform value, Vector3 positionSource)
    {
        var scaled = positionSource * value.Scale;
        var rotated = Vector3.Rotate(scaled, value.Rotation);
        var atDstCenterOfRotation = rotated + value.Translation;
        return atDstCenterOfRotation;
    }

    /// <summary>
    /// Maps a Vector3 position from target to source space.
    /// Applies the full transformation of translation, scaling and rotation.
    /// </summary>
    /// <param name="value">The transform for this operation.</param>
    /// <param name="positionTarget">The source position to map.</param>
    /// <returns>The resulting target position.</returns>
    public static Vector3 MapBackPosition(Transform value, Vector3 positionTarget)
    {
        var atDstCenterOfRotation = positionTarget - value.Translation;
        var rotated = Vector3.RotateInverse(atDstCenterOfRotation, value.Rotation);
        var scaled = rotated / value.Scale;
        return scaled;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Transform other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(Transform other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Scale, Rotation, Translation);

    /// <inheritdoc />
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return new StringBuilder(7 + (separator.Length * 2))
            .Append('<')
            .Append("Scale" + Scale.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append("Rot." + Rotation.ToString(format, formatProvider))
            .Append(separator)
            .Append(' ')
            .Append("Trans." + Translation.ToString(format, formatProvider))
            .Append('>')
            .ToString();
    }

    /// <summary>Creates a new <see cref="Transform" /> instance with <see cref="Rotation" /> set to the specified value.</summary>
    /// <param name="rotation">The new value for the Rotaiton component.</param>
    /// <returns>A new <see cref="Transform" /> instance with <see cref="Rotation" /> set to <paramref name="rotation" />.</returns>
    public Transform WithRotation(Quaternion rotation) => new Transform(rotation, Scale, Translation);

    /// <summary>Creates a new <see cref="Transform" /> instance with <see cref="Scale" /> set to the specified value.</summary>
    /// <param name="scale">The new value for the Scale component.</param>
    /// <returns>A new <see cref="Transform" /> instance with <see cref="Scale" /> set to <paramref name="scale" />.</returns>
    public Transform WithScale(Vector3 scale) => new Transform(Rotation, scale, Translation);

    /// <summary>Creates a new <see cref="Transform" /> instance with <see cref="Translation" /> set to the specified value.</summary>
    /// <param name="translation">The new value for the Translation component.</param>
    /// <returns>A new <see cref="Transform" /> instance with <see cref="Translation" /> set to <paramref name="translation" />.</returns>
    public Transform WithTranslation(Vector3 translation) => new Transform(Rotation, Scale, translation);
}
