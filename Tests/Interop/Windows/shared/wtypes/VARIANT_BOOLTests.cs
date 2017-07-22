// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace TerraFX.Interop.UnitTests
{
    /// <summary>Provides validation of the <see cref="VARIANT_BOOL" /> struct.</summary>
    public static class VARIANT_BOOLTests
    {
        /// <summary>Validates that the layout of the <see cref="VARIANT_BOOL" /> struct is <see cref="LayoutKind.Sequential" />.</summary>
        [Test]
        public static void IsLayoutSequentialTest()
        {
            Assert.That(typeof(VARIANT_BOOL).IsLayoutSequential, Is.True);
        }

        /// <summary>Validates that the size of the <see cref="VARIANT_BOOL" /> struct is correct.</summary>
        [Test]
        public static void SizeOfTest()
        {
            Assert.That(Marshal.SizeOf<VARIANT_BOOL>(), Is.EqualTo(2));
        }
    }
}
