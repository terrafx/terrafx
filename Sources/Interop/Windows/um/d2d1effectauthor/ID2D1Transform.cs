// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface implemented by a transform author.</summary>
    [Guid("EF1A287D-342A-4F76-8FDB-DA0D6EA9F92B")]
    unsafe public /* blittable */ struct ID2D1Transform
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MapOutputRectToInputRects(
            [In] ID2D1Transform* This,
            [In, ComAliasName("D2D1_RECT_L")] /* readonly */ RECT* outputRect,
            [Out, ComAliasName("D2D1_RECT_L")] RECT* inputRects,
            [In, ComAliasName("UINT32")] uint inputRectsCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MapInputRectsToOutputRect(
            [In] ID2D1Transform* This,
            [In, ComAliasName("D2D1_RECT_L")] /* readonly */ RECT* inputRects,
            [In, ComAliasName("D2D1_RECT_L")] /* readonly */ RECT* inputOpaqueSubRects,
            [In, ComAliasName("UINT32")] uint inputRectCount,
            [Out, ComAliasName("D2D1_RECT_L")] RECT* outputRect,
            [Out, ComAliasName("D2D1_RECT_L")] RECT* outputOpaqueSubRect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MapInvalidRect(
            [In] ID2D1Transform* This,
            [In, ComAliasName("UINT32")] uint inputIndex,
            [In, ComAliasName("D2D1_RECT_L")] RECT invalidInputRect,
            [Out, ComAliasName("D2D1_RECT_L")] RECT* invalidOutputRect
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1TransformNode.Vtbl BaseVtbl;

            public IntPtr MapOutputRectToInputRects;

            public IntPtr MapInputRectsToOutputRect;

            public IntPtr MapInvalidRect;
            #endregion
        }
        #endregion
    }
}
