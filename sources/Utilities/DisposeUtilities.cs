// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Immutable;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for disposing values.</summary>
    public static class DisposeUtilities
    {
        /// <summary>Disposes of a given <typeparamref name="T" /> if it is not <c>null</c>.</summary>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="value">The <typeparamref name="T" /> to dispose.</param>
        public static void DisposeIfNotNull<T>(T value)
            where T : IDisposable?
        {
            if (value != null)
            {
                value.Dispose();
            }
        }

        /// <summary>Disposes of a each element in a <typeparamref name="T" /> array if that element is not <c>null</c>.</summary>
        /// <typeparam name="T">The type of elements in <paramref name="values" />.</typeparam>
        /// <param name="values">The <typeparamref name="T" /> array to dispose.</param>
        public static void DisposeIfNotNull<T>(T[] values)
            where T : IDisposable
        {
            if (values != null)
            {
                foreach (var value in values)
                {
                    DisposeIfNotNull(value);
                }
            }
        }

        /// <summary>Disposes of a each element in an <see cref="ImmutableArray{T}" /> if that element is not <c>null</c>.</summary>
        /// <typeparam name="T">The type of elements in <paramref name="values" />.</typeparam>
        /// <param name="values">The <see cref="ImmutableArray{T}" /> containing the values to dispose.</param>
        public static void DisposeIfNotDefault<T>(ImmutableArray<T> values)
            where T : IDisposable
        {
            if (!values.IsDefault)
            {
                foreach (var value in values)
                {
                    DisposeIfNotNull(value);
                }
            }
        }
    }
}
