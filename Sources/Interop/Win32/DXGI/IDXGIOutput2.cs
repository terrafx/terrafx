// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("595E39D1-2724-4663-99B1-DA969DE28364")]
    unsafe public struct IDXGIOutput2
    {
        #region Constants
        public static readonly Guid IID = typeof(IDXGIOutput2).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate BOOL SupportsOverlays(
            [In] IDXGIOutput2* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIOutput1.Vtbl BaseVtbl;

            public SupportsOverlays SupportsOverlays;
            #endregion
        }
        #endregion
    }
}
