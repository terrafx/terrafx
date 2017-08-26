// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("688D15C3-02B0-438D-B13A-D1B44C32C39A")]
    public /* blittable */ unsafe struct ID2D1ResourceTexture
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Update the vertex text.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Update(
            [In] ID2D1ResourceTexture* This,
            [In, Optional, ComAliasName("UINT32")] /* readonly */ uint* minimumExtents,
            [In, Optional, ComAliasName("UINT32")] /* readonly */ uint* maximimumExtents,
            [In, Optional, ComAliasName("UINT32")] /* readonly */ uint* strides,
            [In, ComAliasName("UINT32")] uint dimensions,
            [In, ComAliasName("BYTE")]  /* readonly */ byte* data,
            [In, ComAliasName("UINT32")] uint dataCount
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr Update;
            #endregion
        }
        #endregion
    }
}
