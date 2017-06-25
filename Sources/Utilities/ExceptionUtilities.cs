// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

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

        /// <summary>Creates a new instance of the <see cref="InvalidOperationException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="value">The value of the parameter that caused the exception.</param>
        /// <returns>A new instance of the <see cref="InvalidOperationException" /> class.</returns>
        public static InvalidOperationException NewInvalidOperationException(string paramName, object value)
        {
            var message = string.Format(Resources.InvalidOperationExceptionMessage, paramName, value);
            return new InvalidOperationException(message);
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

        /// <summary>Throws a new instance of the <see cref="InvalidOperationException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="value">The value of the parameter that caused the exception.</param>
        public static void ThrowInvalidOperationException(string paramName, object value)
        {
            throw NewInvalidOperationException(paramName, value);
        }
        #endregion
    }
}
