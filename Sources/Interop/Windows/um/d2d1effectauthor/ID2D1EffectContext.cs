// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The internal context handed to effect authors to create transforms from effects and any other operation tied to context which is not useful to the application facing API.</summary>
    [Guid("3D9F916B-27DC-4AD7-B4F1-64945340F563")]
    unsafe public /* blittable */ struct ID2D1EffectContext
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDpi(
            [In] ID2D1EffectContext* This,
            [Out, ComAliasName("FLOAT")] float* dpiX,
            [Out, ComAliasName("FLOAT")] float* dpiY
        );

        /// <summary>Create a new effect, the effect must either be built in or previously registered through ID2D1Factory1::RegisterEffect.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateEffect(
            [In] ID2D1EffectContext* This,
            [In, ComAliasName("REFCLSID")] /* readonly */ Guid* effectId,
            [Out] ID2D1Effect** effect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetMaximumSupportedFeatureLevel(
            [In] ID2D1EffectContext* This,
            [In] /* readonly */ D3D_FEATURE_LEVEL* featureLevels,
            [In, ComAliasName("UINT32")] uint featureLevelsCount,
            [Out] D3D_FEATURE_LEVEL* maximumSupportedFeatureLevel
        );

        /// <summary>Create a transform node from the passed in effect.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateTransformNodeFromEffect(
            [In] ID2D1EffectContext* This,
            [In] ID2D1Effect* effect,
            [Out] ID2D1TransformNode** transformNode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBlendTransform(
            [In] ID2D1EffectContext* This,
            [In, ComAliasName("UINT32")] uint numInputs,
            [In] /* readonly */ D2D1_BLEND_DESCRIPTION* blendDescription,
            [Out] ID2D1BlendTransform** transform
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBorderTransform(
            [In] ID2D1EffectContext* This,
            [In] D2D1_EXTEND_MODE extendModeX,
            [In] D2D1_EXTEND_MODE extendModeY,
            [Out] ID2D1BorderTransform** transform
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateOffsetTransform(
            [In] ID2D1EffectContext* This,
            [In, ComAliasName("D2D1_POINT_2L")] POINT offset,
            [Out] ID2D1OffsetTransform** transform
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBoundsAdjustmentTransform(
            [In] ID2D1EffectContext* This,
            [In, ComAliasName("D2D1_RECT_L")] /* readonly */ RECT* outputRectangle,
            [Out] ID2D1BoundsAdjustmentTransform** transform
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int LoadPixelShader(
            [In] ID2D1EffectContext* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* shaderId,
            [In, ComAliasName("BYTE")] /* readonly */ byte* shaderBuffer,
            [In, ComAliasName("UINT32")] uint shaderBufferCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int LoadVertexShader(
            [In] ID2D1EffectContext* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* resourceId,
            [In, ComAliasName("BYTE")] /* readonly */ byte* shaderBuffer,
            [In, ComAliasName("UINT32")] uint shaderBufferCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int LoadComputeShader(
            [In] ID2D1EffectContext* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* resourceId,
            [In, ComAliasName("BYTE")]  /* readonly */ byte* shaderBuffer,
            [In, ComAliasName("UINT32")] uint shaderBufferCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsShaderLoaded(
            [In] ID2D1EffectContext* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* shaderId
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateResourceTexture(
            [In] ID2D1EffectContext* This,
            [In, Optional, ComAliasName("GUID")] /* readonly */ Guid* resourceId,
            [In] /* readonly */ D2D1_RESOURCE_TEXTURE_PROPERTIES* resourceTextureProperties,
            [In, Optional, ComAliasName("BYTE")] /* readonly */ byte* data,
            [In, Optional, ComAliasName("UINT32")] /* readonly */ uint* strides,
            [In, ComAliasName("UINT32")] uint dataSize,
            [Out] ID2D1ResourceTexture** resourceTexture
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int FindResourceTexture(
            [In] ID2D1EffectContext* This,
            [In, ComAliasName("GUID")] /* readonly */ Guid* resourceId,
            [Out] ID2D1ResourceTexture** resourceTexture
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateVertexBuffer(
            [In] ID2D1EffectContext* This,
            [In] /* readonly */ D2D1_VERTEX_BUFFER_PROPERTIES* vertexBufferProperties,
            [In, Optional, ComAliasName("GUID")] /* readonly */ Guid* resourceId,
            [In, Optional] /* readonly */ D2D1_CUSTOM_VERTEX_BUFFER_PROPERTIES* customVertexBufferProperties,
            [Out] ID2D1VertexBuffer** buffer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int FindVertexBuffer(
            [In] ID2D1EffectContext* This,
            [In, ComAliasName("GUID")] /* readonly */ Guid* resourceId,
            [Out] ID2D1VertexBuffer** buffer
        );

        /// <summary>Creates a color context from a color space.  If the space is Custom, the context is initialized from the profile/profileSize arguments.  Otherwise the context is initialized with the profile bytes associated with the space and profile/profileSize are ignored.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateColorContext(
            [In] ID2D1EffectContext* This,
            [In] D2D1_COLOR_SPACE space,
            [In, Optional, ComAliasName("BYTE")] /* readonly */ byte* profile,
            [In, ComAliasName("UINT32")] uint profileSize,
            [Out] ID2D1ColorContext** colorContext
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateColorContextFromFilename(
            [In] ID2D1EffectContext* This,
            [In, ComAliasName("PCWSTR")] /* readonly */ char* filename,
            [Out] ID2D1ColorContext** colorContext
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateColorContextFromWicColorContext(
            [In] ID2D1EffectContext* This,
            [In] IWICColorContext* wicColorContext,
            [Out] ID2D1ColorContext** colorContext
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CheckFeatureSupport(
            [In] ID2D1EffectContext* This,
            [In] D2D1_FEATURE feature,
            [Out] void* featureSupportData,
            [In, ComAliasName("UINT32")] uint featureSupportDataSize
        );

        /// <summary>Indicates whether the buffer precision is supported by D2D.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsBufferPrecisionSupported(
            [In] ID2D1EffectContext* This,
            [In] D2D1_BUFFER_PRECISION bufferPrecision
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetDpi GetDpi;

            public CreateEffect CreateEffect;

            public GetMaximumSupportedFeatureLevel GetMaximumSupportedFeatureLevel;

            public CreateTransformNodeFromEffect CreateTransformNodeFromEffect;

            public CreateBlendTransform CreateBlendTransform;

            public CreateBorderTransform CreateBorderTransform;

            public CreateOffsetTransform CreateOffsetTransform;

            public CreateBoundsAdjustmentTransform CreateBoundsAdjustmentTransform;

            public LoadPixelShader LoadPixelShader;

            public LoadVertexShader LoadVertexShader;

            public LoadComputeShader LoadComputeShader;

            public IsShaderLoaded IsShaderLoaded;

            public CreateResourceTexture CreateResourceTexture;

            public FindResourceTexture FindResourceTexture;

            public CreateVertexBuffer CreateVertexBuffer;

            public FindVertexBuffer FindVertexBuffer;

            public CreateColorContext CreateColorContext;

            public CreateColorContextFromFilename CreateColorContextFromFilename;

            public CreateColorContextFromWicColorContext CreateColorContextFromWicColorContext;

            public CheckFeatureSupport CheckFeatureSupport;

            public IsBufferPrecisionSupported IsBufferPrecisionSupported;
            #endregion
        }
        #endregion
    }
}
