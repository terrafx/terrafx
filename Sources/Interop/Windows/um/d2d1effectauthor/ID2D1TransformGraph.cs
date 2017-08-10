// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The implementation of the actual graph.</summary>
    [Guid("13D29038-C3E6-4034-9081-13B53A417992")]
    unsafe public /* blittable */ struct ID2D1TransformGraph
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Return the number of input this graph has.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetInputCount(
            [In] ID2D1TransformGraph* This
        );

        /// <summary>Sets the graph to contain a single transform whose inputs map 1:1 with effect inputs.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetSingleTransformNode(
            [In] ID2D1TransformGraph* This,
            [In] ID2D1TransformNode* node
        );

        /// <summary>Adds the given transform node to the graph.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddNode(
            [In] ID2D1TransformGraph* This,
            [In] ID2D1TransformNode* node
        );

        /// <summary>Removes the given transform node from the graph.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RemoveNode(
            [In] ID2D1TransformGraph* This,
            [In] ID2D1TransformNode* node
        );

        /// <summary>Indicates that the given transform node should be considered to be the output node of the graph.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetOutputNode(
            [In] ID2D1TransformGraph* This,
            [In] ID2D1TransformNode* node
        );

        /// <summary>Connects one node to another node inside the graph.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ConnectNode(
            [In] ID2D1TransformGraph* This,
            [In] ID2D1TransformNode* fromNode,
            [In] ID2D1TransformNode* toNode,
            [In, ComAliasName("UINT32")] uint toNodeInputIndex
        );

        /// <summary>Connects a transform node inside the graph to the corresponding input of the encapsulating effect.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ConnectToEffectInput(
            [In] ID2D1TransformGraph* This,
            [In, ComAliasName("UINT32")] uint toEffectInputIndex,
            [In] ID2D1TransformNode* node,
            [In, ComAliasName("UINT32")] uint toNodeInputIndex
        );

        /// <summary>Clears all nodes and connections from the transform graph.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void Clear(
            [In] ID2D1TransformGraph* This
        );

        /// <summary>Uses the specified input as the effect output.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPassthroughGraph(
            [In] ID2D1TransformGraph* This,
            [In, ComAliasName("UINT32")] uint effectInputIndex
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr GetInputCount;

            public IntPtr SetSingleTransformNode;

            public IntPtr AddNode;

            public IntPtr RemoveNode;

            public IntPtr SetOutputNode;

            public IntPtr ConnectNode;

            public IntPtr ConnectToEffectInput;

            public IntPtr Clear;

            public IntPtr SetPassthroughGraph;
            #endregion
        }
        #endregion
    }
}
