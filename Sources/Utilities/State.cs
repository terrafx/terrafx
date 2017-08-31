// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static System.Threading.Interlocked;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Utilities
{
    /// <summary>Defines the state for an object.</summary>
    public struct State
    {
        #region Constants
        /// <summary>The object is uninitialized.</summary>
        public const int Uninitialized = 0;

        /// <summary>The object is being disposed.</summary>
        public const int Disposing = (Disposed - 1);

        /// <summary>The object is disposed.</summary>
        public const int Disposed = int.MaxValue;
        #endregion

        #region Fields
        /// <summary>The value of the instance.</summary>
        private volatile int _value;
        #endregion

        #region Operators
        /// <summary>Implicitly converts a <see cref="State" /> to a <see cref="uint" />.</summary>
        /// <param name="state">The <see cref="State" /> to convert.</param>
        public static implicit operator int(State state)
        {
            return state._value;
        }
        #endregion

        #region Methods
        /// <summary>Begins a dispose block.</summary>
        /// <returns>The state prior to entering the dispose block.</returns>
        public int BeginDispose()
        {
            return Transition(to: Disposing);
        }

        /// <summary>Ends a dispose block.</summary>
        public void EndDispose()
        {
            var previousState = Transition(to: Disposed);
            Debug.Assert(previousState == Disposing);
        }

        /// <summary>Throws a <see cref="ObjectDisposedException" /> if the object is being disposed or is already disposed.</summary>
        /// <exception cref="ObjectDisposedException">The object is either being disposed or is already disposed.</exception>
        public void ThrowIfDisposed()
        {
            if (_value >= Disposing)
            {
                ThrowObjectDisposedException(nameof(State));
            }
        }

        /// <summary>Transititions the object to a new state.</summary>
        /// <param name="to">The state to transition to.</param>
        /// <returns>The previous state of the object.</returns>
        public int Transition(int to)
        {
            return Exchange(ref _value, to);
        }

        /// <summary>Transititions the object to a new state.</summary>
        /// <param name="from">The state to transition from.</param>
        /// <param name="to">The state to transition to.</param>
        /// <exception cref="InvalidOperationException">The state of the object is not <paramref name="from" />.</exception>
        public void Transition(int from, int to)
        {
            var state = TryTransition(from, to);

            if (state != from)
            {
                ThrowInvalidOperationException(nameof(_value), _value);
            }
        }

        /// <summary>Attempts to transition the object to a new state.</summary>
        /// <param name="from">The state to transition from.</param>
        /// <param name="to">The state to transition to.</param>
        /// <returns>The state of the object before the attempted transition.</returns>
        public int TryTransition(int from, int to)
        {
            return CompareExchange(ref _value, to, from);
        }
        #endregion
    }
}
