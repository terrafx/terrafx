// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface implemented by a transform author.</summary>
    [Guid("EF1A287D-342A-4F76-8FDB-DA0D6EA9F92B")]
    unsafe public /* blittable */ struct ID2D1Transform
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT MapOutputRectToInputRects(
            [In] ID2D1Transform* This,
            [In] /* readonly */ D2D1_RECT_L* outputRect,
            [Out] D2D1_RECT_L *inputRects,
            [In] UINT32 inputRectsCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT MapInputRectsToOutputRect(
            [In] ID2D1Transform* This,
            [In] /* readonly */ D2D1_RECT_L* inputRects,
            [In] /* readonly */ D2D1_RECT_L* inputOpaqueSubRects,
            [In] UINT32 inputRectCount,
            [Out] D2D1_RECT_L* outputRect,
            [Out] D2D1_RECT_L* outputOpaqueSubRect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT MapInvalidRect(
            [In] ID2D1Transform* This,
            [In] UINT32 inputIndex,
            [In] D2D1_RECT_L invalidInputRect,
            [Out] D2D1_RECT_L* invalidOutputRect
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1TransformNode.Vtbl BaseVtbl;

            public MapOutputRectToInputRects MapOutputRectToInputRects;

            public MapInputRectsToOutputRect MapInputRectsToOutputRect;

            public MapInvalidRect MapInvalidRect;
            #endregion
        }
        #endregion
    }
}
