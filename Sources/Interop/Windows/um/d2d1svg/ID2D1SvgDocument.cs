// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1svg.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("86B88E4D-AFA4-4D7B-88E4-68A51C4A0AEC")]
    unsafe public /* blittable */ struct ID2D1SvgDocument
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Sets the size of the initial viewport.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetViewportSize(
            [In] ID2D1SvgDocument* This,
            [In, ComAliasName("D2D1_SIZE_F")] D2D_SIZE_F viewportSize
        );

        /// <summary>Returns the size of the initial viewport.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("D2D1_SIZE_F")]
        public /* static */ delegate D2D_SIZE_F GetViewportSize(
            [In] ID2D1SvgDocument* This
        );

        /// <summary>Sets the root element of the document. The root element must be an 'svg' element. If the element already exists within an svg tree, it is first removed.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetRoot(
            [In] ID2D1SvgDocument* This,
            [In, Optional] ID2D1SvgElement* root
        );

        /// <summary>Gets the root element of the document.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetRoot(
            [In] ID2D1SvgDocument* This,
            [Out] ID2D1SvgElement** root
        );

        /// <summary>Gets the SVG element with the specified ID. If the element cannot be found, the returned element will be null.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int FindElementById(
            [In] ID2D1SvgDocument* This,
            [In, ComAliasName("PCWSTR")] /* readonly */ char* id,
            [Out] ID2D1SvgElement** svgElement
        );

        /// <summary>Serializes an element and its subtree to XML. The output XML is encoded as UTF-8.</summary>
        /// <param name="outputXmlStream">An output stream to contain the SVG XML subtree.</param>
        /// <param name="subtree">The root of the subtree. If null, the entire document is serialized.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Serialize(
            [In] ID2D1SvgDocument* This,
            [In] IStream* outputXmlStream,
            [In] ID2D1SvgElement* subtree = null
        );

        /// <summary>Deserializes a subtree from the stream. The stream must have only one root element, but that root element need not be an 'svg' element. The output element is not inserted into this document tree.</summary>
        /// <param name="inputXmlStream">An input stream containing the SVG XML subtree.</param>
        /// <param name="subtree">The root of the subtree.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Deserialize(
            [In] ID2D1SvgDocument* This,
            [In] IStream* inputXmlStream,
            [Out] ID2D1SvgElement** subtree
        );

        /// <summary>Creates a paint object which can be used to set the 'fill' or 'stroke' properties.</summary>
        /// <param name="color">The color used if the paintType is D2D1_SVG_PAINT_TYPE_COLOR.</param>
        /// <param name="id">The element id which acts as the paint server. This id is used if the paint type is D2D1_SVG_PAINT_TYPE_URI.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreatePaint(
            [In] ID2D1SvgDocument* This,
            [In] D2D1_SVG_PAINT_TYPE paintType,
            [In, Optional, ComAliasName("D2D1_COLOR_F")] /* readonly */ DXGI_RGBA* color,
            [In, Optional, ComAliasName("PCWSTR")] /* readonly */ char* id,
            [Out] ID2D1SvgPaint** paint
        );

        /// <summary>Creates a dash array object which can be used to set the 'stroke-dasharray' property.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateStrokeDashArray(
            [In] ID2D1SvgDocument* This,
            [In, Optional] /* readonly */ D2D1_SVG_LENGTH* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [Out] ID2D1SvgStrokeDashArray** strokeDashArray
        );

        /// <summary>Creates a points object which can be used to set a 'points' attribute on a 'polygon' or 'polyline' element.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreatePointCollection(
            [In] ID2D1SvgDocument* This,
            [In, Optional, ComAliasName("D2D1_POINT_2F")] /* readonly */ D2D_POINT_2F* points,
            [In, ComAliasName("UINT32")] uint pointsCount,
            [Out] ID2D1SvgPointCollection** pointCollection
        );

        /// <summary>Creates a path data object which can be used to set a 'd' attribute on a 'path' element.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreatePathData(
            [In] ID2D1SvgDocument* This,
            [In, Optional, ComAliasName("FLOAT")] /* readonly */ float* segmentData,
            [In, ComAliasName("UINT32")] uint segmentDataCount,
            [In, Optional] /* readonly */ D2D1_SVG_PATH_COMMAND* commands,
            [In, ComAliasName("UINT32")] uint commandsCount,
            [Out] ID2D1SvgPathData** pathData
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;

            public IntPtr SetViewportSize;

            public IntPtr GetViewportSize;

            public IntPtr SetRoot;

            public IntPtr GetRoot;

            public IntPtr FindElementById;

            public IntPtr Serialize;

            public IntPtr Deserialize;

            public IntPtr CreatePaint;

            public IntPtr CreateStrokeDashArray;

            public IntPtr CreatePointCollection;

            public IntPtr CreatePathData;
            #endregion
        }
        #endregion
    }
}
