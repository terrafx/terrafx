// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Creates Direct2D resources.</summary>
    [Guid("BB12D362-DAEE-4B9A-AA1D-14BA401CFA1F")]
    public /* blittable */ unsafe struct ID2D1Factory1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>This creates a new Direct2D device from the given IDXGIDevice.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDevice(
            [In] ID2D1Factory1* This,
            [In] IDXGIDevice* dxgiDevice,
            [Out] ID2D1Device** d2dDevice
        );

        /// <summary>This creates a stroke style with the ability to preserve stroke width in various ways.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateStrokeStyle(
            [In] ID2D1Factory1* This,
            [In] /* readonly */ D2D1_STROKE_STYLE_PROPERTIES1* strokeStyleProperties,
            [In, Optional, ComAliasName("FLOAT")] /* readonly */ float* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [Out] ID2D1StrokeStyle1** strokeStyle
        );

        /// <summary>Creates a path geometry with new operational methods.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreatePathGeometry(
            [In] ID2D1Factory1* This,
            [Out] ID2D1PathGeometry1** pathGeometry
        );

        /// <summary>Creates a new drawing state block, this can be used in subsequent SaveDrawingState and RestoreDrawingState operations on the render target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDrawingStateBlock(
            [In] ID2D1Factory1* This,
            [In, Optional] /* readonly */ D2D1_DRAWING_STATE_DESCRIPTION1* drawingStateDescription,
            [In, Optional] IDWriteRenderingParams* textRenderingParams,
            [Out] ID2D1DrawingStateBlock1** drawingStateBlock
        );

        /// <summary>Creates a new GDI metafile.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateGdiMetafile(
            [In] ID2D1Factory1* This,
            [In] IStream* metafileStream,
            [Out] ID2D1GdiMetafile** metafile
        );

        /// <summary>This globally registers the given effect. The effect can later be instantiated by using the registered class id. The effect registration is reference counted.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RegisterEffectFromStream(
            [In] ID2D1Factory1* This,
            [In, ComAliasName("REFCLSID")] /* readonly */ Guid* classId,
            [In] IStream* propertyXml,
            [In, Optional] /* readonly */ D2D1_PROPERTY_BINDING* bindings,
            [In, ComAliasName("UINT32")] uint bindingsCount,
            [In] /* readonly */ PD2D1_EFFECT_FACTORY effectFactory
        );

        /// <summary>This globally registers the given effect. The effect can later be instantiated by using the registered class id. The effect registration is reference counted.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RegisterEffectFromString(
            [In] ID2D1Factory1* This,
            [In, ComAliasName("REFCLSID")] /* readonly */ Guid* classId,
            [In, ComAliasName("PCWSTR")] /* readonly */ char* propertyXml,
            [In, Optional] /* readonly */ D2D1_PROPERTY_BINDING* bindings,
            [In, ComAliasName("UINT32")] uint bindingsCount,
            [In] /* readonly */ PD2D1_EFFECT_FACTORY effectFactory
        );

        /// <summary>This unregisters the given effect by its class id, you need to call UnregisterEffect for every call to ID2D1Factory1::RegisterEffectFromStream and ID2D1Factory1::RegisterEffectFromString to completely unregister it.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int UnregisterEffect(
            [In] ID2D1Factory1* This,
            [In, ComAliasName("REFCLSID")] /* readonly */ Guid* classId
        );

        /// <summary>This returns all of the registered effects in the process, including any built-in effects.</summary>
        /// <param name="effectsReturned">The number of effects returned into the passed in effects array.</param>
        /// <param name="effectsRegistered">The number of effects currently registered in the system.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetRegisteredEffects(
            [In] ID2D1Factory1* This,
            [Out, Optional, ComAliasName("CLSID")] Guid* effects,
            [In, ComAliasName("UINT32")] uint effectsCount,
            [Out, ComAliasName("UINT32")] uint* effectsReturned = null,
            [Out, ComAliasName("UINT32")] uint* effectsRegistered = null
        );

        /// <summary>This retrieves the effect properties for the given effect, all of the effect properties will be set to a default value since an effect is not instantiated to implement the returned property interface.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetEffectProperties(
            [In] ID2D1Factory1* This,
            [In, ComAliasName("REFCLSID")] /* readonly */ Guid* effectId,
            [Out] ID2D1Properties** properties
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Factory.Vtbl BaseVtbl;

            public IntPtr CreateDevice;

            public IntPtr CreateStrokeStyle;

            public IntPtr CreatePathGeometry;

            public IntPtr CreateDrawingStateBlock;

            public IntPtr CreateGdiMetafile;

            public IntPtr RegisterEffectFromStream;

            public IntPtr RegisterEffectFromString;

            public IntPtr UnregisterEffect;

            public IntPtr GetRegisteredEffects;

            public IntPtr GetEffectProperties;
            #endregion
        }
        #endregion
    }
}
