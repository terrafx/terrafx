// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using TerraFX.Runtime;
using TerraFX.Threading;
using TerraFX.Utilities;

namespace TerraFX.UnitTests.Utilities;

/// <summary>Provides a set of tests covering the <see cref="AssertionUtilities" /> static class.</summary>
[TestFixture(TestOf = typeof(AssertionUtilities))]
public static class AssertionUtilitiesTests
{
    /// <summary>Provides validation of the <see cref="AssertionUtilities.Assert(bool, string?)" /> method.</summary>
    [Test]
    public static void AssertTest()
    {
        Assert.That(() => AssertionUtilities.Assert(false),
            Configuration.IsDebug ? Throws.Exception : Throws.Nothing
        );

        Assert.That(() => AssertionUtilities.Assert(true),
            Throws.Nothing
        );
    }

    /// <summary>Provides validation of the <see cref="AssertionUtilities.AssertDisposing(VolatileState)" /> method.</summary>
    [Test]
    public static void AssertDisposingTest()
    {
        var state = new VolatileState();

        Assert.That(() => AssertionUtilities.AssertDisposing(state),
            Configuration.IsDebug ? Throws.Exception : Throws.Nothing
        );

        _ = state.BeginDispose();

        Assert.That(() => AssertionUtilities.AssertDisposing(state),
            Throws.Nothing
        );

        state.EndDispose();

        Assert.That(() => AssertionUtilities.AssertDisposing(state),
            Configuration.IsDebug ? Throws.Exception : Throws.Nothing
        );
    }

    /// <summary>Provides validation of the <see cref="AssertionUtilities.AssertNotDisposedOrDisposing(VolatileState)" /> method.</summary>
    [Test]
    public static void AssertNotDisposedOrDisposingTest()
    {
        var state = new VolatileState();

        Assert.That(() => AssertionUtilities.AssertNotDisposedOrDisposing(state),
            Throws.Nothing
        );

        _ = state.BeginDispose();

        Assert.That(() => AssertionUtilities.AssertNotDisposedOrDisposing(state),
            Configuration.IsDebug ? Throws.Exception : Throws.Nothing
        );

        state.EndDispose();

        Assert.That(() => AssertionUtilities.AssertNotDisposedOrDisposing(state),
            Configuration.IsDebug ? Throws.Exception : Throws.Nothing
        );
    }

    /// <summary>Provides validation of the <see cref="AssertionUtilities.AssertNotNull{T}(T)" /> method.</summary>
    [Test]
    public static void AssertNotNullObjectTest()
    {
        Assert.That(() => AssertionUtilities.AssertNotNull(string.Empty),
            Throws.Nothing
        );

        Assert.That(() => AssertionUtilities.AssertNotNull<object>(null),
            Configuration.IsDebug ? Throws.Exception : Throws.Nothing
        );
    }

    /// <summary>Provides validation of the <see cref="AssertionUtilities.AssertNotNull(void*)" /> method.</summary>
    [Test]
    public static unsafe void AssertNotNullPointerTest()
    {
        Assert.That(() => AssertionUtilities.AssertNotNull((void*)1),
            Throws.Nothing
        );

        Assert.That(() => AssertionUtilities.AssertNotNull(null),
            Configuration.IsDebug ? Throws.Exception : Throws.Nothing
        );
    }

    /// <summary>Provides validation of the <see cref="AssertionUtilities.AssertNotNull{T}(UnmanagedArray{T})" /> method.</summary>
    [Test]
    public static unsafe void AssertNotNullUnmanagedArrayTest()
    {
        Assert.That(() => AssertionUtilities.AssertNotNull(UnmanagedArray<int>.Empty),
            Throws.Nothing
        );

        Assert.That(() => AssertionUtilities.AssertNotNull(new UnmanagedArray<int>()),
            Configuration.IsDebug ? Throws.Exception : Throws.Nothing
        );
    }

    /// <summary>Provides validation of the <see cref="AssertionUtilities.AssertThread(Thread)" /> method.</summary>
    [Test]
    public static unsafe void AssertThreadTest()
    {
        Assert.That(() => AssertionUtilities.AssertThread(Thread.CurrentThread),
            Throws.Nothing
        );

        Assert.That(() => AssertionUtilities.AssertThread(null!),
            Configuration.IsDebug ? Throws.Exception : Throws.Nothing
        );
    }

    /// <summary>Provides validation of the <see cref="AssertionUtilities.Fail" /> method.</summary>
    [Test]
    public static unsafe void FailTest()
    {
        Assert.That(() => AssertionUtilities.Fail(),
            Configuration.IsDebug ? Throws.InstanceOf<UnreachableException>() : Throws.Nothing
        );
    }
}
