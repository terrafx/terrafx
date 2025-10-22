// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;
using TerraFX.Utilities;

namespace TerraFX.UnitTests.Utilities;

/// <summary>Provides a set of tests covering the <see cref="MarshalUtilities" /> static class.</summary>
[TestFixture(TestOf = typeof(MarshalUtilities))]
internal static class MarshalUtilitiesTests
{
    /// <summary>Provides validation of the <see cref="MarshalUtilities.GetString(ReadOnlySpan{sbyte})" /> method.</summary>
    [Test]
    public static void GetStringReadOnlySpanSByteTest()
    {
        Assert.That(() => MarshalUtilities.GetString(default(sbyte[])),
            Is.Null
        );

        Assert.That(() => MarshalUtilities.GetString(Array.Empty<sbyte>()),
            Is.EqualTo(string.Empty)
        );

        Assert.That(() => MarshalUtilities.GetString([(sbyte)'A', (sbyte)'B', (sbyte)'C']),
            Is.EqualTo("ABC")
        );
    }

    /// <summary>Provides validation of the <see cref="MarshalUtilities.GetString(ReadOnlySpan{char})" /> method.</summary>
    [Test]
    public static void GetStringReadOnlySpanUInt16Test()
    {
        Assert.That(() => MarshalUtilities.GetString(default(char[])),
            Is.Null
        );

        Assert.That(() => MarshalUtilities.GetString(Array.Empty<char>()),
            Is.EqualTo(string.Empty)
        );

        Assert.That(() => MarshalUtilities.GetString(['A', 'B', 'C']),
            Is.EqualTo("ABC")
        );
    }

    /// <summary>Provides validation of the <see cref="MarshalUtilities.GetAsciiSpan(string?)" /> method.</summary>
    [Test]
    public static void GetAsciiSpanFromStringTest()
    {
        Assert.That(() => MarshalUtilities.GetAsciiSpan(null).ToArray(),
            Is.EqualTo(Array.Empty<sbyte>())
        );

        Assert.That(() => MarshalUtilities.GetAsciiSpan(string.Empty).ToArray(),
            Is.EqualTo(Array.Empty<sbyte>())
        );

        Assert.That(() => MarshalUtilities.GetAsciiSpan("ABC").ToArray(),
            Is.EqualTo([(sbyte)'A', (sbyte)'B', (sbyte)'C'])
        );
    }

    /// <summary>Provides validation of the <see cref="MarshalUtilities.GetAsciiSpan(sbyte*, int)" /> method.</summary>
    [Test]
    public static unsafe void GetAsciiSpanFromSBytePointerTest()
    {
        Assert.That(() => MarshalUtilities.GetAsciiSpan(null, -1).ToArray(),
            Is.EqualTo(Array.Empty<sbyte>())
        );

        fixed (sbyte* source = Array.Empty<sbyte>())
        {
            var pSource = source;

            Assert.That(() => MarshalUtilities.GetAsciiSpan(pSource, -1).ToArray(),
                Is.EqualTo(Array.Empty<sbyte>())
            );
        }

        fixed (sbyte* source = new sbyte[] { (sbyte)'A', (sbyte)'B', (sbyte)'C' })
        {
            var pSource = source;

            Assert.That(() => MarshalUtilities.GetAsciiSpan(pSource, -1).ToArray(),
                Is.EqualTo([(sbyte)'A', (sbyte)'B', (sbyte)'C'])
            );

            Assert.That(() => MarshalUtilities.GetAsciiSpan(pSource, 2).ToArray(),
                Is.EqualTo([(sbyte)'A', (sbyte)'B'])
            );
        }
    }

    /// <summary>Provides validation of the <see cref="MarshalUtilities.GetAsciiSpan(ref readonly sbyte, int)" /> method.</summary>
    [Test]
    public static unsafe void GetAsciiSpanFromSByteReferenceTest()
    {
        Assert.That(() => MarshalUtilities.GetAsciiSpan(in UnsafeUtilities.NullRef<sbyte>(), -1).ToArray(),
            Is.EqualTo(Array.Empty<sbyte>())
        );

        var source = new sbyte[] { (sbyte)'A', (sbyte)'B', (sbyte)'C' };

        Assert.That(() => MarshalUtilities.GetAsciiSpan(in source[0], -1).ToArray(),
            Is.EqualTo([(sbyte)'A', (sbyte)'B', (sbyte)'C'])
        );

        Assert.That(() => MarshalUtilities.GetAsciiSpan(in source[0], 2).ToArray(),
            Is.EqualTo([(sbyte)'A', (sbyte)'B'])
        );
    }

    /// <summary>Provides validation of the <see cref="MarshalUtilities.GetUtf8Span(string?)" /> method.</summary>
    [Test]
    public static void GetUtf8SpanFromStringTest()
    {
        Assert.That(() => MarshalUtilities.GetUtf8Span(null).ToArray(),
            Is.EqualTo(Array.Empty<sbyte>())
        );

        Assert.That(() => MarshalUtilities.GetUtf8Span(string.Empty).ToArray(),
            Is.EqualTo(Array.Empty<sbyte>())
        );

        Assert.That(() => MarshalUtilities.GetUtf8Span("ABC").ToArray(),
            Is.EqualTo([(sbyte)'A', (sbyte)'B', (sbyte)'C'])
        );
    }

    /// <summary>Provides validation of the <see cref="MarshalUtilities.GetUtf8Span(sbyte*, int)" /> method.</summary>
    [Test]
    public static unsafe void GetUtf8SpanFromSBytePointerTest()
    {
        Assert.That(() => MarshalUtilities.GetUtf8Span(null, -1).ToArray(),
            Is.EqualTo(Array.Empty<sbyte>())
        );

        fixed (sbyte* source = Array.Empty<sbyte>())
        {
            var pSource = source;

            Assert.That(() => MarshalUtilities.GetUtf8Span(pSource, -1).ToArray(),
                Is.EqualTo(Array.Empty<sbyte>())
            );
        }

        fixed (sbyte* source = new sbyte[] { (sbyte)'A', (sbyte)'B', (sbyte)'C' })
        {
            var pSource = source;

            Assert.That(() => MarshalUtilities.GetUtf8Span(pSource, -1).ToArray(),
                Is.EqualTo([(sbyte)'A', (sbyte)'B', (sbyte)'C'])
            );

            Assert.That(() => MarshalUtilities.GetUtf8Span(pSource, 2).ToArray(),
                Is.EqualTo([(sbyte)'A', (sbyte)'B'])
            );
        }
    }

    /// <summary>Provides validation of the <see cref="MarshalUtilities.GetUtf8Span(ref readonly sbyte, int)" /> method.</summary>
    [Test]
    public static unsafe void GetUtf8SpanFromSByteReferenceTest()
    {
        Assert.That(() => MarshalUtilities.GetUtf8Span(in UnsafeUtilities.NullRef<sbyte>(), -1).ToArray(),
            Is.EqualTo(Array.Empty<sbyte>())
        );

        var source = new sbyte[] { (sbyte)'A', (sbyte)'B', (sbyte)'C' };

        Assert.That(() => MarshalUtilities.GetUtf8Span(in source[0], -1).ToArray(),
            Is.EqualTo([(sbyte)'A', (sbyte)'B', (sbyte)'C'])
        );

        Assert.That(() => MarshalUtilities.GetUtf8Span(in source[0], 2).ToArray(),
            Is.EqualTo([(sbyte)'A', (sbyte)'B'])
        );
    }

    /// <summary>Provides validation of the <see cref="MarshalUtilities.GetUtf16Span(string?)" /> method.</summary>
    [Test]
    public static void GetUtf16SpanFromStringTest()
    {
        Assert.That(() => MarshalUtilities.GetUtf16Span(null).ToArray(),
            Is.EqualTo(Array.Empty<ushort>())
        );

        Assert.That(() => MarshalUtilities.GetUtf16Span(string.Empty).ToArray(),
            Is.EqualTo(Array.Empty<ushort>())
        );

        Assert.That(() => MarshalUtilities.GetUtf16Span("ABC").ToArray(),
            Is.EqualTo(new ushort[] { 'A', 'B', 'C' })
        );
    }

    /// <summary>Provides validation of the <see cref="MarshalUtilities.GetUtf16Span(char*, int)" /> method.</summary>
    [Test]
    public static unsafe void GetUtf16SpanFromSBytePointerTest()
    {
        Assert.That(() => MarshalUtilities.GetUtf16Span(null, -1).ToArray(),
            Is.EqualTo(Array.Empty<ushort>())
        );

        fixed (char* source = Array.Empty<char>())
        {
            var pSource = source;

            Assert.That(() => MarshalUtilities.GetUtf16Span(pSource, -1).ToArray(),
                Is.EqualTo(Array.Empty<char>())
            );
        }

        fixed (char* source = new char[] { 'A', 'B', 'C' })
        {
            var pSource = source;

            Assert.That(() => MarshalUtilities.GetUtf16Span(pSource, -1).ToArray(),
                Is.EqualTo(['A', 'B', 'C'])
            );

            Assert.That(() => MarshalUtilities.GetUtf16Span(pSource, 2).ToArray(),
                Is.EqualTo(['A', 'B'])
            );
        }
    }

    /// <summary>Provides validation of the <see cref="MarshalUtilities.GetUtf16Span(ref readonly char, int)" /> method.</summary>
    [Test]
    public static unsafe void GetUtf16SpanFromSByteReferenceTest()
    {
        Assert.That(() => MarshalUtilities.GetUtf16Span(in UnsafeUtilities.NullRef<char>(), -1).ToArray(),
            Is.EqualTo(Array.Empty<char>())
        );

        var source = new char[] { 'A', 'B', 'C' };

        Assert.That(() => MarshalUtilities.GetUtf16Span(in source[0], -1).ToArray(),
            Is.EqualTo(['A', 'B', 'C'])
        );

        Assert.That(() => MarshalUtilities.GetUtf16Span(in source[0], 2).ToArray(),
            Is.EqualTo(new ushort[] { 'A', 'B' })
        );
    }
}
