// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of extension methods for <see cref="Lazy{T}" />.</summary>
    public static class LazyExtensions
    {
        /// <summary>Disposes of a <see cref="Lazy{T}" /> instance if <see cref="Lazy{T}.IsValueCreated" /> is <c>true</c>.</summary>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="value">The <see cref="Lazy{T}" /> instance to dispose.</param>
        public static void Dispose<T>(this Lazy<T> value)
            where T : IDisposable
        {
            if (value.IsValueCreated)
            {
                value.Value.Dispose();
            }
        }
    }
}
