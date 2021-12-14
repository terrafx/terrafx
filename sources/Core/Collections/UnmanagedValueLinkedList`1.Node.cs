// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ICollectionDebugView<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Collections;

public partial struct UnmanagedValueLinkedList<T>
{
    /// <summary>Represents a node in a linked list.</summary>
    public unsafe struct Node
    {
        internal Node* _next;
        internal Node* _previous;
        internal T _value;
        internal bool _isFirstNode;

        /// <summary>Initializes a new instance of the <see cref="Node" /> class.</summary>
        /// <param name="value">The value held by the node.</param>
        public Node(T value)
        {
            _next = null;
            _previous = null;
            _value = value;
            _isFirstNode = false;
        }

        /// <summary>Gets <c>true</c> if the node belongs to a linked list; otherwise, <c>false</c>.</summary>
        public bool HasParent => _next is not null;

        /// <summary>Gets <c>true</c> if the node is the first node in a linked list; otherwise, <c>false</c>.</summary>
        public bool IsFirstNode => _isFirstNode;

        /// <summary>Gets the next node in the linked list or <c>null</c> if none exists.</summary>
        public Node* Next => ((_next is not null) && !_next->_isFirstNode) ? _next : null;

        /// <summary>Gets the previous node in the linked list or <c>null</c> if none exists.</summary>
        public Node* Previous => ((_previous is not null) && !_isFirstNode) ? _previous : null;

        /// <summary>Gets or sets the value held by the node.</summary>
        public T Value
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

        /// <summary>Gets a reference to the value held by the node.</summary>
        public ref T ValueRef => ref AsRef<T>(AsPointer(ref _value));

        internal void Invalidate()
        {
            _next = null;
            _previous = null;
            _isFirstNode = false;
        }
    }
}
