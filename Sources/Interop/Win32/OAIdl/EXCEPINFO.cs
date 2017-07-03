// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    unsafe public struct EXCEPINFO
    {
        #region Fields
        public ushort wCode;

        public ushort wReserved;

        public BSTR bstrSource;

        public BSTR bstrDescription;

        public BSTR bstrHelpFile;

        public uint dwHelpContext;

        public void* pvReserved;

        public IntPtr pfnDeferredFillIn;

        public SCODE scode;
        #endregion
    }
}
