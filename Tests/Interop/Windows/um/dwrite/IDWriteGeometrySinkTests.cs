// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using static TerraFX.Interop.DWrite;

namespace TerraFX.Interop.UnitTests
{
    /// <summary>Provides validation of the <see cref="IDWriteGeometrySink" /> struct.</summary>
    public static class IDWriteGeometrySinkTests
    {
        /// <summary>Validates that the <see cref="Guid" /> of the <see cref="IDWriteGeometrySink" /> struct is correct.</summary>
        [Test]
        public static void GuidOfTest()
        {
            Assert.That(typeof(IDWriteGeometrySink).GUID, Is.EqualTo(IID_IDWriteGeometrySink));
        }

        /// <summary>Validates that the layout of the <see cref="IDWriteGeometrySink" /> struct is <see cref="LayoutKind.Explicit" />.</summary>
        [Test]
        public static void IsLayoutExplicitTest()
        {
            Assert.That(typeof(IDWriteGeometrySink).IsExplicitLayout, Is.True);
        }

        /// <summary>Validates that the size of the <see cref="IDWriteGeometrySink" /> struct is correct.</summary>
        [Test]
        public static void SizeOfTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.That(Marshal.SizeOf<IDWriteGeometrySink>(), Is.EqualTo(8));
            }
            else
            {
                Assert.That(Marshal.SizeOf<IDWriteGeometrySink>(), Is.EqualTo(4));
            }
        }
    }
}
