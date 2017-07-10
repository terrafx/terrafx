// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>This is the interface implemented by an effect author, along with the constructor and registration information.</summary>
    [Guid("A248FD3F-3E6C-4E63-9F03-7F68ECC91DB9")]
    unsafe public /* blittable */ struct ID2D1EffectImpl
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Initialize the effect with a context and a transform graph. The effect must populate the transform graph with a topology and can update it later.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Initialize(
            [In] ID2D1EffectImpl* This,
            [In] ID2D1EffectContext* effectContext,
            [In] ID2D1TransformGraph* transformGraph
        );

        /// <summary>Initialize the effect with a context and a transform graph. The effect must populate the transform graph with a topology and can update it later.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PrepareForRender(
            [In] ID2D1EffectImpl* This,
            [In] D2D1_CHANGE_TYPE changeType
        );

        /// <summary>Sets a new transform graph to the effect.  This happens when the number of inputs to the effect changes, if the effect support a variable number of inputs.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetGraph(
            [In] ID2D1EffectImpl* This,
            [In] ID2D1TransformGraph* transformGraph
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public Initialize Initialize;

            public PrepareForRender PrepareForRender;

            public SetGraph SetGraph;
            #endregion
        }
        #endregion
    }
}
