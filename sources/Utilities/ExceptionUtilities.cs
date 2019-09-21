// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for throwing exceptions.</summary>
    public static class ExceptionUtilities
    {
        /// <summary>Throws an instance of the <see cref="ArgumentException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="paramType">The type of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException"><paramref name="paramName" /> is an instance of <paramref name="paramType" />.</exception>
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForInvalidType(string paramName, Type paramType)
        {
            var message = string.Format(Resources.ArgumentExceptionForInvalidTypeMessage, paramName, paramType);
            throw new ArgumentException(message, paramName);
        }

        /// <summary>Throws an instance of the <see cref="ArgumentOutOfRangeException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="value">The value of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="paramName" /> has a value of <paramref name="value" />.</exception>
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeException(string paramName, object value)
        {
            var message = string.Format(Resources.ArgumentOutOfRangeExceptionMessage, paramName, value);
            throw new ArgumentOutOfRangeException(paramName, value, message);
        }

        /// <summary>Throws an instance of the <see cref="ExternalException" /> class.</summary>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <param name="errorCode">The error code that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an exit code of <paramref name="errorCode" />.</exception>
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowExternalException(string methodName, int errorCode)
        {
            var message = string.Format(Resources.ExternalExceptionMessage, methodName, errorCode);
            throw new ExternalException(message, errorCode);
        }

        /// <summary>Throws an instance of the <see cref="ExternalException" /> class.</summary>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an exit code of <see cref="Marshal.GetLastWin32Error()" />.</exception>
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowExternalExceptionForLastError(string methodName)
        {
            var errorCode = Marshal.GetLastWin32Error();
            var message = string.Format(Resources.ExternalExceptionMessage, methodName, errorCode);
            throw new ExternalException(message, errorCode);
        }

        /// <summary>Throws an instance of the <see cref="ExternalException" /> class.</summary>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <exception cref="ExternalException"><paramref name="methodName" /> failed with an exit code of <see cref="Marshal.GetHRForLastWin32Error()" />.</exception>
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowExternalExceptionForLastHRESULT(string methodName)
        {
            var hresult = Marshal.GetHRForLastWin32Error();
            var message = string.Format(Resources.ExternalExceptionMessage, methodName, hresult);
            throw new ExternalException(message, hresult);
        }

        /// <summary>Throws a <see cref="ArgumentNullException" /> if <paramref name="value" /> is <c>null</c>.</summary>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="value">The value to be checked for <c>null</c>.</param>
        /// <param name="paramName">The name of the parameter being checked.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull<T>([NotNull] T? value, string paramName)
            where T : class
        {
            if (value is null)
            {
                ThrowArgumentNullException(paramName);
            }
        }

        /// <summary>Throws a <see cref="InvalidOperationException" /> if <see cref="Thread.CurrentThread" /> is not <paramref name="thread" />.</summary>
        /// <param name="thread">The <see cref="Thread" /> to check against <see cref="Thread.CurrentThread" />.</param>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <paramref name="thread" />.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotThread(Thread thread)
        {
            var currentThread = Thread.CurrentThread;

            if (currentThread != thread)
            {
                ThrowInvalidOperationException(nameof(Thread.CurrentThread), currentThread);
            }
        }

        /// <summary>Throws an instance of the <see cref="IOException" /> class.</summary>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowIOException()
        {
            var message = Resources.IOExceptionMessage;
            throw new IOException(message);
        }

        /// <summary>Throws an instance of the <see cref="InvalidOperationException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="value">The value of the parameter that caused the exception.</param>
        /// <exception cref="InvalidOperationException"><paramref name="paramName" /> has a value of <paramref name="value" />.</exception>
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowInvalidOperationException(string paramName, object value)
        {
            var message = string.Format(Resources.InvalidOperationExceptionMessage, paramName, value);
            throw new InvalidOperationException(message);
        }

        /// <summary>Throws an instance of the <see cref="NotSupportedException" /> class.</summary>
        /// <exception cref="ExternalException">The collection is read-only.</exception>
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowNotSupportedExceptionForReadOnlyCollection()
        {
            var message = Resources.NotSupportedExceptionForReadOnlyCollectionMessage;
            throw new NotSupportedException(message);
        }

        /// <summary>Throws an instance of the <see cref="ObjectDisposedException" /> class.</summary>
        /// <param name="objectName">The name of the object that caused the exception.</param>
        /// <exception cref="ObjectDisposedException"><paramref name="objectName" /> is disposed.</exception>
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowObjectDisposedException(string objectName)
        {
            var message = string.Format(Resources.ObjectDisposedExceptionMessage, objectName);
            throw new ObjectDisposedException(objectName, message);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentNullException(string paramName)
        {
            var message = string.Format(Resources.ArgumentNullExceptionMessage, paramName);
            throw new ArgumentNullException(paramName, message);
        }
    }
}
