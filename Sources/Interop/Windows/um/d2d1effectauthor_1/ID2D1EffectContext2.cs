// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The internal context handed to effect authors to create transforms from effects and any other operation tied to context which is not useful to the application facing API.</summary>
    [Guid("577AD2A0-9FC7-4DDA-8B18-DAB810140052")]
    unsafe public /* blittable */ struct ID2D1EffectContext2
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Creates a color context from a DXGI color space type. It is only valid to use this with the Color Management Effect in 'Best' mode.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateColorContextFromDxgiColorSpace(
            [In] ID2D1EffectContext2* This,
            [In] DXGI_COLOR_SPACE_TYPE colorSpace,
            [Out] ID2D1ColorContext1** colorContext
        );

        /// <summary>Creates a color context from a simple color profile. It is only valid to use this with the Color Management Effect in 'Best' mode.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateColorContextFromSimpleColorProfile(
            [In] ID2D1EffectContext2* This,
            [In] /* readonly */ D2D1_SIMPLE_COLOR_PROFILE* simpleProfile,
            [Out] ID2D1ColorContext1** colorContext
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1EffectContext1.Vtbl BaseVtbl;

            public CreateColorContextFromDxgiColorSpace CreateColorContextFromDxgiColorSpace;

            public CreateColorContextFromSimpleColorProfile CreateColorContextFromSimpleColorProfile;
            #endregion
        }
        #endregion
    }
}
