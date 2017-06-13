// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace TerraFX.Interop.UnitTests
{
    /// <summary>Provides validation of the <see cref="ID3D12ShaderReflectionType" /> struct.</summary>
    public static class ID3D12ShaderReflectionTypeTests
    {
        /// <summary>Validates that the <see cref="Guid" /> of the <see cref="ID3D12ShaderReflectionType" /> struct is correct.</summary>
        [Test]
        public static void GuidOfTest()
        {
            var IID_ID3D12ShaderReflectionType = new Guid("E913C351-783D-48CA-A1D1-4F306284AD56");
            Assert.That(typeof(ID3D12ShaderReflectionType).GUID, Is.EqualTo(IID_ID3D12ShaderReflectionType));
        }

        /// <summary>Validates that the layout of the <see cref="ID3D12ShaderReflectionType" /> struct is <see cref="LayoutKind.Sequential" />.</summary>
        [Test]
        public static void IsLayoutSequentialTest()
        {
            Assert.That(typeof(ID3D12ShaderReflectionType).IsLayoutSequential, Is.True);
        }

        /// <summary>Validates that the size of the <see cref="ID3D12ShaderReflectionType" /> struct is correct.</summary>
        [Test]
        public static void SizeOfTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.That(Marshal.SizeOf<ID3D12ShaderReflectionType>(), Is.EqualTo(8));
            }
            else
            {
                Assert.That(Marshal.SizeOf<ID3D12ShaderReflectionType>(), Is.EqualTo(4));
            }
        }

        /// <summary>Provides validation of the <see cref="ID3D12ShaderReflectionType.Vtbl" /> struct.</summary>
        public static class VtblTests
        {
            /// <summary>Validates that the layout of the <see cref="ID3D12ShaderReflectionType" /> struct is <see cref="LayoutKind.Sequential" />.</summary>
            [Test]
            public static void IsLayoutSequentialTest()
            {
                Assert.That(typeof(ID3D12ShaderReflectionType.Vtbl).IsLayoutSequential, Is.True);
            }

            /// <summary>Validates that the size of the <see cref="ID3D12ShaderReflectionType" /> struct is correct.</summary>
            [Test]
            public static void SizeOfTest()
            {
                if (Environment.Is64BitProcess)
                {
                    Assert.That(Marshal.SizeOf<ID3D12ShaderReflectionType.Vtbl>(), Is.EqualTo(88));
                }
                else
                {
                    Assert.That(Marshal.SizeOf<ID3D12ShaderReflectionType.Vtbl>(), Is.EqualTo(44));
                }
            }
        }
    }
}
