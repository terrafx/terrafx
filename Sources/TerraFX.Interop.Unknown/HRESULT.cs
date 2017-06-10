// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\wtypesbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.Unknown
{
    public struct HRESULT
    {
        #region Fields
        private int _value;
        #endregion

        #region Constructors
        public HRESULT(int value)
        {
            _value = value;
        }
        #endregion

        #region Properties
        public int Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }
        #endregion
    }
}
