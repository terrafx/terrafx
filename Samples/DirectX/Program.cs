// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Samples.DirectX.D3D12;
using static TerraFX.Interop.Kernel32;
using static TerraFX.Interop.User32;

namespace TerraFX.Samples.DirectX
{
    public static unsafe class Program
    {
        public static int Main()
        {
            var hInstance = GetModuleHandle();
            var sample = new HelloWindow(1280, 720, "D3D12 Hello Window");
            return Win32Application.Run(sample, hInstance, SW_SHOWDEFAULT);
        }
    }
}
