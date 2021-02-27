// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using X11 = TerraFX.Interop.Xlib;

namespace TerraFX.UI
{
    internal enum XlibErrorCode
    {
        Success = X11.Success,
        BadRequest = X11.BadRequest,
        BadValue = X11.BadValue,
        BadWindow = X11.BadWindow,
        BadPixmap = X11.BadPixmap,
        BadAtom = X11.BadAtom,
        BadCursor = X11.BadCursor,
        BadFont = X11.BadFont,
        BadMatch = X11.BadMatch,
        BadDrawable = X11.BadDrawable,
        BadAccess = X11.BadAccess,
        BadAlloc = X11.BadAlloc,
        BadColor = X11.BadColor,
        BadGC = X11.BadGC,
        BadIDChoice = X11.BadIDChoice,
        BadName = X11.BadName,
        BadLength = X11.BadLength,
        BadImplementation = X11.BadImplementation,
    }
}
