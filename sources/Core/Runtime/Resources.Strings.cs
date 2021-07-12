// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Runtime
{
    /// <summary>Provides a set of methods for looking up localized resources.</summary>
    public static partial class Resources
    {
        /// <summary>Gets a localized string similar to <c>The allocation of '{0}' bytes failed</c>.</summary>
        public static string AllocationFailedMessage => GetString(nameof(AllocationFailedMessage));

        /// <summary>Gets a localized string similar to <c>The allocation of '{0}x{1}' bytes failed</c>.</summary>
        public static string ArrayAllocationFailedMessage => GetString(nameof(ArrayAllocationFailedMessage));

        /// <summary>Gets a localized string similar to <c>The queue is empty</c>.</summary>
        public static string EmptyQueueMessage => GetString(nameof(EmptyQueueMessage));

        /// <summary>Gets a localized string similar to <c>The stack is empty</c>.</summary>
        public static string EmptyStackMessage => GetString(nameof(EmptyStackMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' has an invalid flag combination</c>.</summary>
        public static string InvalidFlagCombinationMessage => GetString(nameof(InvalidFlagCombinationMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is not a valid key to '{1}'</c>.</summary>
        public static string InvalidKeyMessage => GetString(nameof(InvalidKeyMessage));

        /// <summary>Gets a localized string similar to <c>The kind for '{0}' is unsupported</c>.</summary>
        public static string InvalidKindMessage => GetString(nameof(InvalidKindMessage));

        /// <summary>Gets a localized string similar to <c>The kind for '{0}' is not '{1}'</c>.</summary>
        public static string InvalidKindWithExpectedKindMessage => GetString(nameof(InvalidKindWithExpectedKindMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is incompatible as it belongs to a different parent</c>.</summary>
        public static string InvalidParentMessage => GetString(nameof(InvalidParentMessage));

        /// <summary>Gets a localized string similar to <c>The current state is not '{0}'</c>.</summary>
        public static string InvalidStateMessage => GetString(nameof(InvalidStateMessage));

        /// <summary>Gets a localized string similar to <c>The current thread is not '{0}'</c>.</summary>
        public static string InvalidThreadMessage => GetString(nameof(InvalidThreadMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is not an instance of '{1}'</c>.</summary>
        public static string InvalidTypeMessage => GetString(nameof(InvalidTypeMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' failed to complete within '{1}' ms</c>.</summary>
        public static string MethodTimeoutMessage => GetString(nameof(MethodTimeoutMessage));

        /// <summary>Gets a localized string similar to <c>One or more of the required features is not available</c>.</summary>
        public static string MissingRequiredFeaturesMessage => GetString(nameof(MissingRequiredFeaturesMessage));

        /// <summary>Gets a localized string similar to <c>The given code path is not currently implemented</c>.</summary>
        public static string NotImplementedMessage => GetString(nameof(NotImplementedMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is disposed or being disposed</c>.</summary>
        public static string ObjectDisposedOrDisposingMessage => GetString(nameof(ObjectDisposedOrDisposingMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is not currently being disposed</c>.</summary>
        public static string ObjectNotDisposingMessage => GetString(nameof(ObjectNotDisposingMessage));

        /// <summary>Gets a localized string similar to <c>The previous state was not 'Disposing'</c>.</summary>
        public static string PreviousStateNotDisposingMessage => GetString(nameof(PreviousStateNotDisposingMessage));

        /// <summary>Gets a localized string similar to <c>Transitioning the state from '{0}' to '{1}' failed</c>.</summary>
        public static string StateTransitionFailureMessage => GetString(nameof(StateTransitionFailureMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' failed with an error code of '{1}'</c>.</summary>
        public static string UnmanagedMethodFailedMessage => GetString(nameof(UnmanagedMethodFailedMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is not a supported GraphicsSurfaceKind</c>.</summary>
        public static string UnsupportedSurfaceKindMessage => GetString(nameof(UnsupportedSurfaceKindMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is the unsupported type '{1}'</c>.</summary>
        public static string UnsupportedTypeMessage => GetString(nameof(UnsupportedTypeMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is negative</c>.</summary>
        public static string ValueIsNegativeMessage => GetString(nameof(ValueIsNegativeMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is negative or greater than or equal to '{1}'</c>.</summary>
        public static string ValueIsNotInSignedBoundsMessage => GetString(nameof(ValueIsNotInSignedBoundsMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is negative or greater than '{1}'</c>.</summary>
        public static string ValueIsNotInSignedInsertBoundsMessage => GetString(nameof(ValueIsNotInSignedInsertBoundsMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is greater than or equal to '{1}'</c>.</summary>
        public static string ValueIsNotInUnsignedBoundsMessage => GetString(nameof(ValueIsNotInUnsignedBoundsMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is greater than '{1}'</c>.</summary>
        public static string ValueIsNotInUnsignedInsertBoundsMessage => GetString(nameof(ValueIsNotInUnsignedInsertBoundsMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is not a power of two</c>.</summary>
        public static string ValueIsNotPow2Message => GetString(nameof(ValueIsNotPow2Message));

        /// <summary>Gets a localized string similar to <c>'{0}' is not zero</c>.</summary>
        public static string ValueIsNotZeroMessage => GetString(nameof(ValueIsNotZeroMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is null</c>.</summary>
        public static string ValueIsNullMessage => GetString(nameof(ValueIsNullMessage));

        /// <summary>Gets a localized string similar to <c>'{0}' is zero</c>.</summary>
        public static string ValueIsZeroMessage => GetString(nameof(ValueIsZeroMessage));
    }
}
