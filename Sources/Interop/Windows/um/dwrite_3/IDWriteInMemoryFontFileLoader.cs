// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The IDWriteInMemoryFontFileLoader interface enables clients to reference in-memory fonts without having to implement a custom loader. The IDWriteFactory5::CreateInMemoryFontFileLoader method returns an instance of this interface, which the client is responsible for registering and unregistering using IDWriteFactory::RegisterFontFileLoader and IDWriteFactory::UnregisterFontFileLoader.</summary>
    [Guid("DC102F47-A12D-4B1C-822D-9E117E33043F")]
    public /* blittable */ unsafe struct IDWriteInMemoryFontFileLoader
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>The CreateInMemoryFontFileReference method creates a font file reference (IDWriteFontFile object) from an array of bytes. The font file reference is bound to the IDWRiteInMemoryFontFileLoader instance with which it was created and remains valid for as long as that loader is registered with the factory.</summary>
        /// <param name="factory">Factory object used to create the font file reference.</param>
        /// <param name="fontData">Pointer to a memory block containing the font data.</param>
        /// <param name="fontDataSize">Size of the font data.</param>
        /// <param name="ownerObject">Optional object that owns the memory specified by the fontData parameter. If this parameter is not NULL, the method stores a pointer to the font data and adds a reference to the owner object. The fontData pointer must remain valid until the owner object is released. If this parameter is NULL, the method makes a copy of the font data.</param>
        /// <param name="fontFile">Receives a pointer to the newly-created font file reference.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateInMemoryFontFileReference(
            [In] IDWriteInMemoryFontFileLoader* This,
            [In] IDWriteFactory* factory,
            [In] /* readonly */ void* fontData,
            [In, ComAliasName("UINT32")] uint fontDataSize,
            [In, Optional] IUnknown* ownerObject,
            [Out] IDWriteFontFile** fontFile
        );

        /// <summary>The GetFileCount method returns the number of font file references that have been created using this loader instance.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetFileCount(
            [In] IDWriteInMemoryFontFileLoader* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFontFileLoader.Vtbl BaseVtbl;

            public IntPtr CreateInMemoryFontFileReference;

            public IntPtr GetFileCount;
            #endregion
        }
        #endregion
    }
}
