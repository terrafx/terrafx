// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\ocidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct PROPBAG2
    {
        #region Fields
        public DWORD dwType;

        public VARTYPE vt;

        public CLIPFORMAT cfType;

        public DWORD dwHint;

        public LPOLESTR pstrName;

        public CLSID clsid;
        #endregion
    }
}
