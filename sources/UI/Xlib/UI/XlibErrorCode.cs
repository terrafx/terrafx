// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.Xlib;

namespace TerraFX.UI
{
    internal enum XlibErrorCode
    {
        Success = Xlib.Success,
        BadRequest = Xlib.BadRequest,
        BadValue = Xlib.BadValue,
        BadWindow = Xlib.BadWindow,
        BadPixmap = Xlib.BadPixmap,
        BadAtom = Xlib.BadAtom,
        BadCursor = Xlib.BadCursor,
        BadFont = Xlib.BadFont,
        BadMatch = Xlib.BadMatch,
        BadDrawable = Xlib.BadDrawable,
        BadAccess = Xlib.BadAccess,
        BadAlloc = Xlib.BadAlloc,
        BadColor = Xlib.BadColor,
        BadGC = Xlib.BadGC,
        BadIDChoice = Xlib.BadIDChoice,
        BadName = Xlib.BadName,
        BadLength = Xlib.BadLength,
        BadImplementation = Xlib.BadImplementation,
    }
}
