// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Globalization;
using NUnit.Framework;
using TerraFX.Runtime;

namespace TerraFX.UnitTests.Runtime
{
    /// <summary>Provides a set of tests covering the <see cref="Resources" /> static class.</summary>
    [TestFixture(TestOf = typeof(Resources))]
    public static class ResourcesTests
    {
        /// <summary>Provides validation of the <see cref="Resources.AllocationFailedMessage" /> property.</summary>
        [TestCase("en", "The allocation of '{0}' bytes failed")]
        public static void AllocationFailedMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.AllocationFailedMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.ArrayAllocationFailedMessage" /> property.</summary>
        [TestCase("en", "The allocation of '{0}x{1}' bytes failed")]
        public static void ArrayAllocationFailedMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.ArrayAllocationFailedMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.InvalidFlagCombinationMessage" /> property.</summary>
        [TestCase("en", "'{0}' has an invalid flag combination")]
        public static void InvalidFlagCombinationMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.InvalidFlagCombinationMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.InvalidKeyMessage" /> property.</summary>
        [TestCase("en", "'{0}' is not a valid key to '{1}'")]
        public static void InvalidKeyMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.InvalidKeyMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.InvalidKindMessage" /> property.</summary>
        [TestCase("en", "The kind for '{0}' is unsupported")]
        public static void InvalidKindMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.InvalidKindMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.InvalidKindWithExpectedKindMessage" /> property.</summary>
        [TestCase("en", "The kind for '{0}' is not '{1}'")]
        public static void InvalidKindWithExpectedKindMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.InvalidKindWithExpectedKindMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.InvalidParentMessage" /> property.</summary>
        [TestCase("en", "'{0}' is incompatible as it belongs to a different parent")]
        public static void InvalidParentMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.InvalidParentMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.InvalidStateMessage" /> property.</summary>
        [TestCase("en", "The current state is not '{0}'")]
        public static void InvalidStateMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.InvalidStateMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.InvalidThreadMessage" /> property.</summary>
        [TestCase("en", "The current thread is not '{0}'")]
        public static void InvalidThreadMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.InvalidThreadMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.InvalidTypeMessage" /> property.</summary>
        [TestCase("en", "'{0}' is not an instance of '{1}'")]
        public static void InvalidTypeMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.InvalidTypeMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.MethodTimeoutMessage" /> property.</summary>
        [TestCase("en", "'{0}' failed to complete within '{1}' ms")]
        public static void MethodTimeoutMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.MethodTimeoutMessage,
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

            Assert.That(Resources.ObjectDisposedOrDisposingMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.ObjectNotDisposingMessage" /> property.</summary>
        [TestCase("en", "'{0}' is not currently being disposed")]
        public static void ObjectNotDisposingMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.ObjectNotDisposingMessage,
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

            Assert.That(Resources.StateTransitionFailureMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.UnmanagedMethodFailedMessage" /> property.</summary>
        [TestCase("en", "'{0}' failed with an error code of '{1}'")]
        public static void UnmanagedMethodFailedMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.UnmanagedMethodFailedMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.UnsupportedSurfaceKindMessage" /> property.</summary>
        [TestCase("en", "'{0}' is not a supported GraphicsSurfaceKind")]
        public static void UnsupportedSurfaceKindMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.UnsupportedSurfaceKindMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.ValueIsNegativeMessage" /> property.</summary>
        [TestCase("en", "'{0}' is negative")]
        public static void ValueIsNegativeMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.ValueIsNegativeMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.ValueIsNotInSignedBoundsMessage" /> property.</summary>
        [TestCase("en", "'{0}' is negative or greater than or equal to '{1}'")]
        public static void ValueIsNotInBoundsSignedMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.ValueIsNotInSignedBoundsMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.ValueIsNotPow2Message" /> property.</summary>
        [TestCase("en", "'{0}' is not a power of two")]
        public static void ValueIsNotPow2MessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.ValueIsNotPow2Message,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.ValueIsNullMessage" /> property.</summary>
        [TestCase("en", "'{0}' is null")]
        public static void ValueIsNullMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.ValueIsNullMessage,
                Is.EqualTo(expectedMessage)
            );
        }

        /// <summary>Provides validation of the <see cref="Resources.ValueIsZeroMessage" /> property.</summary>
        [TestCase("en", "'{0}' is zero")]
        public static void ValueIsZeroMessageTest(string cultureName, string expectedMessage)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(cultureName);

            Assert.That(Resources.ValueIsZeroMessage,
                Is.EqualTo(expectedMessage)
            );
        }
    }
}
