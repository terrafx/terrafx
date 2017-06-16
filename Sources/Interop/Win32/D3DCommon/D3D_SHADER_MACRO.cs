// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\D3DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    // Preprocessor macro definition. The application pass in a null-terminated
    // array of this structure to various D3D APIs. This enables the application
    // to #define tokens at runtime, before the file is parsed.
    public struct D3D_SHADER_MACRO
    {
        #region Fields
        public /* const */ LPSTR Name;

        public /* const */ LPSTR Definition;
        #endregion
    }
}
