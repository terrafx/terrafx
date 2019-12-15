// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace TerraFX.UI
{
    /// <summary>Provides access to a window subsystem.</summary>
    public abstract class WindowProvider : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="WindowProvider" /> class.</summary>
        protected WindowProvider()
        {
        }

        /// <summary>Gets the <see cref="UI.DispatchProvider" /> for the instance.</summary>
        public abstract DispatchProvider DispatchProvider { get; }

        /// <summary>Gets the <see cref="Window" /> objects created by the instance which are associated with <see cref="Thread.CurrentThread" />.</summary>
        public abstract IEnumerable<Window> WindowsForCurrentThread { get; }

        /// <summary>Create a new <see cref="Window" /> instance.</summary>
        /// <returns>A new <see cref="Window" /> instance</returns>
        public abstract Window CreateWindow();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
