// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace TerraFX.Interop.UnitTests
{
    /// <summary>Provides validation of the <see cref="ID3D12PipelineLibrary1" /> struct.</summary>
    public static class ID3D12PipelineLibrary1Tests
    {
        /// <summary>Validates that the <see cref="Guid" /> of the <see cref="ID3D12PipelineLibrary1" /> struct is correct.</summary>
        [Test]
        public static void GuidOfTest()
        {
            var IID_ID3D12PipelineLibrary1 = new Guid("80EABF42-2568-4E5E-BD82-C37F86961DC3");
            Assert.That(typeof(ID3D12PipelineLibrary1).GUID, Is.EqualTo(IID_ID3D12PipelineLibrary1));
        }

        /// <summary>Validates that the layout of the <see cref="ID3D12PipelineLibrary1" /> struct is <see cref="LayoutKind.Sequential" />.</summary>
        [Test]
        public static void IsLayoutSequentialTest()
        {
            Assert.That(typeof(ID3D12PipelineLibrary1).IsLayoutSequential, Is.True);
        }

        /// <summary>Validates that the size of the <see cref="ID3D12PipelineLibrary1" /> struct is correct.</summary>
        [Test]
        public static void SizeOfTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.That(Marshal.SizeOf<ID3D12PipelineLibrary1>(), Is.EqualTo(8));
            }
            else
            {
                Assert.That(Marshal.SizeOf<ID3D12PipelineLibrary1>(), Is.EqualTo(4));
            }
        }

        /// <summary>Provides validation of the <see cref="ID3D12PipelineLibrary1.Vtbl" /> struct.</summary>
        public static class VtblTests
        {
            /// <summary>Validates that the layout of the <see cref="ID3D12PipelineLibrary1" /> struct is <see cref="LayoutKind.Sequential" />.</summary>
            [Test]
            public static void IsLayoutSequentialTest()
            {
                Assert.That(typeof(ID3D12PipelineLibrary1.Vtbl).IsLayoutSequential, Is.True);
            }

            /// <summary>Validates that the size of the <see cref="ID3D12PipelineLibrary1" /> struct is correct.</summary>
            [Test]
            public static void SizeOfTest()
            {
                if (Environment.Is64BitProcess)
                {
                    Assert.That(Marshal.SizeOf<ID3D12PipelineLibrary1.Vtbl>(), Is.EqualTo(112));
                }
                else
                {
                    Assert.That(Marshal.SizeOf<ID3D12PipelineLibrary1.Vtbl>(), Is.EqualTo(56));
                }
            }
        }
    }
}
