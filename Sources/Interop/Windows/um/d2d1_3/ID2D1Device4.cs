// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("D7BDB159-5683-4A46-BC9C-72DC720B858B")]
    unsafe public /* blittable */ struct ID2D1Device4
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Creates a new device context with no initially assigned target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDeviceContext(
            [In] ID2D1Device4* This,
            [In] D2D1_DEVICE_CONTEXT_OPTIONS options,
            [Out] ID2D1DeviceContext4** deviceContext4
        );

        /// <summary>Sets the maximum capacity of the color glyph cache. This cache is used to store color bitmap glyphs and SVG glyphs, enabling faster performance if the same glyphs are needed again. If the application still references a glyph using GetColorBitmapGlyphImage or GetSvgGlyphImage after it has been evicted, this glyph does not count toward the cache capacity.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetMaximumColorGlyphCacheMemory(
            [In] ID2D1Device4* This,
            [In, ComAliasName("UINT64")] ulong maximumInBytes
        );

        /// <summary>Gets the maximum capacity of the color glyph cache.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetMaximumColorGlyphCacheMemory(
            [In] ID2D1Device4* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Device3.Vtbl BaseVtbl;

            public IntPtr CreateDeviceContext;

            public IntPtr SetMaximumColorGlyphCacheMemory;

            public IntPtr GetMaximumColorGlyphCacheMemory;
            #endregion
        }
        #endregion
    }
}
