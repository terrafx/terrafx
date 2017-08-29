// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>A transform uses this interface to specify how to render a particular pass in D2D.</summary>
    [Guid("519AE1BD-D19A-420D-B849-364F594776B7")]
    public /* blittable */ unsafe struct ID2D1RenderInfo
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] ID2D1RenderInfo* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] ID2D1RenderInfo* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] ID2D1RenderInfo* This
        );
        #endregion

        #region Delegates
        /// <summary>Sets options for sampling the specified image input</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetInputDescription(
            [In] ID2D1RenderInfo* This,
            [In, ComAliasName("UINT32")] uint inputIndex,
            [In] D2D1_INPUT_DESCRIPTION inputDescription
        );

        /// <summary>Controls the output precision and channel-depth for the associated transform.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetOutputBuffer(
            [In] ID2D1RenderInfo* This,
            [In] D2D1_BUFFER_PRECISION bufferPrecision,
            [In] D2D1_CHANNEL_DEPTH channelDepth
        );

        /// <summary>Controls whether the output of the associated transform is cached.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetCached(
            [In] ID2D1RenderInfo* This,
            [In, ComAliasName("BOOL")] int isCached
        );

        /// <summary>Provides a hint of the approximate shader instruction count per pixel.  If provided, it may improve performance when processing large images.  Instructions should be counted multiple times if occurring within loops.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetInstructionCountHint(
            [In] ID2D1RenderInfo* This,
            [In, ComAliasName("UINT32")] uint instructionCount
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region Fields
            public IntPtr SetInputDescription;

            public IntPtr SetOutputBuffer;

            public IntPtr SetCached;

            public IntPtr SetInstructionCountHint;
            #endregion
        }
        #endregion
    }
}
