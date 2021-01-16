// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;
using TerraFX.Utilities;

namespace TerraFX.UnitTests.Utilities
{
    /// <summary>Provides a set of tests covering the <see cref="AppContextUtilities" /> static class.</summary>
    [TestFixture(TestOf = typeof(AppContextUtilities))]
    public static class AppContextUtilitiesTests
    {
        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, bool)" /> method.</summary>
        [Test]
        public static void GetAppContextDataBooleanTest()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, false),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, false),
                Is.EqualTo(false)
            );

            AppContextUtilities.SetAppContextData(string.Empty, true);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, false),
                Is.EqualTo(true)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "false");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, true),
                Is.EqualTo(false)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, true),
                Is.EqualTo(true)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, false),
                Is.EqualTo(false)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, byte)" /> method.</summary>
        [Test]
        public static void GetAppContextDataByteTest()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, (byte)0),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (byte)0),
                Is.EqualTo((byte)0)
            );

            AppContextUtilities.SetAppContextData(string.Empty, (byte)1);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (byte)0),
                Is.EqualTo((byte)1)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "2");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (byte)0),
                Is.EqualTo((byte)2)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (byte)3),
                Is.EqualTo((byte)3)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (byte)4),
                Is.EqualTo((byte)4)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, double)" /> method.</summary>
        [Test]
        public static void GetAppContextDataDoubleTest()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, 0.0),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0.0),
                Is.EqualTo(0.0)
            );

            AppContextUtilities.SetAppContextData(string.Empty, 1.0);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0.0),
                Is.EqualTo(1.0)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "2.0");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0.0),
                Is.EqualTo(2.0)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 3.0),
                Is.EqualTo(3.0)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 4.0),
                Is.EqualTo(4.0)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, short)" /> method.</summary>
        [Test]
        public static void GetAppContextDataInt16Test()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, (short)0),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (short)0),
                Is.EqualTo((short)0)
            );

            AppContextUtilities.SetAppContextData(string.Empty, (short)1);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (short)0),
                Is.EqualTo((short)1)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "2");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (short)0),
                Is.EqualTo((short)2)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (short)3),
                Is.EqualTo((short)3)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (short)4),
                Is.EqualTo((short)4)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, int)" /> method.</summary>
        [Test]
        public static void GetAppContextDataInt32Test()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, 0),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0),
                Is.EqualTo(0)
            );

            AppContextUtilities.SetAppContextData(string.Empty, 1);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0),
                Is.EqualTo(1)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "2");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0),
                Is.EqualTo(2)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 3),
                Is.EqualTo(3)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 4),
                Is.EqualTo(4)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, long)" /> method.</summary>
        [Test]
        public static void GetAppContextDataInt64Test()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, 0L),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0L),
                Is.EqualTo(0L)
            );

            AppContextUtilities.SetAppContextData(string.Empty, 1L);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0L),
                Is.EqualTo(1L)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "2");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0L),
                Is.EqualTo(2L)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 3L),
                Is.EqualTo(3L)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 4L),
                Is.EqualTo(4L)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, nint)" /> method.</summary>
        [Test]
        public static void GetAppContextDataNIntTest()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, (nint)0),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (nint)0),
                Is.EqualTo((nint)0)
            );

            AppContextUtilities.SetAppContextData(string.Empty, (nint)1);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (nint)0),
                Is.EqualTo((nint)1)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "2");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (nint)0),
                Is.EqualTo((nint)2)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (nint)3),
                Is.EqualTo((nint)3)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (nint)4),
                Is.EqualTo((nint)4)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, byte)" /> method.</summary>
        [Test]
        public static void GetAppContextDataSByteTest()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, (sbyte)0),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (sbyte)0),
                Is.EqualTo((sbyte)0)
            );

            AppContextUtilities.SetAppContextData(string.Empty, (sbyte)1);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (sbyte)0),
                Is.EqualTo((sbyte)1)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "2");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (sbyte)0),
                Is.EqualTo((sbyte)2)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (sbyte)3),
                Is.EqualTo((sbyte)3)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (sbyte)4),
                Is.EqualTo((sbyte)4)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, double)" /> method.</summary>
        [Test]
        public static void GetAppContextDataSingleTest()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, 0.0f),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0.0f),
                Is.EqualTo(0.0f)
            );

            AppContextUtilities.SetAppContextData(string.Empty, 1.0f);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0.0f),
                Is.EqualTo(1.0f)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "2.0");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0.0f),
                Is.EqualTo(2.0f)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 3.0f),
                Is.EqualTo(3.0f)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 4.0f),
                Is.EqualTo(4.0f)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, string)" /> method.</summary>
        [Test]
        public static void GetAppContextDataStringTest()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, ""),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            AppContextUtilities.SetAppContextData(string.Empty, "a");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, ""),
                Is.EqualTo("a")
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, "b"),
                Is.EqualTo("b")
            );

            AppContextUtilities.SetAppContextData<string>(string.Empty, null);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, "c"),
                Is.EqualTo("c")
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, ushort)" /> method.</summary>
        [Test]
        public static void GetAppContextDataUInt16Test()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, (ushort)0),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (ushort)0),
                Is.EqualTo((ushort)0)
            );

            AppContextUtilities.SetAppContextData(string.Empty, (ushort)1);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (ushort)0),
                Is.EqualTo((ushort)1)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "2");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (ushort)0),
                Is.EqualTo((ushort)2)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (ushort)3),
                Is.EqualTo((ushort)3)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (ushort)4),
                Is.EqualTo((ushort)4)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, uint)" /> method.</summary>
        [Test]
        public static void GetAppContextDataUInt32Test()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, 0U),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0U),
                Is.EqualTo(0U)
            );

            AppContextUtilities.SetAppContextData(string.Empty, 1U);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0U),
                Is.EqualTo(1U)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "2");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0U),
                Is.EqualTo(2U)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 3U),
                Is.EqualTo(3U)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 4U),
                Is.EqualTo(4U)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, long)" /> method.</summary>
        [Test]
        public static void GetAppContextDataUInt64Test()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, 0UL),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0UL),
                Is.EqualTo(0UL)
            );

            AppContextUtilities.SetAppContextData(string.Empty, 1UL);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0UL),
                Is.EqualTo(1UL)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "2");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 0UL),
                Is.EqualTo(2UL)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 3UL),
                Is.EqualTo(3UL)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, 4UL),
                Is.EqualTo(4UL)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.GetAppContextData(string, nuint)" /> method.</summary>
        [Test]
        public static void GetAppContextDataNUIntTest()
        {
            Assert.That(() => AppContextUtilities.GetAppContextData(null!, (nuint)0),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (nuint)0),
                Is.EqualTo((nuint)0)
            );

            AppContextUtilities.SetAppContextData(string.Empty, (nuint)1);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (nuint)0),
                Is.EqualTo((nuint)1)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "2");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (nuint)0),
                Is.EqualTo((nuint)2)
            );

            AppContextUtilities.SetAppContextData(string.Empty, Guid.Empty);

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (nuint)3),
                Is.EqualTo((nuint)3)
            );

            AppContextUtilities.SetAppContextData(string.Empty, "<bad-value>");

            Assert.That(() => AppContextUtilities.GetAppContextData(string.Empty, (nuint)4),
                Is.EqualTo((nuint)4)
            );
        }

        /// <summary>Provides validation of the <see cref="AppContextUtilities.SetAppContextData{T}(string, T)" /> method.</summary>
        [Test]
        public static void SetAppContextDataTest()
        {
            Assert.That(() => AppContextUtilities.SetAppContextData(null!, string.Empty),
                Throws.ArgumentNullException
                      .With.Property("ParamName").EqualTo("name")
            );
        }
    }
}
