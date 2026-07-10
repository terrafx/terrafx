// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="NotifyCollectionChangedEventArgs{T}" /> class.</summary>
internal static class NotifyCollectionChangedEventArgsTests
{
    /// <summary>Provides validation of the <see cref="NotifyCollectionChangedEventArgs.ForAddAction{T}(T)" /> method.</summary>
    [Test]
    public static void ForAddActionTest()
    {
        var eventArgs = NotifyCollectionChangedEventArgs.ForAddAction(5);

        Assert.That(eventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
        Assert.That(eventArgs.Value, Is.EqualTo(5));
    }

    /// <summary>Provides validation of the <see cref="NotifyCollectionChangedEventArgs.ForRemoveAction{T}(T)" /> method.</summary>
    [Test]
    public static void ForRemoveActionTest()
    {
        var eventArgs = NotifyCollectionChangedEventArgs.ForRemoveAction(7);

        Assert.That(eventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
        Assert.That(eventArgs.Value, Is.EqualTo(7));
    }

    /// <summary>Provides validation of the <see cref="NotifyCollectionChangedEventArgs.ForResetAction{T}()" /> method.</summary>
    [Test]
    public static void ForResetActionTest()
    {
        var eventArgs = NotifyCollectionChangedEventArgs.ForResetAction<int>();

        Assert.That(eventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Reset));

        // The reset instance is cached and shared.
        Assert.That(NotifyCollectionChangedEventArgs.ForResetAction<int>(), Is.SameAs(eventArgs));
    }
}
