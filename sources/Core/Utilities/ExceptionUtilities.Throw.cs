// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using TerraFX.Runtime;
using TerraFX.Threading;

namespace TerraFX.Utilities;

/// <summary>Provides a set of methods for throwing exceptions.</summary>
public static partial class ExceptionUtilities
{
    /// <summary>Throws an <see cref="ArgumentException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <param name="paramName">The name of the value that caused the exception.</param>
    /// <exception cref="ArgumentException"><paramref name="message" /></exception>
    [DoesNotReturn]
    public static void ThrowArgumentException(string message, string paramName)
        => throw new ArgumentException(message, paramName);

    /// <summary>Throws an <see cref="ArgumentException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <param name="paramName">The name of the value that caused the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    /// <exception cref="ArgumentException"><paramref name="message" /></exception>
    [DoesNotReturn]
    public static void ThrowArgumentException(string message, string paramName, Exception innerException)
        => throw new ArgumentException(message, paramName, innerException);

    /// <summary>Throws an <see cref="ArgumentNullException" />.</summary>
    /// <param name="paramName">The name of the value that is <c>null</c>.</param>
    /// <exception cref="InvalidOperationException"><paramref name="paramName" /> is <c>null</c>.</exception>
    [DoesNotReturn]
    public static void ThrowArgumentNullException(string paramName)
    {
        var message = string.Format(Resources.ValueIsNullMessage, paramName);
        throw new ArgumentNullException(paramName, message);
    }

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" />.</summary>
    /// <typeparam name="T">The type of <paramref name="actualValue" />.</typeparam>
    /// <param name="paramName">The name of the value that caused the exception.</param>
    /// <param name="actualValue">The value that caused the exception.</param>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="message" /></exception>
    [DoesNotReturn]
    public static void ThrowArgumentOutOfRangeException<T>(string paramName, T actualValue, string message)
        => throw new ArgumentOutOfRangeException(paramName, actualValue, message);

    /// <summary>Throws an <see cref="DirectoryNotFoundException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <exception cref="DirectoryNotFoundException"><paramref name="message" />.</exception>
    [DoesNotReturn]
    public static void ThrowDirectoryNotFoundException(string message)
        => throw new DirectoryNotFoundException(message);

    /// <summary>Throws an <see cref="DirectoryNotFoundException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    /// <exception cref="DirectoryNotFoundException"><paramref name="message" />.</exception>
    [DoesNotReturn]
    public static void ThrowDirectoryNotFoundException(string message, Exception innerException)
        => throw new DirectoryNotFoundException(message, innerException);

    /// <summary>Throws an <see cref="ExternalException" />.</summary>
    /// <param name="methodName">The name of the method that caused the exception.</param>
    /// <param name="errorCode">The underlying error code for the exception.</param>
    /// <exception cref="ExternalException">'<paramref name="methodName" />' failed with an error code of '<paramref name="errorCode" />'.</exception>
    [DoesNotReturn]
    public static void ThrowExternalException(string methodName, int errorCode)
    {
        var message = string.Format(Resources.UnmanagedMethodFailedMessage, methodName, errorCode);
        throw new ExternalException(message, errorCode);
    }

    /// <summary>Throws an <see cref="FileNotFoundException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <exception cref="FileNotFoundException"><paramref name="message" />.</exception>
    [DoesNotReturn]
    public static void ThrowFileNotFoundException(string message)
        => throw new FileNotFoundException(message);

    /// <summary>Throws an <see cref="FileNotFoundException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    /// <exception cref="FileNotFoundException"><paramref name="message" />.</exception>
    [DoesNotReturn]
    public static void ThrowFileNotFoundException(string message, Exception innerException)
        => throw new FileNotFoundException(message, innerException);

    /// <summary>Throws an <see cref="FileNotFoundException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <param name="fileName">The full name of the file.</param>
    /// <exception cref="FileNotFoundException"><paramref name="message" />.</exception>
    [DoesNotReturn]
    public static void ThrowFileNotFoundException(string message, string fileName)
        => throw new FileNotFoundException(message, fileName);

    /// <summary>Throws an <see cref="FileNotFoundException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <param name="fileName">The full name of the file.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    /// <exception cref="FileNotFoundException"><paramref name="message" />.</exception>
    [DoesNotReturn]
    public static void ThrowFileNotFoundException(string message, string fileName, Exception innerException)
        => throw new FileNotFoundException(message, fileName, innerException);

    /// <summary>Throws a <see cref="InvalidOperationException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <exception cref="InvalidOperationException"><paramref name="message" />.</exception>
    [DoesNotReturn]
    public static void ThrowInvalidOperationException(string message)
        => throw new InvalidOperationException(message);

    /// <summary>Throws a <see cref="IOException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <exception cref="IOException"><paramref name="message" />.</exception>
    [DoesNotReturn]
    public static void ThrowIOException(string message)
        => throw new IOException(message);

    /// <summary>Throws a <see cref="IOException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    /// <exception cref="IOException"><paramref name="message" />.</exception>
    [DoesNotReturn]
    public static void ThrowIOException(string message, Exception innerException)
        => throw new IOException(message, innerException);

    /// <summary>Throws a <see cref="IOException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <param name="hresult">An integer identifying the error that has occurred.</param>
    /// <exception cref="IOException"><paramref name="message" />.</exception>
    [DoesNotReturn]
    public static void ThrowIOException(string message, int hresult)
        => throw new IOException(message, hresult);

    /// <summary>Throws a <see cref="KeyNotFoundException" />.</summary>
    /// <param name="key">The key that is missing from <paramref name="collectionName" />.</param>
    /// <param name="collectionName">The name of the collection which does not contain <paramref name="key" />.</param>
    /// <exception cref="KeyNotFoundException"><paramref name="key" /> is not a valid key to <paramref name="collectionName"/>.</exception>
    [DoesNotReturn]
    public static void ThrowKeyNotFoundException<TKey>(TKey key, string collectionName)
        where TKey : notnull
    {
        var message = string.Format(Resources.InvalidKeyMessage, key, collectionName);
        throw new KeyNotFoundException(message);
    }

    /// <summary>Throws a <see cref="KeyNotFoundException" />.</summary>
    /// <param name="key">The key that is missing from <paramref name="collectionName" />.</param>
    /// <param name="collectionName">The name of the collection which does not contain <paramref name="key" />.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    /// <exception cref="KeyNotFoundException"><paramref name="key" /> is not a valid key to <paramref name="collectionName"/>.</exception>
    [DoesNotReturn]
    public static void ThrowKeyNotFoundException<TKey>(TKey key, string collectionName, Exception innerException)
        where TKey : notnull
    {
        var message = string.Format(Resources.InvalidKeyMessage, key, collectionName);
        throw new KeyNotFoundException(message, innerException);
    }

    /// <summary>Throws a <see cref="NotImplementedException" />.</summary>
    /// <exception cref="NotImplementedException">The given code path is not currently implemented.</exception>
    [DoesNotReturn]
    public static void ThrowNotImplementedException()
        => throw new NotImplementedException(Resources.NotImplementedMessage);

    /// <summary>Throws a <see cref="NotImplementedException" />.</summary>
    /// <exception cref="NotImplementedException">The given code path is not currently implemented.</exception>
    [DoesNotReturn]
    public static void ThrowNotImplementedException(Exception innerException)
        => throw new NotImplementedException(Resources.NotImplementedMessage, innerException);

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
        throw new OutOfMemoryException(message);
    }

    /// <summary>Throws an <see cref="OutOfMemoryException" />.</summary>
    /// <param name="size">The size, in bytes, of the failed allocation.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    /// <exception cref="OutOfMemoryException">The allocation of <paramref name="size" /> bytes failed.</exception>
    [DoesNotReturn]
    public static void ThrowOutOfMemoryException(ulong size, Exception innerException)
    {
        var message = string.Format(Resources.AllocationFailedMessage, size);
        throw new OutOfMemoryException(message, innerException);
    }

    /// <summary>Throws an <see cref="OutOfMemoryException" />.</summary>
    /// <param name="size">The size, in bytes, of the failed allocation.</param>
    /// <exception cref="OutOfMemoryException">The allocation of <paramref name="size" /> bytes failed.</exception>
    [DoesNotReturn]
    public static void ThrowOutOfMemoryException(nuint size)
    {
        var message = string.Format(Resources.AllocationFailedMessage, size);
        throw new OutOfMemoryException(message);
    }

    /// <summary>Throws an <see cref="OutOfMemoryException" />.</summary>
    /// <param name="size">The size, in bytes, of the failed allocation.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    /// <exception cref="OutOfMemoryException">The allocation of <paramref name="size" /> bytes failed.</exception>
    [DoesNotReturn]
    public static void ThrowOutOfMemoryException(nuint size, Exception innerException)
    {
        var message = string.Format(Resources.AllocationFailedMessage, size);
        throw new OutOfMemoryException(message, innerException);
    }

    /// <summary>Throws an <see cref="OutOfMemoryException" />.</summary>
    /// <param name="count">The count, in elements, of the failed allocation.</param>
    /// <param name="size">The size, in bytes, of the elements in the failed allocation.</param>
    /// <exception cref="OutOfMemoryException">The allocation of <paramref name="count" />x<paramref name="size" /> bytes failed.</exception>
    [DoesNotReturn]
    public static void ThrowOutOfMemoryException(ulong count, ulong size)
    {
        var message = string.Format(Resources.ArrayAllocationFailedMessage, count, size);
        throw new OutOfMemoryException(message);
    }

    /// <summary>Throws an <see cref="OutOfMemoryException" />.</summary>
    /// <param name="count">The count, in elements, of the failed allocation.</param>
    /// <param name="size">The size, in bytes, of the elements in the failed allocation.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    /// <exception cref="OutOfMemoryException">The allocation of <paramref name="count" />x<paramref name="size" /> bytes failed.</exception>
    [DoesNotReturn]
    public static void ThrowOutOfMemoryException(ulong count, ulong size, Exception innerException)
    {
        var message = string.Format(Resources.ArrayAllocationFailedMessage, count, size);
        throw new OutOfMemoryException(message, innerException);
    }

    /// <summary>Throws an <see cref="OutOfMemoryException" />.</summary>
    /// <param name="count">The count, in elements, of the failed allocation.</param>
    /// <param name="size">The size, in bytes, of the elements in the failed allocation.</param>
    /// <exception cref="OutOfMemoryException">The allocation of <paramref name="count" />x<paramref name="size" /> bytes failed.</exception>
    [DoesNotReturn]
    public static void ThrowOutOfMemoryException(nuint count, nuint size)
    {
        var message = string.Format(Resources.ArrayAllocationFailedMessage, count, size);
        throw new OutOfMemoryException(message);
    }

    /// <summary>Throws an <see cref="OutOfMemoryException" />.</summary>
    /// <param name="count">The count, in elements, of the failed allocation.</param>
    /// <param name="size">The size, in bytes, of the elements in the failed allocation.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    /// <exception cref="OutOfMemoryException">The allocation of <paramref name="count" />x<paramref name="size" /> bytes failed.</exception>
    [DoesNotReturn]
    public static void ThrowOutOfMemoryException(nuint count, nuint size, Exception innerException)
    {
        var message = string.Format(Resources.ArrayAllocationFailedMessage, count, size);
        throw new OutOfMemoryException(message, innerException);
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
    /// <param name="methodName">The name of the method that failed to complete within <paramref name="millisecondsTimeout" />.</param>
    /// <param name="millisecondsTimeout">The timeout, in milliseconds, for <paramref name="methodName"/>.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    /// <exception cref="TimeoutException"><paramref name="methodName" /> failed to complete within <paramref name="millisecondsTimeout" /> ms.</exception>
    [DoesNotReturn]
    public static void ThrowTimeoutException(string methodName, int millisecondsTimeout, Exception innerException)
    {
        var message = string.Format(Resources.MethodTimeoutMessage, methodName, millisecondsTimeout);
        throw new TimeoutException(message, innerException);
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

    /// <summary>Throws a <see cref="TimeoutException" />.</summary>
    /// <param name="methodName">The name of the method that failed to complete within <paramref name="timeout" />.</param>
    /// <param name="timeout">The timeout for <paramref name="methodName"/>.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    /// <exception cref="TimeoutException"><paramref name="methodName" /> failed to complete within <paramref name="timeout" /> ms.</exception>
    [DoesNotReturn]
    public static void ThrowTimeoutException(string methodName, TimeSpan timeout, Exception innerException)
    {
        var message = string.Format(Resources.MethodTimeoutMessage, methodName, timeout.TotalMilliseconds);
        throw new TimeoutException(message, innerException);
    }

    /// <summary>Throws an <see cref="UnreachableException" />.</summary>
    /// <param name="message">The message detailing the cause of the exception.</param>
    /// <exception cref="UnreachableException"><paramref name="message" /></exception>
    [DoesNotReturn]
    public static void ThrowUnreachableException(string? message)
    {
        throw new UnreachableException(message);
    }
}
