// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents a device-dependent representation of a gradient mesh composed of patches. Use the ID2D1DeviceContext2::CreateGradientMesh method to create an instance of ID2D1GradientMesh.</summary>
    [Guid("F292E401-C050-4CDE-83D7-04962D3B23C2")]
    unsafe public /* blittable */ struct ID2D1GradientMesh
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Returns the number of patches of the gradient mesh.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetPatchCount(
            [In] ID2D1GradientMesh* This
        );

        /// <summary>Retrieve the patch data stored in the gradient mesh.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPatches(
            [In] ID2D1GradientMesh* This,
            [In, ComAliasName("UINT32")] uint startIndex,
            [Out] D2D1_GRADIENT_MESH_PATCH* patches,
            [In, ComAliasName("UINT32")] uint patchesCount
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;

            public GetPatchCount GetPatchCount;

            public GetPatches GetPatches;
            #endregion
        }
        #endregion
    }
}
