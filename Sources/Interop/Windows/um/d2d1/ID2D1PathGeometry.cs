// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents a complex shape that may be composed of arcs, curves, and lines.</summary>
    [Guid("2CD906A5-12E2-11DC-9FED-001143A055F9")]
    public /* blittable */ unsafe struct ID2D1PathGeometry
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Opens a geometry sink that will be used to create this path geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Open(
            [In] ID2D1PathGeometry* This,
            [Out] ID2D1GeometrySink** geometrySink
        );

        /// <summary>Retrieve the contents of this geometry. The caller passes an implementation of a
        /// ID2D1GeometrySink interface to receive the data.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Stream(
            [In] ID2D1PathGeometry* This,
            [In] ID2D1GeometrySink* geometrySink
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSegmentCount(
            [In] ID2D1PathGeometry* This,
            [Out, ComAliasName("UINT32")] uint* count
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFigureCount(
            [In] ID2D1PathGeometry* This,
            [Out, ComAliasName("UINT32")] uint* count
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Geometry.Vtbl BaseVtbl;

            public IntPtr Open;

            public IntPtr Stream;

            public IntPtr GetSegmentCount;

            public IntPtr GetFigureCount;
            #endregion
        }
        #endregion
    }
}
