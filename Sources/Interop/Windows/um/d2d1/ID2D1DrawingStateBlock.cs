// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents the drawing state of a render target: the antialiasing mode, transform, tags, and text-rendering options.</summary>
    [Guid("28506E39-EBF6-46A1-BB47-FD85565AB957")]
    unsafe public /* blittable */ struct ID2D1DrawingStateBlock
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Retrieves the state currently contained within this state block resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDescription(
            [In] ID2D1DrawingStateBlock* This,
            [Out] D2D1_DRAWING_STATE_DESCRIPTION* stateDescription
        );

        /// <summary>Sets the state description of this state block resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetDescription(
            [In] ID2D1DrawingStateBlock* This,
            [In] /* readonly */ D2D1_DRAWING_STATE_DESCRIPTION* stateDescription
        );

        /// <summary>Sets the text rendering parameters of this state block resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetTextRenderingParams(
            [In] ID2D1DrawingStateBlock* This,
            [In] IDWriteRenderingParams* textRenderingParams = null
        );

        /// <summary>Retrieves the text rendering parameters contained within this state block resource. If a NULL text rendering parameter was specified, NULL will be returned.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetTextRenderingParams(
            [In] ID2D1DrawingStateBlock* This,
            [Out] IDWriteRenderingParams** textRenderingParams
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;

            public IntPtr GetDescription;

            public IntPtr SetDescription;

            public IntPtr SetTextRenderingParams;

            public IntPtr GetTextRenderingParams;
            #endregion
        }
        #endregion
    }
}
