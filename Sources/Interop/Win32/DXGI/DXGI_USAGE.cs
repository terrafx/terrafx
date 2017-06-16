// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public struct DXGI_USAGE
    {
        #region Fields
        private uint _value;
        #endregion

        #region Constructors
        public DXGI_USAGE(uint value)
        {
            _value = value;
        }

        public DXGI_USAGE(DXGI_CPU_ACCESS da, DXGI_USAGE_FLAG dxgi)
        {
            _value = ((uint)(da) & 0x0000000F)
                   | ((uint)(dxgi) & 0x0007FFF0);
        }
        #endregion

        #region Properties
        public DXGI_CPU_ACCESS DA
        {
            get
            {
                var da = _value & (uint)(0x0000000F);
                return (DXGI_CPU_ACCESS)(da);
            }

            set
            {
                _value &= ~(uint)(0x0000000F);
                _value |= ((uint)(value) & 0x0000000F);
            }
        }

        public DXGI_USAGE_FLAG DXGI
        {
            get
            {
                var dxgi = _value & (uint)(0x0007FFF0);
                return (DXGI_USAGE_FLAG)(dxgi);
            }

            set
            {
                _value &= ~(uint)(0x0007FFF0);
                _value |= ((uint)(value) & 0x0007FFF0);
            }
        }

        public uint Value
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
