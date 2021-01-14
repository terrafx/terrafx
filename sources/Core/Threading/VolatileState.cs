// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Runtime;
using static System.Threading.Interlocked;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Threading
{
    /// <summary>Defines the state for an object.</summary>
    public struct VolatileState
    {
        /// <summary>The object is uninitialized.</summary>
        public const uint Uninitialized = 0;

        /// <summary>The object is initialized to its default state.</summary>
        public const uint Initialized = 1;

        /// <summary>The object is being disposed.</summary>
        public const uint Disposing = Disposed - 1;

        /// <summary>The object is disposed.</summary>
        public const uint Disposed = int.MaxValue;

        private volatile uint _value;

        /// <summary>Gets a value that indicates whether the object is being disposed or is already disposed.</summary>
        public bool IsDisposedOrDisposing => _value >= Disposing;

        /// <summary>Gets a value that indicates whether the object is not being disposed and is not already disposed.</summary>
        public bool IsNotDisposedOrDisposing => _value < Disposing;

        /// <summary>Implicitly converts a <see cref="VolatileState" /> to a <see cref="uint" />.</summary>
        /// <param name="state">The state to convert.</param>
        public static implicit operator uint(VolatileState state) => state._value;

        /// <summary>Begins a dispose block.</summary>
        /// <returns>The state prior to entering the dispose block.</returns>
        public uint BeginDispose() => Transition(to: Disposing);

        /// <summary>Ends a dispose block.</summary>
        public void EndDispose()
        {
            var previousState = Transition(to: Disposed);
            Assert(AssertionsEnabled && (previousState == Disposing));
        }

        /// <summary>Transititions the object to a new state.</summary>
        /// <param name="to">The state to transition to.</param>
        /// <returns>The previous state of the object.</returns>
        public uint Transition(uint to) => Exchange(ref _value, to);

        /// <summary>Transititions the object to a new state.</summary>
        /// <param name="from">The state to transition from.</param>
        /// <param name="to">The state to transition to.</param>
        /// <exception cref="InvalidOperationException">Transitioning the state from '<paramref name="from" />' to '<paramref name="to" />' failed.</exception>
        public void Transition(uint from, uint to)
        {
            var state = TryTransition(from, to);

            if (state != from)
            {
                var message = string.Format(Resources.StateTransitionFailureMessage, from, to);
                ThrowInvalidOperationException(message);
            }
        }

        /// <summary>Attempts to transition the object to a new state.</summary>
        /// <param name="from">The state to transition from.</param>
        /// <param name="to">The state to transition to.</param>
        /// <returns>The state of the object prior to the attempted transition.</returns>
        public uint TryTransition(uint from, uint to) => CompareExchange(ref _value, to, from);
    }
}
