// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;
using System.Threading;

namespace TerraFX.UI
{
    /// <summary>Provides access to a window subsystem.</summary>
    public interface WindowProvider
    {
        /// <summary>Gets the <see cref="UI.DispatchProvider" /> for the instance.</summary>
        DispatchProvider DispatchProvider { get; }

        /// <summary>Gets the <see cref="Window" /> objects created by the instance which are associated with <see cref="Thread.CurrentThread" />.</summary>
        IEnumerable<Window> WindowsForCurrentThread { get; }

        /// <summary>Create a new <see cref="Window" /> instance.</summary>
        /// <returns>A new <see cref="Window" /> instance</returns>
        Window CreateWindow();
    }
}
