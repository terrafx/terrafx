// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for throwing exceptions.</summary>
    public static class ExceptionUtilities
    {
        #region Static Methods
        /// <summary>Creates a new instance of the <see cref="ArgumentException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="paramType">The type of the parameter that caused the exception.</param>
        /// <returns>A new instance of the <see cref="ArgumentException" /> class.</returns>
        public static ArgumentException NewArgumentExceptionForInvalidType(string paramName, Type paramType)
        {
            var message = string.Format(Resources.ArgumentExceptionForInvalidTypeMessage, paramName, paramType);
            return new ArgumentException(message, paramName);
        }

        /// <summary>Creates a new instance of the <see cref="ArgumentNullException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <returns>A new instance of the <see cref="ArgumentNullException" /> class.</returns>
        public static ArgumentNullException NewArgumentNullException(string paramName)
        {
            var message = string.Format(Resources.ArgumentNullExceptionMessage, paramName);
            return new ArgumentNullException(paramName, message);
        }

        /// <summary>Creates a new instance of the <see cref="ArgumentOutOfRangeException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="value">The value of the parameter that caused the exception.</param>
        /// <returns>A new instance of the <see cref="ArgumentOutOfRangeException" /> class.</returns>
        public static ArgumentOutOfRangeException NewArgumentOutOfRangeException(string paramName, object value)
        {
            var message = string.Format(Resources.ArgumentOutOfRangeExceptionMessage, paramName, value);
            return new ArgumentOutOfRangeException(paramName, value, message);
        }

        /// <summary>Creates a new instance of the <see cref="ExternalException" /> class.</summary>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <param name="errorCode">The error code that caused the exception.</param>
        /// <returns>A new instance of the <see cref="ExternalException" /> class.</returns>
        public static ExternalException NewExternalException(string methodName, int errorCode)
        {
            var message = string.Format(Resources.ExternalExceptionMessage, methodName, errorCode);
            return new ExternalException(message, errorCode);
        }

        /// <summary>Creates a new instance of the <see cref="ExternalException" /> class.</summary>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <returns>A new instance of the <see cref="ExternalException" /> class.</returns>
        public static ExternalException NewExternalExceptionForLastError(string methodName)
        {
            var errorCode = Marshal.GetLastWin32Error();
            return NewExternalException(methodName, errorCode);
        }

        /// <summary>Creates a new instance of the <see cref="ExternalException" /> class.</summary>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <returns>A new instance of the <see cref="ExternalException" /> class.</returns>
        public static ExternalException NewExternalExceptionForLastHRESULT(string methodName)
        {
            var hresult = Marshal.GetHRForLastWin32Error();
            return NewExternalException(methodName, hresult);
        }

        /// <summary>Creates a new instance of the <see cref="InvalidOperationException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="value">The value of the parameter that caused the exception.</param>
        /// <returns>A new instance of the <see cref="InvalidOperationException" /> class.</returns>
        public static InvalidOperationException NewInvalidOperationException(string paramName, object value)
        {
            var message = string.Format(Resources.InvalidOperationExceptionMessage, paramName, value);
            return new InvalidOperationException(message);
        }

        /// <summary>Creates a new instance of the <see cref="ObjectDisposedException" /> class.</summary>
        /// <param name="objectName">The name of the object that caused the exception.</param>
        /// <returns>A new instance of the <see cref="ObjectDisposedException" /> class.</returns>
        public static ObjectDisposedException NewObjectDisposedException(string objectName)
        {
            var message = string.Format(Resources.ObjectDisposedExceptionMessage, objectName);
            return new ObjectDisposedException(objectName, message);
        }

        /// <summary>Throws a new instance of the <see cref="ArgumentException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="paramType">The type of the parameter that caused the exception.</param>
        public static void ThrowArgumentExceptionForInvalidType(string paramName, Type paramType)
        {
            throw NewArgumentExceptionForInvalidType(paramName, paramType);
        }

        /// <summary>Throws a new instance of the <see cref="ArgumentNullException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        public static void ThrowArgumentNullException(string paramName)
        {
            throw NewArgumentNullException(paramName);
        }

        /// <summary>Throws a new instance of the <see cref="ArgumentOutOfRangeException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="value">The value of the parameter that caused the exception.</param>
        public static void ThrowArgumentOutOfRangeException(string paramName, object value)
        {
            throw NewArgumentOutOfRangeException(paramName, value);
        }

        /// <summary>Throws a new instance of the <see cref="ExternalException" /> class.</summary>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        /// <param name="errorCode">The error code that caused the exception.</param>
        public static void ThrowExternalException(string methodName, int errorCode)
        {
            throw NewExternalException(methodName, errorCode);
        }

        /// <summary>Throws a new instance of the <see cref="ExternalException" /> class.</summary>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        public static void ThrowExternalExceptionForLastError(string methodName)
        {
            throw NewExternalExceptionForLastError(methodName);
        }

        /// <summary>Throws a new instance of the <see cref="ExternalException" /> class.</summary>
        /// <param name="methodName">The name of the method that caused the exception.</param>
        public static void ThrowExternalExceptionForLastHRESULT(string methodName)
        {
            throw NewExternalExceptionForLastHRESULT(methodName);
        }

        /// <summary>Throws a new instance of the <see cref="InvalidOperationException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="value">The value of the parameter that caused the exception.</param>
        public static void ThrowInvalidOperationException(string paramName, object value)
        {
            throw NewInvalidOperationException(paramName, value);
        }

        /// <summary>Throws a new instance of the <see cref="ObjectDisposedException" /> class.</summary>
        /// <param name="objectName">The name of the object that caused the exception.</param>
        public static void ThrowObjectDisposedException(string objectName)
        {
            throw NewObjectDisposedException(objectName);
        }
        #endregion
    }
}
