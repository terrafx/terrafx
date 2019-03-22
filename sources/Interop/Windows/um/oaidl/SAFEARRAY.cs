// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct SAFEARRAY
    {
        #region Fields
        [ComAliasName("USHORT")]
        public ushort cDims;

        [ComAliasName("USHORT")]
        public ushort fFeatures;

        [ComAliasName("ULONG")]
        public uint cbElements;

        [ComAliasName("ULONG")]
        public uint cLocks;

        [ComAliasName("PVOID")]
        public void* pvData;

        [ComAliasName("SAFEARRAYBOUND[1]")]
        public _rgsabound_e__FixedBuffer rgsabound;
        #endregion

        #region Structs
        [Unmanaged]
        public unsafe struct _rgsabound_e__FixedBuffer
        {
            #region Fields
            public SAFEARRAYBOUND e0;
            #endregion

            #region Properties
            public ref SAFEARRAYBOUND this[int index]
            {
                get
                {
                    fixed (SAFEARRAYBOUND* e = &e0)
                    {
                        return ref AsRef<SAFEARRAYBOUND>(e + index);
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
