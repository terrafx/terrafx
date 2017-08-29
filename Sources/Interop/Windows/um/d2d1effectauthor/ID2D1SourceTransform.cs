// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface implemented by a transform author to provide a CPU based source effect.</summary>
    [Guid("DB1800DD-0C34-4CF9-BE90-31CC0A5653E1")]
    public /* blittable */ unsafe struct ID2D1SourceTransform
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] ID2D1SourceTransform* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] ID2D1SourceTransform* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] ID2D1SourceTransform* This
        );
        #endregion

        #region ID2D1TransformNode Delegates
        /// <summary>Return the number of input this node has.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetInputCount(
            [In] ID2D1SourceTransform* This
        );
        #endregion

        #region ID2D1Transform Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MapOutputRectToInputRects(
            [In] ID2D1SourceTransform* This,
            [In, ComAliasName("D2D1_RECT_L")] RECT* outputRect,
            [Out, ComAliasName("D2D1_RECT_L[]")] RECT* inputRects,
            [In, ComAliasName("UINT32")] uint inputRectsCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MapInputRectsToOutputRect(
            [In] ID2D1SourceTransform* This,
            [In, ComAliasName("D2D1_RECT_L[]")] RECT* inputRects,
            [In, ComAliasName("D2D1_RECT_L[]")] RECT* inputOpaqueSubRects,
            [In, ComAliasName("UINT32")] uint inputRectCount,
            [Out, ComAliasName("D2D1_RECT_L")] RECT* outputRect,
            [Out, ComAliasName("D2D1_RECT_L")] RECT* outputOpaqueSubRect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MapInvalidRect(
            [In] ID2D1SourceTransform* This,
            [In, ComAliasName("UINT32")] uint inputIndex,
            [In, ComAliasName("D2D1_RECT_L")] RECT invalidInputRect,
            [Out, ComAliasName("D2D1_RECT_L")] RECT* invalidOutputRect
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetRenderInfo(
            [In] ID2D1SourceTransform* This,
            [In] ID2D1RenderInfo* renderInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Draw(
            [In] ID2D1SourceTransform* This,
            [In] ID2D1Bitmap1* target,
            [In, ComAliasName("D2D1_RECT_L")] RECT* drawRect,
            [In, ComAliasName("D2D1_POINT_2U")] D2D_POINT_2U targetOrigin
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

            #region ID2D1TransformNode Fields
            public IntPtr GetInputCount;
            #endregion

            #region ID2D1Transform Fields
            public IntPtr MapOutputRectToInputRects;

            public IntPtr MapInputRectsToOutputRect;

            public IntPtr MapInvalidRect;
            #endregion

            #region Fields
            public IntPtr SetRenderInfo;

            public IntPtr Draw;
            #endregion
        }
        #endregion
    }
}
