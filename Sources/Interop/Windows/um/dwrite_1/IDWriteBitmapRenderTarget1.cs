// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Encapsulates a 32-bit device independent bitmap and device context, which can be used for rendering glyphs.</summary>
    [Guid("791E8298-3EF3-4230-9880-C9BDECC42064")]
    unsafe public /* blittable */ struct IDWriteBitmapRenderTarget1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Gets the current text antialiasing mode of the bitmap render target.</summary>
        /// <returns> Returns the antialiasing mode.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_TEXT_ANTIALIAS_MODE GetTextAntialiasMode(
            [In] IDWriteBitmapRenderTarget1* This
        );

        /// <summary>Sets the current text antialiasing mode of the bitmap render target.</summary>
        /// <returns> Returns S_OK if successful, or E_INVALIDARG if the argument is not valid.</returns>
        /// <remarks> The antialiasing mode of a newly-created bitmap render target defaults to DWRITE_TEXT_ANTIALIAS_MODE_CLEARTYPE. An application can change the antialiasing mode by calling SetTextAntialiasMode. For example, an application might specify grayscale antialiasing when rendering text onto a transparent bitmap.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetTextAntialiasMode(
            [In] IDWriteBitmapRenderTarget1* This,
            [In] DWRITE_TEXT_ANTIALIAS_MODE antialiasMode
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteBitmapRenderTarget.Vtbl BaseVtbl;

            public GetTextAntialiasMode GetTextAntialiasMode;

            public SetTextAntialiasMode SetTextAntialiasMode;
            #endregion
        }
        #endregion
    }
}
