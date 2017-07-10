// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents a color context that can be used with an ID2D1Bitmap1 object.</summary>
    [Guid("1C4820BB-5771-4518-A581-2FE4DD0EC657")]
    unsafe public /* blittable */ struct ID2D1ColorContext
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Retrieves the color space of the color context.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_COLOR_SPACE GetColorSpace(
            [In] ID2D1ColorContext* This
        );

        /// <summary>Retrieves the size of the color profile, in bytes.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT32 GetProfileSize(
            [In] ID2D1ColorContext* This
        );

        /// <summary>Retrieves the color profile bytes.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetProfile(
            [In] ID2D1ColorContext* This,
            [Out] BYTE* profile,
            [In] UINT32 profileSize
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;

            public GetColorSpace GetColorSpace;

            public GetProfileSize GetProfileSize;

            public GetProfile GetProfile;
            #endregion
        }
        #endregion
    }
}