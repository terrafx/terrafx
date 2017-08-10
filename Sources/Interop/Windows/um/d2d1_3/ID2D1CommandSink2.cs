// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>This interface performs all the same functions as the existing ID2D1CommandSink1 interface. It also enables access to ink rendering and gradient mesh rendering.</summary>
    [Guid("3BAB440E-417E-47DF-A2E2-BC0BE6A00916")]
    unsafe public /* blittable */ struct ID2D1CommandSink2
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawInk(
            [In] ID2D1CommandSink2* This,
            [In] ID2D1Ink* ink,
            [In] ID2D1Brush* brush,
            [In, Optional] ID2D1InkStyle* inkStyle
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawGradientMesh(
            [In] ID2D1CommandSink2* This,
            [In] ID2D1GradientMesh* gradientMesh
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawGdiMetafile(
            [In] ID2D1CommandSink2* This,
            [In] ID2D1GdiMetafile* gdiMetafile,
            [In, Optional, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* destinationRectangle,
            [In, Optional, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* sourceRectangle
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1CommandSink1.Vtbl BaseVtbl;

            public IntPtr DrawInk;

            public IntPtr DrawGradientMesh;

            public IntPtr DrawGdiMetafile;
            #endregion
        }
        #endregion
    }
}
