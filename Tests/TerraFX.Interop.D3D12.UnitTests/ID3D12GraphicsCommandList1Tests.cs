// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace TerraFX.Interop.UnitTests
{
    /// <summary>Provides validation of the <see cref="ID3D12GraphicsCommandList1" /> struct.</summary>
    public static class ID3D12GraphicsCommandList1Tests
    {
        /// <summary>Validates that the <see cref="Guid" /> of the <see cref="ID3D12GraphicsCommandList1" /> struct is correct.</summary>
        [Test]
        public static void GuidOfTest()
        {
            var IID_ID3D12GraphicsCommandList1 = new Guid("553103FB-1FE7-4557-BB38-946D7D0E7CA7");
            Assert.That(typeof(ID3D12GraphicsCommandList1).GUID, Is.EqualTo(IID_ID3D12GraphicsCommandList1));
        }

        /// <summary>Validates that the layout of the <see cref="ID3D12GraphicsCommandList1" /> struct is <see cref="LayoutKind.Sequential" />.</summary>
        [Test]
        public static void IsLayoutSequentialTest()
        {
            Assert.That(typeof(ID3D12GraphicsCommandList1).IsLayoutSequential, Is.True);
        }

        /// <summary>Validates that the size of the <see cref="ID3D12GraphicsCommandList1" /> struct is correct.</summary>
        [Test]
        public static void SizeOfTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.That(Marshal.SizeOf<ID3D12GraphicsCommandList1>(), Is.EqualTo(8));
            }
            else
            {
                Assert.That(Marshal.SizeOf<ID3D12GraphicsCommandList1>(), Is.EqualTo(4));
            }
        }

        /// <summary>Provides validation of the <see cref="ID3D12GraphicsCommandList1.Vtbl" /> struct.</summary>
        public static class VtblTests
        {
            /// <summary>Validates that the layout of the <see cref="ID3D12GraphicsCommandList1" /> struct is <see cref="LayoutKind.Sequential" />.</summary>
            [Test]
            public static void IsLayoutSequentialTest()
            {
                Assert.That(typeof(ID3D12GraphicsCommandList1.Vtbl).IsLayoutSequential, Is.True);
            }

            /// <summary>Validates that the size of the <see cref="ID3D12GraphicsCommandList1" /> struct is correct.</summary>
            [Test]
            public static void SizeOfTest()
            {
                if (Environment.Is64BitProcess)
                {
                    Assert.That(Marshal.SizeOf<ID3D12GraphicsCommandList1.Vtbl>(), Is.EqualTo(520));
                }
                else
                {
                    Assert.That(Marshal.SizeOf<ID3D12GraphicsCommandList1.Vtbl>(), Is.EqualTo(260));
                }
            }
        }
    }
}
