// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Font file loader interface handles loading font file resources of a particular type from a key. The font file loader interface is recommended to be implemented by a singleton object. IMPORTANT: font file loader implementations must not register themselves with DirectWrite factory inside their conpublic /* blittable */ structors and must not unregister themselves in their depublic /* blittable */ structors, because registration and unregistration operations increment and decrement the object reference count respectively. Instead, registration and unregistration of font file loaders with DirectWrite factory should be performed outside of the font file loader implementation as a separate step.</summary>
    [Guid("727CAD4E-D6AF-4C9E-8A08-D695B11CAA49")]
    public /* blittable */ unsafe struct IDWriteFontFileLoader
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Creates a font file stream object that encapsulates an open file resource. The resource is closed when the last reference to fontFileStream is released.</summary>
        /// <param name="fontFileReferenceKey">Font file reference key that uniquely identifies the font file resource within the scope of the font loader being used.</param>
        /// <param name="fontFileReferenceKeySize">Size of font file reference key in bytes.</param>
        /// <param name="fontFileStream">Pointer to the newly created font file stream.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateStreamFromKey(
            [In] IDWriteFontFileLoader* This,
            [In] void* fontFileReferenceKey,
            [In, ComAliasName("UINT32")] uint fontFileReferenceKeySize,
            [Out] IDWriteFontFileStream** fontFileStream
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr CreateStreamFromKey;
            #endregion
        }
        #endregion
    }
}
