// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;
using TerraFX.Threading;
using TerraFX.Utilities;

namespace TerraFX.UnitTests.Utilities
{
    /// <summary>Provides a set of tests covering the <see cref="ExceptionUtilities" /> static class.</summary>
    [TestFixture(TestOf = typeof(ExceptionUtilities))]
    public static class ExceptionUtilitiesTests
    {
        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowArgumentException(string, string)" /> method.</summary>
        [Test]
        public static void ThrowArgumentExceptionTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowArgumentException("message", "value"),
                Throws.ArgumentException
                      .And.Message.Contains("message")
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowArgumentNullException(string)" /> method.</summary>
        [Test]
        public static void ThrowArgumentNullExceptionTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowArgumentNullException("value"),
                Throws.ArgumentNullException
                      .And.Message.Contains("'value'")
                      .And.Message.Contains("null")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowArgumentOutOfRangeException{T}(string, T, string)" /> method.</summary>
        [Test]
        public static void ThrowArgumentOutOfRangeExceptionTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowArgumentOutOfRangeException("message", Guid.Empty, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(Guid.Empty)
                      .And.Message.Contains("message")
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowExternalException(string, int)" /> method.</summary>
        [Test]
        public static void ThrowExternalExceptionTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowExternalException("method", -1),
                Throws.InstanceOf<ExternalException>()
                      .And.Property("ErrorCode").EqualTo(-1)
                      .And.Message.Contains("'method'")
                      .And.Message.Contains($"'{-1}'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForEmptyQueue" /> method.</summary>
        [Test]
        public static void ThrowForEmptyQueueTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForEmptyQueue(),
                Throws.InstanceOf<InvalidOperationException>()
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForEmptyStack" /> method.</summary>
        [Test]
        public static void ThrowForEmptyStackTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForEmptyStack(),
                Throws.InstanceOf<InvalidOperationException>()
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForInvalidFlagsCombination{TEnum}(TEnum, string)" /> method.</summary>
        [Test]
        public static void ThrowForInvalidFlagsCombinationTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForInvalidFlagsCombination(AttributeTargets.Assembly, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(AttributeTargets.Assembly)
                      .And.Message.Contains("'value'")
                      .And.Message.Contains(AttributeTargets.Assembly.ToString())
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForInvalidKind{TEnum}(TEnum, string)" /> method.</summary>
        [Test]
        public static void ThrowForInvalidKindTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForInvalidKind(AttributeTargets.Class, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(AttributeTargets.Class)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForInvalidKind{TEnum}(TEnum, string, TEnum)" /> method.</summary>
        [Test]
        public static void ThrowForInvalidKindWithExpectedKindTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForInvalidKind(AttributeTargets.Class, "value", AttributeTargets.Struct),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(AttributeTargets.Class)
                      .And.Message.Contains("'value'")
                      .And.Message.Contains($"'{AttributeTargets.Struct}'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForInvalidParent{T}(T, string)" /> method.</summary>
        [Test]
        public static void ThrowForInvalidParentTest()
        {
            var value = new object();

            Assert.That(() => ExceptionUtilities.ThrowForInvalidParent(value, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").SameAs(value)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForInvalidState(string)" /> method.</summary>
        [Test]
        public static void ThrowForInvalidStateTest()
        {
            var parent = new object();

            Assert.That(() => ExceptionUtilities.ThrowForInvalidState("state"),
                Throws.InvalidOperationException
                      .And.Message.Contains("'state'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForInvalidType(Type, string, Type)" /> method.</summary>
        [Test]
        public static void ThrowForInvalidTypeTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForInvalidType(typeof(object), "value", typeof(string)),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").SameAs(typeof(object))
                      .And.Message.Contains("'value'")
                      .And.Message.Contains($"'{typeof(string)}'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastError(string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastError("method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastErrorIfNotZero(int, string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorIfNotZeroInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfNotZero(0, "method"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfNotZero(1, "method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastErrorIfNotZero(long, string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorIfNotZeroInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfNotZero(0L, "method"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfNotZero(1L, "method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastErrorIfNotZero(nint, string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorIfNotZeroNIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfNotZero((nint)0, "method"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfNotZero((nint)1, "method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastErrorIfNotZero(uint, string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorIfNotZeroUInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfNotZero(0U, "method"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfNotZero(1U, "method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastErrorIfNotZero(ulong, string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorIfNotZeroUInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfNotZero(0UL, "method"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfNotZero(1UL, "method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastErrorIfNotZero(nuint, string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorIfNotZeroNUIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfNotZero((nuint)0, "method"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfNotZero((nuint)1, "method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastErrorIfZero(int, string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorIfZeroInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfZero(1, "method"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfZero(0, "method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastErrorIfZero(long, string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorIfZeroInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfZero(1L, "method"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfZero(0L, "method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastErrorIfZero(nint, string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorIfZeroNIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfZero((nint)1, "method"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfZero((nint)0, "method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastErrorIfZero(uint, string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorIfZeroUInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfZero(1U, "method"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfZero(0U, "method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastErrorIfZero(ulong, string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorIfZeroUInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfZero(1UL, "method"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfZero(0UL, "method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForLastErrorIfZero(nuint, string)" /> method.</summary>
        [Test]
        public static void ThrowForLastErrorIfZeroNUIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfZero((nuint)1, "method"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowForLastErrorIfZero((nuint)0, "method"),
                Throws.InstanceOf<ExternalException>()
                      .And.Message.Contains("'method'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForMissingFeature" /> method.</summary>
        [Test]
        public static void ThrowForMissingFeatureTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForMissingFeature(),
                Throws.InstanceOf<NotSupportedException>()
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowForUnsupportedSurfaceKind(string)" /> method.</summary>
        [Test]
        public static void ThrowForUnsupportedSurfaceKindTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowForUnsupportedSurfaceKind("surface"),
                Throws.InstanceOf<NotSupportedException>()
                      .And.Message.Contains("'surface'")
                      .And.Message.Contains("GraphicsSurfaceKind")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfDisposedOrDisposing(VolatileState, string)" /> method.</summary>
        [Test]
        public static void ThrowIfDisposedOrDisposingTest()
        {
            var state = new VolatileState();

            Assert.That(() => ExceptionUtilities.ThrowIfDisposedOrDisposing(state, "type"),
                Throws.Nothing
            );

            _ = state.BeginDispose();

            Assert.That(() => ExceptionUtilities.ThrowIfDisposedOrDisposing(state, "type"),
                Throws.InstanceOf<ObjectDisposedException>()
                      .And.Message.Contains("'type'")
            );

            state.EndDispose();

            Assert.That(() => ExceptionUtilities.ThrowIfDisposedOrDisposing(state, "type"),
                Throws.InstanceOf<ObjectDisposedException>()
                      .And.Message.Contains("'type'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNegative(int, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNegativeInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNegative(0, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNegative(-1, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(-1)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNegative(long, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNegativeInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNegative(0L, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNegative(-1L, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(-1L)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNegative(nint, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNegativeNIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNegative((nint)0, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNegative((nint)(-1), "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nint)(-1))
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotInBounds(int, int, string, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotInBoundsInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(0, 1, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(-1, 1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(-1)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(1, 1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(1)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(2, 1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(2)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotInBounds(long, long, string, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotInBoundsInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(0L, 1L, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(-1L, 1L, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(-1L)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(1L, 1L, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(1L)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(2L, 1L, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(2L)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotInBounds(nint, nint, string, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotInBoundsNIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds((nint)0, (nint)1, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds((nint)(-1), (nint)1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nint)(-1))
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds((nint)1, (nint)1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nint)1)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds((nint)2, (nint)1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nint)2)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotInBounds(uint, uint, string, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotInBoundsUInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(0U, 1U, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(1U, 1U, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(1U)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(2U, 1U, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(2U)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotInBounds(ulong, ulong, string, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotInBoundsUInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(0UL, 1UL, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(1UL, 1UL, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(1UL)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds(2UL, 1UL, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(2UL)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotInBounds(nuint, nuint, string, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotInBoundsNUIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds((nuint)0, (nuint)1, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds((nuint)1, (nuint)1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nuint)1)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInBounds((nuint)2, (nuint)1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nuint)2)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotInInsertBounds(int, int, string, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotInInsertBoundsInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(0, 1, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(1, 1, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(-1, 1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(-1)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(2, 1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(2)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotInInsertBounds(long, long, string, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotInInsertBoundsInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(0L, 1L, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(1L, 1L, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(-1L, 1L, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(-1L)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(2L, 1L, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(2L)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotInInsertBounds(nint, nint, string, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotInInsertBoundsNIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds((nint)0, (nint)1, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds((nint)1, (nint)1, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds((nint)(-1), (nint)1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nint)(-1))
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds((nint)2, (nint)1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nint)2)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotInInsertBounds(uint, uint, string, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotInInsertBoundsUInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(0U, 1U, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(1U, 1U, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(2U, 1U, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(2U)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotInInsertBounds(ulong, ulong, string, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotInInsertBoundsUInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(0UL, 1UL, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(1UL, 1UL, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds(2UL, 1UL, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(2UL)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotInInsertBounds(nuint, nuint, string, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotInInsertBoundsNUIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds((nuint)0, (nuint)1, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds((nuint)1, (nuint)1, "index", "length"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotInInsertBounds((nuint)2, (nuint)1, "index", "length"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nuint)2)
                      .And.Message.Contains("'index'")
                      .And.Message.Contains("'length'")
                      .And.Property("ParamName").EqualTo("index")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotPow2(uint, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotPow2UInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotPow2(2U, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotPow2(0U, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(0U)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotPow2(ulong, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotPow2UInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotPow2(2UL, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotPow2(0UL, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(0UL)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotPow2(nuint, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNotPow2NUIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotPow2((nuint)2, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotPow2((nuint)0, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nuint)0)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotThread(Thread)" /> method.</summary>
        [Test]
        public static void ThrowIfNotThreadTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotThread(Thread.CurrentThread),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotThread(null!),
                Throws.InstanceOf<InvalidOperationException>()
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotZero(int, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfNotZeroInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotZero(0, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotZero(1, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(1)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotZero(long, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfNotZeroInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotZero(0L, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotZero(1L, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(1L)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotZero(nint, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfNotZeroNIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotZero((nint)0, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotZero((nint)1, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nint)1)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotZero(uint, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfNotZeroUInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotZero(0U, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotZero(1U, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(1U)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotZero(ulong, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfNotZeroUInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotZero(0UL, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotZero(1UL, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(1UL)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNotZero(nuint, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfNotZeroNUIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNotZero((nuint)0, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNotZero((nuint)1, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nuint)1)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNull{T}(T, string)" /> method.</summary>
        [Test]
        public static void ThrowIfNullObjectTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNull("", "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNull<object>(null, "value"),
                Throws.ArgumentNullException
                      .And.Message.Contains("'value'")
                      .And.Message.Contains("null")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNull(void*, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfNullPointerTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNull((void*)1, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNull(null, "value"),
                Throws.ArgumentNullException
                      .And.Message.Contains("'value'")
                      .And.Message.Contains("null")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfNull{T}(UnmanagedArray{T}, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfNullUnmanagedArrayTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfNull(UnmanagedArray<int>.Empty, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfNull(new UnmanagedArray<int>(), "value"),
                Throws.ArgumentNullException
                      .And.Message.Contains("'value'")
                      .And.Message.Contains("null")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfZero(int, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfZeroInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfZero(1, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfZero(0, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(0)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfZero(long, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfZeroInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfZero(1L, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfZero(0L, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(0L)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfZero(nint, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfZeroNIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfZero((nint)1, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfZero((nint)0, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nint)0)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfZero(uint, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfZeroUInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfZero(1U, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfZero(0U, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(0U)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfZero(ulong, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfZeroUInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfZero(1UL, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfZero(0UL, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo(0UL)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowIfZero(nuint, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowIfZeroNUIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowIfZero((nuint)1, "value"),
                Throws.Nothing
            );

            Assert.That(() => ExceptionUtilities.ThrowIfZero((nuint)0, "value"),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .And.Property("ActualValue").EqualTo((nuint)0)
                      .And.Message.Contains("'value'")
                      .And.Property("ParamName").EqualTo("value")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowInvalidOperationException(string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowInvalidOperationExceptionTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowInvalidOperationException("message"),
                Throws.InvalidOperationException
                      .And.Message.Contains("message")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowKeyNotFoundException{TKey}(TKey, string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowKeyNotFoundExceptionTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowKeyNotFoundException("key", "collection"),
                Throws.InstanceOf<KeyNotFoundException>()
                      .And.Message.Contains("'collection'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowNotImplementedException" /> method.</summary>
        [Test]
        public static unsafe void ThrowNotImplementedExceptionTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowNotImplementedException(),
                Throws.InstanceOf<NotImplementedException>()
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowObjectDisposedException(string)" /> method.</summary>
        [Test]
        public static unsafe void ThrowObjectDisposedExceptionTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowObjectDisposedException("value"),
                Throws.InstanceOf<ObjectDisposedException>()
                      .And.Message.Contains("'value'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowOutOfMemoryException(ulong)" /> method.</summary>
        [Test]
        public static unsafe void ThrowOutOfMemoryExceptionUInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowOutOfMemoryException(42UL),
                Throws.InstanceOf<OutOfMemoryException>()
                      .And.Message.Contains($"'{42UL}'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowOutOfMemoryException(nuint)" /> method.</summary>
        [Test]
        public static unsafe void ThrowOutOfMemoryExceptionNUIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowOutOfMemoryException((nuint)42),
                Throws.InstanceOf<OutOfMemoryException>()
                      .And.Message.Contains($"'{(nuint)42}'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowOutOfMemoryException(ulong, ulong)" /> method.</summary>
        [Test]
        public static unsafe void ThrowOutOfMemoryExceptionUInt64UInt64Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowOutOfMemoryException(42UL, 24UL),
                Throws.InstanceOf<OutOfMemoryException>()
                      .And.Message.Contains($"'{42UL}x{24UL}'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowOutOfMemoryException(nuint, nuint)" /> method.</summary>
        [Test]
        public static unsafe void ThrowOutOfMemoryExceptionNUIntNUIntTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowOutOfMemoryException((nuint)42, (nuint)24),
                Throws.InstanceOf<OutOfMemoryException>()
                      .And.Message.Contains($"'{(nuint)42}x{(nuint)24}'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowTimeoutException(string, int)" /> method.</summary>
        [Test]
        public static unsafe void ThrowTimeoutExceptionInt32Test()
        {
            Assert.That(() => ExceptionUtilities.ThrowTimeoutException("method", 42),
                Throws.InstanceOf<TimeoutException>()
                      .And.Message.Contains("'method'")
                      .And.Message.Contains($"'{42}'")
            );
        }

        /// <summary>Provides validation of the <see cref="ExceptionUtilities.ThrowTimeoutException(string, TimeSpan)" /> method.</summary>
        [Test]
        public static unsafe void ThrowTimeoutExceptionTimeSpanTest()
        {
            Assert.That(() => ExceptionUtilities.ThrowTimeoutException("method", TimeSpan.FromMilliseconds(42)),
                Throws.InstanceOf<TimeoutException>()
                      .And.Message.Contains("'method'")
                      .And.Message.Contains($"'{TimeSpan.FromMilliseconds(42).TotalMilliseconds}'")
            );
        }
    }
}
