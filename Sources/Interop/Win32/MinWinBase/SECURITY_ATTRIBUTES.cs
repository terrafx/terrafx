// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\minwinbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Globalization;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>Contains the <c>security descriptor</c> for an object and specifies whether the handle retrieved by specifying this struct is inheritable.</summary>
    unsafe public struct SECURITY_ATTRIBUTES : IEquatable<SECURITY_ATTRIBUTES>
    {
        #region Fields
        /// <summary>The size, in bytes, of this structure.</summary>
        public uint nLength;

        /// <summary>A pointer to a <c>SECURITY_DESCRIPTOR</c> struct that controls access to the object.</summary>
        public void* lpSecurityDescriptor;

        /// <summary>Specifies whether the returned handle is inherited when a new process is created.</summary>
        public BOOL bInheritHandle;
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="SECURITY_ATTRIBUTES" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="SECURITY_ATTRIBUTES" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="SECURITY_ATTRIBUTES" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(SECURITY_ATTRIBUTES left, SECURITY_ATTRIBUTES right)
        {
            return (left.nLength == right.nLength)
                && (left.lpSecurityDescriptor == right.lpSecurityDescriptor)
                && (left.bInheritHandle == right.bInheritHandle);
        }

        /// <summary>Compares two <see cref="SECURITY_ATTRIBUTES" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="SECURITY_ATTRIBUTES" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="SECURITY_ATTRIBUTES" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(SECURITY_ATTRIBUTES left, SECURITY_ATTRIBUTES right)
        {
            return (left.nLength != right.nLength)
                || (left.lpSecurityDescriptor != right.lpSecurityDescriptor)
                || (left.bInheritHandle != right.bInheritHandle);
        }
        #endregion

        #region System.IEquatable<SECURITY_ATTRIBUTES>
        /// <summary>Compares a <see cref="SECURITY_ATTRIBUTES" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="SECURITY_ATTRIBUTES" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(SECURITY_ATTRIBUTES other)
        {
            return (this == other);
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="SECURITY_ATTRIBUTES" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is SECURITY_ATTRIBUTES other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            var combinedValue = 0;
            {
                combinedValue = HashUtilities.CombineValue(nLength.GetHashCode(), combinedValue);
                combinedValue = HashUtilities.CombineValue(((UIntPtr)(lpSecurityDescriptor)).GetHashCode(), combinedValue);
                combinedValue = HashUtilities.CombineValue(bInheritHandle.GetHashCode(), combinedValue);
            }
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
                stringBuilder.Append(nLength);
                stringBuilder.Append(separator);
                stringBuilder.Append(' ');
                stringBuilder.Append(((UIntPtr)(lpSecurityDescriptor)).ToString());
                stringBuilder.Append(separator);
                stringBuilder.Append(' ');
                stringBuilder.Append(bInheritHandle.ToString());
                stringBuilder.Append('>');
            }
            return stringBuilder.ToString();
        }
        #endregion
    }
}
