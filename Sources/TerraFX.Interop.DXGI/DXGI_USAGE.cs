// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 4)]
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
            _value = (uint)(da & DXGI_CPU_ACCESS.FIELD)
                   | (uint)(dxgi & DXGI_USAGE_FLAG.FIELD);
        }
        #endregion

        #region Properties
        public DXGI_CPU_ACCESS DA
        {
            get
            {
                var da = _value & (uint)(DXGI_CPU_ACCESS.FIELD);
                return (DXGI_CPU_ACCESS)(da);
            }

            set
            {
                _value &= ~(uint)(DXGI_CPU_ACCESS.FIELD);
                _value |= (uint)(value & DXGI_CPU_ACCESS.FIELD);
            }
        }

        public DXGI_USAGE_FLAG DXGI
        {
            get
            {
                var dxgi = _value & (uint)(DXGI_USAGE_FLAG.FIELD);
                return (DXGI_USAGE_FLAG)(dxgi);
            }

            set
            {
                _value &= ~(uint)(DXGI_USAGE_FLAG.FIELD);
                _value |= (uint)(value & DXGI_USAGE_FLAG.FIELD);
            }
        }

        // Part of dxgiinternal.h
        // public DXGI_PRIVATE PRIVATE
        // {
        //     get
        //     {
        //         var @private = _value & (uint)(DXGI_PRIVATE.FIELD);
        //         return (DXGI_PRIVATE)(@private);
        //     }
        // 
        //     set
        //     {
        //         _value &= ~(uint)(DXGI_PRIVATE.FIELD);
        //         _value |= (uint)(value & DXGI_PRIVATE.FIELD);
        //     }
        // }

        // Part of dxgiinternal.h
        // public DXGI_PC PC
        // {
        //     get
        //     {
        //         var pc = _value & (uint)(DXGI_PC.FIELD);
        //         return (DXGI_PC)(pc);
        //     }
        // 
        //     set
        //     {
        //         _value &= ~(uint)(DXGI_PC.FIELD);
        //         _value |= (uint)(value & DXGI_PC.FIELD);
        //     }
        // }

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
