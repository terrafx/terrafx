// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

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
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryRawCapabilitiesInfo(
            [In] IWICDevelopRaw* This,
            [In, Out] WICRawCapabilitiesInfo* pInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int LoadParameterSet(
            [In] IWICDevelopRaw* This,
            [In] WICRawParameterSet ParameterSet
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetCurrentParameterSet(
            [In] IWICDevelopRaw* This,
            [Out] IPropertyBag2** ppCurrentParameterSet = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetExposureCompensation(
            [In] IWICDevelopRaw* This,
            [In] double ev
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetExposureCompensation(
            [In] IWICDevelopRaw* This,
            [Out] double* pEV
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetWhitePointRGB(
            [In] IWICDevelopRaw* This,
            [In, ComAliasName("UINT")] uint Red,
            [In, ComAliasName("UINT")] uint Green,
            [In, ComAliasName("UINT")] uint Blue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetWhitePointRGB(
            [In] IWICDevelopRaw* This,
            [Out, ComAliasName("UINT")] uint* pRed,
            [Out, ComAliasName("UINT")] uint* pGreen,
            [Out, ComAliasName("UINT")] uint* pBlue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetNamedWhitePoint(
            [In] IWICDevelopRaw* This,
            [In] WICNamedWhitePoint WhitePoint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetNamedWhitePoint(
            [In] IWICDevelopRaw* This,
            [Out] WICNamedWhitePoint* pWhitePoint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetWhitePointKelvin(
            [In] IWICDevelopRaw* This,
            [In, ComAliasName("UINT")] uint WhitePointKelvin
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetWhitePointKelvin(
            [In] IWICDevelopRaw* This,
            [Out, ComAliasName("UINT")] uint* pWhitePointKelvin
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetKelvinRangeInfo(
            [In] IWICDevelopRaw* This,
            [Out, ComAliasName("UINT")] uint* pMinKelvinTemp,
            [Out, ComAliasName("UINT")] uint* pMaxKelvinTemp,
            [Out, ComAliasName("UINT")] uint* pKelvinTempStepValue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetContrast(
            [In] IWICDevelopRaw* This,
            [In] double Contrast
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetContrast(
            [In] IWICDevelopRaw* This,
            [Out] double* pContrast
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetGamma(
            [In] IWICDevelopRaw* This,
            [In] double Gamma
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGamma(
            [In] IWICDevelopRaw* This,
            [Out] double* pGamma
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetSharpness(
            [In] IWICDevelopRaw* This,
            [In] double Sharpness
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSharpness(
            [In] IWICDevelopRaw* This,
            [Out] double* pSharpness
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetSaturation(
            [In] IWICDevelopRaw* This,
            [In] double Saturation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSaturation(
            [In] IWICDevelopRaw* This,
            [Out] double* pSaturation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetTint(
            [In] IWICDevelopRaw* This,
            [In] double Tint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTint(
            [In] IWICDevelopRaw* This,
            [Out] double* pTint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetNoiseReduction(
            [In] IWICDevelopRaw* This,
            [In] double NoiseReduction
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetNoiseReduction(
            [In] IWICDevelopRaw* This,
            [Out] double* pNoiseReduction
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetDestinationColorContext(
            [In] IWICDevelopRaw* This,
            [In] IWICColorContext* pColorContext = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetToneCurve(
            [In] IWICDevelopRaw* This,
            [In, ComAliasName("UINT")] uint cbToneCurveSize,
            [In] /* readonly */ WICRawToneCurve* pToneCurve
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetToneCurve(
            [In] IWICDevelopRaw* This,
            [In, ComAliasName("UINT")] uint cbToneCurveBufferSize,
            [Out] WICRawToneCurve* pToneCurve = null,
            [In, Out, ComAliasName("UINT")] uint* pcbActualToneCurveBufferSize = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetRotation(
            [In] IWICDevelopRaw* This,
            [In] double Rotation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetRotation(
            [In] IWICDevelopRaw* This,
            [Out] double* pRotation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetRenderMode(
            [In] IWICDevelopRaw* This,
            [In] WICRawRenderMode RenderMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetRenderMode(
            [In] IWICDevelopRaw* This,
            [Out] WICRawRenderMode* pRenderMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetNotificationCallback(
            [In] IWICDevelopRaw* This,
            [In] IWICDevelopRawNotificationCallback* pCallback = null
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICBitmapFrameDecode.Vtbl BaseVtbl;

            public IntPtr QueryRawCapabilitiesInfo;

            public IntPtr LoadParameterSet;

            public IntPtr GetCurrentParameterSet;

            public IntPtr SetExposureCompensation;

            public IntPtr GetExposureCompensation;

            public IntPtr SetWhitePointRGB;

            public IntPtr GetWhitePointRGB;

            public IntPtr SetNamedWhitePoint;

            public IntPtr GetNamedWhitePoint;

            public IntPtr SetWhitePointKelvin;

            public IntPtr GetWhitePointKelvin;

            public IntPtr GetKelvinRangeInfo;

            public IntPtr SetContrast;

            public IntPtr GetContrast;

            public IntPtr SetGamma;

            public IntPtr GetGamma;

            public IntPtr SetSharpness;

            public IntPtr GetSharpness;

            public IntPtr SetSaturation;

            public IntPtr GetSaturation;

            public IntPtr SetTint;

            public IntPtr GetTint;

            public IntPtr SetNoiseReduction;

            public IntPtr GetNoiseReduction;

            public IntPtr SetDestinationColorContext;

            public IntPtr SetToneCurve;

            public IntPtr GetToneCurve;

            public IntPtr SetRotation;

            public IntPtr GetRotation;

            public IntPtr SetRenderMode;

            public IntPtr GetRenderMode;

            public IntPtr SetNotificationCallback;
            #endregion
        }
        #endregion
    }
}
