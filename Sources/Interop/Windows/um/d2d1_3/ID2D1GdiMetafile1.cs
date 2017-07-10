// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Interface encapsulating a GDI/GDI+ metafile.</summary>
    [Guid("2E69F9E8-DD3F-4BF9-95BA-C04F49D788DF")]
    unsafe public /* blittable */ struct ID2D1GdiMetafile1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Returns the DPI reported by the metafile.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDpi(
            [In] ID2D1GdiMetafile1* This,
            [Out] FLOAT* dpiX,
            [Out] FLOAT* dpiY
        );

        /// <summary>Gets the bounds (in DIPs) of the metafile (as specified by the frame rect declared in the metafile).</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSourceBounds(
            [In] ID2D1GdiMetafile1* This,
            [Out] D2D1_RECT_F* bounds
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1GdiMetafile.Vtbl BaseVtbl;

            public GetDpi GetDpi;

            public GetSourceBounds GetSourceBounds;
            #endregion
        }
        #endregion
    }
}
