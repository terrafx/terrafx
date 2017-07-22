// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace TerraFX.Interop.UnitTests
{
    /// <summary>Provides validation of the <see cref="DXGI_RGBA" /> struct.</summary>
    public static class DXGI_RGBATests
    {
        /// <summary>Validates that the layout of the <see cref="DXGI_RGBA" /> struct is <see cref="LayoutKind.Explicit" />.</summary>
        [Test]
        public static void IsLayoutExplicitTest()
        {
            Assert.That(typeof(DXGI_RGBA).IsExplicitLayout, Is.True);
        }

        /// <summary>Validates that the size of the <see cref="DXGI_RGBA" /> struct is correct.</summary>
        [Test]
        public static void SizeOfTest()
        {
            Assert.That(Marshal.SizeOf<DXGI_RGBA>(), Is.EqualTo(16));
        }
    }
}
