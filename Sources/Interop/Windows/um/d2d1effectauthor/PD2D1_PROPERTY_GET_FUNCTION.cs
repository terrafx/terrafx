// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Function pointer that gets a property from an effect.</summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.StdCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
    unsafe public /* static */ delegate HRESULT PD2D1_PROPERTY_GET_FUNCTION(
        [In] /* readonly */ IUnknown* effect,
        [Out, Optional] BYTE* data,
        [In] UINT32 dataSize,
        [Out, Optional] UINT32* actualSize
    );
}
