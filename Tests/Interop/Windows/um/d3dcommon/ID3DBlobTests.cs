// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace TerraFX.Interop.UnitTests
{
    /// <summary>Provides validation of the <see cref="ID3DBlob" /> struct.</summary>
    public static class ID3DBlobTests
    {
        /// <summary>Validates that the layout of the <see cref="ID3DBlob" /> struct is <see cref="LayoutKind.Explicit" />.</summary>
        [Test]
        public static void IsLayoutExplicitTest()
        {
            Assert.That(typeof(ID3DBlob).IsExplicitLayout, Is.True);
        }

        /// <summary>Validates that the size of the <see cref="ID3DBlob" /> struct is correct.</summary>
        [Test]
        public static void SizeOfTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.That(Marshal.SizeOf<ID3DBlob>(), Is.EqualTo(8));
            }
            else
            {
                Assert.That(Marshal.SizeOf<ID3DBlob>(), Is.EqualTo(4));
            }
        }
    }
}
