// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using TerraFX.Numerics;
using TerraFX.Utilities;

namespace TerraFX.Graphics.Geometry3D
{
    /// <summary>Projection modes, such as orthogonal or perspective.</summary>
    public enum ProjectionMode
    {
        /// <summary>Orthogonal (parallel) projection mode.</summary>
        Orthogonal,
        /// <summary>Perspective projection mode.</summary>
        Perspective
    };

    /// <summary>Defines a Camera that is placed and oriented via its ToWorld Transform and can be set to parallel or prespective projection.</summary>
    public struct Camera : IEquatable<Camera>, IEqualEstimate<Camera>, IFormattable
    {
        private readonly Transform _toWorld;
        private readonly Vector2 _screenSize;
        private readonly Vector2 _clipRange;
        private readonly ProjectionMode _projectionMode;

        /// <summary>Initializes a new instance of the Camera struct.</summary>
        /// <param name="toWorld">The <see cref="Transform" /> positioning and orienting this Camera in the world coordinate system.</param>
        /// <param name="screenSize"></param>
        /// <param name="clipRange"></param>
        /// <param name="projectionMode">The Tetrahedron for the new instance.</param>
        public Camera(Transform toWorld, Vector2 screenSize, Vector2 clipRange, ProjectionMode projectionMode)
        {
            _toWorld = toWorld;
            _screenSize = screenSize;
            _clipRange = clipRange;
            _projectionMode = projectionMode;
        }

        /// <summary>Gets the ToWorld <see cref="Transform" /> of the instance.</summary>
        public Transform ToWorld => _toWorld;

        /// <summary>Gets the geometry of the instance.</summary>
        public Vector2 ScreenSize => _screenSize;

        /// <summary>Gets the range from near to far clip plane.</summary>
        public Vector2 ClipRange => _clipRange;

        /// <summary>Gets the geometry of the instance.</summary>
        public ProjectionMode ProjectionMode => _projectionMode;

        /// <summary>Compares two Camera instances to determine equality.</summary>
        /// <param name="left">The Camera to compare with <paramref name="right" />.</param>
        /// <param name="right">The Camera to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Camera left, Camera right) => left.ToWorld == right.ToWorld && left.ScreenSize == right.ScreenSize && left.ClipRange == right.ClipRange && left.ProjectionMode == right.ProjectionMode;

        /// <summary>Compares two Camera instances to determine inequality.</summary>
        /// <param name="left">The Camera to compare with <paramref name="right" />.</param>
        /// <param name="right">The Camera to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Camera left, Camera right) => !(left == right);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Camera other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Camera other) => this == other;

        /// <summary>Tests if two Camera instances (this and right) have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="right">The right instance to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns>True if similar, false otherwise.</returns>
        public bool EqualEstimate(Camera right, Camera epsilon)
        {
            return ToWorld.EqualEstimate(right.ToWorld, epsilon.ToWorld)
                && ScreenSize.EqualEstimate(right.ScreenSize, epsilon.ScreenSize)
                && ClipRange.EqualEstimate(right.ClipRange, epsilon.ClipRange)
                && ProjectionMode.Equals(right.ProjectionMode);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(ToWorld);
                hashCode.Add(ScreenSize);
                hashCode.Add(ClipRange);
                hashCode.Add(ProjectionMode);
            }
            return hashCode.ToHashCode();
        }

        /// <summary>The Projection Matrix.</summary>
        public Matrix4x4 ProjectionMatrix()
        {
            var n = ClipRange.X; // near clip plane
            var f = ClipRange.Y; // far clip plane
            var w = ScreenSize.X; // screen client area width
            var h = ScreenSize.Y; // screen client area height
            Matrix4x4 projectionMatrix = default;

            switch (ProjectionMode)
            {
                case ProjectionMode.Orthogonal:
                    projectionMatrix = new Matrix4x4(
                        new Vector4(2  / w, 0, 0, 0),
                        new Vector4(0, 2   / h, 0, 0),
                        new Vector4(0, 0, 1 / (f - n), -n / (f - n)),
                        new Vector4(0, 0, 1, 0)
                    );
                    break;
                case ProjectionMode.Perspective:
                    projectionMatrix = new Matrix4x4(
                        new Vector4(2 * n / w, 0, 0, 0),
                        new Vector4(0, 2 * n / h, 0, 0),
                        new Vector4(0, 0, f / (f - n), f / (f - n)),
                        new Vector4(0, 0, 1, 0)
                    );
                    break;
                default:
                    break;
            }

            return projectionMatrix;
        }

        /// <inheritdoc />
        public override string ToString() => ToString(format: null, formatProvider: null);

        /// <inheritdoc />
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            return new StringBuilder(5 + separator.Length)
                .Append('<')
                .Append("ToWorld" + ToWorld.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append("screen" + ScreenSize.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append("clip" + ClipRange.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(ProjectionMode.ToString())
                .Append('>')
                .ToString();
        }

        /// <summary>The View Matrix.</summary>
        public Matrix4x4 ViewMatrix() => Transform.ToMatrix4x4(Transform.Inverse(ToWorld));

        /// <summary>The concatenation of View and Projection Matrices,
        /// transposed to column major order as needed for GPU shaders.</summary>
        public Matrix4x4 ViewProjectionMatrixColumnMajor() => Matrix4x4.Transpose(ViewMatrix() * ProjectionMatrix());

        /// <summary>Creates a new <see cref="Camera" /> instance with ClipRange set to the specified value.</summary>
        /// <param name="value">The new ClipRange of the instance.</param>
        /// <returns>A new Camera instance with ClipRange set to <paramref name="value" />.</returns>
        public Camera WithClipRange(Vector2 value) => new Camera(ToWorld, ScreenSize, value, ProjectionMode);

        /// <summary>Creates a new <see cref="Camera" /> instance with the projection mode set to the specified value.</summary>
        /// <param name="value">The new perspective projection mode of the instance.</param>
        /// <returns>A new Camera instance with the projection mode set to <paramref name="value" />.</returns>
        public Camera WithIsPerspective(ProjectionMode value) => new Camera(ToWorld, ScreenSize, ClipRange, value);

        /// <summary>Creates a new <see cref="Camera" /> instance with ScreenSize set to the specified value.</summary>
        /// <param name="value">The new ScreenSize of the instance.</param>
        /// <returns>A new Camera instance with ScreenSize set to <paramref name="value" />.</returns>
        public Camera WithScreenSize(Vector2 value) => new Camera(ToWorld, value, ClipRange, ProjectionMode);

        /// <summary>Creates a new <see cref="Camera" /> instance with <see cref="Transform" /> set to the specified value.</summary>
        /// <param name="value">The new <see cref="Transform" /> of the instance.</param>
        /// <returns>A new Camera instance with <see cref="Transform" /> set to <paramref name="value" />.</returns>
        public Camera WithToWorld(Transform value) => new Camera(value, ScreenSize, ClipRange, ProjectionMode);
    }
}
