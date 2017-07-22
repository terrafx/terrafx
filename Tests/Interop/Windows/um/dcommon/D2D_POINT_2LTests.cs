// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace TerraFX.Interop.UnitTests
{
    /// <summary>Provides validation of the <see cref="D2D_POINT_2L" /> struct.</summary>
    public static class D2D_POINT_2LTests
    {
        /// <summary>Validates that the layout of the <see cref="D2D_POINT_2L" /> struct is <see cref="LayoutKind.Explicit" />.</summary>
        [Test]
        public static void IsLayoutExplicitTest()
        {
            Assert.That(typeof(D2D_POINT_2L).IsExplicitLayout, Is.True);
        }

        /// <summary>Validates that the size of the <see cref="D2D_POINT_2L" /> struct is correct.</summary>
        [Test]
        public static void SizeOfTest()
        {
            Assert.That(Marshal.SizeOf<D2D_POINT_2L>(), Is.EqualTo(8));
        }
    }
}
