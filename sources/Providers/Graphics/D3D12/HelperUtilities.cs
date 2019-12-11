// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.	

using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics.Providers.D3D12
{
    internal static partial class HelperUtilities
    {
        #region Static Methods	
        public static void ThrowExternalExceptionIfFailed(string methodName, int hr)
        {
            if (FAILED(hr))
            {
                ThrowExternalException(methodName, hr);
            }
        }
        #endregion
    }
}
