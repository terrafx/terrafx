// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("770AAE78-F26F-4DBA-A829-253C83D1B387")]
    unsafe public struct IDXGIFactory1
    {
        #region Constants
        public static readonly Guid IID = typeof(IDXGIFactory1).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT EnumAdapters1(
            [In] IDXGIFactory1* This,
            [In] uint Adapter,
            [Out] IDXGIAdapter1** ppAdapter
        );

        public /* static */ delegate BOOL IsCurrent(
            [In] IDXGIFactory1* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIFactory.Vtbl BaseVtbl;

            public EnumAdapters1 EnumAdapters1;

            public IsCurrent IsCurrent;
            #endregion
        }
        #endregion
    }
}
