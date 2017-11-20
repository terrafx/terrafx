// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.Contracts;
using static System.Runtime.InteropServices.Marshal;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Utilities
{
    /// <summary>Defines a delegate that can be passed to native code.</summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    public struct NativeDelegate<TDelegate> where TDelegate : class
    {
        #region Fields
        /// <summary>The native handle to <see cref="_value" />.</summary>
        internal readonly IntPtr _handle;

        /// <summary>The managed delegate that was marshalled.</summary>
        internal readonly TDelegate _value;
        #endregion

        #region Static Constructors
        /// <summary>Initializes the static members of the <see cref="NativeDelegate{TDelegate}" /> struct.</summary>
        static NativeDelegate()
        {
            // Validate `TDelegate is Delegate` in the static constructor so that
            // operators and other static methods go through the required checks.

            if ((typeof(Delegate).IsAssignableFrom(typeof(TDelegate))) == false)
            {
                ThrowArgumentExceptionForInvalidType(nameof(TDelegate), typeof(TDelegate));
            }
        }
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="NativeDelegate{TDelegate}" /> struct.</summary>
        /// <param name="value">The managed delegate represented by the instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> does not target a static method.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> targets more than one method.</exception>
        public NativeDelegate(TDelegate value)
        {
            if (value is null)
            {
                ThrowArgumentNullException(nameof(value));
            }

            // The validation that this a valid cast is done in the static constructor.
            var @delegate = (Delegate)((object)(value));

            if (@delegate.Target != null)
            {
                ThrowArgumentOutOfRangeException(nameof(value), value);
            }

            if (@delegate.GetInvocationList().Length != 1)
            {
                ThrowArgumentOutOfRangeException(nameof(value), value);
            }
            Contract.EndContractBlock();

            _value = value;
            _handle = GetFunctionPointerForDelegate(value);
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="NativeDelegate{TDelegate}" /> to a <see cref="IntPtr" />.</summary>
        /// <param name="value">The <see cref="NativeDelegate{TDelegate}" /> to convert.</param>
        public static implicit operator IntPtr(NativeDelegate<TDelegate> value)
        {
            return value._handle;
        }

        /// <summary>Implicitly converts a <see cref="NativeDelegate{TDelegate}" /> to a <typeparamref name="TDelegate" />.</summary>
        /// <param name="value">The <see cref="NativeDelegate{TDelegate}" /> to convert.</param>
        public static implicit operator TDelegate(NativeDelegate<TDelegate> value)
        {
            return value._value;
        }
        #endregion
    }
}
