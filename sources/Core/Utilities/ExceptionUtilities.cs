// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Runtime;
using TerraFX.Threading;
using static TerraFX.Utilities.MarshalUtilities;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for throwing exceptions.</summary>
    public static unsafe class ExceptionUtilities
    {
        /// <summary>Throws an <see cref="ArgumentException" />.</summary>
        /// <param name="message">The message detailing the cause of the exception.</param>
        /// <param name="valueName">The name of the value that caused the exception.</param>
        /// <exception cref="ArgumentException"><paramref name="message" /></exception>
        [DoesNotReturn]
        public static void ThrowArgumentException(string message, string valueName)
            => throw new ArgumentException(message, valueName);

        /// <summary>Throws an <see cref="ArgumentNullException" />.</summary>
        /// <param name="valueName">The name of the value that is <c>null</c>.</param>
        /// <exception cref="InvalidOperationException"><paramref name="valueName" /> is <c>null</c>.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string valueName)
        {
            var message = string.Format(Resources.ValueIsNullMessage, valueName);
            throw new ArgumentNullException(valueName, message);
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException" />.</summary>
        /// <param name="message">The message detailing the cause of the exception.</param>
        /// <param name="valueName">The name of the value that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="message" /></exception>
        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string message, string valueName)
            => throw new ArgumentOutOfRangeException(valueName, message);

        /// <summary>Throws an <see cref="ExternalException" />.</summary>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <param name="errorCode">The underlying error code for the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="errorCode" />.</exception>
        [DoesNotReturn]
        public static void ThrowExternalException(string methodName, int errorCode)
        {
            var message = string.Format(Resources.UnmanagedMethodFailedMessage, methodName, errorCode);
            throw new ExternalException(message, errorCode);
        }

        /// <summary>Throws an <see cref="ArgumentException" />.</summary>
        /// <param name="valueName">The name of the value that caused the exception.</param>
        /// <exception cref="ArgumentException"><paramref name="valueName" /> has an invalid flag combination.</exception>
        [DoesNotReturn]
        public static void ThrowForInvalidFlagsCombination(string valueName)
        {
            var message = string.Format(Resources.InvalidFlagCombinationMessage, valueName);
            ThrowArgumentException(message, valueName);
        }

        /// <summary>Throws an <see cref="ArgumentException" />.</summary>
        /// <typeparam name="TEnum">The type of <paramref name="expectedKind" />.</typeparam>
        /// <param name="valueName">The name of the value that caused the exception.</param>
        /// <param name="expectedKind">The expected kind of <paramref name="valueName" />.</param>
        /// <exception cref="ArgumentException">The kind for <paramref name="valueName" /> is not <paramref name="expectedKind" />.</exception>
        [DoesNotReturn]
        public static void ThrowForInvalidKind<TEnum>(string valueName, TEnum expectedKind)
            where TEnum : struct, Enum
        {
            var message = string.Format(Resources.InvalidKindMessage, valueName, expectedKind.ToString());
            ThrowArgumentException(message, valueName);
        }

        /// <summary>Throws an <see cref="ArgumentException" />.</summary>
        /// <param name="valueName">The name of the value that caused the exception.</param>
        /// <exception cref="ArgumentException"><paramref name="valueName" /> is incompatible as it belongs to a different parent.</exception>
        [DoesNotReturn]
        public static void ThrowForInvalidParent(string valueName)
        {
            var message = string.Format(Resources.InvalidParentMessage, valueName);
            ThrowArgumentException(message, valueName);
        }

        /// <summary>Throws an <see cref="InvalidOperationException" />.</summary>
        /// <param name="expectedStateName">The name expected state.</param>
        /// <exception cref="ArgumentException">The current state is not <paramref name="expectedStateName" />.</exception>
        [DoesNotReturn]
        public static void ThrowForInvalidState(string expectedStateName)
        {
            var message = string.Format(Resources.InvalidStateMessage, expectedStateName);
            ThrowInvalidOperationException(message);
        }

        /// <summary>Throws an <see cref="ArgumentException" />.</summary>
        /// <param name="valueName">The name of the value that is not <paramref name="expectedType" />.</param>
        /// <param name="expectedType">The expected type of the value.</param>
        /// <exception cref="ArgumentException"><paramref name="valueName" /> is not an instance of <paramref name="expectedType" />.</exception>
        [DoesNotReturn]
        public static void ThrowForInvalidType(string valueName, Type expectedType)
        {
            var message = string.Format(Resources.InvalidTypeMessage, valueName, expectedType);
            ThrowArgumentException(message, valueName);
        }

        /// <summary>Throws an <see cref="ExternalException" /> using the last available error code.</summary>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an exit code of <see cref="Marshal.GetLastWin32Error()" />.</exception>
        [DoesNotReturn]
        public static void ThrowForLastError(string methodName)
            => ThrowExternalException(methodName, GetLastError());

        /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="result" /> is not <c>zero</c>.</summary>
        /// <param name="result">The underlying error code for the exception.</param>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="result" />.</exception>
        public static void ThrowForLastErrorIfNotZero(int result, string methodName)
        {
            if (result != 0)
            {
                ThrowExternalException(methodName, GetLastError());
            }
        }

        /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="result" /> is not <c>zero</c>.</summary>
        /// <param name="result">The underlying error code for the exception.</param>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="result" />.</exception>
        public static void ThrowForLastErrorIfNotZero(long result, string methodName)
        {
            if (result != 0)
            {
                ThrowExternalException(methodName, GetLastError());
            }
        }

        /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="result" /> is not <c>zero</c>.</summary>
        /// <param name="result">The underlying error code for the exception.</param>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="result" />.</exception>
        public static void ThrowForLastErrorIfNotZero(nint result, string methodName)
        {
            if (result != 0)
            {
                ThrowExternalException(methodName, GetLastError());
            }
        }

        /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="result" /> is not <c>zero</c>.</summary>
        /// <param name="result">The underlying error code for the exception.</param>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="result" />.</exception>
        public static void ThrowForLastErrorIfNotZero(uint result, string methodName)
        {
            if (result != 0)
            {
                ThrowExternalException(methodName, GetLastError());
            }
        }

        /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="result" /> is not <c>zero</c>.</summary>
        /// <param name="result">The underlying error code for the exception.</param>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="result" />.</exception>
        public static void ThrowForLastErrorIfNotZero(ulong result, string methodName)
        {
            if (result != 0)
            {
                ThrowExternalException(methodName, GetLastError());
            }
        }

        /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="result" /> is not <c>zero</c>.</summary>
        /// <param name="result">The underlying error code for the exception.</param>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="result" />.</exception>
        public static void ThrowForLastErrorIfNotZero(nuint result, string methodName)
        {
            if (result != 0)
            {
                ThrowExternalException(methodName, GetLastError());
            }
        }

        /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="result" /> is <c>zero</c>.</summary>
        /// <param name="result">The underlying error code for the exception.</param>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="result" />.</exception>
        public static void ThrowForLastErrorIfZero(int result, string methodName)
        {
            if (result == 0)
            {
                ThrowExternalException(methodName, GetLastError());
            }
        }

        /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="result" /> is <c>zero</c>.</summary>
        /// <param name="result">The underlying error code for the exception.</param>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="result" />.</exception>
        public static void ThrowForLastErrorIfZero(long result, string methodName)
        {
            if (result == 0)
            {
                ThrowExternalException(methodName, GetLastError());
            }
        }

        /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="result" /> is <c>zero</c>.</summary>
        /// <param name="result">The underlying error code for the exception.</param>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="result" />.</exception>
        public static void ThrowForLastErrorIfZero(nint result, string methodName)
        {
            if (result == 0)
            {
                ThrowExternalException(methodName, GetLastError());
            }
        }

        /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="result" /> is <c>zero</c>.</summary>
        /// <param name="result">The underlying error code for the exception.</param>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="result" />.</exception>
        public static void ThrowForLastErrorIfZero(uint result, string methodName)
        {
            if (result == 0)
            {
                ThrowExternalException(methodName, GetLastError());
            }
        }

        /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="result" /> is <c>zero</c>.</summary>
        /// <param name="result">The underlying error code for the exception.</param>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="result" />.</exception>
        public static void ThrowForLastErrorIfZero(ulong result, string methodName)
        {
            if (result == 0)
            {
                ThrowExternalException(methodName, GetLastError());
            }
        }

        /// <summary>Throws an <see cref="ExternalException" /> if <paramref name="result" /> is <c>zero</c>.</summary>
        /// <param name="result">The underlying error code for the exception.</param>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an error code of <paramref name="result" />.</exception>
        public static void ThrowForLastErrorIfZero(nuint result, string methodName)
        {
            if (result == 0)
            {
                ThrowExternalException(methodName, GetLastError());
            }
        }

        /// <summary>Throws a <see cref="NotSupportedException" />.</summary>
        /// <exception cref="NotSupportedException">One or more of the required features is not available</exception>
        [DoesNotReturn]
        public static void ThrowForMissingFeature()
            => throw new NotSupportedException(Resources.MissingRequiredFeaturesMessage);

        /// <summary>Throws a <see cref="NotSupportedException" />.</summary>
        /// <param name="surfaceName">The name of the surface that is not available.</param>
        /// <exception cref="NotSupportedException">One or more of the required features is not available</exception>
        [DoesNotReturn]
        public static void ThrowForUnsupportedSurfaceKind(string surfaceName)
        {
            var message = string.Format(Resources.UnsupportedSurfaceKindMessage, surfaceName);
            throw new NotSupportedException(message);
        }

        /// <summary>Throws if <see cref="Configuration.AssertionsEnabled" /> is <c>false</c>.</summary>
        /// <exception cref="InvalidOperationException">TerraFX based assertions are disabled.</exception>
        /// <remarks>
        ///     <para>This throw exists to help catch accidental side-effects.</para>
        ///     <para>Users can avoid this exception by ensuring they do <c>Assert(!AssertionsEnabled &amp;&amp; condition)</c>.</para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfAssertionsDisabled()
        {
            if (!Configuration.AssertionsEnabled)
            {
                ThrowInvalidOperationException(Resources.AssertionsDisabledMessage);
            }
        }

        /// <summary>Throws an <see cref="ObjectDisposedException" /> if <paramref name="state" /> is <see cref="VolatileState.Disposed" /> or <see cref="VolatileState.Disposing" />.</summary>
        /// <param name="state">The state being checked.</param>
        /// <param name="valueName">The name of the value being checked.</param>
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
        /// <param name="valueName">The name of the value being checked.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>negative</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNegative(int value, string valueName)
        {
            if (value < 0)
            {
                var message = string.Format(Resources.ValueIsNegativeMessage, valueName);
                ThrowArgumentOutOfRangeException(message, valueName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>negative</c>.</summary>
        /// <param name="value">The value to be checked if it is <c>negative</c>.</param>
        /// <param name="valueName">The name of the value being checked.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>negative</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNegative(long value, string valueName)
        {
            if (value < 0)
            {
                var message = string.Format(Resources.ValueIsNegativeMessage, valueName);
                ThrowArgumentOutOfRangeException(message, valueName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>negative</c>.</summary>
        /// <param name="value">The value to be checked if it is <c>negative</c>.</param>
        /// <param name="valueName">The name of the value being checked.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>negative</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNegative(nint value, string valueName)
        {
            if (value < 0)
            {
                var message = string.Format(Resources.ValueIsNegativeMessage, valueName);
                ThrowArgumentOutOfRangeException(message, valueName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not a <c>power of two</c>.</summary>
        /// <param name="value">The value to check.</param>
        /// <param name="valueName">The name of <paramref name="value" />.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not a <c>power of two</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotPow2(uint value, string valueName)
        {
            if (!MathUtilities.IsPow2(value))
            {
                var message = string.Format(Resources.ValueIsNotPow2Message, value);
                ThrowArgumentOutOfRangeException(message, valueName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not a <c>power of two</c>.</summary>
        /// <param name="value">The value to check.</param>
        /// <param name="valueName">The name of <paramref name="value" />.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not a <c>power of two</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotPow2(ulong value, string valueName)
        {
            if (!MathUtilities.IsPow2(value))
            {
                var message = string.Format(Resources.ValueIsNotPow2Message, value);
                ThrowArgumentOutOfRangeException(message, valueName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not a <c>power of two</c>.</summary>
        /// <param name="value">The value to check.</param>
        /// <param name="valueName">The name of <paramref name="value" />.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not a <c>power of two</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotPow2(nuint value, string valueName)
        {
            if (!MathUtilities.IsPow2(value))
            {
                var message = string.Format(Resources.ValueIsNotPow2Message, value);
                ThrowArgumentOutOfRangeException(message, valueName);
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

        /// <summary>Throws an <see cref="ArgumentNullException" /> if <paramref name="value" /> is <c>null</c>.</summary>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="value">The value to be checked for <c>null</c>.</param>
        /// <param name="valueName">The name of the value being checked.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull<T>([NotNull] T? value, string valueName)
            where T : class
        {
            if (value is null)
            {
                ThrowArgumentNullException(valueName);
            }
        }

        /// <summary>Throws a <see cref="ArgumentNullException" /> if <paramref name="value" /> is <c>null</c>.</summary>
        /// <param name="value">The value to be checked for <c>null</c>.</param>
        /// <param name="valueName">The name of the value being checked.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(void* value, string valueName)
        {
            if (value == null)
            {
                ThrowArgumentNullException(valueName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
        /// <param name="value">The value to be checked if it is <c>zero</c>.</param>
        /// <param name="valueName">The name of the value being checked.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>zero</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfZero(int value, string valueName)
        {
            if (value < 0)
            {
                var message = string.Format(Resources.ValueIsZeroMessage, valueName);
                ThrowArgumentOutOfRangeException(message, valueName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
        /// <param name="value">The value to be checked if it is <c>zero</c>.</param>
        /// <param name="valueName">The name of the value being checked.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>zero</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfZero(long value, string valueName)
        {
            if (value < 0)
            {
                var message = string.Format(Resources.ValueIsZeroMessage, valueName);
                ThrowArgumentOutOfRangeException(message, valueName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
        /// <param name="value">The value to be checked if it is <c>zero</c>.</param>
        /// <param name="valueName">The name of the value being checked.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>zero</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfZero(nint value, string valueName)
        {
            if (value == 0)
            {
                var message = string.Format(Resources.ValueIsZeroMessage, valueName);
                ThrowArgumentOutOfRangeException(message, valueName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
        /// <param name="value">The value to be checked if it is <c>zero</c>.</param>
        /// <param name="valueName">The name of the value being checked.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>zero</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfZero(uint value, string valueName)
        {
            if (value < 0)
            {
                var message = string.Format(Resources.ValueIsZeroMessage, valueName);
                ThrowArgumentOutOfRangeException(message, valueName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
        /// <param name="value">The value to be checked if it is <c>zero</c>.</param>
        /// <param name="valueName">The name of the value being checked.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>zero</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfZero(ulong value, string valueName)
        {
            if (value < 0)
            {
                var message = string.Format(Resources.ValueIsZeroMessage, valueName);
                ThrowArgumentOutOfRangeException(message, valueName);
            }
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is <c>zero</c>.</summary>
        /// <param name="value">The value to be checked if it is <c>zero</c>.</param>
        /// <param name="valueName">The name of the value being checked.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is <c>zero</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfZero(nuint value, string valueName)
        {
            if (value == 0)
            {
                var message = string.Format(Resources.ValueIsZeroMessage, valueName);
                ThrowArgumentOutOfRangeException(message, valueName);
            }
        }

        /// <summary>Throws a <see cref="InvalidOperationException" />.</summary>
        /// <param name="message">The message detailing the cause of the exception.</param>
        /// <exception cref="InvalidOperationException"><paramref name="message" />.</exception>
        [DoesNotReturn]
        public static void ThrowInvalidOperationException(string message)
            => throw new InvalidOperationException(message);

        /// <summary>Throws a <see cref="KeyNotFoundException" />.</summary>
        /// <param name="key">The key that is missing from <paramref name="collectionName" />.</param>
        /// <param name="collectionName">The name of the collection which does not contain <paramref name="key" />.</param>
        /// <exception cref="KeyNotFoundException"><paramref name="key" /> is not a valid key to <paramref name="collectionName"/>.</exception>
        [DoesNotReturn]
        public static void ThrowKeyNotFoundException(object key, string collectionName)
        {
            var message = string.Format(Resources.InvalidKeyMessage, key, collectionName);
            throw new KeyNotFoundException(message);
        }

        /// <summary>Throws a <see cref="NotImplementedException" />.</summary>
        /// <exception cref="NotImplementedException">The given code path is not currently implemented.</exception>
        [DoesNotReturn]
        public static void ThrowNotImplementedException()
            => throw new NotImplementedException(Resources.NotImplementedMessage);

        /// <summary>Throws an <see cref="ObjectDisposedException" />.</summary>
        /// <param name="valueName">The name of the value that is <see cref="VolatileState.Disposed" /> or <see cref="VolatileState.Disposing" />.</param>
        /// <exception cref="InvalidOperationException"><paramref name="valueName" /> is disposed or being disposed.</exception>
        [DoesNotReturn]
        public static void ThrowObjectDisposedException(string valueName)
        {
            var message = string.Format(Resources.ObjectDisposedOrDisposingMessage, valueName);
            throw new ObjectDisposedException(message);
        }

        /// <summary>Throws an <see cref="OutOfMemoryException" />.</summary>
        /// <param name="size">The size, in bytes, of the failed allocation.</param>
        /// <exception cref="OutOfMemoryException">The allocation of <paramref name="size" /> bytes failed.</exception>
        [DoesNotReturn]
        public static void ThrowOutOfMemoryException(ulong size)
        {
            var message = string.Format(Resources.AllocationFailedMessage, size);
            throw new InvalidOperationException(message);
        }

        /// <summary>Throws an <see cref="OutOfMemoryException" />.</summary>
        /// <param name="size">The size, in bytes, of the failed allocation.</param>
        /// <exception cref="OutOfMemoryException">The allocation of <paramref name="size" /> bytes failed.</exception>
        [DoesNotReturn]
        public static void ThrowOutOfMemoryException(nuint size)
        {
            var message = string.Format(Resources.AllocationFailedMessage, size);
            throw new InvalidOperationException(message);
        }

        /// <summary>Throws an <see cref="OutOfMemoryException" />.</summary>
        /// <param name="count">The count, in elements, of the failed allocation.</param>
        /// <param name="size">The size, in bytes, of the elements in the failed allocation.</param>
        /// <exception cref="OutOfMemoryException">The allocation of <paramref name="size" /> bytes failed.</exception>
        [DoesNotReturn]
        public static void ThrowOutOfMemoryException(ulong count, ulong size)
        {
            var message = string.Format(Resources.ArrayAllocationFailedMessage, count, size);
            throw new InvalidOperationException(message);
        }

        /// <summary>Throws an <see cref="OutOfMemoryException" />.</summary>
        /// <param name="count">The count, in elements, of the failed allocation.</param>
        /// <param name="size">The size, in bytes, of the elements in the failed allocation.</param>
        /// <exception cref="OutOfMemoryException">The allocation of <paramref name="size" /> bytes failed.</exception>
        [DoesNotReturn]
        public static void ThrowOutOfMemoryException(nuint count, nuint size)
        {
            var message = string.Format(Resources.ArrayAllocationFailedMessage, count, size);
            throw new InvalidOperationException(message);
        }

        /// <summary>Throws a <see cref="TimeoutException" />.</summary>
        /// <param name="methodName">The name of the method that failed to complete within <paramref name="millisecondsTimeout" />.</param>
        /// <param name="millisecondsTimeout">The timeout, in milliseconds, for <paramref name="methodName"/>.</param>
        /// <exception cref="TimeoutException"><paramref name="methodName" /> failed to complete within <paramref name="millisecondsTimeout" /> ms.</exception>
        [DoesNotReturn]
        public static void ThrowTimeoutException(string methodName, int millisecondsTimeout)
        {
            var message = string.Format(Resources.MethodTimeoutMessage, methodName, millisecondsTimeout);
            throw new TimeoutException(message);
        }

        /// <summary>Throws a <see cref="TimeoutException" />.</summary>
        /// <param name="methodName">The name of the method that failed to complete within <paramref name="timeout" />.</param>
        /// <param name="timeout">The timeout for <paramref name="methodName"/>.</param>
        /// <exception cref="TimeoutException"><paramref name="methodName" /> failed to complete within <paramref name="timeout" /> ms.</exception>
        [DoesNotReturn]
        public static void ThrowTimeoutException(string methodName, TimeSpan timeout)
        {
            var message = string.Format(Resources.MethodTimeoutMessage, methodName, timeout.TotalMilliseconds);
            throw new TimeoutException(message);
        }
    }
}
