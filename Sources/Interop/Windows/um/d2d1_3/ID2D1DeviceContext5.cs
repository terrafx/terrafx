// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("7836D248-68CC-4DF6-B9E8-DE991BF62EB7")]
    unsafe public /* blittable */ struct ID2D1DeviceContext5
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Creates an SVG document from a stream.</summary>
        /// <param name="inputXmlStream">An input stream containing the SVG XML document. If null, an empty document is created.</param>
        /// <param name="viewportSize">Size of the initial viewport of the document.</param>
        /// <param name="svgDocument">When this method returns, contains a pointer to the SVG document.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateSvgDocument(
            [In] ID2D1DeviceContext5* This,
            [In, Optional] IStream* inputXmlStream,
            [In] D2D1_SIZE_F viewportSize,
            [Out] ID2D1SvgDocument** svgDocument
        );

        /// <summary>Draw an SVG document.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawSvgDocument(
            [In] ID2D1DeviceContext5* This,
            [In] ID2D1SvgDocument* svgDocument
        );

        /// <summary>Creates a color context from a DXGI color space type. It is only valid to use this with the Color Management Effect in 'Best' mode.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateColorContextFromDxgiColorSpace(
            [In] ID2D1DeviceContext5* This,
            [In] DXGI_COLOR_SPACE_TYPE colorSpace,
            [Out] ID2D1ColorContext1** colorContext
        );

        /// <summary>Creates a color context from a simple color profile. It is only valid to use this with the Color Management Effect in 'Best' mode.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateColorContextFromSimpleColorProfile(
            [In] ID2D1DeviceContext5* This,
            [In] /* readonly */ D2D1_SIMPLE_COLOR_PROFILE* simpleProfile,
            [Out] ID2D1ColorContext1** colorContext
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1DeviceContext4.Vtbl BaseVtbl;

            public CreateSvgDocument CreateSvgDocument;

            public DrawSvgDocument DrawSvgDocument;

            public CreateColorContextFromDxgiColorSpace CreateColorContextFromDxgiColorSpace;

            public CreateColorContextFromSimpleColorProfile CreateColorContextFromSimpleColorProfile;
            #endregion
        }
        #endregion
    }
}
