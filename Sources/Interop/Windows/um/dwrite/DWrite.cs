// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    unsafe public static partial class DWRITE
    {
        #region Constants
        /// <summary>Maximum alpha value in a texture returned by IDWriteGlyphRunAnalysis::CreateAlphaTexture.</summary>
        public const int DWRITE_ALPHA_MAX = 255;

        public const int FACILITY_DWRITE = 0x898;

        public const int DWRITE_ERR_BASE = 0x5000;
        #endregion

        #region Methods
        /// <summary>Creates a DirectWrite factory object that is used for subsequent creation of individual DirectWrite objects.</summary>
        /// <param name="factoryType">Identifies whether the factory object will be shared or isolated.</param>
        /// <param name="iid">Identifies the DirectWrite factory interface, such as __uuidof(IDWriteFactory).</param>
        /// <param name="factory">Receives the DirectWrite factory object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        /// <remarks>Obtains DirectWrite factory object that is used for subsequent creation of individual DirectWrite classes. DirectWrite factory contains internal state such as font loader registration and cached font data. In most cases it is recommended to use the shared factory object, because it allows multiple components that use DirectWrite to share internal DirectWrite state and reduce memory usage. However, there are cases when it is desirable to reduce the impact of a component, such as a plug-in from an untrusted source, on the rest of the process by sandboxing and isolating it from the rest of the process components. In such cases, it is recommended to use an isolated factory for the sandboxed component.</remarks>
        [DllImport("DWrite", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DWriteCreateFactory", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern HRESULT DWriteCreateFactory(
            [In] DWRITE_FACTORY_TYPE factoryType,
            [In] REFIID iid,
            [Out] IUnknown** factory
        );
        #endregion
    }
}
