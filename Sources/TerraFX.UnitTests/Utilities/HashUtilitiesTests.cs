// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;

namespace TerraFX.Utilities.UnitTests
{
    /// <summary>Provides a set of tests covering the <see cref="HashUtilities" /> static class.</summary>
    [TestFixture(Author = "Tanner Gooding", TestOf = typeof(HashUtilities))]
    public static class HashUtilitiesTests
    {
        #region Method Tests
        /// <summary>Provides validation of the <see cref="HashUtilities.ComputeHashCode(byte[], int, int, int)" /> method.</summary>
        [Test]
        public static void ComputeHashCodeByteArrayInt32Int32Int32Test()
        {
            Assert.That(() => HashUtilities.ComputeHashCode(null as byte[], 0, 0, 0),
                Throws.InstanceOf<ArgumentNullException>()
                      .With.Property("ParamName").EqualTo("values")
            );

            Assert.That(() => HashUtilities.ComputeHashCode(Array.Empty<byte>(), -1, 0, 0),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("offset")
                      .And.With.Property("ActualValue").EqualTo(-1)
            );

            Assert.That(() => HashUtilities.ComputeHashCode(Array.Empty<byte>(), 1, 0, 0),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("offset")
                      .And.With.Property("ActualValue").EqualTo(1)
            );

            Assert.That(() => HashUtilities.ComputeHashCode(Array.Empty<byte>(), 0, -1, 0),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("count")
                      .And.With.Property("ActualValue").EqualTo(-1)
            );

            Assert.That(() => HashUtilities.ComputeHashCode(Array.Empty<byte>(), 0, 1, 0),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("count")
                      .And.With.Property("ActualValue").EqualTo(1)
            );

            Assert.That(HashUtilities.ComputeHashCode(Array.Empty<byte>(), 0, 0, 0),
                Is.EqualTo(0)
            );
        }

        /// <summary>Provides validation of the <see cref="HashUtilities.ComputeHashCode(int[], int, int, int)" /> method.</summary>
        [Test]
        public static void ComputeHashCodeInt32ArrayInt32Int32Int32Test()
        {
            Assert.That(() => HashUtilities.ComputeHashCode(null as int[], 0, 0, 0),
                Throws.InstanceOf<ArgumentNullException>()
                      .With.Property("ParamName").EqualTo("values")
            );

            Assert.That(() => HashUtilities.ComputeHashCode(Array.Empty<int>(), -1, 0, 0),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("offset")
                      .And.With.Property("ActualValue").EqualTo(-1)
            );

            Assert.That(() => HashUtilities.ComputeHashCode(Array.Empty<int>(), 1, 0, 0),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("offset")
                      .And.With.Property("ActualValue").EqualTo(1)
            );

            Assert.That(() => HashUtilities.ComputeHashCode(Array.Empty<int>(), 0, -1, 0),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("count")
                      .And.With.Property("ActualValue").EqualTo(-1)
            );

            Assert.That(() => HashUtilities.ComputeHashCode(Array.Empty<int>(), 0, 1, 0),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("count")
                      .And.With.Property("ActualValue").EqualTo(1)
            );

            Assert.That(HashUtilities.ComputeHashCode(Array.Empty<int>(), 0, 0, 0),
                Is.EqualTo(0)
            );
        }

        /// <summary>Provides basic validation that the methods in the <see cref="HashUtilities" /> static class match the behavior expected by the Murmur3 algorithm.</summary>
        [Test]
        public static void Murmur3SanityTest()
        {
            var key = new byte[256];
            var hashes = new int[256];

            for (var index = 0; index < 256; index++)
            {
                key[index] = (byte)(index);
                hashes[index] = HashUtilities.ComputeHashCode(key, 0, index, (256 - index));
            }

            Assert.That(HashUtilities.ComputeHashCode(hashes, 0, hashes.Length, 0),
                Is.EqualTo(-1326088477)
            );
        }
        #endregion
    }
}
