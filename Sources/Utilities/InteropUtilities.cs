// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Utilities
{
    /// <summary>Provides a set of methods for simplifying interop code.</summary>
    public static class InteropUtilities
    {
        #region Static Methods
        /// <summary>Gets the size of <typeparamref name="T" />.</summary>
        /// <typeparam name="T">The type for which to get the size.</typeparam>
        /// <returns>The size of <typeparamref name="T" />.</returns>
        public static uint SizeOf<T>()
        {
            return unchecked((uint)(Marshal.SizeOf<T>()));
        }
        #endregion
    }
}
