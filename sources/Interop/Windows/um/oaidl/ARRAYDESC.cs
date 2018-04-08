// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    public /* unmanaged */ struct ARRAYDESC
    {
        #region Fields
        public TYPEDESC tdescElem;

        [ComAliasName("USHORT")]
        public ushort cDims;

        [ComAliasName("SAFEARRAYBOUND[1]")]
        public _rgbounds_e__FixedBuffer rgbounds;
        #endregion

        #region Structs
        public /* unmanaged */ unsafe struct _rgbounds_e__FixedBuffer
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
