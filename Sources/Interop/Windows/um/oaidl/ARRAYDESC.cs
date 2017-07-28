// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct ARRAYDESC
    {
        #region Fields
        public TYPEDESC tdescElem;

        [ComAliasName("USHORT")]
        public ushort cDims;

        public _rgbounds_e__FixedBuffer rgbounds;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _rgbounds_e__FixedBuffer
        {
            #region Fields
            public SAFEARRAYBOUND e0;
            #endregion

            #region Properties
            public SAFEARRAYBOUND this[int index]
            {
                get
                {
                    if ((uint)(index) > 0) // (index1 < 0) || (index1 > 0)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (SAFEARRAYBOUND* e = &e0)
                    {
                        return e[index];
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
