// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Interface that encapsulates information used to render a glyph run.</summary>
    [Guid("7D97DBF7-E085-42D4-81E3-6A883BDED118")]
    unsafe public /* blittable */ struct IDWriteGlyphRunAnalysis
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Gets the bounding rectangle of the physical pixels affected by the glyph run.</summary>
        /// <param name="textureType">Specifies the type of texture requested. If a bi-level texture is requested, the bounding rectangle includes only bi-level glyphs. Otherwise, the bounding rectangle includes only anti-aliased glyphs.</param>
        /// <param name="textureBounds">Receives the bounding rectangle, or an empty rectangle if there are no glyphs if the specified type.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetAlphaTextureBounds(
            [In] IDWriteGlyphRunAnalysis* This,
            [In] DWRITE_TEXTURE_TYPE textureType,
            [Out] RECT* textureBounds
        );

        /// <summary>Creates an alpha texture of the specified type.</summary>
        /// <param name="textureType">Specifies the type of texture requested. If a bi-level texture is requested, the texture contains only bi-level glyphs. Otherwise, the texture contains only anti-aliased glyphs.</param>
        /// <param name="textureBounds">Specifies the bounding rectangle of the texture, which can be different than the bounding rectangle returned by GetAlphaTextureBounds.</param>
        /// <param name="alphaValues">Receives the array of alpha values.</param>
        /// <param name="bufferSize">Size of the alphaValues array. The minimum size depends on the dimensions of the rectangle and the type of texture requested.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateAlphaTexture(
            [In] IDWriteGlyphRunAnalysis* This,
            [In] DWRITE_TEXTURE_TYPE textureType,
            [In] /* readonly */ RECT* textureBounds,
            [Out, ComAliasName("BYTE")] byte* alphaValues,
            [In, ComAliasName("UINT32")] uint bufferSize
        );

        /// <summary>Gets properties required for ClearType blending.</summary>
        /// <param name="renderingParams">Rendering parameters object. In most cases, the values returned in the output parameters are based on the properties of this object. The exception is if a GDI-compatible rendering mode is specified.</param>
        /// <param name="blendGamma">Receives the gamma value to use for gamma correction.</param>
        /// <param name="blendEnhancedContrast">Receives the enhanced contrast value.</param>
        /// <param name="blendClearTypeLevel">Receives the ClearType level.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetAlphaBlendParams(
            [In] IDWriteGlyphRunAnalysis* This,
            [In] IDWriteRenderingParams* renderingParams,
            [Out, ComAliasName("FLOAT")] float* blendGamma,
            [Out, ComAliasName("FLOAT")] float* blendEnhancedContrast,
            [Out, ComAliasName("FLOAT")] float* blendClearTypeLevel
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetAlphaTextureBounds GetAlphaTextureBounds;

            public CreateAlphaTexture CreateAlphaTexture;

            public GetAlphaBlendParams GetAlphaBlendParams;
            #endregion
        }
        #endregion
    }
}
