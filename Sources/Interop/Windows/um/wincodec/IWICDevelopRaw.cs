// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("FBEC5E44-F7BE-4B65-B7F8-C0C81FEF026D")]
    unsafe public /* blittable */ struct IWICDevelopRaw
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT QueryRawCapabilitiesInfo(
            [In] IWICDevelopRaw* This,
            [In, Out] WICRawCapabilitiesInfo* pInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT LoadParameterSet(
            [In] IWICDevelopRaw* This,
            [In] WICRawParameterSet ParameterSet
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetCurrentParameterSet(
            [In] IWICDevelopRaw* This,
            [Out, Optional] IPropertyBag2** ppCurrentParameterSet
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetExposureCompensation(
            [In] IWICDevelopRaw* This,
            [In] double ev
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetExposureCompensation(
            [In] IWICDevelopRaw* This,
            [Out] double* pEV
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetWhitePointRGB(
            [In] IWICDevelopRaw* This,
            [In] UINT Red,
            [In] UINT Green,
            [In] UINT Blue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetWhitePointRGB(
            [In] IWICDevelopRaw* This,
            [Out] UINT* pRed,
            [Out] UINT* pGreen,
            [Out] UINT* pBlue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetNamedWhitePoint(
            [In] IWICDevelopRaw* This,
            [In] WICNamedWhitePoint WhitePoint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetNamedWhitePoint(
            [In] IWICDevelopRaw* This,
            [Out] WICNamedWhitePoint* pWhitePoint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetWhitePointKelvin(
            [In] IWICDevelopRaw* This,
            [In] UINT WhitePointKelvin
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetWhitePointKelvin(
            [In] IWICDevelopRaw* This,
            [Out] UINT* pWhitePointKelvin
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetKelvinRangeInfo(
            [In] IWICDevelopRaw* This,
            [Out] UINT* pMinKelvinTemp,
            [Out] UINT* pMaxKelvinTemp,
            [Out] UINT* pKelvinTempStepValue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetContrast(
            [In] IWICDevelopRaw* This,
            [In] double Contrast
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetContrast(
            [In] IWICDevelopRaw* This,
            [Out] double* pContrast
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetGamma(
            [In] IWICDevelopRaw* This,
            [In] double Gamma
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetGamma(
            [In] IWICDevelopRaw* This,
            [Out] double* pGamma
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetSharpness(
            [In] IWICDevelopRaw* This,
            [In] double Sharpness
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSharpness(
            [In] IWICDevelopRaw* This,
            [Out] double* pSharpness
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetSaturation(
            [In] IWICDevelopRaw* This,
            [In] double Saturation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSaturation(
            [In] IWICDevelopRaw* This,
            [Out] double* pSaturation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetTint(
            [In] IWICDevelopRaw* This,
            [In] double Tint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetTint(
            [In] IWICDevelopRaw* This,
            [Out] double* pTint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetNoiseReduction(
            [In] IWICDevelopRaw* This,
            [In] double NoiseReduction
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetNoiseReduction(
            [In] IWICDevelopRaw* This,
            [Out] double* pNoiseReduction
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetDestinationColorContext(
            [In] IWICDevelopRaw* This,
            [In, Optional] IWICColorContext* pColorContext
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetToneCurve(
            [In] IWICDevelopRaw* This,
            [In] UINT cbToneCurveSize,
            [In] /* readonly */ WICRawToneCurve* pToneCurve
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetToneCurve(
            [In] IWICDevelopRaw* This,
            [In] UINT cbToneCurveBufferSize,
            [Out, Optional] WICRawToneCurve* pToneCurve,
            [In, Out, Optional] UINT* pcbActualToneCurveBufferSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetRotation(
            [In] IWICDevelopRaw* This,
            [In] double Rotation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetRotation(
            [In] IWICDevelopRaw* This,
            [Out] double* pRotation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetRenderMode(
            [In] IWICDevelopRaw* This,
            [In] WICRawRenderMode RenderMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetRenderMode(
            [In] IWICDevelopRaw* This,
            [Out] WICRawRenderMode* pRenderMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetNotificationCallback(
            [In] IWICDevelopRaw* This,
            [In, Optional] IWICDevelopRawNotificationCallback* pCallback
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICBitmapFrameDecode.Vtbl BaseVtbl;

            public QueryRawCapabilitiesInfo QueryRawCapabilitiesInfo;

            public LoadParameterSet LoadParameterSet;

            public GetCurrentParameterSet GetCurrentParameterSet;

            public SetExposureCompensation SetExposureCompensation;

            public GetExposureCompensation GetExposureCompensation;

            public SetWhitePointRGB SetWhitePointRGB;

            public GetWhitePointRGB GetWhitePointRGB;

            public SetNamedWhitePoint SetNamedWhitePoint;

            public GetNamedWhitePoint GetNamedWhitePoint;

            public SetWhitePointKelvin SetWhitePointKelvin;

            public GetWhitePointKelvin GetWhitePointKelvin;

            public GetKelvinRangeInfo GetKelvinRangeInfo;

            public SetContrast SetContrast;

            public GetContrast GetContrast;

            public SetGamma SetGamma;

            public GetGamma GetGamma;

            public SetSharpness SetSharpness;

            public GetSharpness GetSharpness;

            public SetSaturation SetSaturation;

            public GetSaturation GetSaturation;

            public SetTint SetTint;

            public GetTint GetTint;

            public SetNoiseReduction SetNoiseReduction;

            public GetNoiseReduction GetNoiseReduction;

            public SetDestinationColorContext SetDestinationColorContext;

            public SetToneCurve SetToneCurve;

            public GetToneCurve GetToneCurve;

            public SetRotation SetRotation;

            public GetRotation GetRotation;

            public SetRenderMode SetRenderMode;

            public GetRenderMode GetRenderMode;

            public SetNotificationCallback SetNotificationCallback;
            #endregion
        }
        #endregion
    }
}
