// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Resource interface that holds pen style properties.</summary>
    [Guid("2CD9069D-12E2-11DC-9FED-001143A055F9")]
    unsafe public /* blittable */ struct ID2D1StrokeStyle
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_CAP_STYLE GetStartCap(
            [In] ID2D1StrokeStyle* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_CAP_STYLE GetEndCap(
            [In] ID2D1StrokeStyle* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_CAP_STYLE GetDashCap(
            [In] ID2D1StrokeStyle* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("FLOAT")]
        public /* static */ delegate float GetMiterLimit(
            [In] ID2D1StrokeStyle* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_LINE_JOIN GetLineJoin(
            [In] ID2D1StrokeStyle* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("FLOAT")]
        public /* static */ delegate float GetDashOffset(
            [In] ID2D1StrokeStyle* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_DASH_STYLE GetDashStyle(
            [In] ID2D1StrokeStyle* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetDashesCount(
            [In] ID2D1StrokeStyle* This
        );

        /// <summary>Returns the dashes from the object into a user allocated array. The user must call GetDashesCount to retrieve the required size.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDashes(
            [In] ID2D1StrokeStyle* This,
            [Out, ComAliasName("FLOAT")] float* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;

            public GetStartCap GetStartCap;

            public GetEndCap GetEndCap;

            public GetDashCap GetDashCap;

            public GetMiterLimit GetMiterLimit;

            public GetLineJoin GetLineJoin;

            public GetDashOffset GetDashOffset;

            public GetDashStyle GetDashStyle;

            public GetDashesCount GetDashesCount;

            public GetDashes GetDashes;
            #endregion
        }
        #endregion
    }
}
