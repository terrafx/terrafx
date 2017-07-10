// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents a producer of pixels that can fill an arbitrary 2D plane.</summary>
    [Guid("C9B664E5-74A1-4378-9AC2-EEFC37A3F4D8")]
    unsafe public /* blittable */ struct ID2D1ImageSource
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT OfferResources(
            [In] ID2D1ImageSource* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT TryReclaimResources(
            [In] ID2D1ImageSource* This,
            [Out] BOOL* resourcesDiscarded
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Image.Vtbl BaseVtbl;

            public OfferResources OfferResources;

            public TryReclaimResources TryReclaimResources;
            #endregion
        }
        #endregion
    }
}
