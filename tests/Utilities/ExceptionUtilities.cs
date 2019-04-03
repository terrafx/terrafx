// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace TerraFX.Utilities.UnitTests
{
    /// <summary>Provides a set of tests covering the <see cref="ExceptionUtilities" /> static class.</summary>
    [TestFixture(Author = "Tanner Gooding", TestOf = typeof(ExceptionUtilities))]
    public static class ExceptionUtilitiesTests
    {
        #region Static Method Tests
        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowArgumentExceptionForInvalidType(string, Type)" /> static method.</summary>
        [Test]
        public static void ThrowArgumentExceptionForInvalidType(
            [Values(null, "", "param")] string paramName,
            [Values(typeof(object), typeof(string), typeof(int))] Type paramType
        )
        {
            Assert.That(() => ExceptionUtilities.ThrowArgumentExceptionForInvalidType(paramName, paramType),
                Throws.InstanceOf<ArgumentException>()
                      .With.Property("ParamName").EqualTo(paramName)
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowArgumentOutOfRangeException(string, object)" /> static method.</summary>
        [Test]
        public static void ThrowArgumentOutOfRangeExceptionStringObjectTest(
            [Values(null, "", "param")] string paramName,
            [Values(null, "", "value")] object value
        )
        {
            Assert.That(() => ExceptionUtilities.ThrowArgumentOutOfRangeException(paramName, value),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo(paramName)
                      .And.With.Property("ActualValue").EqualTo(value)
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowExternalException(string, int)" /> static method.</summary>
        [Test]
        public static void ThrowExternalExceptionStringInt32Test(
            [Values(null, "", "methodName")] string methodName,
            [Values(0, 1, unchecked((int)0x80000000))] int errorCode
        )
        {
            Assert.That(() => ExceptionUtilities.ThrowExternalException(methodName, errorCode),
                Throws.InstanceOf<ExternalException>()
                      .With.Property("ErrorCode").EqualTo(errorCode)
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowExternalExceptionForLastError(string)" /> static method.</summary>
        [Test]
        public static void ThrowExternalExceptionForLastErrorStringTest(
            [Values(null, "", "methodName")] string methodName
        )
        {
            var errorCode = Marshal.GetLastWin32Error();

            Assert.That(() => ExceptionUtilities.ThrowExternalExceptionForLastError(methodName),
                Throws.InstanceOf<ExternalException>()
                      .With.Property("ErrorCode").EqualTo(errorCode)
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowExternalExceptionForLastHRESULT(string)" /> static method.</summary>
        [Test]
        public static void ThrowExternalExceptionForLastHRESULTStringTest(
            [Values(null, "", "methodName")] string methodName
        )
        {
            var hresult = Marshal.GetHRForLastWin32Error();

            Assert.That(() => ExceptionUtilities.ThrowExternalExceptionForLastHRESULT(methodName),
                Throws.InstanceOf<ExternalException>()
                      .With.Property("ErrorCode").EqualTo(hresult)
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNull{T}(T, string)" /> static method.</summary>
        [Test]
        public static void ThrowIfNullObjectStringTest(
            [Values(null, "", "param")] string paramName
        )
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNull<object>(null, paramName),
                Throws.InstanceOf<ArgumentNullException>()
                      .With.Property("ParamName").EqualTo(paramName)
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowInvalidOperationException(string, object)" /> static method.</summary>
        [Test]
        public static void ThrowInvalidOperationExceptionStringObjectTest(
            [Values(null, "", "param")] string paramName,
            [Values(null, "", "value")] object value
        )
        {
            Assert.That(() => ExceptionUtilities.ThrowInvalidOperationException(paramName, value),
                Throws.InstanceOf<InvalidOperationException>()
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIOException()" /> static method.</summary>
        [Test]
        public static void ThrowIOExceptionStringTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIOException(),
                Throws.InstanceOf<IOException>()
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowObjectDisposedException(string)" /> static method.</summary>
        [Test]
        public static void ThrowObjectDisposedExceptionStringObjectTest(
            [Values("", "object")] string objectName
        )
        {
            Assert.That(() => ExceptionUtilities.ThrowObjectDisposedException(objectName),
                Throws.InstanceOf<ObjectDisposedException>()
                      .With.Property("ObjectName").EqualTo(objectName)
            );
        }
        #endregion
    }
}
