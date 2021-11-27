// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Runtime;
using TerraFX.Threading;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.MarshalUtilities;

namespace TerraFX.Utilities;

public static unsafe partial class ExceptionUtilities
{
    /// <summary>Throws an <see cref="ObjectDisposedException" /> if <paramref name="state" /> is <see cref="VolatileState.Disposed" /> or <see cref="VolatileState.Disposing" />.</summary>
    /// <param name="state">The state being checked.</param>
    /// <param name="valueName">The name of the value that was disposed.</param>
    /// <exception cref="ObjectDisposedException"><paramref name="valueName" /> is disposed or being disposed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfDisposedOrDisposing(VolatileState state, string valueName)
    {
        if (state.IsDisposedOrDisposing)
        {
            ThrowObjectDisposedException(valueName);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>negative</c>.</summary>
    /// <param name="value">The value to be checked if it is <c>negative</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>negative</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNegative(int value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value < 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsNegativeMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>negative</c>.</summary>
    /// <param name="value">The value to be checked if it is <c>negative</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>negative</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNegative(long value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value < 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsNegativeMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>negative</c>.</summary>
    /// <param name="value">The value to be checked if it is <c>negative</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>negative</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNegative(nint value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value < 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsNegativeMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="index" /> is <c>negative</c> or greater than or equal to <paramref name="length" />.</summary>
    /// <param name="index">The index to check.</param>
    /// <param name="length">The length of the collection being indexed.</param>
    /// <param name="indexExpression">The expression of the index being checked.</param>
    /// <param name="lengthExpression">The expression of the length being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexExpression" /> is <c>negative</c> or greater than or equal to <paramref name="lengthExpression" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotInBounds(int index, int length, [CallerArgumentExpression("index")] string? indexExpression = null, [CallerArgumentExpression("length")] string? lengthExpression = null)
    {
        if (unchecked((uint)index >= (uint)length))
        {
            AssertNotNull(indexExpression);
            AssertNotNull(lengthExpression);

            var message = string.Format(Resources.ValueIsNotInSignedBoundsMessage, indexExpression, lengthExpression);
            ThrowArgumentOutOfRangeException(indexExpression, index, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="index" /> is <c>negative</c> or greater than or equal to <paramref name="length" />.</summary>
    /// <param name="index">The index to check.</param>
    /// <param name="length">The length of the collection being indexed.</param>
    /// <param name="indexExpression">The expression of the index being checked.</param>
    /// <param name="lengthExpression">The expression of the length being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexExpression" /> is <c>negative</c> or greater than or equal to <paramref name="lengthExpression" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotInBounds(long index, long length, [CallerArgumentExpression("index")] string? indexExpression = null, [CallerArgumentExpression("length")] string? lengthExpression = null)
    {
        if (unchecked((ulong)index >= (ulong)length))
        {
            AssertNotNull(indexExpression);
            AssertNotNull(lengthExpression);

            var message = string.Format(Resources.ValueIsNotInSignedBoundsMessage, indexExpression, lengthExpression);
            ThrowArgumentOutOfRangeException(indexExpression, index, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="index" /> is <c>negative</c> or greater than or equal to <paramref name="length" />.</summary>
    /// <param name="index">The index to check.</param>
    /// <param name="length">The length of the collection being indexed.</param>
    /// <param name="indexExpression">The expression of the index being checked.</param>
    /// <param name="lengthExpression">The expression of the length being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexExpression" /> is <c>negative</c> or greater than or equal to <paramref name="lengthExpression" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotInBounds(nint index, nint length, [CallerArgumentExpression("index")] string? indexExpression = null, [CallerArgumentExpression("length")] string? lengthExpression = null)
    {
        if (unchecked((nuint)index >= (nuint)length))
        {
            AssertNotNull(indexExpression);
            AssertNotNull(lengthExpression);

            var message = string.Format(Resources.ValueIsNotInSignedBoundsMessage, indexExpression, lengthExpression);
            ThrowArgumentOutOfRangeException(indexExpression, index, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="index" /> is greater than or equal to <paramref name="length" />.</summary>
    /// <param name="index">The index to check.</param>
    /// <param name="length">The length of the collection being indexed.</param>
    /// <param name="indexExpression">The name of the index being checked.</param>
    /// <param name="lengthExpression">The name of the length being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexExpression" /> is greater than or equal to <paramref name="lengthExpression" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotInBounds(uint index, uint length, [CallerArgumentExpression("index")] string? indexExpression = null, [CallerArgumentExpression("length")] string? lengthExpression = null)
    {
        if (index >= length)
        {
            AssertNotNull(indexExpression);
            AssertNotNull(lengthExpression);

            var message = string.Format(Resources.ValueIsNotInUnsignedBoundsMessage, indexExpression, lengthExpression);
            ThrowArgumentOutOfRangeException(indexExpression, index, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="index" /> is greater than or equal to <paramref name="length" />.</summary>
    /// <param name="index">The index to check.</param>
    /// <param name="length">The length of the collection being indexed.</param>
    /// <param name="indexExpression">The name of the index being checked.</param>
    /// <param name="lengthExpression">The name of the length being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexExpression" /> is greater than or equal to <paramref name="lengthExpression" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotInBounds(ulong index, ulong length, [CallerArgumentExpression("index")] string? indexExpression = null, [CallerArgumentExpression("length")] string? lengthExpression = null)
    {
        if (index >= length)
        {
            AssertNotNull(indexExpression);
            AssertNotNull(lengthExpression);

            var message = string.Format(Resources.ValueIsNotInUnsignedBoundsMessage, indexExpression, lengthExpression);
            ThrowArgumentOutOfRangeException(indexExpression, index, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="index" /> is greater than or equal to <paramref name="length" />.</summary>
    /// <param name="index">The index to check.</param>
    /// <param name="length">The length of the collection being indexed.</param>
    /// <param name="indexExpression">The name of the index being checked.</param>
    /// <param name="lengthExpression">The name of the length being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexExpression" /> is greater than or equal to <paramref name="lengthExpression" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotInBounds(nuint index, nuint length, [CallerArgumentExpression("index")] string? indexExpression = null, [CallerArgumentExpression("length")] string? lengthExpression = null)
    {
        if (index >= length)
        {
            AssertNotNull(indexExpression);
            AssertNotNull(lengthExpression);

            var message = string.Format(Resources.ValueIsNotInUnsignedBoundsMessage, indexExpression, lengthExpression);
            ThrowArgumentOutOfRangeException(indexExpression, index, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="index" /> is <c>negative</c> or greater than <paramref name="length" />.</summary>
    /// <param name="index">The index to check.</param>
    /// <param name="length">The length of the collection being indexed.</param>
    /// <param name="indexExpression">The name of the index being checked.</param>
    /// <param name="lengthExpression">The name of the length being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexExpression" /> is <c>negative</c> or greater than <paramref name="lengthExpression" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotInInsertBounds(int index, int length, [CallerArgumentExpression("index")] string? indexExpression = null, [CallerArgumentExpression("length")] string? lengthExpression = null)
    {
        if (unchecked((uint)index > (uint)length))
        {
            AssertNotNull(indexExpression);
            AssertNotNull(lengthExpression);

            var message = string.Format(Resources.ValueIsNotInSignedBoundsMessage, indexExpression, lengthExpression);
            ThrowArgumentOutOfRangeException(indexExpression, index, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="index" /> is <c>negative</c> or greater than <paramref name="length" />.</summary>
    /// <param name="index">The index to check.</param>
    /// <param name="length">The length of the collection being indexed.</param>
    /// <param name="indexExpression">The name of the index being checked.</param>
    /// <param name="lengthExpression">The name of the length being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexExpression" /> is <c>negative</c> or greater than <paramref name="lengthExpression" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotInInsertBounds(long index, long length, [CallerArgumentExpression("index")] string? indexExpression = null, [CallerArgumentExpression("length")] string? lengthExpression = null)
    {
        if (unchecked((ulong)index > (ulong)length))
        {
            AssertNotNull(indexExpression);
            AssertNotNull(lengthExpression);

            var message = string.Format(Resources.ValueIsNotInSignedBoundsMessage, indexExpression, lengthExpression);
            ThrowArgumentOutOfRangeException(indexExpression, index, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="index" /> is <c>negative</c> or greater than <paramref name="length" />.</summary>
    /// <param name="index">The index to check.</param>
    /// <param name="length">The length of the collection being indexed.</param>
    /// <param name="indexExpression">The name of the index being checked.</param>
    /// <param name="lengthExpression">The name of the length being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexExpression" /> is <c>negative</c> or greater than <paramref name="lengthExpression" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotInInsertBounds(nint index, nint length, [CallerArgumentExpression("index")] string? indexExpression = null, [CallerArgumentExpression("length")] string? lengthExpression = null)
    {
        if (unchecked((nuint)index > (nuint)length))
        {
            AssertNotNull(indexExpression);
            AssertNotNull(lengthExpression);

            var message = string.Format(Resources.ValueIsNotInSignedBoundsMessage, indexExpression, lengthExpression);
            ThrowArgumentOutOfRangeException(indexExpression, index, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="index" /> is greater than <paramref name="length" />.</summary>
    /// <param name="index">The index to check.</param>
    /// <param name="length">The length of the collection being indexed.</param>
    /// <param name="indexExpression">The name of the index being checked.</param>
    /// <param name="lengthExpression">The name of the length being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexExpression" /> is greater than <paramref name="lengthExpression" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotInInsertBounds(uint index, uint length, [CallerArgumentExpression("index")] string? indexExpression = null, [CallerArgumentExpression("length")] string? lengthExpression = null)
    {
        if (index > length)
        {
            AssertNotNull(indexExpression);
            AssertNotNull(lengthExpression);

            var message = string.Format(Resources.ValueIsNotInUnsignedBoundsMessage, indexExpression, lengthExpression);
            ThrowArgumentOutOfRangeException(indexExpression, index, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="index" /> is greater than <paramref name="length" />.</summary>
    /// <param name="index">The index to check.</param>
    /// <param name="length">The length of the collection being indexed.</param>
    /// <param name="indexExpression">The name of the index being checked.</param>
    /// <param name="lengthExpression">The name of the length being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexExpression" /> is greater than <paramref name="lengthExpression" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotInInsertBounds(ulong index, ulong length, [CallerArgumentExpression("index")] string? indexExpression = null, [CallerArgumentExpression("length")] string? lengthExpression = null)
    {
        if (index > length)
        {
            AssertNotNull(indexExpression);
            AssertNotNull(lengthExpression);

            var message = string.Format(Resources.ValueIsNotInUnsignedBoundsMessage, indexExpression, lengthExpression);
            ThrowArgumentOutOfRangeException(indexExpression, index, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="index" /> is greater than <paramref name="length" />.</summary>
    /// <param name="index">The index to check.</param>
    /// <param name="length">The length of the collection being indexed.</param>
    /// <param name="indexExpression">The name of the index being checked.</param>
    /// <param name="lengthExpression">The name of the length being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexExpression" /> is greater than <paramref name="lengthExpression" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotInInsertBounds(nuint index, nuint length, [CallerArgumentExpression("index")] string? indexExpression = null, [CallerArgumentExpression("length")] string? lengthExpression = null)
    {
        if (index > length)
        {
            AssertNotNull(indexExpression);
            AssertNotNull(lengthExpression);

            var message = string.Format(Resources.ValueIsNotInUnsignedBoundsMessage, indexExpression, lengthExpression);
            ThrowArgumentOutOfRangeException(indexExpression, index, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not a <c>power of two</c>.</summary>
    /// <param name="value">The value to check.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not a <c>power of two</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotPow2(uint value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (!MathUtilities.IsPow2(value))
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsNotPow2Message, value);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not a <c>power of two</c>.</summary>
    /// <param name="value">The value to check.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not a <c>power of two</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotPow2(ulong value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (!MathUtilities.IsPow2(value))
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsNotPow2Message, value);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not a <c>power of two</c>.</summary>
    /// <param name="value">The value to check.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not a <c>power of two</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotPow2(nuint value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (!MathUtilities.IsPow2(value))
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsNotPow2Message, value);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="InvalidOperationException" /> if <see cref="Thread.CurrentThread" /> is not <paramref name="expectedThread" />.</summary>
    /// <param name="expectedThread">The thread to check that the code is running on.</param>
    /// <exception cref="InvalidOperationException">The current thread is not <paramref name="expectedThread" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotThread(Thread expectedThread)
    {
        if (Thread.CurrentThread != expectedThread)
        {
            var message = string.Format(Resources.InvalidThreadMessage, expectedThread);
            ThrowInvalidOperationException(message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not <c>zero</c>.</summary>
    /// <param name="value">The value to be checked if it is not <c>zero</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotZero(int value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value != 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsNotZeroMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not <c>zero</c>.</summary>
    /// <param name="value">The value to be checked if it is not <c>zero</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotZero(long value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value != 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsNotZeroMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not <c>zero</c>.</summary>
    /// <param name="value">The value to be checked if it is not <c>zero</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotZero(nint value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value != 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsNotZeroMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not <c>zero</c>.</summary>
    /// <param name="value">The value to be checked if it is not <c>zero</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotZero(uint value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value != 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsNotZeroMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not <c>zero</c>.</summary>
    /// <param name="value">The value to be checked if it is not <c>zero</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotZero(ulong value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value != 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsNotZeroMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not <c>zero</c>.</summary>
    /// <param name="value">The value to be checked if it is not <c>zero</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotZero(nuint value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value != 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsNotZeroMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentNullException" /> if <paramref name="value" /> is <c>null</c>.</summary>
    /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
    /// <param name="value">The value to be checked for <c>null</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNull<T>([NotNull] T? value, [CallerArgumentExpression("value")] string? valueExpression = null)
        where T : class
    {
        if (value is null)
        {
            AssertNotNull(valueExpression);
            ThrowArgumentNullException(valueExpression);
        }
    }

    /// <summary>Throws an <see cref="ArgumentNullException" /> if <paramref name="array" /> is <c>null</c>.</summary>
    /// <typeparam name="T">The type of items in <paramref name="array" />.</typeparam>
    /// <param name="array">The array to be checked for <c>null</c>.</param>
    /// <param name="valueExpression">The expression of the array being checked.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNull<T>(UnmanagedArray<T> array, [CallerArgumentExpression("array")] string? valueExpression = null)
        where T : unmanaged
    {
        if (array.IsNull)
        {
            AssertNotNull(valueExpression);
            ThrowArgumentNullException(valueExpression);
        }
    }

    /// <summary>Throws a <see cref="ArgumentNullException" /> if <paramref name="value" /> is <c>null</c>.</summary>
    /// <param name="value">The value to be checked for <c>null</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNull(void* value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value == null)
        {
            AssertNotNull(valueExpression);
            ThrowArgumentNullException(valueExpression);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
    /// <param name="value">The value to be checked if it is <c>zero</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfZero(int value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value == 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsZeroMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
    /// <param name="value">The value to be checked if it is <c>zero</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfZero(long value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value == 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsZeroMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
    /// <param name="value">The value to be checked if it is <c>zero</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfZero(nint value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value == 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsZeroMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
    /// <param name="value">The value to be checked if it is <c>zero</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfZero(uint value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value == 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsZeroMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
    /// <param name="value">The value to be checked if it is <c>zero</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfZero(ulong value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value == 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsZeroMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
    /// <param name="value">The value to be checked if it is <c>zero</c>.</param>
    /// <param name="valueExpression">The expression of the value being checked.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>zero</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfZero(nuint value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if (value == 0)
        {
            AssertNotNull(valueExpression);
            var message = string.Format(Resources.ValueIsZeroMessage, valueExpression);
            ThrowArgumentOutOfRangeException(valueExpression, value, message);
        }
    }
}
