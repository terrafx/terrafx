// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Text;
using NUnit.Framework;

namespace TerraFX.Utilities.UnitTests
{
    /// <summary>Provides a set of tests covering the <see cref="InteropUtilities" /> static class.</summary>
    [TestFixture(Author = "Tanner Gooding", TestOf = typeof(InteropUtilities))]
    public static unsafe class InteropUtilitiesTests
    {
        /// <summary>Provides validation of the <see cref="InteropUtilities.MarshalUtf8ToReadOnlySpan(sbyte*, int)" /> static method.</summary>
        [TestCase(null, -1, null)]
        [TestCase(null, +3, null)]
        [TestCase("", -1, "")]
        [TestCase("", +3, "")]
        [TestCase("param", -1, "param")]
        [TestCase("param", +3, "par")]
        [TestCase("para\0m", -1, "para")]
        [TestCase("para\0m", +3, "par")]
        public static void MarshalUtf8ToReadOnlySpan(string value, int maxLength, string expectedResult)
        {
            var span = InteropUtilities.MarshalStringToUtf8(value);
            var result = InteropUtilities.MarshalUtf8ToReadOnlySpan(span.AsPointer(), maxLength).AsString();
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>Provides validation of the <see cref="InteropUtilities.MarshalUtf16ToReadOnlySpan(ushort*, int)" /> static method.</summary>
        [TestCase(null, -1, null)]
        [TestCase(null, +3, null)]
        [TestCase("", -1, "")]
        [TestCase("", +3, "")]
        [TestCase("param", -1, "param")]
        [TestCase("param", +3, "par")]
        [TestCase("para\0m", -1, "para")]
        [TestCase("para\0m", +3, "par")]
        public static void MarshalUtf16ToReadOnlySpan(string value, int maxLength, string expectedResult)
        {
            var span = InteropUtilities.MarshalStringToUtf16(value);
            var result = InteropUtilities.MarshalUtf16ToReadOnlySpan(span.AsPointer(), maxLength).AsString();
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
