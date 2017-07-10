// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Creates Direct2D resources.</summary>
    [Guid("BB12D362-DAEE-4B9A-AA1D-14BA401CFA1F")]
    unsafe public /* blittable */ struct ID2D1Factory1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>This creates a new Direct2D device from the given IDXGIDevice.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateDevice(
            [In] ID2D1Factory1* This,
            [In] IDXGIDevice* dxgiDevice,
            [Out] ID2D1Device** d2dDevice
        );

        /// <summary>This creates a stroke style with the ability to preserve stroke width in various ways.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateStrokeStyle(
            [In] ID2D1Factory1* This,
            [In] /* readonly */ D2D1_STROKE_STYLE_PROPERTIES1* strokeStyleProperties,
            [In, Optional] /* readonly */ FLOAT* dashes,
            [In] UINT32 dashesCount,
            [Out] ID2D1StrokeStyle1** strokeStyle
        );

        /// <summary>Creates a path geometry with new operational methods.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreatePathGeometry(
            [In] ID2D1Factory1* This,
            [Out] ID2D1PathGeometry1** pathGeometry
        );

        /// <summary>Creates a new drawing state block, this can be used in subsequent SaveDrawingState and RestoreDrawingState operations on the render target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateDrawingStateBlock(
            [In] ID2D1Factory1* This,
            [In, Optional] /* readonly */ D2D1_DRAWING_STATE_DESCRIPTION1* drawingStateDescription,
            [In, Optional] IDWriteRenderingParams* textRenderingParams,
            [Out] ID2D1DrawingStateBlock1** drawingStateBlock
        );

        /// <summary>Creates a new GDI metafile.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateGdiMetafile(
            [In] ID2D1Factory1* This,
            [In] IStream* metafileStream,
            [Out] ID2D1GdiMetafile** metafile
        );

        /// <summary>This globally registers the given effect. The effect can later be instantiated by using the registered class id. The effect registration is reference counted.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RegisterEffectFromStream(
            [In] ID2D1Factory1* This,
            [In] REFCLSID classId,
            [In] IStream* propertyXml,
            [In, Optional] /* readonly */ D2D1_PROPERTY_BINDING* bindings,
            [In] UINT32 bindingsCount,
            [In] /* readonly */ PD2D1_EFFECT_FACTORY effectFactory
        );

        /// <summary>This globally registers the given effect. The effect can later be instantiated by using the registered class id. The effect registration is reference counted.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RegisterEffectFromString(
            [In] ID2D1Factory1* This,
            [In] REFCLSID classId,
            [In] PCWSTR propertyXml,
            [In, Optional] /* readonly */ D2D1_PROPERTY_BINDING* bindings,
            [In] UINT32 bindingsCount,
            [In] /* readonly */ PD2D1_EFFECT_FACTORY effectFactory
        );

        /// <summary>This unregisters the given effect by its class id, you need to call UnregisterEffect for every call to ID2D1Factory1::RegisterEffectFromStream and ID2D1Factory1::RegisterEffectFromString to completely unregister it.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT UnregisterEffect(
            [In] ID2D1Factory1* This,
            [In] REFCLSID classId
        );

        /// <summary>This returns all of the registered effects in the process, including any built-in effects.</summary>
        /// <param name="effectsReturned">The number of effects returned into the passed in effects array.</param>
        /// <param name="effectsRegistered">The number of effects currently registered in the system.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetRegisteredEffects(
            [In] ID2D1Factory1* This,
            [Out, Optional] CLSID* effects,
            [In] UINT32 effectsCount,
            [Out, Optional] UINT32* effectsReturned,
            [Out, Optional] UINT32* effectsRegistered
        );

        /// <summary>This retrieves the effect properties for the given effect, all of the effect properties will be set to a default value since an effect is not instantiated to implement the returned property interface.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetEffectProperties(
            [In] ID2D1Factory1* This,
            [In] REFCLSID effectId,
            [Out] ID2D1Properties** properties
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Factory.Vtbl BaseVtbl;

            public CreateDevice CreateDevice;

            public CreateStrokeStyle CreateStrokeStyle;

            public CreatePathGeometry CreatePathGeometry;

            public CreateDrawingStateBlock CreateDrawingStateBlock;

            public CreateGdiMetafile CreateGdiMetafile;

            public RegisterEffectFromStream RegisterEffectFromStream;

            public RegisterEffectFromString RegisterEffectFromString;

            public UnregisterEffect UnregisterEffect;

            public GetRegisteredEffects GetRegisteredEffects;

            public GetEffectProperties GetEffectProperties;
            #endregion
        }
        #endregion
    }
}
