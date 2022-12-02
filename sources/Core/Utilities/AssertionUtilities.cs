// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using TerraFX.Threading;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Utilities;

/// <summary>Provides a set of methods to supplement <see cref="Debug" />.</summary>
public static unsafe class AssertionUtilities
{
    //
    // The public entry points are generic to avoid boxing in the success case
    //

    /// <summary>Asserts that a condition is <c>true</c>.</summary>
    /// <param name="condition">The condition to assert.</param>
    /// <param name="conditionExpression">The expression of the condition that caused the exception.</param>
    /// <exception cref="InvalidOperationException">TerraFX based assertions are disabled.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert([DoesNotReturnIf(false)] bool condition, [CallerArgumentExpression("condition")] string? conditionExpression = null)
    {
        if (AssertionsEnabled && !condition)
        {
            Fail(conditionExpression);
        }
    }

    /// <summary>Asserts that the state is <see cref="VolatileState.Disposing" />.</summary>
    /// <param name="state">The state to assert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertDisposing(VolatileState state)
        => Assert(AssertionsEnabled && (state == VolatileState.Disposing));

    /// <summary>Asserts that <paramref name="value" /> is defined by <typeparamref name="TEnum" />.</summary>
    /// <param name="value">The value to be checked if it is defined by <typeparamref name="TEnum" />.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not defined by <typeparamref name="TEnum" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertIsDefined<TEnum>(TEnum value)
        where TEnum : struct, Enum => Assert(!Enum.IsDefined(value));

    /// <summary>Asserts that the state is not <see cref="VolatileState.Disposed" /> or <see cref="VolatileState.Disposing" />.</summary>
    /// <param name="state">The state to assert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertNotDisposedOrDisposing(VolatileState state)
        => Assert(AssertionsEnabled && state.IsNotDisposedOrDisposing);

    /// <summary>Asserts that <paramref name="value" /> is not <c>null</c>.</summary>
    /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
    /// <param name="value">The value to assert is not <c>null</c>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertNotNull<T>([NotNull] T? value)
        where T : class => Assert(AssertionsEnabled && (value is not null));

    /// <summary>Asserts that <paramref name="array" /> is not <c>null</c>.</summary>
    /// <typeparam name="T">The type of items in <paramref name="array" />.</typeparam>
    /// <param name="array">The array to assert is not <c>null</c>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertNotNull<T>(UnmanagedArray<T> array)
        where T : unmanaged => Assert(AssertionsEnabled && !array.IsNull);

    /// <summary>Asserts that <paramref name="value" /> is not <c>null</c>.</summary>
    /// <param name="value">The value to assert is not <c>null</c>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertNotNull(void* value)
        => Assert(AssertionsEnabled && (value != null));

    /// <summary>Asserts that <see cref="Thread.CurrentThread" /> is <paramref name="expectedThread" />.</summary>
    /// <param name="expectedThread">The thread to assert the code is running on.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertThread(Thread expectedThread)
        => Assert(AssertionsEnabled && (Thread.CurrentThread == expectedThread));

    /// <summary>Throws an <see cref="Exception" />.</summary>
    [DoesNotReturn]
    public static void Fail(string? message = null)
    {
        if (BreakOnFailedAssert)
        {
            Debugger.Break();
        }
        ThrowUnreachableException(message);
    }
}
