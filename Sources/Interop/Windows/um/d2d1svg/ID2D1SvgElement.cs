// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1svg.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Interface for all SVG elements.</summary>
    [Guid("AC7B67A6-183E-49C1-A823-0EBE40B0DB29")]
    unsafe public /* blittable */ struct ID2D1SvgElement
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Gets the document that contains this element. Returns null if the element has been removed from the tree.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDocument(
            [In] ID2D1SvgElement* This,
            [Out] ID2D1SvgDocument** document
        );

        /// <summary>Gets the tag name.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetTagName(
            [In] ID2D1SvgElement* This,
            [Out] PWSTR name,
            [In] UINT32 nameCount
        );

        /// <summary>Gets the string length of the tag name. The returned string length does not include room for the null terminator.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT32 GetTagNameLength(
            [In] ID2D1SvgElement* This
        );

        /// <summary>Returns TRUE if this element represents text content, e.g. the content of a 'title' or 'desc' element. Text content does not have a tag name.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL IsTextContent(
            [In] ID2D1SvgElement* This
        );

        /// <summary>Gets the parent element.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetParent(
            [In] ID2D1SvgElement* This,
            [Out] ID2D1SvgElement** parent
        );

        /// <summary>Returns whether this element has children.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL HasChildren(
            [In] ID2D1SvgElement* This
        );

        /// <summary>Gets the first child of this element.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetFirstChild(
            [In] ID2D1SvgElement* This,
            [Out] ID2D1SvgElement** child
        );

        /// <summary>Gets the last child of this element.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetLastChild(
            [In] ID2D1SvgElement* This,
            [Out] ID2D1SvgElement** child
        );

        /// <summary>Gets the previous sibling of the referenceChild element.</summary>
        /// <param name="referenceChild">The referenceChild must be an immediate child of this element.</param>
        /// <param name="previousChild">The output previousChild element will be non-null if the referenceChild has a previous sibling. If the referenceChild is the first child, the output is null.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetPreviousChild(
            [In] ID2D1SvgElement* This,
            [In] ID2D1SvgElement* referenceChild,
            [Out] ID2D1SvgElement** previousChild
        );

        /// <summary>Gets the next sibling of the referenceChild element.</summary>
        /// <param name="referenceChild">The referenceChild must be an immediate child of this element.</param>
        /// <param name="nextChild">The output nextChild element will be non-null if the referenceChild has a next sibling. If the referenceChild is the last child, the output is null.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetNextChild(
            [In] ID2D1SvgElement* This,
            [In] ID2D1SvgElement* referenceChild,
            [Out] ID2D1SvgElement** nextChild
        );

        /// <summary>Inserts newChild as a child of this element, before the referenceChild element. If the newChild element already has a parent, it is removed from this parent as part of the insertion. Returns an error if this element cannot accept children of the type of newChild. Returns an error if the newChild is an ancestor of this element.</summary>
        /// <param name="newChild">The element to be inserted.</param>
        /// <param name="referenceChild">The element that the child should be inserted before. If referenceChild is null, the newChild is placed as the last child. If referenceChild is non-null, it must be an immediate child of this element.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT InsertChildBefore(
            [In] ID2D1SvgElement* This,
            [In] ID2D1SvgElement* newChild,
            [In] ID2D1SvgElement* referenceChild = null
        );

        /// <summary>Appends newChild to the list of children. If the newChild element already has a parent, it is removed from this parent as part of the append operation. Returns an error if this element cannot accept children of the type of newChild. Returns an error if the newChild is an ancestor of this element.</summary>
        /// <param name="newChild">The element to be appended.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AppendChild(
            [In] ID2D1SvgElement* This,
            [In] ID2D1SvgElement* newChild
        );

        /// <summary>Replaces the oldChild element with the newChild. This operation removes the oldChild from the tree. If the newChild element already has a parent, it is removed from this parent as part of the replace operation. Returns an error if this element cannot accept children of the type of newChild. Returns an error if the newChild is an ancestor of this element.</summary>
        /// <param name="newChild">The element to be inserted.</param>
        /// <param name="oldChild">The child element to be replaced. The oldChild element must be an immediate child of this element.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT ReplaceChild(
            [In] ID2D1SvgElement* This,
            [In] ID2D1SvgElement* newChild,
            [In] ID2D1SvgElement* oldChild
        );

        /// <summary>Removes the oldChild from the tree. Children of oldChild remain children of oldChild.</summary>
        /// <param name="oldChild">The child element to be removed. The oldChild element must be an immediate child of this element.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RemoveChild(
            [In] ID2D1SvgElement* This,
            [In] ID2D1SvgElement* oldChild
        );

        /// <summary>Creates an element from a tag name. The element is appended to the list of children. Returns an error if this element cannot accept children of the specified type.</summary>
        /// <param name="tagName">The tag name of the new child. A NULL tagName or an empty string is interpreted to be a text content element.</param>
        /// <param name="newChild">The new child element.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateChild(
            [In] ID2D1SvgElement* This,
            [In, Optional] PCWSTR tagName,
            [Out] ID2D1SvgElement** newChild
        );

        /// <summary>Returns true if the attribute is explicitly set on the element or if it is present within an inline style. Returns FALSE if the attribute is not a valid attribute on this element.</summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="inherited">Outputs whether the attribute is set to the 'inherit' value.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL IsAttributeSpecified(
            [In] ID2D1SvgElement* This,
            [In] PCWSTR name,
            [Out] BOOL* inherited = null
        );

        /// <summary>Returns the number of specified attributes on this element. Attributes are only considered specified if they are explicitly set on the element or present within an inline style. Properties that receive their value through CSS inheritance are not considered specified. An attribute can become specified if it is set through a method call. It can become unspecified if it is removed via RemoveAttribute.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT32 GetSpecifiedAttributeCount(
            [In] ID2D1SvgElement* This
        );

        /// <summary>Gets the name of the specified attribute at the given index.</summary>
        /// <param name="index">The specified index of the attribute.</param>
        /// <param name="name">Outputs the name of the attribute.</param>
        /// <param name="inherited">Outputs whether the attribute is set to the 'inherit' value.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSpecifiedAttributeName(
            [In] ID2D1SvgElement* This,
            [In] UINT32 index,
            [Out] PWSTR name,
            [In] UINT32 nameCount,
            [Out] BOOL* inherited = null
        );

        /// <summary>Gets the string length of the name of the specified attribute at the given index. The output string length does not include room for the null terminator.</summary>
        /// <param name="index">The specified index of the attribute.</param>
        /// <param name="nameLength">Outputs the string length of the name of the specified attribute.</param>
        /// <param name="inherited">Outputs whether the attribute is set to the 'inherit' value.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSpecifiedAttributeNameLength(
            [In] ID2D1SvgElement* This,
            [In] UINT32 index,
            [Out] UINT32* nameLength,
            [Out] BOOL* inherited = null
        );

        /// <summary>Removes the attribute from this element. Also removes this attribute from within an inline style if present. Returns an error if the attribute name is not valid on this element.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RemoveAttribute(
            [In] ID2D1SvgElement* This,
            [In] PCWSTR name
        );

        /// <summary>Sets the value of a text content element.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetTextValue(
            [In] ID2D1SvgElement* This,
            [In] /* readonly */ WCHAR* name,
            [In] UINT32 nameCount
        );

        /// <summary>Gets the value of a text content element.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetTextValue(
            [In] ID2D1SvgElement* This,
            [Out] PWSTR name,
            [In] UINT32 nameCount
        );

        /// <summary>Gets the length of the text content value. The returned string length does not include room for the null terminator.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT32 GetTextValueLength(
            [In] ID2D1SvgElement* This
        );

        /// <summary>Sets an attribute of this element using a string. Returns an error if the attribute name is not valid on this element. Returns an error if the attribute cannot be expressed as the specified type.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetAttributeValue(
            [In] ID2D1SvgElement* This,
            [In] PCWSTR name,
            [In] D2D1_SVG_ATTRIBUTE_STRING_TYPE type,
            [In] PCWSTR value
        );

        /// <summary>Gets an attribute of this element as a string. Returns an error if the attribute is not specified. Returns an error if the attribute name is not valid on this element. Returns an error if the attribute cannot be expressed as the specified string type.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetAttributeValue(
            [In] ID2D1SvgElement* This,
            [In] PCWSTR name,
            [In] D2D1_SVG_ATTRIBUTE_STRING_TYPE type,
            [Out] PWSTR value,
            [In] UINT32 valueCount
        );

        /// <summary>Gets the string length of an attribute of this element. The returned string length does not include room for the null terminator. Returns an error if the attribute is not specified. Returns an error if the attribute name is not valid on this element. Returns an error if the attribute cannot be expressed as the specified string type.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetAttributeValueLength(
            [In] ID2D1SvgElement* This,
            [In] PCWSTR name,
            [In] D2D1_SVG_ATTRIBUTE_STRING_TYPE type,
            [Out] UINT32* valueLength
        );

        /// <summary>Sets an attribute of this element using a POD type. Returns an error if the attribute name is not valid on this element. Returns an error if the attribute cannot be expressed as the specified type.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetAttributeValue1(
            [In] ID2D1SvgElement* This,
            [In] PCWSTR name,
            [In] D2D1_SVG_ATTRIBUTE_POD_TYPE type,
            [In] /* readonly */ void* value,
            [In] UINT32 valueSizeInBytes
        );

        /// <summary>Gets an attribute of this element as a POD type. Returns an error if the attribute is not specified. Returns an error if the attribute name is not valid on this element. Returns an error if the attribute cannot be expressed as the specified POD type.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetAttributeValue1(
            [In] ID2D1SvgElement* This,
            [In] PCWSTR name,
            [In] D2D1_SVG_ATTRIBUTE_POD_TYPE type,
            [Out] void* value,
            [In] UINT32 valueSizeInBytes
        );

        /// <summary>Sets an attribute of this element using an interface. Returns an error if the attribute name is not valid on this element. Returns an error if the attribute cannot be expressed as the specified interface type. Returns an error if the attribute object is already set on an element. A given attribute object may only be set on one element in one attribute location at a time.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetAttributeValue2(
            [In] ID2D1SvgElement* This,
            [In] PCWSTR name,
            [In] ID2D1SvgAttribute* value
        );

        /// <summary>Gets an attribute of this element as an interface type. Returns an error if the attribute is not specified. Returns an error if the attribute name is not valid on this element. Returns an error if the attribute cannot be expressed as the specified interface type.</summary>
        /// <param name="riid">The interface ID of the attribute value.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetAttributeValue2(
            [In] ID2D1SvgElement* This,
            [In] PCWSTR name,
            [In] REFIID riid,
            [Out] void** value
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;

            public GetDocument GetDocument;

            public GetTagName GetTagName;

            public GetTagNameLength GetTagNameLength;

            public IsTextContent IsTextContent;

            public GetParent GetParent;

            public HasChildren HasChildren;

            public GetFirstChild GetFirstChild;

            public GetLastChild GetLastChild;

            public GetPreviousChild GetPreviousChild;

            public GetNextChild GetNextChild;

            public InsertChildBefore InsertChildBefore;

            public AppendChild AppendChild;

            public ReplaceChild ReplaceChild;

            public RemoveChild RemoveChild;

            public CreateChild CreateChild;

            public IsAttributeSpecified IsAttributeSpecified;

            public GetSpecifiedAttributeCount GetSpecifiedAttributeCount;

            public GetSpecifiedAttributeName GetSpecifiedAttributeName;

            public GetSpecifiedAttributeNameLength GetSpecifiedAttributeNameLength;

            public RemoveAttribute RemoveAttribute;

            public SetTextValue SetTextValue;

            public GetTextValue GetTextValue;

            public GetTextValueLength GetTextValueLength;

            public SetAttributeValue SetAttributeValue;

            public GetAttributeValue GetAttributeValue;

            public GetAttributeValueLength GetAttributeValueLength;

            public SetAttributeValue1 SetAttributeValue1;

            public GetAttributeValue1 GetAttributeValue1;

            public SetAttributeValue2 SetAttributeValue2;

            public GetAttributeValue2 GetAttributeValue2;
            #endregion
        }
        #endregion
    }
}
