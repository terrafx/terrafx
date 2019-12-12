// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for disposing values.</summary>
    public static class DisposeUtilities
    {
        /// <summary>Disposes of a <see cref="ValueLazy{T}" /> if <see cref="ValueLazy{T}.IsCreated" /> returns <c>true</c>.</summary>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="value">The lazy <typeparamref name="T" /> to dispose.</param>
        public static void DisposeIfCreated<T>(ValueLazy<T> value)
            where T : IDisposable
        {
            if (value.IsCreated)
            {
                DisposeIfNotNull(value.Value);
            }
        }

        /// <summary>Disposes of each element in a <see cref="ValueLazy{T}" /> if <see cref="ValueLazy{T}.IsCreated" /> returns <c>true</c>.</summary>
        /// <typeparam name="T">The type of elements in <paramref name="values" />.</typeparam>
        /// <param name="values">The lazy <see cref="IEnumerable{T}" /> containing the values to dispose.</param>
        public static void DisposeIfCreated<T>(ValueLazy<ImmutableArray<T>> values)
            where T : IDisposable
        {
            if (values.IsCreated)
            {
                DisposeIfNotNull(values.Value);
            }
        }

        /// <summary>Disposes of a given <typeparamref name="T" /> if it is not <c>null</c>.</summary>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="value">The <typeparamref name="T" /> to dispose.</param>
        public static void DisposeIfNotNull<T>(T value)
            where T : IDisposable
        {
            if (value != null)
            {
                value.Dispose();
            }
        }

        /// <summary>Disposes of a each element in an <see cref="IEnumerable{T}" /> if that element is not <c>null</c>.</summary>
        /// <typeparam name="T">The type of elements in <paramref name="values" />.</typeparam>
        /// <param name="values">The <see cref="IEnumerable{T}" /> containing the values to dispose.</param>
        public static void DisposeIfNotNull<T>(IEnumerable<T> values)
            where T : IDisposable
        {
            AssertNotNull(values, nameof(values));

            if (values != null)
            {
                foreach (var value in values)
                {
                    DisposeIfNotNull(value);
                }
            }
        }
    }
}
