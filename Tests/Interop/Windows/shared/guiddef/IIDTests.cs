// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace TerraFX.Interop.UnitTests
{
    /// <summary>Provides validation of the <see cref="IID" /> struct.</summary>
    public static class IIDTests
    {
        /// <summary>Validates that the layout of the <see cref="IID" /> struct is <see cref="LayoutKind.Sequential" />.</summary>
        [Test]
        public static void IsLayoutSequentialTest()
        {
            Assert.That(typeof(IID).IsLayoutSequential, Is.True);
        }

        /// <summary>Validates that the size of the <see cref="IID" /> struct is correct.</summary>
        [Test]
        public static void SizeOfTest()
        {
            Assert.That(Marshal.SizeOf<IID>(), Is.EqualTo(16));
        }
    }
}
