// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents drawing state.</summary>
    [Guid("689F1F85-C72E-4E33-8F19-85754EFD5ACE")]
    public /* blittable */ unsafe struct ID2D1DrawingStateBlock1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] ID2D1DrawingStateBlock1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] ID2D1DrawingStateBlock1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] ID2D1DrawingStateBlock1* This
        );
        #endregion

        #region ID2D1Resource Delegates
        /// <summary>Retrieve the factory associated with this resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetFactory(
            [In] ID2D1DrawingStateBlock1* This,
            [Out] ID2D1Factory** factory
        );
        #endregion

        #region ID2D1DrawingStateBlock Delegates
        /// <summary>Retrieves the state currently contained within this state block resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDescription(
            [In] ID2D1DrawingStateBlock1* This,
            [Out] D2D1_DRAWING_STATE_DESCRIPTION* stateDescription
        );

        /// <summary>Sets the state description of this state block resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetDescription(
            [In] ID2D1DrawingStateBlock1* This,
            [In] D2D1_DRAWING_STATE_DESCRIPTION* stateDescription
        );

        /// <summary>Sets the text rendering parameters of this state block resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetTextRenderingParams(
            [In] ID2D1DrawingStateBlock1* This,
            [In] IDWriteRenderingParams* textRenderingParams = null
        );

        /// <summary>Retrieves the text rendering parameters contained within this state block resource. If a NULL text rendering parameter was specified, NULL will be returned.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetTextRenderingParams(
            [In] ID2D1DrawingStateBlock1* This,
            [Out] IDWriteRenderingParams** textRenderingParams
        );
        #endregion

        #region Delegates
        /// <summary>Retrieves the state currently contained within this state block resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDescription1(
            [In] ID2D1DrawingStateBlock1* This,
            [Out] D2D1_DRAWING_STATE_DESCRIPTION1* stateDescription
        );

        /// <summary>Sets the state description of this state block resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetDescription1(
            [In] ID2D1DrawingStateBlock1* This,
            [In] D2D1_DRAWING_STATE_DESCRIPTION1* stateDescription
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region ID2D1Resource Fields
            public IntPtr GetFactory;
            #endregion

            #region ID2D1DrawingStateBlock Fields
            public IntPtr GetDescription;

            public IntPtr SetDescription;

            public IntPtr SetTextRenderingParams;

            public IntPtr GetTextRenderingParams;
            #endregion

            #region Fields
            public IntPtr GetDescription1;

            public IntPtr SetDescription1;
            #endregion
        }
        #endregion
    }
}
