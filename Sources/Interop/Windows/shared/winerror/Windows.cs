// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\winerror.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public static partial class Windows
    {
        #region Constants
        #region FACILITY_*
        public const int FACILITY_NULL = 0;

        public const int FACILITY_WIN32 = 7;

        public const int FACILITY_DXGI = 2170;

        public const int FACILITY_DIRECT3D12 = 2174;

        public const int FACILITY_DIRECT3D12_DEBUG = 2175;

        public const int FACILITY_WINCODEC_DWRITE_DWM = 2200;

        public const int FACILITY_DIRECT2D = 2201;
        #endregion

        #region ERROR_*
        public const int ERROR_FILE_NOT_FOUND = 2;

        public const int ERROR_ACCESS_DENIED = 5;

        public const int ERROR_INVALID_HANDLE = 6;

        public const int ERROR_OUTOFMEMORY = 14;

        public const int ERROR_INVALID_PARAMETER = 87;

        public const int ERROR_INSUFFICIENT_BUFFER = 122;

        public const int ERROR_ARITHMETIC_OVERFLOW = 534;
        #endregion

        #region E_*
        public static readonly HRESULT E_UNEXPECTED = (HRESULT)(0x8000FFFF);

        public static readonly HRESULT E_NOTIMPL = (HRESULT)(0x80004001);

        public static readonly HRESULT E_OUTOFMEMORY = (HRESULT)(0x8007000E);

        public static readonly HRESULT E_INVALIDARG = (HRESULT)(0x80070057);

        public static readonly HRESULT E_NOINTERFACE = (HRESULT)(0x80004002);

        public static readonly HRESULT E_POINTER = (HRESULT)(0x80004003);

        public static readonly HRESULT E_HANDLE = (HRESULT)(0x80070006);

        public static readonly HRESULT E_ABORT = (HRESULT)(0x80004004);

        public static readonly HRESULT E_FAIL = (HRESULT)(0x80004005);

        public static readonly HRESULT E_ACCESSDENIED = (HRESULT)(0x80070005);
        #endregion

        #region DXGI_STATUS_*
        public static readonly HRESULT DXGI_STATUS_OCCLUDED = 0x087A0001;

        public static readonly HRESULT DXGI_STATUS_CLIPPED = 0x087A0002;

        public static readonly HRESULT DXGI_STATUS_NO_REDIRECTION = 0x087A0004;

        public static readonly HRESULT DXGI_STATUS_NO_DESKTOP_ACCESS = 0x087A0005;

        public static readonly HRESULT DXGI_STATUS_GRAPHICS_VIDPN_SOURCE_IN_USE = 0x087A0006;

        public static readonly HRESULT DXGI_STATUS_MODE_CHANGED = 0x087A0007;

        public static readonly HRESULT DXGI_STATUS_MODE_CHANGE_IN_PROGRESS = 0x087A0008;

        public static readonly HRESULT DXGI_STATUS_UNOCCLUDED = 0x087A0009;

        public static readonly HRESULT DXGI_STATUS_DDA_WAS_STILL_DRAWING = 0x087A000A;

        public static readonly HRESULT DXGI_STATUS_PRESENT_REQUIRED = 0x087A002F;
        #endregion

        #region DXGI_ERROR_*
        public static readonly HRESULT DXGI_ERROR_INVALID_CALL = (HRESULT)(0x887A0001);

        public static readonly HRESULT DXGI_ERROR_NOT_FOUND = (HRESULT)(0x887A0002);

        public static readonly HRESULT DXGI_ERROR_MORE_DATA = (HRESULT)(0x887A0003);

        public static readonly HRESULT DXGI_ERROR_UNSUPPORTED = (HRESULT)(0x887A0004);

        public static readonly HRESULT DXGI_ERROR_DEVICE_REMOVED = (HRESULT)(0x887A0005);

        public static readonly HRESULT DXGI_ERROR_DEVICE_HUNG = (HRESULT)(0x887A0006);

        public static readonly HRESULT DXGI_ERROR_DEVICE_RESET = (HRESULT)(0x887A0007);

        public static readonly HRESULT DXGI_ERROR_WAS_STILL_DRAWING = (HRESULT)(0x887A000A);

        public static readonly HRESULT DXGI_ERROR_FRAME_STATISTICS_DISJOINT = (HRESULT)(0x887A000B);

        public static readonly HRESULT DXGI_ERROR_GRAPHICS_VIDPN_SOURCE_IN_USE = (HRESULT)(0x887A000C);

        public static readonly HRESULT DXGI_ERROR_DRIVER_INTERNAL_ERROR = (HRESULT)(0x887A0020);

        public static readonly HRESULT DXGI_ERROR_NONEXCLUSIVE = (HRESULT)(0x887A0021);

        public static readonly HRESULT DXGI_ERROR_NOT_CURRENTLY_AVAILABLE = (HRESULT)(0x887A0022);

        public static readonly HRESULT DXGI_ERROR_REMOTE_CLIENT_DISCONNECTED = (HRESULT)(0x887A0023);

        public static readonly HRESULT DXGI_ERROR_REMOTE_OUTOFMEMORY = (HRESULT)(0x887A0024);

        public static readonly HRESULT DXGI_ERROR_ACCESS_LOST = (HRESULT)(0x887A0026);

        public static readonly HRESULT DXGI_ERROR_WAIT_TIMEOUT = (HRESULT)(0x887A0027);

        public static readonly HRESULT DXGI_ERROR_SESSION_DISCONNECTED = (HRESULT)(0x887A0028);

        public static readonly HRESULT DXGI_ERROR_RESTRICT_TO_OUTPUT_STALE = (HRESULT)(0x887A0029);

        public static readonly HRESULT DXGI_ERROR_CANNOT_PROTECT_CONTENT = (HRESULT)(0x887A002A);

        public static readonly HRESULT DXGI_ERROR_ACCESS_DENIED = (HRESULT)(0x887A002B);

        public static readonly HRESULT DXGI_ERROR_NAME_ALREADY_EXISTS = (HRESULT)(0x887A002C);

        public static readonly HRESULT DXGI_ERROR_SDK_COMPONENT_MISSING = (HRESULT)(0x887A002D);

        public static readonly HRESULT DXGI_ERROR_NOT_CURRENT = (HRESULT)(0x887A002E);

        public static readonly HRESULT DXGI_ERROR_HW_PROTECTION_OUTOFMEMORY = (HRESULT)(0x887A0030);

        public static readonly HRESULT DXGI_ERROR_DYNAMIC_CODE_POLICY_VIOLATION = (HRESULT)(0x887A0031);

        public static readonly HRESULT DXGI_ERROR_NON_COMPOSITED_UI = (HRESULT)(0x887A0032);

        public static readonly HRESULT DXGI_ERROR_MODE_CHANGE_IN_PROGRESS = (HRESULT)(0x887A0025);

        public static readonly HRESULT DXGI_ERROR_CACHE_CORRUPT = (HRESULT)(0x887A0033);

        public static readonly HRESULT DXGI_ERROR_CACHE_FULL = (HRESULT)(0x887A0034);

        public static readonly HRESULT DXGI_ERROR_CACHE_HASH_COLLISION = (HRESULT)(0x887A0035);

        public static readonly HRESULT DXGI_ERROR_ALREADY_EXISTS = (HRESULT)(0x887A0036);
        #endregion

        #region D3D12_ERROR_*
        public static readonly HRESULT D3D12_ERROR_ADAPTER_NOT_FOUND = (HRESULT)(0x887E0001);

        public static readonly HRESULT D3D12_ERROR_DRIVER_VERSION_MISMATCH = (HRESULT)(0x887E0002);
        #endregion

        #region D2DERR_*
        public static readonly HRESULT D2DERR_WRONG_STATE = (HRESULT)(0x88990001);

        public static readonly HRESULT D2DERR_NOT_INITIALIZED = (HRESULT)(0x88990002);

        public static readonly HRESULT D2DERR_UNSUPPORTED_OPERATION = (HRESULT)(0x88990003);

        public static readonly HRESULT D2DERR_SCANNER_FAILED = (HRESULT)(0x88990004);

        public static readonly HRESULT D2DERR_SCREEN_ACCESS_DENIED = (HRESULT)(0x88990005);

        public static readonly HRESULT D2DERR_DISPLAY_STATE_INVALID = (HRESULT)(0x88990006);

        public static readonly HRESULT D2DERR_ZERO_VECTOR = (HRESULT)(0x88990007);

        public static readonly HRESULT D2DERR_INTERNAL_ERROR = (HRESULT)(0x88990008);

        public static readonly HRESULT D2DERR_DISPLAY_FORMAT_NOT_SUPPORTED = (HRESULT)(0x88990009);

        public static readonly HRESULT D2DERR_INVALID_CALL = (HRESULT)(0x8899000A);

        public static readonly HRESULT D2DERR_NO_HARDWARE_DEVICE = (HRESULT)(0x8899000B);

        public static readonly HRESULT D2DERR_RECREATE_TARGET = (HRESULT)(0x8899000C);

        public static readonly HRESULT D2DERR_TOO_MANY_SHADER_ELEMENTS = (HRESULT)(0x8899000D);

        public static readonly HRESULT D2DERR_SHADER_COMPILE_FAILED = (HRESULT)(0x8899000E);

        public static readonly HRESULT D2DERR_MAX_TEXTURE_SIZE_EXCEEDED = (HRESULT)(0x8899000F);

        public static readonly HRESULT D2DERR_UNSUPPORTED_VERSION = (HRESULT)(0x88990010);

        public static readonly HRESULT D2DERR_BAD_NUMBER = (HRESULT)(0x88990011);

        public static readonly HRESULT D2DERR_WRONG_FACTORY = (HRESULT)(0x88990012);

        public static readonly HRESULT D2DERR_LAYER_ALREADY_IN_USE = (HRESULT)(0x88990013);

        public static readonly HRESULT D2DERR_POP_CALL_DID_NOT_MATCH_PUSH = (HRESULT)(0x88990014);

        public static readonly HRESULT D2DERR_WRONG_RESOURCE_DOMAIN = (HRESULT)(0x88990015);

        public static readonly HRESULT D2DERR_PUSH_POP_UNBALANCED = (HRESULT)(0x88990016);

        public static readonly HRESULT D2DERR_RENDER_TARGET_HAS_LAYER_OR_CLIPRECT = (HRESULT)(0x88990017);

        public static readonly HRESULT D2DERR_INCOMPATIBLE_BRUSH_TYPES = (HRESULT)(0x88990018);

        public static readonly HRESULT D2DERR_WIN32_ERROR = (HRESULT)(0x88990019);

        public static readonly HRESULT D2DERR_TARGET_NOT_GDI_COMPATIBLE = (HRESULT)(0x8899001A);

        public static readonly HRESULT D2DERR_TEXT_EFFECT_IS_WRONG_TYPE = (HRESULT)(0x8899001B);

        public static readonly HRESULT D2DERR_TEXT_RENDERER_NOT_RELEASED = (HRESULT)(0x8899001C);

        public static readonly HRESULT D2DERR_EXCEEDS_MAX_BITMAP_SIZE = (HRESULT)(0x8899001D);

        public static readonly HRESULT D2DERR_INVALID_GRAPH_CONFIGURATION = (HRESULT)(0x8899001E);

        public static readonly HRESULT D2DERR_INVALID_INTERNAL_GRAPH_CONFIGURATION = (HRESULT)(0x8899001F);

        public static readonly HRESULT D2DERR_CYCLIC_GRAPH = (HRESULT)(0x88990020);

        public static readonly HRESULT D2DERR_BITMAP_CANNOT_DRAW = (HRESULT)(0x88990021);

        public static readonly HRESULT D2DERR_OUTSTANDING_BITMAP_REFERENCES = (HRESULT)(0x88990022);

        public static readonly HRESULT D2DERR_ORIGINAL_TARGET_NOT_BOUND = (HRESULT)(0x88990023);

        public static readonly HRESULT D2DERR_INVALID_TARGET = (HRESULT)(0x88990024);

        public static readonly HRESULT D2DERR_BITMAP_BOUND_AS_TARGET = (HRESULT)(0x88990025);

        public static readonly HRESULT D2DERR_INSUFFICIENT_DEVICE_CAPABILITIES = (HRESULT)(0x88990026);

        public static readonly HRESULT D2DERR_INTERMEDIATE_TOO_LARGE = (HRESULT)(0x88990027);

        public static readonly HRESULT D2DERR_EFFECT_IS_NOT_REGISTERED = (HRESULT)(0x88990028);

        public static readonly HRESULT D2DERR_INVALID_PROPERTY = (HRESULT)(0x88990029);

        public static readonly HRESULT D2DERR_NO_SUBPROPERTIES = (HRESULT)(0x8899002A);

        public static readonly HRESULT D2DERR_PRINT_JOB_CLOSED = (HRESULT)(0x8899002B);

        public static readonly HRESULT D2DERR_PRINT_FORMAT_NOT_SUPPORTED = (HRESULT)(0x8899002C);

        public static readonly HRESULT D2DERR_TOO_MANY_TRANSFORM_INPUTS = (HRESULT)(0x8899002D);

        public static readonly HRESULT D2DERR_INVALID_GLYPH_IMAGE = (HRESULT)(0x8899002E);
        #endregion

        #region WINCODEC_ERR_*
        public static readonly HRESULT WINCODEC_ERR_WRONGSTATE = (HRESULT)(0x88982F04);

        public static readonly HRESULT WINCODEC_ERR_VALUEOUTOFRANGE = (HRESULT)(0x88982F05);

        public static readonly HRESULT WINCODEC_ERR_UNKNOWNIMAGEFORMAT = (HRESULT)(0x88982F07);

        public static readonly HRESULT WINCODEC_ERR_UNSUPPORTEDVERSION = (HRESULT)(0x88982F0B);

        public static readonly HRESULT WINCODEC_ERR_NOTINITIALIZED = (HRESULT)(0x88982F0C);

        public static readonly HRESULT WINCODEC_ERR_ALREADYLOCKED = (HRESULT)(0x88982F0D);

        public static readonly HRESULT WINCODEC_ERR_PROPERTYNOTFOUND = (HRESULT)(0x88982F40);

        public static readonly HRESULT WINCODEC_ERR_PROPERTYNOTSUPPORTED = (HRESULT)(0x88982F41);

        public static readonly HRESULT WINCODEC_ERR_PROPERTYSIZE = (HRESULT)(0x88982F42);

        public static readonly HRESULT WINCODEC_ERR_CODECPRESENT = (HRESULT)(0x88982F43);

        public static readonly HRESULT WINCODEC_ERR_CODECNOTHUMBNAIL = (HRESULT)(0x88982F44);

        public static readonly HRESULT WINCODEC_ERR_PALETTEUNAVAILABLE = (HRESULT)(0x88982F45);

        public static readonly HRESULT WINCODEC_ERR_CODECTOOMANYSCANLINES = (HRESULT)(0x88982F46);

        public static readonly HRESULT WINCODEC_ERR_INTERNALERROR = (HRESULT)(0x88982F48);

        public static readonly HRESULT WINCODEC_ERR_SOURCERECTDOESNOTMATCHDIMENSIONS = (HRESULT)(0x88982F49);

        public static readonly HRESULT WINCODEC_ERR_COMPONENTNOTFOUND = (HRESULT)(0x88982F50);

        public static readonly HRESULT WINCODEC_ERR_IMAGESIZEOUTOFRANGE = (HRESULT)(0x88982F51);

        public static readonly HRESULT WINCODEC_ERR_TOOMUCHMETADATA = (HRESULT)(0x88982F52);

        public static readonly HRESULT WINCODEC_ERR_BADIMAGE = (HRESULT)(0x88982F60);

        public static readonly HRESULT WINCODEC_ERR_BADHEADER = (HRESULT)(0x88982F61);

        public static readonly HRESULT WINCODEC_ERR_FRAMEMISSING = (HRESULT)(0x88982F62);

        public static readonly HRESULT WINCODEC_ERR_BADMETADATAHEADER = (HRESULT)(0x88982F63);

        public static readonly HRESULT WINCODEC_ERR_BADSTREAMDATA = (HRESULT)(0x88982F70);

        public static readonly HRESULT WINCODEC_ERR_STREAMWRITE = (HRESULT)(0x88982F71);

        public static readonly HRESULT WINCODEC_ERR_STREAMREAD = (HRESULT)(0x88982F72);

        public static readonly HRESULT WINCODEC_ERR_STREAMNOTAVAILABLE = (HRESULT)(0x88982F73);

        public static readonly HRESULT WINCODEC_ERR_UNSUPPORTEDPIXELFORMAT = (HRESULT)(0x88982F80);

        public static readonly HRESULT WINCODEC_ERR_UNSUPPORTEDOPERATION = (HRESULT)(0x88982F81);

        public static readonly HRESULT WINCODEC_ERR_INVALIDREGISTRATION = (HRESULT)(0x88982F8A);

        public static readonly HRESULT WINCODEC_ERR_COMPONENTINITIALIZEFAILURE = (HRESULT)(0x88982F8B);

        public static readonly HRESULT WINCODEC_ERR_INSUFFICIENTBUFFER = (HRESULT)(0x88982F8C);

        public static readonly HRESULT WINCODEC_ERR_DUPLICATEMETADATAPRESENT = (HRESULT)(0x88982F8D);

        public static readonly HRESULT WINCODEC_ERR_PROPERTYUNEXPECTEDTYPE = (HRESULT)(0x88982F8E);

        public static readonly HRESULT WINCODEC_ERR_UNEXPECTEDSIZE = (HRESULT)(0x88982F8F);

        public static readonly HRESULT WINCODEC_ERR_INVALIDQUERYREQUEST = (HRESULT)(0x88982F90);

        public static readonly HRESULT WINCODEC_ERR_UNEXPECTEDMETADATATYPE = (HRESULT)(0x88982F91);

        public static readonly HRESULT WINCODEC_ERR_REQUESTONLYVALIDATMETADATAROOT = (HRESULT)(0x88982F92);

        public static readonly HRESULT WINCODEC_ERR_INVALIDQUERYCHARACTER = (HRESULT)(0x88982F93);

        public static readonly HRESULT WINCODEC_ERR_WIN32ERROR = (HRESULT)(0x88982F94);

        public static readonly HRESULT WINCODEC_ERR_INVALIDPROGRESSIVELEVEL = (HRESULT)(0x88982F95);

        public static readonly HRESULT WINCODEC_ERR_INVALIDJPEGSCANINDEX = (HRESULT)(0x88982F96);
        #endregion
        #endregion
    }
}
