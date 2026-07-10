// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;

namespace TerraFX.UnitTests;

/// <summary>Provides a set of tests covering the <see cref="PropertyChangedEventArgs{T}" /> class.</summary>
internal static class PropertyChangedEventArgsTests
{
    /// <summary>Provides validation of the <see cref="PropertyChangedEventArgs{T}.PropertyChangedEventArgs(T, T)" /> constructor and the <see cref="PropertyChangedEventArgs{T}.PreviousValue" /> and <see cref="PropertyChangedEventArgs{T}.CurrentValue" /> properties.</summary>
    [Test]
    public static void CtorTest()
    {
        var eventArgs = new PropertyChangedEventArgs<int>(previousValue: 1, currentValue: 2);

        Assert.That(eventArgs.PreviousValue, Is.EqualTo(1));
        Assert.That(eventArgs.CurrentValue, Is.EqualTo(2));
    }
}
