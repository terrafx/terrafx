// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h and X11\X.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    public static unsafe partial class libX11
    {
        private const string DllName = nameof(libX11);

        #region XID Constants
        public const uint None = 0;
        #endregion

        #region Background Pixmap Constants
        public const int ParentRelative = 1;
        #endregion

        #region Border Pixmap Constants
        public const int CopyFromParent = 0;
        #endregion

        #region SendEvent Destination Window Constants
        public const int PointerWindow = 0;

        public const int InputFocus = 1;
        #endregion

        #region Focus Window Constants
        public const int PointerRoot = 1;
        #endregion

        #region GetProperty Constants
        public const int AnyPropertyType = 0;
        #endregion

        #region GrabKey Constants
        public const int AnyKey = 0;
        #endregion

        #region GrabButton Constants
        public const int AnyButton = 0;
        #endregion

        #region KillClient Constants
        public const int AllTemporary = 0;
        #endregion

        #region Time Constants
        public const int CurrentTime = 0;
        #endregion

        #region KeySym Constants
        public const int NoSymbol = 0;
        #endregion

        #region Event Mask Constants
        public const int NoEventMask = 0;

        public const int KeyPressMask = (1 << 0);

        public const int KeyReleaseMask = (1 << 1);

        public const int ButtonPressMask = (1 << 2);

        public const int ButtonReleaseMask = (1 << 3);

        public const int EnterWindowMask = (1 << 4);

        public const int LeaveWindowMask = (1 << 5);

        public const int PointerMotionMask = (1 << 6);

        public const int PointerMotionHintMask = (1 << 7);

        public const int Button1MotionMask = (1 << 8);

        public const int Button2MotionMask = (1 << 9);

        public const int Button3MotionMask = (1 << 10);

        public const int Button4MotionMask = (1 << 11);

        public const int Button5MotionMask = (1 << 12);

        public const int ButtonMotionMask = (1 << 13);

        public const int KeymapStateMask = (1 << 14);

        public const int ExposureMask = (1 << 15);

        public const int VisibilityChangeMask = (1 << 16);

        public const int StructureNotifyMask = (1 << 17);

        public const int ResizeRedirectMask = (1 << 18);

        public const int SubstructureNotifyMask = (1 << 19);

        public const int SubstructureRedirectMask = (1 << 20);

        public const int FocusChangeMask = (1 << 21);

        public const int PropertyChangeMask = (1 << 22);

        public const int ColormapChangeMask = (1 << 23);

        public const int OwnerGrabButtonMask = (1 << 24);
        #endregion

        #region Event Name Constants
        public const int KeyPress = 2;

        public const int KeyRelease = 3;

        public const int ButtonPress = 4;

        public const int ButtonRelease = 5;

        public const int MotionNotify = 6;

        public const int EnterNotify = 7;

        public const int LeaveNotify = 8;

        public const int FocusIn = 9;

        public const int FocusOut = 10;

        public const int KeymapNotify = 11;

        public const int Expose = 12;

        public const int GraphicsExpose = 13;

        public const int NoExpose = 14;

        public const int VisibilityNotify = 15;

        public const int CreateNotify = 16;

        public const int DestroyNotify = 17;

        public const int UnmapNotify = 18;

        public const int MapNotify = 19;

        public const int MapRequest = 20;

        public const int ReparentNotify = 21;

        public const int ConfigureNotify = 22;

        public const int ConfigureRequest = 23;

        public const int GravityNotify = 24;

        public const int ResizeRequest = 25;

        public const int CirculateNotify = 26;

        public const int CirculateRequest = 27;

        public const int PropertyNotify = 28;

        public const int SelectionClear = 29;

        public const int SelectionRequest = 30;

        public const int SelectionNotify = 31;

        public const int ColormapNotify = 32;

        public const int ClientMessage = 33;

        public const int MappingNotify = 34;

        public const int GenericEvent = 35;

        public const int LASTEvent = 36;
        #endregion

        #region Key Mask Constants
        public const int ShiftMask = (1 << 0);

        public const int LockMask = (1 << 1);

        public const int ControlMask = (1 << 2);

        public const int Mod1Mask = (1 << 3);

        public const int Mod2Mask = (1 << 4);

        public const int Mod3Mask = (1 << 5);

        public const int Mod4Mask = (1 << 6);

        public const int Mod5Mask = (1 << 7);
        #endregion

        #region Modifier Name Constants
        public const int ShiftMapIndex = 0;

        public const int LockMapIndex = 1;

        public const int ControlMapIndex = 2;

        public const int Mod1MapIndex = 3;

        public const int Mod2MapIndex = 4;

        public const int Mod3MapIndex = 5;

        public const int Mod4MapIndex = 6;

        public const int Mod5MapIndex = 7;
        #endregion

        #region Button Mask Constants
        public const int Button1Mask = (1 << 8);

        public const int Button2Mask = (1 << 9);

        public const int Button3Mask = (1 << 10);

        public const int Button4Mask = (1 << 11);

        public const int Button5Mask = (1 << 12);

        public const int AnyModifier = (1 << 15);
        #endregion

        #region Button Name Constants
        public const int Button1 = 1;

        public const int Button2 = 2;

        public const int Button3 = 3;

        public const int Button4 = 4;

        public const int Button5 = 5;
        #endregion

        #region Notify Mode Constants
        public const int NotifyNormal = 0;

        public const int NotifyGrab = 1;

        public const int NotifyUngrab = 2;

        public const int NotifyWhileGrabbed = 3;
        #endregion

        #region Notify Constants
        public const int NotifyHint = 1;
        #endregion

        #region Notify Detail Constants
        public const int NotifyAncestor = 0;

        public const int NotifyVirtual = 1;

        public const int NotifyInferior = 2;

        public const int NotifyNonlinear = 3;

        public const int NotifyNonlinearVirtual = 4;

        public const int NotifyPointer = 5;

        public const int NotifyPointerRoot = 6;

        public const int NotifyDetailNone = 7;
        #endregion

        #region Visibility Notify Constsants;

        public const int VisibilityUnobscured = 0;

        public const int VisibilityPartiallyObscured = 1;

        public const int VisibilityFullyObscured = 2;
        #endregion

        #region Circulation Request Constants
        public const int PlaceOnTop = 0;

        public const int PlaceOnBottom = 1;
        #endregion

        #region Protocol Family Constants
        public const int FamilyInternet = 0;

        public const int FamilyDECnet = 1;

        public const int FamilyChaos = 2;

        public const int FamilyServerInterpreted = 5;

        public const int FamilyInternet6 = 6;
        #endregion

        #region Property Notification Constants
        public const int PropertyNewValue = 0;

        public const int PropertyDelete = 1;
        #endregion

        #region Colormap Notification Constants
        public const int ColormapUninstalled = 0;

        public const int ColormapInstalled = 1;
        #endregion

        #region Grab Mode Constants
        public const int GrabModeSync = 0;

        public const int GrabModeAsync = 1;
        #endregion

        #region Grab Reply Status Constantsa;

        public const int GrabSuccess = 0;

        public const int AlreadyGrabbed = 1;

        public const int GrabInvalidTime = 2;

        public const int GrabNotViewable = 3;

        public const int GrabFrozen = 4;
        #endregion

        #region AllowEvents Mode Constants
        public const int AsyncPointer = 0;

        public const int SyncPointer = 1;

        public const int ReplayPointer = 2;

        public const int AsyncKeyboard = 3;

        public const int SyncKeyboard = 4;

        public const int ReplayKeyboard = 5;

        public const int AsyncBoth = 6;

        public const int SyncBoth = 7;
        #endregion

        #region InputFocus Constants
        public const int RevertToNone = (int)None;

        public const int RevertToPointerRoot = (int)PointerRoot;

        public const int RevertToParent = 2;
        #endregion

        #region Error Code Constants
        public const int Success = 0;

        public const int BadRequest = 1;

        public const int BadValue = 2;

        public const int BadWindow = 3;

        public const int BadPixmap = 4;

        public const int BadAtom = 5;

        public const int BadCursor = 6;

        public const int BadFont = 7;

        public const int BadMatch = 8;

        public const int BadDrawable = 9;

        public const int BadAccess = 10;

        public const int BadAlloc = 11;

        public const int BadColor = 12;

        public const int BadGC = 13;

        public const int BadIDChoice = 14;

        public const int BadName = 15;

        public const int BadLength = 16;

        public const int BadImplementation = 17;

        public const int FirstExtensionError = 128;

        public const int LastExtensionError = 255;
        #endregion

        #region Window Class Constants
        public const int InputOutput = 1;

        public const int InputOnly = 2;
        #endregion

        #region CreateWindow and ChangeWindowAttributes Constants
        public const int CWBackPixmap = (1 << 0);

        public const int CWBackPixel = (1 << 1);

        public const int CWBorderPixmap = (1 << 2);

        public const int CWBorderPixel = (1 << 3);

        public const int CWBitGravity = (1 << 4);

        public const int CWWinGravity = (1 << 5);

        public const int CWBackingStore = (1 << 6);

        public const int CWBackingPlanes = (1 << 7);

        public const int CWBackingPixel = (1 << 8);

        public const int CWOverrideRedirect = (1 << 9);

        public const int CWSaveUnder = (1 << 10);

        public const int CWEventMask = (1 << 11);

        public const int CWDontPropagate = (1 << 12);

        public const int CWColormap = (1 << 13);

        public const int CWCursor = (1 << 14);
        #endregion

        #region ConfigureWindow Constants
        public const int CWX = (1<<0);

        public const int CWY = (1<<1);

        public const int CWWidth = (1<<2);

        public const int CWHeight = (1<<3);

        public const int CWBorderWidth = (1<<4);

        public const int CWSibling = (1<<5);

        public const int CWStackMode = (1<<6);
        #endregion

        #region Bit Gravity Constants
        public const int ForgetGravity = 0;

        public const int NorthWestGravity = 1;

        public const int NorthGravity = 2;

        public const int NorthEastGravity = 3;

        public const int WestGravity = 4;

        public const int CenterGravity = 5;

        public const int EastGravity = 6;

        public const int SouthWestGravity = 7;

        public const int SouthGravity = 8;

        public const int SouthEastGravity = 9;

        public const int StaticGravity = 10;
        #endregion

        #region Gravity Constants
        public const int UnmapGravity = 0;
        #endregion

        #region Backing-Store Constants
        public const int NotUseful = 0;

        public const int WhenMapped = 1;

        public const int Always = 2;
        #endregion

        #region Map State Constants
        public const int IsUnmapped = 0;

        public const int IsUnviewable = 1;

        public const int IsViewable = 2;
        #endregion

        #region ChangeSaveSet Constants
        public const int SetModeInsert = 0;

        public const int SetModeDelete = 1;
        #endregion

        #region ChangeCloseDownMode Constants
        public const int DestroyAll = 0;

        public const int RetainPermanent = 1;

        public const int RetainTemporary = 2;
        #endregion

        #region Window Stacking Method Constants
        public const int Above = 0;

        public const int Below = 1;

        public const int TopIf = 2;

        public const int BottomIf = 3;

        public const int Opposite = 4;
        #endregion

        #region Circulation Direction Constants
        public const int RaiseLowest = 0;

        public const int LowerHighest = 1;
        #endregion

        #region Property Mode Constants
        public const int PropModeReplace = 0;

        public const int PropModePrepend = 1;

        public const int PropModeAppend = 2;
        #endregion

        #region Bool Constants
        /// <summary>A <see cref="int" /> value that represents <c>false</c>.</summary>
        public const int False = 0;

        /// <summary>A <see cref="int" /> value that represents <c>true</c>.</summary>
        public const int True = 1;
        #endregion

        #region External Methods
        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XAddConnectionWatch", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("int")]
        public static extern int XAddConnectionWatch(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("XConnectionWatchProc")] IntPtr procedure,
            [In, ComAliasName("XPointer")] sbyte* client_data
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XAllPlanes", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern nuint XAllPlanes(
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XBitmapOrder", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XBitmapBitOrder(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XBitmapPad", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XBitmapPad(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XBitmapUnit", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XBitmapUnit(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XBlackPixel", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern nuint XBlackPixel(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XBlackPixelOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern nuint XBlackPixelOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCellsOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XCellsOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCheckIfEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Bool")]
        public static extern int XCheckIfEvent(
            [In, ComAliasName("Display")] IntPtr display,
            [Out] XEvent* event_return,
            [In, ComAliasName("predicate")] IntPtr predicate,
            [In, ComAliasName("XPointer")] sbyte* arg
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XChangeProperty", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XChangeProperty(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In, ComAliasName("Atom")] nuint property,
            [In, ComAliasName("Atom")] nuint type,
            [In] int format,
            [In] int mode,
            [In] byte* data,
            [In] int nelements
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XChangeWindowAttributes", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XChangeWindowAttributes(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In] nuint valuemask,
            [In] XSetWindowAttributes* attributes
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCirculateSubwindows", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XCirculateSubwindows(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In] int direction
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCirculateSubwindowsDown", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XCirculateSubwindowsDown(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCirculateSubwindowsUp", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XCirculateSubwindowsUp(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCloseDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XCloseDisplay(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XConfigureWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XConfigureWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In] uint value_mask,
            [In] XWindowChanges* values
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XConnectionNumber", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XConnectionNumber(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XConvertSelection", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XConvertSelection(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Atom")] nuint selection,
            [In, ComAliasName("Atom")] nuint target,
            [In, ComAliasName("Atom")] nuint property,
            [In, ComAliasName("Window")] nuint requestor,
            [In, ComAliasName("Time")] nuint time
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCreateWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Window")]
        public static extern nuint XCreateWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint parent,
            [In] int x,
            [In] int y,
            [In] uint width,
            [In] uint height,
            [In] uint border_width,
            [In] int depth,
            [In] uint @class,
            [In] Visual* visual,
            [In] nuint valuemask,
            [In] XSetWindowAttributes* attributes
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XCreateSimpleWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Window")]
        public static extern nuint XCreateSimpleWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint parent,
            [In] int x,
            [In] int y,
            [In] uint width,
            [In] uint height,
            [In] uint border_width,
            [In] nuint border,
            [In] nuint background
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultColormap", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Colormap")]
        public static extern nuint XDefaultColormap(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultColormapOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Colormap")]
        public static extern nuint XDefaultColormapOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultDepth", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDefaultDepth(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultDepthOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDefaultDepthOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultGC", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("GC")]
        public static extern IntPtr XDefaultGC(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultGCOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("GC")]
        public static extern IntPtr XDefaultGCOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultRootWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Window")]
        public static extern nuint XDefaultRootWindow(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDefaultScreen(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultScreenOfDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Screen* XDefaultScreenOfDisplay(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultVisual", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Visual* XDefaultVisual(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefaultVisualOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Visual* XDefaultVisualOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDefineCursor", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDefineCursor(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In, ComAliasName("Cursor")] nuint cursor
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDeleteProperty", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDeleteProperty(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In, ComAliasName("Atom")] nuint property
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDestroySubwindows", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDestroySubwindows(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDestroyWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDestroyWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayCells", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDisplayCells(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayHeight", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDisplayHeight(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayHeightMM", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDisplayHeightMM(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Display")]
        public static extern IntPtr XDisplayOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayPlanes", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDisplayPlanes(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayString", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern sbyte* XDisplayString(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayWidth", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDisplayWidth(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDisplayWidthMM", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDisplayWidthMM(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDoesBackingStore", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XDoesBackingStore(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XDoesSaveUnders", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Bool")]
        public static extern int XDoesSaveUnders(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XEventMaskOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern nint XEventMaskOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XEventsQueued", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XEventsQueued(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int mode
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XExtendedMaxRequestSize", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern nint XExtendedMaxRequestSize(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XFree", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XFree(
            [In] void* data
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XFlush", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XFlush(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XGetAtomName", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern sbyte* XGetAtomName(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Atom")] nuint atom
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XGetAtomNames", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("int")]
        public static extern int XGetAtomNames(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Atom")] nuint* atoms,
            [In] int count,
            [Out] sbyte** names_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XGetGeometry", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("int")]
        public static extern int XGetGeometry(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Drawable")] nuint d,
            [Out, ComAliasName("Window")] nuint* root_return,
            [Out] int* x_return,
            [Out] int* y_return,
            [Out] uint* width_return,
            [Out] uint* height_return,
            [Out] uint* border_width_return,
            [Out] uint* depth_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XGetSelectionOwner", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Window")]
        public static extern nuint XGetSelectionOwner(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Atom")] nuint selection
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XGetWindowAttributes", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("int")]
        public static extern int XGetWindowAttributes(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [Out] out XWindowAttributes window_attributes_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XGetWindowProperty", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XGetWindowProperty(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In, ComAliasName("Atom")] nuint property,
            [In] nint long_offset,
            [In] nint long_length,
            [In, ComAliasName("Bool")] int delete,
            [In, ComAliasName("Atom")] nuint req_type,
            [Out, ComAliasName("Atom")] out nuint actual_type_return,
            [Out] out int actual_format_return,
            [Out] out nuint nitems_return,
            [Out] out nuint bytes_after_return,
            [Out] out byte* prop_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XHeightOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XHeightOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XHeightMMOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XHeightMMOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XIconifyWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Status")]
        public static extern nuint XIconifyWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XIfEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XIfEvent(
            [In, ComAliasName("Display")] IntPtr display,
            [Out] XEvent* event_return,
            [In, ComAliasName("predicate")] IntPtr predicate,
            [In, ComAliasName("XPointer")] sbyte* arg
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XImageByteOrder", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XImageByteOrder(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XInitThreads", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("int")]
        public static extern int XInitThreads(
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XInternalConnectionNumbers", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("int")]
        public static extern int XInternalConnectionNumbers(
            [In, ComAliasName("Display")] IntPtr display,
            [Out] int** fd,
            [Out] int* count_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XInternAtom", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Atom")]
        public static extern nuint XInternAtom(
            [In, ComAliasName("Display")] IntPtr display,
            [In] sbyte* atom_name,
            [In, ComAliasName("Bool")] int only_if_exists
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XInternAtoms", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("int")]
        public static extern int XInternAtoms(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("string[]")] sbyte** names,
            [In] int count,
            [In, ComAliasName("Bool")] int only_if_exists,
            [Out, ComAliasName("Atom")] nuint* atoms_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XLastKnownRequestProcessed", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern nuint XLastKnownRequestProcessed(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XListDepths", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int* XListDepths(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number,
            [Out] int* count_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XLockDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern XPixmapFormatValues* XListPixmapFormats(
            [In, ComAliasName("Display")] IntPtr display,
            [Out] int* count_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XListProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Atom")]
        public static extern nuint* XListProperties(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [Out] int* num_prop_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XLockDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void XLockDisplay(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XLowerWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XLowerWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMapRaised", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XMapRaised(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMapSubwindows", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XMapSubwindows(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMapWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XMapWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMaxCmapsOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XMaxCmapsOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMaxRequestSize", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern nint XMaxRequestSize(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMinCmapsOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XMinCmapsOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMoveWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XMoveWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In] int x,
            [In] int y
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XMoveResizeWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XMoveResizeWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In] int x,
            [In] int y,
            [In] uint width,
            [In] uint height
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XNextEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XNextEvent(
            [In, ComAliasName("Display")] IntPtr display,
            [Out] out XEvent event_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XNextRequest", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern nuint XNextRequest(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XNoOp", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XNoOp(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XOpenDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Display")]
        public static extern IntPtr XOpenDisplay(
            [In] byte* display_name
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XPeekEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void XPeekEvent(
            [In, ComAliasName("Display")] IntPtr display,
            [Out] XEvent* event_Return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XPeekIfEvent", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XPeekIfEvent(
            [In, ComAliasName("Display")] IntPtr display,
            [Out] XEvent* event_return,
            [In, ComAliasName("predicate")] IntPtr predicate,
            [In, ComAliasName("XPointer")] sbyte* arg
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XPending", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XPending(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XPlanesOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XPlanesOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XProcessInternalConnection", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void XProcessInternalConnection(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int fd
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XProtocolRevision", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XProtocolRevision(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XProtocolVersion", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XProtocolVersion(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XQLength", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XQLength(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XQueryPointer", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Bool")]
        public static extern int XQueryPointer(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [Out, ComAliasName("Window")] nuint* root_return,
            [Out, ComAliasName("Window")] nuint* child_return,
            [Out] int* root_x_return,
            [Out] int* root_y_return,
            [Out] int* win_x_return,
            [Out] int* win_y_return,
            [Out] uint* mask_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XQueryTree", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("int")]
        public static extern int XQueryTree(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [Out, ComAliasName("Window")] nuint* root_return,
            [Out, ComAliasName("Window")] nuint* parent_return,
            [Out, ComAliasName("Window")] nuint** children_return,
            [Out] uint* nchildren_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XRaiseWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XRaiseWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XRemoveConnectionWatch", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void XRemoveConnectionWatch(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("XConnectionWatchProc")] IntPtr procedure,
            [In, ComAliasName("XPointer")] sbyte* client_data
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XResizeWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XResizeWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In] uint width,
            [In] uint height
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XRestackWindows", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XRestackWindows(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint* windows,
            [In] int nwindows
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XRootWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Window")]
        public static extern nuint XRootWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XRootWindowOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Window")]
        public static extern nuint XRootWindowOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XRotateWindowProperties", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XRotateWindowProperties(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In, ComAliasName("Atom")] nuint* properties,
            [In] int num_prop,
            [In] int npositions
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XScreenCount", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XScreenCount(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XScreenNumberOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XScreenNumberOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XScreenOfDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Screen* XScreenOfDisplay(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSelectInput", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XSelectInput(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In] nint event_mask
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XServerVendor", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern sbyte* XServerVendor(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetCloseDownMode", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XSetCloseDownMode(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int close_mode
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetSelectionOwner", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XSetSelectionOwner(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Atom")] nuint selection,
            [In, ComAliasName("Window")] nuint owner,
            [In, ComAliasName("Time")] nuint time
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetWindowBackground", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XSetWindowBackground(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In] nuint background_pixel
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetWindowBackgroundPixmap", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XSetWindowBackgroundPixmap(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In, ComAliasName("Pixmap")] nuint background_pixmap
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetWindowBorder", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XSetWindowBorder(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In] nuint border_pixel
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetWindowBorderPixmap", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XSetWindowBorderPixmap(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In, ComAliasName("Pixmap")] nuint border_pixmap
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetWindowBorderWidth", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XSetWindowBorderWidth(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In] uint width
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSetWindowColormap", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XSetWindowColormap(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w,
            [In, ComAliasName("Colormap")] nuint colormap
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XSync", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XSync(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Bool")] int discard
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XTranslateCoordinates", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("Bool")]
        public static extern int XTranslateCoordinates(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint src_w,
            [In, ComAliasName("Window")] nuint dest_w,
            [In] int src_x,
            [In] int src_y,
            [Out] int* dest_x_return,
            [Out] int* dest_y_return,
            [Out, ComAliasName("Window")] nuint* child_return
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XUndefineCursor", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XUndefineCursor(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XUnlockDisplay", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void XUnlockDisplay(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XUnmapSubwindows", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XUnmapSubwindows(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XUnmapWindow", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XUnmapWindow(
            [In, ComAliasName("Display")] IntPtr display,
            [In, ComAliasName("Window")] nuint w
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XVendorRelease", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XVendorRelease(
            [In, ComAliasName("Display")] IntPtr display
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XVisualIDFromVisual", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("VisualID")]
        public static extern nuint XVisualIDFromVisual(
            [In] Visual* visual
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XWhitePixel", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern nuint XWhitePixel(
            [In, ComAliasName("Display")] IntPtr display,
            [In] int screen_number
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XWhitePixelOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern nuint XWhitePixelOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XWidthOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XWidthOfScreen(
            [In] Screen* screen
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "XWidthMMOfScreen", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int XWidthMMOfScreen(
            [In] Screen* screen
        );
        #endregion
    }
}
