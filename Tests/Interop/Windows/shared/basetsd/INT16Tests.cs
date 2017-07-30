// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using NUnit.Framework;

namespace TerraFX.Interop.UnitTests
{
    /// <summary>Provides validation of the <see cref="INT16" /> struct.</summary>
    public static class INT16Tests
    {
        /// <summary>Validates that the layout of the <see cref="INT16" /> struct is <see cref="LayoutKind.Sequential" />.</summary>
        [Test]
        public static void IsLayoutSequentialTest()
        {
            Assert.That(typeof(INT16).IsLayoutSequential, Is.True);
        }

        /// <summary>Validates that the size of the <see cref="INT16" /> struct is correct.</summary>
        [Test]
        public static void SizeOfTest()
        {
            Assert.That(Marshal.SizeOf<INT16>(), Is.EqualTo(2));
        }
    }
}
