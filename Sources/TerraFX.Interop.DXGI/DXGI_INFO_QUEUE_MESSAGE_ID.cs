// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.DXGI
{
    public enum DXGI_INFO_QUEUE_MESSAGE_ID
    {
        STRING_FROM_APPLICATION = 0,

        IDXGISwapChain_CreationOrResizeBuffers_InvalidOutputWindow = 0,

        IDXGISwapChain_CreationOrResizeBuffers_BufferWidthInferred = 1,

        IDXGISwapChain_CreationOrResizeBuffers_BufferHeightInferred = 2,

        IDXGISwapChain_CreationOrResizeBuffers_NoScanoutFlagChanged = 3,

        IDXGISwapChain_Creation_MaxBufferCountExceeded = 4,

        IDXGISwapChain_Creation_TooFewBuffers = 5,

        IDXGISwapChain_Creation_NoOutputWindow = 6,

        IDXGISwapChain_Destruction_OtherMethodsCalled = 7,

        IDXGISwapChain_GetDesc_pDescIsNULL = 8,

        IDXGISwapChain_GetBuffer_ppSurfaceIsNULL = 9,

        IDXGISwapChain_GetBuffer_NoAllocatedBuffers = 10,

        IDXGISwapChain_GetBuffer_iBufferMustBeZero = 11,

        IDXGISwapChain_GetBuffer_iBufferOOB = 12,

        IDXGISwapChain_GetContainingOutput_ppOutputIsNULL = 13,

        IDXGISwapChain_Present_SyncIntervalOOB = 14,

        IDXGISwapChain_Present_InvalidNonPreRotatedFlag = 15,

        IDXGISwapChain_Present_NoAllocatedBuffers = 16,

        IDXGISwapChain_Present_GetDXGIAdapterFailed = 17,

        IDXGISwapChain_ResizeBuffers_BufferCountOOB = 18,

        IDXGISwapChain_ResizeBuffers_UnreleasedReferences = 19,

        IDXGISwapChain_ResizeBuffers_InvalidSwapChainFlag = 20,

        IDXGISwapChain_ResizeBuffers_InvalidNonPreRotatedFlag = 21,

        IDXGISwapChain_ResizeTarget_RefreshRateDivideByZero = 22,

        IDXGISwapChain_SetFullscreenState_InvalidTarget = 23,

        IDXGISwapChain_GetFrameStatistics_pStatsIsNULL = 24,

        IDXGISwapChain_GetLastPresentCount_pLastPresentCountIsNULL = 25,

        IDXGISwapChain_SetFullscreenState_RemoteNotSupported = 26,

        IDXGIOutput_TakeOwnership_FailedToAcquireFullscreenMutex = 27,

        IDXGIFactory_CreateSoftwareAdapter_ppAdapterInterfaceIsNULL = 28,

        IDXGIFactory_EnumAdapters_ppAdapterInterfaceIsNULL = 29,

        IDXGIFactory_CreateSwapChain_ppSwapChainIsNULL = 30,

        IDXGIFactory_CreateSwapChain_pDescIsNULL = 31,

        IDXGIFactory_CreateSwapChain_UnknownSwapEffect = 32,

        IDXGIFactory_CreateSwapChain_InvalidFlags = 33,

        IDXGIFactory_CreateSwapChain_NonPreRotatedFlagAndWindowed = 34,

        IDXGIFactory_CreateSwapChain_NullDeviceInterface = 35,

        IDXGIFactory_GetWindowAssociation_phWndIsNULL = 36,

        IDXGIFactory_MakeWindowAssociation_InvalidFlags = 37,

        IDXGISurface_Map_InvalidSurface = 38,

        IDXGISurface_Map_FlagsSetToZero = 39,

        IDXGISurface_Map_DiscardAndReadFlagSet = 40,

        IDXGISurface_Map_DiscardButNotWriteFlagSet = 41,

        IDXGISurface_Map_NoCPUAccess = 42,

        IDXGISurface_Map_ReadFlagSetButCPUAccessIsDynamic = 43,

        IDXGISurface_Map_DiscardFlagSetButCPUAccessIsNotDynamic = 44,

        IDXGIOutput_GetDisplayModeList_pNumModesIsNULL = 45,

        IDXGIOutput_FindClosestMatchingMode_ModeHasInvalidWidthOrHeight = 46,

        IDXGIOutput_GetCammaControlCapabilities_NoOwnerDevice = 47,

        IDXGIOutput_TakeOwnership_pDeviceIsNULL = 48,

        IDXGIOutput_GetDisplaySurfaceData_NoOwnerDevice = 49,

        IDXGIOutput_GetDisplaySurfaceData_pDestinationIsNULL = 50,

        IDXGIOutput_GetDisplaySurfaceData_MapOfDestinationFailed = 51,

        IDXGIOutput_GetFrameStatistics_NoOwnerDevice = 52,

        IDXGIOutput_GetFrameStatistics_pStatsIsNULL = 53,

        IDXGIOutput_SetGammaControl_NoOwnerDevice = 54,

        IDXGIOutput_GetGammaControl_NoOwnerDevice = 55,

        IDXGIOutput_GetGammaControl_NoGammaControls = 56,

        IDXGIOutput_SetDisplaySurface_IDXGIResourceNotSupportedBypPrimary = 57,

        IDXGIOutput_SetDisplaySurface_pPrimaryIsInvalid = 58,

        IDXGIOutput_SetDisplaySurface_NoOwnerDevice = 59,

        IDXGIOutput_TakeOwnership_RemoteDeviceNotSupported = 60,

        IDXGIOutput_GetDisplayModeList_RemoteDeviceNotSupported = 61,

        IDXGIOutput_FindClosestMatchingMode_RemoteDeviceNotSupported = 62,

        IDXGIDevice_CreateSurface_InvalidParametersWithpSharedResource = 63,

        IDXGIObject_GetPrivateData_puiDataSizeIsNULL = 64,

        IDXGISwapChain_Creation_InvalidOutputWindow = 65,

        IDXGISwapChain_Release_SwapChainIsFullscreen = 66,

        IDXGIOutput_GetDisplaySurfaceData_InvalidTargetSurfaceFormat = 67,

        IDXGIFactory_CreateSoftwareAdapter_ModuleIsNULL = 68,

        IDXGIOutput_FindClosestMatchingMode_IDXGIDeviceNotSupportedBypConcernedDevice = 69,

        IDXGIOutput_FindClosestMatchingMode_pModeToMatchOrpClosestMatchIsNULL = 70,

        IDXGIOutput_FindClosestMatchingMode_ModeHasRefreshRateDenominatorZero = 71,

        IDXGIOutput_FindClosestMatchingMode_UnknownFormatIsInvalidForConfiguration = 72,

        IDXGIOutput_FindClosestMatchingMode_InvalidDisplayModeScanlineOrdering = 73,

        IDXGIOutput_FindClosestMatchingMode_InvalidDisplayModeScaling = 74,

        IDXGIOutput_FindClosestMatchingMode_InvalidDisplayModeFormatAndDeviceCombination = 75,

        IDXGIFactory_Creation_CalledFromDllMain = 76,

        IDXGISwapChain_SetFullscreenState_OutputNotOwnedBySwapChainDevice = 77,

        IDXGISwapChain_Creation_InvalidWindowStyle = 78,

        IDXGISwapChain_GetFrameStatistics_UnsupportedStatistics = 79,

        IDXGISwapChain_GetContainingOutput_SwapchainAdapterDoesNotControlOutput = 80,

        IDXGIOutput_SetOrGetGammaControl_pArrayIsNULL = 81,

        IDXGISwapChain_SetFullscreenState_FullscreenInvalidForChildWindows = 82,

        IDXGIFactory_Release_CalledFromDllMain = 83,

        IDXGISwapChain_Present_UnreleasedHDC = 84,

        IDXGISwapChain_ResizeBuffers_NonPreRotatedAndGDICompatibleFlags = 85,

        IDXGIFactory_CreateSwapChain_NonPreRotatedAndGDICompatibleFlags = 86,

        IDXGISurface1_GetDC_pHdcIsNULL = 87,

        IDXGISurface1_GetDC_SurfaceNotTexture2D = 88,

        IDXGISurface1_GetDC_GDICompatibleFlagNotSet = 89,

        IDXGISurface1_GetDC_UnreleasedHDC = 90,

        IDXGISurface_Map_NoCPUAccess2 = 91,

        IDXGISurface1_ReleaseDC_GetDCNotCalled = 92,

        IDXGISurface1_ReleaseDC_InvalidRectangleDimensions = 93,

        IDXGIOutput_TakeOwnership_RemoteOutputNotSupported = 94,

        IDXGIOutput_FindClosestMatchingMode_RemoteOutputNotSupported = 95,

        IDXGIOutput_GetDisplayModeList_RemoteOutputNotSupported = 96,

        IDXGIFactory_CreateSwapChain_pDeviceHasMismatchedDXGIFactory = 97,

        IDXGISwapChain_Present_NonOptimalFSConfiguration = 98,

        IDXGIFactory_CreateSwapChain_FlipSequentialNotSupportedOnD3D10 = 99,

        IDXGIFactory_CreateSwapChain_BufferCountOOBForFlipSequential = 100,

        IDXGIFactory_CreateSwapChain_InvalidFormatForFlipSequential = 101,

        IDXGIFactory_CreateSwapChain_MultiSamplingNotSupportedForFlipSequential = 102,

        IDXGISwapChain_ResizeBuffers_BufferCountOOBForFlipSequential = 103,

        IDXGISwapChain_ResizeBuffers_InvalidFormatForFlipSequential = 104,

        IDXGISwapChain_Present_PartialPresentationBeforeStandardPresentation = 105,

        IDXGISwapChain_Present_FullscreenPartialPresentIsInvalid = 106,

        IDXGISwapChain_Present_InvalidPresentTestOrDoNotSequenceFlag = 107,

        IDXGISwapChain_Present_ScrollInfoWithNoDirtyRectsSpecified = 108,

        IDXGISwapChain_Present_EmptyScrollRect = 109,

        IDXGISwapChain_Present_ScrollRectOutOfBackbufferBounds = 110,

        IDXGISwapChain_Present_ScrollRectOutOfBackbufferBoundsWithOffset = 111,

        IDXGISwapChain_Present_EmptyDirtyRect = 112,

        IDXGISwapChain_Present_DirtyRectOutOfBackbufferBounds = 113,

        IDXGIFactory_CreateSwapChain_UnsupportedBufferUsageFlags = 114,

        IDXGISwapChain_Present_DoNotSequenceFlagSetButPreviousBufferIsUndefined = 115,

        IDXGISwapChain_Present_UnsupportedFlags = 116,

        IDXGISwapChain_Present_FlipModelChainMustResizeOrCreateOnFSTransition = 117,

        IDXGIFactory_CreateSwapChain_pRestrictToOutputFromOtherIDXGIFactory = 118,

        IDXGIFactory_CreateSwapChain_RestrictOutputNotSupportedOnAdapter = 119,

        IDXGISwapChain_Present_RestrictToOutputFlagSetButInvalidpRestrictToOutput = 120,

        IDXGISwapChain_Present_RestrictToOutputFlagdWithFullscreen = 121,

        IDXGISwapChain_Present_RestrictOutputFlagWithStaleSwapChain = 122,

        IDXGISwapChain_Present_OtherFlagsCausingInvalidPresentTestFlag = 123,

        IDXGIFactory_CreateSwapChain_UnavailableInSession0 = 124,

        IDXGIFactory_MakeWindowAssociation_UnavailableInSession0 = 125,

        IDXGIFactory_GetWindowAssociation_UnavailableInSession0 = 126,

        IDXGIAdapter_EnumOutputs_UnavailableInSession0 = 127,

        IDXGISwapChain_CreationOrSetFullscreenState_StereoDisabled = 128,

        IDXGIFactory2_UnregisterStatus_CookieNotFound = 129,

        IDXGISwapChain_Present_ProtectedContentInWindowedModeWithoutFSOrOverlay = 130,

        IDXGISwapChain_Present_ProtectedContentInWindowedModeWithoutFlipSequential = 131,

        IDXGISwapChain_Present_ProtectedContentWithRDPDriver = 132,

        IDXGISwapChain_Present_ProtectedContentInWindowedModeWithDWMOffOrInvalidDisplayAffinity = 133,

        IDXGIFactory_CreateSwapChainForComposition_WidthOrHeightIsZero = 134,

        IDXGIFactory_CreateSwapChainForComposition_OnlyFlipSequentialSupported = 135,

        IDXGIFactory_CreateSwapChainForComposition_UnsupportedOnAdapter = 136,

        IDXGIFactory_CreateSwapChainForComposition_UnsupportedOnWindows7 = 137,

        IDXGISwapChain_SetFullscreenState_FSTransitionWithCompositionSwapChain = 138,

        IDXGISwapChain_ResizeTarget_InvalidWithCompositionSwapChain = 139,

        IDXGISwapChain_ResizeBuffers_WidthOrHeightIsZero = 140,

        IDXGIFactory_CreateSwapChain_ScalingNoneIsFlipModelOnly = 141,

        IDXGIFactory_CreateSwapChain_ScalingUnrecognized = 142,

        IDXGIFactory_CreateSwapChain_DisplayOnlyFullscreenUnsupported = 143,

        IDXGIFactory_CreateSwapChain_DisplayOnlyUnsupported = 144,

        IDXGISwapChain_Present_RestartIsFullscreenOnly = 145,

        IDXGISwapChain_Present_ProtectedWindowlessPresentationRequiresDisplayOnly = 146,

        IDXGISwapChain_SetFullscreenState_DisplayOnlyUnsupported = 147,

        IDXGISwapChain1_SetBackgroundColor_OutOfRange = 148,

        IDXGISwapChain_ResizeBuffers_DisplayOnlyFullscreenUnsupported = 149,

        IDXGISwapChain_ResizeBuffers_DisplayOnlyUnsupported = 150,

        IDXGISwapchain_Present_ScrollUnsupported = 151,

        IDXGISwapChain1_SetRotation_UnsupportedOS = 152,

        IDXGISwapChain1_GetRotation_UnsupportedOS = 153,

        IDXGISwapchain_Present_FullscreenRotation = 154,

        IDXGISwapChain_Present_PartialPresentationWithMSAABuffers = 155,

        IDXGISwapChain1_SetRotation_FlipSequentialRequired = 156,

        IDXGISwapChain1_SetRotation_InvalidRotation = 157,

        IDXGISwapChain1_GetRotation_FlipSequentialRequired = 158,

        IDXGISwapChain_GetHwnd_WrongType = 159,

        IDXGISwapChain_GetCompositionSurface_WrongType = 160,

        IDXGISwapChain_GetCoreWindow_WrongType = 161,

        IDXGISwapChain_GetFullscreenDesc_NonHwnd = 162,

        IDXGISwapChain_SetFullscreenState_CoreWindow = 163,

        IDXGIFactory2_CreateSwapChainForCoreWindow_UnsupportedOnWindows7 = 164,

        IDXGIFactory2_CreateSwapChainForCoreWindow_pWindowIsNULL = 165,

        IDXGIFactory_CreateSwapChain_FSUnsupportedForModernApps = 166,

        IDXGIFactory_MakeWindowAssociation_ModernApp = 167,

        IDXGISwapChain_ResizeTarget_ModernApp = 168,

        IDXGISwapChain_ResizeTarget_pNewTargetParametersIsNULL = 169,

        IDXGIOutput_SetDisplaySurface_ModernApp = 170,

        IDXGIOutput_TakeOwnership_ModernApp = 171,

        IDXGIFactory2_CreateSwapChainForCoreWindow_pWindowIsInvalid = 172,

        IDXGIFactory2_CreateSwapChainForCompositionSurface_InvalidHandle = 173,

        IDXGISurface1_GetDC_ModernApp = 174,

        IDXGIFactory_CreateSwapChain_ScalingNoneRequiresWindows8OrNewer = 175,

        IDXGISwapChain_Present_TemporaryMonoAndPreferRight = 176,

        IDXGISwapChain_Present_TemporaryMonoOrPreferRightWithDoNotSequence = 177,

        IDXGISwapChain_Present_TemporaryMonoOrPreferRightWithoutStereo = 178,

        IDXGISwapChain_Present_TemporaryMonoUnsupported = 179,

        IDXGIOutput_GetDisplaySurfaceData_ArraySizeMismatch = 180,

        IDXGISwapChain_Present_PartialPresentationWithSwapEffectDiscard = 181,

        IDXGIFactory_CreateSwapChain_AlphaUnrecognized = 182,

        IDXGIFactory_CreateSwapChain_AlphaIsWindowlessOnly = 183,

        IDXGIFactory_CreateSwapChain_AlphaIsFlipModelOnly = 184,

        IDXGIFactory_CreateSwapChain_RestrictToOutputAdapterMismatch = 185,

        IDXGIFactory_CreateSwapChain_DisplayOnlyOnLegacy = 186,

        IDXGISwapChain_ResizeBuffers_DisplayOnlyOnLegacy = 187,

        IDXGIResource1_CreateSubresourceSurface_InvalidIndex = 188,

        IDXGIFactory_CreateSwapChainForComposition_InvalidScaling = 189,

        IDXGIFactory_CreateSwapChainForCoreWindow_InvalidSwapEffect = 190,

        IDXGIResource1_CreateSharedHandle_UnsupportedOS = 191,

        IDXGIFactory2_RegisterOcclusionStatusWindow_UnsupportedOS = 192,

        IDXGIFactory2_RegisterOcclusionStatusEvent_UnsupportedOS = 193,

        IDXGIOutput1_DuplicateOutput_UnsupportedOS = 194,

        IDXGIDisplayControl_IsStereoEnabled_UnsupportedOS = 195,

        IDXGIFactory_CreateSwapChainForComposition_InvalidAlphaMode = 196,

        IDXGIFactory_GetSharedResourceAdapterLuid_InvalidResource = 197,

        IDXGIFactory_GetSharedResourceAdapterLuid_InvalidLUID = 198,

        IDXGIFactory_GetSharedResourceAdapterLuid_UnsupportedOS = 199,

        IDXGIOutput1_GetDisplaySurfaceData1_2DOnly = 200,

        IDXGIOutput1_GetDisplaySurfaceData1_StagingOnly = 201,

        IDXGIOutput1_GetDisplaySurfaceData1_NeedCPUAccessWrite = 202,

        IDXGIOutput1_GetDisplaySurfaceData1_NoShared = 203,

        IDXGIOutput1_GetDisplaySurfaceData1_OnlyMipLevels1 = 204,

        IDXGIOutput1_GetDisplaySurfaceData1_MappedOrOfferedResource = 205,

        IDXGISwapChain_SetFullscreenState_FSUnsupportedForModernApps = 206,

        IDXGIFactory_CreateSwapChain_FailedToGoFSButNonPreRotated = 207,

        IDXGIFactory_CreateSwapChainOrRegisterOcclusionStatus_BlitModelUsedWhileRegisteredForOcclusionStatusEvents = 208,

        IDXGISwapChain_Present_BlitModelUsedWhileRegisteredForOcclusionStatusEvents = 209,

        IDXGIFactory_CreateSwapChain_WaitableSwapChainsAreFlipModelOnly = 210,

        IDXGIFactory_CreateSwapChain_WaitableSwapChainsAreNotFullscreen = 211,

        IDXGISwapChain_SetFullscreenState_Waitable = 212,

        IDXGISwapChain_ResizeBuffers_CannotAddOrRemoveWaitableFlag = 213,

        IDXGISwapChain_GetFrameLatencyWaitableObject_OnlyWaitable = 214,

        IDXGISwapChain_GetMaximumFrameLatency_OnlyWaitable = 215,

        IDXGISwapChain_GetMaximumFrameLatency_pMaxLatencyIsNULL = 216,

        IDXGISwapChain_SetMaximumFrameLatency_OnlyWaitable = 217,

        IDXGISwapChain_SetMaximumFrameLatency_MaxLatencyIsOutOfBounds = 218,

        IDXGIFactory_CreateSwapChain_ForegroundIsCoreWindowOnly = 219,

        IDXGIFactory2_CreateSwapChainForCoreWindow_ForegroundUnsupportedOnAdapter = 220,

        IDXGIFactory2_CreateSwapChainForCoreWindow_InvalidScaling = 221,

        IDXGIFactory2_CreateSwapChainForCoreWindow_InvalidAlphaMode = 222,

        IDXGISwapChain_ResizeBuffers_CannotAddOrRemoveForegroundFlag = 223,

        IDXGISwapChain_SetMatrixTransform_MatrixPointerCannotBeNull = 224,

        IDXGISwapChain_SetMatrixTransform_RequiresCompositionSwapChain = 225,

        IDXGISwapChain_SetMatrixTransform_MatrixMustBeFinite = 226,

        IDXGISwapChain_SetMatrixTransform_MatrixMustBeTranslateAndOrScale = 227,

        IDXGISwapChain_GetMatrixTransform_MatrixPointerCannotBeNull = 228,

        IDXGISwapChain_GetMatrixTransform_RequiresCompositionSwapChain = 229,

        DXGIGetDebugInterface1_NULL_ppDebug = 230,

        DXGIGetDebugInterface1_InvalidFlags = 231,

        IDXGISwapChain_Present_Decode = 232,

        IDXGISwapChain_ResizeBuffers_Decode = 233,

        IDXGISwapChain_SetSourceSize_FlipModel = 234,

        IDXGISwapChain_SetSourceSize_Decode = 235,

        IDXGISwapChain_SetSourceSize_WidthHeight = 236,

        IDXGISwapChain_GetSourceSize_NullPointers = 237,

        IDXGISwapChain_GetSourceSize_Decode = 238,

        IDXGIDecodeSwapChain_SetColorSpace_InvalidFlags = 239,

        IDXGIDecodeSwapChain_SetSourceRect_InvalidRect = 240,

        IDXGIDecodeSwapChain_SetTargetRect_InvalidRect = 241,

        IDXGIDecodeSwapChain_SetDestSize_InvalidSize = 242,

        IDXGIDecodeSwapChain_GetSourceRect_InvalidPointer = 243,

        IDXGIDecodeSwapChain_GetTargetRect_InvalidPointer = 244,

        IDXGIDecodeSwapChain_GetDestSize_InvalidPointer = 245,

        IDXGISwapChain_PresentBuffer_YUV = 246,

        IDXGISwapChain_SetSourceSize_YUV = 247,

        IDXGISwapChain_GetSourceSize_YUV = 248,

        IDXGISwapChain_SetMatrixTransform_YUV = 249,

        IDXGISwapChain_GetMatrixTransform_YUV = 250,

        IDXGISwapChain_Present_PartialPresentation_YUV = 251,

        IDXGISwapChain_ResizeBuffers_CannotAddOrRemoveFlag_YUV = 252,

        IDXGISwapChain_ResizeBuffers_Alignment_YUV = 253,

        IDXGIFactory_CreateSwapChain_ShaderInputUnsupported_YUV = 254,

        IDXGIOutput3_CheckOverlaySupport_NullPointers = 255,

        IDXGIOutput3_CheckOverlaySupport_IDXGIDeviceNotSupportedBypConcernedDevice = 256,

        IDXGIAdapter_EnumOutputs2_InvalidEnumOutputs2Flag = 257,

        IDXGISwapChain_CreationOrSetFullscreenState_FSUnsupportedForFlipDiscard = 258,

        IDXGIOutput4_CheckOverlayColorSpaceSupport_NullPointers = 259,

        IDXGIOutput4_CheckOverlayColorSpaceSupport_IDXGIDeviceNotSupportedBypConcernedDevice = 260,

        IDXGISwapChain3_CheckColorSpaceSupport_NullPointers = 261,

        IDXGISwapChain3_SetColorSpace1_InvalidColorSpace = 262,

        IDXGIFactory_CreateSwapChain_InvalidHwProtect = 263,

        IDXGIFactory_CreateSwapChain_HwProtectUnsupported = 264,

        IDXGISwapChain_ResizeBuffers_InvalidHwProtect = 265,

        IDXGISwapChain_ResizeBuffers_HwProtectUnsupported = 266,

        IDXGISwapChain_ResizeBuffers1_D3D12Only = 267,

        IDXGISwapChain_ResizeBuffers1_FlipModel = 268,

        IDXGISwapChain_ResizeBuffers1_NodeMaskAndQueueRequired = 269,

        IDXGISwapChain_CreateSwapChain_InvalidHwProtectGdiFlag = 270,

        IDXGISwapChain_ResizeBuffers_InvalidHwProtectGdiFlag = 271,

        IDXGIFactory_CreateSwapChain_10BitFormatNotSupported = 272,

        IDXGIFactory_CreateSwapChain_FlipSwapEffectRequired = 273,

        IDXGIFactory_CreateSwapChain_InvalidDevice = 274,

        IDXGIOutput_TakeOwnership_Unsupported = 275,

        IDXGIFactory_CreateSwapChain_InvalidQueue = 276,

        IDXGISwapChain3_ResizeBuffers1_InvalidQueue = 277,

        IDXGIFactory_CreateSwapChainForHwnd_InvalidScaling = 278,

        IDXGISwapChain3_SetHDRMetaData_InvalidSize = 279,

        IDXGISwapChain3_SetHDRMetaData_InvalidPointer = 280,

        IDXGISwapChain3_SetHDRMetaData_InvalidType = 281,

        IDXGISwapChain_Present_FullscreenAllowTearingIsInvalid = 282,

        IDXGISwapChain_Present_AllowTearingRequiresPresentIntervalZero = 283,

        IDXGISwapChain_Present_AllowTearingRequiresCreationFlag = 284,

        IDXGISwapChain_ResizeBuffers_CannotAddOrRemoveAllowTearingFlag = 285,

        IDXGIFactory_CreateSwapChain_AllowTearingFlagIsFlipModelOnly = 286,

        IDXGIFactory_CheckFeatureSupport_InvalidFeature = 287,

        IDXGIFactory_CheckFeatureSupport_InvalidSize = 288,

        IDXGIOutput6_CheckHardwareCompositionSupport_NullPointer = 289,

        IDXGISwapChain_SetFullscreenState_PerMonitorDpiShimApplied = 290,

        IDXGIOutput_DuplicateOutput_PerMonitorDpiShimApplied = 291,

        IDXGIOutput_DuplicateOutput1_PerMonitorDpiRequired = 292,


        Phone_IDXGIFactory_CreateSwapChain_NotForegroundWindow = 1000,

        Phone_IDXGIFactory_CreateSwapChain_DISCARD_BufferCount = 1001,

        Phone_IDXGISwapChain_SetFullscreenState_NotAvailable = 1002,

        Phone_IDXGISwapChain_ResizeBuffers_NotAvailable = 1003,

        Phone_IDXGISwapChain_ResizeTarget_NotAvailable = 1004,

        Phone_IDXGISwapChain_Present_InvalidLayerIndex = 1005,

        Phone_IDXGISwapChain_Present_MultipleLayerIndex = 1006,

        Phone_IDXGISwapChain_Present_InvalidLayerFlag = 1007,

        Phone_IDXGISwapChain_Present_InvalidRotation = 1008,

        Phone_IDXGISwapChain_Present_InvalidBlend = 1009,

        Phone_IDXGISwapChain_Present_InvalidResource = 1010,

        Phone_IDXGISwapChain_Present_InvalidMultiPlaneOverlayResource = 1011,

        Phone_IDXGISwapChain_Present_InvalidIndexForPrimary = 1012,

        Phone_IDXGISwapChain_Present_InvalidIndexForOverlay = 1013,

        Phone_IDXGISwapChain_Present_InvalidSubResourceIndex = 1014,

        Phone_IDXGISwapChain_Present_InvalidSourceRect = 1015,

        Phone_IDXGISwapChain_Present_InvalidDestinationRect = 1016,

        Phone_IDXGISwapChain_Present_MultipleResource = 1017,

        Phone_IDXGISwapChain_Present_NotSharedResource = 1018,

        Phone_IDXGISwapChain_Present_InvalidFlag = 1019,

        Phone_IDXGISwapChain_Present_InvalidInterval = 1020,

        Phone_IDXGIFactory_CreateSwapChain_MSAA_NotSupported = 1021,

        Phone_IDXGIFactory_CreateSwapChain_ScalingAspectRatioStretch_Supported_ModernApp = 1022,

        Phone_IDXGISwapChain_GetFrameStatistics_NotAvailable_ModernApp = 1023,

        Phone_IDXGISwapChain_Present_ReplaceInterval0With1 = 1024,

        Phone_IDXGIFactory_CreateSwapChain_FailedRegisterWithCompositor = 1025,

        Phone_IDXGIFactory_CreateSwapChain_NotForegroundWindow_AtRendering = 1026,

        Phone_IDXGIFactory_CreateSwapChain_FLIP_SEQUENTIAL_BufferCount = 1027,

        Phone_IDXGIFactory_CreateSwapChain_FLIP_Modern_CoreWindow_Only = 1028,

        Phone_IDXGISwapChain_Present1_RequiresOverlays = 1029,

        Phone_IDXGISwapChain_SetBackgroundColor_FlipSequentialRequired = 1030,

        Phone_IDXGISwapChain_GetBackgroundColor_FlipSequentialRequired = 1031
    }
}
