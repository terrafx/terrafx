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
        /// <summary>Provides basic validation that the methods in the <see cref="HashUtilities" /> static class match the behavior expected by the Murmur3 algorithm.</summary>
        [Test]
        public static void Murmur3SanityTest()
        {
            var key = new byte[256];
            var hashes = new int[256];

            for (var index = 0; index < 256; index++)
            {
                key[index] = (byte)index;
                hashes[index] = HashUtilities.ComputeHashCode(key.AsSpan(0, index), 256 - index);
            }

            Assert.That(HashUtilities.ComputeHashCode(hashes.AsSpan(0, hashes.Length), 0),
                Is.EqualTo(-1326088477)
            );
        }
        #endregion
    }
}
