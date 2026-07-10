// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;
using TerraFX.Threading;

namespace TerraFX.UnitTests.Threading;

/// <summary>Provides a set of tests covering the <see cref="VolatileState" /> struct.</summary>
internal static class VolatileStateTests
{
    /// <summary>Provides validation that a default instance is in the uninitialized state.</summary>
    [Test]
    public static void DefaultStateTest()
    {
        var state = default(VolatileState);

        Assert.That((uint)state, Is.EqualTo(VolatileState.Uninitialized));
        Assert.That(state.IsDisposedOrDisposing, Is.False);
        Assert.That(state.IsNotDisposedOrDisposing, Is.True);
    }

    /// <summary>Provides validation of the <see cref="VolatileState.Transition(uint)" /> method.</summary>
    [Test]
    public static void TransitionTest()
    {
        var state = default(VolatileState);

        var previousState = state.Transition(VolatileState.Initialized);

        Assert.That(previousState, Is.EqualTo(VolatileState.Uninitialized));
        Assert.That((uint)state, Is.EqualTo(VolatileState.Initialized));
    }

    /// <summary>Provides validation that <see cref="VolatileState.TryTransition(uint, uint)" /> succeeds from the expected state.</summary>
    [Test]
    public static void TryTransitionSuccessTest()
    {
        var state = default(VolatileState);

        var previousState = state.TryTransition(VolatileState.Uninitialized, VolatileState.Initialized);

        Assert.That(previousState, Is.EqualTo(VolatileState.Uninitialized));
        Assert.That((uint)state, Is.EqualTo(VolatileState.Initialized));
    }

    /// <summary>Provides validation that <see cref="VolatileState.TryTransition(uint, uint)" /> is a no-op from an unexpected state.</summary>
    [Test]
    public static void TryTransitionFailureTest()
    {
        var state = default(VolatileState);

        // The current state is Uninitialized, so a transition expecting Initialized does nothing.
        var previousState = state.TryTransition(VolatileState.Initialized, VolatileState.Disposed);

        Assert.That(previousState, Is.EqualTo(VolatileState.Uninitialized));
        Assert.That((uint)state, Is.EqualTo(VolatileState.Uninitialized));
    }

    /// <summary>Provides validation that <see cref="VolatileState.Transition(uint, uint)" /> succeeds from the expected state.</summary>
    [Test]
    public static void TransitionFromToSuccessTest()
    {
        var state = default(VolatileState);

        Assert.That(() => state.Transition(VolatileState.Uninitialized, VolatileState.Initialized), Throws.Nothing);
        Assert.That((uint)state, Is.EqualTo(VolatileState.Initialized));
    }

    /// <summary>Provides validation that <see cref="VolatileState.Transition(uint, uint)" /> throws from an unexpected state.</summary>
    [Test]
    public static void TransitionFromToFailureThrowsTest()
    {
        var state = default(VolatileState);

        // The current state is Uninitialized, so a transition expecting Initialized throws.
        Assert.That(() => state.Transition(VolatileState.Initialized, VolatileState.Disposed),
            Throws.InvalidOperationException
        );

        Assert.That((uint)state, Is.EqualTo(VolatileState.Uninitialized));
    }

    /// <summary>Provides validation of the <see cref="VolatileState.BeginDispose()" /> and <see cref="VolatileState.EndDispose()" /> methods.</summary>
    [Test]
    public static void BeginEndDisposeTest()
    {
        var state = default(VolatileState);
        _ = state.Transition(VolatileState.Initialized);

        var previousState = state.BeginDispose();

        Assert.That(previousState, Is.EqualTo(VolatileState.Initialized));
        Assert.That((uint)state, Is.EqualTo(VolatileState.Disposing));
        Assert.That(state.IsDisposedOrDisposing, Is.True);
        Assert.That(state.IsNotDisposedOrDisposing, Is.False);

        state.EndDispose();

        Assert.That((uint)state, Is.EqualTo(VolatileState.Disposed));
        Assert.That(state.IsDisposedOrDisposing, Is.True);
    }

    /// <summary>Provides validation of the <see cref="VolatileState.op_Equality" /> and <see cref="VolatileState.op_Inequality" /> methods.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        var left = default(VolatileState);
        var right = default(VolatileState);

        Assert.That(left == right, Is.True);
        Assert.That(left != right, Is.False);

        _ = left.Transition(VolatileState.Initialized);

        Assert.That(left == right, Is.False);
        Assert.That(left != right, Is.True);
    }

    /// <summary>Provides validation of the <see cref="VolatileState.Equals(VolatileState)" /> and <see cref="VolatileState.GetHashCode()" /> methods.</summary>
    [Test]
    public static void EqualsAndGetHashCodeTest()
    {
        var state = default(VolatileState);
        _ = state.Transition(VolatileState.Initialized);

        var other = default(VolatileState);
        _ = other.Transition(VolatileState.Initialized);

        Assert.That(state.Equals(other), Is.True);
        Assert.That(state.Equals((object)other), Is.True);
        Assert.That(state.Equals(42), Is.False);
        Assert.That(state.GetHashCode(), Is.EqualTo(other.GetHashCode()));
    }
}
