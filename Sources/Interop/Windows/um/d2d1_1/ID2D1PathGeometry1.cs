// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The ID2D1PathGeometry1 interface adds functionality to ID2D1PathGeometry. In particular, it provides the path geometry-specific ComputePointAndSegmentAtLength method.</summary>
    [Guid("62BAA2D2-AB54-41B7-B872-787E0106A421")]
    unsafe public /* blittable */ struct ID2D1PathGeometry1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT ComputePointAndSegmentAtLength(
            [In] ID2D1PathGeometry1* This,
            [In] FLOAT length,
            [In] UINT32 startSegment,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [In] FLOAT flatteningTolerance,
            [Out] D2D1_POINT_DESCRIPTION* pointDescription
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1PathGeometry.Vtbl BaseVtbl;

            public ComputePointAndSegmentAtLength ComputePointAndSegmentAtLength;
            #endregion
        }
        #endregion
    }
}
