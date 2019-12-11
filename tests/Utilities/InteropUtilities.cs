// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Text;
using NUnit.Framework;

namespace TerraFX.Utilities.UnitTests
{
    /// <summary>Provides a set of tests covering the <see cref="InteropUtilities" /> static class.</summary>
    [TestFixture(Author = "Tanner Gooding", TestOf = typeof(InteropUtilities))]
    public static unsafe class InteropUtilitiesTests
    {
        /// <summary>Provides validation of the <see cref="InteropUtilities.MarshalNullTerminatedStringUtf8(sbyte*, int)" /> static method.</summary>
        [TestCase("param", -1, "param")]
        [TestCase("param", +3, "par")]
        [TestCase("para\0m", -1, "para")]
        [TestCase("para\0m", +3, "par")]
        public static void MarshalNullTerminatedStringUtf8(string value, int maxLength, string expectedResult)
        {
            string? result;

            fixed (byte* pValue = Encoding.UTF8.GetBytes(value))
            {
                result = InteropUtilities.MarshalNullTerminatedStringUtf8((sbyte*)pValue, maxLength);
            }

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>Provides validation of the <see cref="InteropUtilities.MarshalNullTerminatedStringUtf16(ushort*, int)" /> static method.</summary>
        [TestCase(null, -1, null)]
        [TestCase(null, +3, null)]
        [TestCase("", -1, "")]
        [TestCase("", +3, "")]
        [TestCase("param", -1, "param")]
        [TestCase("param", +3, "par")]
        [TestCase("para\0m", -1, "para")]
        [TestCase("para\0m", +3, "par")]
        public static void MarshalNullTerminatedStringUtf16(string value, int maxLength, string expectedResult)
        {
            string? result;

            fixed (char* pValue = value)
            {
                result = InteropUtilities.MarshalNullTerminatedStringUtf16((ushort*)pValue, maxLength);
            }

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
