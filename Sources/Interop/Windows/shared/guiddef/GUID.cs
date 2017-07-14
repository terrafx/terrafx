// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\guiddef.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct GUID : IEquatable<GUID>, IFormattable
    {
        #region Fields
        #region struct
        [FieldOffset(0)]
        public uint Data1;

        [FieldOffset(4)]
        public ushort Data2;

        [FieldOffset(6)]
        public ushort Data3;

        [FieldOffset(8)]
        public _Data4_e__FixedBuffer Data4;
        #endregion

        [FieldOffset(0)]
        internal Guid _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="GUID" /> struct.</summary>
        /// <param name="value">The <see cref="Guid" /> used to initialize the instance.</param>
        public GUID(Guid value) : this()
        {
            _value = value;
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="GUID" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="GUID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="GUID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(GUID left, GUID right)
        {
            return (left._value == right._value);
        }

        /// <summary>Compares two <see cref="GUID" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="GUID" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="GUID" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(GUID left, GUID right)
        {
            return (left._value != right._value);
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="GUID" /> value to a <see cref="Guid" /> value.</summary>
        /// <param name="value">The <see cref="GUID" /> value to convert.</param>
        public static implicit operator Guid(GUID value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="Guid" /> value to a <see cref="GUID" /> value.</summary>
        /// <param name="value">The <see cref="Guid" /> value to convert.</param>
        public static implicit operator GUID(Guid value)
        {
            return new GUID(value);
        }
        #endregion

        #region System.IEquatable<GUID> Methods
        /// <summary>Compares a <see cref="GUID" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="GUID" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(GUID other)
        {
            var otherValue = other._value;
            return _value.Equals(otherValue);
        }
        #endregion

        #region System.IFormattable Methods
        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }
        #endregion

        #region System.Object Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="GUID" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is GUID other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString()
        {
            return _value.ToString();
        }
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _Data4_e__FixedBuffer
        {
            #region Fields
            public byte e0;

            public byte e1;

            public byte e2;

            public byte e3;

            public byte e4;

            public byte e5;

            public byte e6;

            public byte e7;
            #endregion

            #region Properties
            public byte this[int index]
            {
                get
                {
                    if ((uint)(index) > 7) // (index < 0) || (index > 7)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (byte* e = &e0)
                    {
                        return e[index];
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
