// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>Blend description which configures a blend transform object.</summary>
    public /* blittable */ struct D2D1_BLEND_DESCRIPTION
    {
        #region Fields
        public D2D1_BLEND sourceBlend;

        public D2D1_BLEND destinationBlend;

        public D2D1_BLEND_OPERATION blendOperation;

        public D2D1_BLEND sourceBlendAlpha;

        public D2D1_BLEND destinationBlendAlpha;

        public D2D1_BLEND_OPERATION blendOperationAlpha;

        public _blendFactor_e__FixedBuffer blendFactor;
        #endregion

        #region Structs
        unsafe public struct _blendFactor_e__FixedBuffer
        {
            #region Fields
            public FLOAT e0;

            public FLOAT e1;

            public FLOAT e2;

            public FLOAT e3;
            #endregion

            #region Properties
            public FLOAT this[int index]
            {
                get
                {
                    if ((uint)(index) > 3) // (index < 0) || (index > 3)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (FLOAT* e = &e0)
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
