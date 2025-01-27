// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Globalization;
using NUnit.Framework;
using TerraFX.Runtime;

namespace TerraFX.UnitTests.Runtime;

/// <summary>Provides a set of tests covering the <see cref="Resources" /> static class.</summary>
[TestFixture(TestOf = typeof(Resources))]
internal static class ResourcesTests
{
    /// <summary>Provides validation of the <see cref="Resources.AllocationFailedMessage" /> property.</summary>
    [TestCase("en", "The allocation of '{0}' bytes failed")]
    public static void AllocationFailedMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.AllocationFailedMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.ArrayAllocationFailedMessage" /> property.</summary>
    [TestCase("en", "The allocation of '{0}x{1}' bytes failed")]
    public static void ArrayAllocationFailedMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.ArrayAllocationFailedMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.EmptyQueueMessage" /> property.</summary>
    [TestCase("en", "The queue is empty")]
    public static void EmptyQueueMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.EmptyQueueMessage,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.EmptyStackMessage" /> property.</summary>
    [TestCase("en", "The stack is empty")]
    public static void EmptyStackMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.EmptyStackMessage,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.InvalidFlagCombinationMessage" /> property.</summary>
    [TestCase("en", "'{0}' has an invalid flag combination")]
    public static void InvalidFlagCombinationMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.InvalidFlagCombinationMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.InvalidKeyMessage" /> property.</summary>
    [TestCase("en", "'{0}' is not a valid key to '{1}'")]
    public static void InvalidKeyMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.InvalidKeyMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.InvalidKindMessage" /> property.</summary>
    [TestCase("en", "The kind for '{0}' is unsupported")]
    public static void InvalidKindMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.InvalidKindMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.InvalidKindWithExpectedKindMessage" /> property.</summary>
    [TestCase("en", "The kind for '{0}' is not '{1}'")]
    public static void InvalidKindWithExpectedKindMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.InvalidKindWithExpectedKindMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.InvalidParentMessage" /> property.</summary>
    [TestCase("en", "'{0}' is incompatible as it belongs to a different parent")]
    public static void InvalidParentMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.InvalidParentMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.InvalidStateMessage" /> property.</summary>
    [TestCase("en", "The current state is not '{0}'")]
    public static void InvalidStateMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.InvalidStateMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.InvalidThreadMessage" /> property.</summary>
    [TestCase("en", "The current thread is not '{0}'")]
    public static void InvalidThreadMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.InvalidThreadMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.InvalidTypeMessage" /> property.</summary>
    [TestCase("en", "'{0}' is not an instance of '{1}'")]
    public static void InvalidTypeMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.InvalidTypeMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.MethodTimeoutMessage" /> property.</summary>
    [TestCase("en", "'{0}' failed to complete within '{1}' ms")]
    public static void MethodTimeoutMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.MethodTimeoutMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.MissingRequiredFeaturesMessage" /> property.</summary>
    [TestCase("en", "One or more of the required features is not available")]
    public static void MissingRequiredFeaturesMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.MissingRequiredFeaturesMessage,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.NotImplementedMessage" /> property.</summary>
    [TestCase("en", "The given code path is not currently implemented")]
    public static void NotImplementedMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.NotImplementedMessage,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.ObjectDisposedOrDisposingMessage" /> property.</summary>
    [TestCase("en", "'{0}' is disposed or being disposed")]
    public static void ObjectDisposedOrDisposingMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.ObjectDisposedOrDisposingMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.ObjectNotDisposingMessage" /> property.</summary>
    [TestCase("en", "'{0}' is not currently being disposed")]
    public static void ObjectNotDisposingMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.ObjectNotDisposingMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.PreviousStateNotDisposingMessage" /> property.</summary>
    [TestCase("en", "The previous state was not 'Disposing'")]
    public static void PreviousStateNotDisposingMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.PreviousStateNotDisposingMessage,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.StateTransitionFailureMessage" /> property.</summary>
    [TestCase("en", "Transitioning the state from '{0}' to '{1}' failed")]
    public static void StateTransitionFailureMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.StateTransitionFailureMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.UnmanagedMethodFailedMessage" /> property.</summary>
    [TestCase("en", "'{0}' failed with an error code of '{1:X8}'")]
    public static void UnmanagedMethodFailedMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.UnmanagedMethodFailedMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.ValueIsNegativeMessage" /> property.</summary>
    [TestCase("en", "'{0}' is negative")]
    public static void ValueIsNegativeMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.ValueIsNegativeMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.ValueIsNotInSignedBoundsMessage" /> property.</summary>
    [TestCase("en", "'{0}' is negative or greater than or equal to '{1}'")]
    public static void ValueIsNotInSignedBoundsMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.ValueIsNotInSignedBoundsMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.ValueIsNotInSignedBoundsMessage" /> property.</summary>
    [TestCase("en", "'{0}' is negative or greater than '{1}'")]
    public static void ValueIsNotInSignedInsertBoundsMessage(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.ValueIsNotInSignedInsertBoundsMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.ValueIsNotInUnsignedBoundsMessage" /> property.</summary>
    [TestCase("en", "'{0}' is greater than or equal to '{1}'")]
    public static void ValueIsNotInUnsignedBoundsMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.ValueIsNotInUnsignedBoundsMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.ValueIsNotInUnsignedBoundsMessage" /> property.</summary>
    [TestCase("en", "'{0}' is greater than '{1}'")]
    public static void ValueIsNotInUnsignedInsertBoundsMessage(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.ValueIsNotInUnsignedInsertBoundsMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.ValueIsNotPow2Message" /> property.</summary>
    [TestCase("en", "'{0}' is not a power of two")]
    public static void ValueIsNotPow2MessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.ValueIsNotPow2Message.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.ValueIsNotZeroMessage" /> property.</summary>
    [TestCase("en", "'{0}' is not zero")]
    public static void ValueIsNotZeroMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.ValueIsNotZeroMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.ValueIsNullMessage" /> property.</summary>
    [TestCase("en", "'{0}' is null")]
    public static void ValueIsNullMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.ValueIsNullMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }

    /// <summary>Provides validation of the <see cref="Resources.ValueIsZeroMessage" /> property.</summary>
    [TestCase("en", "'{0}' is zero")]
    public static void ValueIsZeroMessageTest(string cultureName, string expectedMessage)
    {
        Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.That(Resources.ValueIsZeroMessage.Format,
            Is.EqualTo(expectedMessage)
        );
    }
}
