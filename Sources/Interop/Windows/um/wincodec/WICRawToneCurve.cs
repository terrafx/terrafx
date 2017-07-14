// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct WICRawToneCurve
    {
        #region Fields
        public UINT cPoints;

        public _aPoints_e__FixedBuffer aPoints;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _aPoints_e__FixedBuffer
        {
            #region Fields
            public WICRawToneCurvePoint e0;
            #endregion

            #region Properties
            public WICRawToneCurvePoint this[int index]
            {
                get
                {
                    if ((uint)(index) > 0) // (index < 0) || (index > 0)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (WICRawToneCurvePoint* e = &e0)
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
