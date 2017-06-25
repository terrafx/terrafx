// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.UI
{
    /// <summary>Provides a means of managing the windows created for an application.</summary>
    public interface IWindowManager
    {
        #region Methods
        /// <summary>Create a new <see cref="IWindow"/> instance.</summary>
        /// <returns>A new <see cref="IWindow" /> instance</returns>
        IWindow CreateWindow();
        #endregion
    }
}
