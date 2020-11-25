// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.	

using System.Runtime.CompilerServices;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI.Providers.Win32
{
    internal static unsafe partial class HelperUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowExternalExceptionIfFalse(int value, string methodName)
        {
            if (value == FALSE)
            {
                ThrowExternalExceptionForLastError(methodName);
            }
        }
    }
}
