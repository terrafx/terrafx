// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("688D15C3-02B0-438D-B13A-D1B44C32C39A")]
    unsafe public /* blittable */ struct ID2D1ResourceTexture
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Update the vertex text.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Update(
            [In] ID2D1ResourceTexture* This,
            [In, Optional] /* readonly */ UINT32* minimumExtents,
            [In, Optional] /* readonly */ UINT32* maximimumExtents,
            [In, Optional] /* readonly */ UINT32* strides,
            [In] UINT32 dimensions,
            [In]  /* readonly */ BYTE* data,
            [In] UINT32 dataCount
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public Update Update;
            #endregion
        }
        #endregion
    }
}
