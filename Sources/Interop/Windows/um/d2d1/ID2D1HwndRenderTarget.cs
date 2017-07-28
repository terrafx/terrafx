// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Renders drawing instructions to a window.</summary>
    [Guid("2CD90698-12E2-11DC-9FED-001143A055F9")]
    unsafe public /* blittable */ struct ID2D1HwndRenderTarget
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_WINDOW_STATE CheckWindowState(
            [In] ID2D1HwndRenderTarget* This
        );

        /// <summary>Resize the buffer underlying the render target. This operation might fail if there is insufficient video memory or system memory, or if the render target is resized beyond the maximum bitmap size. If the method fails, the render target will be placed in a zombie state and D2DERR_RECREATE_TARGET will be returned from it when EndDraw is called. In addition an appropriate failure result will be returned from Resize.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Resize(
            [In] ID2D1HwndRenderTarget* This,
            [In, ComAliasName("D2D1_SIZE_U")] /* readonly */ D2D_SIZE_U* pixelSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HWND")]
        public /* static */ delegate void* GetHwnd(
            [In] ID2D1HwndRenderTarget* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1RenderTarget.Vtbl BaseVtbl;

            public CheckWindowState CheckWindowState;

            public Resize Resize;

            public GetHwnd GetHwnd;
            #endregion
        }
        #endregion
    }
}
