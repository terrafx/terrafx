// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface implemented by a transform author to provide a Compute Shader based effect.</summary>
    [Guid("0D85573C-01E3-4F7D-BFD9-0D60608BF3C3")]
    public /* blittable */ unsafe struct ID2D1ComputeTransform
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetComputeInfo(
            [In] ID2D1ComputeTransform* This,
            [In] ID2D1ComputeInfo* computeInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CalculateThreadgroups(
            [In] ID2D1ComputeTransform* This,
            [In, ComAliasName("D2D1_RECT_L")] /* readonly */ RECT* outputRect,
            [Out, ComAliasName("UINT32")] uint* dimensionX,
            [Out, ComAliasName("UINT32")] uint* dimensionY,
            [Out, ComAliasName("UINT32")] uint* dimensionZ
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Transform.Vtbl BaseVtbl;

            public IntPtr SetComputeInfo;

            public IntPtr CalculateThreadgroups;
            #endregion
        }
        #endregion
    }
}
