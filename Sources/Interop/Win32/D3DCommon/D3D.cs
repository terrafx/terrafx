// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\D3DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    unsafe public static class D3D
    {
        #region Constants
        #region REQ_TEXTURE1D_U_DIMENSION
        public const int FL9_1_REQ_TEXTURE1D_U_DIMENSION = 2048;

        public const int FL9_3_REQ_TEXTURE1D_U_DIMENSION = 4096;
        #endregion

        #region REQ_TEXTURE2D_U_OR_V_DIMENSION
        public const int FL9_1_REQ_TEXTURE2D_U_OR_V_DIMENSION = 2048;

        public const int FL9_3_REQ_TEXTURE2D_U_OR_V_DIMENSION = 4096;
        #endregion

        #region REQ_TEXTURECUBE_DIMENSION
        public const int FL9_1_REQ_TEXTURECUBE_DIMENSION = 512;

        public const int FL9_3_REQ_TEXTURECUBE_DIMENSION = 4096;
        #endregion

        #region REQ_TEXTURE3D_U_V_OR_W_DIMENSION
        public const int FL9_1_REQ_TEXTURE3D_U_V_OR_W_DIMENSION = 256;
        #endregion

        #region DEFAULT_MAX_ANISOTROPY
        public const int FL9_1_DEFAULT_MAX_ANISOTROPY = 2;
        #endregion

        #region IA_PRIMITIVE_MAX_COUNT
        public const int FL9_1_IA_PRIMITIVE_MAX_COUNT = 65535;

        public const int FL9_2_IA_PRIMITIVE_MAX_COUNT = 1048575;
        #endregion

        #region SIMULTANEOUS_RENDER_TARGET_COUNT
        public const int FL9_1_SIMULTANEOUS_RENDER_TARGET_COUNT = 1;

        public const int FL9_3_SIMULTANEOUS_RENDER_TARGET_COUNT = 4;
        #endregion

        #region MAX_TEXTURE_REPEAT
        public const int FL9_1_MAX_TEXTURE_REPEAT = 128;

        public const int FL9_2_MAX_TEXTURE_REPEAT = 2048;

        public const int FL9_3_MAX_TEXTURE_REPEAT = 8192;
        #endregion
        #endregion

        #region Methods
        // SET_OBJECT_NAME_N_A(pObject, Chars, pName)
        //      (pObject)->SetPrivateData(WKPDID_D3DDebugObjectName, Chars, pName);

        // SET_OBJECT_NAME_A(pObject, pName)
        //      D3D_SET_OBJECT_NAME_N_A(pObject, lstrlenA(pName), pName);

        // SET_OBJECT_NAME_N_W(pObject, Chars, pName)
        //      (pObject)->SetPrivateData(WKPDID_D3DDebugObjectNameW, Chars*2, pName);

        // SET_OBJECT_NAME_W(pObject, pName)
        //      D3D_SET_OBJECT_NAME_N_W(pObject, wcslen(pName), pName);
        #endregion
    }
}
