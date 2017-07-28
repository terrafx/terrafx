// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Interface encapsulating a GDI/GDI+ metafile.</summary>
    [Guid("2F543DC3-CFC1-4211-864F-CFD91C6F3395")]
    unsafe public /* blittable */ struct ID2D1GdiMetafile
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Play the metafile into a caller-supplied sink interface.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Stream(
            [In] ID2D1GdiMetafile* This,
            [In] ID2D1GdiMetafileSink* sink
        );

        /// <summary>Gets the bounds of the metafile.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetBounds(
            [In] ID2D1GdiMetafile* This,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* bounds
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;

            public Stream Stream;

            public GetBounds GetBounds;
            #endregion
        }
        #endregion
    }
}
