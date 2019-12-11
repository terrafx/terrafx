// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for asserting conditions.</summary>
    public static class AssertionUtilities
    {
        /// <summary>Asserts that a condition is <c>true</c>.</summary>
        /// <param name="condition">The condition to assert.</param>
        /// <param name="message">The message to print if <paramref name="condition" /> is <c>false</c>.</param>
        [Conditional("DEBUG")]
        public static void Assert([DoesNotReturnIf(false)] bool condition, string message)
        {
            if (!condition)
            {
                Debug.Assert(condition, message);
            }
        }

        /// <summary>Asserts that a condition is <c>true</c>.</summary>
        /// <param name="condition">The condition to assert.</param>
        /// <param name="messageFormat">The message to format if <paramref name="condition" /> is <c>false</c>.</param>
        /// <param name="formatArg">The argument to use when formatting <paramref name="messageFormat" />.</param>
        /// <typeparam name="T">The type of <paramref name="formatArg" />.</typeparam>
        [Conditional("DEBUG")]
        public static void Assert<T>([DoesNotReturnIf(false)] bool condition, string messageFormat, T formatArg)
        {
            if (!condition)
            {
                var message = string.Format(messageFormat, formatArg);
                Debug.Assert(condition, message);
            }
        }

        /// <summary>Asserts that a condition is <c>true</c>.</summary>
        /// <param name="condition">The condition to assert.</param>
        /// <param name="messageFormat">The message to format if <paramref name="condition" /> is <c>false</c>.</param>
        /// <param name="formatArgs">The arguments to use when formatting <paramref name="messageFormat" />.</param>
        [Conditional("DEBUG")]
        public static void Assert([DoesNotReturnIf(false)] bool condition, string messageFormat, params object?[] formatArgs)
        {
            if (!condition)
            {
                var message = string.Format(messageFormat, formatArgs);
                Debug.Assert(condition, message);
            }
        }

        /// <summary>Asserts that a condition is <c>true</c>.</summary>
        /// <param name="condition">The condition to assert.</param>
        /// <param name="messageFormat">The message to format if <paramref name="condition" /> is <c>false</c>.</param>
        /// <param name="formatArg0">The first argument to use when formatting <paramref name="messageFormat" />.</param>
        /// <param name="formatArg1">The second argument to use when formatting <paramref name="messageFormat" />.</param>
        /// <typeparam name="T0">The type of <paramref name="formatArg0" />.</typeparam>
        /// <typeparam name="T1">The type of <paramref name="formatArg1" />.</typeparam>
        [Conditional("DEBUG")]
        public static void Assert<T0, T1>([DoesNotReturnIf(false)] bool condition, string messageFormat, T0 formatArg0, T1 formatArg1)
        {
            if (!condition)
            {
                var message = string.Format(messageFormat, formatArg0, formatArg1);
                Debug.Assert(condition, message);
            }
        }

        /// <summary>Asserts that a condition is <c>true</c>.</summary>
        /// <param name="condition">The condition to assert.</param>
        /// <param name="messageFormat">The message to format if <paramref name="condition" /> is <c>false</c>.</param>
        /// <param name="formatArg0">The first argument to use when formatting <paramref name="messageFormat" />.</param>
        /// <param name="formatArg1">The second argument to use when formatting <paramref name="messageFormat" />.</param>
        /// <param name="formatArg2">The third argument to use when formatting <paramref name="messageFormat" />.</param>
        /// <typeparam name="T0">The type of <paramref name="formatArg0" />.</typeparam>
        /// <typeparam name="T1">The type of <paramref name="formatArg1" />.</typeparam>
        /// <typeparam name="T2">The type of <paramref name="formatArg2" />.</typeparam>
        [Conditional("DEBUG")]
        public static void Assert<T0, T1, T2>([DoesNotReturnIf(false)] bool condition, string messageFormat, T0 formatArg0, T1 formatArg1, T2 formatArg2)
        {
            if (!condition)
            {
                var message = string.Format(messageFormat, formatArg0, formatArg1, formatArg2);
                Debug.Assert(condition, message);
            }
        }

        /// <summary>Asserts that <paramref name="value" /> is not <c>null</c>.</summary>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="value">The value to assert is not <c>null</c>.</param>
        /// <param name="paramName">The name of the parameter being asserted.</param>
        [Conditional("DEBUG")]
        public static void AssertNotNull<T>([NotNull] T? value, string paramName)
            where T : class => Assert(value != null, Resources.ArgumentNullExceptionMessage, paramName);
    }
}
