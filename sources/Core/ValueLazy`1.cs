// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Lazy<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Threading;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX
{
    /// <summary>Provides support for lazily initializing values.</summary>
    /// <typeparam name="T">The type of the value being lazily initialized.</typeparam>
    [DebuggerDisplay("IsValueCreated={IsValueCreated}, IsValueFaulted={IsValueFaulted}, Value={ValueOrDefault}")]
    [DebuggerTypeProxy(typeof(ValueLazy<>.DebugView))]
    public partial struct ValueLazy<T> : IDisposable
    {
        private const int Creating = 2;
        private const int Faulted = 3;
        private const int Created = 4;

        private Func<T>? _factory;
        private T _value;
        private VolatileState _state;

        /// <summary>Initializes a new instance of the <see cref="ValueLazy{T}" /> struct.</summary>
        /// <param name="factory">The factory method to call when initializing the value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="factory" /> is <c>null</c>.</exception>
        public ValueLazy(Func<T> factory)
        {
            Unsafe.SkipInit(out this);
            Reset(factory);
        }

        /// <summary><c>true</c> if the value has already been created; otherwise, <c>false</c>.</summary>
        public bool IsValueCreated => _state == Created;

        /// <summary><c>true</c> if the creating the value faulted; otherwise, <c>false</c>.</summary>
        public bool IsValueFaulted => _state == Faulted;

        /// <summary>Gets the value for the instance.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public T Value
        {
            get
            {
                AssertNotDisposedOrDisposing(_state);

                if (!IsValueCreated)
                {
                    CreateValue();
                }

                return _value;
            }
        }

        /// <summary>Gets the underlying value if it has been created; otherwise, <c>default</c>.</summary>
        public T? ValueOrDefault => IsValueCreated ? _value : default;

        /// <summary>Gets a reference to the underyling value for the instance.</summary>
        /// <remarks>This property is unsafe as it returns a reference to a struct field.</remarks>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ref T ValueRef
        {
            get
            {
                AssertNotDisposedOrDisposing(_state);

                if (!IsValueCreated)
                {
                    CreateValue();
                }

                return ref MemoryMarshal.GetReference(MemoryMarshal.CreateSpan(ref _value, 1));
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

        /// <inheritdoc />
        public override string ToString() => IsValueCreated ? _value!.ToString()! : string.Empty;

        /// <summary>Resets the instance so the value can be recreated.</summary>
        /// <param name="factory">The factory method to call when initializing the value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="factory" /> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">The lazy value has been disposed.</exception>
        public void Reset(Func<T> factory)
        {
            ThrowIfDisposedOrDisposing(_state, nameof(ValueLazy<T>));
            ThrowIfNull(factory, nameof(factory));

            _factory = factory;
            _ = _state.Transition(to: Initialized);
        }

        private void CreateValue()
        {
            ThrowIfDisposedOrDisposing(_state, nameof(ValueLazy<T>));

            var spinWait = new SpinWait();

            while (!IsValueCreated)
            {
                var previousState = _state.TryTransition(from: Initialized, to: Creating);

                if (previousState == Initialized)
                {
                    AssertNotNull(_factory);

                    try
                    {
                        _value = _factory();
                        _state.Transition(from: Creating, to: Created);
                    }
                    catch
                    {
                        _ = _state.Transition(to: Faulted);
                        throw;
                    }

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
