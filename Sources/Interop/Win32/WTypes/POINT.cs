// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Globalization;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>Defines the x and y-coordinates of a point.</summary>
    public struct POINT : IEquatable<POINT>
    {
        #region Fields
        /// <summary>The x-coorindate of the point.</summary>
        public int x;

        /// <summary>The y-coorindate of the point.</summary>
        public int y;
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="POINT" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="POINT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="POINT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(POINT left, POINT right)
        {
            return (left.x == right.x)
                && (left.y == right.y);
        }

        /// <summary>Compares two <see cref="POINT" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="POINT" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="POINT" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(POINT left, POINT right)
        {
            return (left.x != right.x)
                || (left.y != right.y);
        }
        #endregion

        #region System.IEquatable<POINT>
        /// <summary>Compares a <see cref="POINT" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="POINT" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(POINT other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="POINT" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is POINT other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            var combinedValue = HashUtilities.CombineValue(y.GetHashCode(), x.GetHashCode());
            return HashUtilities.FinalizeValue(combinedValue, (sizeof(int) * 2));
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString()
        {
            var separator = NumberFormatInfo.CurrentInfo.NumberGroupSeparator;

            var stringBuilder = new StringBuilder(5 + separator.Length);
            {
                stringBuilder.Append('<');
                stringBuilder.Append(x);
                stringBuilder.Append(separator);
                stringBuilder.Append(' ');
                stringBuilder.Append(y);
                stringBuilder.Append('>');
            }
            return stringBuilder.ToString();
        }
        #endregion
    }
}
