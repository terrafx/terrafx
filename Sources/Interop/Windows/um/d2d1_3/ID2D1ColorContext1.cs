// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents a color context to be used with the Color Management Effect.</summary>
    [Guid("1AB42875-C57F-4BE9-BD85-9CD78D6F55EE")]
    unsafe public /* blittable */ struct ID2D1ColorContext1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Retrieves the color context type.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_COLOR_CONTEXT_TYPE GetColorContextType(
            [In] ID2D1ColorContext1* This
        );

        /// <summary>Retrieves the DXGI color space of this context. Returns DXGI_COLOR_SPACE_CUSTOM when color context type is ICC.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DXGI_COLOR_SPACE_TYPE GetDXGIColorSpace(
            [In] ID2D1ColorContext1* This
        );

        /// <summary>Retrieves a set simple color profile.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSimpleColorProfile(
            [In] ID2D1ColorContext1* This,
            [Out] D2D1_SIMPLE_COLOR_PROFILE* simpleProfile
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1ColorContext.Vtbl BaseVtbl;

            public IntPtr GetColorContextType;

            public IntPtr GetDXGIColorSpace;

            public IntPtr GetSimpleColorProfile;
            #endregion
        }
        #endregion
    }
}
