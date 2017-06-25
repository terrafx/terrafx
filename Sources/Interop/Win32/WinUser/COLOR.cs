// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\WinUser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum COLOR : uint
    {
        SCROLLBAR = 0,

        BACKGROUND = 1,

        DESKTOP = BACKGROUND,

        ACTIVECAPTION = 2,

        INACTIVECAPTION = 3,

        MENU = 4,

        WINDOW = 5,

        WINDOWFRAME = 6,

        MENUTEXT = 7,

        WINDOWTEXT = 8,

        CAPTIONTEXT = 9,

        ACTIVEBORDER = 10,

        INACTIVEBORDER = 11,

        APPWORKSPACE = 12,

        HIGHLIGHT = 13,

        HIGHLIGHTTEXT = 14,

        BTNFACE = 15,

        _3DFACE = BTNFACE,

        BTNSHADOW = 16,

        _3DSHADOW = BTNSHADOW,

        GRAYTEXT = 17,

        BTNTEXT = 18,

        INACTIVECAPTIONTEXT = 19,

        BTNHIGHLIGHT = 20,

        _3DHIGHLIGHT = BTNHIGHLIGHT,

        _3DHILIGHT = BTNHIGHLIGHT,

        BTNHILIGHT = BTNHIGHLIGHT,

        _3DDKSHADOW = 21,

        _3DLIGHT = 22,

        INFOTEXT = 23,

        INFOBK = 24,

        HOTLIGHT = 26,

        GRADIENTACTIVECAPTION = 27,

        GRADIENTINACTIVECAPTION = 28,

        MENUHILIGHT = 29,

        MENUBAR = 30
    }
}
