// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Utilities
{
    /// <summary>Provides support for lazily initializing values.</summary>
    /// <typeparam name="T">The type of the value being lazily initialized.</typeparam>
    public struct ResettableLazy<T>
    {
        private const int Initialized = 1;
        private const int Creating = 2;
        private const int Created = 3;

        private readonly Func<T> _factory;
        private T _value;
        private State _state;

        /// <summary>Initializes a new instance of the <see cref="ResettableLazy{T}" /> struct.</summary>
        /// <param name="factory">The factory method to call when initializing the value.</param>
        public ResettableLazy(Func<T> factory)
        {
            ThrowIfNull(factory, nameof(factory));

            _factory = factory;
            _value = default!;
            _state = new State();

            _ = _state.Transition(to: Initialized);
        }

        /// <summary><c>true</c> if the value has already been created; otherwise, <c>false</c>.</summary>
        public bool IsCreated => _state == Created;

        /// <summary>Gets the value for the instance.</summary>
        public T Value => IsCreated ? _value : CreateValue();

        /// <summary>Resets the instance so the value can be recreated.</summary>
        public void Reset() => _state.Transition(to: Initialized);

        private T CreateValue()
        {
            var spinWait = new SpinWait();

            while (!IsCreated)
            {
                var previousState = _state.TryTransition(from: Initialized, to: Creating);

                if (previousState == Initialized)
                {
                    _value = _factory();
                    _state.Transition(from: Creating, to: Created);
                }
                else
                {
                    spinWait.SpinOnce();
                }
            }
            return _value;
        }
    }
}
