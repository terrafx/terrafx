// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="NotifyDictionaryChangedEventArgs{TKey, TValue}" /> class.</summary>
internal static class NotifyDictionaryChangedEventArgsTests
{
    /// <summary>Provides validation of the <see cref="NotifyDictionaryChangedEventArgs.ForAddAction{TKey, TValue}(TKey)" /> method.</summary>
    [Test]
    public static void ForAddActionTest()
    {
        var eventArgs = NotifyDictionaryChangedEventArgs.ForAddAction<int, string>(5);

        Assert.That(eventArgs.Action, Is.EqualTo(NotifyDictionaryChangedAction.Add));
        Assert.That(eventArgs.Key, Is.EqualTo(5));
    }

    /// <summary>Provides validation of the <see cref="NotifyDictionaryChangedEventArgs.ForRemoveAction{TKey, TValue}(TKey)" /> method.</summary>
    [Test]
    public static void ForRemoveActionTest()
    {
        var eventArgs = NotifyDictionaryChangedEventArgs.ForRemoveAction<int, string>(7);

        Assert.That(eventArgs.Action, Is.EqualTo(NotifyDictionaryChangedAction.Remove));
        Assert.That(eventArgs.Key, Is.EqualTo(7));
    }

    /// <summary>Provides validation of the <see cref="NotifyDictionaryChangedEventArgs.ForResetAction{TKey, TValue}()" /> method.</summary>
    [Test]
    public static void ForResetActionTest()
    {
        var eventArgs = NotifyDictionaryChangedEventArgs.ForResetAction<int, string>();

        Assert.That(eventArgs.Action, Is.EqualTo(NotifyDictionaryChangedAction.Reset));

        // The reset instance is cached and shared.
        Assert.That(NotifyDictionaryChangedEventArgs.ForResetAction<int, string>(), Is.SameAs(eventArgs));
    }

    /// <summary>Provides validation of the <see cref="NotifyDictionaryChangedEventArgs.ForValueChangedAction{TKey, TValue}(TKey, TValue, TValue)" /> method.</summary>
    [Test]
    public static void ForValueChangedActionTest()
    {
        var eventArgs = NotifyDictionaryChangedEventArgs.ForValueChangedAction(3, "old", "new");

        Assert.That(eventArgs.Action, Is.EqualTo(NotifyDictionaryChangedAction.ValueChanged));
        Assert.That(eventArgs.Key, Is.EqualTo(3));
        Assert.That(eventArgs.OldValue, Is.EqualTo("old"));
        Assert.That(eventArgs.NewValue, Is.EqualTo("new"));
    }
}
