// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    unsafe public  /* blittable */ struct SAFEARRAY
    {
        #region Fields
        public USHORT cDims;

        public USHORT fFeatures;

        public ULONG cbElements;

        public ULONG cLocks;

        public PVOID pvData;

        public _rgsabound_e__FixedBuffer rgsabound;
        #endregion

        #region Structs
        public /* blittable */ struct _rgsabound_e__FixedBuffer
        {
            #region Fields
            public SAFEARRAYBOUND _0;
            #endregion
        }
        #endregion
    }
}
