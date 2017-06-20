// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Globalization;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>Defines the coordinates of the upper-left and lower-right corners of a rectangle.</summary>
    public struct RECT : IEquatable<RECT>
    {
        #region Fields
        /// <summary>The x-coordinate of the upper-left corner of the rectangle.</summary>
        public int left;

        /// <summary>The y-coordinate of the upper-left corner of the rectangle.</summary>
        public int top;

        /// <summary>The x-coordinate of the lower-right corner of the rectangle.</summary>
        public int right;

        /// <summary>The y-coordinate of the lower-right corner of the rectangle.</summary>
        public int bottom;
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="RECT" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="RECT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="RECT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(RECT left, RECT right)
        {
            return (left.left == right.left)
                && (left.top == right.top)
                && (left.right == right.right)
                && (left.bottom == right.bottom);
        }

        /// <summary>Compares two <see cref="RECT" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="RECT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="RECT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(RECT left, RECT right)
        {
            return (left.left != right.left)
                || (left.top != right.top)
                || (left.right != right.right)
                || (left.bottom != right.bottom);
        }
        #endregion

        #region System.IEquatable<RECT>
        /// <summary>Compares a <see cref="RECT" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="RECT" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(RECT other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="RECT" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is RECT other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            var combinedValue = HashUtilities.CombineValue(top.GetHashCode(), left.GetHashCode());
            combinedValue = HashUtilities.CombineValue(right.GetHashCode(), combinedValue);
            combinedValue = HashUtilities.CombineValue(bottom.GetHashCode(), combinedValue);
            return HashUtilities.FinalizeValue(combinedValue, (sizeof(int) * 4));
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString()
        {
            var separator = NumberFormatInfo.CurrentInfo.NumberGroupSeparator;

            var stringBuilder = new StringBuilder(9 + (separator.Length * 3));
            {
                stringBuilder.Append('<');
                stringBuilder.Append(left);
                stringBuilder.Append(separator);
                stringBuilder.Append(' ');
                stringBuilder.Append(top);
                stringBuilder.Append(separator);
                stringBuilder.Append(' ');
                stringBuilder.Append(right);
                stringBuilder.Append(separator);
                stringBuilder.Append(' ');
                stringBuilder.Append(bottom);
                stringBuilder.Append('>');
            }
            return stringBuilder.ToString();
        }
        #endregion
    }
}
