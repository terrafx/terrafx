// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Interop.Windows;

namespace TerraFX.Interop
{
    /// <summary>The effect interface. Properties control how the effect is rendered. The effect is Drawn with the DrawImage call.</summary>
    [Guid("28211A43-7D89-476F-8181-2D6159B220AD")]
    unsafe public /* blittable */ struct ID2D1Effect
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Sets the input to the given effect. The input can be a concrete bitmap or the output of another effect.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetInput(
            [In] ID2D1Effect* This,
            [In, ComAliasName("UINT32")] uint index,
            [In, Optional] ID2D1Image* input,
            [In, DefaultParameterValue(TRUE), ComAliasName("BOOL")] int invalidate
        );

        /// <summary>If the effect supports a variable number of inputs, this sets the number of input that are currently active on the effect.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetInputCount(
            [In] ID2D1Effect* This,
            [In, ComAliasName("UINT32")] uint inputCount
        );

        /// <summary>Returns the input image to the effect. The input could be another effect or a bitmap.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetInput(
            [In] ID2D1Effect* This,
            [In, ComAliasName("UINT32")] uint index,
            [Out] ID2D1Image** input
        );

        /// <summary>This returns the number of input that are bound into this effect.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetInputCount(
            [In] ID2D1Effect* This
        );

        /// <summary>Returns the output image of the given effect. This can be set as the input to another effect or can be drawn with DrawImage.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetOutput(
            [In] ID2D1Effect* This,
            [Out] ID2D1Image** outputImage
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Properties.Vtbl BaseVtbl;

            public SetInput SetInput;

            public SetInputCount SetInputCount;

            public GetInput GetInput;

            public GetInputCount GetInputCount;

            public GetOutput GetOutput;
            #endregion
        }
        #endregion
    }
}
