// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("FBEC5E44-F7BE-4B65-B7F8-C0C81FEF026D")]
    public /* blittable */ unsafe struct IWICDevelopRaw
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICDevelopRaw* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICDevelopRaw* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICDevelopRaw* This
        );
        #endregion

        #region IWICBitmapSource Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSize(
            [In] IWICDevelopRaw* This,
            [Out, ComAliasName("UINT")] uint* puiWidth,
            [Out, ComAliasName("UINT")] uint* puiHeight
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPixelFormat(
            [In] IWICDevelopRaw* This,
            [Out, ComAliasName("WICPixelFormatGUID")] Guid* pPixelFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetResolution(
            [In] IWICDevelopRaw* This,
            [Out] double* pDpiX,
            [Out] double* pDpiY
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyPalette(
            [In] IWICDevelopRaw* This,
            [In] IWICPalette* pIPalette = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyPixels(
            [In] IWICDevelopRaw* This,
            [In, Optional] WICRect* prc,
            [In, ComAliasName("UINT")] uint cbStride,
            [In, ComAliasName("UINT")] uint cbBufferSize,
            [Out, ComAliasName("BYTE[]")] byte* pbBuffer
        );
        #endregion

        #region IWICBitmapFrameDecode Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetMetadataQueryReader(
            [In] IWICDevelopRaw* This,
            [Out] IWICMetadataQueryReader** ppIMetadataQueryReader = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetColorContexts(
            [In] IWICDevelopRaw* This,
            [In, ComAliasName("UINT")] uint cCount,
            [In, Out, Optional, ComAliasName("IWICColorContext*[]")] IWICColorContext** ppIColorContexts,
            [Out, ComAliasName("UINT")] uint* pcActualCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetThumbnail(
            [In] IWICDevelopRaw* This,
            [Out] IWICBitmapSource** ppIThumbnail = null
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryRawCapabilitiesInfo(
            [In] IWICDevelopRaw* This,
            [In, Out] WICRawCapabilitiesInfo* pInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _LoadParameterSet(
            [In] IWICDevelopRaw* This,
            [In] WICRawParameterSet ParameterSet
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetCurrentParameterSet(
            [In] IWICDevelopRaw* This,
            [Out] IPropertyBag2** ppCurrentParameterSet = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetExposureCompensation(
            [In] IWICDevelopRaw* This,
            [In] double ev
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetExposureCompensation(
            [In] IWICDevelopRaw* This,
            [Out] double* pEV
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetWhitePointRGB(
            [In] IWICDevelopRaw* This,
            [In, ComAliasName("UINT")] uint Red,
            [In, ComAliasName("UINT")] uint Green,
            [In, ComAliasName("UINT")] uint Blue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetWhitePointRGB(
            [In] IWICDevelopRaw* This,
            [Out, ComAliasName("UINT")] uint* pRed,
            [Out, ComAliasName("UINT")] uint* pGreen,
            [Out, ComAliasName("UINT")] uint* pBlue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetNamedWhitePoint(
            [In] IWICDevelopRaw* This,
            [In] WICNamedWhitePoint WhitePoint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetNamedWhitePoint(
            [In] IWICDevelopRaw* This,
            [Out] WICNamedWhitePoint* pWhitePoint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetWhitePointKelvin(
            [In] IWICDevelopRaw* This,
            [In, ComAliasName("UINT")] uint WhitePointKelvin
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetWhitePointKelvin(
            [In] IWICDevelopRaw* This,
            [Out, ComAliasName("UINT")] uint* pWhitePointKelvin
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetKelvinRangeInfo(
            [In] IWICDevelopRaw* This,
            [Out, ComAliasName("UINT")] uint* pMinKelvinTemp,
            [Out, ComAliasName("UINT")] uint* pMaxKelvinTemp,
            [Out, ComAliasName("UINT")] uint* pKelvinTempStepValue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetContrast(
            [In] IWICDevelopRaw* This,
            [In] double Contrast
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetContrast(
            [In] IWICDevelopRaw* This,
            [Out] double* pContrast
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetGamma(
            [In] IWICDevelopRaw* This,
            [In] double Gamma
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetGamma(
            [In] IWICDevelopRaw* This,
            [Out] double* pGamma
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetSharpness(
            [In] IWICDevelopRaw* This,
            [In] double Sharpness
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSharpness(
            [In] IWICDevelopRaw* This,
            [Out] double* pSharpness
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetSaturation(
            [In] IWICDevelopRaw* This,
            [In] double Saturation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSaturation(
            [In] IWICDevelopRaw* This,
            [Out] double* pSaturation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetTint(
            [In] IWICDevelopRaw* This,
            [In] double Tint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetTint(
            [In] IWICDevelopRaw* This,
            [Out] double* pTint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetNoiseReduction(
            [In] IWICDevelopRaw* This,
            [In] double NoiseReduction
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetNoiseReduction(
            [In] IWICDevelopRaw* This,
            [Out] double* pNoiseReduction
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetDestinationColorContext(
            [In] IWICDevelopRaw* This,
            [In] IWICColorContext* pColorContext = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetToneCurve(
            [In] IWICDevelopRaw* This,
            [In, ComAliasName("UINT")] uint cbToneCurveSize,
            [In, ComAliasName("WICRawToneCurve[]")] WICRawToneCurve* pToneCurve
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetToneCurve(
            [In] IWICDevelopRaw* This,
            [In, ComAliasName("UINT")] uint cbToneCurveBufferSize,
            [Out, ComAliasName("WICRawToneCurve[]")] WICRawToneCurve* pToneCurve = null,
            [In, Out, ComAliasName("UINT")] uint* pcbActualToneCurveBufferSize = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetRotation(
            [In] IWICDevelopRaw* This,
            [In] double Rotation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetRotation(
            [In] IWICDevelopRaw* This,
            [Out] double* pRotation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetRenderMode(
            [In] IWICDevelopRaw* This,
            [In] WICRawRenderMode RenderMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetRenderMode(
            [In] IWICDevelopRaw* This,
            [Out] WICRawRenderMode* pRenderMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetNotificationCallback(
            [In] IWICDevelopRaw* This,
            [In] IWICDevelopRawNotificationCallback* pCallback = null
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_QueryInterface>(lpVtbl->QueryInterface)(
                    This,
                    riid,
                    ppvObject
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint AddRef()
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region IWICBitmapSource Methods
        [return: ComAliasName("HRESULT")]
        public int GetSize(
            [Out, ComAliasName("UINT")] uint* puiWidth,
            [Out, ComAliasName("UINT")] uint* puiHeight
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetSize>(lpVtbl->GetSize)(
                    This,
                    puiWidth,
                    puiHeight
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetPixelFormat(
            [Out, ComAliasName("WICPixelFormatGUID")] Guid* pPixelFormat
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetPixelFormat>(lpVtbl->GetPixelFormat)(
                    This,
                    pPixelFormat
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetResolution(
            [Out] double* pDpiX,
            [Out] double* pDpiY
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetResolution>(lpVtbl->GetResolution)(
                    This,
                    pDpiX,
                    pDpiY
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyPalette(
            [In] IWICPalette* pIPalette = null
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_CopyPalette>(lpVtbl->CopyPalette)(
                    This,
                    pIPalette
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyPixels(
            [In, Optional] WICRect* prc,
            [In, ComAliasName("UINT")] uint cbStride,
            [In, ComAliasName("UINT")] uint cbBufferSize,
            [Out, ComAliasName("BYTE[]")] byte* pbBuffer
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_CopyPixels>(lpVtbl->CopyPixels)(
                    This,
                    prc,
                    cbStride,
                    cbBufferSize,
                    pbBuffer
                );
            }
        }
        #endregion

        #region IWICBitmapFrameDecode Methods
        [return: ComAliasName("HRESULT")]
        public int GetMetadataQueryReader(
            [Out] IWICMetadataQueryReader** ppIMetadataQueryReader = null
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetMetadataQueryReader>(lpVtbl->GetMetadataQueryReader)(
                    This,
                    ppIMetadataQueryReader
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetColorContexts(
            [In, ComAliasName("UINT")] uint cCount,
            [In, Out, Optional, ComAliasName("IWICColorContext*[]")] IWICColorContext** ppIColorContexts,
            [Out, ComAliasName("UINT")] uint* pcActualCount
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetColorContexts>(lpVtbl->GetColorContexts)(
                    This,
                    cCount,
                    ppIColorContexts,
                    pcActualCount
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetThumbnail(
            [Out] IWICBitmapSource** ppIThumbnail = null
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetThumbnail>(lpVtbl->GetThumbnail)(
                    This,
                    ppIThumbnail
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int QueryRawCapabilitiesInfo(
            [In, Out] WICRawCapabilitiesInfo* pInfo
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_QueryRawCapabilitiesInfo>(lpVtbl->QueryRawCapabilitiesInfo)(
                    This,
                    pInfo
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int LoadParameterSet(
            [In] WICRawParameterSet ParameterSet
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_LoadParameterSet>(lpVtbl->LoadParameterSet)(
                    This,
                    ParameterSet
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetCurrentParameterSet(
            [Out] IPropertyBag2** ppCurrentParameterSet = null
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetCurrentParameterSet>(lpVtbl->GetCurrentParameterSet)(
                    This,
                    ppCurrentParameterSet
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetExposureCompensation(
            [In] double ev
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetExposureCompensation>(lpVtbl->SetExposureCompensation)(
                    This,
                    ev
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetExposureCompensation(
            [Out] double* pEV
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetExposureCompensation>(lpVtbl->GetExposureCompensation)(
                    This,
                    pEV
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetWhitePointRGB(
            [In, ComAliasName("UINT")] uint Red,
            [In, ComAliasName("UINT")] uint Green,
            [In, ComAliasName("UINT")] uint Blue
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetWhitePointRGB>(lpVtbl->SetWhitePointRGB)(
                    This,
                    Red,
                    Green,
                    Blue
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetWhitePointRGB(
            [Out, ComAliasName("UINT")] uint* pRed,
            [Out, ComAliasName("UINT")] uint* pGreen,
            [Out, ComAliasName("UINT")] uint* pBlue
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetWhitePointRGB>(lpVtbl->GetWhitePointRGB)(
                    This,
                    pRed,
                    pGreen,
                    pBlue
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetNamedWhitePoint(
            [In] WICNamedWhitePoint WhitePoint
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetNamedWhitePoint>(lpVtbl->SetNamedWhitePoint)(
                    This,
                    WhitePoint
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetNamedWhitePoint(
            [Out] WICNamedWhitePoint* pWhitePoint
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetNamedWhitePoint>(lpVtbl->GetNamedWhitePoint)(
                    This,
                    pWhitePoint
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetWhitePointKelvin(
            [In, ComAliasName("UINT")] uint WhitePointKelvin
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetWhitePointKelvin>(lpVtbl->SetWhitePointKelvin)(
                    This,
                    WhitePointKelvin
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetWhitePointKelvin(
            [Out, ComAliasName("UINT")] uint* pWhitePointKelvin
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetWhitePointKelvin>(lpVtbl->GetWhitePointKelvin)(
                    This,
                    pWhitePointKelvin
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetKelvinRangeInfo(
            [Out, ComAliasName("UINT")] uint* pMinKelvinTemp,
            [Out, ComAliasName("UINT")] uint* pMaxKelvinTemp,
            [Out, ComAliasName("UINT")] uint* pKelvinTempStepValue
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetKelvinRangeInfo>(lpVtbl->GetKelvinRangeInfo)(
                    This,
                    pMinKelvinTemp,
                    pMaxKelvinTemp,
                    pKelvinTempStepValue
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetContrast(
            [In] double Contrast
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetContrast>(lpVtbl->SetContrast)(
                    This,
                    Contrast
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetContrast(
            [Out] double* pContrast
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetContrast>(lpVtbl->GetContrast)(
                    This,
                    pContrast
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetGamma(
            [In] double Gamma
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetGamma>(lpVtbl->SetGamma)(
                    This,
                    Gamma
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetGamma(
            [Out] double* pGamma
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetGamma>(lpVtbl->GetGamma)(
                    This,
                    pGamma
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetSharpness(
            [In] double Sharpness
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetSharpness>(lpVtbl->SetSharpness)(
                    This,
                    Sharpness
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetSharpness(
            [Out] double* pSharpness
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetSharpness>(lpVtbl->GetSharpness)(
                    This,
                    pSharpness
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetSaturation(
            [In] double Saturation
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetSaturation>(lpVtbl->SetSaturation)(
                    This,
                    Saturation
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetSaturation(
            [Out] double* pSaturation
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetSaturation>(lpVtbl->GetSaturation)(
                    This,
                    pSaturation
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetTint(
            [In] double Tint
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetTint>(lpVtbl->SetTint)(
                    This,
                    Tint
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetTint(
            [Out] double* pTint
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetTint>(lpVtbl->GetTint)(
                    This,
                    pTint
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetNoiseReduction(
            [In] double NoiseReduction
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetNoiseReduction>(lpVtbl->SetNoiseReduction)(
                    This,
                    NoiseReduction
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetNoiseReduction(
            [Out] double* pNoiseReduction
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetNoiseReduction>(lpVtbl->GetNoiseReduction)(
                    This,
                    pNoiseReduction
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetDestinationColorContext(
            [In] IWICColorContext* pColorContext = null
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetDestinationColorContext>(lpVtbl->SetDestinationColorContext)(
                    This,
                    pColorContext
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetToneCurve(
            [In, ComAliasName("UINT")] uint cbToneCurveSize,
            [In, ComAliasName("WICRawToneCurve[]")] WICRawToneCurve* pToneCurve
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetToneCurve>(lpVtbl->SetToneCurve)(
                    This,
                    cbToneCurveSize,
                    pToneCurve
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetToneCurve(
            [In, ComAliasName("UINT")] uint cbToneCurveBufferSize,
            [Out, ComAliasName("WICRawToneCurve[]")] WICRawToneCurve* pToneCurve = null,
            [In, Out, ComAliasName("UINT")] uint* pcbActualToneCurveBufferSize = null
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetToneCurve>(lpVtbl->GetToneCurve)(
                    This,
                    cbToneCurveBufferSize,
                    pToneCurve,
                    pcbActualToneCurveBufferSize
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetRotation(
            [In] double Rotation
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetRotation>(lpVtbl->SetRotation)(
                    This,
                    Rotation
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetRotation(
            [Out] double* pRotation
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetRotation>(lpVtbl->GetRotation)(
                    This,
                    pRotation
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetRenderMode(
            [In] WICRawRenderMode RenderMode
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetRenderMode>(lpVtbl->SetRenderMode)(
                    This,
                    RenderMode
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetRenderMode(
            [Out] WICRawRenderMode* pRenderMode
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_GetRenderMode>(lpVtbl->GetRenderMode)(
                    This,
                    pRenderMode
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetNotificationCallback(
            [In] IWICDevelopRawNotificationCallback* pCallback = null
        )
        {
            fixed (IWICDevelopRaw* This = &this)
            {
                return MarshalFunction<_SetNotificationCallback>(lpVtbl->SetNotificationCallback)(
                    This,
                    pCallback
                );
            }
        }
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region IWICBitmapSource Fields
            public IntPtr GetSize;

            public IntPtr GetPixelFormat;

            public IntPtr GetResolution;

            public IntPtr CopyPalette;

            public IntPtr CopyPixels;
            #endregion

            #region IWICBitmapFrameDecode Fields
            public IntPtr GetMetadataQueryReader;

            public IntPtr GetColorContexts;

            public IntPtr GetThumbnail;
            #endregion

            #region Fields
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

