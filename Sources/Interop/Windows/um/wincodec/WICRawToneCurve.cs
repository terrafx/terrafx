// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct WICRawToneCurve
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint cPoints;

        [ComAliasName("WICRawToneCurvePoint[1]")]
        public _aPoints_e__FixedBuffer aPoints;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _aPoints_e__FixedBuffer
        {
            #region Fields
            public WICRawToneCurvePoint e0;
            #endregion

            #region Properties
            public ref WICRawToneCurvePoint this[int index]
            {
                get
                {
                    fixed (WICRawToneCurvePoint* e = &e0)
                    {
                        return ref AsRef<WICRawToneCurvePoint>(e + index);
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
