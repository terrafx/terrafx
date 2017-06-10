// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    unsafe public struct DXGI_OUTPUT_DESC
    {
        #region Fields
        public _DeviceName_e__FixedBuffer DeviceName;

        public RECT DesktopCoordinates;

        public BOOL AttachedToDesktop;

        public DXGI_MODE_ROTATION Rotation;

        public HMONITOR Monitor;
        #endregion

        #region Structs
        public struct _DeviceName_e__FixedBuffer
        {
            #region Fields
            public WCHAR _0;
            public WCHAR _1;
            public WCHAR _2;
            public WCHAR _3;
            public WCHAR _4;
            public WCHAR _5;
            public WCHAR _6;
            public WCHAR _7;
            public WCHAR _8;
            public WCHAR _9;
            public WCHAR _10;
            public WCHAR _11;
            public WCHAR _12;
            public WCHAR _13;
            public WCHAR _14;
            public WCHAR _15;
            public WCHAR _16;
            public WCHAR _17;
            public WCHAR _18;
            public WCHAR _19;
            public WCHAR _20;
            public WCHAR _21;
            public WCHAR _22;
            public WCHAR _23;
            public WCHAR _24;
            public WCHAR _25;
            public WCHAR _26;
            public WCHAR _27;
            public WCHAR _28;
            public WCHAR _29;
            public WCHAR _30;
            public WCHAR _31;
            #endregion
        }
        #endregion
    }
}
