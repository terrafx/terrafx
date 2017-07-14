// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Utilities
{
    /// <summary>Defines an ANSI string that can be passed to native code.</summary>
    public struct NativeStringAnsi : IDisposable
    {
        #region Fields
        internal IntPtr _handle;

        internal readonly string _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="NativeStringAnsi" /> struct.</summary>
        /// <param name="value">The managed string represented by the instance.</param>
        public NativeStringAnsi(string value)
        {
            _handle = (value is null) ? IntPtr.Zero : Marshal.StringToHGlobalAnsi(value);
            _value = value;
        }

        /// <summary>Initializes a new instance of the <see cref="NativeStringAnsi" /> struct.</summary>
        /// <param name="handle">The native handle represented by the instance.</param>
        public NativeStringAnsi(IntPtr handle)
        {
            _handle = handle;
            _value = (handle == IntPtr.Zero) ? null : Marshal.PtrToStringAnsi(handle);
        }

        /// <summary>Initializes a new instance of the <see cref="NativeStringAnsi" /> struct.</summary>
        /// <param name="handle">The native handle represented by the instance.</param>
        /// <param name="length">The length of the string represented by <paramref name="handle" />.</param>
        public NativeStringAnsi(IntPtr handle, int length)
        {
            _handle = handle;
            _value = (handle == IntPtr.Zero) ? null : Marshal.PtrToStringAnsi(handle, length);
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="NativeStringAnsi" /> to a <see cref="IntPtr" />.</summary>
        /// <param name="value">The <see cref="NativeStringAnsi" /> to convert.</param>
        public static implicit operator IntPtr(NativeStringAnsi value)
        {
            return value._handle;
        }

        /// <summary>Implicitly converts a <see cref="NativeStringAnsi" /> to a <see cref="string" />.</summary>
        /// <param name="value">The <see cref="NativeStringAnsi" /> to convert.</param>
        public static implicit operator string(NativeStringAnsi value)
        {
            return value._value;
        }
        #endregion

        #region System.IDisposable Methods
        /// <summary>Disposes any unmanaged resources assocaited with the instance.</summary>
        public void Dispose()
        {
            var handle = _handle;

            if (handle != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(handle);
                _handle = IntPtr.Zero;
            }
        }
        #endregion
    }
}
