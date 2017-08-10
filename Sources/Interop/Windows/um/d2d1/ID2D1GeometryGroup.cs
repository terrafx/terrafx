// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents a composite geometry, composed of other ID2D1Geometry objects.</summary>
    [Guid("2CD906A6-12E2-11DC-9FED-001143A055F9")]
    unsafe public /* blittable */ struct ID2D1GeometryGroup
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_FILL_MODE GetFillMode(
            [In] ID2D1GeometryGroup* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetSourceGeometryCount(
            [In] ID2D1GeometryGroup* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetSourceGeometries(
            [In] ID2D1GeometryGroup* This,
            [Out] ID2D1Geometry** geometries,
            [In, ComAliasName("UINT32")] uint geometriesCount
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Geometry.Vtbl BaseVtbl;

            public IntPtr GetFillMode;

            public IntPtr GetSourceGeometryCount;

            public IntPtr GetSourceGeometries;
            #endregion
        }
        #endregion
    }
}
