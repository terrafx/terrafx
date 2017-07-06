// Copyright © Tanner Gooding and Contributors.Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\objidlbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public  /* blittable */ struct STATSTG
    {
        #region Fields
        public LPOLESTR pwcsName;

        public DWORD type;

        public ULARGE_INTEGER cbSize;

        public FILETIME mtime;

        public FILETIME ctime;

        public FILETIME atime;

        public DWORD grfMode;

        public DWORD grfLocksSupported;

        public CLSID clsid;

        public DWORD grfStateBits;

        public DWORD reserved;
        #endregion
    }
}
