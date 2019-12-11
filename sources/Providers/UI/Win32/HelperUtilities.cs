// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.	

using System;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI.Providers.Win32
{
    internal static unsafe partial class HelperUtilities
    {
        public static void ThrowExternalExceptionIfFalse(string methodName, int value)
        {
            if (value == FALSE)
            {
                ThrowExternalExceptionForLastError(methodName);
            }
        }

        public static void ThrowExternalExceptionIfZero(string methodName, int value)
        {
            if (value == 0)
            {
                ThrowExternalExceptionForLastError(methodName);
            }
        }

        public static void ThrowExternalExceptionIfZero(string methodName, IntPtr value)
        {
            if (value == IntPtr.Zero)
            {
                ThrowExternalExceptionForLastError(methodName);
            }
        }
    }
}
