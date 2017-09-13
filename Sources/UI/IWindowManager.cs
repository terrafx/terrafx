// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;

namespace TerraFX.UI
{
    /// <summary>Provides a means of managing the windows created for an application.</summary>
    public interface IWindowManager
    {
        #region Properties
        /// <summary>Gets the <see cref="IWindow" /> objects created by the instance.</summary>
        IEnumerable<IWindow> Windows { get; }
        #endregion

        #region Methods
        /// <summary>Create a new <see cref="IWindow"/> instance.</summary>
        /// <returns>A new <see cref="IWindow" /> instance</returns>
        IWindow CreateWindow();
        #endregion
    }
}
