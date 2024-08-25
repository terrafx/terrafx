// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using TerraFX.Collections;

namespace TerraFX;

public partial struct UnmanagedReadOnlySpan<T>
{
    /// <summary>An enumerator which can iterate through the items in a span.</summary>
    public struct Enumerator : IRefEnumerator<T>
    {
        private readonly UnmanagedReadOnlySpan<T> _span;
        private nuint _index;

        internal Enumerator(UnmanagedReadOnlySpan<T> span)
        {
            _span = span;
            _index = nuint.MaxValue;
        }

        /// <inheritdoc />
        public readonly T Current => CurrentRef;

        /// <inheritdoc />
        public readonly ref readonly T CurrentRef => ref _span.GetReferenceUnsafe(_index);

        /// <inheritdoc />
        public bool MoveNext()
        {
            var succeeded = true;
            var index = unchecked(_index + 1);

            if (index == _span.Length)
            {
                index--;
                succeeded = false;
            }

            _index = index;
            return succeeded;
        }

        /// <inheritdoc />
        public void Reset() => _index = nuint.MaxValue;

        readonly object IEnumerator.Current => Current;

        readonly void IDisposable.Dispose() { }
    }
}
