// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Converts Direct2D primitives stored in an ID2D1CommandList into a fixed page representation. The print sub-system then consumes the primitives.</summary>
    [Guid("2C1D867D-C290-41C8-AE7E-34A98702E9A5")]
    unsafe public /* blittable */ struct ID2D1PrintControl
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddPage(
            [In] ID2D1PrintControl* This,
            [In] ID2D1CommandList* commandList,
            [In] D2D_SIZE_F pageSize,
            [In, Optional] IStream* pagePrintTicketStream,
            [Out, ComAliasName("D2D1_TAG")] ulong* tag1 = null,
            [Out, ComAliasName("D2D1_TAG")] ulong* tag2 = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Close(
            [In] ID2D1PrintControl* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public AddPage AddPage;

            public Close Close;
            #endregion
        }
        #endregion
    }
}
