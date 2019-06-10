using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using TerraFX.Interop;

namespace TerraFX.Provider.PulseAudio.Audio
{
    internal static class ErrorMappings
    {
        private static readonly ConcurrentDictionary<int, string> Mappings
            = new ConcurrentDictionary<int, string>();

        internal static unsafe string GetErrorMapping(int errno)
        {
            return Mappings.GetOrAdd(errno, GetErrorString);

            static string GetErrorString(int errno)
            {
                return Marshal.PtrToStringUTF8((IntPtr)libpulse.pa_strerror(errno));
            }
        }
    }
}
