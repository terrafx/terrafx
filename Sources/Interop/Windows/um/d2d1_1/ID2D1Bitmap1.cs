// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents a bitmap that can be used as a surface for an ID2D1DeviceContext or mapped into system memory, and can contain additional color context information.</summary>
    [Guid("A898A84C-3873-4588-B08B-EBBF978DF041")]
    unsafe public /* blittable */ struct ID2D1Bitmap1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Retrieves the color context information associated with the bitmap.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetColorContext(
            [In] ID2D1Bitmap1* This,
            [Out] ID2D1ColorContext** colorContext
        );

        /// <summary>Retrieves the bitmap options used when creating the API.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_BITMAP_OPTIONS GetOptions(
            [In] ID2D1Bitmap1* This
        );

        /// <summary>Retrieves the DXGI surface from the corresponding bitmap, if the bitmap was created from a device derived from a D3D device.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSurface(
            [In] ID2D1Bitmap1* This,
            [Out] IDXGISurface** dxgiSurface
        );

        /// <summary>Maps the given bitmap into memory. The bitmap must have been created with the D2D1_BITMAP_OPTIONS_CPU_READ flag.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Map(
            [In] ID2D1Bitmap1* This,
            [In] D2D1_MAP_OPTIONS options,
            [Out] D2D1_MAPPED_RECT* mappedRect
        );

        /// <summary>Unmaps the given bitmap from memory.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Unmap(
            [In] ID2D1Bitmap1* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Bitmap.Vtbl BaseVtbl;

            public GetColorContext GetColorContext;

            public GetOptions GetOptions;

            public GetSurface GetSurface;

            public Map Map;

            public Unmap Unmap;
            #endregion
        }
        #endregion
    }
}
