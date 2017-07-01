// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public static partial class libX11
    {
        public static UIntPtr AllPlanes()
        {
            if (UIntPtr.Size == sizeof(int))
            {
                return (UIntPtr)(~0);
            }
            else
            {
                Debug.Assert(UIntPtr.Size == sizeof(long));
                return (UIntPtr)(~0L);
            }
        }

        public static int BitmapBitOrder(Display* display)
        {
            return display->bitmap_bit_order;
        }

        public static int BitmapPad(Display* display)
        {
            return display->bitmap_pad;
        }

        public static int BitmapUnit(Display* display)
        {
            return display->bitmap_unit;
        }

        public static UIntPtr BlackPixel(Display* display, int screen_number)
        {
            return ScreenOfDisplay(display, screen_number)->black_pixel;
        }

        public static UIntPtr BlackPixelOfScreen(Screen* screen)
        {
            return screen->black_pixel;
        }

        public static int CellsOfScreen(Screen* screen)
        {
            return DefaultVisualOfScreen(screen)->map_entries;
        }

        public static int ConnectionNumber(Display* display)
        {
            return display->fd;
        }

        public static Colormap DefaultColormap(Display* display, int screen_number)
        {
            return ScreenOfDisplay(display, screen_number)->cmap;
        }

        public static Colormap DefaultColormapOfScreen(Screen* screen)
        {
            return screen->cmap;
        }

        public static int DefaultDepth(Display* display, int screen_number)
        {
            return ScreenOfDisplay(display, screen_number)->root_depth;
        }

        public static int DefaultDepthOfScreen(Screen* screen)
        {
            return screen->root_depth;
        }

        public static GC DefaultGC(Display* display, int screen_number)
        {
            return ScreenOfDisplay(display, screen_number)->default_gc;
        }

        public static GC DefaultGCOfScreen(Screen* screen)
        {
            return screen->default_gc;
        }

        public static Window DefaultRootWindow(Display* display)
        {
            return ScreenOfDisplay(display, DefaultScreen(display))->root;
        }

        public static int DefaultScreen(Display* display)
        {
            return display->default_screen;
        }

        public static Screen* DefaultScreenOfDisplay(Display* display)
        {
            return ScreenOfDisplay(display, DefaultScreen(display));
        }

        public static Visual* DefaultVisual(Display* display, int screen_number)
        {
            return ScreenOfDisplay(display, screen_number)->root_visual;
        }

        public static Visual* DefaultVisualOfScreen(Screen* screen)
        {
            return screen->root_visual;
        }

        public static int DisplayCells(Display* display, int screen_number)
        {
            return DefaultVisual(display, screen_number)->map_entries;
        }

        public static int DisplayHeight(Display* display, int screen_number)
        {
            return ScreenOfDisplay(display, screen_number)->width;
        }

        public static int DisplayHeightMM(Display* display, int screen_number)
        {
            return ScreenOfDisplay(display, screen_number)->mheight;
        }

        public static Display* DisplayOfScreen(Screen* screen)
        {
            return screen->display;
        }

        public static int DisplayPlanes(Display* display, int screen_number)
        {
            return ScreenOfDisplay(display, screen_number)->root_depth;
        }

        public static int DisplayWidth(Display* display, int screen_number)
        {
            return ScreenOfDisplay(display, screen_number)->width;
        }

        public static int DisplayWidthMM(Display* display, int screen_number)
        {
            return ScreenOfDisplay(display, screen_number)->mwidth;
        }

        public static int DoesBackingStore(Screen* screen)
        {
            return screen->backing_store;
        }

        public static Bool DoesSaveUnders(Screen* screen)
        {
            return screen->save_unders;
        }

        public static IntPtr EventMaskOfScreen(Screen* screen)
        {
            return screen->root_input_mask;
        }

        public static int HeightOfScreen(Screen* screen)
        {
            return screen->height;
        }

        public static int HeightMMOfScreen(Screen* screen)
        {
            return screen->mheight;
        }

        public static int ImageByteOrder(Display* display)
        {
            return display->byte_order;
        }

        public static UIntPtr LastKnownRequestProcessed(Display* display)
        {
            return display->last_request_read;
        }

        public static int MaxCmapsOfScreen(Screen* screen)
        {
            return screen->max_maps;
        }

        public static int MinCmapsOfScreen(Screen* screen)
        {
            return screen->min_maps;
        }

        public static UIntPtr NextRequest(Display* display)
        {
            return (display->request + 1);
        }

        public static int PlanesOfScreen(Screen* screen)
        {
            return screen->root_depth;
        }

        public static int ProtocolRevision(Display* display)
        {
            return display->proto_minor_version;
        }

        public static int ProtocolVersion(Display* display)
        {
            return display->proto_major_version;
        }

        public static int QLength(Display* display)
        {
            return display->qlen;
        }

        public static Window RootWindow(Display* display, int screen_number)
        {
            return ScreenOfDisplay(display, screen_number)->root;
        }

        public static Window RootWindowOfScreen(Screen* screen)
        {
            return screen->root;
        }

        public static int ScreenCount(Display* display)
        {
            return display->nscreens;
        }

        public static Screen* ScreenOfDisplay(Display* display, int screen_number)
        {
            return &(display->screens[screen_number]);
        }

        public static byte* ServerVendor(Display* display)
        {
            return display->vendor;
        }

        public static int VendorRelease(Display* display)
        {
            return display->release;
        }

        public static UIntPtr WhitePixel(Display* display, int screen_number)
        {
            return ScreenOfDisplay(display, screen_number)->white_pixel;
        }

        public static UIntPtr WhitePixelOfScreen(Screen* screen)
        {
            return screen->white_pixel;
        }

        public static int WidthOfScreen(Screen* screen)
        {
            return screen->width;
        }

        public static int WidthMMOfScreen(Screen* screen)
        {
            return screen->mwidth;
        }

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XAddConnectionWatch", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Status XAddConnectionWatch(
            [In] Display* display,
            [In] XConnectionWatchProc procedure,
            [In] XPointer client_data
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XAllPlanes", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern UIntPtr XAllPlanes(
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XBitmapOrder", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XBitmapBitOrder(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XBitmapPad", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XBitmapPad(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XBitmapUnit", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XBitmapUnit(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XBlackPixel", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern UIntPtr XBlackPixel(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XBlackPixelOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern UIntPtr XBlackPixelOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCellsOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XCellsOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCheckIfEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Bool XCheckIfEvent(
            [In] Display* display,
            [Out] XEvent* event_return,
            [In] predicate predicate,
            [In] XPointer arg
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XChangeProperty", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XChangeProperty(
            [In] Display* display,
            [In] Window w,
            [In] Atom property,
            [In] Atom type,
            [In] int format,
            [In] int mode,
            [In] byte* data,
            [In] int nelements
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XChangeWindowAttributes", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XChangeWindowAttributes(
            [In] Display* display,
            [In] Window w,
            [In] UIntPtr valuemask,
            [In] XSetWindowAttributes* attributes
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCirculateSubwindows", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XCirculateSubwindows(
            [In] Display* display,
            [In] Window w,
            [In] int direction
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCirculateSubwindowsDown", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XCirculateSubwindowsDown(
            [In] Display* display,
            [In] Window w
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCirculateSubwindowsUp", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XCirculateSubwindowsUp(
            [In] Display* display,
            [In] Window w
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCloseDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XCloseDisplay(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XConfigureWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XConfigureWindow(
            [In] Display* display,
            [In] Window w,
            [In] uint value_mask,
            [In] XWindowChanges* values
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XConnectionNumber", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XConnectionNumber(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XConvertSelection", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XConvertSelection(
            [In] Display* display,
            [In] Atom selection,
            [In] Atom target,
            [In] Atom property,
            [In] Window requestor,
            [In] Time time
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCreateWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Window XCreateWindow(
            [In] Display* display,
            [In] Window parent,
            [In] int x,
            [In] int y,
            [In] uint width,
            [In] uint height,
            [In] uint border_width,
            [In] int depth,
            [In] uint @class,
            [In] Visual* visual,
            [In] UIntPtr valuemask,
            [In] XSetWindowAttributes* attributes
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCreateSimpleWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Window XCreateSimpleWindow(
            [In] Display* display,
            [In] Window parent,
            [In] int x,
            [In] int y,
            [In] uint width,
            [In] uint height,
            [In] uint border_width,
            [In] UIntPtr border,
            [In] UIntPtr background
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultColormap", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Colormap XDefaultColormap(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultColormapOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Colormap XDefaultColormapOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultDepth", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDefaultDepth(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultDepthOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDefaultDepthOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultGC", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern GC XDefaultGC(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultGCOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern GC XDefaultGCOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultRootWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Window XDefaultRootWindow(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDefaultScreen(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultScreenOfDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Screen* XDefaultScreenOfDisplay(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultVisual", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Visual* XDefaultVisual(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultVisualOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Visual* XDefaultVisualOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefineCursor", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDefineCursor(
            [In] Display* display,
            [In] Window w,
            [In] Cursor cursor
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDeleteProperty", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDeleteProperty(
            [In] Display* display,
            [In] Window w,
            [In] Atom property
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDestroySubwindows", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDestroySubwindows(
            [In] Display* display,
            [In] Window w
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDestroyWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDestroyWindow(
            [In] Display* display,
            [In] Window w
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayCells", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDisplayCells(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayHeight", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDisplayHeight(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayHeightMM", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDisplayHeightMM(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Display* XDisplayOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayPlanes", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDisplayPlanes(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayString", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern byte* XDisplayString(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayWidth", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDisplayWidth(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayWidthMM", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDisplayWidthMM(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDoesBackingStore", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XDoesBackingStore(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDoesSaveUnders", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Bool XDoesSaveUnders(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XEventMaskOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern IntPtr XEventMaskOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XEventsQueued", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XEventsQueued(
            [In] Display* display,
            [In] int mode
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XExtendedMaxRequestSize", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern IntPtr XExtendedMaxRequestSize(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XFree", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XFree(
            [In] void* data
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XFlush", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XFlush(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XGetAtomName", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern byte* XGetAtomName(
            [In] Display* display,
            [In] Atom atom
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XGetAtomNames", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Status XGetAtomNames(
            [In] Display* display,
            [In] Atom* atoms,
            [In] int count,
            [Out] byte** names_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XGetGeometry", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Status XGetGeometry(
            [In] Display* display,
            [In] Drawable d,
            [Out] Window* root_return,
            [Out] int* x_return,
            [Out] int* y_return,
            [Out] uint* width_return,
            [Out] uint* height_return,
            [Out] uint* border_width_return,
            [Out] uint* depth_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XGetSelectionOwner", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Window XGetSelectionOwner(
            [In] Display* display,
            [In] Atom selection
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XGetWindowAttributes", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Status XGetWindowAttributes(
            [In] Display* display,
            [In] Window w,
            [Out] XSetWindowAttributes* window_attributes_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XGetWindowProperty", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XGetWindowProperty(
            [In] Display* display,
            [In] Window w,
            [In] Atom property,
            [In] IntPtr long_offset,
            [In] IntPtr long_length,
            [In] Bool delete,
            [In] Atom req_type,
            [Out] Atom* actual_type_return,
            [Out] int* actual_format_return,
            [Out] UIntPtr* nitems_return,
            [Out] UIntPtr* bytes_after_return,
            [Out] byte** prop_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XHeightOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XHeightOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XHeightMMOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XHeightMMOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XIfEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XIfEvent(
            [In] Display* display,
            [Out] XEvent* event_return,
            [In] predicate predicate,
            [In] XPointer arg
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XImageByteOrder", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XImageByteOrder(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XInitThreads", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Status XInitThreads(
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XInternalConnectionNumbers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Status XInternalConnectionNumbers(
            [In] Display display,
            [Out] int** fd,
            [Out] int* count_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XInternAtom", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Atom XInternAtom(
            [In] Display* display,
            [In] byte* atom_name,
            [In] Bool only_if_exists
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XInternAtoms", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Status XInternAtoms(
            [In] Display* display,
            [In] byte** names,
            [In] int count,
            [In] Bool only_if_exists,
            [Out] Atom* atoms_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XLastKnownRequestProcessed", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern UIntPtr XLastKnownRequestProcessed(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XListDepths", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int* XListDepths(
            [In] Display* display,
            [In] int screen_number,
            [Out] int* count_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XLockDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern XPixmapFormatValues* XListPixmapFormats(
            [In] Display* display,
            [Out] int* count_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XListProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Atom* XListProperties(
            [In] Display* display,
            [In] Window w,
            [Out] int* num_prop_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XLockDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void XLockDisplay(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XLowerWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XLowerWindow(
            [In] Display* display,
            [In] Window w
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMapRaised", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XMapRaised(
            [In] Display* display,
            [In] Window w
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMapSubwindows", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XMapSubwindows(
            [In] Display* display,
            [In] Window w
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMapWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XMapWindow(
            [In] Display* display,
            [In] Window w
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMaxCmapsOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XMaxCmapsOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMaxRequestSize", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern IntPtr XMaxRequestSize(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMinCmapsOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XMinCmapsOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMoveWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XMoveWindow(
            [In] Display* display,
            [In] Window w,
            [In] int x,
            [In] int y
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMoveResizeWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XMoveResizeWindow(
            [In] Display* display,
            [In] Window w,
            [In] int x,
            [In] int y,
            [In] uint width,
            [In] uint height
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XNextEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XNextEvent(
            [In] Display* display,
            [Out] out XEvent event_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XNextRequest", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern UIntPtr XNextRequest(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XNoOp", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XNoOp(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XOpenDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Display* XOpenDisplay(
            [In] byte* display_name
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XPeekEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void XPeekEvent(
            [In] Display* display,
            [Out] XEvent* event_Return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XPeekIfEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XPeekIfEvent(
            [In] Display* display,
            [Out] XEvent* event_return,
            [In] predicate predicate,
            [In] XPointer arg
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XPending", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XPending(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XPlanesOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XPlanesOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XProcessInternalConnection", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void XProcessInternalConnection(
            [In] Display* display,
            [In] int fd
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XProtocolRevision", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XProtocolRevision(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XProtocolVersion", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XProtocolVersion(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XQLength", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XQLength(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XQueryPointer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Bool XQueryPointer(
            [In] Display* display,
            [In] Window w,
            [Out] Window* root_return,
            [Out] Window* child_return,
            [Out] int* root_x_return,
            [Out] int* root_y_return,
            [Out] int* win_x_return,
            [Out] int* win_y_return,
            [Out] uint* mask_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XQueryTree", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Status XQueryTree(
            [In] Display* display,
            [In] Window w,
            [Out] Window* root_return,
            [Out] Window* parent_return,
            [Out] Window** children_return,
            [Out] uint* nchildren_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XRaiseWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XRaiseWindow(
            [In] Display* display,
            [In] Window w
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XRemoveConnectionWatch", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void XRemoveConnectionWatch(
            [In] Display* display,
            [In] XConnectionWatchProc procedure,
            [In] XPointer client_data
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XResizeWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XResizeWindow(
            [In] Display* display,
            [In] Window w,
            [In] uint width,
            [In] uint height
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XRestackWindows", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XRestackWindows(
            [In] Display* display,
            [In] Window* windows,
            [In] int nwindows
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XRootWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Window XRootWindow(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XRootWindowOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Window XRootWindowOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XRotateWindowProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XRotateWindowProperties(
            [In] Display* display,
            [In] Window w,
            [In] Atom* properties,
            [In] int num_prop,
            [In] int npositions
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XScreenCount", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XScreenCount(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XScreenNumberOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XScreenNumberOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XScreenOfDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Screen* XScreenOfDisplay(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSelectInput", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XSelectInput(
            [In] Display* display,
            [In] Window w,
            [In] IntPtr event_mask
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XServerVendor", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern byte* XServerVendor(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetCloseDownMode", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XSetCloseDownMode(
            [In] Display* display,
            [In] int close_mode
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetSelectionOwner", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XSetSelectionOwner(
            [In] Display* display,
            [In] Atom selection,
            [In] Window owner,
            [In] Time time
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetWindowBackground", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XSetWindowBackground(
            [In] Display* display,
            [In] Window w,
            [In] UIntPtr background_pixel
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetWindowBackgroundPixmap", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XSetWindowBackgroundPixmap(
            [In] Display* display,
            [In] Window w,
            [In] Pixmap background_pixmap
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetWindowBorder", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XSetWindowBorder(
            [In] Display* display,
            [In] Window w,
            [In] UIntPtr border_pixel
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetWindowBorderPixmap", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XSetWindowBorderPixmap(
            [In] Display* display,
            [In] Window w,
            [In] Pixmap border_pixmap
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetWindowBorderWidth", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XSetWindowBorderWidth(
            [In] Display* display,
            [In] Window w,
            [In] uint width
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetWindowColormap", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XSetWindowColormap(
            [In] Display* display,
            [In] Window w,
            [In] Colormap colormap
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSync", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XSync(
            [In] Display* display,
            [In] Bool discard
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XTranslateCoordinates", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern Bool XTranslateCoordinates(
            [In] Display* display,
            [In] Window src_w,
            [In] Window dest_w,
            [In] int src_x,
            [In] int src_y,
            [Out] int* dest_x_return,
            [Out] int* dest_y_return,
            [Out] Window* child_return
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XUndefineCursor", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XUndefineCursor(
            [In] Display* display,
            [In] Window w
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XUnlockDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void XUnlockDisplay(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XUnmapSubwindows", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XUnmapSubwindows(
            [In] Display* display,
            [In] Window w
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XUnmapWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XUnmapWindow(
            [In] Display* display,
            [In] Window w
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XVendorRelease", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XVendorRelease(
            [In] Display* display
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XVisualIDFromVisual", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern VisualID XVisualIDFromVisual(
            [In] Visual* visual
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XWhitePixel", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern UIntPtr XWhitePixel(
            [In] Display* display,
            [In] int screen_number
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XWhitePixelOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern UIntPtr XWhitePixelOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XWidthOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XWidthOfScreen(
            [In] Screen* screen
        );

        [DllImport("libX11", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XWidthMMOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern int XWidthMMOfScreen(
            [In] Screen* screen
        );
    }
}
