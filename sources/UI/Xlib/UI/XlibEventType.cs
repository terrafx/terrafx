// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.Xlib;

namespace TerraFX.UI;

internal enum XlibEventType
{
    KeyPress = Xlib.KeyPress,
    KeyRelease = Xlib.KeyRelease,
    ButtonPress = Xlib.ButtonPress,
    ButtonRelease = Xlib.ButtonRelease,
    MotionNotify = Xlib.MotionNotify,
    EnterNotify = Xlib.EnterNotify,
    LeaveNotify = Xlib.LeaveNotify,
    FocusIn = Xlib.FocusIn,
    FocusOut = Xlib.FocusOut,
    KeymapNotify = Xlib.KeymapNotify,
    Expose = Xlib.Expose,
    GraphicsExpose = Xlib.GraphicsExpose,
    NoExpose = Xlib.NoExpose,
    VisibilityNotify = Xlib.VisibilityNotify,
    CreateNotify = Xlib.CreateNotify,
    DestroyNotify = Xlib.DestroyNotify,
    UnmapNotify = Xlib.UnmapNotify,
    MapNotify = Xlib.MapNotify,
    MapRequest = Xlib.MapRequest,
    ReparentNotify = Xlib.ReparentNotify,
    ConfigureNotify = Xlib.ConfigureNotify,
    ConfigureRequest = Xlib.ConfigureRequest,
    GravityNotify = Xlib.GravityNotify,
    ResizeRequest = Xlib.ResizeRequest,
    CirculateNotify = Xlib.CirculateNotify,
    CirculateRequest = Xlib.CirculateRequest,
    PropertyNotify = Xlib.PropertyNotify,
    SelectionClear = Xlib.SelectionClear,
    SelectionRequest = Xlib.SelectionRequest,
    SelectionNotify = Xlib.SelectionNotify,
    ColormapNotify = Xlib.ColormapNotify,
    ClientMessage = Xlib.ClientMessage,
    MappingNotify = Xlib.MappingNotify,
    GenericEvent = Xlib.GenericEvent,
}
