// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace TerraFX.Interop.Unknown.UnitTests
{
    /// <summary>Provides validation of the <see cref="HMODULE" /> struct.</summary>
    public static class HMODULETests
    {
        /// <summary>Validates that the layout of the <see cref="HMODULE" /> struct is <see cref="LayoutKind.Sequential" />.</summary>
        [Test]
        public static void IsLayoutSequentialTest()
        {
            Assert.That(typeof(HMODULE).IsLayoutSequential, Is.True);
        }

        /// <summary>Validates that the size of the <see cref="HMODULE" /> struct is correct.</summary>
        [Test]
        public static void SizeOfTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.That(Marshal.SizeOf<HMODULE>(), Is.EqualTo(8));
            }
            else
            {
                Assert.That(Marshal.SizeOf<HMODULE>(), Is.EqualTo(4));
            }
        }
    }
}
