// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using X11 = TerraFX.Interop.Xlib;

namespace TerraFX.UI.Providers.Xlib
{
    internal enum XlibEventType
    {
        KeyPress = X11.KeyPress,
        KeyRelease = X11.KeyRelease,
        ButtonPress = X11.ButtonPress,
        ButtonRelease = X11.ButtonRelease,
        MotionNotify = X11.MotionNotify,
        EnterNotify = X11.EnterNotify,
        LeaveNotify = X11.LeaveNotify,
        FocusIn = X11.FocusIn,
        FocusOut = X11.FocusOut,
        KeymapNotify = X11.KeymapNotify,
        Expose = X11.Expose,
        GraphicsExpose = X11.GraphicsExpose,
        NoExpose = X11.NoExpose,
        VisibilityNotify = X11.VisibilityNotify,
        CreateNotify = X11.CreateNotify,
        DestroyNotify = X11.DestroyNotify,
        UnmapNotify = X11.UnmapNotify,
        MapNotify = X11.MapNotify,
        MapRequest = X11.MapRequest,
        ReparentNotify = X11.ReparentNotify,
        ConfigureNotify = X11.ConfigureNotify,
        ConfigureRequest = X11.ConfigureRequest,
        GravityNotify = X11.GravityNotify,
        ResizeRequest = X11.ResizeRequest,
        CirculateNotify = X11.CirculateNotify,
        CirculateRequest = X11.CirculateRequest,
        PropertyNotify = X11.PropertyNotify,
        SelectionClear = X11.SelectionClear,
        SelectionRequest = X11.SelectionRequest,
        SelectionNotify = X11.SelectionNotify,
        ColormapNotify = X11.ColormapNotify,
        ClientMessage = X11.ClientMessage,
        MappingNotify = X11.MappingNotify,
        GenericEvent = X11.GenericEvent,
    }
}
