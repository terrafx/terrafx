// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for throwing exceptions.</summary>
    public static class ExceptionUtilities
    {
        #region Static Methods
        /// <summary>Throws a new instance of the <see cref="ArgumentException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="paramType">The type of the parameter that caused the exception.</param>
        public static void ThrowArgumentExceptionForInvalidType(string paramName, Type paramType)
        {
            var message = string.Format(Resources.ArgumentExceptionForInvalidTypeMessage, paramName, paramType);
            throw new ArgumentException(message, paramName);
        }

        /// <summary>Throws a new instance of the <see cref="ArgumentNullException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        public static void ThrowArgumentNullException(string paramName)
        {
            var message = string.Format(Resources.ArgumentNullExceptionMessage, paramName);
            throw new ArgumentNullException(paramName, message);
        }

        /// <summary>Throws a new instance of the <see cref="ArgumentOutOfRangeException" /> class.</summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="value">The value of the parameter that caused the exception.</param>
        public static void ThrowArgumentOutOfRangeException(string paramName, object value)
        {
            var message = string.Format(Resources.ArgumentOutOfRangeExceptionMessage, paramName, value);
            throw new ArgumentOutOfRangeException(paramName, value, message);
        }
        #endregion
    }
}
