// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.InteropUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Utilities
{
    /// <summary>Provides support for lazily initializing values.</summary>
    /// <typeparam name="T">The type of the value being lazily initialized.</typeparam>
    public struct ValueLazy<T> : IDisposable
    {
        private const int Creating = 2;
        private const int Created = 3;

        private Func<T>? _factory;
        private T _value;
        private State _state;

        /// <summary>Initializes a new instance of the <see cref="ValueLazy{T}" /> struct.</summary>
        /// <param name="factory">The factory method to call when initializing the value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="factory" /> is <c>null</c>.</exception>
        public ValueLazy(Func<T> factory)
        {
            SkipInit(out this);
            Reset(factory);
        }

        /// <summary><c>true</c> if the value has already been created; otherwise, <c>false</c>.</summary>
        public bool IsCreated => _state == Created;

        /// <summary>Gets a reference to the underyling value for the instance.</summary>
        /// <remarks>This property is unsafe as it returns a reference to a struct field.</remarks>
        /// <exception cref="ObjectDisposedException">The lazy value has been disposed.</exception>
        public ref T RefValue
        {
            get
            {
                _state.AssertNotDisposedOrDisposing();

                if (!IsCreated)
                {
                    CreateValue();
                }

                return ref MemoryMarshal.GetReference(MemoryMarshal.CreateSpan(ref _value, 1));
            }
        }

        /// <summary>Gets the value for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The lazy value has been disposed.</exception>
        public T Value
        {
            get
            {
                _state.AssertNotDisposedOrDisposing();

                if (!IsCreated)
                {
                    CreateValue();
                }

                return _value;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _ = _state.BeginDispose();
            _state.EndDispose();
        }

        /// <inheritdoc cref="Dispose()" />
        /// <param name="action">The action to call, if the value was created, which performs the appropriate disposal.</param>
        public void Dispose(Action<T> action)
        {
            var priorState = _state.BeginDispose();

            if (priorState == Created)
            {
                action(_value);
            }

            _state.EndDispose();
        }

        /// <summary>Resets the instance so the value can be recreated.</summary>
        /// <param name="factory">The factory method to call when initializing the value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="factory" /> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">The lazy value has been disposed.</exception>
        public void Reset(Func<T> factory)
        {
            _state.ThrowIfDisposedOrDisposing();
            ThrowIfNull(factory, nameof(factory));

            _factory = factory;
            _ = _state.Transition(to: Initialized);
        }

        private void CreateValue()
        {
            _state.ThrowIfDisposedOrDisposing();

            var spinWait = new SpinWait();

            while (!IsCreated)
            {
                var previousState = _state.TryTransition(from: Initialized, to: Creating);

                if (previousState == Initialized)
                {
                    AssertNotNull(_factory, nameof(_factory));

                    _value = _factory();
                    _state.Transition(from: Creating, to: Created);

                    _factory = null;
                }
                else
                {
                    spinWait.SpinOnce();
                }
            }
        }
    }
}
