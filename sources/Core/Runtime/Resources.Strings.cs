// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Text;

namespace TerraFX.Runtime;

/// <summary>Provides a set of methods for looking up localized resources.</summary>
public static partial class Resources
{
    /// <summary>Gets a localized composite format string similar to <c>The allocation of '{0}' bytes failed</c>.</summary>
    public static CompositeFormat AllocationFailedMessage { get; } = CompositeFormat.Parse(GetString(nameof(AllocationFailedMessage)));

    /// <summary>Gets a localized composite format string similar to <c>The allocation of '{0}x{1}' bytes failed</c>.</summary>
    public static CompositeFormat ArrayAllocationFailedMessage { get; } = CompositeFormat.Parse(GetString(nameof(ArrayAllocationFailedMessage)));

    /// <summary>Gets a localized composite format string similar to <c>The queue is empty</c>.</summary>
    public static string EmptyQueueMessage { get; } = GetString(nameof(EmptyQueueMessage));

    /// <summary>Gets a localized composite format string similar to <c>The stack is empty</c>.</summary>
    public static string EmptyStackMessage { get; } = GetString(nameof(EmptyStackMessage));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' has an invalid flag combination</c>.</summary>
    public static CompositeFormat InvalidFlagCombinationMessage { get; } = CompositeFormat.Parse(GetString(nameof(InvalidFlagCombinationMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is not a valid key to '{1}'</c>.</summary>
    public static CompositeFormat InvalidKeyMessage { get; } = CompositeFormat.Parse(GetString(nameof(InvalidKeyMessage)));

    /// <summary>Gets a localized composite format string similar to <c>The kind for '{0}' is unsupported</c>.</summary>
    public static CompositeFormat InvalidKindMessage { get; } = CompositeFormat.Parse(GetString(nameof(InvalidKindMessage)));

    /// <summary>Gets a localized composite format string similar to <c>The kind for '{0}' is not '{1}'</c>.</summary>
    public static CompositeFormat InvalidKindWithExpectedKindMessage { get; } = CompositeFormat.Parse(GetString(nameof(InvalidKindWithExpectedKindMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is incompatible as it belongs to a different parent</c>.</summary>
    public static CompositeFormat InvalidParentMessage { get; } = CompositeFormat.Parse(GetString(nameof(InvalidParentMessage)));

    /// <summary>Gets a localized composite format string similar to <c>The current state is not '{0}'</c>.</summary>
    public static CompositeFormat InvalidStateMessage { get; } = CompositeFormat.Parse(GetString(nameof(InvalidStateMessage)));

    /// <summary>Gets a localized composite format string similar to <c>The current thread is not '{0}'</c>.</summary>
    public static CompositeFormat InvalidThreadMessage { get; } = CompositeFormat.Parse(GetString(nameof(InvalidThreadMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is not an instance of '{1}'</c>.</summary>
    public static CompositeFormat InvalidTypeMessage { get; } = CompositeFormat.Parse(GetString(nameof(InvalidTypeMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' failed to complete within '{1}' ms</c>.</summary>
    public static CompositeFormat MethodTimeoutMessage { get; } = CompositeFormat.Parse(GetString(nameof(MethodTimeoutMessage)));

    /// <summary>Gets a localized composite format string similar to <c>One or more of the required features is not available</c>.</summary>
    public static string MissingRequiredFeaturesMessage { get; } = GetString(nameof(MissingRequiredFeaturesMessage));

    /// <summary>Gets a localized composite format string similar to <c>The given code path is not currently implemented</c>.</summary>
    public static string NotImplementedMessage { get; } = GetString(nameof(NotImplementedMessage));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is disposed or being disposed</c>.</summary>
    public static CompositeFormat ObjectDisposedOrDisposingMessage { get; } = CompositeFormat.Parse(GetString(nameof(ObjectDisposedOrDisposingMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is not currently being disposed</c>.</summary>
    public static CompositeFormat ObjectNotDisposingMessage { get; } = CompositeFormat.Parse(GetString(nameof(ObjectNotDisposingMessage)));

    /// <summary>Gets a localized composite format string similar to <c>The previous state was not 'Disposing'</c>.</summary>
    public static string PreviousStateNotDisposingMessage { get; } = GetString(nameof(PreviousStateNotDisposingMessage));

    /// <summary>Gets a localized composite format string similar to <c>Transitioning the state from '{0}' to '{1}' failed</c>.</summary>
    public static CompositeFormat StateTransitionFailureMessage { get; } = CompositeFormat.Parse(GetString(nameof(StateTransitionFailureMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' failed with an error code of '{1}'</c>.</summary>
    public static CompositeFormat UnmanagedMethodFailedMessage { get; } = CompositeFormat.Parse(GetString(nameof(UnmanagedMethodFailedMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' has an unsupported value of '{1}'</c>.</summary>
    public static CompositeFormat UnsupportedValueMessage { get; } = CompositeFormat.Parse(GetString(nameof(UnsupportedValueMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is negative</c>.</summary>
    public static CompositeFormat ValueIsNegativeMessage { get; } = CompositeFormat.Parse(GetString(nameof(ValueIsNegativeMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is not defined</c>.</summary>
    public static CompositeFormat ValueIsNotDefinedMessage { get; } = CompositeFormat.Parse(GetString(nameof(ValueIsNotDefinedMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is negative or greater than or equal to '{1}'</c>.</summary>
    public static CompositeFormat ValueIsNotInSignedBoundsMessage { get; } = CompositeFormat.Parse(GetString(nameof(ValueIsNotInSignedBoundsMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is negative or greater than '{1}'</c>.</summary>
    public static CompositeFormat ValueIsNotInSignedInsertBoundsMessage { get; } = CompositeFormat.Parse(GetString(nameof(ValueIsNotInSignedInsertBoundsMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is greater than or equal to '{1}'</c>.</summary>
    public static CompositeFormat ValueIsNotInUnsignedBoundsMessage { get; } = CompositeFormat.Parse(GetString(nameof(ValueIsNotInUnsignedBoundsMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is greater than '{1}'</c>.</summary>
    public static CompositeFormat ValueIsNotInUnsignedInsertBoundsMessage { get; } = CompositeFormat.Parse(GetString(nameof(ValueIsNotInUnsignedInsertBoundsMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is not a power of two</c>.</summary>
    public static CompositeFormat ValueIsNotPow2Message { get; } = CompositeFormat.Parse(GetString(nameof(ValueIsNotPow2Message)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is not zero</c>.</summary>
    public static CompositeFormat ValueIsNotZeroMessage { get; } = CompositeFormat.Parse(GetString(nameof(ValueIsNotZeroMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is null</c>.</summary>
    public static CompositeFormat ValueIsNullMessage { get; } = CompositeFormat.Parse(GetString(nameof(ValueIsNullMessage)));

    /// <summary>Gets a localized composite format string similar to <c>'{0}' is zero</c>.</summary>
    public static CompositeFormat ValueIsZeroMessage { get; } = CompositeFormat.Parse(GetString(nameof(ValueIsZeroMessage)));
}
