// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>This interface performs all the same functions as the existing ID2D1CommandSink interface. It also enables access to the new primitive blend modes, MIN and ADD, through its SetPrimitiveBlend1 method.</summary>
    [Guid("9EB767FD-4269-4467-B8C2-EB30CB305743")]
    unsafe public /* blittable */ struct ID2D1CommandSink1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>This method is called if primitiveBlend value was added after Windows 8. SetPrimitiveBlend method is used for Win8 values (_SOURCE_OVER and _COPY).</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPrimitiveBlend1(
            [In] ID2D1CommandSink1* This,
            [In] D2D1_PRIMITIVE_BLEND primitiveBlend
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1CommandSink.Vtbl BaseVtbl;

            public SetPrimitiveBlend1 SetPrimitiveBlend1;
            #endregion
        }
        #endregion
    }
}
