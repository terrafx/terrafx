// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for disposing objects.</summary>
    public static class DisposeUtilities
    {
        #region Static Methods
        /// <summary>Disposes of a <see cref="Lazy{T}" /> instance if <see cref="Lazy{T}.IsValueCreated" /> is <c>true</c>.</summary>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="IDisposable.Dispose" />; otherwise, <c>false</c>.</param>
        /// <param name="value">The <see cref="Lazy{T}" /> instance to dispose.</param>
        public static void DisposeIfValueCreated<T>(bool isDisposing, Lazy<T> value)
            where T : IDisposable
        {
            if (isDisposing && (bool)(value?.IsValueCreated))
            {
                value.Value.Dispose();
            }
        }
        #endregion
    }
}
