// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TerraFX.Runtime;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.MarshalUtilities;

namespace TerraFX.Utilities;

public static unsafe partial class ExceptionUtilities
{
    /// <summary>Throws an <see cref="InvalidOperationException" /> for an empty queue.</summary>
    /// <exception cref="InvalidOperationException">The queue is empty.</exception>
    [DoesNotReturn]
    public static void ThrowForEmptyQueue()
    {
        var message = Resources.EmptyQueueMessage;
        ThrowInvalidOperationException(message);
    }

    /// <summary>Throws an <see cref="InvalidOperationException" /> for an empty stack.</summary>
    /// <exception cref="InvalidOperationException">The stack is empty.</exception>
    [DoesNotReturn]
    public static void ThrowForEmptyStack()
    {
        var message = Resources.EmptyStackMessage;
        ThrowInvalidOperationException(message);
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> for an invalid flags enum combination.</summary>
    /// <param name="value">The value that caused the exception.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="valueExpression" /> has an invalid flag combination.</exception>
    [DoesNotReturn]
    public static void ThrowForInvalidFlagsCombination<TEnum>(TEnum value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
        where TEnum : struct, Enum
    {
        AssertNotNull(valueExpression);
        var message = string.Format(CultureInfo.InvariantCulture, Resources.InvalidFlagCombinationMessage, valueExpression);
        ThrowArgumentOutOfRangeException(valueExpression, value, message);
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> for an invalid enum kind.</summary>
    /// <typeparam name="TEnum">The type of <paramref name="value" />.</typeparam>
    /// <param name="value">The value that caused the exception.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ArgumentOutOfRangeException">The kind for <paramref name="valueExpression" /> is unsupported.</exception>
    [DoesNotReturn]
    public static void ThrowForInvalidKind<TEnum>(TEnum value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
        where TEnum : struct, Enum
    {
        AssertNotNull(valueExpression);
        var message = string.Format(CultureInfo.InvariantCulture, Resources.InvalidKindMessage, valueExpression);
        ThrowArgumentOutOfRangeException(valueExpression, value, message);
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" />.</summary>
    /// <typeparam name="TEnum">The type of <paramref name="value" /> and <paramref name="expectedValue" />.</typeparam>
    /// <param name="value">The value that caused the exception.</param>
    /// <param name="expectedValue">The expected value of <paramref name="value" />.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ArgumentOutOfRangeException">The kind for <paramref name="valueExpression" /> is not <paramref name="expectedValue" />.</exception>
    [DoesNotReturn]
    public static void ThrowForInvalidKind<TEnum>(TEnum value, TEnum expectedValue, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
        where TEnum : struct, Enum
    {
        AssertNotNull(valueExpression);
        var message = string.Format(CultureInfo.InvariantCulture, Resources.InvalidKindWithExpectedKindMessage, valueExpression, expectedValue);
        ThrowArgumentOutOfRangeException(valueExpression, value, message);
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> for an invalid parent.</summary>
    /// <param name="value">The value that caused the exception.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="valueExpression" /> is incompatible as it belongs to a different parent.</exception>
    [DoesNotReturn]
    public static void ThrowForInvalidParent<T>(T value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        AssertNotNull(valueExpression);
        var message = string.Format(CultureInfo.InvariantCulture, Resources.InvalidParentMessage, valueExpression);
        ThrowArgumentOutOfRangeException(valueExpression, value, message);
    }

    /// <summary>Throws an <see cref="InvalidOperationException" /> for an invalid state.</summary>
    /// <param name="expectedStateName">The name expected state.</param>
    /// <exception cref="ArgumentException">The current state is not <paramref name="expectedStateName" />.</exception>
    [DoesNotReturn]
    public static void ThrowForInvalidState(string expectedStateName)
    {
        var message = string.Format(CultureInfo.InvariantCulture, Resources.InvalidStateMessage, expectedStateName);
        ThrowInvalidOperationException(message);
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> for an invalid type.</summary>
    /// <param name="type">The object that caused the exception.</param>
    /// <param name="expectedType">The expected type of <paramref name="type" />.</param>
    /// <param name="typeExpression">The expression of the object that is not <paramref name="expectedType" />.</param>
    /// <exception cref="ArgumentException"><paramref name="typeExpression" /> is not an instance of <paramref name="expectedType" />.</exception>
    [DoesNotReturn]
    public static void ThrowForInvalidType(Type type, Type expectedType, [CallerArgumentExpression(nameof(type))] string? typeExpression = null)
    {
        AssertNotNull(typeExpression);
        var message = string.Format(CultureInfo.InvariantCulture, Resources.InvalidTypeMessage, typeExpression, expectedType);
        ThrowArgumentOutOfRangeException(typeExpression, type, message);
    }

    /// <summary>Throws an <see cref="ExternalException" /> using the last available error code.</summary>
    /// <param name="methodName">The name of the method that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an exit code of <see cref="Marshal.GetLastWin32Error()" />.</exception>
    [DoesNotReturn]
    public static void ThrowForLastError(string methodName)
        => ThrowExternalException(methodName, GetLastSystemError());

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is not <c>zero</c>.</summary>
    /// <param name="value">The value to compare against zero to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfNotZero(int value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value != 0)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is not <c>zero</c>.</summary>
    /// <param name="value">The value to compare against zero to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfNotZero(long value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value != 0)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is not <c>zero</c>.</summary>
    /// <param name="value">The value to compare against zero to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfNotZero(nint value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value != 0)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is not <c>zero</c>.</summary>
    /// <param name="value">The value to compare against zero to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfNotZero(uint value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value != 0)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is not <c>zero</c>.</summary>
    /// <param name="value">The value to compare against zero to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfNotZero(ulong value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value != 0)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is not <c>zero</c>.</summary>
    /// <param name="value">The value to compare against zero to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfNotZero(nuint value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value != 0)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is <c>null</c>.</summary>
    /// <param name="value">The value to compare against null to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfNull(void* value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value == null)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
    /// <param name="value">The value to compare against zero to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfZero(int value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value == 0)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
    /// <param name="value">The value to compare against zero to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfZero(long value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value == 0)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
    /// <param name="value">The value to compare against zero to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfZero(nint value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value == 0)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
    /// <param name="value">The value to compare against zero to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfZero(uint value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value == 0)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
    /// <param name="value">The value to compare against zero to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfZero(ulong value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value == 0)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
    /// <param name="value">The value to compare against zero to determine failure.</param>
    /// <param name="valueExpression">The expression of the value that caused the exception.</param>
    /// <exception cref="ExternalException"><paramref name="valueExpression" /> failed with an error code of <paramref name="value" />.</exception>
    public static void ThrowForLastErrorIfZero(nuint value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        if (value == 0)
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, GetLastSystemError());
        }
    }

    /// <summary>Throws a <see cref="NotSupportedException" />.</summary>
    /// <exception cref="NotSupportedException">One or more of the required features is not available</exception>
    [DoesNotReturn]
    public static void ThrowForMissingFeature()
        => throw new NotSupportedException(Resources.MissingRequiredFeaturesMessage);

    /// <summary>Throws a <see cref="ArgumentOutOfRangeException" /> for an unsupported value.</summary>
    /// <param name="value">The value that caused the exception.</param>
    /// <param name="valueExpression">The expression of the value that is not supported.</param>
    /// <exception cref="ArgumentException"><paramref name="valueExpression" /> has an unsupported value of <paramref name="value" />.</exception>
    [DoesNotReturn]
    public static void ThrowForUnsupportedValue<T>(T value, [CallerArgumentExpression(nameof(value))] string? valueExpression = null)
    {
        AssertNotNull(valueExpression);
        var message = string.Format(CultureInfo.InvariantCulture, Resources.UnsupportedValueMessage, value);
        ThrowArgumentOutOfRangeException(valueExpression, value, message);
    }
}
