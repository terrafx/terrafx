// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;

namespace TerraFX.Numerics
{
    /// <summary>Defines a Transform that maps points and normals from one coordinate system to another.
    /// It assumes that the rotation center of the source coordinate system is at it's origin.
    /// It maps via
    /// 1) anisotropic scaling
    /// 2) rotation about (0,0,0)
    /// 3) translation.</summary>
    public readonly struct Transform : IEquatable<Transform>, IFormattable
    {
        /// <summary>Defines a <see cref="Transform" /> that represents the Identity mapping.</summary>
        public static readonly Transform Identity = new Transform(Quaternion.Identity, Vector3.One, Vector3.Zero);

        /// <summary>Defines a <see cref="Transform" /> that has zeros for all components.</summary>
        public static readonly Transform Zero = new Transform(Quaternion.Zero, Vector3.Zero, Vector3.Zero);

        /// <summary>Defines a <see cref="Transform" /> that has ones for all components.</summary>
        public static readonly Transform One = new Transform(Quaternion.One, Vector3.One, Vector3.One);

        /// <summary>The rotation from source space to target space.</summary>
        private readonly Quaternion _rotation;

        /// <summary>The scaling along x, y and z to apply before rotation when mapping to the target coordinate space.</summary>
        private readonly Vector3 _scale;

        /// <summary>Rotation center in target space, typically in world space coordinates.</summary>
        private readonly Vector3 _translation;

        /// <summary>Initializes a new instance of the <see cref="Transform" /> struct from individual components.</summary>
        public Transform(Quaternion rotation, Vector3 scale, Vector3 translation)
        {
            _rotation = Quaternion.Normalize(rotation);
            _scale = scale;
            _translation = translation;
        }

        /// <summary>Initializes a new instance of the <see cref="Transform" /> struct from a tuple of its components</summary>
        public Transform((Quaternion rotation, Vector3 scale, Vector3 translation) components)
        {
            _rotation = Quaternion.Normalize(components.rotation);
            _scale = components.scale;
            _translation = components.translation;
        }

        /// <summary>The rotation from source space to target space.</summary>
        public Quaternion Rotation => _rotation;

        /// <summary>The scaling along x, y and z to apply before rotation when mapping to the target coordinate space.</summary>
        public Vector3 Scale => _scale;

        /// <summary>Rotation center in target space, typically in world space coordinates.</summary>
        public Vector3 Translation => _translation;

        /// <summary>Compares two <see cref="Transform" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Transform" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Transform" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Transform left, Transform right)
        {
            return (left.Scale == right.Scale)
                && (left.Rotation == right.Rotation)
                && (left.Translation == right.Translation);
        }

        /// <summary>Compares two <see cref="Transform" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Transform" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Transform" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Transform left, Transform right)
        {
            return (left.Scale != right.Scale)
                || (left.Rotation != right.Rotation)
                || (left.Translation != right.Translation);
        }

        /// <summary>Returns the value of the <see cref="Transform" /> operand (the sign of the operand is unchanged).</summary>
        /// <param name="value">The operand to return</param>
        /// <returns>The value of the operand, <paramref name="value" />.</returns>
        public static Transform operator +(Transform value) => value;

        /// <summary>Negates the value of the specified <see cref="Transform" /> operand.</summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The result of <paramref name="value" /> negated.</returns>
        public static Transform operator -(Transform value) => new Transform(-value.Rotation, -value.Scale, -value.Translation);

        /// <summary>Adds two specified <see cref="Transform" /> values.</summary>
        /// <param name="left">The first value to add.</param>
        /// <param name="right">The second value to add.</param>
        /// <returns>The result of adding <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Transform operator +(Transform left, Transform right) => new Transform(left.Rotation + right.Rotation, left.Scale + right.Scale, left.Translation + right.Translation);

        /// <summary>Subtracts two specified <see cref="Transform" /> values.</summary>
        /// <param name="left">The minuend.</param>
        /// <param name="right">The subtrahend.</param>
        /// <returns>The result of subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        public static Transform operator -(Transform left, Transform right) => new Transform(left.Rotation - right.Rotation, left.Scale - right.Scale, left.Translation - right.Translation);

        /// <summary>Multiplies each component of a <see cref="Transform" /> value by a given <see cref="float" /> value.</summary>
        /// <param name="left">The first value to multiply.</param>
        /// <param name="right">The value to multiply each component by.</param>
        /// <returns>The result of multiplying each component of <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Transform operator *(Transform left, float right) => new Transform(left.Rotation * right, left.Scale * right, left.Translation * right);

        /// <summary>Multiplies two specified <see cref="Transform" /> values.</summary>
        /// <param name="left">The first value to multiply.</param>
        /// <param name="right">The second value to multiply.</param>
        /// <returns>The result of multiplying <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Transform operator *(Transform left, Transform right) => new Transform(left.Rotation * right.Rotation, left.Scale * right.Scale, left.Translation * right.Translation);

        /// <summary>Divides two specified <see cref="Transform" /> values.</summary>
        /// <param name="left">The dividend.</param>
        /// <param name="right">The divisor.</param>
        /// <returns>The result of dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Transform operator /(Transform left, Transform right) => new Transform(left.Rotation / right.Rotation, left.Scale / right.Scale, left.Translation / right.Translation);

        /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the given axis in the target coordinate system.</summary>
        /// <param name="t">The transform for this operation.</param>
        /// <param name="axisInDst">The rotation axis in the destinatian space. Does not have to be normalized.</param>
        /// <param name="radians">The desired rotation angle in radians.</param>
        /// <returns>A new Transform with the rotation appended.</returns>
        public static Transform AddRotationAroundTargetAxis(Transform t, Vector3 axisInDst, float radians) => t.WithRotation(Quaternion.Concatenate(t.Rotation, Quaternion.CreateFromAxisAngle(axisInDst, radians)));

        /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the source X axis.</summary>
        /// <param name="t">The transform for this operation.</param>
        /// <param name="radians">The desired rotation angle in radians.</param>
        /// <returns>A new Transform with the rotation appended.</returns>
        public static Transform AddRotationAroundSourceX(Transform t, float radians) => AddRotationAroundTargetAxis(t, MapDirection(t, new Vector3(1, 0, 0)), radians);

        /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the source Y axis.</summary>
        /// <param name="t">The transform for this operation.</param>
        /// <param name="radians">The desired rotation angle in radians.</param>
        /// <returns>A new Transform with the rotation appended.</returns>
        public static Transform AddRotationAroundSourceY(Transform t, float radians) => AddRotationAroundTargetAxis(t, MapDirection(t, new Vector3(0, 1, 0)), radians);

        /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the source Z axis.</summary>
        /// <param name="t">The transform for this operation.</param>
        /// <param name="radians">The desired rotation angle in radians.</param>
        /// <returns>A new Transform with the rotation appended.</returns>
        public static Transform AddRotationAroundSourceZ(Transform t, float radians) => AddRotationAroundTargetAxis(t, MapDirection(t, new Vector3(0, 0, 1)), radians);

        /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the target X axis.</summary>
        /// <param name="t">The transform for this operation.</param>
        /// <param name="radians">The desired rotation angle in radians.</param>
        /// <returns>A new Transform with the rotation appended.</returns>
        public static Transform AddRotationAroundTargetX(Transform t, float radians) => AddRotationAroundTargetAxis(t, new Vector3(1, 0, 0), radians);

        /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the target Y axis.</summary>
        /// <param name="t">The transform for this operation.</param>
        /// <param name="radians">The desired rotation angle in radians.</param>
        /// <returns>A new Transform with the rotation appended.</returns>
        public static Transform AddRotationAroundTargetY(Transform t, float radians) => AddRotationAroundTargetAxis(t, new Vector3(0, 1, 0), radians);

        /// <summary>A new <see cref="Transform" /> with an incrementally added rotation about the target Z axis.</summary>
        /// <param name="t">The transform for this operation.</param>
        /// <param name="radians">The desired rotation angle in radians.</param>
        /// <returns>A new Transform with the rotation appended.</returns>
        public static Transform AddRotationAroundTargetZ(Transform t, float radians) => AddRotationAroundTargetAxis(t, new Vector3(0, 0, 1), radians);

        /// <summary>
        /// Creates a new Transform with the combined effect of this one and the given next one.
        /// So the this mapping is done first, the next mapping second.
        /// </summary>
        /// <param name="t">The transform for this operation.</param>
        /// <param name="next">The Transform who's mapping is to be appended to the current one.</param>
        /// <returns>The combined Transform</returns>
        public static Transform Concatenate(Transform t, Transform next)
        {
            var newRotation = Quaternion.Concatenate(t.Rotation, next.Rotation);
            var newScale = t.Scale * next.Scale;
            var newTranslation = (Vector3.Rotate(t.Translation, next.Rotation) * next.Scale) + next.Translation;
            var resultTransform = new Transform(newRotation, newScale, newTranslation);
            return resultTransform;
        }

        /// <summary>
        /// Creates the inverse of this Transform.
        /// Adding it to this will yield the identity Transform
        /// </summary>
        /// <param name="t">The transform for this operation.</param>
        /// <returns>The inverted Transform.</returns>
        public static Transform Inverse(Transform t)
        {
            var inverseRotation = Quaternion.Invert(t.Rotation);
            var inverseScale = new Vector3(1.0f / t.Scale.X, 1.0f / t.Scale.Y, 1.0f / t.Scale.Z);
            var inverseTranslation = -t.Translation;
            var inverseTransform = new Transform(inverseRotation, inverseScale, inverseTranslation);
            return inverseTransform;
        }

        /// <summary>
        /// Maps a Vector3 direction from source to target space.
        /// Applies only the rotation part of the transformation.
        /// If the input is a normalized vector, then so will be the output.
        /// </summary>
        /// <param name="t">The transform for this operation.</param>
        /// <param name="directionSrc">The direction vector to map.</param>
        /// <returns>The resulting target direction vector.</returns>
        public static Vector3 MapDirection(Transform t, Vector3 directionSrc)
        {
            var rotated = Vector3.Rotate(directionSrc, t.Rotation);
            return rotated;
        }

        /// <summary>
        /// Maps a Vector3 direction from target to source space.
        /// Applies only the rotation part of the transformation.
        /// If the input is a normalized vector, then so will be the output.
        /// </summary>
        /// <param name="t">The transform for this operation.</param>
        /// <param name="directionDst">The direction vector to map.</param>
        /// <returns>The resulting target direction vector.</returns>
        public static Vector3 MapBackDirection(Transform t, Vector3 directionDst)
        {
            var rotated = Vector3.RotateInverse(directionDst, t.Rotation);
            return rotated;
        }

        /// <summary>
        /// Maps a Vector3 position from source to target space.
        /// Applies the full transformation of translation, scaling and rotation.
        /// </summary>
        /// <param name="t">The transform for this operation.</param>
        /// <param name="positionSource">The source position to map.</param>
        /// <returns>The resulting target position.</returns>
        public static Vector3 MapPosition(Transform t, Vector3 positionSource)
        {
            var scaled = positionSource * t.Scale;
            var rotated = Vector3.Rotate(scaled, t.Rotation);
            var atDstCenterOfRotation = rotated + t.Translation;
            return atDstCenterOfRotation;
        }

        /// <summary>
        /// Maps a Vector3 position from target to source space.
        /// Applies the full transformation of translation, scaling and rotation.
        /// </summary>
        /// <param name="t">The transform for this operation.</param>
        /// <param name="positionTarget">The source position to map.</param>
        /// <returns>The resulting target position.</returns>
        public static Vector3 MapBackPosition(Transform t, Vector3 positionTarget)
        {
            var atDstCenterOfRotation = positionTarget - t.Translation;
            var rotated = Vector3.RotateInverse(atDstCenterOfRotation, t.Rotation);
            var scaled = rotated / t.Scale;
            return scaled;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Transform other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Transform other) => this == other;

        /// <summary>Tests if two <see cref="Transform" /> instances (this and right) have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="right">The right instance to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns><c>True</c> if similar, <c>False</c> otherwise.</returns>
        public bool EqualEstimate(Transform right, Transform epsilon)
        {
            return Quaternion.EqualEstimate(Rotation, right.Rotation, epsilon.Rotation)
                && Vector3.EqualEstimate(Scale, right.Scale, epsilon.Scale)
                && Vector3.EqualEstimate(Translation, right.Translation, epsilon.Translation);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(Scale);
                hashCode.Add(Rotation);
                hashCode.Add(Translation);
            }
            return hashCode.ToHashCode();
        }

        /// <summary>Creates a new <see cref="Matrix4x4" />  representing the total transform of this Transform: translation, scaling and rotation.</summary>
        /// <param name="t">The transform for this operation.</param>
        /// <returns>The Matrix4x4 transform.</returns>
        public static Matrix4x4 ToMatrix4x4(Transform t)
        {
            var rotation3x3 = Quaternion.ToMatrix3x3(t.Rotation);
            var x = new Vector4(rotation3x3.X * t.Scale.X, 0.0f);
            var y = new Vector4(rotation3x3.Y * t.Scale.Y, 0.0f);
            var z = new Vector4(rotation3x3.Z * t.Scale.Z, 0.0f);
            var w = new Vector4(t.Translation.X, t.Translation.Y, t.Translation.Z, 1.0f);
            var total4x4 = new Matrix4x4(x, y, z, w);
            return total4x4;
        }

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
        /// <param name="value">The new value for the Rotaiton component.</param>
        /// <returns>A new <see cref="Transform" /> instance with <see cref="Rotation" /> set to <paramref name="value" />.</returns>
        public Transform WithRotation(Quaternion value) => new Transform(value, Scale, Translation);

        /// <summary>Creates a new <see cref="Transform" /> instance with <see cref="Scale" /> set to the specified value.</summary>
        /// <param name="value">The new value for the Scale component.</param>
        /// <returns>A new <see cref="Transform" /> instance with <see cref="Scale" /> set to <paramref name="value" />.</returns>
        public Transform WithScale(Vector3 value) => new Transform(Rotation, value, Translation);

        /// <summary>Creates a new <see cref="Transform" /> instance with <see cref="Translation" /> set to the specified value.</summary>
        /// <param name="value">The new value for the Translation component.</param>
        /// <returns>A new <see cref="Transform" /> instance with <see cref="Translation" /> set to <paramref name="value" />.</returns>
        public Transform WithTranslation(Vector3 value) => new Transform(Rotation, Scale, value);
    }
}
