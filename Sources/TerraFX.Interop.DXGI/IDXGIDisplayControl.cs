// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("EA9DBF1A-C88E-4486-854A-98AA0138F30C")]
    unsafe public struct IDXGIDisplayControl
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate BOOL IsStereoEnabled(
            IDXGIDisplayControl* This
        );

        public /* static */ delegate void SetStereoEnabled(
            IDXGIDisplayControl* This,
            BOOL enabled
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IsStereoEnabled IsStereoEnabled;

            public SetStereoEnabled SetStereoEnabled;
            #endregion
        }
        #endregion
    }
}
